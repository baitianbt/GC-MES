using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GC_MES.Model;

namespace GC_MES.WinForm.Forms
{
    public partial class QualityInspectionManagementForm : Form
    {
        private readonly ILogger<QualityInspectionManagementForm> _logger;
        private readonly IConfiguration _configuration;
        
        // 用于存储当前检索条件
        private string currentFilter = string.Empty;
        private DataTable dtInspections;
        
        public QualityInspectionManagementForm(ILogger<QualityInspectionManagementForm> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            InitializeComponent();
            
            // 设置窗体标题
            this.Text = "质量检验管理";
            
            // 订阅主题变更事件
            ThemeManager.ThemeChanged += ThemeManager_ThemeChanged;
        }
        
        private void ThemeManager_ThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            ThemeManager.ApplyTheme(this);
        }
        
        private void QualityInspectionManagementForm_Load(object sender, EventArgs e)
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
            lblTitle.Text = "质量检验管理";
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
            
            // 检验单号标签
            Label lblCode = new Label();
            lblCode.Text = "检验单号:";
            lblCode.AutoSize = true;
            lblCode.Location = new Point(20, 22);
            searchPanel.Controls.Add(lblCode);
            
            // 检验单号输入框
            TextBox txtCode = new TextBox();
            txtCode.Name = "txtCode";
            txtCode.Location = new Point(100, 18);
            txtCode.Size = new Size(120, 25);
            searchPanel.Controls.Add(txtCode);
            
            // 检验类型标签
            Label lblType = new Label();
            lblType.Text = "检验类型:";
            lblType.AutoSize = true;
            lblType.Location = new Point(230, 22);
            searchPanel.Controls.Add(lblType);
            
            // 检验类型下拉框
            ComboBox cmbType = new ComboBox();
            cmbType.Name = "cmbType";
            cmbType.Location = new Point(300, 18);
            cmbType.Size = new Size(120, 25);
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            searchPanel.Controls.Add(cmbType);
            
            // 产品标签
            Label lblProduct = new Label();
            lblProduct.Text = "产品:";
            lblProduct.AutoSize = true;
            lblProduct.Location = new Point(430, 22);
            searchPanel.Controls.Add(lblProduct);
            
            // 产品输入框
            TextBox txtProduct = new TextBox();
            txtProduct.Name = "txtProduct";
            txtProduct.Location = new Point(480, 18);
            txtProduct.Size = new Size(120, 25);
            searchPanel.Controls.Add(txtProduct);
            
            // 检验结果标签
            Label lblResult = new Label();
            lblResult.Text = "检验结果:";
            lblResult.AutoSize = true;
            lblResult.Location = new Point(610, 22);
            searchPanel.Controls.Add(lblResult);
            
            // 检验结果下拉框
            ComboBox cmbResult = new ComboBox();
            cmbResult.Name = "cmbResult";
            cmbResult.Location = new Point(680, 18);
            cmbResult.Size = new Size(100, 25);
            cmbResult.DropDownStyle = ComboBoxStyle.DropDownList;
            searchPanel.Controls.Add(cmbResult);
            
            // 搜索按钮
            Button btnSearch = new Button();
            btnSearch.Name = "btnSearch";
            btnSearch.Text = "搜索";
            btnSearch.Location = new Point(790, 18);
            btnSearch.Size = new Size(80, 30);
            btnSearch.Click += BtnSearch_Click;
            searchPanel.Controls.Add(btnSearch);
            
            // 重置按钮
            Button btnReset = new Button();
            btnReset.Name = "btnReset";
            btnReset.Text = "重置";
            btnReset.Location = new Point(880, 18);
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
            
            // 新建检验按钮
            Button btnAdd = new Button();
            btnAdd.Name = "btnAdd";
            btnAdd.Text = "新建检验";
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
            
            // 检验项目按钮
            Button btnItems = new Button();
            btnItems.Name = "btnItems";
            btnItems.Text = "检验项目";
            btnItems.Location = new Point(310, 5);
            btnItems.Size = new Size(100, 30);
            btnItems.Click += BtnItems_Click;
            actionPanel.Controls.Add(btnItems);
            
            // 生成不合格品单按钮
            Button btnNonconforming = new Button();
            btnNonconforming.Name = "btnNonconforming";
            btnNonconforming.Text = "生成不合格品单";
            btnNonconforming.Location = new Point(420, 5);
            btnNonconforming.Size = new Size(120, 30);
            btnNonconforming.Click += BtnNonconforming_Click;
            actionPanel.Controls.Add(btnNonconforming);
            
