namespace Secure_tool
{
    partial class ProcessManagerForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }
        #region 设计器生成代码
        private void InitializeComponent()
        {
            this.listViewProcess = new System.Windows.Forms.ListView();
            this.colPID = new System.Windows.Forms.ColumnHeader();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colPath = new System.Windows.Forms.ColumnHeader();
            this.panelBtn = new System.Windows.Forms.Panel();
            this.btnKillSelected = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panelBtn.SuspendLayout();
            this.SuspendLayout();

            this.listViewProcess.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
            {
                this.colPID,
                this.colName,
                this.colPath
            });
            this.listViewProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewProcess.FullRowSelect = true;
            this.listViewProcess.Location = new System.Drawing.Point(0, 40);
            this.listViewProcess.Name = "listViewProcess";
            this.listViewProcess.Size = new System.Drawing.Size(900, 520);
            this.listViewProcess.TabIndex = 0;
            this.listViewProcess.View = System.Windows.Forms.View.Details;

            this.colPID.Text = "PID";
            this.colPID.Width = 80;
            this.colName.Text = "进程名";
            this.colName.Width = 180;
            this.colPath.Text = "程序路径";
            this.colPath.Width = 600;

            this.panelBtn.Controls.Add(this.btnKillSelected);
            this.panelBtn.Controls.Add(this.btnRefresh);
            this.panelBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBtn.Location = new System.Drawing.Point(0, 0);
            this.panelBtn.Name = "panelBtn";
            this.panelBtn.Size = new System.Drawing.Size(900, 40);
            this.panelBtn.TabIndex = 1;

            this.btnRefresh.Location = new System.Drawing.Point(12, 6);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(110, 28);
            this.btnRefresh.Text = "刷新进程列表";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            this.btnKillSelected.Location = new System.Drawing.Point(130, 6);
            this.btnKillSelected.Name = "btnKillSelected";
            this.btnKillSelected.Size = new System.Drawing.Size(110, 28);
            this.btnKillSelected.Text = "终止选中进程";
            this.btnKillSelected.UseVisualStyleBackColor = true;
            this.btnKillSelected.Click += new System.EventHandler(this.btnKillSelected_Click);

            this.ClientSize = new System.Drawing.Size(900, 560);
            this.Controls.Add(this.listViewProcess);
            this.Controls.Add(this.panelBtn);
            this.Name = "ProcessManagerForm";
            this.Text = "进程管理器";
            this.Load += new System.EventHandler(this.ProcessManagerForm_Load);
            this.panelBtn.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        #endregion
        public System.Windows.Forms.ListView listViewProcess;
        private System.Windows.Forms.ColumnHeader colPID;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colPath;
        private System.Windows.Forms.Panel panelBtn;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnKillSelected;
    }
}