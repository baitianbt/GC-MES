using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GC_MES.WinForm.Forms
{
    public partial class LoginForm : Form
    {
        private readonly ILogger<LoginForm> _logger;
        private readonly IConfiguration _configuration;
        
        // 依赖注入
        public LoginForm(ILogger<LoginForm> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            InitializeComponent();
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
                MessageBox.Show("请输入用户名和密码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 这里实现实际的登录逻辑
            // 简单模拟登录成功
            if (username == "admin" && password == "admin")
            {
                _logger.LogInformation($"用户 {username} 登录成功");
                
                // 登录成功后，关闭当前窗体并打开主窗体
                this.Hide();
                using (var mainForm = Program.ServiceProvider.GetRequiredService<MainForm>())
                {
                    mainForm.ShowDialog();
                }
                this.Close();
            }
            else
            {
                _logger.LogWarning($"用户 {username} 登录失败");
                MessageBox.Show("用户名或密码错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 