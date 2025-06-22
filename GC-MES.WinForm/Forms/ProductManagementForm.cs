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
using System.Data.Entity;

namespace GC_MES.WinForm.Forms
{
    public partial class ProductManagementForm : Form
    {
        private readonly ILogger<ProductManagementForm> _logger;
        private readonly IConfiguration _configuration;
        private readonly MesDbContext _dbContext;
        
        // 分页相关变量
        private int _currentPageIndex = 0;
        private int _pageSize = 10;
        private int _totalPages = 0;
        private List<Product> _allProducts = new List<Product>();

        public ProductManagementForm(ILogger<ProductManagementForm> logger, IConfiguration configuration, MesDbContext dbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
            
            InitializeComponent();
            
            // 应用当前主题
            ThemeManager.ApplyTheme(this);
        }

        private void ProductManagementForm_Load(object sender, EventArgs e)
        {
            // 初始化类别下拉框
            InitializeCategoryComboBox();
            
            // 初始化数据网格视图
            InitializeDataGridView();
            
            // 加载产品数据
            LoadProductData();
        }

        private void InitializeCategoryComboBox()
        {
            try
            {
                // 从数据库中获取所有不同的产品类别
                var categories = _dbContext.Products.Select(p => p.Category).Distinct().ToList();
                
                // 添加到下拉框
                cmbCategory.Items.Clear();
                cmbCategory.Items.Add("全部");
                foreach (var category in categories.Where(c => !string.IsNullOrEmpty(c)))
                {
                    cmbCategory.Items.Add(category);
                }
                
                // 默认选择"全部"
                cmbCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化类别下拉框时出错");
                
                // 出错时添加默认类别
                cmbCategory.Items.Clear();
                cmbCategory.Items.Add("全部");
                cmbCategory.Items.Add("原材料");
                cmbCategory.Items.Add("半成品");
                cmbCategory.Items.Add("成品");
                cmbCategory.SelectedIndex = 0;
            }
        }

        private void InitializeDataGridView()
        {
            // 设置列宽
            dataGridViewProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            // 禁止添加行
            dataGridViewProducts.AllowUserToAddRows = false;
            
            // 整行选择
            dataGridViewProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            // 只读
            dataGridViewProducts.ReadOnly = true;
            
            // 交替行颜色
            dataGridViewProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            
            // 双击编辑产品
            dataGridViewProducts.CellDoubleClick += DataGridViewProducts_CellDoubleClick;
        }

