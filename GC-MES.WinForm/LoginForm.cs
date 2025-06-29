using GC_MES.BLL.System.IService;
using GC_MES.Core.Extensions;
using GC_MES.Model.System;
using GC_MES.WinForm.Common;
using GC_MES.WinForm.Forms.SystemForm;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GC_MES.WinForm.Forms
{
    public partial class LoginForm : Form
    {
        private ISys_UserService sys_UserService;
        private readonly ILogger<LoginForm> _logger;

        // 依赖注入
        public LoginForm(ISys_UserService sys_UserService, ILogger<LoginForm> logger)
        {
            this.sys_UserService = sys_UserService;
            this._logger = logger;

            InitializeComponent();
            _logger.LogInformation("LoginForm initialized.");
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // 设置窗体样式
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            // 添加拖动窗体的支持
            pnlHeader.MouseDown += PnlHeader_MouseDown;
            pnlHeader.MouseMove += PnlHeader_MouseMove;
            pnlHeader.MouseUp += PnlHeader_MouseUp;

            lblTitle.MouseDown += PnlHeader_MouseDown;
            lblTitle.MouseMove += PnlHeader_MouseMove;
            lblTitle.MouseUp += PnlHeader_MouseUp;
            
            // 应用主题
            ApplyTheme();
            
            // 注册主题变更事件
            ThemeManager.Instance.OnThemeChanged += ThemeManager_OnThemeChanged;
        }
        
        /// <summary>
        /// 应用主题到登录窗体
        /// </summary>
        private void ApplyTheme()
        {
            ThemeManager.Instance.ApplyTheme(this);
            
            // 美化登录按钮
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            //btnLogin.BackColor = ThemeManager.Instance.CurrentTheme.ButtonColor;
            //btnLogin.ForeColor = ThemeManager.Instance.CurrentTheme.ButtonTextColor;
            
            // 美化输入框
            //txtUsername.BackColor = ThemeManager.Instance.CurrentTheme.TextBoxBackColor;
            //txtUsername.ForeColor = ThemeManager.Instance.CurrentTheme.TextColor;
            //txtPassword.BackColor = ThemeManager.Instance.CurrentTheme.TextBoxBackColor;
            //txtPassword.ForeColor = ThemeManager.Instance.CurrentTheme.TextColor;
            
            // 更新标签颜色
            labTip.ForeColor = Color.Red; // 错误提示保持红色
        }
        
        /// <summary>
        /// 主题变更事件处理
        /// </summary>
        private void ThemeManager_OnThemeChanged(object sender, EventArgs e)
        {
            // 在主线程上执行UI更新
            if (InvokeRequired)
            {
                BeginInvoke(new Action(ApplyTheme));
            }
            else
            {
                ApplyTheme();
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

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                labTip.Text = "用户名或密码不能为空！";
                return;
            }

            var user = sys_UserService.QueryListByClause(p => p.UserName.Equals(username)).FirstOrDefault();

            if (user == null)
            {
                labTip.Text = "用户不存在！";
                return;
            }

            // 检查密码是否正确, 这里密钥要从配置文件或安全存储中获取
            if (!password.EncryptDES("C5ABA9E202D94C43A3CA66002BF77FAF").Equals(user.UserPwd))
            {
                labTip.Text = "密码错误！";
                return;
            }

            AppInfo.CurrentUser = user; // 设置当前用户信息

            // 登录成功后，关闭当前窗体并打开主窗体
            this.Hide();
            using (var mainForm = Program.Services.GetRequiredService<MainForm>())
            {
                mainForm.ShowDialog();
            }
            this.Close();
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