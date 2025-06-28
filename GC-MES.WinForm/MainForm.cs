using GC_MES.BLL.System.IService;
using GC_MES.BLL.System.Service;
using GC_MES.WinForm.Common;
using GC_MES.WinForm.Controls;
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

        // 用于存储打开的窗体
        private Dictionary<string, Form> openForms = new Dictionary<string, Form>();

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


            var ParentMenus = AppInfo.Menus.FindAll(m => m.ParentId == 0);
            // 清空现有菜单项
            foreach (var ParentMenu in ParentMenus)
            {
                // 添加一级菜单项
                var parentItem = menu.AddItem(ParentMenu.MenuName);
                parentItem.Click += (s, e) => 
                { 
                    // 处理点击事件，打开对应的窗体或执行其他操作
                    if (!openForms.ContainsKey(ParentMenu.MenuName))
                    {
                        Form form = new Form(); // 替换为实际的窗体类
                        form.Text = ParentMenu.MenuName;
                        openForms[ParentMenu.MenuName] = form;
                        form.Show();
                    }
                };
                // 添加二级子菜单
                var childMenus = AppInfo.Menus.FindAll(m => m.ParentId == ParentMenu.Menu_Id);
                foreach (var childMenu in childMenus)
                {
                    var childItem = parentItem.AddSubItem(childMenu.MenuName);
                    childItem.Click += (s, e) => 
                    { 
                        // 处理点击事件，打开对应的窗体或执行其他操作
                        if (!openForms.ContainsKey(childMenu.MenuName))
                        {
                            Form form = new Form(); // 替换为实际的窗体类
                            form.Text = childMenu.MenuName;
                            openForms[childMenu.MenuName] = form;
                            form.Show();
                        }
                    };
                }
            }

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
            lblUserName.Text = "管理员";




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