using GC_MES.BLL.System.IService;
using GC_MES.BLL.System.Service;
using GC_MES.WinForm.Common;
using GC_MES.WinForm.Controls;
using GC_MES.WinForm.Forms.SystemForm;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GC_MES.WinForm.Forms
{
    public partial class MainForm : Form
    {
        ISys_MenuService sys_MenuService;

        // 依赖注入
        public MainForm(ISys_MenuService sys_MenuService)
        {
            this.sys_MenuService = sys_MenuService;
            InitializeComponent();
            InitialMenu();
            
            // 注册主题变更事件
            ThemeManager.Instance.OnThemeChanged += ThemeManager_OnThemeChanged;
        }

        private void InitialMenu()
        {
            var MainItem = menu.AddItem("首页");
            MainItem.Click += (s, e) =>
            {
                var formInstance = Program.Services.GetRequiredService<ShowForm>();
                labChildrenFormName.Text = "首页";
                formInstance.TopLevel = false;
                formInstance.FormBorderStyle = FormBorderStyle.None;
                formInstance.Dock = DockStyle.Fill;
                pnlContent.Controls.Clear(); // 可选：只显示一个子窗体
                pnlContent.Controls.Add(formInstance);
                formInstance.Show();
            };

            var DevelopItem = menu.AddItem("开发者功能");

           var CodeGeneratorItem =  DevelopItem.AddSubItem("代码生成器");

            CodeGeneratorItem.Click += (s, e) =>
            {
                var formInstance = Program.Services.GetRequiredService<CodeGeneratorForm>();
                labChildrenFormName.Text = "开发者功能 > 代码生成器";
                formInstance.TopLevel = false;
                formInstance.FormBorderStyle = FormBorderStyle.None;
                formInstance.Dock = DockStyle.Fill;
                pnlContent.Controls.Clear(); // 可选：只显示一个子窗体
                pnlContent.Controls.Add(formInstance);
                formInstance.Show();
            };




            AppInfo.Menus = sys_MenuService.Query();

            #region 向主页添加菜单项
            var ParentMenus = AppInfo.Menus.FindAll(m => m.ParentId == 0);
            // 清空现有菜单项
            foreach (var ParentMenu in ParentMenus)
            {
                // 添加一级菜单项
                var parentItem = menu.AddItem(ParentMenu.MenuName);
                parentItem.Click += (s, e) =>
                {
                    MessageBox.Show("Test");
                };
                // 添加二级子菜单
                var childMenus = AppInfo.Menus.FindAll(m => m.ParentId == ParentMenu.Menu_Id);
                foreach (var childMenu in childMenus)
                {
                    var childItem = parentItem.AddSubItem(childMenu.MenuName);
                    childItem.Click += (s, e) =>
                    {
                        MessageBox.Show("T123est");
                        // 反射获取窗体类型（命名空间+类名）
                        var formType = Type.GetType("GC_MES.WinForm.Forms.SystemForm." + childMenu.Url);
                        if (formType == null)
                        {
                            MessageBox.Show($"未找到窗体类型：{childMenu.Url}");
                            return;
                        }

                        // 从服务容器中获取窗体实例
                        var formInstance = Program.Services.GetRequiredService(formType) as Form;
                        if (formInstance == null)
                        {
                            MessageBox.Show($"无法从 DI 获取窗体实例：{formType.FullName}");
                            return;
                        }
                        // 如果已存在相同类型窗体则激活
                        foreach (Control ctrl in pnlContent.Controls)
                        {
                            if (ctrl is Form form && form.GetType() == formType)
                            {
                                form.BringToFront();
                                return;
                            }
                        }
                        labChildrenFormName.Text = ParentMenu.MenuName + " > " + childMenu.MenuName;
                        formInstance.TopLevel = false;
                        formInstance.FormBorderStyle = FormBorderStyle.None;
                        formInstance.Dock = DockStyle.Fill;
                        pnlContent.Controls.Clear(); // 可选：只显示一个子窗体
                        pnlContent.Controls.Add(formInstance);
                        formInstance.Show();
                        
                        // 应用主题到子窗体
                        ApplyThemeToChildForm(formInstance);
                    };
                }
            }

            #endregion
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 添加拖动窗体的支持
            pnlHeader.MouseDown += PnlHeader_MouseDown;
            pnlHeader.MouseMove += PnlHeader_MouseMove;
            pnlHeader.MouseUp += PnlHeader_MouseUp;

            lblTitle.MouseDown += PnlHeader_MouseDown;
            lblTitle.MouseMove += PnlHeader_MouseMove;
            lblTitle.MouseUp += PnlHeader_MouseUp;

            // 显示用户信息
            lblUserName.Text = AppInfo.CurrentUser.UserName;
            
            // 应用当前主题
            ApplyCurrentTheme();
        }
        
        /// <summary>
        /// 应用当前主题到窗体
        /// </summary>
        private void ApplyCurrentTheme()
        {
            // 应用主题到主窗体
            ThemeManager.Instance.ApplyTheme(this);
            
            // 更新菜单样式
            ThemeManager.Instance.UpdateMenuStyle(menu);
            
            // 应用主题到已打开的子窗体
            foreach (Control control in pnlContent.Controls)
            {
                if (control is Form form)
                {
                    ApplyThemeToChildForm(form);
                }
            }
        }
        
        /// <summary>
        /// 应用主题到子窗体
        /// </summary>
        /// <param name="form">子窗体</param>
        private void ApplyThemeToChildForm(Form form)
        {
            ThemeManager.Instance.ApplyTheme(form);
            
            // 查找子窗体中的DataGridView并应用样式
            foreach (Control control in form.Controls)
            {
                if (control is DataGridView dgv)
                {
                    ThemeManager.Instance.UpdateDataGridViewStyle(dgv);
                }
                else if (control is Panel panel)
                {
                    // 判断是否为工具栏Panel
                    bool isToolbar = panel.Name?.ToLower().Contains("toolbar") == true || 
                                    panel.Name?.ToLower().Contains("pnltool") == true;
                    ThemeManager.Instance.UpdatePanelStyle(panel, isToolbar);
                }
            }
        }
        
        /// <summary>
        /// 主题更改事件处理
        /// </summary>
        private void ThemeManager_OnThemeChanged(object sender, EventArgs e)
        {
            // 在主线程上执行UI更新
            if (InvokeRequired)
            {
                BeginInvoke(new Action(ApplyCurrentTheme));
            }
            else
            {
                ApplyCurrentTheme();
            }
        }

        private bool isDragging = false;
        private Point dragStartPoint;

        private void PnlHeader_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            dragStartPoint = new Point(e.X, e.Y);
        }

        private void PnlHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentPoint = PointToScreen(new Point(e.X, e.Y));
                Location = new Point(currentPoint.X - dragStartPoint.X, currentPoint.Y - dragStartPoint.Y);
            }
        }

        private void PnlHeader_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }
        
        private void btnSetting_Click(object sender, EventArgs e)
        {
            // 显示设置窗体
            var formInstance = Program.Services.GetRequiredService<SettingsForm>();
            formInstance.TopLevel = true;
            
            // 首先应用当前主题到设置窗体
            ThemeManager.Instance.ApplyTheme(formInstance);
            
            formInstance.ShowDialog();
        }
        
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            else
                this.WindowState = FormWindowState.Maximized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
   

        /// <summary>
        /// 重写窗体销毁方法，注销事件处理
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ThemeManager.Instance.OnThemeChanged -= ThemeManager_OnThemeChanged;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}