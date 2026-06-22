namespace Secare_tool
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsShow;
        private System.Windows.Forms.ToolStripMenuItem tsExit;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器代码
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnStartFileMonitor = new System.Windows.Forms.Button();
            this.btnStartProcMonitor = new System.Windows.Forms.Button();
            this.btnRepairSys = new System.Windows.Forms.Button();
            this.btnKillProc = new System.Windows.Forms.Button();
            this.btnExportLog = new System.Windows.Forms.Button();
            this.btnSelfGuard = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnDelRiskFile = new System.Windows.Forms.Button();
            // 新增监控面板
            this.lstNormalProc = new System.Windows.Forms.ListBox();
            this.lstSuspectProc = new System.Windows.Forms.ListBox();
            this.lstService = new System.Windows.Forms.ListBox();
            this.lstStartup = new System.Windows.Forms.ListBox();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            // 新增监控开关按钮
            this.btnStartServiceMon = new System.Windows.Forms.Button();
            this.btnStartStartupMon = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsShow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsExit = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();

            // 顶部按钮行1
            this.btnStartFileMonitor.Location = new System.Drawing.Point(12, 12);
            this.btnStartFileMonitor.Size = new System.Drawing.Size(100, 26);
            this.btnStartFileMonitor.Text = "启动文件监控";
            this.btnStartFileMonitor.UseVisualStyleBackColor = true;
            this.btnStartFileMonitor.Click += btnStartFileMonitor_Click;

            this.btnStartProcMonitor.Location = new System.Drawing.Point(118, 12);
            this.btnStartProcMonitor.Size = new System.Drawing.Size(100, 26);
            this.btnStartProcMonitor.Text = "启动进程监控";
            this.btnStartProcMonitor.UseVisualStyleBackColor = true;
            this.btnStartProcMonitor.Click += btnStartProcMonitor_Click;

            this.btnStartServiceMon.Location = new System.Drawing.Point(224, 12);
            this.btnStartServiceMon.Size = new System.Drawing.Size(100, 26);
            this.btnStartServiceMon.Text = "启动服务监控";
            this.btnStartServiceMon.UseVisualStyleBackColor = true;
            this.btnStartServiceMon.Click += btnStartServiceMon_Click;

            this.btnStartStartupMon.Location = new System.Drawing.Point(330, 12);
            this.btnStartStartupMon.Size = new System.Drawing.Size(100, 26);
            this.btnStartStartupMon.Text = "启动启动项监控";
            this.btnStartStartupMon.UseVisualStyleBackColor = true;
            this.btnStartStartupMon.Click += btnStartStartupMon_Click;

            this.btnRepairSys.Location = new System.Drawing.Point(436, 12);
            this.btnRepairSys.Size = new System.Drawing.Size(100, 26);
            this.btnRepairSys.Text = "一键修复系统";
            this.btnRepairSys.UseVisualStyleBackColor = true;
            this.btnRepairSys.Click += btnRepairSys_Click;

            // 顶部按钮行2（扩展功能）
            this.btnKillProc.Location = new System.Drawing.Point(12, 44);
            this.btnKillProc.Size = new System.Drawing.Size(100, 26);
            this.btnKillProc.Text = "终止选中进程";
            this.btnKillProc.UseVisualStyleBackColor = true;
            this.btnKillProc.Click += btnKillProc_Click;

            this.btnExportLog.Location = new System.Drawing.Point(118, 44);
            this.btnExportLog.Size = new System.Drawing.Size(100, 26);
            this.btnExportLog.Text = "导出监控日志";
            this.btnExportLog.UseVisualStyleBackColor = true;
            this.btnExportLog.Click += btnExportLog_Click;

            this.btnSelfGuard.Location = new System.Drawing.Point(224, 44);
            this.btnSelfGuard.Size = new System.Drawing.Size(100, 26);
            this.btnSelfGuard.Text = "开启自我守护";
            this.btnSelfGuard.UseVisualStyleBackColor = true;
            this.btnSelfGuard.Click += btnSelfGuard_Click;

            // 文件删除输入区域
            this.txtFilePath.Location = new System.Drawing.Point(330, 44);
            this.txtFilePath.Size = new System.Drawing.Size(400, 21);
            this.btnDelRiskFile.Location = new System.Drawing.Point(736, 42);
            this.btnDelRiskFile.Size = new System.Drawing.Size(100, 25);
            this.btnDelRiskFile.Text = "手动删除风险文件";
            this.btnDelRiskFile.Click += btnDelRiskFile_Click;

            // 监控面板布局（4个分栏）
            this.lstNormalProc.Location = new System.Drawing.Point(12, 75);
            this.lstNormalProc.Size = new System.Drawing.Size(400, 140);
            this.lstNormalProc.TabIndex = 10;
            this.lstNormalProc.Text = "正常进程面板";

            this.lstSuspectProc.Location = new System.Drawing.Point(418, 75);
            this.lstSuspectProc.Size = new System.Drawing.Size(418, 140);
            this.lstSuspectProc.TabIndex = 11;
            this.lstSuspectProc.Text = "可疑进程面板";

            this.lstService.Location = new System.Drawing.Point(12, 221);
            this.lstService.Size = new System.Drawing.Size(400, 140);
            this.lstService.TabIndex = 12;
            this.lstService.Text = "系统服务面板";

            this.lstStartup.Location = new System.Drawing.Point(418, 221);
            this.lstStartup.Size = new System.Drawing.Size(418, 140);
            this.lstStartup.TabIndex = 13;
            this.lstStartup.Text = "开机启动项面板";

            // 日志输出框
            this.rtbLog.Location = new System.Drawing.Point(12, 367);
            this.rtbLog.Size = new System.Drawing.Size(824, 180);
            this.rtbLog.TabIndex = 14;

            // 托盘菜单
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Text = "安全防护工具";
            this.notifyIcon1.Visible = true;
            this.contextMenuStrip1.Items.AddRange(new[] { this.tsShow, this.tsExit });
            this.tsShow.Text = "显示主窗口";
            this.tsExit.Text = "彻底退出程序";

            // 窗体基础配置
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 560);
            this.Text = "系统安全辅助工具【增强多监控版】";
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.lstStartup);
            this.Controls.Add(this.lstService);
            this.Controls.Add(this.lstSuspectProc);
            this.Controls.Add(this.lstNormalProc);
            this.Controls.Add(this.btnDelRiskFile);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.btnSelfGuard);
            this.Controls.Add(this.btnExportLog);
            this.Controls.Add(this.btnKillProc);
            this.Controls.Add(this.btnStartStartupMon);
            this.Controls.Add(this.btnStartServiceMon);
            this.Controls.Add(this.btnRepairSys);
            this.Controls.Add(this.btnStartProcMonitor);
            this.Controls.Add(this.btnStartFileMonitor);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        // 原有控件
        private System.Windows.Forms.Button btnStartFileMonitor;
        private System.Windows.Forms.Button btnStartProcMonitor;
        private System.Windows.Forms.Button btnRepairSys;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnDelRiskFile;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Button btnKillProc;
        private System.Windows.Forms.Button btnExportLog;
        private System.Windows.Forms.Button btnSelfGuard;
        // 新增监控按钮
        private System.Windows.Forms.Button btnStartServiceMon;
        private System.Windows.Forms.Button btnStartStartupMon;
        // 新增4个监控面板
        private System.Windows.Forms.ListBox lstNormalProc;
        private System.Windows.Forms.ListBox lstSuspectProc;
        private System.Windows.Forms.ListBox lstService;
        private System.Windows.Forms.ListBox lstStartup;
    }
}