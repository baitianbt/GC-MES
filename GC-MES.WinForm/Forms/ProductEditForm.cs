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
using System.IO;

namespace GC_MES.WinForm.Forms
{
    public partial class ProductEditForm : Form
    {
        private readonly ILogger<ProductEditForm> _logger;
        private readonly IConfiguration _configuration;
        private readonly MesDbContext _dbContext;
        
        // 当前编辑的产品
        private Product _currentProduct;
        private bool _isNewProduct = true;
        
        // 类别列表
        private List<string> _categories = new List<string>() { "原材料", "半成品", "成品" };
        
        // 产品类型列表
        private List<string> _productTypes = new List<string>() { "自制", "外购", "委外" };

        public ProductEditForm(ILogger<ProductEditForm> logger, IConfiguration configuration, MesDbContext dbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
            
            InitializeComponent();
            
            // 应用当前主题
            ThemeManager.ApplyTheme(this);
        }

        /// <summary>
        /// 设置当前编辑的产品
        /// </summary>
        /// <param name="product">要编辑的产品，如果为null则表示新增产品</param>
        public void SetProduct(Product product)
        {
            if (product == null)
            {
                // 新增产品
                _currentProduct = new Product
                {
                    IsActive = true,
                    CreateTime = DateTime.Now,
                    CreateBy = "admin" // 实际应用中应使用当前登录用户
                };
                _isNewProduct = true;
                
                // 更新窗体标题
                this.Text = "新增产品";
            }
            else
            {
                // 编辑产品
                _currentProduct = product;
                _isNewProduct = false;
                
                // 更新窗体标题
                this.Text = $"编辑产品 - {product.ProductName}";
            }
        }

