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
using GC_MES.Model;

namespace GC_MES.WinForm.Forms
{
    public partial class BOMEditForm : Form
    {
        private readonly ILogger<BOMEditForm> _logger;
        private readonly IConfiguration _configuration;
        private readonly MesDbContext _dbContext;
        
        // 当前编辑的BOM
        private BOM _currentBOM;
        private bool _isNewBOM = true;
        
        // 当前所属产品
        private int _parentProductId;
        private string _parentProductName;
        
        // 产品列表
        private List<Product> _products = new List<Product>();

        public BOMEditForm(ILogger<BOMEditForm> logger, IConfiguration configuration, MesDbContext dbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
            
            InitializeComponent();
            
            // 应用当前主题
            ThemeManager.ApplyTheme(this);
        }

        /// <summary>
        /// 设置当前编辑的BOM
        /// </summary>
        /// <param name="bom">要编辑的BOM，如果为null则表示新增BOM</param>
        /// <param name="productId">父产品ID</param>
        /// <param name="productName">父产品名称</param>
        public void SetBOM(BOM bom, int productId, string productName)
        {
            _parentProductId = productId;
            _parentProductName = productName;
            
            if (bom == null)
            {
                // 新增BOM
                _currentBOM = new BOM
                {
                    ProductId = _parentProductId,
                    Level = 1, // 默认为一级BOM
                    Quantity = 1, // 默认用量为1
                    ScrapRate = 0, // 默认废品率为0
                    Version = "1.0", // 默认版本为1.0
                    IsActive = true,
                    CreateTime = DateTime.Now,
                    CreateBy = "admin" // 实际应用中应使用当前登录用户
                };
                _isNewBOM = true;
                
                // 更新窗体标题
                this.Text = $"添加物料 - {_parentProductName}";
            }
            else
            {
                // 编辑BOM
                _currentBOM = bom;
                _isNewBOM = false;
                
                // 更新窗体标题
                this.Text = $"编辑物料 - {_parentProductName}";
            }
        }

