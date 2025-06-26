using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;
using System.Data;

namespace GC_MES.WinForm.Forms
{
    public partial class MainForm : Form
    {
        private readonly ILogger<MainForm> _logger;
        private readonly IConfiguration _configuration;
        
        // 当前选中的菜单按钮
        private Button currentButton;
        // 用于存储打开的窗体
        private Dictionary<string, Form> openForms = new Dictionary<string, Form>();
        
        // 子菜单面板字典
        private Dictionary<Button, Panel> subMenuPanels = new Dictionary<Button, Panel>();
        // 当前展开的子菜单面板
        private Panel currentSubMenuPanel = null;
        
        // 依赖注入
        public MainForm(ILogger<MainForm> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            InitializeComponent();
            
            // 初始化图标
            InitializeMenuIcons();
            
            // 初始化子菜单
            InitializeSubMenus();
            
            // 设置默认活动按钮
            ActivateButton(btnDashboard);
            
            // 订阅主题变更事件
            ThemeManager.ThemeChanged += ThemeManager_ThemeChanged;
        }
        
        // 主题变更处理
        private void ThemeManager_ThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            // 应用新主题到当前窗体
            ThemeManager.ApplyTheme(this);
            
            // 同时应用到所有打开的子窗体
            foreach (var form in openForms.Values)
            {
                if (form != null && !form.IsDisposed)
                    ThemeManager.ApplyTheme(form);
            }
            
            // 应用到子菜单
            foreach (var panel in subMenuPanels.Values)
            {
                ApplyThemeToSubMenu(panel);
            }
            
            _logger.LogInformation($"主题已切换为: {e.ThemeType}");
        }
        
        /// <summary>
        /// 应用主题到子菜单
        /// </summary>
        private void ApplyThemeToSubMenu(Panel subMenuPanel)
        {
            if (subMenuPanel == null) return;
            
            Color btnBackColor = Color.FromArgb(42, 57, 76); // 深一点的背景色
            Color btnForeColor = Color.Gainsboro;
            
            switch (ThemeManager.CurrentTheme)
            {
                case ThemeType.Dark:
                    btnBackColor = Color.FromArgb(42, 57, 76);
                    btnForeColor = Color.Gainsboro;
                    break;
                case ThemeType.Light:
                    btnBackColor = Color.FromArgb(230, 230, 230);
                    btnForeColor = Color.Black;
                    break;
                case ThemeType.Blue:
                    btnBackColor = Color.FromArgb(0, 102, 184);
                    btnForeColor = Color.White;
                    break;
                case ThemeType.Green:
                    btnBackColor = Color.FromArgb(20, 120, 60);
                    btnForeColor = Color.White;
                    break;
            }
            
            subMenuPanel.BackColor = btnBackColor;
            
            foreach (Control control in subMenuPanel.Controls)
            {
                if (control is Button btn)
                {
                    btn.BackColor = btnBackColor;
                    btn.ForeColor = btnForeColor;
                    btn.FlatAppearance.BorderColor = btnBackColor;
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(btnBackColor.R + 20, btnBackColor.G + 20, btnBackColor.B + 20);
                }
            }
        }
        
        /// <summary>
        /// 初始化子菜单
        /// </summary>
        private void InitializeSubMenus()
        {
            // 质量模块子菜单
            CreateSubMenu(btnQuality, new string[] { 
                "质量检验管理", "质量标准管理", "不合格品管理", "质量统计报表" 
            });
            
            // 生产模块子菜单
            CreateSubMenu(btnProduction, new string[] { 
                "生产任务", "生产进度", "工序操作", "生产报工" 
            });
            
            // 计划模块子菜单
            CreateSubMenu(btnPlanning, new string[] { 
                "生产计划", "物料需求计划", "排程管理", "能力分析" 
            });
            
            // 仓储模块子菜单
            CreateSubMenu(btnInventory, new string[] { 
                "库存管理", "入库管理", "出库管理", "库位管理", "盘点管理" 
            });
            
            // 系统设置子菜单
            CreateSubMenu(btnSettings, new string[] { 
                "基础参数", "系统配置", "主题设置", "数据备份", "系统日志" 
            });
            
            // 用户管理子菜单
            CreateSubMenu(btnUserManage, new string[] { 
                "用户管理", "角色管理", "权限管理", "部门管理" 
            });
        }
        
        /// <summary>
        /// 创建子菜单面板
        /// </summary>
        private void CreateSubMenu(Button parentButton, string[] menuItems)
        {
            if (parentButton == null || menuItems == null || menuItems.Length == 0)
                return;
            
            // 创建子菜单面板
            Panel subMenuPanel = new Panel();
            subMenuPanel.Dock = DockStyle.None;
            subMenuPanel.AutoSize = true;
            subMenuPanel.BackColor = Color.FromArgb(42, 57, 76); // 比主菜单深一点的颜色
            subMenuPanel.Visible = false;
            
            // 设置子菜单面板的位置
            subMenuPanel.Location = new Point(parentButton.Right, parentButton.Top);
            
            // 创建子菜单项
            int buttonHeight = 40;
            int panelWidth = 180;
            
            for (int i = 0; i < menuItems.Length; i++)
            {
                Button subButton = new Button();
                subButton.Text = menuItems[i];
                subButton.Size = new Size(panelWidth, buttonHeight);
                subButton.Location = new Point(0, i * buttonHeight);
                subButton.FlatStyle = FlatStyle.Flat;
                subButton.FlatAppearance.BorderSize = 0;
                subButton.BackColor = Color.FromArgb(42, 57, 76);
                subButton.ForeColor = Color.Gainsboro;
                subButton.TextAlign = ContentAlignment.MiddleLeft;
                subButton.Padding = new Padding(20, 0, 0, 0);
                subButton.Tag = parentButton.Text + "." + menuItems[i]; // 存储完整的菜单路径
                subButton.Click += SubMenuItem_Click;
                
                subMenuPanel.Controls.Add(subButton);
            }
            
            // 调整面板大小
            subMenuPanel.Size = new Size(panelWidth, buttonHeight * menuItems.Length);
            
            // 添加到父容器
            this.Controls.Add(subMenuPanel);
            subMenuPanel.BringToFront();
            
            // 存储子菜单面板引用
            subMenuPanels[parentButton] = subMenuPanel;
            
            // 修改父按钮点击事件
            parentButton.Click -= GetEventHandler(parentButton);
            parentButton.Click += ParentMenuItem_Click;
        }
        
