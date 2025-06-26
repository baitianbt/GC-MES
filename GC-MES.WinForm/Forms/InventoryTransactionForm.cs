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
    public partial class InventoryTransactionForm : Form
    {
        private readonly ILogger<InventoryTransactionForm> _logger;
        private readonly IConfiguration _configuration;
        
        // 用于存储当前检索条件
        private string currentFilter = string.Empty;
        private DataTable dtTransactions;
        
        public InventoryTransactionForm(ILogger<InventoryTransactionForm> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            InitializeComponent();
            
            // 设置窗体标题
            this.Text = "库存收发明细";
            
            // 订阅主题变更事件
            ThemeManager.ThemeChanged += ThemeManager_ThemeChanged;
        }
        
        private void ThemeManager_ThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            ThemeManager.ApplyTheme(this);
        }
        
        private void InventoryTransactionForm_Load(object sender, EventArgs e)
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
            lblTitle.Text = "库存收发明细";
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
            
            // 日期范围标签
            Label lblDateRange = new Label();
            lblDateRange.Text = "日期范围:";
            lblDateRange.AutoSize = true;
            lblDateRange.Location = new Point(20, 22);
            searchPanel.Controls.Add(lblDateRange);
            
            // 开始日期选择器
            DateTimePicker dtpStart = new DateTimePicker();
            dtpStart.Name = "dtpStart";
            dtpStart.Format = DateTimePickerFormat.Short;
            dtpStart.Value = DateTime.Today.AddDays(-30);
            dtpStart.Location = new Point(100, 18);
            dtpStart.Size = new Size(100, 25);
            searchPanel.Controls.Add(dtpStart);
            
            // 日期分隔符
            Label lblDateSeparator = new Label();
            lblDateSeparator.Text = "至";
            lblDateSeparator.AutoSize = true;
            lblDateSeparator.Location = new Point(205, 22);
            searchPanel.Controls.Add(lblDateSeparator);
            
            // 结束日期选择器
            DateTimePicker dtpEnd = new DateTimePicker();
            dtpEnd.Name = "dtpEnd";
            dtpEnd.Format = DateTimePickerFormat.Short;
            dtpEnd.Value = DateTime.Today;
            dtpEnd.Location = new Point(220, 18);
            dtpEnd.Size = new Size(100, 25);
            searchPanel.Controls.Add(dtpEnd);
            
            // 事务类型标签
            Label lblType = new Label();
            lblType.Text = "事务类型:";
            lblType.AutoSize = true;
            lblType.Location = new Point(330, 22);
            searchPanel.Controls.Add(lblType);
            
            // 事务类型下拉框
            ComboBox cmbType = new ComboBox();
            cmbType.Name = "cmbType";
            cmbType.Location = new Point(400, 18);
            cmbType.Size = new Size(100, 25);
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            searchPanel.Controls.Add(cmbType);
            
            // 物料标签
            Label lblProduct = new Label();
            lblProduct.Text = "物料:";
            lblProduct.AutoSize = true;
            lblProduct.Location = new Point(510, 22);
            searchPanel.Controls.Add(lblProduct);
            
            // 物料输入框
            TextBox txtProduct = new TextBox();
            txtProduct.Name = "txtProduct";
            txtProduct.Location = new Point(550, 18);
            txtProduct.Size = new Size(120, 25);
            searchPanel.Controls.Add(txtProduct);
            
            // 仓库标签
            Label lblWarehouse = new Label();
            lblWarehouse.Text = "仓库:";
            lblWarehouse.AutoSize = true;
            lblWarehouse.Location = new Point(680, 22);
            searchPanel.Controls.Add(lblWarehouse);
            
            // 仓库下拉框
            ComboBox cmbWarehouse = new ComboBox();
            cmbWarehouse.Name = "cmbWarehouse";
            cmbWarehouse.Location = new Point(720, 18);
            cmbWarehouse.Size = new Size(100, 25);
            cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            searchPanel.Controls.Add(cmbWarehouse);
            
            // 搜索按钮
            Button btnSearch = new Button();
            btnSearch.Name = "btnSearch";
            btnSearch.Text = "搜索";
            btnSearch.Location = new Point(830, 18);
            btnSearch.Size = new Size(70, 30);
            btnSearch.Click += BtnSearch_Click;
            searchPanel.Controls.Add(btnSearch);
            
            // 重置按钮
            Button btnReset = new Button();
            btnReset.Name = "btnReset";
            btnReset.Text = "重置";
            btnReset.Location = new Point(905, 18);
            btnReset.Size = new Size(70, 30);
            btnReset.Click += BtnReset_Click;
            searchPanel.Controls.Add(btnReset);
            
            // 创建操作按钮面板
            Panel actionPanel = new Panel();
            actionPanel.Dock = DockStyle.Top;
            actionPanel.Height = 40;
            actionPanel.BackColor = SystemColors.Control;
            actionPanel.Padding = new Padding(10);
            this.Controls.Add(actionPanel);
            
            // 导出按钮
            Button btnExport = new Button();
            btnExport.Name = "btnExport";
            btnExport.Text = "导出";
            btnExport.Location = new Point(20, 5);
            btnExport.Size = new Size(80, 30);
            btnExport.Click += BtnExport_Click;
            actionPanel.Controls.Add(btnExport);
            
            // 打印按钮
            Button btnPrint = new Button();
            btnPrint.Name = "btnPrint";
            btnPrint.Text = "打印";
            btnPrint.Location = new Point(110, 5);
            btnPrint.Size = new Size(80, 30);
            btnPrint.Click += BtnPrint_Click;
            actionPanel.Controls.Add(btnPrint);
            
            // 创建数据网格视图
            DataGridView dgvTransactions = new DataGridView();
            dgvTransactions.Name = "dgvTransactions";
            dgvTransactions.Dock = DockStyle.Fill;
            dgvTransactions.AllowUserToAddRows = false;
            dgvTransactions.AllowUserToDeleteRows = false;
            dgvTransactions.ReadOnly = true;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactions.RowHeadersWidth = 30;
            dgvTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTransactions.BackgroundColor = Color.White;
            dgvTransactions.BorderStyle = BorderStyle.Fixed3D;
            dgvTransactions.RowTemplate.Height = 25;
            this.Controls.Add(dgvTransactions);
            
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
            // 获取事务类型下拉框
            ComboBox cmbType = this.Controls.Find("cmbType", true).FirstOrDefault() as ComboBox;
            if (cmbType != null)
            {
                // 添加选项
                cmbType.Items.Add(new { Text = "全部", Value = "" });
                cmbType.Items.Add(new { Text = "入库", Value = "入库" });
                cmbType.Items.Add(new { Text = "出库", Value = "出库" });
                cmbType.Items.Add(new { Text = "调拨", Value = "调拨" });
                cmbType.Items.Add(new { Text = "盘点", Value = "盘点" });
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
        }
        
        private void InitializeDataGrid()
        {
            // 获取数据网格视图
            DataGridView dgvTransactions = this.Controls.Find("dgvTransactions", true).FirstOrDefault() as DataGridView;
            if (dgvTransactions != null)
            {
                // 创建列
                dgvTransactions.Columns.Add("TransactionId", "ID");
                dgvTransactions.Columns.Add("TransactionDate", "事务日期");
                dgvTransactions.Columns.Add("TransactionType", "事务类型");
                dgvTransactions.Columns.Add("DocumentType", "单据类型");
                dgvTransactions.Columns.Add("DocumentCode", "单据编号");
                dgvTransactions.Columns.Add("ProductCode", "物料编码");
                dgvTransactions.Columns.Add("ProductName", "物料名称");
                dgvTransactions.Columns.Add("BatchNo", "批次号");
                dgvTransactions.Columns.Add("WarehouseName", "仓库");
                dgvTransactions.Columns.Add("LocationName", "库位");
                dgvTransactions.Columns.Add("BeforeQuantity", "变动前数量");
                dgvTransactions.Columns.Add("TransactionQuantity", "变动数量");
                dgvTransactions.Columns.Add("AfterQuantity", "变动后数量");
                dgvTransactions.Columns.Add("Unit", "单位");
                dgvTransactions.Columns.Add("Operator", "操作人");
                dgvTransactions.Columns.Add("Remark", "备注");
                
                // 设置列的样式
                dgvTransactions.Columns["TransactionId"].Width = 60;
                dgvTransactions.Columns["TransactionId"].Visible = false;
                dgvTransactions.Columns["TransactionDate"].Width = 140;
                dgvTransactions.Columns["TransactionType"].Width = 80;
                dgvTransactions.Columns["DocumentType"].Width = 100;
                dgvTransactions.Columns["DocumentCode"].Width = 120;
                dgvTransactions.Columns["ProductCode"].Width = 100;
                dgvTransactions.Columns["ProductName"].Width = 150;
                dgvTransactions.Columns["BatchNo"].Width = 100;
                dgvTransactions.Columns["WarehouseName"].Width = 100;
                dgvTransactions.Columns["LocationName"].Width = 100;
                dgvTransactions.Columns["BeforeQuantity"].Width = 100;
                dgvTransactions.Columns["TransactionQuantity"].Width = 100;
                dgvTransactions.Columns["AfterQuantity"].Width = 100;
                dgvTransactions.Columns["Unit"].Width = 60;
                dgvTransactions.Columns["Operator"].Width = 80;
                dgvTransactions.Columns["Remark"].Width = 150;
                
                // 设置默认排序列
                dgvTransactions.Sort(dgvTransactions.Columns["TransactionDate"], ListSortDirection.Descending);
                
                // 设置数值列的格式
                dgvTransactions.Columns["BeforeQuantity"].DefaultCellStyle.Format = "N2";
                dgvTransactions.Columns["TransactionQuantity"].DefaultCellStyle.Format = "N2";
                dgvTransactions.Columns["AfterQuantity"].DefaultCellStyle.Format = "N2";
                
                // 使变动数量根据正负显示不同颜色
                dgvTransactions.CellFormatting += DgvTransactions_CellFormatting;
            }
        }
        
        private void DgvTransactions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 11 && e.Value != null) // TransactionQuantity 列
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal value))
                {
                    if (value > 0)
                    {
                        e.CellStyle.ForeColor = Color.Green;  // 正数用绿色
                        e.Value = "+" + value.ToString("N2");
                    }
                    else if (value < 0)
                    {
                        e.CellStyle.ForeColor = Color.Red;   // 负数用红色
                    }
                }
            }
        }
        
        private void LoadData()
        {
            try
            {
                // 获取数据网格视图和搜索条件
                DataGridView dgvTransactions = this.Controls.Find("dgvTransactions", true).FirstOrDefault() as DataGridView;
                DateTimePicker dtpStart = this.Controls.Find("dtpStart", true).FirstOrDefault() as DateTimePicker;
                DateTimePicker dtpEnd = this.Controls.Find("dtpEnd", true).FirstOrDefault() as DateTimePicker;
                ComboBox cmbType = this.Controls.Find("cmbType", true).FirstOrDefault() as ComboBox;
                TextBox txtProduct = this.Controls.Find("txtProduct", true).FirstOrDefault() as TextBox;
                ComboBox cmbWarehouse = this.Controls.Find("cmbWarehouse", true).FirstOrDefault() as ComboBox;
                
                if (dgvTransactions != null)
                {
                    // 创建模拟数据
                    dtTransactions = new DataTable();
                    dtTransactions.Columns.Add("TransactionId", typeof(int));
                    dtTransactions.Columns.Add("TransactionDate", typeof(DateTime));
                    dtTransactions.Columns.Add("TransactionType", typeof(string));
                    dtTransactions.Columns.Add("DocumentType", typeof(string));
                    dtTransactions.Columns.Add("DocumentCode", typeof(string));
                    dtTransactions.Columns.Add("ProductId", typeof(int));
                    dtTransactions.Columns.Add("ProductCode", typeof(string));
                    dtTransactions.Columns.Add("ProductName", typeof(string));
                    dtTransactions.Columns.Add("BatchNo", typeof(string));
                    dtTransactions.Columns.Add("WarehouseId", typeof(int));
                    dtTransactions.Columns.Add("WarehouseName", typeof(string));
                    dtTransactions.Columns.Add("LocationId", typeof(int));
                    dtTransactions.Columns.Add("LocationName", typeof(string));
                    dtTransactions.Columns.Add("BeforeQuantity", typeof(decimal));
                    dtTransactions.Columns.Add("TransactionQuantity", typeof(decimal));
                    dtTransactions.Columns.Add("AfterQuantity", typeof(decimal));
                    dtTransactions.Columns.Add("Unit", typeof(string));
                    dtTransactions.Columns.Add("Operator", typeof(string));
                    dtTransactions.Columns.Add("Remark", typeof(string));
                    
                    // 添加模拟数据
                    // 入库事务
                    dtTransactions.Rows.Add(1, DateTime.Now.AddDays(-10), "入库", "入库单", "RK-2023-001", 1, "M001", "钢板", "B20230601", 1, "原料仓库", 1, "A-01-01", 100, 50, 150, "kg", "张三", "采购入库");
                    dtTransactions.Rows.Add(2, DateTime.Now.AddDays(-9), "入库", "入库单", "RK-2023-002", 2, "M002", "电子元件", "B20230602", 1, "原料仓库", 2, "A-02-01", 200, 100, 300, "pcs", "李四", "采购入库");
                    dtTransactions.Rows.Add(3, DateTime.Now.AddDays(-8), "入库", "入库单", "RK-2023-003", 3, "P001", "成品A", "B20230603", 2, "成品仓库", 3, "B-01-01", 0, 20, 20, "pcs", "王五", "生产入库");
                    
                    // 出库事务
                    dtTransactions.Rows.Add(4, DateTime.Now.AddDays(-7), "出库", "出库单", "CK-2023-001", 1, "M001", "钢板", "B20230601", 1, "原料仓库", 1, "A-01-01", 150, -30, 120, "kg", "张三", "生产领料");
                    dtTransactions.Rows.Add(5, DateTime.Now.AddDays(-6), "出库", "出库单", "CK-2023-002", 2, "M002", "电子元件", "B20230602", 1, "原料仓库", 2, "A-02-01", 300, -50, 250, "pcs", "李四", "生产领料");
                    dtTransactions.Rows.Add(6, DateTime.Now.AddDays(-5), "出库", "出库单", "CK-2023-003", 3, "P001", "成品A", "B20230603", 2, "成品仓库", 3, "B-01-01", 20, -10, 10, "pcs", "王五", "销售出库");
                    
                    // 调拨事务
                    dtTransactions.Rows.Add(7, DateTime.Now.AddDays(-4), "调拨", "调拨单", "DB-2023-001", 1, "M001", "钢板", "B20230601", 1, "原料仓库", 1, "A-01-01", 120, -20, 100, "kg", "张三", "库位调拨-出");
                    dtTransactions.Rows.Add(8, DateTime.Now.AddDays(-4), "调拨", "调拨单", "DB-2023-001", 1, "M001", "钢板", "B20230601", 1, "原料仓库", 4, "A-01-02", 0, 20, 20, "kg", "张三", "库位调拨-入");
                    
                    // 盘点事务
                    dtTransactions.Rows.Add(9, DateTime.Now.AddDays(-3), "盘点", "盘点单", "PD-2023-001", 2, "M002", "电子元件", "B20230602", 1, "原料仓库", 2, "A-02-01", 250, -5, 245, "pcs", "赵六", "盘点差异调整");
                    dtTransactions.Rows.Add(10, DateTime.Now.AddDays(-2), "盘点", "盘点单", "PD-2023-001", 3, "P001", "成品A", "B20230603", 2, "成品仓库", 3, "B-01-01", 10, 2, 12, "pcs", "赵六", "盘点差异调整");
                    
                    // 应用筛选条件
                    string filter = "";
                    
                    // 日期范围筛选
                    if (dtpStart != null && dtpEnd != null)
                    {
                        DateTime startDate = dtpStart.Value.Date;
                        DateTime endDate = dtpEnd.Value.Date.AddDays(1).AddSeconds(-1); // 包括结束日期的最后一秒
                        filter += $"TransactionDate >= #{startDate:yyyy-MM-dd}# AND TransactionDate <= #{endDate:yyyy-MM-dd HH:mm:ss}#";
                    }
                    
                    // 事务类型筛选
                    if (cmbType != null && cmbType.SelectedIndex > 0)
                    {
                        string selectedType = (cmbType.SelectedItem as dynamic).Value;
                        filter += string.IsNullOrEmpty(filter) ? $"TransactionType = '{selectedType}'" : $" AND TransactionType = '{selectedType}'";
                    }
                    
                    // 物料筛选
                    if (txtProduct != null && !string.IsNullOrEmpty(txtProduct.Text))
                    {
                        filter += string.IsNullOrEmpty(filter) ? 
                            $"(ProductCode LIKE '%{txtProduct.Text}%' OR ProductName LIKE '%{txtProduct.Text}%')" : 
                            $" AND (ProductCode LIKE '%{txtProduct.Text}%' OR ProductName LIKE '%{txtProduct.Text}%')";
                    }
                    
                    // 仓库筛选
                    if (cmbWarehouse != null && cmbWarehouse.SelectedIndex > 0)
                    {
                        int warehouseId = (cmbWarehouse.SelectedItem as dynamic).Value;
                        filter += string.IsNullOrEmpty(filter) ? $"WarehouseId = {warehouseId}" : $" AND WarehouseId = {warehouseId}";
                    }
                    
                    currentFilter = filter;
                    
                    // 应用筛选
                    if (!string.IsNullOrEmpty(filter))
                        dtTransactions.DefaultView.RowFilter = filter;
                    
                    // 绑定数据
                    dgvTransactions.DataSource = dtTransactions;
                    
                    // 更新分页信息
                    Label lblPage = this.Controls.Find("lblPage", true).FirstOrDefault() as Label;
                    if (lblPage != null)
                        lblPage.Text = $"共 {dtTransactions.DefaultView.Count} 条记录";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载库存事务数据时出错");
                MessageBox.Show($"加载数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        
        private void BtnReset_Click(object sender, EventArgs e)
        {
            DateTimePicker dtpStart = this.Controls.Find("dtpStart", true).FirstOrDefault() as DateTimePicker;
            DateTimePicker dtpEnd = this.Controls.Find("dtpEnd", true).FirstOrDefault() as DateTimePicker;
            ComboBox cmbType = this.Controls.Find("cmbType", true).FirstOrDefault() as ComboBox;
            TextBox txtProduct = this.Controls.Find("txtProduct", true).FirstOrDefault() as TextBox;
            ComboBox cmbWarehouse = this.Controls.Find("cmbWarehouse", true).FirstOrDefault() as ComboBox;
            
            if (dtpStart != null) dtpStart.Value = DateTime.Today.AddDays(-30);
            if (dtpEnd != null) dtpEnd.Value = DateTime.Today;
            if (cmbType != null) cmbType.SelectedIndex = 0;
            if (txtProduct != null) txtProduct.Text = string.Empty;
            if (cmbWarehouse != null) cmbWarehouse.SelectedIndex = 0;
            
            LoadData();
        }
        
        private void BtnExport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("导出功能正在开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            MessageBox.Show("打印功能正在开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
} 