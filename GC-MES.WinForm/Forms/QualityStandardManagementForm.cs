using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GC_MES.Model;

namespace GC_MES.WinForm.Forms
{
    public partial class QualityStandardManagementForm : Form
    {
        private readonly ILogger<QualityStandardManagementForm> _logger;
        private readonly IConfiguration _configuration;
        
        // 存储当前检索条件
        private string currentFilter = string.Empty;
        private DataTable dtStandards;
        
        public QualityStandardManagementForm(ILogger<QualityStandardManagementForm> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            InitializeComponent();
            
            // 设置窗体标题
            this.Text = "质量标准管理";
            
            // 订阅主题变更事件
            ThemeManager.ThemeChanged += ThemeManager_ThemeChanged;
        }
        
        private void ThemeManager_ThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            ThemeManager.ApplyTheme(this);
        }
        
        private void QualityStandardManagementForm_Load(object sender, EventArgs e)
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
            lblTitle.Text = "质量标准管理";
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
            
            // 标准编号标签
            Label lblCode = new Label();
            lblCode.Text = "标准编号:";
            lblCode.AutoSize = true;
            lblCode.Location = new Point(20, 22);
            searchPanel.Controls.Add(lblCode);
            
            // 标准编号输入框
            TextBox txtCode = new TextBox();
            txtCode.Name = "txtCode";
            txtCode.Location = new Point(110, 18);
            txtCode.Size = new Size(120, 25);
            searchPanel.Controls.Add(txtCode);
            
            // 标准名称标签
            Label lblName = new Label();
            lblName.Text = "标准名称:";
            lblName.AutoSize = true;
            lblName.Location = new Point(240, 22);
            searchPanel.Controls.Add(lblName);
            
            // 标准名称输入框
            TextBox txtName = new TextBox();
            txtName.Name = "txtName";
            txtName.Location = new Point(310, 18);
            txtName.Size = new Size(120, 25);
            searchPanel.Controls.Add(txtName);
            
            // 检验类型标签
            Label lblType = new Label();
            lblType.Text = "检验类型:";
            lblType.AutoSize = true;
            lblType.Location = new Point(440, 22);
            searchPanel.Controls.Add(lblType);
            
            // 检验类型下拉框
            ComboBox cmbType = new ComboBox();
            cmbType.Name = "cmbType";
            cmbType.Location = new Point(510, 18);
            cmbType.Size = new Size(120, 25);
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            searchPanel.Controls.Add(cmbType);
            
            // 搜索按钮
            Button btnSearch = new Button();
            btnSearch.Name = "btnSearch";
            btnSearch.Text = "搜索";
            btnSearch.Location = new Point(640, 18);
            btnSearch.Size = new Size(80, 30);
            btnSearch.Click += BtnSearch_Click;
            searchPanel.Controls.Add(btnSearch);
            
            // 重置按钮
            Button btnReset = new Button();
            btnReset.Name = "btnReset";
            btnReset.Text = "重置";
            btnReset.Location = new Point(730, 18);
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
            
            // 新建按钮
            Button btnAdd = new Button();
            btnAdd.Name = "btnAdd";
            btnAdd.Text = "新建标准";
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
            
            // 查看标准项目按钮
            Button btnViewItems = new Button();
            btnViewItems.Name = "btnViewItems";
            btnViewItems.Text = "查看标准项目";
            btnViewItems.Location = new Point(310, 5);
            btnViewItems.Size = new Size(120, 30);
            btnViewItems.Click += BtnViewItems_Click;
            actionPanel.Controls.Add(btnViewItems);
            
            // 导出按钮
            Button btnExport = new Button();
            btnExport.Name = "btnExport";
            btnExport.Text = "导出";
            btnExport.Location = new Point(440, 5);
            btnExport.Size = new Size(80, 30);
            btnExport.Click += BtnExport_Click;
            actionPanel.Controls.Add(btnExport);
            