        private void ProductEditForm_Load(object sender, EventArgs e)
        {
            try
            {
                // 初始化下拉框
                InitializeComboBoxes();
                
                // 如果是编辑，则填充表单数据
                if (!_isNewProduct)
                {
                    FillProductData();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载产品编辑窗体时出错");
                MessageBox.Show($"加载产品编辑窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComboBoxes()
        {
            // 初始化类别下拉框
            try
            {
                // 从数据库获取所有不同的类别
                var dbCategories = _dbContext.Products
                    .Where(p => !string.IsNullOrEmpty(p.Category))
                    .Select(p => p.Category)
                    .Distinct()
                    .ToList();
                
                foreach(var category in dbCategories)
                {
                    if(!_categories.Contains(category))
                    {
                        _categories.Add(category);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从数据库加载类别数据时出错");
            }
            
            cmbCategory.Items.Clear();
            foreach (var category in _categories)
            {
                cmbCategory.Items.Add(category);
            }
            
            // 初始化产品类型下拉框
            try
            {
                // 从数据库获取所有不同的产品类型
                var dbProductTypes = _dbContext.Products
                    .Where(p => !string.IsNullOrEmpty(p.ProductType))
                    .Select(p => p.ProductType)
                    .Distinct()
                    .ToList();
                
                foreach(var type in dbProductTypes)
                {
                    if(!_productTypes.Contains(type))
                    {
                        _productTypes.Add(type);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从数据库加载产品类型数据时出错");
            }
            
            cmbProductType.Items.Clear();
            foreach (var type in _productTypes)
            {
                cmbProductType.Items.Add(type);
            }
        }

        private void FillProductData()
        {
            // 填充产品基本信息
            txtProductCode.Text = _currentProduct.ProductCode;
            txtProductName.Text = _currentProduct.ProductName;
            txtDescription.Text = _currentProduct.Description;
            txtSpecification.Text = _currentProduct.Specification;
            txtUnit.Text = _currentProduct.Unit;
            
            // 设置下拉框选中值
            if (!string.IsNullOrEmpty(_currentProduct.Category) && cmbCategory.Items.Contains(_currentProduct.Category))
            {
                cmbCategory.SelectedItem = _currentProduct.Category;
            }
            
            if (!string.IsNullOrEmpty(_currentProduct.ProductType) && cmbProductType.Items.Contains(_currentProduct.ProductType))
            {
                cmbProductType.SelectedItem = _currentProduct.ProductType;
            }
            
            // 填充其他字段
            txtWarehouse.Text = _currentProduct.DefaultWarehouse;
            txtLocation.Text = _currentProduct.DefaultLocation;
            
            if (_currentProduct.ShelfLife.HasValue)
            {
                numShelfLife.Value = _currentProduct.ShelfLife.Value;
            }
            
            if (_currentProduct.MinStock.HasValue)
            {
                numMinStock.Value = _currentProduct.MinStock.Value;
            }
            
            if (_currentProduct.MaxStock.HasValue)
            {
                numMaxStock.Value = _currentProduct.MaxStock.Value;
            }
            
            if (_currentProduct.StandardCost.HasValue)
            {
                numStandardCost.Value = _currentProduct.StandardCost.Value;
            }
            
            if (_currentProduct.SalePrice.HasValue)
            {
                numSalePrice.Value = _currentProduct.SalePrice.Value;
            }
            
            txtMaterialCode.Text = _currentProduct.MaterialCode;
            txtBarcode.Text = _currentProduct.Barcode;
            txtVersion.Text = _currentProduct.Version;
            chkIsActive.Checked = _currentProduct.IsActive;
            
            // 显示产品图片
            if (!string.IsNullOrEmpty(_currentProduct.ImagePath))
            {
                try
                {
                    string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", _currentProduct.ImagePath);
                    if (File.Exists(fullPath))
                    {
                        pictureBoxProduct.Image = Image.FromFile(fullPath);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "加载产品图片时出错");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证输入
                if (string.IsNullOrWhiteSpace(txtProductCode.Text))
                {
                    MessageBox.Show("产品编码不能为空", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtProductCode.Focus();
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(txtProductName.Text))
                {
                    MessageBox.Show("产品名称不能为空", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtProductName.Focus();
                    return;
                }
                
                // 验证产品编码唯一性
                if (_isNewProduct || txtProductCode.Text != _currentProduct.ProductCode)
                {
                    bool exists = _dbContext.Products.Any(p => p.ProductCode == txtProductCode.Text);
                    if (exists)
                    {
                        MessageBox.Show("产品编码已存在", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtProductCode.Focus();
                        return;
                    }
                }
                
                // 更新产品信息
                _currentProduct.ProductCode = txtProductCode.Text.Trim();
                _currentProduct.ProductName = txtProductName.Text.Trim();
                _currentProduct.Description = txtDescription.Text.Trim();
                _currentProduct.Specification = txtSpecification.Text.Trim();
                _currentProduct.Unit = txtUnit.Text.Trim();
                _currentProduct.Category = cmbCategory.SelectedItem?.ToString();
                _currentProduct.ProductType = cmbProductType.SelectedItem?.ToString();
                _currentProduct.DefaultWarehouse = txtWarehouse.Text.Trim();
                _currentProduct.DefaultLocation = txtLocation.Text.Trim();
                
                // 数值字段
                _currentProduct.ShelfLife = (int)numShelfLife.Value;
                _currentProduct.MinStock = numMinStock.Value;
                _currentProduct.MaxStock = numMaxStock.Value;
                _currentProduct.StandardCost = numStandardCost.Value;
                _currentProduct.SalePrice = numSalePrice.Value;
                
                // 其他字段
                _currentProduct.MaterialCode = txtMaterialCode.Text.Trim();
                _currentProduct.Barcode = txtBarcode.Text.Trim();
                _currentProduct.Version = txtVersion.Text.Trim();
                _currentProduct.IsActive = chkIsActive.Checked;
                
                // 保存图片路径
                // 实际应用中，这里应该处理图片上传并保存路径
                
                // 保存到数据库
                if (_isNewProduct)
                {
                    _dbContext.Products.Add(_currentProduct);
                }
                else
                {
                    // 更新时间和更新人
                    _currentProduct.UpdateTime = DateTime.Now;
                    _currentProduct.UpdateBy = "admin"; // 实际应用中应使用当前登录用户
                }
                
                _dbContext.SaveChanges();
                
                // 关闭窗体
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存产品时出错");
                MessageBox.Show($"保存产品时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 取消操作
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "图片文件|*.jpg;*.jpeg;*.png;*.gif;*.bmp|所有文件|*.*";
                    openFileDialog.Title = "选择产品图片";
                    
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFile = openFileDialog.FileName;
                        
                        // 显示选中的图片
                        pictureBoxProduct.Image = Image.FromFile(selectedFile);
                        
                        // 在实际应用中，这里应该将图片保存到指定目录并更新ImagePath
                        // 为简化处理，这里仅记录文件名
                        string fileName = Path.GetFileName(selectedFile);
                        _currentProduct.ImagePath = fileName;
                        
                        // 实际应用中应该将图片复制到应用的图片目录
                        // string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", fileName);
                        // File.Copy(selectedFile, destPath, true);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "选择产品图片时出错");
                MessageBox.Show($"选择产品图片时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearImage_Click(object sender, EventArgs e)
        {
            // 清除图片
            pictureBoxProduct.Image = null;
            _currentProduct.ImagePath = null;
        }
    }
} 