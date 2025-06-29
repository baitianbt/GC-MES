using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace GC_MES.WinForm.Common
{
    /// <summary>
    /// 主题管理器
    /// </summary>
    public class ThemeManager
    {
        #region 单例模式

        private static ThemeManager _instance;
        public static ThemeManager Instance => _instance ?? (_instance = new ThemeManager());

        #endregion

        #region 主题颜色定义

        /// <summary>
        /// 主题定义
        /// </summary>
        public class Theme
        {
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public Color PrimaryColor { get; set; }
            public Color SecondaryColor { get; set; }
            public Color AccentColor { get; set; }
            public Color TextColor { get; set; }
            public Color BackgroundColor { get; set; }
            public Color PanelBackColor { get; set; }
            public Color ButtonPrimaryColor { get; set; }
            public Color ButtonSecondaryColor { get; set; }
            public Color ButtonDangerColor { get; set; }
            public Color ButtonTextColor { get; set; }
            public Color GridHeaderBackColor { get; set; }
            public Color GridHeaderForeColor { get; set; }
            public Color GridRowBackColor { get; set; }
            public Color GridAlternatingRowBackColor { get; set; }
            public Color GridSelectionBackColor { get; set; }
            public Color GridSelectionForeColor { get; set; }
            public Color MenuBackColor { get; set; }
            public Color MenuItemHoverColor { get; set; }
            public Color MenuItemSelectedColor { get; set; }
            public Color MenuTextColor { get; set; }
        }

        // 默认主题 - 深色
        public static readonly Theme DarkTheme = new Theme
        {
            Name = "Dark",
            DisplayName = "深色",
            PrimaryColor = Color.FromArgb(45, 45, 48),
            SecondaryColor = Color.FromArgb(67, 67, 70),
            AccentColor = Color.FromArgb(0, 122, 204),
            TextColor = Color.White,
            BackgroundColor = Color.FromArgb(30, 30, 30),
            PanelBackColor = Color.FromArgb(240, 240, 240),
            ButtonPrimaryColor = Color.FromArgb(45, 45, 48),
            ButtonSecondaryColor = Color.FromArgb(67, 67, 70),
            ButtonDangerColor = Color.FromArgb(159, 68, 74),
            ButtonTextColor = Color.White,
            GridHeaderBackColor = Color.FromArgb(45, 45, 48),
            GridHeaderForeColor = Color.White,
            GridRowBackColor = Color.White,
            GridAlternatingRowBackColor = Color.FromArgb(250, 250, 250),
            GridSelectionBackColor = Color.FromArgb(67, 67, 70),
            GridSelectionForeColor = Color.White,
            MenuBackColor = Color.FromArgb(45, 45, 48),
            MenuItemHoverColor = Color.FromArgb(55, 55, 58),
            MenuItemSelectedColor = Color.FromArgb(62, 62, 64),
            MenuTextColor = Color.White
        };

        // 蓝色主题
        public static readonly Theme BlueTheme = new Theme
        {
            Name = "Blue",
            DisplayName = "蓝色",
            PrimaryColor = Color.FromArgb(37, 54, 75),
            SecondaryColor = Color.FromArgb(45, 63, 88),
            AccentColor = Color.FromArgb(65, 137, 221),
            TextColor = Color.White,
            BackgroundColor = Color.FromArgb(240, 242, 245),
            PanelBackColor = Color.FromArgb(250, 250, 250),
            ButtonPrimaryColor = Color.FromArgb(37, 54, 75),
            ButtonSecondaryColor = Color.FromArgb(45, 63, 88),
            ButtonDangerColor = Color.FromArgb(159, 68, 74),
            ButtonTextColor = Color.White,
            GridHeaderBackColor = Color.FromArgb(37, 54, 75),
            GridHeaderForeColor = Color.White,
            GridRowBackColor = Color.White,
            GridAlternatingRowBackColor = Color.FromArgb(245, 248, 250),
            GridSelectionBackColor = Color.FromArgb(65, 137, 221),
            GridSelectionForeColor = Color.White,
            MenuBackColor = Color.FromArgb(37, 54, 75),
            MenuItemHoverColor = Color.FromArgb(45, 63, 88),
            MenuItemSelectedColor = Color.FromArgb(65, 137, 221),
            MenuTextColor = Color.White
        };

        // 绿色主题
        public static readonly Theme GreenTheme = new Theme
        {
            Name = "Green",
            DisplayName = "绿色",
            PrimaryColor = Color.FromArgb(44, 62, 53),
            SecondaryColor = Color.FromArgb(53, 74, 64),
            AccentColor = Color.FromArgb(60, 179, 113),
            TextColor = Color.White,
            BackgroundColor = Color.FromArgb(240, 245, 240),
            PanelBackColor = Color.FromArgb(250, 250, 250),
            ButtonPrimaryColor = Color.FromArgb(44, 62, 53),
            ButtonSecondaryColor = Color.FromArgb(53, 74, 64),
            ButtonDangerColor = Color.FromArgb(159, 68, 74),
            ButtonTextColor = Color.White,
            GridHeaderBackColor = Color.FromArgb(44, 62, 53),
            GridHeaderForeColor = Color.White,
            GridRowBackColor = Color.White,
            GridAlternatingRowBackColor = Color.FromArgb(245, 250, 245),
            GridSelectionBackColor = Color.FromArgb(60, 179, 113),
            GridSelectionForeColor = Color.White,
            MenuBackColor = Color.FromArgb(44, 62, 53),
            MenuItemHoverColor = Color.FromArgb(53, 74, 64),
            MenuItemSelectedColor = Color.FromArgb(60, 179, 113),
            MenuTextColor = Color.White
        };

        // 紫色主题
        public static readonly Theme PurpleTheme = new Theme
        {
            Name = "Purple",
            DisplayName = "紫色",
            PrimaryColor = Color.FromArgb(60, 45, 80),
            SecondaryColor = Color.FromArgb(80, 60, 100),
            AccentColor = Color.FromArgb(147, 112, 219),
            TextColor = Color.White,
            BackgroundColor = Color.FromArgb(242, 240, 245),
            PanelBackColor = Color.FromArgb(250, 250, 250),
            ButtonPrimaryColor = Color.FromArgb(60, 45, 80),
            ButtonSecondaryColor = Color.FromArgb(80, 60, 100),
            ButtonDangerColor = Color.FromArgb(159, 68, 74),
            ButtonTextColor = Color.White,
            GridHeaderBackColor = Color.FromArgb(60, 45, 80),
            GridHeaderForeColor = Color.White,
            GridRowBackColor = Color.White,
            GridAlternatingRowBackColor = Color.FromArgb(245, 240, 250),
            GridSelectionBackColor = Color.FromArgb(147, 112, 219),
            GridSelectionForeColor = Color.White,
            MenuBackColor = Color.FromArgb(60, 45, 80),
            MenuItemHoverColor = Color.FromArgb(80, 60, 100),
            MenuItemSelectedColor = Color.FromArgb(147, 112, 219),
            MenuTextColor = Color.White
        };

        #endregion

        #region 属性

        // 所有可用主题
        public List<Theme> AvailableThemes { get; } = new List<Theme>
        {
            DarkTheme,
            BlueTheme,
            GreenTheme,
            PurpleTheme
        };

        // 当前主题
        private Theme _currentTheme = DarkTheme;
        public Theme CurrentTheme
        {
            get => _currentTheme;
            private set
            {
                if (_currentTheme != value)
                {
                    _currentTheme = value;
                    OnThemeChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        // 主题更改事件
        public event EventHandler OnThemeChanged;

        #endregion

        #region 构造函数

        private ThemeManager()
        {
            // 读取保存的主题设置
            LoadTheme();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 更改当前主题
        /// </summary>
        /// <param name="themeName">主题名称</param>
        public void ChangeTheme(string themeName)
        {
            var theme = AvailableThemes.FirstOrDefault(t => t.Name.Equals(themeName, StringComparison.OrdinalIgnoreCase));
            if (theme != null)
            {
                CurrentTheme = theme;
                SaveTheme();
            }
        }

        /// <summary>
        /// 应用主题到窗体
        /// </summary>
        /// <param name="form">窗体</param>
        public void ApplyTheme(Form form)
        {
            if (form == null) return;

            // 应用到表单级控件
            ApplyThemeToControls(form.Controls);

            // 更新窗体背景色
            form.BackColor = CurrentTheme.BackgroundColor;
        }

        /// <summary>
        /// 更新DataGridView样式
        /// </summary>
        /// <param name="dgv">DataGridView控件</param>
        public void UpdateDataGridViewStyle(DataGridView dgv)
        {
            if (dgv == null) return;

            // 设置列头样式
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = CurrentTheme.GridHeaderBackColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = CurrentTheme.GridHeaderForeColor;

            // 设置行样式
            dgv.RowsDefaultCellStyle.BackColor = CurrentTheme.GridRowBackColor;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = CurrentTheme.GridAlternatingRowBackColor;
            dgv.DefaultCellStyle.SelectionBackColor = CurrentTheme.GridSelectionBackColor;
            dgv.DefaultCellStyle.SelectionForeColor = CurrentTheme.GridSelectionForeColor;

            // 设置边框和网格线
            dgv.GridColor = Color.FromArgb(230, 230, 230);
        }

        /// <summary>
        /// 更新Panel样式
        /// </summary>
        /// <param name="panel">Panel控件</param>
        /// <param name="isToolbar">是否为工具栏Panel</param>
        public void UpdatePanelStyle(Panel panel, bool isToolbar = false)
        {
            if (panel == null) return;

            if (isToolbar)
            {
                panel.BackColor = Color.FromArgb(240, 240, 240);
            }
            else
            {
                panel.BackColor = CurrentTheme.PanelBackColor;
            }
            
            ApplyThemeToControls(panel.Controls);
        }

        /// <summary>
        /// 更新菜单样式
        /// </summary>
        /// <param name="menu">DMenu控件</param>
        public void UpdateMenuStyle(Controls.DMenu menu)
        {
            if (menu == null) return;

            menu.BackColor = CurrentTheme.MenuBackColor;
            menu.MenuBackColor = CurrentTheme.MenuBackColor;
            menu.MenuItemHoverColor = CurrentTheme.MenuItemHoverColor;
            menu.MenuItemSelectedColor = CurrentTheme.MenuItemSelectedColor;
            menu.MenuTextColor = CurrentTheme.MenuTextColor;
            menu.ForeColor = CurrentTheme.MenuTextColor;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 应用主题到控件集合
        /// </summary>
        /// <param name="controls">控件集合</param>
        private void ApplyThemeToControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                // 递归应用到子控件
                if (control.Controls.Count > 0)
                {
                    ApplyThemeToControls(control.Controls);
                }

                // 根据控件类型应用样式
                if (control is Panel panel)
                {
                    // 特殊情况：标题栏和状态栏
                    if (panel.Name == "pnlHeader" || panel.Name == "pnlStatus")
                    {
                        panel.BackColor = CurrentTheme.PrimaryColor;
                        ApplyThemeToControls(panel.Controls);
                    }
                    else if (panel.Name == "pnlToolbar")
                    {
                        UpdatePanelStyle(panel, true);
                    }
                    else
                    {
                        UpdatePanelStyle(panel);
                    }
                }
                else if (control is Button button)
                {
                    // 根据按钮标签决定风格
                    if (button.Text.Contains("新增") || button.Text == "登 录")
                    {
                        button.BackColor = CurrentTheme.ButtonPrimaryColor;
                        button.ForeColor = CurrentTheme.ButtonTextColor;
                    }
                    else if (button.Text.Contains("删除"))
                    {
                        button.BackColor = CurrentTheme.ButtonDangerColor;
                        button.ForeColor = CurrentTheme.ButtonTextColor;
                    }
                    else if (button.Text.Contains("编辑") || button.Text.Contains("导出") || button.Text.Contains("导入"))
                    {
                        button.BackColor = CurrentTheme.ButtonSecondaryColor;
                        button.ForeColor = CurrentTheme.ButtonTextColor;
                    }
                    // 特殊按钮（标题栏按钮）保持原样
                    else if (button.Name == "btnClose" || button.Name == "btnMinimize" || 
                             button.Name == "btnMaximize" || button.Name == "btnSetting")
                    {
                        // 保持原样
                    }
                }
                else if (control is Label label)
                {
                    // 特殊情况：标题栏标签
                    if (control.Parent != null && (control.Parent.Name == "pnlHeader" || control.Parent.Name == "pnlStatus"))
                    {
                        label.ForeColor = CurrentTheme.TextColor;
                    }
                }
                else if (control is DataGridView dataGridView)
                {
                    UpdateDataGridViewStyle(dataGridView);
                }
                else if (control is Controls.DMenu menu)
                {
                    UpdateMenuStyle(menu);
                }
            }
        }

        /// <summary>
        /// 保存主题设置
        /// </summary>
        private void SaveTheme()
        {
            try
            {
                string configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "theme.config");
                
                using (StreamWriter writer = new StreamWriter(configFile))
                {
                    writer.WriteLine(CurrentTheme.Name);
                }
            }
            catch
            {
                // 忽略保存错误
            }
        }

        /// <summary>
        /// 加载主题设置
        /// </summary>
        private void LoadTheme()
        {
            try
            {
                string configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "theme.config");
                
                if (File.Exists(configFile))
                {
                    string themeName = File.ReadAllText(configFile).Trim();
                    var theme = AvailableThemes.FirstOrDefault(t => t.Name.Equals(themeName, StringComparison.OrdinalIgnoreCase));
                    if (theme != null)
                    {
                        CurrentTheme = theme;
                    }
                }
            }
            catch
            {
                // 加载失败使用默认主题
                CurrentTheme = DarkTheme;
            }
        }

        #endregion
    }
} 