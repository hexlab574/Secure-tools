using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Secure_tool
{
    public partial class MainMonitorForm : Form
    {
        #region 全局缓存与锁
        private readonly string _selfExePath = Path.GetFullPath(Assembly.GetExecutingAssembly().Location).ToLowerInvariant();
        private readonly int _selfPid = Process.GetCurrentProcess().Id;
        private readonly string _configPath = Path.Combine(Application.StartupPath, "Config.json");
        private readonly object _signLock = new();
        private readonly object _fileLogLock = new();
        private readonly Dictionary<string, bool> _signCache = new();
        private readonly Dictionary<int, string> _procSnapshotCache = new();
        private static readonly JsonSerializerOptions _jsonOpt = new() { WriteIndented = true };
        private volatile bool _globalMonitorRunning = false;
        private AppConfig _cfg;

        private FileSystemWatcher _fileWatcher;
        private Thread _thProcess;
        private Thread _thService;
        private Thread _thStartup;
        private Thread _manualScanThread = null;
        #endregion

        public MainMonitorForm()
        {
            InitializeComponent();
            InitTrayBindEvents();
            LoadConfig();
            InitFileWatcher();
        }

        #region 托盘事件绑定
        private void InitTrayBindEvents()
        {
            _menuShowMain.Click += (s, e) => RestoreWindow();
            _menuStartMonitor.Click += btnStartAll_Click;
            _menuStopMonitor.Click += btnStopAll_Click;
            _menuOpenConfig.Click += OpenConfigWindow;
            _menuExit.Click += (s, e) => Application.Exit();
            _trayIcon.DoubleClick += (s, e) => RestoreWindow();
        }

        private void RestoreWindow()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(RestoreWindow);
                return;
            }
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }

        private void OpenConfigWindow(object sender, EventArgs e)
        {
            try
            {
                var frm = new ConfigEditForm(_configPath, ref _cfg);
                if (frm.ShowDialog() == DialogResult.OK)
                    LoadConfig();
            }
            catch (Exception ex)
            {
                WriteSysLog($"打开配置窗口异常：{ex.Message}");
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                if (_trayIcon != null)
                {
                    _trayIcon.Visible = true;
                    _trayIcon.ShowBalloonTip(1000, "后台挂起提醒", "防护程序驻留托盘持续运行", ToolTipIcon.Info);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                return;
            }

            _globalMonitorRunning = false;
            if (_fileWatcher != null)
            {
                _fileWatcher.EnableRaisingEvents = false;
                _fileWatcher.Dispose();
                _fileWatcher = null;
            }
            _thProcess?.Join(2000);
            _thService?.Join(2000);
            _thStartup?.Join(2000);
            _manualScanThread?.Join(1000);
            if (_trayIcon != null)
            {
                _trayIcon.Visible = false;
                _trayIcon.Dispose();
                _trayIcon = null;
            }
            base.OnFormClosing(e);
        }
        #endregion

        #region 配置加载（新增可信厂商白名单）
        private void LoadConfig()
        {
            if (!File.Exists(_configPath))
            {
                var defaultCfg = new AppConfig
                {
                    TrustDirList = new()
                    {
                        Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                        Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                        Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32"),
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SysWOW64"),
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "WinSxS"),
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "WindowsApps")
                    },
                    TrustVendorList = new()
                    {
                        "ByteDance", "字节跳动",
                        "Tencent", "腾讯",
                        "Alibaba", "阿里",
                        "Huawei", "华为",
                        "Xiaomi", "小米",
                        "NetEase", "网易"
                    },
                    CoreSysProcList = new() { "svchost", "dwm", "explorer", "winlogon", "ntoskrnl", "csrss", "smss", "lsass", "RuntimeBroker", "wuauclt", "TabTip" },
                    MalKeywordList = new() { "virus", "trojan", "miner", "hack", "backdoor", "rat", "keylog", "加固", "killsys", "cleanwin", "sysdel", "清理", "卸载服务", "关闭防护" },
                    KernelPidMin = 4,
                    KernelPidMax = 1000,
                    AutoKillMalware = true,
                    ShowTrustedLog = false
                };
                File.WriteAllText(_configPath, JsonSerializer.Serialize(defaultCfg, _jsonOpt), new UTF8Encoding(true));
                _cfg = defaultCfg;
                WriteSysLog("首次运行，自动生成多层拦截默认配置（内置正规厂商签名白名单）");
            }
            else
            {
                try
                {
                    string jsonText = File.ReadAllText(_configPath, Encoding.UTF8);
                    _cfg = JsonSerializer.Deserialize<AppConfig>(jsonText);
                    WriteSysLog("成功加载多层威胁拦截配置");
                }
                catch (Exception ex)
                {
                    WriteSysLog($"配置文件损坏，启用默认防护配置：{ex.Message}");
                    _cfg = new AppConfig();
                }
            }
        }
        #endregion

        #region 文件监视器初始化
        private void InitFileWatcher()
        {
            try
            {
                _fileWatcher = new FileSystemWatcher
                {
                    InternalBufferSize = 65536,
                    IncludeSubdirectories = true,
                    Path = Application.StartupPath
                };
                _fileWatcher.Error += (s, e) => WriteSysLog($"文件监视器异常：{e.GetException().Message}");
                _fileWatcher.Created += FileChangeEvent;
                _fileWatcher.Deleted += FileChangeEvent;
                _fileWatcher.Renamed += FileChangeEvent;
            }
            catch (Exception ex)
            {
                WriteSysLog($"文件监视器初始化失败：{ex.Message}");
            }
        }
        private void FileChangeEvent(object s, FileSystemEventArgs e)
        {
            WriteSysLog($"文件变动 {e.ChangeType}：{e.FullPath}");
        }
        #endregion

        #region 签名校验核心方法（新增可信厂商签名判断，修复豆包误报）
        private bool IsCriticalSystemPath(string exePath)
        {
            if (string.IsNullOrWhiteSpace(exePath)) return false;
            string winDir = Environment.GetFolderPath(Environment.SpecialFolder.Windows).ToLowerInvariant();
            string sys32 = Path.Combine(winDir, "system32").ToLowerInvariant();
            string target = Path.GetFullPath(exePath).ToLowerInvariant();
            return target.StartsWith(winDir) || target.StartsWith(sys32);
        }

        private bool IsMsSigned(string exePath)
        {
            if (!File.Exists(exePath)) return false;
            lock (_signLock)
            {
                if (_signCache.TryGetValue(exePath, out var cache))
                    return cache;
            }
            bool res = false;
            try
            {
                using var cert = X509Certificate2.CreateFromSignedFile(exePath);
                string sub = cert.Subject.ToLowerInvariant(), iss = cert.Issuer.ToLowerInvariant();
                res = sub.Contains("microsoft corporation") || iss.Contains("microsoft code signing");
            }
            catch (CryptographicException) { }
            lock (_signLock)
            {
                if (!_signCache.ContainsKey(exePath))
                    _signCache.Add(exePath, res);
            }
            return res;
        }

        /// <summary>校验字节、腾讯、阿里等正规厂商自有签名，放行豆包等国产软件</summary>
        private bool IsTrustVendorSigned(string exePath)
        {
            if (!File.Exists(exePath)) return false;
            try
            {
                using var cert = X509Certificate2.CreateFromSignedFile(exePath);
                string subject = cert.Subject.ToLowerInvariant();
                string issuer = cert.Issuer.ToLowerInvariant();
                foreach (var vendor in _cfg.TrustVendorList)
                {
                    string vKey = vendor.ToLowerInvariant();
                    if (subject.Contains(vKey) || issuer.Contains(vKey))
                        return true;
                }
            }
            catch (CryptographicException)
            {
                return false;
            }
            return false;
        }

        private bool IsKernelPid(int pid) => pid >= _cfg.KernelPidMin && pid <= _cfg.KernelPidMax;

        private bool MatchMalWord(string procLower)
        {
            foreach (var kw in _cfg.MalKeywordList)
                if (Regex.IsMatch(procLower, $@"\b{Regex.Escape(kw)}\b")) return true;
            return false;
        }

        private bool IsTrustDir(string rawPath)
        {
            string full = Path.GetFullPath(rawPath).ToLowerInvariant();
            foreach (var dir in _cfg.TrustDirList)
            {
                string std = Path.GetFullPath(dir).ToLowerInvariant() + Path.DirectorySeparatorChar;
                if (full.StartsWith(std)) return true;
            }
            return false;
        }

        /// <summary>重构信任判断：微软签名 || 正规厂商签名 直接放行，彻底解决豆包误判</summary>
        private bool IsTrusted(int pid, string path, string procName)
        {
            // 过滤自身进程，杜绝自查误报
            if (pid == _selfPid) return true;
            if (IsKernelPid(pid)) return true;

            if (!string.IsNullOrEmpty(path))
            {
                string fullPath = Path.GetFullPath(path).ToLowerInvariant();
                if (fullPath == _selfExePath) return true;

                bool msSign = IsMsSigned(path);
                bool vendorSign = IsTrustVendorSigned(path);
                // 核心修复：有微软/字节/腾讯等厂商签名直接判定可信
                if (msSign || vendorSign)
                    return true;

                if (IsTrustDir(path))
                    return true;
            }

            if (string.IsNullOrEmpty(path)) return true;
            if (_cfg.CoreSysProcList.Any(x => x.Equals(procName, StringComparison.OrdinalIgnoreCase)))
                return true;

            // 无任何可信签名、不在信任目录 → 仅标记低风险提示，不判定高危
            return false;
        }
        #endregion

        #region 高危进程自动拦截
        private void AutoBlockMalware(int pid, string procName, string path, string riskDesc)
        {
            string alert = $"【高危拦截】{riskDesc}\nPID:{pid}\n程序名称:{procName}\n完整路径:{path}";
            MessageBox.Show(alert, "SecureTool 安全高危告警", MessageBoxButtons.OK, MessageBoxIcon.Error);
            _trayIcon?.ShowBalloonTip(3000, "⚠️ 检测到恶意程序并拦截", alert, ToolTipIcon.Error);
            WriteRiskLog(RiskLevel.HighRisk, pid, procName, path);

            try
            {
                using var p = Process.GetProcessById(pid);
                p.Kill();
                WriteSysLog($"已强制终止高危进程 PID:{pid}");
                string blockLog = Path.Combine(Application.StartupPath, "MalwareBlock.log");
                lock (_fileLogLock)
                {
                    File.AppendAllText(blockLog, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {riskDesc} | PID:{pid} | {procName} | {path}\r\n", new UTF8Encoding(true));
                }
            }
            catch (Exception ex)
            {
                WriteSysLog($"终止进程失败 PID:{pid}：{ex.Message}");
            }
        }
        #endregion

        #region 分层日志写入（区分可信/低风险/高危，UTF8带BOM解决中文乱码）
        public enum RiskLevel
        {
            Trusted,
            Kernel,
            UnknownDir,
            HighRisk
        }
        private void WriteRiskLog(RiskLevel lv, int pid, string proc, string path)
        {
            if ((lv == RiskLevel.Trusted || lv == RiskLevel.Kernel) && !_cfg.ShowTrustedLog) return;
            string tag = lv switch
            {
                RiskLevel.Trusted => "[可信程序｜微软/正规厂商数字签名]",
                RiskLevel.Kernel => "[系统内核进程]",
                RiskLevel.UnknownDir => "[提示：第三方无可信签名软件，无恶意特征]",
                RiskLevel.HighRisk => "[高危恶意程序｜触发自动拦截规则]",
                _ => "[未知]"
            };
            string line = $"{DateTime.Now:HH:mm:ss} {tag} PID:{pid} | {proc} | {path}\r\n";
            SaveLogFile(line);
            if (txtLog.InvokeRequired)
                txtLog.Invoke(() => { txtLog.AppendText(line); txtLog.ScrollToCaret(); });
            else
            { txtLog.AppendText(line); txtLog.ScrollToCaret(); }
        }

        private void WriteSysLog(string msg)
        {
            string line = $"{DateTime.Now:HH:mm:ss} [系统日志] {msg}\r\n";
            SaveLogFile(line);
            if (txtLog.InvokeRequired)
                txtLog.Invoke(() => { txtLog.AppendText(line); txtLog.ScrollToCaret(); });
            else
            { txtLog.AppendText(line); txtLog.ScrollToCaret(); }
        }

        private void SaveLogFile(string content)
        {
            try
            {
                string logDir = Path.Combine(Application.StartupPath, "Logs");
                if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);
                string logFile = Path.Combine(logDir, $"{DateTime.Now:yyyy-MM-dd}.log");
                lock (_fileLogLock)
                {
                    File.AppendAllText(logFile, content, new UTF8Encoding(true));
                }
            }
            catch { }
        }
        #endregion

        #region 后台监控循环（分层风险判定，区分普通第三方和真正恶意程序）
        private void ProcessMonitorLoop()
        {
            _procSnapshotCache.Clear();
            while (_globalMonitorRunning)
            {
                try
                {
                    using var search = new ManagementObjectSearcher("SELECT ProcessId,Name,ExecutablePath FROM Win32_Process");
                    var currentProcSet = new Dictionary<int, string>();
                    foreach (ManagementBaseObject obj in search.Get())
                    {
                        int pid = Convert.ToInt32(obj["ProcessId"]);
                        string pName = obj["Name"]?.ToString()?.Trim() ?? "";
                        string exePath = obj["ExecutablePath"]?.ToString();
                        string pShort = Path.GetFileNameWithoutExtension(pName).ToLowerInvariant();
                        currentProcSet[pid] = $"{pShort}|{exePath}";

                        bool trusted = IsTrusted(pid, exePath, pShort);
                        bool hitMalKw = MatchMalWord(pShort);
                        bool runSysDir = IsCriticalSystemPath(exePath);

                        // 规则1：命中恶意关键词 → 高危拦截
                        if (!trusted && hitMalKw)
                        {
                            if (_cfg.AutoKillMalware)
                                AutoBlockMalware(pid, pName, exePath, "进程名称匹配恶意特征关键词");
                            else
                                WriteRiskLog(RiskLevel.HighRisk, pid, pName, exePath);
                            continue;
                        }
                        // 规则2：无签名程序写入系统目录 → 高危拦截
                        if (!trusted && runSysDir && !hitMalKw)
                        {
                            if (_cfg.AutoKillMalware)
                                AutoBlockMalware(pid, pName, exePath, "无可信签名程序运行系统核心目录，存在篡改风险");
                            else
                                WriteRiskLog(RiskLevel.HighRisk, pid, pName, exePath);
                            continue;
                        }
                        // 规则3：仅无签名、无恶意特征（豆包、绿色软件）→ 仅日志提示，不弹窗、不查杀
                        if (!trusted && !_procSnapshotCache.ContainsKey(pid))
                        {
                            WriteRiskLog(RiskLevel.UnknownDir, pid, pName, exePath);
                        }
                    }
                    _procSnapshotCache.Clear();
                    foreach (var kv in currentProcSet) _procSnapshotCache[kv.Key] = kv.Value;
                }
                catch (Exception ex)
                {
                    WriteSysLog($"进程扫描单次循环异常：{ex.Message}");
                }
                Thread.Sleep(1000);
            }
        }

        private void ServiceMonitorLoop()
        {
            while (_globalMonitorRunning)
            {
                try
                {
                    using var search = new ManagementObjectSearcher("SELECT DisplayName,PathName,ProcessId FROM Win32_Service");
                    foreach (ManagementBaseObject svc in search.Get())
                    {
                        string path = svc["PathName"]?.ToString() ?? "";
                        int pid = Convert.ToInt32(svc["ProcessId"] ?? 0);
                        string sName = svc["DisplayName"]?.ToString() ?? "";
                        if (string.IsNullOrWhiteSpace(path) || IsKernelPid(pid) || pid == _selfPid) continue;
                        string exeName = Path.GetFileNameWithoutExtension(path.Split(' ')[0]).ToLowerInvariant();
                        if (MatchMalWord(exeName) && !IsTrusted(pid, path, exeName))
                        {
                            AutoBlockMalware(pid, sName, path, "恶意伪装系统服务，持久化后门风险");
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteSysLog($"服务扫描单次循环异常：{ex.Message}");
                }
                Thread.Sleep(3000);
            }
        }

        private void StartupMonitorLoop()
        {
            while (_globalMonitorRunning)
            {
                try
                {
                    string[] regPaths = { @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", @"SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce" };
                    foreach (var regPath in regPaths)
                    {
                        using var key = Registry.LocalMachine.OpenSubKey(regPath);
                        if (key == null) continue;
                        foreach (var valName in key.GetValueNames())
                        {
                            string launchPath = key.GetValue(valName)?.ToString() ?? "";
                            if (!launchPath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)) continue;
                            string exeName = Path.GetFileNameWithoutExtension(launchPath).ToLowerInvariant();
                            if (MatchMalWord(exeName) && !IsTrusted(0, launchPath, exeName))
                            {
                                string alert = $"检测恶意开机自启项 [{valName}]：{launchPath}";
                                _trayIcon?.ShowBalloonTip(3000, "开机持久化威胁预警", alert, ToolTipIcon.Warning);
                                WriteSysLog(alert);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteSysLog($"开机启动项扫描单次循环异常：{ex.Message}");
                }
                Thread.Sleep(5000);
            }
        }
        #endregion

        #region 按钮点击事件（手动扫描防重复启动）
        private void btnStartAll_Click(object sender, EventArgs e)
        {
            if (_globalMonitorRunning)
            {
                WriteSysLog("监控服务已处于运行状态，无需重复启动");
                return;
            }
            _globalMonitorRunning = true;
            lblStatus.Text = "监控状态：四层防护全部运行中";
            progressMonitor.Visible = true;
            if (_trayIcon != null) _trayIcon.Text = "SecureTool 多维度安全防护工具 | 监控运行中";
            _fileWatcher?.EnableRaisingEvents = true;
            _thProcess = new Thread(ProcessMonitorLoop) { IsBackground = true };
            _thService = new Thread(ServiceMonitorLoop) { IsBackground = true };
            _thStartup = new Thread(StartupMonitorLoop) { IsBackground = true };
            _thProcess.Start();
            _thService.Start();
            _thStartup.Start();
            WriteSysLog("四层威胁拦截机制已全部启动，自动查杀功能已开启");
            _trayIcon?.ShowBalloonTip(1500, "防护启动完成", "进程/服务/开机项/系统目录四层实时监控已生效", ToolTipIcon.Info);
        }

        private void btnStopAll_Click(object sender, EventArgs e)
        {
            if (!_globalMonitorRunning) return;
            _globalMonitorRunning = false;
            if (_fileWatcher != null) _fileWatcher.EnableRaisingEvents = false;
            _thProcess?.Join(2000);
            _thService?.Join(2000);
            _thStartup?.Join(2000);
            lblStatus.Text = "监控状态：所有防护已停止";
            progressMonitor.Visible = false;
            if (_trayIcon != null) _trayIcon.Text = "SecureTool 多维度安全防护工具 | 监控已暂停";
            WriteSysLog("全部监控线程已停止，自动拦截功能关闭");
            _trayIcon?.ShowBalloonTip(1500, "防护已暂停", "后台四层实时监控已关闭，设备存在安全风险", ToolTipIcon.Warning);
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            if (txtLog.InvokeRequired)
                txtLog.Invoke(() => txtLog.Clear());
            else
                txtLog.Clear();
        }

        private void btnManualScan_Click(object sender, EventArgs e)
        {
            if (_manualScanThread != null && _manualScanThread.IsAlive)
            {
                WriteSysLog("手动扫描任务正在执行，请勿重复触发");
                return;
            }
            WriteSysLog("执行手动全盘进程扫描任务");
            _manualScanThread = new Thread(ProcessMonitorLoop) { IsBackground = true };
            _manualScanThread.Start();
        }

        private void btnOpenProcessMgr_Click(object sender, EventArgs e)
        {
            new ProcessManagerForm(_cfg, this).Show();
        }

        private void btnOpenLogViewer_Click(object sender, EventArgs e)
        {
            new LogViewerForm().Show();
        }
        #endregion
    }

    /// <summary>配置模型：新增可信厂商白名单TrustVendorList</summary>
    public class AppConfig
    {
        public List<string> TrustDirList { get; set; } = new();
        public List<string> TrustVendorList { get; set; } = new();
        public List<string> CoreSysProcList { get; set; } = new();
        public List<string> MalKeywordList { get; set; } = new();
        public int KernelPidMin { get; set; }
        public int KernelPidMax { get; set; }
        public bool AutoKillMalware { get; set; }
        public bool ShowTrustedLog { get; set; }
    }
}