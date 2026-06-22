using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace Secure_tool
{
    public partial class ConfigEditForm : Form
    {
        private readonly string _cfgPath;
        private AppConfig _editCfg;

        // 构造：接收配置文件路径与配置对象引用
        public ConfigEditForm(string path, ref AppConfig cfg)
        {
            InitializeComponent();
            _cfgPath = path;
            _editCfg = cfg;

            // 加载现有配置到文本框
            txtTrustDir.Text = string.Join("\r\n", _editCfg.TrustDirList);
            txtCoreProc.Text = string.Join("\r\n", _editCfg.CoreSysProcList);
            txtMalKey.Text = string.Join("\r\n", _editCfg.MalKeywordList);

            chkAutoKill.Checked = _editCfg.AutoKillMalware;
            chkShowTrustLog.Checked = _editCfg.ShowTrustedLog;
        }

        // 保存按钮
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 按行分割文本，去除空行
            _editCfg.TrustDirList = txtTrustDir.Text
                .Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
            _editCfg.CoreSysProcList = txtCoreProc.Text
                .Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
            _editCfg.MalKeywordList = txtMalKey.Text
                .Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            // 复选框开关
            _editCfg.AutoKillMalware = chkAutoKill.Checked;
            _editCfg.ShowTrustedLog = chkShowTrustLog.Checked;

            // 写入JSON配置文件
            File.WriteAllText(_cfgPath, JsonSerializer.Serialize(_editCfg, new JsonSerializerOptions { WriteIndented = true }));

            DialogResult = DialogResult.OK;
            Close();
        }

        // 取消按钮
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}