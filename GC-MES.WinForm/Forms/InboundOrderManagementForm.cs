using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GC_MES.Model;

namespace GC_MES.WinForm.Forms
{
    public partial class InboundOrderManagementForm : Form
    {
        private readonly ILogger<InboundOrderManagementForm> _logger;
        private readonly IConfiguration _configuration;
        
        // 用于存储当前检索条件
        private string currentFilter = string.Empty;
        private DataTable dtInbounds;
        
        public InboundOrderManagementForm(ILogger<InboundOrderManagementForm> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            InitializeComponent();
            
            // 设置窗体标题
            this.Text = "入库单管理";
            
            // 订阅主题变更事件
            ThemeManager.ThemeChanged += ThemeManager_ThemeChanged;
        }
        
        private void ThemeManager_ThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            ThemeManager.ApplyTheme(this);
        }
        
        private void InboundOrderManagementForm_Load(object sender, EventArgs e)
        {
            InitializeComponents();
            LoadComboBoxData();
            InitializeDataGrid();
            LoadData();
            
            // 应用当前主题
            ThemeManager.ApplyTheme(this);
        }
        
        private void InitializeComponents()
        {
            // 设置窗体大小和属性
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
            // 创建顶部面板
            Panel topPanel = new Panel();
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 60;
            topPanel.BackColor = SystemColors.Control;
            this.Controls.Add(topPanel);
            
            // 创建标题标签
            Label lblTitle = new Label();
            lblTitle.Text = "入库单管理";
            lblTitle.Font = new Font("Microsoft YaHei UI", 14, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(20, 15);
            topPanel.Controls.Add(lblTitle);
            
            // 创建搜索面板
            Panel searchPanel = new Panel();
            searchPanel.Dock = DockStyle.Top;
            searchPanel.Height = 60;
            searchPanel.BackColor = SystemColors.Control;
            searchPanel.Padding = new Padding(10);
            this.Controls.Add(searchPanel);
            
            // 入库单号标签
            Label lblCode = new Label();
            lblCode.Text = "入库单号:";
            lblCode.AutoSize = true;
            lblCode.Location = new Point(20, 22);
            searchPanel.Controls.Add(lblCode);
            
            // 入库单号输入框
            TextBox txtCode = new TextBox();
            txtCode.Name = "txtCode";
            txtCode.Location = new Point(100, 18);
            txtCode.Size = new Size(120, 25);
            searchPanel.Controls.Add(txtCode);
            
            // 入库类型标签
            Label lblType = new Label();
            lblType.Text = "入库类型:";
            lblType.AutoSize = true;
            lblType.Location = new Point(230, 22);
            searchPanel.Controls.Add(lblType);
            
            // 入库类型下拉框
            ComboBox cmbType = new ComboBox();
            cmbType.Name = "cmbType";
            cmbType.Location = new Point(300, 18);
            cmbType.Size = new Size(120, 25);
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            searchPanel.Controls.Add(cmbType);
            
            // 仓库标签
            Label lblWarehouse = new Label();
            lblWarehouse.Text = "仓库:";
            lblWarehouse.AutoSize = true;
            lblWarehouse.Location = new Point(430, 22);
            searchPanel.Controls.Add(lblWarehouse);
            
            // 仓库下拉框
            ComboBox cmbWarehouse = new ComboBox();
            cmbWarehouse.Name = "cmbWarehouse";
            cmbWarehouse.Location = new Point(480, 18);
            cmbWarehouse.Size = new Size(120, 25);
            cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            searchPanel.Controls.Add(cmbWarehouse);
            
            // 状态标签
            Label lblStatus = new Label();
            lblStatus.Text = "状态:";
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(610, 22);
            searchPanel.Controls.Add(lblStatus);
            
            // 状态下拉框
            ComboBox cmbStatus = new ComboBox();
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Location = new Point(650, 18);
            cmbStatus.Size = new Size(100, 25);
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            searchPanel.Controls.Add(cmbStatus);
            
            // 搜索按钮
            Button btnSearch = new Button();
            btnSearch.Name = "btnSearch";
            btnSearch.Text = "搜索";
            btnSearch.Location = new Point(760, 18);
            btnSearch.Size = new Size(80, 30);
            btnSearch.Click += BtnSearch_Click;
            searchPanel.Controls.Add(btnSearch);
            
            // 重置按钮
            Button btnReset = new Button();
            btnReset.Name = "btnReset";
            btnReset.Text = "重置";
            btnReset.Location = new Point(850, 18);
            btnReset.Size = new Size(80, 30);
            btnReset.Click += BtnReset_Click;
            searchPanel.Controls.Add(btnReset);
            
            // 创建操作按钮面板
            Panel actionPanel = new Panel();
            actionPanel.Dock = DockStyle.Top;
            actionPanel.Height = 40;
            actionPanel.BackColor = SystemColors.Control;
            actionPanel.Padding = new Padding(10);
            this.Controls.Add(actionPanel);
            
            // 新建入库单按钮
            Button btnAdd = new Button();
            btnAdd.Name = "btnAdd";
            btnAdd.Text = "新建入库单";
            btnAdd.Location = new Point(20, 5);
            btnAdd.Size = new Size(100, 30);
            btnAdd.Click += BtnAdd_Click;
            actionPanel.Controls.Add(btnAdd);
            
            // 编辑按钮
            Button btnEdit = new Button();
            btnEdit.Name = "btnEdit";
            btnEdit.Text = "编辑";
            btnEdit.Location = new Point(130, 5);
            btnEdit.Size = new Size(80, 30);
            btnEdit.Click += BtnEdit_Click;
            actionPanel.Controls.Add(btnEdit);
            
            // 删除按钮
            Button btnDelete = new Button();
            btnDelete.Name = "btnDelete";
            btnDelete.Text = "删除";
            btnDelete.Location = new Point(220, 5);
            btnDelete.Size = new Size(80, 30);
            btnDelete.Click += BtnDelete_Click;
            actionPanel.Controls.Add(btnDelete);
            
            // 查看明细按钮
            Button btnViewItems = new Button();
            btnViewItems.Name = "btnViewItems";
            btnViewItems.Text = "查看明细";
            btnViewItems.Location = new Point(310, 5);
            btnViewItems.Size = new Size(100, 30);
            btnViewItems.Click += BtnViewItems_Click;
            actionPanel.Controls.Add(btnViewItems);
            
            // 审核按钮
            Button btnApprove = new Button();
            btnApprove.Name = "btnApprove";
            btnApprove.Text = "审核";
            btnApprove.Location = new Point(420, 5);
            btnApprove.Size = new Size(80, 30);
            btnApprove.Click += BtnApprove_Click;
            actionPanel.Controls.Add(btnApprove);
            
            // 打印按钮
            Button btnPrint = new Button();
            btnPrint.Name = "btnPrint";
            btnPrint.Text = "打印";
            btnPrint.Location = new Point(510, 5);
            btnPrint.Size = new Size(80, 30);
            btnPrint.Click += BtnPrint_Click;
            actionPanel.Controls.Add(btnPrint);
            
            // 导出按钮
            Button btnExport = new Button();
            btnExport.Name = "btnExport";
            btnExport.Text = "导出";
            btnExport.Location = new Point(600, 5);
            btnExport.Size = new Size(80, 30);
            btnExport.Click += BtnExport_Click;
            actionPanel.Controls.Add(btnExport);
            
            // 创建数据网格视图
            DataGridView dgvInbounds = new DataGridView();
            dgvInbounds.Name = "dgvInbounds";
            dgvInbounds.Dock = DockStyle.Fill;
            dgvInbounds.AllowUserToAddRows = false;
            dgvInbounds.AllowUserToDeleteRows = false;
            dgvInbounds.ReadOnly = true;
            dgvInbounds.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInbounds.RowHeadersWidth = 30;
            dgvInbounds.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInbounds.BackgroundColor = Color.White;
            dgvInbounds.BorderStyle = BorderStyle.Fixed3D;
            dgvInbounds.RowTemplate.Height = 25;
            dgvInbounds.DoubleClick += DgvInbounds_DoubleClick;
            this.Controls.Add(dgvInbounds);
            
            // 创建底部面板
            Panel bottomPanel = new Panel();
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Height = 40;
            bottomPanel.BackColor = SystemColors.Control;
            this.Controls.Add(bottomPanel);
            
            // 创建分页控件
            Label lblPage = new Label();
            lblPage.Name = "lblPage";
            lblPage.Text = "第 1 页，共 1 页";
            lblPage.Location = new Point(20, 10);
            lblPage.AutoSize = true;
            bottomPanel.Controls.Add(lblPage);
            
            // 上一页按钮
            Button btnPrev = new Button();
            btnPrev.Name = "btnPrev";
            btnPrev.Text = "上一页";
            btnPrev.Location = new Point(150, 5);
            btnPrev.Size = new Size(80, 30);
            btnPrev.Click += BtnPrev_Click;
            bottomPanel.Controls.Add(btnPrev);
            
            // 下一页按钮
            Button btnNext = new Button();
            btnNext.Name = "btnNext";
            btnNext.Text = "下一页";
            btnNext.Location = new Point(240, 5);
            btnNext.Size = new Size(80, 30);
            btnNext.Click += BtnNext_Click;
            bottomPanel.Controls.Add(btnNext);
            
            // 关闭按钮
            Button btnClose = new Button();
            btnClose.Name = "btnClose";
            btnClose.Text = "关闭";
            btnClose.Location = new Point(880, 5);
            btnClose.Size = new Size(80, 30);
            btnClose.Click += BtnClose_Click;
            bottomPanel.Controls.Add(btnClose);
        }
        
        private void LoadComboBoxData()
        {
            // 获取入库类型下拉框
            ComboBox cmbType = this.Controls.Find("cmbType", true).FirstOrDefault() as ComboBox;
            if (cmbType != null)
            {
                // 添加选项
                cmbType.Items.Add(new { Text = "全部", Value = "" });
                cmbType.Items.Add(new { Text = "采购入库", Value = "采购入库" });
                cmbType.Items.Add(new { Text = "生产入库", Value = "生产入库" });
                cmbType.Items.Add(new { Text = "退货入库", Value = "退货入库" });
                cmbType.Items.Add(new { Text = "其他入库", Value = "其他入库" });
                cmbType.DisplayMember = "Text";
                cmbType.ValueMember = "Value";
                cmbType.SelectedIndex = 0;
            }
            
            // 获取仓库下拉框
            ComboBox cmbWarehouse = this.Controls.Find("cmbWarehouse", true).FirstOrDefault() as ComboBox;
            if (cmbWarehouse != null)
            {
                // 添加选项（模拟数据）
                cmbWarehouse.Items.Add(new { Text = "全部", Value = 0 });
                cmbWarehouse.Items.Add(new { Text = "原料仓库", Value = 1 });
                cmbWarehouse.Items.Add(new { Text = "成品仓库", Value = 2 });
                cmbWarehouse.Items.Add(new { Text = "半成品仓库", Value = 3 });
                cmbWarehouse.DisplayMember = "Text";
                cmbWarehouse.ValueMember = "Value";
                cmbWarehouse.SelectedIndex = 0;
            }
            
            // 获取状态下拉框
            ComboBox cmbStatus = this.Controls.Find("cmbStatus", true).FirstOrDefault() as ComboBox;
            if (cmbStatus != null)
            {
                // 添加选项
                cmbStatus.Items.Add(new { Text = "全部", Value = "" });
                cmbStatus.Items.Add(new { Text = "待入库", Value = "待入库" });
                cmbStatus.Items.Add(new { Text = "部分入库", Value = "部分入库" });
                cmbStatus.Items.Add(new { Text = "已完成", Value = "已完成" });
                cmbStatus.DisplayMember = "Text";
                cmbStatus.ValueMember = "Value";
                cmbStatus.SelectedIndex = 0;
            }
        }
        
        private void InitializeDataGrid()
        {
            // 获取数据网格视图
            DataGridView dgvInbounds = this.Controls.Find("dgvInbounds", true).FirstOrDefault() as DataGridView;
            if (dgvInbounds != null)
            {
                // 创建列
                dgvInbounds.Columns.Add("InboundId", "ID");
                dgvInbounds.Columns.Add("InboundCode", "入库单号");
                dgvInbounds.Columns.Add("InboundType", "入库类型");
                dgvInbounds.Columns.Add("RelatedCode", "关联单号");
                dgvInbounds.Columns.Add("WarehouseName", "仓库");
                dgvInbounds.Columns.Add("InboundDate", "入库日期");
                dgvInbounds.Columns.Add("Operator", "操作人");
                dgvInbounds.Columns.Add("Status", "状态");
                dgvInbounds.Columns.Add("ApprovalStatus", "审核状态");
                dgvInbounds.Columns.Add("Approver", "审核人");
                dgvInbounds.Columns.Add("ApprovalDate", "审核日期");
                dgvInbounds.Columns.Add("CreateTime", "创建时间");
                
                // 设置列的样式
                dgvInbounds.Columns["InboundId"].Width = 60;
                dgvInbounds.Columns["InboundId"].Visible = false;
                dgvInbounds.Columns["InboundCode"].Width = 120;
                dgvInbounds.Columns["InboundType"].Width = 100;
                dgvInbounds.Columns["RelatedCode"].Width = 120;
                dgvInbounds.Columns["WarehouseName"].Width = 100;
                dgvInbounds.Columns["InboundDate"].Width = 100;
                dgvInbounds.Columns["Operator"].Width = 80;
                dgvInbounds.Columns["Status"].Width = 80;
                dgvInbounds.Columns["ApprovalStatus"].Width = 80;
                dgvInbounds.Columns["Approver"].Width = 80;
                dgvInbounds.Columns["ApprovalDate"].Width = 100;
                dgvInbounds.Columns["CreateTime"].Width = 140;
                
                // 设置默认排序列
                dgvInbounds.Sort(dgvInbounds.Columns["CreateTime"], ListSortDirection.Descending);
            }
        }
        
        private void LoadData()
        {
            try
            {
                // 获取数据网格视图和搜索条件
                DataGridView dgvInbounds = this.Controls.Find("dgvInbounds", true).FirstOrDefault() as DataGridView;
                TextBox txtCode = this.Controls.Find("txtCode", true).FirstOrDefault() as TextBox;
                ComboBox cmbType = this.Controls.Find("cmbType", true).FirstOrDefault() as ComboBox;
                ComboBox cmbWarehouse = this.Controls.Find("cmbWarehouse", true).FirstOrDefault() as ComboBox;
                ComboBox cmbStatus = this.Controls.Find("cmbStatus", true).FirstOrDefault() as ComboBox;
                
                if (dgvInbounds != null)
                {
                    // 创建模拟数据
                    dtInbounds = new DataTable();
                    dtInbounds.Columns.Add("InboundId", typeof(int));
                    dtInbounds.Columns.Add("InboundCode", typeof(string));
                    dtInbounds.Columns.Add("InboundType", typeof(string));
                    dtInbounds.Columns.Add("RelatedCode", typeof(string));
                    dtInbounds.Columns.Add("WarehouseId", typeof(int));
                    dtInbounds.Columns.Add("WarehouseName", typeof(string));
                    dtInbounds.Columns.Add("InboundDate", typeof(DateTime));
                    dtInbounds.Columns.Add("Operator", typeof(string));
                    dtInbounds.Columns.Add("Status", typeof(string));
                    dtInbounds.Columns.Add("ApprovalStatus", typeof(string));
                    dtInbounds.Columns.Add("Approver", typeof(string));
                    dtInbounds.Columns.Add("ApprovalDate", typeof(DateTime?));
                    dtInbounds.Columns.Add("CreateTime", typeof(DateTime));
                    
                    // 添加模拟数据
                    dtInbounds.Rows.Add(1, "RK-2023-001", "采购入库", "CG-2023-001", 1, "原料仓库", DateTime.Now.AddDays(-10), "张三", "已完成", "已审核", "李四", DateTime.Now.AddDays(-9), DateTime.Now.AddDays(-10));
                    dtInbounds.Rows.Add(2, "RK-2023-002", "生产入库", "SC-2023-001", 2, "成品仓库", DateTime.Now.AddDays(-9), "李四", "已完成", "已审核", "王五", DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-9));
                    dtInbounds.Rows.Add(3, "RK-2023-003", "采购入库", "CG-2023-002", 1, "原料仓库", DateTime.Now.AddDays(-8), "张三", "已完成", "已审核", "李四", DateTime.Now.AddDays(-7), DateTime.Now.AddDays(-8));
                    dtInbounds.Rows.Add(4, "RK-2023-004", "退货入库", "TH-2023-001", 2, "成品仓库", DateTime.Now.AddDays(-7), "王五", "已完成", "已审核", "赵六", DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-7));
                    dtInbounds.Rows.Add(5, "RK-2023-005", "采购入库", "CG-2023-003", 1, "原料仓库", DateTime.Now.AddDays(-6), "张三", "部分入库", "已审核", "李四", DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-6));
                    dtInbounds.Rows.Add(6, "RK-2023-006", "其他入库", "", 3, "半成品仓库", DateTime.Now.AddDays(-5), "李四", "待入库", "未审核", null, null, DateTime.Now.AddDays(-5));
                    dtInbounds.Rows.Add(7, "RK-2023-007", "采购入库", "CG-2023-004", 1, "原料仓库", DateTime.Now.AddDays(-4), "张三", "待入库", "未审核", null, null, DateTime.Now.AddDays(-4));
                    dtInbounds.Rows.Add(8, "RK-2023-008", "生产入库", "SC-2023-002", 2, "成品仓库", DateTime.Now.AddDays(-3), "王五", "待入库", "未审核", null, null, DateTime.Now.AddDays(-3));
                    
                    // 应用筛选条件
                    string filter = "";
                    if (txtCode != null && !string.IsNullOrEmpty(txtCode.Text))
                        filter += string.IsNullOrEmpty(filter) ? $"InboundCode LIKE '%{txtCode.Text}%'" : $" AND InboundCode LIKE '%{txtCode.Text}%'";
                    
                    if (cmbType != null && cmbType.SelectedIndex > 0)
                    {
                        string selectedType = (cmbType.SelectedItem as dynamic).Value;
                        filter += string.IsNullOrEmpty(filter) ? $"InboundType = '{selectedType}'" : $" AND InboundType = '{selectedType}'";
                    }
                    
                    if (cmbWarehouse != null && cmbWarehouse.SelectedIndex > 0)
                    {
                        int warehouseId = (cmbWarehouse.SelectedItem as dynamic).Value;
                        filter += string.IsNullOrEmpty(filter) ? $"WarehouseId = {warehouseId}" : $" AND WarehouseId = {warehouseId}";
                    }
                    
                    if (cmbStatus != null && cmbStatus.SelectedIndex > 0)
                    {
                        string selectedStatus = (cmbStatus.SelectedItem as dynamic).Value;
                        filter += string.IsNullOrEmpty(filter) ? $"Status = '{selectedStatus}'" : $" AND Status = '{selectedStatus}'";
                    }
                    
                    currentFilter = filter;
                    
                    // 应用筛选
                    if (!string.IsNullOrEmpty(filter))
                        dtInbounds.DefaultView.RowFilter = filter;
                    
                    // 绑定数据
                    dgvInbounds.DataSource = dtInbounds;
                    
                    // 更新分页信息
                    Label lblPage = this.Controls.Find("lblPage", true).FirstOrDefault() as Label;
                    if (lblPage != null)
                        lblPage.Text = $"共 {dtInbounds.DefaultView.Count} 条记录";
                    
                    // 设置按钮状态
                    UpdateButtonStates();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载入库单数据时出错");
                MessageBox.Show($"加载数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void UpdateButtonStates()
        {
            DataGridView dgvInbounds = this.Controls.Find("dgvInbounds", true).FirstOrDefault() as DataGridView;
            Button btnEdit = this.Controls.Find("btnEdit", true).FirstOrDefault() as Button;
            Button btnDelete = this.Controls.Find("btnDelete", true).FirstOrDefault() as Button;
            Button btnViewItems = this.Controls.Find("btnViewItems", true).FirstOrDefault() as Button;
            Button btnApprove = this.Controls.Find("btnApprove", true).FirstOrDefault() as Button;
            
            bool hasSelection = dgvInbounds != null && dgvInbounds.SelectedRows.Count > 0;
            
            if (btnEdit != null) btnEdit.Enabled = hasSelection;
            if (btnDelete != null) btnDelete.Enabled = hasSelection;
            if (btnViewItems != null) btnViewItems.Enabled = hasSelection;
            
            if (btnApprove != null && hasSelection)
            {
                string approvalStatus = dgvInbounds.SelectedRows[0].Cells["ApprovalStatus"].Value.ToString();
                btnApprove.Enabled = approvalStatus == "未审核";
            }
            else if (btnApprove != null)
            {
                btnApprove.Enabled = false;
            }
        }
        
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        
        private void BtnReset_Click(object sender, EventArgs e)
        {
            TextBox txtCode = this.Controls.Find("txtCode", true).FirstOrDefault() as TextBox;
            ComboBox cmbType = this.Controls.Find("cmbType", true).FirstOrDefault() as ComboBox;
            ComboBox cmbWarehouse = this.Controls.Find("cmbWarehouse", true).FirstOrDefault() as ComboBox;
            ComboBox cmbStatus = this.Controls.Find("cmbStatus", true).FirstOrDefault() as ComboBox;
            
            if (txtCode != null) txtCode.Text = string.Empty;
            if (cmbType != null) cmbType.SelectedIndex = 0;
            if (cmbWarehouse != null) cmbWarehouse.SelectedIndex = 0;
            if (cmbStatus != null) cmbStatus.SelectedIndex = 0;
            
            LoadData();
        }
        
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("创建入库单功能正在开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            DataGridView dgvInbounds = this.Controls.Find("dgvInbounds", true).FirstOrDefault() as DataGridView;
            if (dgvInbounds != null && dgvInbounds.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvInbounds.SelectedRows[0].Cells["InboundId"].Value);
                string code = dgvInbounds.SelectedRows[0].Cells["InboundCode"].Value.ToString();
                string status = dgvInbounds.SelectedRows[0].Cells["Status"].Value.ToString();
                string approvalStatus = dgvInbounds.SelectedRows[0].Cells["ApprovalStatus"].Value.ToString();
                
                if (approvalStatus == "已审核")
                {
                    MessageBox.Show("已审核的入库单不能编辑！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                MessageBox.Show($"编辑入库单: {code} 功能正在开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("请先选择要编辑的入库单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DataGridView dgvInbounds = this.Controls.Find("dgvInbounds", true).FirstOrDefault() as DataGridView;
            if (dgvInbounds != null && dgvInbounds.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvInbounds.SelectedRows[0].Cells["InboundId"].Value);
                string code = dgvInbounds.SelectedRows[0].Cells["InboundCode"].Value.ToString();
                string status = dgvInbounds.SelectedRows[0].Cells["Status"].Value.ToString();
                string approvalStatus = dgvInbounds.SelectedRows[0].Cells["ApprovalStatus"].Value.ToString();
                
                if (status != "待入库" || approvalStatus == "已审核")
                {
                    MessageBox.Show("只能删除待入库且未审核的入库单！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                DialogResult result = MessageBox.Show($"确定要删除入库单: {code} 吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // 这里添加删除逻辑
                    // 模拟删除成功
                    dgvInbounds.Rows.Remove(dgvInbounds.SelectedRows[0]);
                    MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("请先选择要删除的入库单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void BtnViewItems_Click(object sender, EventArgs e)
        {
            DataGridView dgvInbounds = this.Controls.Find("dgvInbounds", true).FirstOrDefault() as DataGridView;
            if (dgvInbounds != null && dgvInbounds.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvInbounds.SelectedRows[0].Cells["InboundId"].Value);
                string code = dgvInbounds.SelectedRows[0].Cells["InboundCode"].Value.ToString();
                
                MessageBox.Show($"查看入库单明细: {code} 功能正在开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("请先选择要查看的入库单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void BtnApprove_Click(object sender, EventArgs e)
        {
            DataGridView dgvInbounds = this.Controls.Find("dgvInbounds", true).FirstOrDefault() as DataGridView;
            if (dgvInbounds != null && dgvInbounds.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvInbounds.SelectedRows[0].Cells["InboundId"].Value);
                string code = dgvInbounds.SelectedRows[0].Cells["InboundCode"].Value.ToString();
                string approvalStatus = dgvInbounds.SelectedRows[0].Cells["ApprovalStatus"].Value.ToString();
                
                if (approvalStatus == "已审核")
                {
                    MessageBox.Show("该入库单已审核！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                DialogResult result = MessageBox.Show($"确定要审核入库单: {code} 吗？", "确认审核", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // 这里添加审核逻辑
                    // 模拟审核成功
                    dgvInbounds.SelectedRows[0].Cells["ApprovalStatus"].Value = "已审核";
                    dgvInbounds.SelectedRows[0].Cells["Approver"].Value = "当前用户";
                    dgvInbounds.SelectedRows[0].Cells["ApprovalDate"].Value = DateTime.Now;
                    MessageBox.Show("审核成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // 更新按钮状态
                    UpdateButtonStates();
                }
            }
            else
            {
                MessageBox.Show("请先选择要审核的入库单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            DataGridView dgvInbounds = this.Controls.Find("dgvInbounds", true).FirstOrDefault() as DataGridView;
            if (dgvInbounds != null && dgvInbounds.SelectedRows.Count > 0)
            {
                string code = dgvInbounds.SelectedRows[0].Cells["InboundCode"].Value.ToString();
                MessageBox.Show($"打印入库单: {code} 功能正在开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("请先选择要打印的入库单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void BtnExport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("导出功能正在开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void BtnPrev_Click(object sender, EventArgs e)
        {
            MessageBox.Show("上一页功能正在开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void BtnNext_Click(object sender, EventArgs e)
        {
            MessageBox.Show("下一页功能正在开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void DgvInbounds_DoubleClick(object sender, EventArgs e)
        {
            BtnViewItems_Click(sender, e);
        }
    }
} 