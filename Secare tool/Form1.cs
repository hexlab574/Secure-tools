using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Secare_tool
{
    public partial class Form1 : Form
    {
        #region 全局常量（统一维护黑白名单，优化代码整洁度）
        // 高危文件后缀
        private readonly List<string> _riskExt = new List<string> { ".exe", ".bat", ".cmd", ".vbs", ".ps1", ".js", ".scr", ".com" };
        // 恶意进程黑名单
        private readonly List<string> _malProcName = new List<string> { "virus", "trojan", "miner", "hack", "backdoor", "rat", "keylog" };
        // 自身程序白名单（修复自身误报可疑进程）
        private readonly string _selfExeName = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
        private readonly string _selfExePath = Assembly.GetExecutingAssembly().Location;
        // 系统可信目录（不在此目录的exe判定高风险）
        private readonly List<string> _trustSysDir = new List<string>
        {
            Environment.GetFolderPath(Environment.SpecialFolder.Windows),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SysWOW64")
        };
        #endregion

        #region 基础监控对象
        private FileSystemWatcher _fileWatcher;
        private Thread _procMonitorThread;
        private Thread _serviceMonitorThread;
        private Thread _startupMonitorThread;
        private bool _procMonitorRunning = false;
        private bool _serviceMonitorRunning = false;
        private bool _startupMonitorRunning = false;
        #endregion

        #region 托盘&自我守护
        private NotifyIcon _trayIcon;
        private ContextMenuStrip _trayMenu;
        private Thread _selfGuardThread;
        private bool _selfGuardEnable = false;
        #endregion

        public Form1()
        {
            InitializeComponent();
            this.Text = "系统安全辅助工具【增强多监控版】";
            this.FormClosing += Form1_FormClosing;

            InitWatcher();
            InitTrayIcon();
        }

        #region 托盘后台逻辑
        private void InitTrayIcon()
        {
            _trayMenu = new ContextMenuStrip();
            var menuShow = new ToolStripMenuItem("显示主窗口", null, TrayShowWindow);
            var menuExit = new ToolStripMenuItem("彻底退出程序", null, TrayExitApp);
            _trayMenu.Items.AddRange(new[] { menuShow, menuExit });

            _trayIcon = new NotifyIcon
            {
                Text = "安全防护工具",
                Icon = this.Icon,
                ContextMenuStrip = _trayMenu,
                Visible = true
            };
            _trayIcon.DoubleClick += TrayShowWindow;
        }

        private void TrayShowWindow(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
            this.Activate();
        }

        private void TrayExitApp(object sender, EventArgs e)
        {
            CleanAllResource();
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                this.WindowState = FormWindowState.Minimized;
                _trayIcon.ShowBalloonTip(1200, "后台防护中", "程序已最小化托盘，右键菜单可退出", ToolTipIcon.Info);
            }
        }
        #endregion

        #region 文件监控+自动拦截
        private void InitWatcher()
        {
            _fileWatcher = new FileSystemWatcher
            {
                Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = "*.*",
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime
            };
            _fileWatcher.Created += AutoBlockRiskFile;
            _fileWatcher.Deleted += FileChangeEvent;
            _fileWatcher.Changed += FileChangeEvent;
            _fileWatcher.Renamed += FileRenameEvent;
            _fileWatcher.EnableRaisingEvents = false;
        }

        private void AutoBlockRiskFile(object sender, FileSystemEventArgs e)
        {
            string ext = Path.GetExtension(e.FullPath).ToLower();
            if (!_riskExt.Contains(ext)) return;

            string log = $"[{DateTime.Now:HH:mm:ss}] ⚠️ 自动拦截高危文件：{e.FullPath}";
            AppendWarnLog(log);
            _trayIcon.ShowBalloonTip(2000, "风险文件告警", $"检测可疑文件 {Path.GetFileName(e.FullPath)}", ToolTipIcon.Warning);

            DialogResult res = MessageBox.Show($"检测高危程序文件：\n{e.FullPath}\n是否永久删除？", "拦截确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                try
                {
                    File.Delete(e.FullPath);
                    AppendNormalLog($"✅ 已删除风险文件：{e.FullPath}");
                }
                catch (Exception ex)
                {
                    AppendErrorLog($"❌ 删除失败：{ex.Message}");
                }
            }
        }

        private void FileChangeEvent(object sender, FileSystemEventArgs e)
        {
            string ext = Path.GetExtension(e.FullPath).ToLower();
            string log = $"[{DateTime.Now:HH:mm:ss}] 文件操作：{e.ChangeType} | {e.FullPath}";
            if (_riskExt.Contains(ext)) AppendWarnLog(log);
            else AppendNormalLog(log);
        }

        private void FileRenameEvent(object sender, RenamedEventArgs e)
        {
            AppendNormalLog($"[{DateTime.Now:HH:mm:ss}] 文件重命名 | {e.OldFullPath} → {e.FullPath}");
        }
        #endregion

        #region 进程监控 + 可疑进程判定（修复自身误报）
        /// <summary>
        /// 判断进程是否为可疑进程，过滤自身程序
        /// </summary>
        private bool IsSuspiciousProcess(Process p, out string riskDesc)
        {
            riskDesc = "";
            try
            {
                string procName = p.ProcessName.ToLower();
                string exePath = p.MainModule.FileName;

                // 规则1：匹配自身程序，直接判定可信（修复自身误报）
                if (exePath.Equals(_selfExePath, StringComparison.OrdinalIgnoreCase) || procName == _selfExeName.ToLower())
                    return false;

                // 规则2：匹配恶意进程名黑名单
                if (_malProcName.Any(x => procName.Contains(x)))
                {
                    riskDesc = "匹配恶意进程名黑名单";
                    return true;
                }

                // 规则3：不在系统可信目录内的exe程序
                bool inTrustDir = _trustSysDir.Any(dir => exePath.StartsWith(dir, StringComparison.OrdinalIgnoreCase));
                if (!inTrustDir && Path.GetExtension(exePath).ToLower() == ".exe")
                {
                    riskDesc = "程序存放于非系统可信目录";
                    return true;
                }

                // 规则4：随机短名称进程（纯数字/乱码命名）
                if (Regex.IsMatch(procName, @"^[0-9a-z]{3,8}$") && !_trustSysDir.Any(d => exePath.StartsWith(d)))
                {
                    riskDesc = "进程名称疑似随机生成";
                    return true;
                }
            }
            catch
            {
                riskDesc = "无法读取进程信息，权限异常";
                return true;
            }
            return false;
        }

        // 进程监控开关
        private void btnStartProcMonitor_Click(object sender, EventArgs e)
        {
            if (!_procMonitorRunning)
            {
                _procMonitorRunning = true;
                _procMonitorThread = new Thread(ProcessMonitorLoop) { IsBackground = true };
                _procMonitorThread.Start();
                AppendNormalLog("===== 进程监控（含可疑进程识别）已启动 =====");
                btnStartProcMonitor.Text = "停止进程监控";
            }
            else
            {
                _procMonitorRunning = false;
                AppendNormalLog("===== 进程监控已关闭 =====");
                btnStartProcMonitor.Text = "启动进程监控";
            }
        }

        // 进程循环刷新，区分正常/可疑进程
        private void ProcessMonitorLoop()
        {
            while (_procMonitorRunning)
            {
                List<string> normalList = new List<string>();
                List<string> suspectList = new List<string>();
                Process[] allProcs = Process.GetProcesses();

                foreach (var p in allProcs)
                {
                    string riskTip;
                    bool isSus = IsSuspiciousProcess(p, out riskTip);
                    try
                    {
                        string path = p.MainModule?.FileName ?? "未知路径";
                        string info = $"PID:{p.Id} | 名称:{p.ProcessName} | 路径:{path}";
                        if (isSus) suspectList.Add(info + $" 【风险：{riskTip}】");
                        else normalList.Add(info);
                    }
                    catch
                    {
                        string info = $"PID:{p.Id} | 名称:{p.ProcessName} | 读取失败";
                        suspectList.Add(info + " 【风险：权限读取异常】");
                    }
                }

                // 刷新双面板
                lstNormalProc.Invoke(new Action(() =>
                {
                    lstNormalProc.Items.Clear();
                    lstNormalProc.Items.AddRange(normalList.ToArray());
                }));
                lstSuspectProc.Invoke(new Action(() =>
                {
                    lstSuspectProc.Items.Clear();
                    lstSuspectProc.Items.AddRange(suspectList.ToArray());
                }));

                Thread.Sleep(3000);
            }
        }
        #endregion

        #region 新增面板1：系统服务监控
        private void btnStartServiceMon_Click(object sender, EventArgs e)
        {
            if (!_serviceMonitorRunning)
            {
                _serviceMonitorRunning = true;
                _serviceMonitorThread = new Thread(ServiceMonitorLoop) { IsBackground = true };
                _serviceMonitorThread.Start();
                AppendNormalLog("===== 系统服务监控已启动 =====");
                btnStartServiceMon.Text = "停止服务监控";
            }
            else
            {
                _serviceMonitorRunning = false;
                AppendNormalLog("===== 系统服务监控已关闭 =====");
                btnStartServiceMon.Text = "启动服务监控";
            }
        }

        private void ServiceMonitorLoop()
        {
            while (_serviceMonitorRunning)
            {
                List<string> serviceList = new List<string>();
                try
                {
                    using (ManagementObjectSearcher search = new ManagementObjectSearcher("SELECT Name, DisplayName, State FROM Win32_Service"))
                    {
                        foreach (ManagementBaseObject srv in search.Get())
                        {
                            string name = srv["Name"]?.ToString();
                            string disp = srv["DisplayName"]?.ToString();
                            string state = srv["State"]?.ToString();
                            serviceList.Add($"服务名:{name} | 显示名:{disp} | 状态:{state}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    serviceList.Add($"服务读取失败：{ex.Message}");
                }

                lstService.Invoke(new Action(() =>
                {
                    lstService.Items.Clear();
                    lstService.Items.AddRange(serviceList.ToArray());
                }));
                Thread.Sleep(5000);
            }
        }
        #endregion

        #region 新增面板2：开机启动项监控
        private void btnStartStartupMon_Click(object sender, EventArgs e)
        {
            if (!_startupMonitorRunning)
            {
                _startupMonitorRunning = true;
                _startupMonitorThread = new Thread(StartupMonitorLoop) { IsBackground = true };
                _startupMonitorThread.Start();
                AppendNormalLog("===== 开机启动项监控已启动 =====");
                btnStartStartupMon.Text = "停止启动项监控";
            }
            else
            {
                _startupMonitorRunning = false;
                AppendNormalLog("===== 开机启动项监控已关闭 =====");
                btnStartStartupMon.Text = "启动启动项监控";
            }
        }

        private void StartupMonitorLoop()
        {
            while (_startupMonitorRunning)
            {
                List<string> startupList = new List<string>();
                string[] regPaths =
                {
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce"
                };

                foreach (var regPath in regPaths)
                {
                    try
                    {
                        using (var regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(regPath))
                        {
                            if (regKey == null) continue;
                            foreach (var valName in regKey.GetValueNames())
                            {
                                var valData = regKey.GetValue(valName);
                                startupList.Add($"注册表:{regPath} | 项名:{valName} | 程序:{valData}");
                            }
                        }
                    }
                    catch { }
                }

                lstStartup.Invoke(new Action(() =>
                {
                    lstStartup.Items.Clear();
                    lstStartup.Items.AddRange(startupList.ToArray());
                }));
                Thread.Sleep(8000);
            }
        }
        #endregion

        #region 原有按钮功能
        private void btnStartFileMonitor_Click(object sender, EventArgs e)
        {
            if (!_fileWatcher.EnableRaisingEvents)
            {
                _fileWatcher.EnableRaisingEvents = true;
                AppendNormalLog("===== 文件监控&自动拦截已启动 =====");
                btnStartFileMonitor.Text = "停止文件监控";
            }
            else
            {
                _fileWatcher.EnableRaisingEvents = false;
                AppendNormalLog("===== 文件监控已关闭 =====");
                btnStartFileMonitor.Text = "启动文件监控";
            }
        }

        private void btnRepairSys_Click(object sender, EventArgs e)
        {
            AppendNormalLog("开始执行系统修复 SFC + DISM");
            RunCmd("sfc /scannow");
            RunCmd("DISM /Online /Cleanup-Image /RestoreHealth");
            AppendNormalLog("系统修复命令执行完毕");
        }

        private void btnDelRiskFile_Click(object sender, EventArgs e)
        {
            string path = txtFilePath.Text.Trim();
            if (!File.Exists(path))
            {
                MessageBox.Show("文件不存在！");
                return;
            }
            DialogResult confirm = MessageBox.Show($"确认删除：{path}", "危险操作", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                File.Delete(path);
                AppendNormalLog($"手动删除文件：{path}");
                txtFilePath.Clear();
            }
        }
        #endregion

        #region 3个扩展按钮
        private void btnKillProc_Click(object sender, EventArgs e)
        {
            ListBox targetBox = lstSuspectProc.SelectedItem != null ? lstSuspectProc : lstNormalProc;
            if (targetBox.SelectedItem == null)
            {
                MessageBox.Show("请选中一条进程记录");
                return;
            }
            string pidStr = targetBox.SelectedItem.ToString().Split('|')[0].Replace("PID:", "").Trim();
            if (!int.TryParse(pidStr, out int pid))
            {
                MessageBox.Show("PID解析失败");
                return;
            }
            DialogResult res = MessageBox.Show($"强制终止 PID:{pid}？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                try
                {
                    Process.GetProcessById(pid).Kill();
                    AppendNormalLog($"✅ 已终止进程 PID:{pid}");
                }
                catch (Exception ex)
                {
                    AppendErrorLog($"❌ 终止失败 PID:{pid}：{ex.Message}");
                }
            }
        }

        private void btnExportLog_Click(object sender, EventArgs e)
        {
            string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"安全日志_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            File.WriteAllText(savePath, rtbLog.Text);
            AppendNormalLog($"日志导出至：{savePath}");
            MessageBox.Show("日志导出完成！");
        }

        private void btnSelfGuard_Click(object sender, EventArgs e)
        {
            if (!_selfGuardEnable)
            {
                _selfGuardEnable = true;
                btnSelfGuard.Text = "关闭自我守护";
                _selfGuardThread = new Thread(SelfGuardLoop) { IsBackground = true };
                _selfGuardThread.Start();
                AppendNormalLog("===== 自我防杀守护已开启 =====");
            }
            else
            {
                _selfGuardEnable = false;
                btnSelfGuard.Text = "开启自我守护";
                AppendNormalLog("===== 自我守护已关闭 =====");
            }
        }

        private void SelfGuardLoop()
        {
            string exePath = Application.ExecutablePath;
            while (_selfGuardEnable)
            {
                bool alive = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(exePath)).Any(p => p.MainModule.FileName == exePath);
                if (!alive)
                {
                    AppendWarnLog("⚠️ 主程序进程被终止，自动重启");
                    Process.Start(exePath);
                    Application.Exit();
                }
                Thread.Sleep(2000);
            }
        }
        #endregion

        #region 分级日志输出（优化区分正常/警告/错误）
        private void AppendNormalLog(string msg)
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(new Action(() => AppendNormalLog(msg)));
                return;
            }
            rtbLog.SelectionColor = System.Drawing.Color.Black;
            rtbLog.AppendText(msg + Environment.NewLine);
            rtbLog.ScrollToCaret();
        }

        private void AppendWarnLog(string msg)
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(new Action(() => AppendWarnLog(msg)));
                return;
            }
            rtbLog.SelectionColor = System.Drawing.Color.DarkOrange;
            rtbLog.AppendText(msg + Environment.NewLine);
            rtbLog.ScrollToCaret();
        }

        private void AppendErrorLog(string msg)
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(new Action(() => AppendErrorLog(msg)));
                return;
            }
            rtbLog.SelectionColor = System.Drawing.Color.Red;
            rtbLog.AppendText(msg + Environment.NewLine);
            rtbLog.ScrollToCaret();
        }
        #endregion

        #region 通用工具与资源释放
        private void RunCmd(string cmdText)
        {
            Process cmd = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {cmdText}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = false
                }
            };
            cmd.Start();
            cmd.WaitForExit();
        }

        private void CleanAllResource()
        {
            // 全部监控线程停止
            _procMonitorRunning = false;
            _serviceMonitorRunning = false;
            _startupMonitorRunning = false;
            _selfGuardEnable = false;
            // 释放资源
            _fileWatcher?.Dispose();
            _trayIcon?.Dispose();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            CleanAllResource();
            base.OnFormClosed(e);
        }
        #endregion
    }
}