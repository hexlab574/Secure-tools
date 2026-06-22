using System;
using System.IO;
using System.Windows.Forms;

namespace Secure_tool
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 全局捕获所有线程未处理异常，生成崩溃堆栈日志
            AppDomain.CurrentDomain.UnhandledException += GlobalExceptionCatch;
            Application.ThreadException += UIThreadExceptionCatch;

            try
            {
                Application.Run(new MainMonitorForm());
            }
            catch (Exception ex)
            {
                WriteCrashLog(ex, "主线程启动崩溃");
                MessageBox.Show($"程序启动崩溃：{ex.Message}\n崩溃日志保存在程序目录 CrashLog.txt", "致命错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 后台非UI线程全局异常
        private static void GlobalExceptionCatch(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
                WriteCrashLog(ex, "后台监控线程未捕获异常");
        }

        // UI界面线程异常
        private static void UIThreadExceptionCatch(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            WriteCrashLog(e.Exception, "UI界面操作异常");
            MessageBox.Show($"界面发生异常：{e.Exception.Message}\n程序将继续运行", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // 写入崩溃日志，静默捕获写入失败避免二次崩溃
        private static void WriteCrashLog(Exception ex, string crashType)
        {
            try
            {
                string logPath = Path.Combine(Application.StartupPath, "CrashLog.txt");
                string content = $"==================== {DateTime.Now:yyyy-MM-dd HH:mm:ss} ====================\n" +
                                 $"崩溃类型：{crashType}\n" +
                                 $"异常消息：{ex.Message}\n" +
                                 $"完整堆栈：\n{ex.StackTrace}\n\n";
                File.AppendAllText(logPath, content, System.Text.Encoding.UTF8);
            }
            catch
            {
            }
        }
    }
}