        private void LoadProductData()
        {
            try
            {
                // 清空现有数据
                _allProducts.Clear();

                try
                {
                    // 从数据库加载数据
                    _allProducts = _dbContext.Products.OrderBy(p => p.ProductCode).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "从数据库加载产品数据时发生错误");
                    
                    // 如果数据库加载失败，则使用模拟数据
                    _allProducts = new List<Product>
                    {
                        new Product { ProductId = 1, ProductCode = "P001", ProductName = "铝型材", Specification = "100mm*50mm*3m", Unit = "根", Category = "原材料", MaterialCode = "M001", IsActive = true, CreateTime = DateTime.Now.AddMonths(-6) },
                        new Product { ProductId = 2, ProductCode = "P002", ProductName = "螺丝", Specification = "M8x25", Unit = "个", Category = "原材料", MaterialCode = "M002", IsActive = true, CreateTime = DateTime.Now.AddMonths(-5) },
                        new Product { ProductId = 3, ProductCode = "P003", ProductName = "支架组件", Specification = "标准型", Unit = "个", Category = "半成品", MaterialCode = "M003", IsActive = true, CreateTime = DateTime.Now.AddMonths(-4) },
                        new Product { ProductId = 4, ProductCode = "P004", ProductName = "电机", Specification = "YE2-160M-4 18.5Kw", Unit = "台", Category = "原材料", MaterialCode = "M004", IsActive = true, CreateTime = DateTime.Now.AddMonths(-3) },
                        new Product { ProductId = 5, ProductCode = "P005", ProductName = "驱动组件", Specification = "YE2系列", Unit = "套", Category = "半成品", MaterialCode = "M005", IsActive = true, CreateTime = DateTime.Now.AddMonths(-2) },
                        new Product { ProductId = 6, ProductCode = "P006", ProductName = "自动包装机", Specification = "ZD-580", Unit = "台", Category = "成品", MaterialCode = "M006", IsActive = true, CreateTime = DateTime.Now.AddMonths(-1) }
                    };
                }

                // 更新总页数
                _totalPages = (_allProducts.Count + _pageSize - 1) / _pageSize;
                if (_totalPages == 0) _totalPages = 1;

                // 设置当前页为第一页
                _currentPageIndex = 0;

                // 显示第一页数据
                UpdatePageDisplay();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载产品数据时出错");
                MessageBox.Show($"加载产品数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdatePageDisplay()
        {
            try
            {
                // 清空当前绑定数据
                dataGridViewProducts.DataSource = null;

                // 创建当前页数据表
                DataTable currentPageTable = new DataTable();
                currentPageTable.Columns.Add("产品ID", typeof(int));
                currentPageTable.Columns.Add("产品编码", typeof(string));
                currentPageTable.Columns.Add("产品名称", typeof(string));
                currentPageTable.Columns.Add("规格型号", typeof(string));
                currentPageTable.Columns.Add("单位", typeof(string));
                currentPageTable.Columns.Add("类别", typeof(string));
                currentPageTable.Columns.Add("物料编码", typeof(string));
                currentPageTable.Columns.Add("状态", typeof(string));
                currentPageTable.Columns.Add("创建时间", typeof(DateTime));

                // 计算当前页起始和结束索引
                int startIndex = _currentPageIndex * _pageSize;
                int endIndex = Math.Min(startIndex + _pageSize, _allProducts.Count);

                // 填充当前页数据
                for (int i = startIndex; i < endIndex; i++)
                {
                    var product = _allProducts[i];
                    currentPageTable.Rows.Add(
                        product.ProductId,
                        product.ProductCode,
                        product.ProductName,
                        product.Specification,
                        product.Unit,
                        product.Category,
                        product.MaterialCode,
                        product.IsActive ? "启用" : "禁用",
                        product.CreateTime
                    );
                }

                // 更新数据源
                dataGridViewProducts.DataSource = currentPageTable;

                // 更新页码显示
                lblPageInfo.Text = $"第 {_currentPageIndex + 1} 页，共 {_totalPages} 页";

                // 启用/禁用分页按钮
                btnFirstPage.Enabled = _currentPageIndex > 0;
                btnPrevPage.Enabled = _currentPageIndex > 0;
                btnNextPage.Enabled = _currentPageIndex < _totalPages - 1;
                btnLastPage.Enabled = _currentPageIndex < _totalPages - 1;

                // 更新记录数量显示
                lblRecordCount.Text = $"共 {_allProducts.Count} 条记录";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新分页显示时出错");
                MessageBox.Show($"更新分页显示时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterProducts()
        {
            try
            {
                // 获取搜索条件
                string searchText = txtSearch.Text.Trim().ToLower();
                string category = cmbCategory.SelectedItem?.ToString();
                string status = cmbStatus.SelectedItem?.ToString();
                
                if (category == "全部") category = null;

                // 清空列表并重新筛选
                List<Product> filteredProducts = new List<Product>();

                // 从数据库或缓存获取所有产品
                var allProductsSource = _dbContext.Products.ToList();  // 实际应用中可以优化为缓存

                foreach (var product in allProductsSource)
                {
                    bool matchSearch = string.IsNullOrEmpty(searchText) ||
                                      product.ProductCode.ToLower().Contains(searchText) ||
                                      product.ProductName.ToLower().Contains(searchText) ||
                                      (product.Specification?.ToLower()?.Contains(searchText) ?? false) ||
                                      (product.MaterialCode?.ToLower()?.Contains(searchText) ?? false);

                    bool matchCategory = string.IsNullOrEmpty(category) ||
                                       product.Category == category;

                    bool matchStatus = string.IsNullOrEmpty(status) ||
                                     (status == "启用" && product.IsActive) ||
                                     (status == "禁用" && !product.IsActive);

                    if (matchSearch && matchCategory && matchStatus)
                    {
                        filteredProducts.Add(product);
                    }
                }

                // 更新产品列表
                _allProducts = filteredProducts;

                // 更新总页数
                _totalPages = (_allProducts.Count + _pageSize - 1) / _pageSize;
                if (_totalPages == 0) _totalPages = 1;

                // 重置为第一页
                _currentPageIndex = 0;

                // 更新显示
                UpdatePageDisplay();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "筛选产品数据时出错");
                MessageBox.Show($"筛选产品数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region 事件处理

        private void DataGridViewProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 忽略列头和无效行
            if (e.RowIndex < 0) return;
            
            EditSelectedProduct();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FilterProducts();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // 清空搜索条件
            txtSearch.Text = string.Empty;
            cmbCategory.SelectedIndex = 0;
            cmbStatus.SelectedIndex = -1;
            
            // 重新加载所有数据
            LoadProductData();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            OpenProductEditForm(null);
        }

        private void btnEditProduct_Click(object sender, EventArgs e)
        {
            EditSelectedProduct();
        }

        private void EditSelectedProduct()
        {
            try
            {
                // 获取选中的行
                if (dataGridViewProducts.CurrentRow != null)
                {
                    // 获取产品ID
                    int productId = Convert.ToInt32(dataGridViewProducts.CurrentRow.Cells["产品ID"].Value);
                    
                    // 查找对应的产品对象
                    Product selectedProduct = _allProducts.FirstOrDefault(p => p.ProductId == productId);
                    
                    if (selectedProduct != null)
                    {
                        OpenProductEditForm(selectedProduct);
                    }
                }
                else
                {
                    MessageBox.Show("请先选择一个产品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "编辑产品时出错");
                MessageBox.Show($"编辑产品时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenProductEditForm(Product product)
        {
            try
            {
                // 创建并显示产品编辑窗体
                using (var productEditForm = Program.ServiceProvider.GetRequiredService<ProductEditForm>())
                {
                    productEditForm.SetProduct(product);
                    if (productEditForm.ShowDialog() == DialogResult.OK)
                    {
                        // 刷新产品列表
                        LoadProductData();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "打开产品编辑窗体时出错");
                MessageBox.Show($"打开产品编辑窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取选中的行
                if (dataGridViewProducts.CurrentRow != null)
                {
                    // 获取产品ID和名称
                    int productId = Convert.ToInt32(dataGridViewProducts.CurrentRow.Cells["产品ID"].Value);
                    string productName = dataGridViewProducts.CurrentRow.Cells["产品名称"].Value.ToString();
                    
                    // 确认删除
                    DialogResult result = MessageBox.Show($"确定要删除产品 [{productName}] 吗？\n\n删除产品将同时删除相关的BOM和工艺路线信息！", 
                        "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    
                    if (result == DialogResult.Yes)
                    {
                        // 从数据库中删除
                        Product productToDelete = _dbContext.Products.Find(productId);
                        if (productToDelete != null)
                        {
                            // 检查是否存在引用关系
                            int bomCount = _dbContext.BOMs.Count(b => b.ComponentId == productId);
                            if (bomCount > 0)
                            {
                                MessageBox.Show($"无法删除产品 [{productName}]，该产品被其它产品的BOM引用。", 
                                    "删除失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            
                            // 删除关联的BOM数据
                            var boms = _dbContext.BOMs.Where(b => b.ProductId == productId).ToList();
                            foreach (var bom in boms)
                            {
                                _dbContext.BOMs.Remove(bom);
                            }
                            
                            // 删除关联的工序数据
                            var routings = _dbContext.ProductRoutings.Include(r => r.Operations).Where(r => r.ProductId == productId).ToList();
                            foreach (var routing in routings)
                            {
                                foreach (var operation in routing.Operations.ToList())
                                {
                                    _dbContext.RoutingOperations.Remove(operation);
                                }
                                _dbContext.ProductRoutings.Remove(routing);
                            }

                            // 删除产品
                            _dbContext.Products.Remove(productToDelete);
                            _dbContext.SaveChanges();

                            // 重新加载数据
                            LoadProductData();
                            
                            MessageBox.Show("产品删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("产品不存在或已被删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请先选择一个产品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除产品时出错");
                MessageBox.Show($"删除产品时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewBOM_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取选中的行
                if (dataGridViewProducts.CurrentRow != null)
                {
                    // 获取产品ID和名称
                    int productId = Convert.ToInt32(dataGridViewProducts.CurrentRow.Cells["产品ID"].Value);
                    string productName = dataGridViewProducts.CurrentRow.Cells["产品名称"].Value.ToString();
                    
                    // 打开BOM管理窗口
                    using (var bomManagementForm = Program.ServiceProvider.GetRequiredService<BOMManagementForm>())
                    {
                        bomManagementForm.SetProduct(productId, productName);
                        bomManagementForm.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("请先选择一个产品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查看产品BOM时出错");
                MessageBox.Show($"查看产品BOM时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewRouting_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取选中的行
                if (dataGridViewProducts.CurrentRow != null)
                {
                    // 获取产品ID和名称
                    int productId = Convert.ToInt32(dataGridViewProducts.CurrentRow.Cells["产品ID"].Value);
                    string productName = dataGridViewProducts.CurrentRow.Cells["产品名称"].Value.ToString();
                    
                    // 打开工艺路线管理窗口
                    using (var routingManagementForm = Program.ServiceProvider.GetRequiredService<RoutingManagementForm>())
                    {
                        routingManagementForm.SetProduct(productId, productName);
                        routingManagementForm.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("请先选择一个产品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查看产品工艺路线时出错");
                MessageBox.Show($"查看产品工艺路线时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            _currentPageIndex = 0;
            UpdatePageDisplay();
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (_currentPageIndex > 0)
            {
                _currentPageIndex--;
                UpdatePageDisplay();
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (_currentPageIndex < _totalPages - 1)
            {
                _currentPageIndex++;
                UpdatePageDisplay();
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            _currentPageIndex = _totalPages - 1;
            UpdatePageDisplay();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FilterProducts();
                e.SuppressKeyPress = true;
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }

        #endregion
    }
} 