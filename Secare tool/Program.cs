using System;
using System.Diagnostics;
using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace Secare_tool
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // 检测管理员权限
            bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            if (!isAdmin)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    WorkingDirectory = Environment.CurrentDirectory,
                    FileName = Application.ExecutablePath,
                    Verb = "runas"
                };
                try
                {
                    Process.Start(startInfo);
                }
                catch
                {
                    MessageBox.Show("本工具必须以管理员身份运行！", "权限错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Application.Exit();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}