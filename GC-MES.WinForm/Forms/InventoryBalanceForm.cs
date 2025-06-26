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
    public partial class InventoryBalanceForm : Form
    {
        private readonly ILogger<InventoryBalanceForm> _logger;
        private readonly IConfiguration _configuration;
        
        // 用于存储当前检索条件
        private string currentFilter = string.Empty;
        private DataTable dtInventory;
        
        public InventoryBalanceForm(ILogger<InventoryBalanceForm> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            InitializeComponent();
            
            // 设置窗体标题
            this.Text = "库存余额";
            
            // 订阅主题变更事件
            ThemeManager.ThemeChanged += ThemeManager_ThemeChanged;
        }
        
        private void ThemeManager_ThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            ThemeManager.ApplyTheme(this);
        }
        
        private void InventoryBalanceForm_Load(object sender, EventArgs e)
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
            lblTitle.Text = "库存余额";
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
            
            // 物料标签
            Label lblProduct = new Label();
            lblProduct.Text = "物料:";
            lblProduct.AutoSize = true;
            lblProduct.Location = new Point(20, 22);
            searchPanel.Controls.Add(lblProduct);
            
            // 物料输入框
            TextBox txtProduct = new TextBox();
            txtProduct.Name = "txtProduct";
            txtProduct.Location = new Point(70, 18);
            txtProduct.Size = new Size(120, 25);
            searchPanel.Controls.Add(txtProduct);
            
            // 批次号标签
            Label lblBatch = new Label();
            lblBatch.Text = "批次号:";
            lblBatch.AutoSize = true;
            lblBatch.Location = new Point(200, 22);
            searchPanel.Controls.Add(lblBatch);
            
            // 批次号输入框
            TextBox txtBatch = new TextBox();
            txtBatch.Name = "txtBatch";
            txtBatch.Location = new Point(260, 18);
            txtBatch.Size = new Size(120, 25);
            searchPanel.Controls.Add(txtBatch);
            
            // 仓库标签
            Label lblWarehouse = new Label();
            lblWarehouse.Text = "仓库:";
            lblWarehouse.AutoSize = true;
            lblWarehouse.Location = new Point(390, 22);
            searchPanel.Controls.Add(lblWarehouse);
            
            // 仓库下拉框
            ComboBox cmbWarehouse = new ComboBox();
            cmbWarehouse.Name = "cmbWarehouse";
            cmbWarehouse.Location = new Point(440, 18);
            cmbWarehouse.Size = new Size(120, 25);
            cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            searchPanel.Controls.Add(cmbWarehouse);
            
            // 库存状态标签
            Label lblStatus = new Label();
            lblStatus.Text = "状态:";
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(570, 22);
            searchPanel.Controls.Add(lblStatus);
            
            // 库存状态下拉框
            ComboBox cmbStatus = new ComboBox();
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Location = new Point(620, 18);
            cmbStatus.Size = new Size(100, 25);
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            searchPanel.Controls.Add(cmbStatus);
            
            // 搜索按钮
            Button btnSearch = new Button();
            btnSearch.Name = "btnSearch";
            btnSearch.Text = "搜索";
            btnSearch.Location = new Point(730, 18);
            btnSearch.Size = new Size(80, 30);
            btnSearch.Click += BtnSearch_Click;
            searchPanel.Controls.Add(btnSearch);
            
            // 重置按钮
            Button btnReset = new Button();
            btnReset.Name = "btnReset";
            btnReset.Text = "重置";
            btnReset.Location = new Point(820, 18);
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
            
            // 查看明细按钮
            Button btnViewHistory = new Button();
            btnViewHistory.Name = "btnViewHistory";
            btnViewHistory.Text = "查看收发明细";
            btnViewHistory.Location = new Point(20, 5);
            btnViewHistory.Size = new Size(120, 30);
            btnViewHistory.Click += BtnViewHistory_Click;
            actionPanel.Controls.Add(btnViewHistory);
            
            // 库存盘点按钮
            Button btnInventoryCheck = new Button();
            btnInventoryCheck.Name = "btnInventoryCheck";
            btnInventoryCheck.Text = "库存盘点";
            btnInventoryCheck.Location = new Point(150, 5);
            btnInventoryCheck.Size = new Size(100, 30);
            btnInventoryCheck.Click += BtnInventoryCheck_Click;
            actionPanel.Controls.Add(btnInventoryCheck);
            
            // 导出按钮
            Button btnExport = new Button();
            btnExport.Name = "btnExport";
            btnExport.Text = "导出";
            btnExport.Location = new Point(260, 5);
            btnExport.Size = new Size(80, 30);
            btnExport.Click += BtnExport_Click;
            actionPanel.Controls.Add(btnExport);
            
            // 打印按钮
            Button btnPrint = new Button();
            btnPrint.Name = "btnPrint";
            btnPrint.Text = "打印";
            btnPrint.Location = new Point(350, 5);
            btnPrint.Size = new Size(80, 30);
            btnPrint.Click += BtnPrint_Click;
            actionPanel.Controls.Add(btnPrint);
            
            // 创建数据网格视图
            DataGridView dgvInventory = new DataGridView();
            dgvInventory.Name = "dgvInventory";
            dgvInventory.Dock = DockStyle.Fill;
            dgvInventory.AllowUserToAddRows = false;
            dgvInventory.AllowUserToDeleteRows = false;
            dgvInventory.ReadOnly = true;
            dgvInventory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventory.RowHeadersWidth = 30;
            dgvInventory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInventory.BackgroundColor = Color.White;
            dgvInventory.BorderStyle = BorderStyle.Fixed3D;
            dgvInventory.RowTemplate.Height = 25;
            dgvInventory.DoubleClick += DgvInventory_DoubleClick;
            this.Controls.Add(dgvInventory);
            
            // 创建底部面板
            Panel bottomPanel = new Panel();
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Height = 40;
            bottomPanel.BackColor = SystemColors.Control;
            this.Controls.Add(bottomPanel);
            
            // 创建统计信息标签
            Label lblSummary = new Label();
            lblSummary.Name = "lblSummary";
            lblSummary.Text = "共 0 条记录";
            lblSummary.Location = new Point(20, 10);
            lblSummary.AutoSize = true;
            bottomPanel.Controls.Add(lblSummary);
            
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
            
            // 获取库存状态下拉框
            ComboBox cmbStatus = this.Controls.Find("cmbStatus", true).FirstOrDefault() as ComboBox;
            if (cmbStatus != null)
            {
                // 添加选项
                cmbStatus.Items.Add(new { Text = "全部", Value = "" });
                cmbStatus.Items.Add(new { Text = "正常", Value = "正常" });
                cmbStatus.Items.Add(new { Text = "冻结", Value = "冻结" });
                cmbStatus.Items.Add(new { Text = "待检验", Value = "待检验" });
                cmbStatus.DisplayMember = "Text";
                cmbStatus.ValueMember = "Value";
                cmbStatus.SelectedIndex = 0;
            }
        }
        
        private void InitializeDataGrid()
        {
            // 获取数据网格视图
            DataGridView dgvInventory = this.Controls.Find("dgvInventory", true).FirstOrDefault() as DataGridView;
            if (dgvInventory != null)
            {
                // 创建列
                dgvInventory.Columns.Add("InventoryId", "ID");
                dgvInventory.Columns.Add("ProductCode", "物料编码");
                dgvInventory.Columns.Add("ProductName", "物料名称");
                dgvInventory.Columns.Add("BatchNo", "批次号");
                dgvInventory.Columns.Add("WarehouseName", "仓库");
                dgvInventory.Columns.Add("LocationName", "库位");
                dgvInventory.Columns.Add("Quantity", "数量");
                dgvInventory.Columns.Add("Unit", "单位");
                dgvInventory.Columns.Add("Status", "状态");
                dgvInventory.Columns.Add("Remark", "备注");
                dgvInventory.Columns.Add("CreateTime", "创建时间");
                dgvInventory.Columns.Add("UpdateTime", "更新时间");
                
                // 设置列的样式
                dgvInventory.Columns["InventoryId"].Width = 60;
                dgvInventory.Columns["InventoryId"].Visible = false;
                dgvInventory.Columns["ProductCode"].Width = 100;
                dgvInventory.Columns["ProductName"].Width = 150;
                dgvInventory.Columns["BatchNo"].Width = 100;
                dgvInventory.Columns["WarehouseName"].Width = 100;
                dgvInventory.Columns["LocationName"].Width = 100;
                dgvInventory.Columns["Quantity"].Width = 80;
                dgvInventory.Columns["Unit"].Width = 60;
                dgvInventory.Columns["Status"].Width = 80;
                dgvInventory.Columns["Remark"].Width = 150;
                dgvInventory.Columns["CreateTime"].Width = 140;
                dgvInventory.Columns["UpdateTime"].Width = 140;
                
                // 设置数值列的格式
                dgvInventory.Columns["Quantity"].DefaultCellStyle.Format = "N2";
                dgvInventory.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                
                // 设置状态列的样式
                dgvInventory.CellFormatting += DgvInventory_CellFormatting;
            }
        }
        
        private void DgvInventory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 8 && e.Value != null) // Status 列
            {
                string status = e.Value.ToString();
                if (status == "正常")
                {
                    e.CellStyle.BackColor = Color.LightGreen;
                }
                else if (status == "冻结")
                {
                    e.CellStyle.BackColor = Color.LightSalmon;
                }
                else if (status == "待检验")
                {
                    e.CellStyle.BackColor = Color.LightYellow;
                }
            }
            else if (e.ColumnIndex == 6 && e.Value != null) // Quantity 列
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal quantity))
                {
                    if (quantity <= 0)
                    {
                        e.CellStyle.ForeColor = Color.Red;
                    }
                }
            }
        }
        
        private void LoadData()
        {
            try
            {
                // 获取数据网格视图和搜索条件
                DataGridView dgvInventory = this.Controls.Find("dgvInventory", true).FirstOrDefault() as DataGridView;
                TextBox txtProduct = this.Controls.Find("txtProduct", true).FirstOrDefault() as TextBox;
                TextBox txtBatch = this.Controls.Find("txtBatch", true).FirstOrDefault() as TextBox;
                ComboBox cmbWarehouse = this.Controls.Find("cmbWarehouse", true).FirstOrDefault() as ComboBox;
                ComboBox cmbStatus = this.Controls.Find("cmbStatus", true).FirstOrDefault() as ComboBox;
                
                if (dgvInventory != null)
                {
                    // 创建模拟数据
                    dtInventory = new DataTable();
                    dtInventory.Columns.Add("InventoryId", typeof(int));
                    dtInventory.Columns.Add("ProductId", typeof(int));
                    dtInventory.Columns.Add("ProductCode", typeof(string));
                    dtInventory.Columns.Add("ProductName", typeof(string));
                    dtInventory.Columns.Add("BatchNo", typeof(string));
                    dtInventory.Columns.Add("WarehouseId", typeof(int));
                    dtInventory.Columns.Add("WarehouseName", typeof(string));
                    dtInventory.Columns.Add("LocationId", typeof(int));
                    dtInventory.Columns.Add("LocationName", typeof(string));
                    dtInventory.Columns.Add("Quantity", typeof(decimal));
                    dtInventory.Columns.Add("Unit", typeof(string));
                    dtInventory.Columns.Add("Status", typeof(string));
                    dtInventory.Columns.Add("Remark", typeof(string));
                    dtInventory.Columns.Add("CreateTime", typeof(DateTime));
                    dtInventory.Columns.Add("UpdateTime", typeof(DateTime?));
                    
                    // 添加模拟数据
                    dtInventory.Rows.Add(1, 1, "M001", "钢板", "B20230601", 1, "原料仓库", 1, "A-01-01", 100, "kg", "正常", "", DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-3));
                    dtInventory.Rows.Add(2, 1, "M001", "钢板", "B20230601", 1, "原料仓库", 4, "A-01-02", 20, "kg", "正常", "", DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-4));
                    dtInventory.Rows.Add(3, 2, "M002", "电子元件", "B20230602", 1, "原料仓库", 2, "A-02-01", 245, "pcs", "正常", "", DateTime.Now.AddDays(-9), DateTime.Now.AddDays(-2));
                    dtInventory.Rows.Add(4, 3, "P001", "成品A", "B20230603", 2, "成品仓库", 3, "B-01-01", 12, "pcs", "正常", "", DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-2));
                    dtInventory.Rows.Add(5, 4, "M003", "包装材料", "B20230604", 2, "成品仓库", 5, "B-02-01", 500, "pcs", "正常", "", DateTime.Now.AddDays(-7), null);
                    dtInventory.Rows.Add(6, 5, "M004", "五金件", "B20230605", 1, "原料仓库", 6, "A-03-01", 1000, "pcs", "正常", "", DateTime.Now.AddDays(-6), null);
                    dtInventory.Rows.Add(7, 6, "M005", "电子元件", "B20230606", 1, "原料仓库", 7, "A-02-02", 300, "pcs", "待检验", "待质检", DateTime.Now.AddDays(-5), null);
                    dtInventory.Rows.Add(8, 7, "P002", "成品B", "B20230607", 2, "成品仓库", 8, "B-01-02", 5, "pcs", "冻结", "客户投诉冻结", DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-1));
                    dtInventory.Rows.Add(9, 8, "S001", "半成品A", "B20230608", 3, "半成品仓库", 9, "C-01-01", 50, "pcs", "正常", "", DateTime.Now.AddDays(-3), null);
                    dtInventory.Rows.Add(10, 9, "S002", "半成品B", "B20230609", 3, "半成品仓库", 10, "C-01-02", 30, "pcs", "正常", "", DateTime.Now.AddDays(-2), null);
                    
                    // 应用筛选条件
                    string filter = "";
                    
                    // 物料筛选
                    if (txtProduct != null && !string.IsNullOrEmpty(txtProduct.Text))
                    {
                        filter += string.IsNullOrEmpty(filter) ? 
                            $"(ProductCode LIKE '%{txtProduct.Text}%' OR ProductName LIKE '%{txtProduct.Text}%')" : 
                            $" AND (ProductCode LIKE '%{txtProduct.Text}%' OR ProductName LIKE '%{txtProduct.Text}%')";
                    }
                    
                    // 批次号筛选
                    if (txtBatch != null && !string.IsNullOrEmpty(txtBatch.Text))
                    {
                        filter += string.IsNullOrEmpty(filter) ? $"BatchNo LIKE '%{txtBatch.Text}%'" : $" AND BatchNo LIKE '%{txtBatch.Text}%'";
                    }
                    
                    // 仓库筛选
                    if (cmbWarehouse != null && cmbWarehouse.SelectedIndex > 0)
                    {
                        int warehouseId = (cmbWarehouse.SelectedItem as dynamic).Value;
                        filter += string.IsNullOrEmpty(filter) ? $"WarehouseId = {warehouseId}" : $" AND WarehouseId = {warehouseId}";
                    }
                    
                    // 状态筛选
                    if (cmbStatus != null && cmbStatus.SelectedIndex > 0)
                    {
                        string selectedStatus = (cmbStatus.SelectedItem as dynamic).Value;
                        filter += string.IsNullOrEmpty(filter) ? $"Status = '{selectedStatus}'" : $" AND Status = '{selectedStatus}'";
                    }
                    
                    currentFilter = filter;
                    
                    // 应用筛选
                    if (!string.IsNullOrEmpty(filter))
                        dtInventory.DefaultView.RowFilter = filter;
                    
                    // 绑定数据
                    dgvInventory.DataSource = dtInventory;
                    
                    // 更新统计信息
                    UpdateSummary();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载库存数据时出错");
                MessageBox.Show($"加载数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void UpdateSummary()
        {
            try
            {
                // 获取统计信息标签和数据网格视图
                Label lblSummary = this.Controls.Find("lblSummary", true).FirstOrDefault() as Label;
                DataGridView dgvInventory = this.Controls.Find("dgvInventory", true).FirstOrDefault() as DataGridView;
                
                if (lblSummary != null && dgvInventory != null && dgvInventory.DataSource != null)
                {
                    // 计算总数量
                    decimal totalQuantity = 0;
                    foreach (DataGridViewRow row in dgvInventory.Rows)
                    {
                        if (row.Cells["Quantity"].Value != null)
                        {
                            totalQuantity += Convert.ToDecimal(row.Cells["Quantity"].Value);
                        }
                    }
                    
                    // 更新统计信息
                    lblSummary.Text = $"共 {dgvInventory.Rows.Count} 条记录，合计数量: {totalQuantity:N2}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新统计信息时出错");
            }
        }
        
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        
        private void BtnReset_Click(object sender, EventArgs e)
        {
            TextBox txtProduct = this.Controls.Find("txtProduct", true).FirstOrDefault() as TextBox;
            TextBox txtBatch = this.Controls.Find("txtBatch", true).FirstOrDefault() as TextBox;
            ComboBox cmbWarehouse = this.Controls.Find("cmbWarehouse", true).FirstOrDefault() as ComboBox;
            ComboBox cmbStatus = this.Controls.Find("cmbStatus", true).FirstOrDefault() as ComboBox;
            
            if (txtProduct != null) txtProduct.Text = string.Empty;
            if (txtBatch != null) txtBatch.Text = string.Empty;
            if (cmbWarehouse != null) cmbWarehouse.SelectedIndex = 0;
            if (cmbStatus != null) cmbStatus.SelectedIndex = 0;
            
            LoadData();
        }
        
        private void BtnViewHistory_Click(object sender, EventArgs e)
        {
            DataGridView dgvInventory = this.Controls.Find("dgvInventory", true).FirstOrDefault() as DataGridView;
            if (dgvInventory != null && dgvInventory.SelectedRows.Count > 0)
            {
                int productId = Convert.ToInt32(dgvInventory.SelectedRows[0].Cells["ProductId"].Value);
                string productCode = dgvInventory.SelectedRows[0].Cells["ProductCode"].Value.ToString();
                string productName = dgvInventory.SelectedRows[0].Cells["ProductName"].Value.ToString();
                string batchNo = dgvInventory.SelectedRows[0].Cells["BatchNo"].Value.ToString();
                int warehouseId = Convert.ToInt32(dgvInventory.SelectedRows[0].Cells["WarehouseId"].Value);
                
                MessageBox.Show($"查看物料 {productCode} {productName} 批次 {batchNo} 的收发明细功能正在开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("请先选择要查看的库存记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void BtnInventoryCheck_Click(object sender, EventArgs e)
        {
            MessageBox.Show("库存盘点功能正在开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        
        private void DgvInventory_DoubleClick(object sender, EventArgs e)
        {
            BtnViewHistory_Click(sender, e);
        }
    }
} 