            // 打印按钮
            Button btnPrint = new Button();
            btnPrint.Name = "btnPrint";
            btnPrint.Text = "打印";
            btnPrint.Location = new Point(550, 5);
            btnPrint.Size = new Size(80, 30);
            btnPrint.Click += BtnPrint_Click;
            actionPanel.Controls.Add(btnPrint);
            
            // 导出按钮
            Button btnExport = new Button();
            btnExport.Name = "btnExport";
            btnExport.Text = "导出";
            btnExport.Location = new Point(640, 5);
            btnExport.Size = new Size(80, 30);
            btnExport.Click += BtnExport_Click;
            actionPanel.Controls.Add(btnExport);
            
            // 创建数据网格视图
            DataGridView dgvInspections = new DataGridView();
            dgvInspections.Name = "dgvInspections";
            dgvInspections.Dock = DockStyle.Fill;
            dgvInspections.AllowUserToAddRows = false;
            dgvInspections.AllowUserToDeleteRows = false;
            dgvInspections.ReadOnly = true;
            dgvInspections.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInspections.RowHeadersWidth = 30;
            dgvInspections.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInspections.BackgroundColor = Color.White;
            dgvInspections.BorderStyle = BorderStyle.Fixed3D;
            dgvInspections.RowTemplate.Height = 25;
            dgvInspections.DoubleClick += DgvInspections_DoubleClick;
            this.Controls.Add(dgvInspections);
            
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
            // 获取检验类型下拉框
            ComboBox cmbType = this.Controls.Find("cmbType", true).FirstOrDefault() as ComboBox;
            if (cmbType != null)
            {
                // 添加选项
                cmbType.Items.Add(new { Text = "全部", Value = "" });
                cmbType.Items.Add(new { Text = "来料检验(IQC)", Value = "IQC" });
                cmbType.Items.Add(new { Text = "过程检验(IPQC)", Value = "IPQC" });
                cmbType.Items.Add(new { Text = "成品检验(FQC)", Value = "FQC" });
                cmbType.DisplayMember = "Text";
                cmbType.ValueMember = "Value";
                cmbType.SelectedIndex = 0;
            }
            
