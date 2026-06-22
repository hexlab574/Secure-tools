namespace Secure_tool
{
    partial class LogViewerForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }
        #region 设计器代码
        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnReloadLog = new System.Windows.Forms.Button();
            this.txtFullLog = new System.Windows.Forms.TextBox();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();

            this.panelTop.Controls.Add(this.btnReloadLog);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1100, 40);
            this.panelTop.TabIndex = 0;

            this.btnReloadLog.Location = new System.Drawing.Point(12, 6);
            this.btnReloadLog.Name = "btnReloadLog";
            this.btnReloadLog.Size = new System.Drawing.Size(110, 28);
            this.btnReloadLog.Text = "刷新今日日志";
            this.btnReloadLog.UseVisualStyleBackColor = true;
            this.btnReloadLog.Click += new System.EventHandler(this.btnReloadLog_Click);

            this.txtFullLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFullLog.Location = new System.Drawing.Point(0, 40);
            this.txtFullLog.Multiline = true;
            this.txtFullLog.ReadOnly = true;
            this.txtFullLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFullLog.Size = new System.Drawing.Size(1100, 600);
            this.txtFullLog.TabIndex = 1;

            this.ClientSize = new System.Drawing.Size(1100, 640);
            this.Controls.Add(this.txtFullLog);
            this.Controls.Add(this.panelTop);
            this.Name = "LogViewerForm";
            this.Text = "完整日志查看器";
            this.Load += new System.EventHandler(this.LogViewerForm_Load);
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        #endregion
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnReloadLog;
        private System.Windows.Forms.TextBox txtFullLog;
    }
}