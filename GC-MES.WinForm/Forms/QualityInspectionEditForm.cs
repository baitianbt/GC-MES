using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GC_MES.DAL.DbContexts;
using GC_MES.Model;

namespace GC_MES.WinForm.Forms
{
    public partial class QualityInspectionEditForm : Form
    {
        private readonly ILogger<QualityInspectionEditForm> _logger;
        private readonly IConfiguration _configuration;
        private readonly MesDbContext _dbContext;
        
        // 当前编辑的检验单
        private QualityInspection _currentInspection;
        private bool _isNewInspection = true;
        
        // 检验项目列表
        private List<QualityInspectionItem> _inspectionItems = new List<QualityInspectionItem>();

        public QualityInspectionEditForm(ILogger<QualityInspectionEditForm> logger, IConfiguration configuration, MesDbContext dbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
            
            InitializeComponent();
            
            // 应用当前主题
            ThemeManager.ApplyTheme(this);
        }

        /// <summary>
        /// 设置当前编辑的检验单
        /// </summary>
        /// <param name="inspection">要编辑的检验单，如果为null则表示新增检验单</param>
        public void SetInspection(QualityInspection inspection)
        {
            if (inspection == null)
            {
                // 新增检验单
                _currentInspection = new QualityInspection
                {
                    InspectionDate = DateTime.Now,
                    Status = "待检验",
                    CreateTime = DateTime.Now,
                    CreateBy = "admin" // 实际应用中应使用当前登录用户
                };
                _isNewInspection = true;
                
                // 更新窗体标题
                this.Text = "新增质量检验单";
            }
            else
            {
                // 编辑检验单
                _currentInspection = inspection;
                _isNewInspection = false;
                
                // 更新窗体标题
                this.Text = $"编辑质量检验单 - {inspection.InspectionCode}";
                
                // 加载检验项目
                LoadInspectionItems();
            }
        }

