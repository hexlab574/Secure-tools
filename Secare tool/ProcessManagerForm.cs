using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Windows.Forms;

namespace Secure_tool
{
    public partial class ProcessManagerForm : Form
    {
        private readonly AppConfig _cfg;
        private readonly MainMonitorForm _mainFrm;

        // 带参数构造，接收主窗口与配置
        public ProcessManagerForm(AppConfig cfg, MainMonitorForm main)
        {
            InitializeComponent();
            _cfg = cfg;
            _mainFrm = main;
        }

        private void ProcessManagerForm_Load(object sender, EventArgs e)
        {
            RefreshProcessList();
        }

        // 刷新全部进程列表
        private void RefreshProcessList()
        {
            listViewProcess.Items.Clear();
            try
            {
                using var search = new ManagementObjectSearcher("SELECT ProcessId,Name,ExecutablePath FROM Win32_Process");
                foreach (ManagementBaseObject obj in search.Get())
                {
                    int pid = Convert.ToInt32(obj["ProcessId"]);
                    string name = obj["Name"]?.ToString() ?? "";
                    string path = obj["ExecutablePath"]?.ToString() ?? "无路径";

                    ListViewItem item = new ListViewItem(pid.ToString());
                    item.SubItems.Add(name);
                    item.SubItems.Add(path);
                    listViewProcess.Items.Add(item);
                }
            }
            catch
            {

            }
        }

        // 刷新按钮
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            listViewProcess.Items.Clear();
            RefreshProcessList();
        }

        // 终止选中进程
        private void btnKillSelected_Click(object sender, EventArgs e)
        {
            if (listViewProcess.SelectedItems.Count == 0)
                return;

            foreach (ListViewItem item in listViewProcess.SelectedItems)
            {
                int pid = int.Parse(item.Text);
                try
                {
                    using var p = Process.GetProcessById(pid);
                    p.Kill();
                    MessageBox.Show($"PID {pid} 进程已终止");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"终止失败：{ex.Message}");
                }
            }
            RefreshProcessList();
        }
    }
}