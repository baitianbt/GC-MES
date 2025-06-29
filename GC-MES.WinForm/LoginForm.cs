using GC_MES.BLL.System.IService;
using GC_MES.Core.Extensions;
using GC_MES.Model.System;
using GC_MES.WinForm.Common;
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
    }
}