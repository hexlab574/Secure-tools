namespace Secure_tool
{
    partial class MainMonitorForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成代码
        private void InitializeComponent()
        {
            // ========== 修复核心：实例化组件容器，解决container null崩溃 ==========
            this.components = new System.ComponentModel.Container();

            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageMonitor = new System.Windows.Forms.TabPage();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressMonitor = new System.Windows.Forms.ProgressBar();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.panelBtn = new System.Windows.Forms.Panel();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.btnOpenLogViewer = new System.Windows.Forms.Button();
            this.btnOpenConfig = new System.Windows.Forms.Button();
            this.btnOpenProcessMgr = new System.Windows.Forms.Button();
            this.btnStopAll = new System.Windows.Forms.Button();
            this.btnStartAll = new System.Windows.Forms.Button();
            this.tabPageQuickScan = new System.Windows.Forms.TabPage();
            this.labelScanTip = new System.Windows.Forms.Label();
            this.btnManualScan = new System.Windows.Forms.Button();
            this.tabControlMain.SuspendLayout();
            this.tabPageMonitor.SuspendLayout();
            this.panelStatus.SuspendLayout();
            this.panelBtn.SuspendLayout();
            this.tabPageQuickScan.SuspendLayout();
            this.SuspendLayout();

            this.tabControlMain.Controls.Add(this.tabPageMonitor);
            this.tabControlMain.Controls.Add(this.tabPageQuickScan);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1080, 620);
            this.tabControlMain.TabIndex = 0;

            this.tabPageMonitor.Controls.Add(this.panelStatus);
            this.tabPageMonitor.Controls.Add(this.txtLog);
            this.tabPageMonitor.Controls.Add(this.panelBtn);
            this.tabPageMonitor.Location = new System.Drawing.Point(4, 24);
            this.tabPageMonitor.Name = "tabPageMonitor";
            this.tabPageMonitor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMonitor.Size = new System.Drawing.Size(1072, 592);
            this.tabPageMonitor.TabIndex = 0;
            this.tabPageMonitor.Text = "实时安全监控";
            this.tabPageMonitor.UseVisualStyleBackColor = true;

            this.panelStatus.Controls.Add(this.lblStatus);
            this.panelStatus.Controls.Add(this.progressMonitor);
            this.panelStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStatus.Location = new System.Drawing.Point(3, 3);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(1066, 32);
            this.panelStatus.TabIndex = 2;

            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(8, 8);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(92, 15);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "监控状态：未启动";

            this.progressMonitor.Location = new System.Drawing.Point(120, 6);
            this.progressMonitor.Name = "progressMonitor";
            this.progressMonitor.Size = new System.Drawing.Size(300, 20);
            this.progressMonitor.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressMonitor.Visible = false;
            this.progressMonitor.TabIndex = 1;

            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(3, 35);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(1066, 514);
            this.txtLog.TabIndex = 1;

            this.panelBtn.Controls.Add(this.btnClearLog);
            this.panelBtn.Controls.Add(this.btnOpenLogViewer);
            this.panelBtn.Controls.Add(this.btnOpenConfig);
            this.panelBtn.Controls.Add(this.btnOpenProcessMgr);
            this.panelBtn.Controls.Add(this.btnStopAll);
            this.panelBtn.Controls.Add(this.btnStartAll);
            this.panelBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBtn.Location = new System.Drawing.Point(3, 549);
            this.panelBtn.Name = "panelBtn";
            this.panelBtn.Size = new System.Drawing.Size(1066, 40);
            this.panelBtn.TabIndex = 0;

            this.btnStartAll.Location = new System.Drawing.Point(8, 6);
            this.btnStartAll.Name = "btnStartAll";
            this.btnStartAll.Size = new System.Drawing.Size(120, 28);
            this.btnStartAll.TabIndex = 0;
            this.btnStartAll.Text = "启动全部监控";
            this.btnStartAll.UseVisualStyleBackColor = true;
            this.btnStartAll.Click += new System.EventHandler(this.btnStartAll_Click);

            this.btnStopAll.Location = new System.Drawing.Point(134, 6);
            this.btnStopAll.Name = "btnStopAll";
            this.btnStopAll.Size = new System.Drawing.Size(120, 28);
            this.btnStopAll.TabIndex = 1;
            this.btnStopAll.Text = "停止全部监控";
            this.btnStopAll.UseVisualStyleBackColor = true;
            this.btnStopAll.Click += new System.EventHandler(this.btnStopAll_Click);

            this.btnOpenProcessMgr.Location = new System.Drawing.Point(260, 6);
            this.btnOpenProcessMgr.Name = "btnOpenProcessMgr";
            this.btnOpenProcessMgr.Size = new System.Drawing.Size(120, 28);
            this.btnOpenProcessMgr.TabIndex = 2;
            this.btnOpenProcessMgr.Text = "进程管理器";
            this.btnOpenProcessMgr.UseVisualStyleBackColor = true;
            this.btnOpenProcessMgr.Click += new System.EventHandler(this.btnOpenProcessMgr_Click);

            this.btnOpenConfig.Location = new System.Drawing.Point(386, 6);
            this.btnOpenConfig.Name = "btnOpenConfig";
            this.btnOpenConfig.Size = new System.Drawing.Size(120, 28);
            this.btnOpenConfig.TabIndex = 3;
            this.btnOpenConfig.Text = "黑白名单配置";
            this.btnOpenConfig.UseVisualStyleBackColor = true;
            this.btnOpenConfig.Click += new System.EventHandler(this.OpenConfigWindow);

            this.btnOpenLogViewer.Location = new System.Drawing.Point(512, 6);
            this.btnOpenLogViewer.Name = "btnOpenLogViewer";
            this.btnOpenLogViewer.Size = new System.Drawing.Size(120, 28);
            this.btnOpenLogViewer.TabIndex = 4;
            this.btnOpenLogViewer.Text = "完整日志查看器";
            this.btnOpenLogViewer.UseVisualStyleBackColor = true;
            this.btnOpenLogViewer.Click += new System.EventHandler(this.btnOpenLogViewer_Click);

            this.btnClearLog.Location = new System.Drawing.Point(638, 6);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(100, 28);
            this.btnClearLog.TabIndex = 5;
            this.btnClearLog.Text = "清空界面日志";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);

            this.tabPageQuickScan.Controls.Add(this.labelScanTip);
            this.tabPageQuickScan.Controls.Add(this.btnManualScan);
            this.tabPageQuickScan.Location = new System.Drawing.Point(4, 24);
            this.tabPageQuickScan.Name = "tabPageQuickScan";
            this.tabPageQuickScan.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageQuickScan.Size = new System.Drawing.Size(1072, 592);
            this.tabPageQuickScan.TabIndex = 1;
            this.tabPageQuickScan.Text = "手动一键扫描";
            this.tabPageQuickScan.UseVisualStyleBackColor = true;

            this.btnManualScan.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.btnManualScan.Location = new System.Drawing.Point(420, 240);
            this.btnManualScan.Name = "btnManualScan";
            this.btnManualScan.Size = new System.Drawing.Size(220, 60);
            this.btnManualScan.TabIndex = 0;
            this.btnManualScan.Text = "手动全盘进程扫描";
            this.btnManualScan.UseVisualStyleBackColor = true;
            this.btnManualScan.Click += new System.EventHandler(this.btnManualScan_Click);

            this.labelScanTip.AutoSize = true;
            this.labelScanTip.Location = new System.Drawing.Point(360, 320);
            this.labelScanTip.Name = "labelScanTip";
            this.labelScanTip.Size = new System.Drawing.Size(340, 15);
            this.labelScanTip.TabIndex = 1;
            this.labelScanTip.Text = "手动扫描会一次性遍历所有进程，标记并拦截当前恶意程序";

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 620);
            this.Controls.Add(this.tabControlMain);
            this.Name = "MainMonitorForm";
            this.Text = "SecureTool 多维度安全防护工具";

            // 托盘控件，components已实例化，不再传null
            this._trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this._trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._menuShowMain = new System.Windows.Forms.ToolStripMenuItem();
            this._menuStartMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this._menuStopMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this._menuOpenConfig = new System.Windows.Forms.ToolStripMenuItem();
            this._menuSep = new System.Windows.Forms.ToolStripSeparator();
            this._menuExit = new System.Windows.Forms.ToolStripMenuItem();

            this._trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this._menuShowMain,
                this._menuStartMonitor,
                this._menuStopMonitor,
                this._menuOpenConfig,
                this._menuSep,
                this._menuExit
            });

            this._menuShowMain.Text = "显示主窗口";
            this._menuStartMonitor.Text = "启动全部监控";
            this._menuStopMonitor.Text = "停止全部监控";
            this._menuOpenConfig.Text = "打开黑白名单配置";
            this._menuExit.Text = "完全退出程序";

            this._trayIcon.ContextMenuStrip = this._trayMenu;
            this._trayIcon.Icon = System.Drawing.SystemIcons.Shield;
            this._trayIcon.Text = "SecureTool 多维度安全防护工具 | 未启动监控";

            this.tabControlMain.ResumeLayout(false);
            this.tabPageMonitor.ResumeLayout(false);
            this.tabPageMonitor.PerformLayout();
            this.panelStatus.ResumeLayout(false);
            this.panelStatus.PerformLayout();
            this.panelBtn.ResumeLayout(false);
            this.tabPageQuickScan.ResumeLayout(false);
            this.tabPageQuickScan.PerformLayout();
            this.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageMonitor;
        private System.Windows.Forms.TabPage tabPageQuickScan;
        private System.Windows.Forms.Panel panelBtn;
        private System.Windows.Forms.Button btnStartAll;
        private System.Windows.Forms.Button btnStopAll;
        private System.Windows.Forms.Button btnOpenProcessMgr;
        private System.Windows.Forms.Button btnOpenConfig;
        private System.Windows.Forms.Button btnOpenLogViewer;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressMonitor;
        private System.Windows.Forms.Button btnManualScan;
        private System.Windows.Forms.Label labelScanTip;
        private System.Windows.Forms.NotifyIcon _trayIcon;
        private System.Windows.Forms.ContextMenuStrip _trayMenu;
        private System.Windows.Forms.ToolStripMenuItem _menuShowMain;
        private System.Windows.Forms.ToolStripMenuItem _menuStartMonitor;
        private System.Windows.Forms.ToolStripMenuItem _menuStopMonitor;
        private System.Windows.Forms.ToolStripMenuItem _menuOpenConfig;
        private System.Windows.Forms.ToolStripSeparator _menuSep;
        private System.Windows.Forms.ToolStripMenuItem _menuExit;
    }
}