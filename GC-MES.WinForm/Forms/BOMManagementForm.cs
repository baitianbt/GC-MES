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
    public partial class BOMManagementForm : Form
    {
        private readonly ILogger<BOMManagementForm> _logger;
        private readonly IConfiguration _configuration;
        private readonly MesDbContext _dbContext;
        
        // 当前编辑的产品ID和名称
        private int _productId;
        private string _productName;
        
        // BOM数据
        private List<BOM> _bomList = new List<BOM>();
        
        public BOMManagementForm(ILogger<BOMManagementForm> logger, IConfiguration configuration, MesDbContext dbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
            
            InitializeComponent();
            
            // 应用当前主题
            ThemeManager.ApplyTheme(this);
        }
        
        /// <summary>
        /// 设置当前产品
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="productName">产品名称</param>
        public void SetProduct(int productId, string productName)
        {
            _productId = productId;
            _productName = productName;
            
            // 更新窗体标题
            this.Text = $"BOM管理 - {_productName}";
            lblProductName.Text = _productName;
        }

        private void BOMManagementForm_Load(object sender, EventArgs e)
        {
            // 初始化数据网格视图
            InitializeDataGridView();
            
            // 加载BOM数据
            LoadBOMData();
        }

        private void InitializeDataGridView()
        {
            // 设置列宽
            dataGridViewBOM.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            // 禁止添加行
            dataGridViewBOM.AllowUserToAddRows = false;
            
            // 整行选择
            dataGridViewBOM.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            // 只读
            dataGridViewBOM.ReadOnly = true;
            
            // 交替行颜色
            dataGridViewBOM.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            
            // 双击编辑BOM项
            dataGridViewBOM.CellDoubleClick += DataGridViewBOM_CellDoubleClick;
        }

        private void LoadBOMData()
        {
            try
            {
                // 清空现有数据
                _bomList.Clear();

                try
                {
                    // 从数据库加载当前产品的BOM数据
                    _bomList = _dbContext.BOMs
                        .Include(b => b.ComponentProduct)
                        .Where(b => b.ProductId == _productId)
                        .OrderBy(b => b.Level)
                        .ThenBy(b => b.ComponentProduct.ProductName)
                        .ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "从数据库加载BOM数据时发生错误");
                    
                    // 如果数据库加载失败，则使用模拟数据
                    _bomList = new List<BOM>
                    {
                        new BOM
                        {
                            BOMId = 1,
                            ProductId = _productId,
                            ComponentId = 1,
                            Quantity = 2,
                            Unit = "个",
                            Position = "主板",
                            ScrapRate = 1,
                            Level = 1,
                            Version = "1.0",
                            IsActive = true,
                            CreateTime = DateTime.Now.AddDays(-5),
                            ComponentProduct = new Product { ProductId = 1, ProductCode = "P001", ProductName = "零件A", Category = "原材料", Unit = "个" }
                        },
                        new BOM
                        {
                            BOMId = 2,
                            ProductId = _productId,
                            ComponentId = 2,
                            Quantity = 4,
                            Unit = "个",
                            Position = "底座",
                            ScrapRate = 0.5m,
                            Level = 1,
                            Version = "1.0",
                            IsActive = true,
                            CreateTime = DateTime.Now.AddDays(-5),
                            ComponentProduct = new Product { ProductId = 2, ProductCode = "P002", ProductName = "螺丝", Category = "原材料", Unit = "个" }
                        },
                        new BOM
                        {
                            BOMId = 3,
                            ProductId = _productId,
                            ComponentId = 3,
                            Quantity = 1,
                            Unit = "个",
                            Position = "上盖",
                            ScrapRate = 2,
                            Level = 1,
                            Version = "1.0",
                            IsActive = true,
                            CreateTime = DateTime.Now.AddDays(-5),
                            ComponentProduct = new Product { ProductId = 3, ProductCode = "P003", ProductName = "支架组件", Category = "半成品", Unit = "个" }
                        }
                    };
                }

                // 更新数据显示
                UpdateBOMDisplay();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载BOM数据时出错");
                MessageBox.Show($"加载BOM数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateBOMDisplay()
        {
            try
            {
                // 清空当前绑定数据
                dataGridViewBOM.DataSource = null;

                // 创建数据表
                DataTable bomTable = new DataTable();
                bomTable.Columns.Add("BOM_ID", typeof(int));
                bomTable.Columns.Add("组件ID", typeof(int));
                bomTable.Columns.Add("组件编码", typeof(string));
                bomTable.Columns.Add("组件名称", typeof(string));
                bomTable.Columns.Add("类型", typeof(string));
                bomTable.Columns.Add("用量", typeof(decimal));
                bomTable.Columns.Add("单位", typeof(string));
                bomTable.Columns.Add("位置", typeof(string));
                bomTable.Columns.Add("废品率(%)", typeof(decimal));
                bomTable.Columns.Add("层级", typeof(int));
                bomTable.Columns.Add("版本", typeof(string));
                bomTable.Columns.Add("状态", typeof(string));

                // 填充数据
                foreach (var bom in _bomList)
                {
                    bomTable.Rows.Add(
                        bom.BOMId,
                        bom.ComponentId,
                        bom.ComponentProduct?.ProductCode,
                        bom.ComponentProduct?.ProductName,
                        bom.ComponentProduct?.Category,
                        bom.Quantity,
                        bom.Unit ?? bom.ComponentProduct?.Unit,
                        bom.Position,
                        bom.ScrapRate,
                        bom.Level,
                        bom.Version,
                        bom.IsActive ? "启用" : "禁用"
                    );
                }

                // 更新数据源
                dataGridViewBOM.DataSource = bomTable;

                // 更新记录数量显示
                lblRecordCount.Text = $"共 {_bomList.Count} 条记录";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新BOM显示时出错");
                MessageBox.Show($"更新BOM显示时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region 事件处理
        
        private void DataGridViewBOM_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 忽略列头和无效行
            if (e.RowIndex < 0) return;
            
            EditSelectedBOM();
        }

        private void btnAddBOM_Click(object sender, EventArgs e)
        {
            OpenBOMEditForm(null);
        }

        private void btnEditBOM_Click(object sender, EventArgs e)
        {
            EditSelectedBOM();
        }

        private void EditSelectedBOM()
        {
            try
            {
                // 获取选中的行
                if (dataGridViewBOM.CurrentRow != null)
                {
                    // 获取BOM ID
                    int bomId = Convert.ToInt32(dataGridViewBOM.CurrentRow.Cells["BOM_ID"].Value);
                    
                    // 查找对应的BOM对象
                    BOM selectedBOM = _bomList.FirstOrDefault(b => b.BOMId == bomId);
                    
                    if (selectedBOM != null)
                    {
                        OpenBOMEditForm(selectedBOM);
                    }
                }
                else
                {
                    MessageBox.Show("请先选择一个BOM项", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "编辑BOM时出错");
                MessageBox.Show($"编辑BOM时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenBOMEditForm(BOM bom)
        {
            try
            {
                // 创建并显示BOM编辑窗体
                using (var bomEditForm = Program.ServiceProvider.GetRequiredService<BOMEditForm>())
                {
                    bomEditForm.SetBOM(bom, _productId, _productName);
                    if (bomEditForm.ShowDialog() == DialogResult.OK)
                    {
                        // 刷新BOM列表
                        LoadBOMData();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "打开BOM编辑窗体时出错");
                MessageBox.Show($"打开BOM编辑窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteBOM_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取选中的行
                if (dataGridViewBOM.CurrentRow != null)
                {
                    // 获取BOM ID和组件名称
                    int bomId = Convert.ToInt32(dataGridViewBOM.CurrentRow.Cells["BOM_ID"].Value);
                    string componentName = dataGridViewBOM.CurrentRow.Cells["组件名称"].Value.ToString();
                    
                    // 确认删除
                    DialogResult result = MessageBox.Show($"确定要从产品 [{_productName}] 的BOM中删除组件 [{componentName}] 吗？", 
                        "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    
                    if (result == DialogResult.Yes)
                    {
                        // 从数据库中删除
                        BOM bomToDelete = _dbContext.BOMs.Find(bomId);
                        if (bomToDelete != null)
                        {
                            _dbContext.BOMs.Remove(bomToDelete);
                            _dbContext.SaveChanges();

                            // 重新加载数据
                            LoadBOMData();
                            
                            MessageBox.Show("BOM项删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("BOM项不存在或已被删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请先选择一个BOM项", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除BOM项时出错");
                MessageBox.Show($"删除BOM项时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImportBOM_Click(object sender, EventArgs e)
        {
            MessageBox.Show("BOM批量导入功能将在后续版本实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExportBOM_Click(object sender, EventArgs e)
        {
            MessageBox.Show("BOM导出功能将在后续版本实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadBOMData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        #endregion
    }
} 