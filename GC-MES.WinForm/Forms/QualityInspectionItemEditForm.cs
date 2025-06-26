using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    public partial class QualityInspectionItemEditForm : Form
    {
        private readonly ILogger<QualityInspectionItemEditForm> _logger;
        private readonly IConfiguration _configuration;
        private readonly MesDbContext _dbContext;
        
        // 当前编辑的检验项目
        private QualityInspectionItem _currentItem;
        private bool _isNewItem = true;
        
        // 当前项目的序号
        private int _sequenceNo = 1;

        public QualityInspectionItemEditForm(ILogger<QualityInspectionItemEditForm> logger, IConfiguration configuration, MesDbContext dbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
            
            InitializeComponent();
            
            // 应用当前主题
            ThemeManager.ApplyTheme(this);
        }

        /// <summary>
        /// 设置当前编辑的检验项目
        /// </summary>
        /// <param name="item">要编辑的检验项目，如果为null则表示新增项目</param>
        /// <param name="sequenceNo">项目序号</param>
        public void SetInspectionItem(QualityInspectionItem item, int sequenceNo)
        {
            _sequenceNo = sequenceNo;
            
            if (item == null)
            {
                // 新增检验项目
                _currentItem = new QualityInspectionItem
                {
                    SequenceNo = _sequenceNo,
                    IsCritical = false,
                    CreateTime = DateTime.Now,
                    CreateBy = "admin" // 实际应用中应使用当前登录用户
                };
                _isNewItem = true;
                
                // 更新窗体标题
                this.Text = "新增检验项目";
            }
            else
            {
                // 编辑检验项目
                _currentItem = new QualityInspectionItem
                {
                    ItemId = item.ItemId,
                    InspectionId = item.InspectionId,
                    ItemCode = item.ItemCode,
                    ItemName = item.ItemName,
                    InspectionMethod = item.InspectionMethod,
                    InspectionTool = item.InspectionTool,
                    StandardValue = item.StandardValue,
                    UpperLimit = item.UpperLimit,
                    LowerLimit = item.LowerLimit,
                    Unit = item.Unit,
                    ActualValue = item.ActualValue,
                    Result = item.Result,
                    DefectReason = item.DefectReason,
                    DefectType = item.DefectType,
                    SequenceNo = item.SequenceNo,
                    IsCritical = item.IsCritical,
                    Remark = item.Remark,
                    CreateTime = item.CreateTime,
                    CreateBy = item.CreateBy,
                    UpdateTime = DateTime.Now,
                    UpdateBy = "admin" // 实际应用中应使用当前登录用户
                };
                _isNewItem = false;
                
                // 更新窗体标题
                this.Text = $"编辑检验项目 - {item.ItemName}";
            }
        }

        /// <summary>
        /// 获取编辑后的检验项目
        /// </summary>
        /// <returns>检验项目实体</returns>
        public QualityInspectionItem GetInspectionItem()
        {
            return _currentItem;
        }

        private void QualityInspectionItemEditForm_Load(object sender, EventArgs e)
        {
            try
            {
                // 初始化结果下拉框
                InitializeResultComboBox();
                
                // 如果是编辑，则填充表单数据
                if (!_isNewItem)
                {
                    FillItemData();
                }
                else
                {
                    // 新增时的默认值
                    numSequenceNo.Value = _sequenceNo;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载检验项目编辑窗体时出错");
                MessageBox.Show($"加载检验项目编辑窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void InitializeResultComboBox()
        {
            cmbResult.Items.Clear();
            cmbResult.Items.Add("");
            cmbResult.Items.Add("合格");
            cmbResult.Items.Add("不合格");
            
            if (_isNewItem)
            {
                cmbResult.SelectedIndex = 0;
            }
        }
        
        private void FillItemData()
        {
            try
            {
                // 填充检验项目基本信息
                txtItemCode.Text = _currentItem.ItemCode;
                txtItemName.Text = _currentItem.ItemName;
                txtInspectionMethod.Text = _currentItem.InspectionMethod;
                txtInspectionTool.Text = _currentItem.InspectionTool;
                txtStandardValue.Text = _currentItem.StandardValue;
                
                if (_currentItem.UpperLimit.HasValue)
                {
                    numUpperLimit.Value = _currentItem.UpperLimit.Value;
                }
                
                if (_currentItem.LowerLimit.HasValue)
                {
                    numLowerLimit.Value = _currentItem.LowerLimit.Value;
                }
                
                txtUnit.Text = _currentItem.Unit;
                txtActualValue.Text = _currentItem.ActualValue;
                
                // 设置检验结果
                if (!string.IsNullOrEmpty(_currentItem.Result))
                {
                    for (int i = 0; i < cmbResult.Items.Count; i++)
                    {
                        if (cmbResult.Items[i].ToString() == _currentItem.Result)
                        {
                            cmbResult.SelectedIndex = i;
                            break;
                        }
                    }
                }
                
                txtDefectReason.Text = _currentItem.DefectReason;
                txtDefectType.Text = _currentItem.DefectType;
                numSequenceNo.Value = _currentItem.SequenceNo;
                chkIsCritical.Checked = _currentItem.IsCritical;
                txtRemark.Text = _currentItem.Remark;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "填充检验项目数据时出错");
                throw;
            }
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证输入
                if (string.IsNullOrWhiteSpace(txtItemCode.Text))
                {
                    MessageBox.Show("项目编号不能为空", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtItemCode.Focus();
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(txtItemName.Text))
                {
                    MessageBox.Show("项目名称不能为空", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtItemName.Focus();
                    return;
                }
                
                // 更新检验项目信息
                _currentItem.ItemCode = txtItemCode.Text.Trim();
                _currentItem.ItemName = txtItemName.Text.Trim();
                _currentItem.InspectionMethod = txtInspectionMethod.Text.Trim();
                _currentItem.InspectionTool = txtInspectionTool.Text.Trim();
                _currentItem.StandardValue = txtStandardValue.Text.Trim();
                _currentItem.UpperLimit = numUpperLimit.Value > 0 ? numUpperLimit.Value : (decimal?)null;
                _currentItem.LowerLimit = numLowerLimit.Value > 0 ? numLowerLimit.Value : (decimal?)null;
                _currentItem.Unit = txtUnit.Text.Trim();
                _currentItem.ActualValue = txtActualValue.Text.Trim();
                _currentItem.Result = cmbResult.SelectedItem?.ToString() ?? "";
                _currentItem.DefectReason = txtDefectReason.Text.Trim();
                _currentItem.DefectType = txtDefectType.Text.Trim();
                _currentItem.SequenceNo = (int)numSequenceNo.Value;
                _currentItem.IsCritical = chkIsCritical.Checked;
                _currentItem.Remark = txtRemark.Text.Trim();
                
                // 关闭窗体
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存检验项目时出错");
                MessageBox.Show($"保存检验项目时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 取消操作
            DialogResult = DialogResult.Cancel;
            Close();
        }
        
        private void cmbResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 如果选择"不合格"，启用不合格原因和类型
            bool isDefect = cmbResult.SelectedIndex > 0 && cmbResult.SelectedItem.ToString() == "不合格";
            
            lblDefectReason.Enabled = isDefect;
            txtDefectReason.Enabled = isDefect;
            lblDefectType.Enabled = isDefect;
            txtDefectType.Enabled = isDefect;
        }
    }
} 