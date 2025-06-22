using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GC_MES.WinForm.Forms
{
    /// <summary>
    /// 主题类型枚举
    /// </summary>
    public enum ThemeType
    {
        Dark,
        Light,
        Blue,
        Green
    }

    /// <summary>
    /// 主题颜色定义
    /// </summary>
    public class ThemeColors
    {
        public Color PrimaryColor { get; set; }      // 主色调
        public Color SecondaryColor { get; set; }    // 次级色调
        public Color BackgroundColor { get; set; }   // 背景色
        public Color MenuColor { get; set; }         // 菜单背景色
        public Color MenuActiveColor { get; set; }   // 菜单活动项色
        public Color TextColor { get; set; }         // 文本色
        public Color BorderColor { get; set; }       // 边框色
        public Color HeaderColor { get; set; }       // 标题栏色
        public Color StatusColor { get; set; }       // 状态栏色
    }

    /// <summary>
    /// 主题管理器
    /// </summary>
    public static class ThemeManager
    {
        private static ThemeType _currentThemeType = ThemeType.Dark;
        
        /// <summary>
        /// 当前主题类型
        /// </summary>
        public static ThemeType CurrentTheme
        {
            get { return _currentThemeType; }
            set
            {
                if (_currentThemeType != value)
                {
                    _currentThemeType = value;
                    if (ThemeChanged != null)
                        ThemeChanged.Invoke(null, new ThemeChangedEventArgs { ThemeType = value });
                }
            }
        }
        
        /// <summary>
        /// 主题切换事件
        /// </summary>
        public static event EventHandler<ThemeChangedEventArgs> ThemeChanged;
        
        /// <summary>
        /// 当前主题颜色
        /// </summary>
        public static ThemeColors CurrentThemeColors
        {
            get { return GetThemeColors(_currentThemeType); }
        }
        
        /// <summary>
        /// 根据主题类型获取主题颜色
        /// </summary>
        public static ThemeColors GetThemeColors(ThemeType themeType)
        {
            switch (themeType)
            {
                case ThemeType.Dark:
                    return new ThemeColors
                    {
                        PrimaryColor = Color.FromArgb(45, 63, 88),
                        SecondaryColor = Color.FromArgb(32, 47, 66),
                        BackgroundColor = Color.FromArgb(50, 50, 50),
                        MenuColor = Color.FromArgb(32, 47, 66),
                        MenuActiveColor = Color.FromArgb(45, 63, 88),
                        TextColor = Color.White,
                        BorderColor = Color.FromArgb(60, 60, 60),
                        HeaderColor = Color.FromArgb(45, 63, 88),
                        StatusColor = Color.FromArgb(45, 63, 88)
                    };
                    
                case ThemeType.Light:
                    return new ThemeColors
                    {
                        PrimaryColor = Color.FromArgb(0, 122, 204),
                        SecondaryColor = Color.FromArgb(153, 217, 234),
                        BackgroundColor = Color.FromArgb(240, 240, 240),
                        MenuColor = Color.FromArgb(230, 230, 230),
                        MenuActiveColor = Color.FromArgb(0, 122, 204),
                        TextColor = Color.FromArgb(40, 40, 40),
                        BorderColor = Color.FromArgb(200, 200, 200),
                        HeaderColor = Color.FromArgb(0, 122, 204),
                        StatusColor = Color.FromArgb(0, 102, 184)
                    };
                    
                case ThemeType.Blue:
                    return new ThemeColors
                    {
                        PrimaryColor = Color.FromArgb(41, 128, 185),
                        SecondaryColor = Color.FromArgb(52, 152, 219),
                        BackgroundColor = Color.FromArgb(236, 240, 241),
                        MenuColor = Color.FromArgb(41, 128, 185),
                        MenuActiveColor = Color.FromArgb(52, 152, 219),
                        TextColor = Color.White,
                        BorderColor = Color.FromArgb(200, 200, 200),
                        HeaderColor = Color.FromArgb(41, 128, 185),
                        StatusColor = Color.FromArgb(41, 128, 185)
                    };
                    
                case ThemeType.Green:
                    return new ThemeColors
                    {
                        PrimaryColor = Color.FromArgb(39, 174, 96),
                        SecondaryColor = Color.FromArgb(46, 204, 113),
                        BackgroundColor = Color.FromArgb(236, 240, 241),
                        MenuColor = Color.FromArgb(39, 174, 96),
                        MenuActiveColor = Color.FromArgb(46, 204, 113),
                        TextColor = Color.White,
                        BorderColor = Color.FromArgb(200, 200, 200),
                        HeaderColor = Color.FromArgb(39, 174, 96),
                        StatusColor = Color.FromArgb(39, 174, 96)
                    };
                    
                default:
                    return GetThemeColors(ThemeType.Dark);
            }
        }
        
        /// <summary>
        /// 应用当前主题到窗体
        /// </summary>
        public static void ApplyTheme(Form form)
        {
            if (form == null)
                return;
                
            ThemeColors colors = CurrentThemeColors;
            
            // 应用到特定窗体类型
            if (form is MainForm mainForm)
                ApplyThemeToMainForm(mainForm, colors);
            else if (form is LoginForm loginForm)
                ApplyThemeToLoginForm(loginForm, colors);
            else
            {
                // 应用通用设置
                form.BackColor = colors.BackgroundColor;
                ApplyToAllControls(form.Controls, colors);
            }
        }
        
        /// <summary>
        /// 应用主题到登录窗体
        /// </summary>
        private static void ApplyThemeToLoginForm(LoginForm form, ThemeColors colors)
        {
            // 标题栏
            Control pnlHeader = GetControlByName(form, "pnlHeader");
            if (pnlHeader != null)
                pnlHeader.BackColor = colors.HeaderColor;
                
            // 主面板
            Control pnlMain = GetControlByName(form, "pnlMain");
            if (pnlMain != null)
                pnlMain.BackColor = colors.BackgroundColor;
                
            // 登录按钮
            Button btnLogin = GetControlByName(form, "btnLogin") as Button;
            if (btnLogin != null)
            {
                btnLogin.BackColor = colors.PrimaryColor;
                btnLogin.ForeColor = colors.TextColor;
                btnLogin.FlatAppearance.MouseOverBackColor = colors.MenuActiveColor;
            }
            
            // 标题文本
            Label lblTitle = GetControlByName(form, "lblTitle") as Label;
            if (lblTitle != null)
                lblTitle.ForeColor = colors.TextColor;
                
            // 应用到所有文本框等控件
            ApplyToAllControls(form.Controls, colors);
        }
        
        /// <summary>
        /// 应用主题到主窗体
        /// </summary>
        private static void ApplyThemeToMainForm(MainForm form, ThemeColors colors)
        {
            // 标题栏
            Control pnlHeader = GetControlByName(form, "pnlHeader");
            if (pnlHeader != null)
                pnlHeader.BackColor = colors.HeaderColor;
                
            // 菜单面板
            Control pnlMenu = GetControlByName(form, "pnlMenu");
            if (pnlMenu != null)
            {
                pnlMenu.BackColor = colors.MenuColor;
                
                // 菜单按钮
                foreach (Control control in pnlMenu.Controls)
                {
                    if (control is Button btn)
                    {
                        btn.BackColor = colors.MenuColor;
                        btn.ForeColor = colors.TextColor;
                        btn.FlatAppearance.MouseOverBackColor = colors.MenuActiveColor;
                    }
                }
            }
            
            // Logo面板
            Control pnlLogo = GetControlByName(form, "pnlLogo");
            if (pnlLogo != null)
                pnlLogo.BackColor = colors.SecondaryColor;
                
            // 内容面板
            Control pnlContent = GetControlByName(form, "pnlContent");
            if (pnlContent != null)
                pnlContent.BackColor = colors.BackgroundColor;
                
            // 状态栏
            Control pnlStatus = GetControlByName(form, "pnlStatus");
            if (pnlStatus != null)
                pnlStatus.BackColor = colors.StatusColor;
                
            // 标题文本
            Label lblTitle = GetControlByName(form, "lblTitle") as Label;
            if (lblTitle != null)
                lblTitle.ForeColor = colors.TextColor;
                
            // 状态文本
            Label lblStatus = GetControlByName(form, "lblStatus") as Label;
            if (lblStatus != null)
                lblStatus.ForeColor = colors.TextColor;
                
            // 用户名文本
            Label lblUserName = GetControlByName(form, "lblUserName") as Label;
            if (lblUserName != null)
                lblUserName.ForeColor = colors.TextColor;
                
            // Logo文本
            Label lblLogo = GetControlByName(form, "lblLogo") as Label;
            if (lblLogo != null)
                lblLogo.ForeColor = colors.TextColor;
                
            // 应用到所有子控件
            ApplyToAllControls(form.Controls, colors);
        }
        
        /// <summary>
        /// 递归应用主题到所有控件
        /// </summary>
        private static void ApplyToAllControls(Control.ControlCollection controls, ThemeColors colors)
        {
            foreach (Control control in controls)
            {
                // 文本框
                if (control is TextBox textBox)
                {
                    textBox.BackColor = Color.FromArgb(255, colors.BackgroundColor);
                    textBox.ForeColor = colors.TextColor;
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                }
                // 标签
                else if (control is Label label)
                {
                    label.ForeColor = colors.TextColor;
                }
                // 按钮
                else if (control is Button button)
                {
                    button.BackColor = colors.PrimaryColor;
                    button.ForeColor = colors.TextColor;
                    button.FlatAppearance.BorderColor = colors.BorderColor;
                }
                // 复选框
                else if (control is CheckBox checkBox)
                {
                    checkBox.ForeColor = colors.TextColor;
                }
                // 单选按钮
                else if (control is RadioButton radioButton)
                {
                    radioButton.ForeColor = colors.TextColor;
                }
                // 下拉框
                else if (control is ComboBox comboBox)
                {
                    comboBox.BackColor = Color.FromArgb(255, colors.BackgroundColor);
                    comboBox.ForeColor = colors.TextColor;
                }
                // 数据网格
                else if (control is DataGridView grid)
                {
                    grid.BackgroundColor = colors.BackgroundColor;
                    grid.ForeColor = colors.TextColor;
                    grid.GridColor = colors.BorderColor;
                    grid.DefaultCellStyle.BackColor = colors.BackgroundColor;
                    grid.DefaultCellStyle.ForeColor = colors.TextColor;
                    grid.ColumnHeadersDefaultCellStyle.BackColor = colors.PrimaryColor;
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = colors.TextColor;
                    grid.RowHeadersDefaultCellStyle.BackColor = colors.PrimaryColor;
                }
                // 面板
                else if (control is Panel panel)
                {
                    // 跳过已经处理的特殊面板
                    if (control.Name != "pnlHeader" && control.Name != "pnlMenu" && 
                        control.Name != "pnlLogo" && control.Name != "pnlContent" && 
                        control.Name != "pnlStatus")
                    {
                        panel.BackColor = colors.BackgroundColor;
                    }
                }
                
                // 递归处理子控件
                if (control.Controls.Count > 0)
                {
                    ApplyToAllControls(control.Controls, colors);
                }
            }
        }
        
        /// <summary>
        /// 根据名称获取控件
        /// </summary>
        private static Control GetControlByName(Control parent, string name)
        {
            if (parent.Name == name)
                return parent;
                
            foreach (Control control in parent.Controls)
            {
                if (control.Name == name)
                    return control;
                    
                Control found = GetControlByName(control, name);
                if (found != null)
                    return found;
            }
            
            return null;
        }
    }
    
    /// <summary>
    /// 主题更改事件参数
    /// </summary>
    public class ThemeChangedEventArgs : EventArgs
    {
        public ThemeType ThemeType { get; set; }
    }
} 