        /// <summary>
        /// 获取按钮原有的点击事件处理器
        /// </summary>
        private EventHandler GetEventHandler(Button button)
        {
            if (button == btnDashboard) return btnDashboard_Click;
            else if (button == btnWorkOrders) return btnWorkOrders_Click;
            else if (button == btnInventory) return btnInventory_Click;
            else if (button == btnPlanning) return btnPlanning_Click;
            else if (button == btnProduction) return btnProduction_Click;
            else if (button == btnQuality) return btnQuality_Click;
            else if (button == btnMaintenance) return btnMaintenance_Click;
            else if (button == btnReports) return btnReports_Click;
            else if (button == btnSettings) return btnSettings_Click;
            else if (button == btnUserManage) return btnUserManage_Click;
            else if (button == btnLogout) return btnLogout_Click;
            else return null;
        }
        
        /// <summary>
        /// 父菜单项点击事件处理
        /// </summary>
        private void ParentMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is Button parentButton && subMenuPanels.ContainsKey(parentButton))
            {
                // 激活当前按钮
                ActivateButton(parentButton);
                
                Panel subMenuPanel = subMenuPanels[parentButton];
                
                // 隐藏其他子菜单
                foreach (var panel in subMenuPanels.Values)
                {
                    if (panel != subMenuPanel)
                        panel.Visible = false;
                }
                
                // 切换当前子菜单的可见性
                subMenuPanel.Visible = !subMenuPanel.Visible;
                
                // 更新当前可见子菜单的引用
                currentSubMenuPanel = subMenuPanel.Visible ? subMenuPanel : null;
                
                // 相应的打开主窗体内容
                string formName = parentButton.Text.Replace(" ", "");
                if (!subMenuPanel.Visible) // 如果关闭子菜单，打开对应的主表单
                {
                    OpenDefaultParentForm(formName);
                }
            }
        }
        
        /// <summary>
        /// 打开父菜单对应的默认窗体
        /// </summary>
        private void OpenDefaultParentForm(string formName)
        {
            switch (formName)
            {
                case "质量":
                    OpenQualityForm();
                    break;
                case "生产":
                    OpenProductionForm();
                    break;
                case "计划":
                    OpenPlanningForm();
                    break;
                case "仓储":
                    OpenInventoryForm();
                    break;
                case "设置":
                    OpenSettingsForm();
                    break;
                case "用户":
                    OpenUserManagementForm();
                    break;
                default:
                    OpenChildForm(formName, null);
                    break;
            }
        }
        
        /// <summary>
        /// 子菜单项点击事件处理
        /// </summary>
        private void SubMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is Button subButton && subButton.Tag is string menuPath)
            {
                // 根据子菜单的Tag打开相应窗体
                string[] pathParts = menuPath.ToString().Split('.');
                if (pathParts.Length >= 2)
                {
                    string parentMenu = pathParts[0];
                    string subMenu = pathParts[1];
                    
                    // 隐藏所有子菜单
                    foreach (var panel in subMenuPanels.Values)
                        panel.Visible = false;
                    
                    currentSubMenuPanel = null;
                    
                    // 打开相应的窗体
                    OpenFormByMenuPath(parentMenu, subMenu);
                }
            }
        }
        
        /// <summary>
        /// 根据菜单路径打开相应窗体
        /// </summary>
        private void OpenFormByMenuPath(string parentMenu, string subMenu)
        {
            // 激活父菜单按钮
            Button parentButton = GetButtonByText(parentMenu);
            if (parentButton != null)
                ActivateButton(parentButton);
            
            // 根据父菜单和子菜单组合打开相应窗体
            switch ($"{parentMenu}.{subMenu}")
            {
                case "质量.质量检验管理":
                    OpenQualityInspectionManagement();
                    break;
                case "质量.质量标准管理":
                    OpenQualityStandardManagement();
                    break;
                case "质量.不合格品管理":
                    OpenNonconformingProductManagement();
                    break;
                case "质量.质量统计报表":
                    OpenQualityReports();
                    break;
                case "用户.用户管理":
                    OpenUserManagement();
                    break;
                case "用户.角色管理":
                    OpenRoleManagement();
                    break;
                case "用户.权限管理":
                    OpenPermissionManagement();
                    break;
                // 其他菜单项处理...
                default:
                    MessageBox.Show($"功能{subMenu}开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }
        
        /// <summary>
        /// 根据文本获取对应按钮
        /// </summary>
        private Button GetButtonByText(string text)
        {
            foreach (Control control in pnlMenu.Controls)
            {
                if (control is Button button && button.Text.Trim() == text.Trim())
                    return button;
            }
            
            return null;
        }
        
        // 质量模块相关窗体打开方法
        private void OpenQualityForm()
        {
            Panel qualityPanel = new Panel();
            qualityPanel.Dock = DockStyle.Fill;
            qualityPanel.BackColor = Color.White;
            
            // 添加标题
            Label lblTitle = new Label();
            lblTitle.Text = "质量管理";
            lblTitle.Font = new Font("Microsoft YaHei UI", 14, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.AutoSize = true;
            qualityPanel.Controls.Add(lblTitle);
            
            // 添加质量模块说明
            Label lblDescription = new Label();
            lblDescription.Text = "质量管理模块提供对产品质量全生命周期的管理，包括质量检验标准制定、质量检验管理、不合格品处理和质量统计分析等功能。";
            lblDescription.Font = new Font("Microsoft YaHei UI", 10);
            lblDescription.Location = new Point(20, 60);
            lblDescription.Size = new Size(800, 40);
            qualityPanel.Controls.Add(lblDescription);
            
            // 添加功能按钮
            FlowLayoutPanel flowButtons = new FlowLayoutPanel();
            flowButtons.Location = new Point(20, 120);
            flowButtons.Size = new Size(800, 200);
            flowButtons.FlowDirection = FlowDirection.LeftToRight;
            flowButtons.WrapContents = true;
            flowButtons.AutoSize = true;
            qualityPanel.Controls.Add(flowButtons);
            
            // 创建质量模块主要功能按钮
            CreateQualityButton(flowButtons, "质量检验管理", "对生产过程中的来料检验、过程检验和成品检验进行管理", OpenQualityInspectionManagement);
            CreateQualityButton(flowButtons, "质量标准管理", "制定和管理产品的质量检验标准和检验项目", OpenQualityStandardManagement);
            CreateQualityButton(flowButtons, "不合格品管理", "对检验不合格的产品进行处理和跟踪", OpenNonconformingProductManagement);
            CreateQualityButton(flowButtons, "质量统计报表", "提供质量相关的统计分析和报表功能", OpenQualityReports);
            
            // 打开质量模块面板
            OpenChildForm("Quality", qualityPanel);
        }
        
        private void CreateQualityButton(FlowLayoutPanel container, string text, string description, EventHandler clickHandler)
        {
            Panel btnPanel = new Panel();
            btnPanel.Size = new Size(380, 120);
            btnPanel.Margin = new Padding(10);
            btnPanel.BackColor = Color.FromArgb(240, 240, 240);
            btnPanel.BorderStyle = BorderStyle.FixedSingle;
            container.Controls.Add(btnPanel);
            
            Label lblTitle = new Label();
            lblTitle.Text = text;
            lblTitle.Font = new Font("Microsoft YaHei UI", 12, FontStyle.Bold);
            lblTitle.Location = new Point(10, 10);
            lblTitle.AutoSize = true;
            btnPanel.Controls.Add(lblTitle);
            
            Label lblDesc = new Label();
            lblDesc.Text = description;
            lblDesc.Font = new Font("Microsoft YaHei UI", 9);
            lblDesc.Location = new Point(10, 40);
            lblDesc.Size = new Size(360, 40);
            btnPanel.Controls.Add(lblDesc);
            
            Button btnOpen = new Button();
            btnOpen.Text = "打开";
            btnOpen.Size = new Size(80, 30);
            btnOpen.Location = new Point(btnPanel.Width - 90, btnPanel.Height - 40);
            btnOpen.FlatStyle = FlatStyle.Flat;
            btnOpen.BackColor = Color.FromArgb(0, 122, 204);
            btnOpen.ForeColor = Color.White;
            btnOpen.Click += clickHandler;
            btnPanel.Controls.Add(btnOpen);
        }
        
        private void OpenQualityInspectionManagement()
        {
            try
            {
                var inspectionForm = Program.ServiceProvider.GetRequiredService<QualityInspectionManagementForm>();
                inspectionForm.ShowDialog();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "打开质量检验管理窗体时出错");
                MessageBox.Show($"打开质量检验管理窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void OpenQualityStandardManagement()
        {
            try
            {
                var standardForm = Program.ServiceProvider.GetRequiredService<QualityStandardManagementForm>();
                standardForm.ShowDialog();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "打开质量标准管理窗体时出错");
                MessageBox.Show($"打开质量标准管理窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void OpenNonconformingProductManagement()
        {
            try
            {
                var nonconformingForm = Program.ServiceProvider.GetRequiredService<NonconformingProductForm>();
                nonconformingForm.ShowDialog();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "打开不合格品管理窗体时出错");
                MessageBox.Show($"打开不合格品管理窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void OpenQualityReports()
        {
            MessageBox.Show("质量统计报表功能开发中，敬请期待！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        // 用户管理相关窗体打开方法
        private void OpenUserManagementForm()
        {
            OpenChildForm("UserManagement", null);
        }
        
        private void OpenUserManagement()
        {
            try
            {
                var userForm = Program.ServiceProvider.GetRequiredService<UserManagementForm>();
                OpenChildForm("UserManagement", userForm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "打开用户管理窗体时出错");
                MessageBox.Show($"打开用户管理窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void OpenRoleManagement()
        {
            try
            {
                var roleForm = Program.ServiceProvider.GetRequiredService<RoleManagementForm>();
                OpenChildForm("RoleManagement", roleForm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "打开角色管理窗体时出错");
                MessageBox.Show($"打开角色管理窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void OpenPermissionManagement()
        {
            try
            {
                var permissionForm = Program.ServiceProvider.GetRequiredService<PermissionManagementForm>();
                OpenChildForm("PermissionManagement", permissionForm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "打开权限管理窗体时出错");
                MessageBox.Show($"打开权限管理窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        // 生产管理相关
        private void OpenProductionForm()
        {
            MessageBox.Show("生产管理模块主界面开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        // 计划管理相关
        private void OpenPlanningForm()
        {
            MessageBox.Show("计划管理模块主界面开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        // 库存管理相关
        private void OpenInventoryForm()
        {
            MessageBox.Show("库存管理模块主界面开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        // 设置模块相关
        private void OpenSettingsForm()
        {
            // 打开设置窗体并显示主题设置
            Form settingsForm = CreateOrGetSettingsForm();
            OpenChildForm("Settings", settingsForm);
        }
        
        /// <summary>
        /// 初始化菜单图标
        /// </summary>
        private void InitializeMenuIcons()
        {
            try
            {
                // 设置图标尺寸
                Size iconSize = new Size(24, 24);

                // 使用FontAwesome或其他图标库字体图标
                // 通过嵌入资源或文件加载图标
                string resourcePath = Path.Combine(Application.StartupPath, "Resources", "Icons");

                // 这里使用内置的系统图标作为示例，实际项目中应替换为适合MES系统的专业图标
                btnDashboard.Image = ResizeImage(SystemIcons.Application.ToBitmap(), iconSize);
                btnDashboard.ImageAlign = ContentAlignment.MiddleLeft;
                btnDashboard.TextAlign = ContentAlignment.MiddleLeft;
                btnDashboard.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnDashboard.Padding = new Padding(10, 0, 0, 0);
                
                btnWorkOrders.Image = ResizeImage(SystemIcons.Information.ToBitmap(), iconSize);
                btnWorkOrders.ImageAlign = ContentAlignment.MiddleLeft;
                btnWorkOrders.TextAlign = ContentAlignment.MiddleLeft;
                btnWorkOrders.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnWorkOrders.Padding = new Padding(10, 0, 0, 0);
                
                btnInventory.Image = ResizeImage(SystemIcons.Shield.ToBitmap(), iconSize);
                btnInventory.ImageAlign = ContentAlignment.MiddleLeft;
                btnInventory.TextAlign = ContentAlignment.MiddleLeft;
                btnInventory.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnInventory.Padding = new Padding(10, 0, 0, 0);
                
                btnPlanning.Image = ResizeImage(SystemIcons.Question.ToBitmap(), iconSize);
                btnPlanning.ImageAlign = ContentAlignment.MiddleLeft;
                btnPlanning.TextAlign = ContentAlignment.MiddleLeft;
                btnPlanning.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnPlanning.Padding = new Padding(10, 0, 0, 0);
                
                btnProduction.Image = ResizeImage(SystemIcons.Warning.ToBitmap(), iconSize);
                btnProduction.ImageAlign = ContentAlignment.MiddleLeft;
                btnProduction.TextAlign = ContentAlignment.MiddleLeft;
                btnProduction.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnProduction.Padding = new Padding(10, 0, 0, 0);
                
                btnQuality.Image = ResizeImage(SystemIcons.Error.ToBitmap(), iconSize);
                btnQuality.ImageAlign = ContentAlignment.MiddleLeft;
                btnQuality.TextAlign = ContentAlignment.MiddleLeft;
                btnQuality.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnQuality.Padding = new Padding(10, 0, 0, 0);
                
                btnMaintenance.Image = ResizeImage(SystemIcons.WinLogo.ToBitmap(), iconSize);
                btnMaintenance.ImageAlign = ContentAlignment.MiddleLeft;
                btnMaintenance.TextAlign = ContentAlignment.MiddleLeft;
                btnMaintenance.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnMaintenance.Padding = new Padding(10, 0, 0, 0);
                
                btnReports.Image = ResizeImage(SystemIcons.Asterisk.ToBitmap(), iconSize);
                btnReports.ImageAlign = ContentAlignment.MiddleLeft;
                btnReports.TextAlign = ContentAlignment.MiddleLeft;
                btnReports.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnReports.Padding = new Padding(10, 0, 0, 0);
                
                btnSettings.Image = ResizeImage(SystemIcons.Exclamation.ToBitmap(), iconSize);
                btnSettings.ImageAlign = ContentAlignment.MiddleLeft;
                btnSettings.TextAlign = ContentAlignment.MiddleLeft;
                btnSettings.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnSettings.Padding = new Padding(10, 0, 0, 0);
                
                btnUserManage.Image = ResizeImage(SystemIcons.UserProfile.ToBitmap(), iconSize);
                btnUserManage.ImageAlign = ContentAlignment.MiddleLeft;
                btnUserManage.TextAlign = ContentAlignment.MiddleLeft;
                btnUserManage.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnUserManage.Padding = new Padding(10, 0, 0, 0);
                
                btnLogout.Image = ResizeImage(SystemIcons.Hand.ToBitmap(), iconSize);
                btnLogout.ImageAlign = ContentAlignment.MiddleLeft;
                btnLogout.TextAlign = ContentAlignment.MiddleLeft;
                btnLogout.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnLogout.Padding = new Padding(10, 0, 0, 0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化菜单图标时出错");
            }
        }
        
        /// <summary>
        /// 调整图片大小
        /// </summary>
        private Bitmap ResizeImage(Image image, Size size)
        {
            Bitmap result = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, size.Width, size.Height);
            }
            return result;
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            // 设置窗体样式
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            
            // 添加拖动窗体的支持
            pnlHeader.MouseDown += PnlHeader_MouseDown;
            pnlHeader.MouseMove += PnlHeader_MouseMove;
            pnlHeader.MouseUp += PnlHeader_MouseUp;
            
            lblTitle.MouseDown += PnlHeader_MouseDown;
            lblTitle.MouseMove += PnlHeader_MouseMove;
            lblTitle.MouseUp += PnlHeader_MouseUp;
            
            // 显示用户信息
            lblUserName.Text = "管理员";
            
            // 默认打开仪表板
            OpenChildForm("Dashboard", null);
            
            // 应用当前主题
            ThemeManager.ApplyTheme(this);
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
        
        // 激活选中的菜单按钮
        private void ActivateButton(object btnSender)
        {
            if (btnSender != null)
            {
                if (currentButton != (Button)btnSender)
                {
                    // 重置之前的按钮样式
                    DisableButton();
                    
                    currentButton = (Button)btnSender;
                    currentButton.BackColor = Color.FromArgb(45, 63, 88);
                    currentButton.ForeColor = Color.White;
                    currentButton.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
                    
                    // 更新标题
                    lblTitle.Text = "GC-MES 系统 - " + currentButton.Text;
                }
            }
        }
        
        // 重置按钮样式
        private void DisableButton()
        {
            foreach (Control previousBtn in pnlMenu.Controls)
            {
                if (previousBtn.GetType() == typeof(Button))
                {
                    previousBtn.BackColor = Color.FromArgb(32, 47, 66);
                    previousBtn.ForeColor = Color.Gainsboro;
                    previousBtn.Font = new Font("Microsoft YaHei UI", 10F);
                }
            }
        }
        
        // 打开子窗体
        private void OpenChildForm(string formName, object formParams)
        {
            // 清除当前内容面板中的所有控件
            pnlContent.Controls.Clear();
            
            // 如果已经存在该名称的窗体，先从字典中移除
            if (openForms.ContainsKey(formName))
                openForms.Remove(formName);
            
            Control childControl = null;
            
            // 检查是否传入了已经创建的控件
            if (formParams is Control existingControl)
            {
                childControl = existingControl;
            }
            else
            {
                // 根据formName创建对应的窗体
                switch (formName)
                {
                    case "Dashboard":
                        // 这里创建仪表盘窗体
                        Panel dashboardPanel = new Panel();
                        dashboardPanel.Dock = DockStyle.Fill;
                        dashboardPanel.BackColor = Color.White;
                        
                        // 添加仪表盘内容
                        Label lblDashboard = new Label();
                        lblDashboard.Text = "仪表盘";
                        lblDashboard.Font = new Font("Microsoft YaHei UI", 14, FontStyle.Bold);
                        lblDashboard.Location = new Point(20, 20);
                        lblDashboard.AutoSize = true;
                        dashboardPanel.Controls.Add(lblDashboard);
                        
                        childControl = dashboardPanel;
                        break;
                        
                    case "UserManagement":
                        // 创建用户管理界面
                        childControl = CreateUserManagementForm();
                        break;
                        
                    case "WorkOrderManagement":
                        // 创建工单管理界面
                        childControl = Program.ServiceProvider.GetService<WorkOrderManagementForm>();
                        break;
                        
                    default:
                        // 默认创建一个空白面板
                        Panel defaultPanel = new Panel();
                        defaultPanel.Dock = DockStyle.Fill;
                        defaultPanel.BackColor = Color.White;
                        
                        Label lblDefault = new Label();
                        lblDefault.Text = $"{formName}";
                        lblDefault.Font = new Font("Microsoft YaHei UI", 14, FontStyle.Bold);
                        lblDefault.Location = new Point(20, 20);
                        lblDefault.AutoSize = true;
                        defaultPanel.Controls.Add(lblDefault);
                        
                        childControl = defaultPanel;
                        break;
                }
            }
            
            if (childControl != null)
            {
                // 设置控件的通用属性
                childControl.Dock = DockStyle.Fill;
                
                // 添加到内容面板
                pnlContent.Controls.Add(childControl);
                
                // 如果是窗体，设置相关属性
                if (childControl is Form childForm)
                {
                    childForm.TopLevel = false;
                    childForm.FormBorderStyle = FormBorderStyle.None;
                    childForm.Show();
                }
                
                // 添加到字典
                if (childControl is Form form)
                    openForms[formName] = form;
                else
                {
                    // 创建一个包装Form以在字典中存储
                    Form wrapperForm = new Form();
                    wrapperForm.Controls.Add(childControl);
                    openForms[formName] = wrapperForm;
                }
                
                // 应用当前主题
                ThemeManager.ApplyTheme(childControl);
                
                // 将控件带到前面
                childControl.BringToFront();
            }
        }
        
        // 菜单按钮点击事件
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildForm("Dashboard", null);
        }
        
        private void btnWorkOrders_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildForm("WorkOrderManagement", null);
        }
        
        private void btnInventory_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildForm("Inventory", null);
        }
        
        private void btnPlanning_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildForm("Planning", null);
        }
        
        private void btnProduction_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildForm("Production", null);
        }
        
        private void btnQuality_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            
            // 创建质量模块的面板
            Panel qualityPanel = new Panel();
            qualityPanel.Dock = DockStyle.Fill;
            qualityPanel.BackColor = Color.White;
            
            // 添加标题
            Label lblTitle = new Label();
            lblTitle.Text = "质量管理";
            lblTitle.Font = new Font("Microsoft YaHei UI", 14, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.AutoSize = true;
            qualityPanel.Controls.Add(lblTitle);
            
            // 添加功能按钮
            FlowLayoutPanel flowButtons = new FlowLayoutPanel();
            flowButtons.Location = new Point(20, 60);
            flowButtons.Size = new Size(800, 120);
            flowButtons.FlowDirection = FlowDirection.LeftToRight;
            flowButtons.WrapContents = true;
            flowButtons.AutoSize = true;
            qualityPanel.Controls.Add(flowButtons);
            
            // 质量检验管理按钮
            Button btnInspection = new Button();
            btnInspection.Text = "质量检验管理";
            btnInspection.Size = new Size(180, 40);
            btnInspection.FlatStyle = FlatStyle.Flat;
            btnInspection.BackColor = Color.FromArgb(0, 122, 204);
            btnInspection.ForeColor = Color.White;
            btnInspection.Font = new Font("Microsoft YaHei UI", 10);
            btnInspection.Click += (s, args) => {
                try
                {
                    var serviceProvider = Program.ServiceProvider;
                    var inspectionForm = serviceProvider.GetRequiredService<QualityInspectionManagementForm>();
                    inspectionForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "打开质量检验管理窗体时出错");
                    MessageBox.Show($"打开质量检验管理窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            flowButtons.Controls.Add(btnInspection);
            
            // 质量标准管理按钮
            Button btnStandard = new Button();
            btnStandard.Text = "质量标准管理";
            btnStandard.Size = new Size(180, 40);
            btnStandard.FlatStyle = FlatStyle.Flat;
            btnStandard.BackColor = Color.FromArgb(0, 122, 204);
            btnStandard.ForeColor = Color.White;
            btnStandard.Font = new Font("Microsoft YaHei UI", 10);
            btnStandard.Click += (s, args) => {
                try
                {
                    var serviceProvider = Program.ServiceProvider;
                    var standardForm = serviceProvider.GetRequiredService<QualityStandardManagementForm>();
                    standardForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "打开质量标准管理窗体时出错");
                    MessageBox.Show($"打开质量标准管理窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            flowButtons.Controls.Add(btnStandard);
            
            // 不合格品管理按钮
            Button btnNonconforming = new Button();
            btnNonconforming.Text = "不合格品管理";
            btnNonconforming.Size = new Size(180, 40);
            btnNonconforming.FlatStyle = FlatStyle.Flat;
            btnNonconforming.BackColor = Color.FromArgb(0, 122, 204);
            btnNonconforming.ForeColor = Color.White;
            btnNonconforming.Font = new Font("Microsoft YaHei UI", 10);
            btnNonconforming.Click += (s, args) => {
                try
                {
                    var serviceProvider = Program.ServiceProvider;
                    var nonconformingForm = serviceProvider.GetRequiredService<NonconformingProductForm>();
                    nonconformingForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "打开不合格品管理窗体时出错");
                    MessageBox.Show($"打开不合格品管理窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            flowButtons.Controls.Add(btnNonconforming);
            
            // 质量报表按钮
            Button btnQualityReport = new Button();
            btnQualityReport.Text = "质量统计报表";
            btnQualityReport.Size = new Size(180, 40);
            btnQualityReport.FlatStyle = FlatStyle.Flat;
            btnQualityReport.BackColor = Color.FromArgb(0, 122, 204);
            btnQualityReport.ForeColor = Color.White;
            btnQualityReport.Font = new Font("Microsoft YaHei UI", 10);
            btnQualityReport.Click += (s, args) => {
                MessageBox.Show("质量统计报表功能开发中，敬请期待！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            flowButtons.Controls.Add(btnQualityReport);
            
            // 应用主题
            ThemeManager.ApplyTheme(qualityPanel);
            
            // 打开质量模块面板
            OpenChildForm("Quality", qualityPanel);
        }
        
        private void btnMaintenance_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildForm("Maintenance", null);
        }
        
        private void btnReports_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildForm("Reports", null);
        }
        
        private void btnSettings_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            
            // 打开设置窗体并显示主题设置
            Form settingsForm = CreateOrGetSettingsForm();
            OpenChildForm("Settings", settingsForm);
        }
        
        /// <summary>
        /// 创建或获取设置窗体
        /// </summary>
        private Form CreateOrGetSettingsForm()
        {
            if (openForms.ContainsKey("Settings") && openForms["Settings"] != null && !openForms["Settings"].IsDisposed)
                return openForms["Settings"];
            
            // 创建设置窗体
            Panel settingsForm = new Panel();
            settingsForm.Dock = DockStyle.Fill;
            settingsForm.BackColor = Color.White;
            
            // 添加标题
            Label lblSettings = new Label();
            lblSettings.Text = "系统设置";
            lblSettings.Font = new Font("Microsoft YaHei UI", 14, FontStyle.Bold);
            lblSettings.Location = new Point(20, 20);
            lblSettings.AutoSize = true;
            settingsForm.Controls.Add(lblSettings);
            
            // 添加主题设置组
            GroupBox grpTheme = new GroupBox();
            grpTheme.Text = "主题设置";
            grpTheme.Font = new Font("Microsoft YaHei UI", 10);
            grpTheme.Location = new Point(20, 70);
            grpTheme.Size = new Size(400, 180);
            settingsForm.Controls.Add(grpTheme);
            
            // 添加主题选择的单选按钮
            RadioButton rbDark = new RadioButton();
            rbDark.Text = "暗色主题";
            rbDark.Location = new Point(20, 30);
            rbDark.Tag = ThemeType.Dark;
            rbDark.Checked = ThemeManager.CurrentTheme == ThemeType.Dark;
            rbDark.CheckedChanged += ThemeRadioButton_CheckedChanged;
            grpTheme.Controls.Add(rbDark);
            
            RadioButton rbLight = new RadioButton();
            rbLight.Text = "亮色主题";
            rbLight.Location = new Point(20, 60);
            rbLight.Tag = ThemeType.Light;
            rbLight.Checked = ThemeManager.CurrentTheme == ThemeType.Light;
            rbLight.CheckedChanged += ThemeRadioButton_CheckedChanged;
            grpTheme.Controls.Add(rbLight);
            
            RadioButton rbBlue = new RadioButton();
            rbBlue.Text = "蓝色主题";
            rbBlue.Location = new Point(20, 90);
            rbBlue.Tag = ThemeType.Blue;
            rbBlue.Checked = ThemeManager.CurrentTheme == ThemeType.Blue;
            rbBlue.CheckedChanged += ThemeRadioButton_CheckedChanged;
            grpTheme.Controls.Add(rbBlue);
            
            RadioButton rbGreen = new RadioButton();
            rbGreen.Text = "绿色主题";
            rbGreen.Location = new Point(20, 120);
            rbGreen.Tag = ThemeType.Green;
            rbGreen.Checked = ThemeManager.CurrentTheme == ThemeType.Green;
            rbGreen.CheckedChanged += ThemeRadioButton_CheckedChanged;
            grpTheme.Controls.Add(rbGreen);
            
            // 应用主题
            ThemeManager.ApplyTheme(settingsForm);
            
            return settingsForm;
        }
        
        /// <summary>
        /// 主题单选按钮变更事件
        /// </summary>
        private void ThemeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null && rb.Checked && rb.Tag is ThemeType themeType)
            {
                // 设置新主题
                ThemeManager.CurrentTheme = themeType;
            }
        }
        
        private void btnLogout_Click(object sender, EventArgs e)
        {
            _logger.LogInformation("用户登出系统");
            
            // 关闭主窗体并打开登录窗体
            this.Hide();
            using (var loginForm = Program.ServiceProvider.GetRequiredService<LoginForm>())
            {
                loginForm.ShowDialog();
            }
            this.Close();
        }
        
        /// <summary>
        /// 创建用户管理界面
        /// </summary>
        private Control CreateUserManagementForm()
        {
            Panel userPanel = new Panel();
            userPanel.Dock = DockStyle.Fill;
            userPanel.BackColor = Color.White;
            
            // 添加标题
            Label lblTitle = new Label();
            lblTitle.Text = "用户管理";
            lblTitle.Font = new Font("Microsoft YaHei UI", 14, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.AutoSize = true;
            userPanel.Controls.Add(lblTitle);
            
            // 添加功能按钮区域
            Panel pnlButtons = new Panel();
            pnlButtons.Location = new Point(20, 60);
            pnlButtons.Size = new Size(800, 40);
            userPanel.Controls.Add(pnlButtons);
            
            // 添加新用户按钮
            Button btnAddUser = new Button();
            btnAddUser.Text = "添加用户";
            btnAddUser.Location = new Point(0, 0);
            btnAddUser.Size = new Size(100, 30);
            btnAddUser.FlatStyle = FlatStyle.Flat;
            btnAddUser.Click += BtnAddUser_Click;
            pnlButtons.Controls.Add(btnAddUser);
            
            // 编辑用户按钮
            Button btnEditUser = new Button();
            btnEditUser.Text = "编辑用户";
            btnEditUser.Location = new Point(110, 0);
            btnEditUser.Size = new Size(100, 30);
            btnEditUser.FlatStyle = FlatStyle.Flat;
            btnEditUser.Click += BtnEditUser_Click;
            pnlButtons.Controls.Add(btnEditUser);
            
            // 删除用户按钮
            Button btnDeleteUser = new Button();
            btnDeleteUser.Text = "删除用户";
            btnDeleteUser.Location = new Point(220, 0);
            btnDeleteUser.Size = new Size(100, 30);
            btnDeleteUser.FlatStyle = FlatStyle.Flat;
            btnDeleteUser.Click += BtnDeleteUser_Click;
            pnlButtons.Controls.Add(btnDeleteUser);
            
            // 搜索框
            TextBox txtSearch = new TextBox();
            txtSearch.Location = new Point(500, 0);
            txtSearch.Size = new Size(200, 30);
           
            pnlButtons.Controls.Add(txtSearch);
            
            Button btnSearch = new Button();
            btnSearch.Text = "搜索";
            btnSearch.Location = new Point(710, 0);
            btnSearch.Size = new Size(80, 30);
            btnSearch.FlatStyle = FlatStyle.Flat;
            pnlButtons.Controls.Add(btnSearch);
            
            // 用户列表数据网格
            DataGridView dgvUsers = new DataGridView();
            dgvUsers.Location = new Point(20, 110);
            dgvUsers.Size = new Size(800, 400);
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.ReadOnly = true;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.RowHeadersWidth = 30;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.BackgroundColor = Color.White;
            dgvUsers.BorderStyle = BorderStyle.Fixed3D;
            dgvUsers.Name = "dgvUsers";
            userPanel.Controls.Add(dgvUsers);
            
            // 创建数据表
            DataTable dt = new DataTable();
            dt.Columns.Add("用户ID", typeof(string));
            dt.Columns.Add("用户名", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("部门", typeof(string));
            dt.Columns.Add("角色", typeof(string));
            dt.Columns.Add("状态", typeof(string));
            dt.Columns.Add("创建时间", typeof(DateTime));
            
            // 添加示例数据
            dt.Rows.Add("001", "admin", "系统管理员", "IT部门", "管理员", "启用", DateTime.Now.AddMonths(-6));
            dt.Rows.Add("002", "zhangsan", "张三", "生产部", "主管", "启用", DateTime.Now.AddMonths(-3));
            dt.Rows.Add("003", "lisi", "李四", "仓库", "仓管员", "启用", DateTime.Now.AddMonths(-2));
            dt.Rows.Add("004", "wangwu", "王五", "质检部", "质检员", "停用", DateTime.Now.AddMonths(-1));
            dt.Rows.Add("005", "zhaoliu", "赵六", "设备部", "维修工程师", "启用", DateTime.Now.AddDays(-15));
            
            // 绑定数据
            dgvUsers.DataSource = dt;
            
            // 添加分页控件
            Panel pnlPager = new Panel();
            pnlPager.Location = new Point(20, 520);
            pnlPager.Size = new Size(800, 40);
            userPanel.Controls.Add(pnlPager);
            
            Button btnFirst = new Button();
            btnFirst.Text = "首页";
            btnFirst.Location = new Point(0, 0);
            btnFirst.Size = new Size(60, 30);
            btnFirst.FlatStyle = FlatStyle.Flat;
            pnlPager.Controls.Add(btnFirst);
            
            Button btnPrev = new Button();
            btnPrev.Text = "上一页";
            btnPrev.Location = new Point(70, 0);
            btnPrev.Size = new Size(60, 30);
            btnPrev.FlatStyle = FlatStyle.Flat;
            pnlPager.Controls.Add(btnPrev);
            
            Label lblPage = new Label();
            lblPage.Text = "第 1 页，共 1 页";
            lblPage.Location = new Point(140, 5);
            lblPage.Size = new Size(150, 20);
            lblPage.TextAlign = ContentAlignment.MiddleCenter;
            pnlPager.Controls.Add(lblPage);
            
            Button btnNext = new Button();
            btnNext.Text = "下一页";
            btnNext.Location = new Point(300, 0);
            btnNext.Size = new Size(60, 30);
            btnNext.FlatStyle = FlatStyle.Flat;
            pnlPager.Controls.Add(btnNext);
            
            Button btnLast = new Button();
            btnLast.Text = "末页";
            btnLast.Location = new Point(370, 0);
            btnLast.Size = new Size(60, 30);
            btnLast.FlatStyle = FlatStyle.Flat;
            pnlPager.Controls.Add(btnLast);
            
            return userPanel;
        }
        
        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            MessageBox.Show("添加用户功能将在此实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void BtnEditUser_Click(object sender, EventArgs e)
        {
            // 获取当前选中行
            DataGridView dgv = FindControlByName(pnlContent, "dgvUsers") as DataGridView;
            if (dgv != null && dgv.SelectedRows.Count > 0)
            {
                string userId = dgv.SelectedRows[0].Cells["用户ID"].Value.ToString();
                string username = dgv.SelectedRows[0].Cells["用户名"].Value.ToString();
                MessageBox.Show($"编辑用户: {userId} - {username}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("请先选择要编辑的用户", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void BtnDeleteUser_Click(object sender, EventArgs e)
        {
            // 获取当前选中行
            DataGridView dgv = FindControlByName(pnlContent, "dgvUsers") as DataGridView;
            if (dgv != null && dgv.SelectedRows.Count > 0)
            {
                string userId = dgv.SelectedRows[0].Cells["用户ID"].Value.ToString();
                string username = dgv.SelectedRows[0].Cells["用户名"].Value.ToString();
                
                DialogResult result = MessageBox.Show($"确定要删除用户 {username} 吗？", "确认删除", 
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // 执行删除操作
                    dgv.Rows.RemoveAt(dgv.SelectedRows[0].Index);
                    MessageBox.Show("用户已删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("请先选择要删除的用户", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        /// <summary>
        /// 根据名称查找控件
        /// </summary>
        private Control FindControlByName(Control parent, string name)
        {
            if (parent.Name == name)
                return parent;
                
            foreach (Control control in parent.Controls)
            {
                if (control.Name == name)
                    return control;
                    
                Control found = FindControlByName(control, name);
                if (found != null)
                    return found;
            }
            
            return null;
        }
        
        // ... 添加用户管理菜单点击事件 ...
        private void btnUserManage_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildForm("UserManagement", null);
        }

        private void btnUserManagement_Click(object sender, EventArgs e)
        {
            OpenChildForm(new UserManagementForm(_logger, _configuration));
        }
    }
} 