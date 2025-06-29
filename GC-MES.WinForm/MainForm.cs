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

        }

        private void InitialMenu()
        {

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
                       labChildrenFormName.Text = ParentMenu.MenuName +" > "+ childMenu.MenuName;
                        formInstance.TopLevel = false;
                        formInstance.FormBorderStyle = FormBorderStyle.None;
                        formInstance.Dock = DockStyle.Fill;
                        pnlContent.Controls.Clear(); // 可选：只显示一个子窗体
                        pnlContent.Controls.Add(formInstance);
                        formInstance.Show();
                      

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
            throw new NotImplementedException();
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







    }
}