        private void QualityInspectionEditForm_Load(object sender, EventArgs e)
        {
            try
            {
                // 初始化检验类型下拉框
                InitializeInspectionTypeComboBox();
                
                // 初始化检验结果下拉框
                InitializeResultComboBox();
                
                // 初始化检验状态下拉框
                InitializeStatusComboBox();
                
                // 加载产品列表
                LoadProducts();
                
                // 如果是编辑，则填充表单数据
                if (!_isNewInspection)
                {
                    FillInspectionData();
                }
                else
                {
                    // 新增时，生成检验单号
                    GenerateInspectionCode();
                    
                    // 设置默认值
                    dtpInspectionDate.Value = DateTime.Now;
                    cmbStatus.SelectedItem = "待检验";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载质量检验编辑窗体时出错");
                MessageBox.Show($"加载质量检验编辑窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void InitializeInspectionTypeComboBox()
        {
            cmbInspectionType.Items.Clear();
            cmbInspectionType.Items.Add("IQC");
            cmbInspectionType.Items.Add("IPQC");
            cmbInspectionType.Items.Add("FQC");
            
            if (_isNewInspection)
            {
                cmbInspectionType.SelectedIndex = 0;
            }
        }
        
        private void InitializeResultComboBox()
        {
            cmbResult.Items.Clear();
            cmbResult.Items.Add("");
            cmbResult.Items.Add("合格");
            cmbResult.Items.Add("不合格");
            cmbResult.Items.Add("让步接收");
            
            if (_isNewInspection)
            {
                cmbResult.SelectedIndex = 0;
            }
        }
        
        private void InitializeStatusComboBox()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("待检验");
            cmbStatus.Items.Add("检验中");
            cmbStatus.Items.Add("已完成");
            
            if (_isNewInspection)
            {
                cmbStatus.SelectedIndex = 0;
            }
        }
        
        private void LoadProducts()
        {
            try
            {
                // 加载所有产品
                var products = _dbContext.Products
                    .Where(p => p.IsActive)
                    .OrderBy(p => p.ProductCode)
                    .ToList();
                
                // 设置数据源
                cmbProduct.DataSource = products;
                cmbProduct.DisplayMember = "ProductName";
                cmbProduct.ValueMember = "ProductId";
                
                // 清空选择
                cmbProduct.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载产品列表时出错");
                throw;
            }
        }
        
        private void GenerateInspectionCode()
        {
            try
            {
                // 生成检验单号：前缀 + 日期 + 4位序号
                string prefix = string.Empty;
                
                // 根据检验类型设置前缀
                if (cmbInspectionType.SelectedItem != null)
                {
                    prefix = cmbInspectionType.SelectedItem.ToString();
                }
                else
                {
                    prefix = "QC";
                }
                
                string dateStr = DateTime.Now.ToString("yyyyMMdd");
                
                // 查询当天最大序号
                string codePattern = $"{prefix}{dateStr}";
                int maxSeq = 0;
                
                var maxCode = _dbContext.QualityInspections
                    .Where(q => q.InspectionCode.StartsWith(codePattern))
                    .OrderByDescending(q => q.InspectionCode)
                    .Select(q => q.InspectionCode)
                    .FirstOrDefault();
                
                if (maxCode != null && maxCode.Length >= codePattern.Length + 4)
                {
                    string seqStr = maxCode.Substring(codePattern.Length);
                    int.TryParse(seqStr, out maxSeq);
                }
                
                // 生成新的检验单号
                string newCode = $"{prefix}{dateStr}{(maxSeq + 1).ToString("D4")}";
                txtInspectionCode.Text = newCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "生成检验单号时出错");
                throw;
            }
        }
        
        private void FillInspectionData()
        {
            try
            {
                // 填充检验单基本信息
                txtInspectionCode.Text = _currentInspection.InspectionCode;
                
                // 设置检验类型
                if (!string.IsNullOrEmpty(_currentInspection.InspectionType))
                {
                    for (int i = 0; i < cmbInspectionType.Items.Count; i++)
                    {
                        if (cmbInspectionType.Items[i].ToString() == _currentInspection.InspectionType)
                        {
                            cmbInspectionType.SelectedIndex = i;
                            break;
                        }
                    }
                }
                
                // 设置产品
                cmbProduct.SelectedValue = _currentInspection.ProductId;
                
                // 设置批次号
                txtBatchNo.Text = _currentInspection.BatchNo;
                
                // 设置数量
                numQuantity.Value = _currentInspection.Quantity;
                numPassedQuantity.Value = _currentInspection.PassedQuantity;
                numFailedQuantity.Value = _currentInspection.FailedQuantity;
                
                // 设置检验结果
                if (!string.IsNullOrEmpty(_currentInspection.Result))
                {
                    for (int i = 0; i < cmbResult.Items.Count; i++)
                    {
                        if (cmbResult.Items[i].ToString() == _currentInspection.Result)
                        {
                            cmbResult.SelectedIndex = i;
                            break;
                        }
                    }
                }
                
                // 设置检验状态
                if (!string.IsNullOrEmpty(_currentInspection.Status))
                {
                    for (int i = 0; i < cmbStatus.Items.Count; i++)
                    {
                        if (cmbStatus.Items[i].ToString() == _currentInspection.Status)
                        {
                            cmbStatus.SelectedIndex = i;
                            break;
                        }
                    }
                }
                
                // 设置检验日期
                dtpInspectionDate.Value = _currentInspection.InspectionDate;
                
                // 设置检验人员
                txtInspector.Text = _currentInspection.Inspector;
                
                // 设置备注
                txtRemark.Text = _currentInspection.Remark;
                
                // 更新检验项目列表
                UpdateInspectionItemsGrid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "填充检验单数据时出错");
                throw;
            }
        }
        
        private void LoadInspectionItems()
        {
            try
            {
                // 加载检验项目
                _inspectionItems = _dbContext.QualityInspectionItems
                    .Where(i => i.InspectionId == _currentInspection.InspectionId)
                    .OrderBy(i => i.SequenceNo)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载检验项目时出错");
                throw;
            }
        }
        
        private void UpdateInspectionItemsGrid()
        {
            try
            {
                // 清空数据
                dgvItems.DataSource = null;
                
                // 创建数据源
                var dataSource = _inspectionItems.Select(i => new
                {
                    项目ID = i.ItemId,
                    序号 = i.SequenceNo,
                    项目编号 = i.ItemCode,
                    项目名称 = i.ItemName,
                    检验方法 = i.InspectionMethod,
                    标准值 = i.StandardValue,
                    上限值 = i.UpperLimit,
                    下限值 = i.LowerLimit,
                    单位 = i.Unit,
                    实测值 = i.ActualValue,
                    检验结果 = i.Result,
                    是否关键项 = i.IsCritical ? "是" : "否"
                }).ToList();
                
                // 绑定数据源
                dgvItems.DataSource = dataSource;
                
                // 隐藏项目ID列
                if (dgvItems.Columns.Contains("项目ID"))
                {
                    dgvItems.Columns["项目ID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新检验项目列表时出错");
                throw;
            }
        }
        
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否已选择产品
                if (cmbProduct.SelectedValue == null)
                {
                    MessageBox.Show("请先选择产品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                // 创建质量检验项目编辑窗体
                var serviceProvider = Program.ServiceProvider;
                var itemEditForm = serviceProvider.GetRequiredService<QualityInspectionItemEditForm>();
                
                // 设置为新增模式
                itemEditForm.SetInspectionItem(null, _inspectionItems.Count + 1);
                
                // 显示窗体
                if (itemEditForm.ShowDialog() == DialogResult.OK)
                {
                    // 获取新增的检验项目
                    var newItem = itemEditForm.GetInspectionItem();
                    
                    // 添加到列表
                    _inspectionItems.Add(newItem);
                    
                    // 更新列表
                    UpdateInspectionItemsGrid();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "添加检验项目时出错");
                MessageBox.Show($"添加检验项目时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnEditItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否已选择项目
                if (dgvItems.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选择一个检验项目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                // 获取选中的项目ID
                int itemId = Convert.ToInt32(dgvItems.SelectedRows[0].Cells["项目ID"].Value);
                
                // 查找对应的检验项目
                var item = _inspectionItems.FirstOrDefault(i => i.ItemId == itemId);
                if (item == null)
                {
                    MessageBox.Show("未找到选中的检验项目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                // 创建质量检验项目编辑窗体
                var serviceProvider = Program.ServiceProvider;
                var itemEditForm = serviceProvider.GetRequiredService<QualityInspectionItemEditForm>();
                
                // 设置为编辑模式
                itemEditForm.SetInspectionItem(item, item.SequenceNo);
                
                // 显示窗体
                if (itemEditForm.ShowDialog() == DialogResult.OK)
                {
                    // 获取编辑后的检验项目
                    var editedItem = itemEditForm.GetInspectionItem();
                    
                    // 更新列表中的项目
                    int index = _inspectionItems.FindIndex(i => i.ItemId == itemId);
                    if (index >= 0)
                    {
                        _inspectionItems[index] = editedItem;
                    }
                    
                    // 更新列表
                    UpdateInspectionItemsGrid();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "编辑检验项目时出错");
                MessageBox.Show($"编辑检验项目时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否已选择项目
                if (dgvItems.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选择一个检验项目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                // 获取选中的项目ID
                int itemId = Convert.ToInt32(dgvItems.SelectedRows[0].Cells["项目ID"].Value);
                
                // 查找对应的检验项目
                var item = _inspectionItems.FirstOrDefault(i => i.ItemId == itemId);
                if (item == null)
                {
                    MessageBox.Show("未找到选中的检验项目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                // 确认删除
                if (MessageBox.Show($"确定要删除检验项目 {item.ItemName} 吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // 从列表中移除
                    _inspectionItems.Remove(item);
                    
                    // 更新序号
                    for (int i = 0; i < _inspectionItems.Count; i++)
                    {
                        _inspectionItems[i].SequenceNo = i + 1;
                    }
                    
                    // 更新列表
                    UpdateInspectionItemsGrid();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除检验项目时出错");
                MessageBox.Show($"删除检验项目时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnLoadStandard_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否已选择产品
                if (cmbProduct.SelectedValue == null)
                {
                    MessageBox.Show("请先选择产品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                int productId = (int)cmbProduct.SelectedValue;
                
                // 获取检验类型
                string inspectionType = cmbInspectionType.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(inspectionType))
                {
                    MessageBox.Show("请先选择检验类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                // 查询适用的质量标准
                var standards = _dbContext.QualityStandards
                    .Where(s => s.IsActive && s.InspectionType == inspectionType &&
                           (s.ProductId == productId || s.ProductId == null))
                    .OrderByDescending(s => s.ProductId) // 优先使用产品专用标准
                    .ToList();
                
                if (standards.Count == 0)
                {
                    MessageBox.Show("未找到适用的质量检验标准", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                // 使用第一个匹配的标准
                var standard = standards.First();
                
                // 加载标准项目
                var standardItems = _dbContext.QualityStandardItems
                    .Where(i => i.StandardId == standard.StandardId)
                    .OrderBy(i => i.SequenceNo)
                    .ToList();
                
                if (standardItems.Count == 0)
                {
                    MessageBox.Show("选中的质量标准没有检验项目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                // 确认加载
                if (MessageBox.Show($"确定要加载标准 {standard.StandardName} 的 {standardItems.Count} 个检验项目吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // 清空现有项目
                    _inspectionItems.Clear();
                    
                    // 添加标准项目
                    foreach (var stdItem in standardItems)
                    {
                        var newItem = new QualityInspectionItem
                        {
                            ItemCode = stdItem.ItemCode,
                            ItemName = stdItem.ItemName,
                            InspectionMethod = stdItem.InspectionMethod,
                            InspectionTool = stdItem.InspectionTool,
                            StandardValue = stdItem.StandardValue,
                            UpperLimit = stdItem.UpperLimit,
                            LowerLimit = stdItem.LowerLimit,
                            Unit = stdItem.Unit,
                            SequenceNo = stdItem.SequenceNo,
                            IsCritical = stdItem.IsCritical,
                            Remark = stdItem.Remark,
                            CreateTime = DateTime.Now,
                            CreateBy = "admin" // 实际应用中应使用当前登录用户
                        };
                        
                        _inspectionItems.Add(newItem);
                    }
                    
                    // 更新列表
                    UpdateInspectionItemsGrid();
                    
                    MessageBox.Show($"已成功加载 {_inspectionItems.Count} 个检验项目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载质量标准时出错");
                MessageBox.Show($"加载质量标准时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void cmbInspectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isNewInspection)
            {
                // 新增时，更新检验单号
                GenerateInspectionCode();
            }
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证输入
                if (string.IsNullOrWhiteSpace(txtInspectionCode.Text))
                {
                    MessageBox.Show("检验单号不能为空", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtInspectionCode.Focus();
                    return;
                }
                
                if (cmbInspectionType.SelectedItem == null)
                {
                    MessageBox.Show("请选择检验类型", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbInspectionType.Focus();
                    return;
                }
                
                if (cmbProduct.SelectedValue == null)
                {
                    MessageBox.Show("请选择产品", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbProduct.Focus();
                    return;
                }
                
                if (numQuantity.Value <= 0)
                {
                    MessageBox.Show("检验数量必须大于0", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    numQuantity.Focus();
                    return;
                }
                
                // 验证检验单号唯一性
                if (_isNewInspection || txtInspectionCode.Text != _currentInspection.InspectionCode)
                {
                    bool exists = _dbContext.QualityInspections.Any(q => q.InspectionCode == txtInspectionCode.Text && q.InspectionId != _currentInspection.InspectionId);
                    if (exists)
                    {
                        MessageBox.Show("检验单号已存在", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtInspectionCode.Focus();
                        return;
                    }
                }
                
                // 验证检验项目
                if (_inspectionItems.Count == 0)
                {
                    MessageBox.Show("请至少添加一个检验项目", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // 更新检验单信息
                _currentInspection.InspectionCode = txtInspectionCode.Text.Trim();
                _currentInspection.InspectionType = cmbInspectionType.SelectedItem.ToString();
                _currentInspection.ProductId = (int)cmbProduct.SelectedValue;
                _currentInspection.BatchNo = txtBatchNo.Text.Trim();
                _currentInspection.Quantity = numQuantity.Value;
                _currentInspection.PassedQuantity = numPassedQuantity.Value;
                _currentInspection.FailedQuantity = numFailedQuantity.Value;
                _currentInspection.Result = cmbResult.SelectedItem?.ToString() ?? "";
                _currentInspection.Status = cmbStatus.SelectedItem.ToString();
                _currentInspection.InspectionDate = dtpInspectionDate.Value;
                _currentInspection.Inspector = txtInspector.Text.Trim();
                _currentInspection.Remark = txtRemark.Text.Trim();
                
                // 保存到数据库
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (_isNewInspection)
                        {
                            _dbContext.QualityInspections.Add(_currentInspection);
                            _dbContext.SaveChanges();
                        }
                        else
                        {
                            // 更新时间和更新人
                            _currentInspection.UpdateTime = DateTime.Now;
                            _currentInspection.UpdateBy = "admin"; // 实际应用中应使用当前登录用户
                            
                            // 删除原有的检验项目
                            var oldItems = _dbContext.QualityInspectionItems.Where(i => i.InspectionId == _currentInspection.InspectionId).ToList();
                            foreach (var item in oldItems)
                            {
                                _dbContext.QualityInspectionItems.Remove(item);
                            }
                            
                            _dbContext.SaveChanges();
                        }
                        
                        // 添加检验项目
                        foreach (var item in _inspectionItems)
                        {
                            item.InspectionId = _currentInspection.InspectionId;
                            _dbContext.QualityInspectionItems.Add(item);
                        }
                        
                        _dbContext.SaveChanges();
                        transaction.Commit();
                        
                        // 关闭窗体
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存质量检验单时出错");
                MessageBox.Show($"保存质量检验单时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 取消操作
            DialogResult = DialogResult.Cancel;
            Close();
        }
        
        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {
            // 更新合格数量和不合格数量
            decimal total = numQuantity.Value;
            decimal passed = numPassedQuantity.Value;
            decimal failed = numFailedQuantity.Value;
            
            // 如果合格数量 + 不合格数量 > 总数量，则调整
            if (passed + failed > total)
            {
                // 优先调整不合格数量
                numFailedQuantity.Value = Math.Max(0, total - passed);
            }
        }
        
        private void numPassedQuantity_ValueChanged(object sender, EventArgs e)
        {
            // 更新不合格数量
            decimal total = numQuantity.Value;
            decimal passed = numPassedQuantity.Value;
            
            // 确保合格数量不超过总数量
            if (passed > total)
            {
                numPassedQuantity.Value = total;
                passed = total;
            }
            
            // 计算不合格数量
            numFailedQuantity.Value = total - passed;
        }
        
        private void numFailedQuantity_ValueChanged(object sender, EventArgs e)
        {
            // 更新合格数量
            decimal total = numQuantity.Value;
            decimal failed = numFailedQuantity.Value;
            
            // 确保不合格数量不超过总数量
            if (failed > total)
            {
                numFailedQuantity.Value = total;
                failed = total;
            }
            
            // 计算合格数量
            numPassedQuantity.Value = total - failed;
        }
    }
} 