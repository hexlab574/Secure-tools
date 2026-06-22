
using System;
using System.IO;
using System.Windows.Forms;

namespace Secure_tool
{
    public partial class LogViewerForm : Form
    {
        public LogViewerForm()
        {
            InitializeComponent();
        }

        private void LogViewerForm_Load(object sender, EventArgs e)
        {
            LoadTodayLog();
        }

        // 读取今日日志
        private void LoadTodayLog()
        {
            string logDir = Path.Combine(Application.StartupPath, "Logs");
            string logFile = Path.Combine(logDir, $"{DateTime.Now:yyyy-MM-dd}.log");
            if (File.Exists(logFile))
                txtFullLog.Text = File.ReadAllText(logFile);
            else
                txtFullLog.Text = "今日暂无日志记录";
        }

        // 刷新按钮
        private void btnReloadLog_Click(object sender, EventArgs e)
        {
            LoadTodayLog();
        }
    }
}