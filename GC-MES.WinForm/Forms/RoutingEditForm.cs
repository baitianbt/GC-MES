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
    public partial class RoutingEditForm : Form
    {
        private readonly ILogger<RoutingEditForm> _logger;
        private readonly IConfiguration _configuration;
        private readonly MesDbContext _dbContext;
        
        // 当前编辑的工艺路线
        private ProductRouting _currentRouting;
        private bool _isNewRouting = true;
        
        // 当前所属产品
        private int _parentProductId;
        private string _parentProductName;

        public RoutingEditForm(ILogger<RoutingEditForm> logger, IConfiguration configuration, MesDbContext dbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
            
            InitializeComponent();
            
            // 应用当前主题
            ThemeManager.ApplyTheme(this);
        }

        /// <summary>
        /// 设置当前编辑的工艺路线
        /// </summary>
        /// <param name="routing">要编辑的工艺路线，如果为null则表示新增工艺路线</param>
        /// <param name="productId">产品ID</param>
        /// <param name="productName">产品名称</param>
        public void SetRouting(ProductRouting routing, int productId, string productName)
        {
            _parentProductId = productId;
            _parentProductName = productName;
            
            if (routing == null)
            {
                // 新增工艺路线
                _currentRouting = new ProductRouting
                {
                    ProductId = _parentProductId,
                    IsDefault = false,
                    IsActive = true,
                    Version = "1.0", // 默认版本为1.0
                    RoutingSequence = 10, // 默认序号为10
                    CreateTime = DateTime.Now,
                    CreateBy = "admin" // 实际应用中应使用当前登录用户
                };
                _isNewRouting = true;
                
                // 更新窗体标题
                this.Text = $"添加工艺路线 - {_parentProductName}";
            }
            else
            {
                // 编辑工艺路线
                _currentRouting = routing;
                _isNewRouting = false;
                
                // 更新窗体标题
                this.Text = $"编辑工艺路线 - {_parentProductName}";
            }
        }

        private void RoutingEditForm_Load(object sender, EventArgs e)
        {
            try
            {
                // 设置父产品名称显示
                lblParentProduct.Text = _parentProductName;
                
                // 如果是编辑，则填充表单数据
                if (!_isNewRouting)
                {
                    FillRoutingData();
                }
                else
                {
                    // 新增默认值
                    chkIsActive.Checked = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载工艺路线编辑窗体时出错");
                MessageBox.Show($"加载工艺路线编辑窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillRoutingData()
        {
            try
            {
                // 填充工艺路线信息
                txtRoutingName.Text = _currentRouting.RoutingName;
                txtDescription.Text = _currentRouting.Description;
                txtVersion.Text = _currentRouting.Version;
                numSequence.Value = _currentRouting.RoutingSequence;
                chkIsDefault.Checked = _currentRouting.IsDefault;
                chkIsActive.Checked = _currentRouting.IsActive;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "填充工艺路线数据时出错");
                throw;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证输入
                if (string.IsNullOrWhiteSpace(txtRoutingName.Text))
                {
                    MessageBox.Show("工艺路线名称不能为空", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRoutingName.Focus();
                    return;
                }
                
                // 验证工艺路线名称唯一性
                if (_isNewRouting || txtRoutingName.Text != _currentRouting.RoutingName)
                {
                    bool exists = _dbContext.ProductRoutings.Any(r => r.ProductId == _parentProductId && r.RoutingName == txtRoutingName.Text && r.RoutingId != _currentRouting.RoutingId);
                    if (exists)
                    {
                        MessageBox.Show("工艺路线名称已存在", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtRoutingName.Focus();
                        return;
                    }
                }
                
                // 如果设置为默认工艺，需要取消其他默认工艺
                if (chkIsDefault.Checked)
                {
                    var defaultRoutings = _dbContext.ProductRoutings.Where(r => r.ProductId == _parentProductId && r.IsDefault && r.RoutingId != _currentRouting.RoutingId).ToList();
                    foreach (var routing in defaultRoutings)
                    {
                        routing.IsDefault = false;
                        routing.UpdateTime = DateTime.Now;
                        routing.UpdateBy = "admin"; // 实际应用中应使用当前登录用户
                    }
                }
                
                // 更新工艺路线信息
                _currentRouting.RoutingName = txtRoutingName.Text.Trim();
                _currentRouting.Description = txtDescription.Text.Trim();
                _currentRouting.Version = txtVersion.Text.Trim();
                _currentRouting.RoutingSequence = (int)numSequence.Value;
                _currentRouting.IsDefault = chkIsDefault.Checked;
                _currentRouting.IsActive = chkIsActive.Checked;
                
                // 保存到数据库
                if (_isNewRouting)
                {
                    _dbContext.ProductRoutings.Add(_currentRouting);
                }
                else
                {
                    // 更新时间和更新人
                    _currentRouting.UpdateTime = DateTime.Now;
                    _currentRouting.UpdateBy = "admin"; // 实际应用中应使用当前登录用户
                }
                
                _dbContext.SaveChanges();
                
                // 关闭窗体
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存工艺路线时出错");
                MessageBox.Show($"保存工艺路线时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 取消操作
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
} 