using GC_MES.WinForm.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GC_MES.WinForm.Forms.SystemForm
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            Load += SettingsForm_Load;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // 加载主题列表
            LoadThemes();

            // 设置当前主题选中
            SetCurrentTheme();

            // 应用当前主题到窗体
            ApplyTheme();
        }

        private void LoadThemes()
        {
            cmbTheme.Items.Clear();
            foreach (var theme in ThemeManager.Instance.AvailableThemes)
            {
                cmbTheme.Items.Add(theme.DisplayName);
            }
        }

        private void SetCurrentTheme()
        {
            var currentTheme = ThemeManager.Instance.CurrentTheme;
            int index = ThemeManager.Instance.AvailableThemes.FindIndex(t => t.Name == currentTheme.Name);
            if (index >= 0)
            {
                cmbTheme.SelectedIndex = index;
            }
        }

        private void ApplyTheme()
        {
            ThemeManager.Instance.ApplyTheme(this);
        }

        private void cmbTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = cmbTheme.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < ThemeManager.Instance.AvailableThemes.Count)
            {
                var selectedTheme = ThemeManager.Instance.AvailableThemes[selectedIndex];
                ThemeManager.Instance.ChangeTheme(selectedTheme.Name);
                ApplyTheme();

                // 显示重启提示
                lblRestartHint.Visible = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MessageBox.Show("设置已保存，部分设置在重启应用后生效。", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
} 