            // 获取检验结果下拉框
            ComboBox cmbResult = this.Controls.Find("cmbResult", true).FirstOrDefault() as ComboBox;
            if (cmbResult != null)
            {
                // 添加选项
                cmbResult.Items.Add(new { Text = "全部", Value = "" });
                cmbResult.Items.Add(new { Text = "合格", Value = "合格" });
                cmbResult.Items.Add(new { Text = "不合格", Value = "不合格" });
                cmbResult.Items.Add(new { Text = "让步接收", Value = "让步接收" });
                cmbResult.DisplayMember = "Text";
                cmbResult.ValueMember = "Value";
                cmbResult.SelectedIndex = 0;
            }
        }
        
        private void InitializeDataGrid()
        {
            // 获取数据网格视图
            DataGridView dgvInspections = this.Controls.Find("dgvInspections", true).FirstOrDefault() as DataGridView;
            if (dgvInspections != null)
            {
                // 创建列
                dgvInspections.Columns.Add("InspectionId", "ID");
                dgvInspections.Columns.Add("InspectionCode", "检验单号");
                dgvInspections.Columns.Add("InspectionType", "检验类型");
                dgvInspections.Columns.Add("ProductCode", "产品编号");
                dgvInspections.Columns.Add("ProductName", "产品名称");
                dgvInspections.Columns.Add("BatchNo", "批次号");
                dgvInspections.Columns.Add("Quantity", "检验数量");
                dgvInspections.Columns.Add("PassedQuantity", "合格数量");
                dgvInspections.Columns.Add("FailedQuantity", "不合格数量");
                dgvInspections.Columns.Add("Result", "检验结果");
                dgvInspections.Columns.Add("Status", "检验状态");
                dgvInspections.Columns.Add("InspectionDate", "检验日期");
                dgvInspections.Columns.Add("Inspector", "检验人");
                dgvInspections.Columns.Add("CreateTime", "创建时间");
                
                // 设置列的样式
                dgvInspections.Columns["InspectionId"].Width = 60;
                dgvInspections.Columns["InspectionId"].Visible = false;
                dgvInspections.Columns["InspectionCode"].Width = 100;
                dgvInspections.Columns["InspectionType"].Width = 100;
                dgvInspections.Columns["ProductCode"].Width = 100;
                dgvInspections.Columns["ProductName"].Width = 120;
                dgvInspections.Columns["BatchNo"].Width = 100;
                dgvInspections.Columns["Quantity"].Width = 80;
                dgvInspections.Columns["PassedQuantity"].Width = 80;
                dgvInspections.Columns["FailedQuantity"].Width = 80;
                dgvInspections.Columns["Result"].Width = 80;
                dgvInspections.Columns["Status"].Width = 80;
                dgvInspections.Columns["InspectionDate"].Width = 100;
                dgvInspections.Columns["Inspector"].Width = 80;
                dgvInspections.Columns["CreateTime"].Width = 140;
                
                // 设置默认排序列
                dgvInspections.Sort(dgvInspections.Columns["CreateTime"], ListSortDirection.Descending);
            }
        }
        
        private void LoadData()
        {
            try
            {
                // 获取数据网格视图和搜索条件
                DataGridView dgvInspections = this.Controls.Find("dgvInspections", true).FirstOrDefault() as DataGridView;
                TextBox txtCode = this.Controls.Find("txtCode", true).FirstOrDefault() as TextBox;
                TextBox txtProduct = this.Controls.Find("txtProduct", true).FirstOrDefault() as TextBox;
                ComboBox cmbType = this.Controls.Find("cmbType", true).FirstOrDefault() as ComboBox;
                ComboBox cmbResult = this.Controls.Find("cmbResult", true).FirstOrDefault() as ComboBox;
                
                if (dgvInspections != null)
                {
                    // 创建模拟数据
                    dtInspections = new DataTable();
                    dtInspections.Columns.Add("InspectionId", typeof(int));
                    dtInspections.Columns.Add("InspectionCode", typeof(string));
                    dtInspections.Columns.Add("InspectionType", typeof(string));
                    dtInspections.Columns.Add("ProductCode", typeof(string));
                    dtInspections.Columns.Add("ProductName", typeof(string));
                    dtInspections.Columns.Add("BatchNo", typeof(string));
                    dtInspections.Columns.Add("Quantity", typeof(decimal));
                    dtInspections.Columns.Add("PassedQuantity", typeof(decimal));
                    dtInspections.Columns.Add("FailedQuantity", typeof(decimal));
                    dtInspections.Columns.Add("Result", typeof(string));
                    dtInspections.Columns.Add("Status", typeof(string));
                    dtInspections.Columns.Add("InspectionDate", typeof(DateTime));
                    dtInspections.Columns.Add("Inspector", typeof(string));
                    dtInspections.Columns.Add("CreateTime", typeof(DateTime));
                    
                    // 添加模拟数据
                    dtInspections.Rows.Add(1, "QI-2023-001", "IQC", "P001", "电路板", "B20230601", 100, 95, 5, "合格", "已完成", DateTime.Now.AddDays(-10), "张三", DateTime.Now.AddDays(-10));
                    dtInspections.Rows.Add(2, "QI-2023-002", "IPQC", "P001", "电路板", "B20230601", 50, 50, 0, "合格", "已完成", DateTime.Now.AddDays(-9), "李四", DateTime.Now.AddDays(-9));
                    dtInspections.Rows.Add(3, "QI-2023-003", "IQC", "P002", "温度传感器", "B20230602", 200, 198, 2, "合格", "已完成", DateTime.Now.AddDays(-8), "张三", DateTime.Now.AddDays(-8));
                    dtInspections.Rows.Add(4, "QI-2023-004", "FQC", "P003", "控制器", "B20230603", 50, 45, 5, "不合格", "已完成", DateTime.Now.AddDays(-7), "王五", DateTime.Now.AddDays(-7));
                    dtInspections.Rows.Add(5, "QI-2023-005", "IPQC", "P003", "控制器", "B20230603", 100, 90, 10, "不合格", "已完成", DateTime.Now.AddDays(-6), "李四", DateTime.Now.AddDays(-6));
                    dtInspections.Rows.Add(6, "QI-2023-006", "FQC", "P002", "温度传感器", "B20230604", 150, 145, 5, "让步接收", "已完成", DateTime.Now.AddDays(-5), "王五", DateTime.Now.AddDays(-5));
                    dtInspections.Rows.Add(7, "QI-2023-007", "IQC", "P002", "温度传感器", "B20230605", 80, 0, 0, "", "检验中", DateTime.Now.AddDays(-1), "张三", DateTime.Now.AddDays(-2));
                    dtInspections.Rows.Add(8, "QI-2023-008", "IPQC", "P003", "控制器", "B20230606", 60, 0, 0, "", "待检验", null, null, DateTime.Now.AddDays(-1));
                    
                    // 应用筛选条件
                    string filter = "";
                    if (txtCode != null && !string.IsNullOrEmpty(txtCode.Text))
                        filter += string.IsNullOrEmpty(filter) ? $"InspectionCode LIKE '%{txtCode.Text}%'" : $" AND InspectionCode LIKE '%{txtCode.Text}%'";
                    
                    if (txtProduct != null && !string.IsNullOrEmpty(txtProduct.Text))
                        filter += string.IsNullOrEmpty(filter) ? 
                            $"(ProductName LIKE '%{txtProduct.Text}%' OR ProductCode LIKE '%{txtProduct.Text}%')" : 
                            $" AND (ProductName LIKE '%{txtProduct.Text}%' OR ProductCode LIKE '%{txtProduct.Text}%')";
                    
                    if (cmbType != null && cmbType.SelectedIndex > 0)
                    {
                        string selectedType = (cmbType.SelectedItem as dynamic).Value;
                        filter += string.IsNullOrEmpty(filter) ? $"InspectionType = '{selectedType}'" : $" AND InspectionType = '{selectedType}'";
                    }
                    
                    if (cmbResult != null && cmbResult.SelectedIndex > 0)
                    {
                        string selectedResult = (cmbResult.SelectedItem as dynamic).Value;
                        filter += string.IsNullOrEmpty(filter) ? $"Result = '{selectedResult}'" : $" AND Result = '{selectedResult}'";
                    }
                    
                    currentFilter = filter;
                    
                    // 应用筛选
                    if (!string.IsNullOrEmpty(filter))
                        dtInspections.DefaultView.RowFilter = filter;
                    
                    // 绑定数据
                    dgvInspections.DataSource = dtInspections;
                    
                    // 更新分页信息
                    Label lblPage = this.Controls.Find("lblPage", true).FirstOrDefault() as Label;
                    if (lblPage != null)
                        lblPage.Text = $"共 {dtInspections.DefaultView.Count} 条记录";
                    
                    // 设置按钮状态
                    UpdateButtonStates();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载质量检验数据时出错");
                MessageBox.Show($"加载数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void UpdateButtonStates()
        {
            DataGridView dgvInspections = this.Controls.Find("dgvInspections", true).FirstOrDefault() as DataGridView;
            Button btnEdit = this.Controls.Find("btnEdit", true).FirstOrDefault() as Button;
            Button btnDelete = this.Controls.Find("btnDelete", true).FirstOrDefault() as Button;
            Button btnItems = this.Controls.Find("btnItems", true).FirstOrDefault() as Button;
            Button btnNonconforming = this.Controls.Find("btnNonconforming", true).FirstOrDefault() as Button;
            
            bool hasSelection = dgvInspections != null && dgvInspections.SelectedRows.Count > 0;
            
            if (btnEdit != null) btnEdit.Enabled = hasSelection;
            if (btnDelete != null) btnDelete.Enabled = hasSelection;
            if (btnItems != null) btnItems.Enabled = hasSelection;
            
            if (btnNonconforming != null && hasSelection)
            {
                string result = dgvInspections.SelectedRows[0].Cells["Result"].Value.ToString();
                string status = dgvInspections.SelectedRows[0].Cells["Status"].Value.ToString();
                decimal failedQty = Convert.ToDecimal(dgvInspections.SelectedRows[0].Cells["FailedQuantity"].Value);
                
                btnNonconforming.Enabled = status == "已完成" && (result == "不合格" || result == "让步接收") && failedQty > 0;
            }
            else if (btnNonconforming != null)
            {
                btnNonconforming.Enabled = false;
            }
        }
        
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        
        private void BtnReset_Click(object sender, EventArgs e)
        {
            TextBox txtCode = this.Controls.Find("txtCode", true).FirstOrDefault() as TextBox;
            TextBox txtProduct = this.Controls.Find("txtProduct", true).FirstOrDefault() as TextBox;
            ComboBox cmbType = this.Controls.Find("cmbType", true).FirstOrDefault() as ComboBox;
            ComboBox cmbResult = this.Controls.Find("cmbResult", true).FirstOrDefault() as ComboBox;
            
            if (txtCode != null) txtCode.Text = string.Empty;
            if (txtProduct != null) txtProduct.Text = string.Empty;
            if (cmbType != null) cmbType.SelectedIndex = 0;
            if (cmbResult != null) cmbResult.SelectedIndex = 0;
            
            LoadData();
        }
        
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("新建质量检验功能正在开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // 后续可以添加打开质量检验编辑表单的代码
            // QualityInspectionEditForm editForm = Program.ServiceProvider.GetRequiredService<QualityInspectionEditForm>();
            // editForm.ShowDialog();
            // if (editForm.DialogResult == DialogResult.OK)
            // {
            //     LoadData();
            // }
        }
        
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            DataGridView dgvInspections = this.Controls.Find("dgvInspections", true).FirstOrDefault() as DataGridView;
            if (dgvInspections != null && dgvInspections.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvInspections.SelectedRows[0].Cells["InspectionId"].Value);
                string code = dgvInspections.SelectedRows[0].Cells["InspectionCode"].Value.ToString();
                string status = dgvInspections.SelectedRows[0].Cells["Status"].Value.ToString();
                
                if (status == "已完成")
                {
                    MessageBox.Show("已完成的检验记录不能编辑！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                MessageBox.Show($"编辑质量检验: {code} 功能正在开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("请先选择要编辑的质量检验", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DataGridView dgvInspections = this.Controls.Find("dgvInspections", true).FirstOrDefault() as DataGridView;
            if (dgvInspections != null && dgvInspections.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvInspections.SelectedRows[0].Cells["InspectionId"].Value);
                string code = dgvInspections.SelectedRows[0].Cells["InspectionCode"].Value.ToString();
                string status = dgvInspections.SelectedRows[0].Cells["Status"].Value.ToString();
                
                if (status == "已完成")
                {
                    MessageBox.Show("已完成的检验记录不能删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                DialogResult result = MessageBox.Show($"确定要删除质量检验: {code} 吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // 这里添加删除逻辑
                    // 模拟删除成功
                    dgvInspections.Rows.Remove(dgvInspections.SelectedRows[0]);
                    MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("请先选择要删除的质量检验", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void BtnItems_Click(object sender, EventArgs e)
        {
            DataGridView dgvInspections = this.Controls.Find("dgvInspections", true).FirstOrDefault() as DataGridView;
            if (dgvInspections != null && dgvInspections.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvInspections.SelectedRows[0].Cells["InspectionId"].Value);
                string code = dgvInspections.SelectedRows[0].Cells["InspectionCode"].Value.ToString();
                
                MessageBox.Show($"查看检验项目: {code} 功能正在开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("请先选择要查看的质量检验", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void BtnNonconforming_Click(object sender, EventArgs e)
        {
            DataGridView dgvInspections = this.Controls.Find("dgvInspections", true).FirstOrDefault() as DataGridView;
            if (dgvInspections != null && dgvInspections.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvInspections.SelectedRows[0].Cells["InspectionId"].Value);
                string code = dgvInspections.SelectedRows[0].Cells["InspectionCode"].Value.ToString();
                string status = dgvInspections.SelectedRows[0].Cells["Status"].Value.ToString();
                string result = dgvInspections.SelectedRows[0].Cells["Result"].Value.ToString();
                decimal failedQty = Convert.ToDecimal(dgvInspections.SelectedRows[0].Cells["FailedQuantity"].Value);
                
                if (status != "已完成" || !(result == "不合格" || result == "让步接收") || failedQty <= 0)
                {
                    MessageBox.Show("只能为已完成且有不合格品的检验记录生成不合格品单！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                MessageBox.Show($"为检验: {code} 生成不合格品单功能正在开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("请先选择要生成不合格品单的质量检验", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            DataGridView dgvInspections = this.Controls.Find("dgvInspections", true).FirstOrDefault() as DataGridView;
            if (dgvInspections != null && dgvInspections.SelectedRows.Count > 0)
            {
                string code = dgvInspections.SelectedRows[0].Cells["InspectionCode"].Value.ToString();
                MessageBox.Show($"打印检验单: {code} 功能正在开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("请先选择要打印的质量检验单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        
        private void DgvInspections_DoubleClick(object sender, EventArgs e)
        {
            BtnEdit_Click(sender, e);
        }
    }
} 