        private void BOMEditForm_Load(object sender, EventArgs e)
        {
            try
            {
                // 加载产品下拉框
                LoadProducts();
                
                // 设置父产品名称显示
                lblParentProduct.Text = _parentProductName;
                
                // 如果是编辑，则填充表单数据
                if (!_isNewBOM)
                {
                    FillBOMData();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载BOM编辑窗体时出错");
                MessageBox.Show($"加载BOM编辑窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProducts()
        {
            try
            {
                // 从数据库加载所有产品
                _products = _dbContext.Products
                    .Where(p => p.IsActive)
                    .OrderBy(p => p.ProductCode)
                    .ToList();
                
                // 初始化产品下拉框
                cmbComponent.Items.Clear();
                cmbComponent.DisplayMember = "ProductName";
                cmbComponent.ValueMember = "ProductId";
                
                foreach (var product in _products)
                {
                    cmbComponent.Items.Add(product);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载产品列表时出错");
                
                // 如果数据库加载失败，则使用模拟数据
                _products = new List<Product>
                {
                    new Product { ProductId = 1, ProductCode = "P001", ProductName = "零件A", Category = "原材料", Unit = "个" },
                    new Product { ProductId = 2, ProductCode = "P002", ProductName = "螺丝", Category = "原材料", Unit = "个" },
                    new Product { ProductId = 3, ProductCode = "P003", ProductName = "支架组件", Category = "半成品", Unit = "个" },
                    new Product { ProductId = 4, ProductCode = "P004", ProductName = "电机", Category = "原材料", Unit = "台" }
                };
                
                // 使用模拟数据填充下拉框
                cmbComponent.Items.Clear();
                cmbComponent.DisplayMember = "ProductName";
                cmbComponent.ValueMember = "ProductId";
                
                foreach (var product in _products)
                {
                    cmbComponent.Items.Add(product);
                }
            }
        }

        private void FillBOMData()
        {
            try
            {
                // 选择组件产品
                foreach (Product product in cmbComponent.Items)
                {
                    if (product.ProductId == _currentBOM.ComponentId)
                    {
                        cmbComponent.SelectedItem = product;
                        break;
                    }
                }
                
                // 填充其他字段
                numQuantity.Value = _currentBOM.Quantity;
                txtPosition.Text = _currentBOM.Position;
                
                if (_currentBOM.ScrapRate.HasValue)
                {
                    numScrapRate.Value = _currentBOM.ScrapRate.Value;
                }
                
                txtUnit.Text = _currentBOM.Unit;
                numLevel.Value = _currentBOM.Level;
                txtVersion.Text = _currentBOM.Version;
                txtRemark.Text = _currentBOM.Remark;
                chkIsActive.Checked = _currentBOM.IsActive;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "填充BOM数据时出错");
                throw;
            }
        }

        private void cmbComponent_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 当选择的组件变化时，自动填充单位
            if (cmbComponent.SelectedItem != null)
            {
                Product selectedProduct = (Product)cmbComponent.SelectedItem;
                txtUnit.Text = selectedProduct.Unit;
            }
        }

        private void btnSelectComponent_Click(object sender, EventArgs e)
        {
            // 弹出产品选择窗口（实际应用中应实现）
            MessageBox.Show("产品选择功能将在后续版本实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证输入
                if (cmbComponent.SelectedItem == null)
                {
                    MessageBox.Show("请选择组件", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbComponent.Focus();
                    return;
                }
                
                if (numQuantity.Value <= 0)
                {
                    MessageBox.Show("用量必须大于0", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    numQuantity.Focus();
                    return;
                }
                
                // 获取选中的组件产品
                Product selectedComponent = (Product)cmbComponent.SelectedItem;
                int componentId = selectedComponent.ProductId;
                
                // 验证组件不能是产品自身
                if (componentId == _parentProductId)
                {
                    MessageBox.Show("组件不能是产品自身", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbComponent.Focus();
                    return;
                }
                
                // 验证组件没有循环引用
                if (!_isNewBOM && HasCircularReference(componentId, _parentProductId))
                {
                    MessageBox.Show("检测到循环引用，该组件直接或间接地将当前产品作为其组件", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbComponent.Focus();
                    return;
                }
                
                // 验证该组件是否已存在于当前产品的BOM中
                if (_isNewBOM || _currentBOM.ComponentId != componentId)
                {
                    bool exists = _dbContext.BOMs.Any(b => b.ProductId == _parentProductId && b.ComponentId == componentId && b.BOMId != _currentBOM.BOMId);
                    if (exists)
                    {
                        MessageBox.Show("该组件已存在于当前产品的BOM中", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbComponent.Focus();
                        return;
                    }
                }
                
                // 更新BOM信息
                _currentBOM.ComponentId = componentId;
                _currentBOM.Quantity = numQuantity.Value;
                _currentBOM.Position = txtPosition.Text.Trim();
                _currentBOM.ScrapRate = numScrapRate.Value;
                _currentBOM.Unit = txtUnit.Text.Trim();
                _currentBOM.Level = (int)numLevel.Value;
                _currentBOM.Version = txtVersion.Text.Trim();
                _currentBOM.Remark = txtRemark.Text.Trim();
                _currentBOM.IsActive = chkIsActive.Checked;
                
                // 保存到数据库
                if (_isNewBOM)
                {
                    _dbContext.BOMs.Add(_currentBOM);
                }
                else
                {
                    // 更新时间和更新人
                    _currentBOM.UpdateTime = DateTime.Now;
                    _currentBOM.UpdateBy = "admin"; // 实际应用中应使用当前登录用户
                }
                
                _dbContext.SaveChanges();
                
                // 关闭窗体
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存BOM时出错");
                MessageBox.Show($"保存BOM时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool HasCircularReference(int componentId, int productId)
        {
            try
            {
                // 检查组件的BOM中是否包含当前产品
                var componentBOMs = _dbContext.BOMs.Where(b => b.ProductId == componentId).ToList();
                
                foreach (var bom in componentBOMs)
                {
                    if (bom.ComponentId == productId)
                    {
                        return true;
                    }
                    
                    // 递归检查子组件
                    if (HasCircularReference(bom.ComponentId, productId))
                    {
                        return true;
                    }
                }
                
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查循环引用时出错");
                return false; // 如果出错，默认允许添加
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