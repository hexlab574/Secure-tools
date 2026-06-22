namespace Secure_tool
{
    partial class ConfigEditForm
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
            this.tabControlCfg = new System.Windows.Forms.TabControl();
            this.tabTrustDir = new System.Windows.Forms.TabPage();
            this.txtTrustDir = new System.Windows.Forms.TextBox();
            this.tabCoreProc = new System.Windows.Forms.TabPage();
            this.txtCoreProc = new System.Windows.Forms.TextBox();
            this.tabMalKey = new System.Windows.Forms.TabPage();
            this.txtMalKey = new System.Windows.Forms.TextBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.chkShowTrustLog = new System.Windows.Forms.CheckBox();
            this.chkAutoKill = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tabControlCfg.SuspendLayout();
            this.tabTrustDir.SuspendLayout();
            this.tabCoreProc.SuspendLayout();
            this.tabMalKey.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();

            this.tabControlCfg.Controls.Add(this.tabTrustDir);
            this.tabControlCfg.Controls.Add(this.tabCoreProc);
            this.tabControlCfg.Controls.Add(this.tabMalKey);
            this.tabControlCfg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlCfg.Location = new System.Drawing.Point(0, 0);
            this.tabControlCfg.Name = "tabControlCfg";
            this.tabControlCfg.SelectedIndex = 0;
            this.tabControlCfg.Size = new System.Drawing.Size(700, 460);
            this.tabControlCfg.TabIndex = 0;

            this.tabTrustDir.Controls.Add(this.txtTrustDir);
            this.tabTrustDir.Location = new System.Drawing.Point(4, 24);
            this.tabTrustDir.Name = "tabTrustDir";
            this.tabTrustDir.Size = new System.Drawing.Size(692, 432);
            this.tabTrustDir.Text = "可信目录（一行一条）";
            this.tabTrustDir.UseVisualStyleBackColor = true;

            this.txtTrustDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTrustDir.Multiline = true;
            this.txtTrustDir.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTrustDir.Size = new System.Drawing.Size(692, 432);

            this.tabCoreProc.Controls.Add(this.txtCoreProc);
            this.tabCoreProc.Location = new System.Drawing.Point(4, 24);
            this.tabCoreProc.Name = "tabCoreProc";
            this.tabCoreProc.Size = new System.Drawing.Size(692, 432);
            this.tabCoreProc.Text = "核心可信进程";
            this.tabCoreProc.UseVisualStyleBackColor = true;

            this.txtCoreProc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCoreProc.Multiline = true;
            this.txtCoreProc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCoreProc.Size = new System.Drawing.Size(692, 432);

            this.tabMalKey.Controls.Add(this.txtMalKey);
            this.tabMalKey.Location = new System.Drawing.Point(4, 24);
            this.tabMalKey.Name = "tabMalKey";
            this.tabMalKey.Size = new System.Drawing.Size(692, 432);
            this.tabMalKey.Text = "恶意关键词";
            this.tabMalKey.UseVisualStyleBackColor = true;

            this.txtMalKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMalKey.Multiline = true;
            this.txtMalKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMalKey.Size = new System.Drawing.Size(692, 432);

            this.panelBottom.Controls.Add(this.chkShowTrustLog);
            this.panelBottom.Controls.Add(this.chkAutoKill);
            this.panelBottom.Controls.Add(this.btnCancel);
            this.panelBottom.Controls.Add(this.btnSave);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 460);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(700, 60);
            this.panelBottom.TabIndex = 1;

            this.chkAutoKill.Location = new System.Drawing.Point(12, 18);
            this.chkAutoKill.Name = "chkAutoKill";
            this.chkAutoKill.Size = new System.Drawing.Size(140, 20);
            this.chkAutoKill.Text = "自动终止恶意程序";

            this.chkShowTrustLog.Location = new System.Drawing.Point(160, 18);
            this.chkShowTrustLog.Name = "chkShowTrustLog";
            this.chkShowTrustLog.Size = new System.Drawing.Size(160, 20);
            this.chkShowTrustLog.Text = "显示可信进程日志";

            this.btnSave.Location = new System.Drawing.Point(460, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 32);
            this.btnSave.Text = "保存配置";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnCancel.Location = new System.Drawing.Point(570, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 32);
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            this.ClientSize = new System.Drawing.Size(700, 520);
            this.Controls.Add(this.tabControlCfg);
            this.Controls.Add(this.panelBottom);
            this.Name = "ConfigEditForm";
            this.Text = "黑白名单配置编辑器";
            this.tabControlCfg.ResumeLayout(false);
            this.tabTrustDir.ResumeLayout(false);
            this.tabTrustDir.PerformLayout();
            this.tabCoreProc.ResumeLayout(false);
            this.tabCoreProc.PerformLayout();
            this.tabMalKey.ResumeLayout(false);
            this.tabMalKey.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        #endregion
        private System.Windows.Forms.TabControl tabControlCfg;
        private System.Windows.Forms.TabPage tabTrustDir;
        private System.Windows.Forms.TextBox txtTrustDir;
        private System.Windows.Forms.TabPage tabCoreProc;
        private System.Windows.Forms.TextBox txtCoreProc;
        private System.Windows.Forms.TabPage tabMalKey;
        private System.Windows.Forms.TextBox txtMalKey;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.CheckBox chkAutoKill;
        private System.Windows.Forms.CheckBox chkShowTrustLog;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}