            // 创建数据网格视图
            DataGridView dgvStandards = new DataGridView();
            dgvStandards.Name = "dgvStandards";
            dgvStandards.Dock = DockStyle.Fill;
            dgvStandards.AllowUserToAddRows = false;
            dgvStandards.AllowUserToDeleteRows = false;
            dgvStandards.ReadOnly = true;
            dgvStandards.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStandards.RowHeadersWidth = 30;
            dgvStandards.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStandards.BackgroundColor = Color.White;
            dgvStandards.BorderStyle = BorderStyle.Fixed3D;
            dgvStandards.RowTemplate.Height = 25;
            dgvStandards.DoubleClick += DgvStandards_DoubleClick;
            this.Controls.Add(dgvStandards);
            
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
        }
        
        private void InitializeDataGrid()
        {
            // 获取数据网格视图
            DataGridView dgvStandards = this.Controls.Find("dgvStandards", true).FirstOrDefault() as DataGridView;
            if (dgvStandards != null)
            {
                // 创建列
                dgvStandards.Columns.Add("StandardId", "ID");
                dgvStandards.Columns.Add("StandardCode", "标准编号");
                dgvStandards.Columns.Add("StandardName", "标准名称");
                dgvStandards.Columns.Add("InspectionType", "检验类型");
                dgvStandards.Columns.Add("Version", "版本");
                dgvStandards.Columns.Add("ProductName", "适用产品");
                dgvStandards.Columns.Add("OperationName", "适用工序");
                dgvStandards.Columns.Add("AQL", "AQL值");
                dgvStandards.Columns.Add("SamplingPlan", "抽样方案");
                dgvStandards.Columns.Add("IsActive", "是否启用");
                dgvStandards.Columns.Add("CreateBy", "创建人");
                dgvStandards.Columns.Add("CreateTime", "创建时间");
                dgvStandards.Columns.Add("UpdateBy", "更新人");
                dgvStandards.Columns.Add("UpdateTime", "更新时间");
                
                // 设置列的样式
                dgvStandards.Columns["StandardId"].Width = 60;
                dgvStandards.Columns["StandardId"].Visible = false;
                dgvStandards.Columns["StandardCode"].Width = 100;
                dgvStandards.Columns["StandardName"].Width = 150;
                dgvStandards.Columns["InspectionType"].Width = 100;
                dgvStandards.Columns["Version"].Width = 80;
                dgvStandards.Columns["ProductName"].Width = 120;
                dgvStandards.Columns["OperationName"].Width = 120;
                dgvStandards.Columns["AQL"].Width = 80;
                dgvStandards.Columns["SamplingPlan"].Width = 100;
                dgvStandards.Columns["IsActive"].Width = 80;
                dgvStandards.Columns["CreateBy"].Width = 80;
                dgvStandards.Columns["CreateTime"].Width = 140;
                dgvStandards.Columns["UpdateBy"].Width = 80;
                dgvStandards.Columns["UpdateTime"].Width = 140;
                
                // 设置默认排序列
                dgvStandards.Sort(dgvStandards.Columns["CreateTime"], ListSortDirection.Descending);
            }
        }
        
        private void LoadData()
        {
            try
            {
                // 获取数据网格视图和搜索条件
                DataGridView dgvStandards = this.Controls.Find("dgvStandards", true).FirstOrDefault() as DataGridView;
                TextBox txtCode = this.Controls.Find("txtCode", true).FirstOrDefault() as TextBox;
                TextBox txtName = this.Controls.Find("txtName", true).FirstOrDefault() as TextBox;
                ComboBox cmbType = this.Controls.Find("cmbType", true).FirstOrDefault() as ComboBox;
                
                if (dgvStandards != null)
                {
                    // 创建模拟数据
                    dtStandards = new DataTable();
                    dtStandards.Columns.Add("StandardId", typeof(int));
                    dtStandards.Columns.Add("StandardCode", typeof(string));
                    dtStandards.Columns.Add("StandardName", typeof(string));
                    dtStandards.Columns.Add("InspectionType", typeof(string));
                    dtStandards.Columns.Add("Version", typeof(string));
                    dtStandards.Columns.Add("ProductName", typeof(string));
                    dtStandards.Columns.Add("OperationName", typeof(string));
                    dtStandards.Columns.Add("AQL", typeof(decimal));
                    dtStandards.Columns.Add("SamplingPlan", typeof(string));
                    dtStandards.Columns.Add("IsActive", typeof(bool));
                    dtStandards.Columns.Add("CreateBy", typeof(string));
                    dtStandards.Columns.Add("CreateTime", typeof(DateTime));
                    dtStandards.Columns.Add("UpdateBy", typeof(string));
                    dtStandards.Columns.Add("UpdateTime", typeof(DateTime?));
                    
                    // 添加模拟数据
                    dtStandards.Rows.Add(1, "STD-IQC-001", "PCB板来料检验标准", "IQC", "V1.0", "电路板", null, 2.5M, "GB2828-1", true, "admin", DateTime.Now.AddDays(-30), "admin", DateTime.Now.AddDays(-15));
                    dtStandards.Rows.Add(2, "STD-IPQC-001", "PCB焊接过程检验标准", "IPQC", "V1.0", "电路板", "焊接", 1.5M, "GB2828-1", true, "admin", DateTime.Now.AddDays(-28), null, null);
                    dtStandards.Rows.Add(3, "STD-FQC-001", "控制器成品检验标准", "FQC", "V1.0", "温控器", null, 1.0M, "GB2828-1", true, "admin", DateTime.Now.AddDays(-25), "admin", DateTime.Now.AddDays(-10));
                    dtStandards.Rows.Add(4, "STD-IQC-002", "温控元器件来料检验标准", "IQC", "V1.0", "温度传感器", null, 2.5M, "GB2828-1", true, "admin", DateTime.Now.AddDays(-20), null, null);
                    dtStandards.Rows.Add(5, "STD-IPQC-002", "装配过程检验标准", "IPQC", "V1.0", "温控器", "装配", 1.5M, "GB2828-1", true, "admin", DateTime.Now.AddDays(-18), null, null);
                    dtStandards.Rows.Add(6, "STD-FQC-002", "温控器外观检验标准", "FQC", "V1.0", "温控器", null, 1.0M, "GB2828-1", false, "admin", DateTime.Now.AddDays(-15), "admin", DateTime.Now.AddDays(-5));
                    
                    // 应用筛选条件
                    string filter = "";
                    if (txtCode != null && !string.IsNullOrEmpty(txtCode.Text))
                        filter += string.IsNullOrEmpty(filter) ? $"StandardCode LIKE '%{txtCode.Text}%'" : $" AND StandardCode LIKE '%{txtCode.Text}%'";
                    
                    if (txtName != null && !string.IsNullOrEmpty(txtName.Text))
                        filter += string.IsNullOrEmpty(filter) ? $"StandardName LIKE '%{txtName.Text}%'" : $" AND StandardName LIKE '%{txtName.Text}%'";
                    
                    if (cmbType != null && cmbType.SelectedIndex > 0)
                    {
                        string selectedType = (cmbType.SelectedItem as dynamic).Value;
                        filter += string.IsNullOrEmpty(filter) ? $"InspectionType = '{selectedType}'" : $" AND InspectionType = '{selectedType}'";
                    }
                    
                    currentFilter = filter;
                    
                    // 应用筛选
                    if (!string.IsNullOrEmpty(filter))
                        dtStandards.DefaultView.RowFilter = filter;
                        
                    // 绑定数据
                    dgvStandards.DataSource = dtStandards;
                    
                    // 更新分页信息
                    Label lblPage = this.Controls.Find("lblPage", true).FirstOrDefault() as Label;
                    if (lblPage != null)
                        lblPage.Text = $"共 {dtStandards.DefaultView.Count} 条记录";
                        
                    // 设置按钮状态
                    UpdateButtonStates();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载质量标准数据时出错");
                MessageBox.Show($"加载数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void UpdateButtonStates()
        {
            DataGridView dgvStandards = this.Controls.Find("dgvStandards", true).FirstOrDefault() as DataGridView;
            Button btnEdit = this.Controls.Find("btnEdit", true).FirstOrDefault() as Button;
            Button btnDelete = this.Controls.Find("btnDelete", true).FirstOrDefault() as Button;
            Button btnViewItems = this.Controls.Find("btnViewItems", true).FirstOrDefault() as Button;
            
            bool hasSelection = dgvStandards != null && dgvStandards.SelectedRows.Count > 0;
            
            if (btnEdit != null) btnEdit.Enabled = hasSelection;
            if (btnDelete != null) btnDelete.Enabled = hasSelection;
            if (btnViewItems != null) btnViewItems.Enabled = hasSelection;
        }
        
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        
        private void BtnReset_Click(object sender, EventArgs e)
        {
            TextBox txtCode = this.Controls.Find("txtCode", true).FirstOrDefault() as TextBox;
            TextBox txtName = this.Controls.Find("txtName", true).FirstOrDefault() as TextBox;
            ComboBox cmbType = this.Controls.Find("cmbType", true).FirstOrDefault() as ComboBox;
            
            if (txtCode != null) txtCode.Text = string.Empty;
            if (txtName != null) txtName.Text = string.Empty;
            if (cmbType != null) cmbType.SelectedIndex = 0;
            
            LoadData();
        }
        
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("新建质量标准功能正在开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // 后续可以添加打开质量标准编辑表单的代码
            // QualityStandardEditForm editForm = Program.ServiceProvider.GetRequiredService<QualityStandardEditForm>();
            // editForm.ShowDialog();
            // if (editForm.DialogResult == DialogResult.OK)
            // {
            //     LoadData();
            // }
        }
        
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            DataGridView dgvStandards = this.Controls.Find("dgvStandards", true).FirstOrDefault() as DataGridView;
            if (dgvStandards != null && dgvStandards.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvStandards.SelectedRows[0].Cells["StandardId"].Value);
                string code = dgvStandards.SelectedRows[0].Cells["StandardCode"].Value.ToString();
                
                MessageBox.Show($"编辑质量标准: {code} 功能正在开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("请先选择要编辑的质量标准", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DataGridView dgvStandards = this.Controls.Find("dgvStandards", true).FirstOrDefault() as DataGridView;
            if (dgvStandards != null && dgvStandards.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvStandards.SelectedRows[0].Cells["StandardId"].Value);
                string code = dgvStandards.SelectedRows[0].Cells["StandardCode"].Value.ToString();
                
                DialogResult result = MessageBox.Show($"确定要删除质量标准: {code} 吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // 这里添加删除逻辑
                    // 模拟删除成功
                    dgvStandards.Rows.Remove(dgvStandards.SelectedRows[0]);
                    MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("请先选择要删除的质量标准", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void BtnViewItems_Click(object sender, EventArgs e)
        {
            DataGridView dgvStandards = this.Controls.Find("dgvStandards", true).FirstOrDefault() as DataGridView;
            if (dgvStandards != null && dgvStandards.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvStandards.SelectedRows[0].Cells["StandardId"].Value);
                string code = dgvStandards.SelectedRows[0].Cells["StandardCode"].Value.ToString();
                
                MessageBox.Show($"查看质量标准项目: {code} 功能正在开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("请先选择要查看的质量标准", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        
        private void DgvStandards_DoubleClick(object sender, EventArgs e)
        {
            BtnEdit_Click(sender, e);
        }
    }
}
