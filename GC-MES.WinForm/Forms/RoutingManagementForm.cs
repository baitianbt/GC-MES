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
    public partial class RoutingManagementForm : Form
    {
        private readonly ILogger<RoutingManagementForm> _logger;
        private readonly IConfiguration _configuration;
        private readonly MesDbContext _dbContext;
        
        // 当前编辑的产品ID和名称
        private int _productId;
        private string _productName;
        
        // 工艺路线数据
        private List<ProductRouting> _routingList = new List<ProductRouting>();
        
        public RoutingManagementForm(ILogger<RoutingManagementForm> logger, IConfiguration configuration, MesDbContext dbContext)
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
            this.Text = $"工艺路线管理 - {_productName}";
            lblProductName.Text = _productName;
        }

        private void RoutingManagementForm_Load(object sender, EventArgs e)
        {
            // 初始化数据网格视图
            InitializeDataGridView();
            
            // 加载工艺路线数据
            LoadRoutingData();
        }

        private void InitializeDataGridView()
        {
            // 设置列宽
            dataGridViewRouting.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            // 禁止添加行
            dataGridViewRouting.AllowUserToAddRows = false;
            
            // 整行选择
            dataGridViewRouting.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            // 只读
            dataGridViewRouting.ReadOnly = true;
            
            // 交替行颜色
            dataGridViewRouting.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            
            // 双击编辑工艺路线
            dataGridViewRouting.CellDoubleClick += DataGridViewRouting_CellDoubleClick;
        }

        private void LoadRoutingData()
        {
            try
            {
                // 清空现有数据
                _routingList.Clear();

                try
                {
                    // 从数据库加载当前产品的工艺路线数据
                    _routingList = _dbContext.ProductRoutings
                        .Where(r => r.ProductId == _productId)
                        .OrderBy(r => r.RoutingSequence)
                        .ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "从数据库加载工艺路线数据时发生错误");
                    
                    // 如果数据库加载失败，则使用模拟数据
                    _routingList = new List<ProductRouting>
                    {
                        new ProductRouting
                        {
                            RoutingId = 1,
                            ProductId = _productId,
                            RoutingName = "标准工艺",
                            Description = "标准制造工艺路线",
                            RoutingSequence = 10,
                            Version = "1.0",
                            IsDefault = true,
                            IsActive = true
                        },
                        new ProductRouting
                        {
                            RoutingId = 2,
                            ProductId = _productId,
                            RoutingName = "备用工艺",
                            Description = "备用制造工艺路线",
                            RoutingSequence = 20,
                            Version = "1.0",
                            IsDefault = false,
                            IsActive = true
                        }
                    };
                }

                // 更新数据显示
                UpdateRoutingDisplay();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载工艺路线数据时出错");
                MessageBox.Show($"加载工艺路线数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateRoutingDisplay()
        {
            try
            {
                // 清空当前绑定数据
                dataGridViewRouting.DataSource = null;

                // 创建数据表
                DataTable routingTable = new DataTable();
                routingTable.Columns.Add("工艺ID", typeof(int));
                routingTable.Columns.Add("工艺名称", typeof(string));
                routingTable.Columns.Add("工艺说明", typeof(string));
                routingTable.Columns.Add("版本", typeof(string));
                routingTable.Columns.Add("序号", typeof(int));
                routingTable.Columns.Add("默认工艺", typeof(bool));
                routingTable.Columns.Add("状态", typeof(string));
                routingTable.Columns.Add("创建时间", typeof(DateTime));
                routingTable.Columns.Add("创建人", typeof(string));

                // 填充数据
                foreach (var routing in _routingList)
                {
                    routingTable.Rows.Add(
                        routing.RoutingId,
                        routing.RoutingName,
                        routing.Description,
                        routing.Version,
                        routing.RoutingSequence,
                        routing.IsDefault,
                        routing.IsActive ? "启用" : "禁用",
                        routing.CreateTime,
                        routing.CreateBy
                    );
                }

                // 更新数据源
                dataGridViewRouting.DataSource = routingTable;

                // 更新记录数量显示
                lblRecordCount.Text = $"共 {_routingList.Count} 条记录";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新工艺路线显示时出错");
                MessageBox.Show($"更新工艺路线显示时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region 事件处理
        
        private void DataGridViewRouting_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 忽略列头和无效行
            if (e.RowIndex < 0) return;
            
            EditSelectedRouting();
        }

        private void btnAddRouting_Click(object sender, EventArgs e)
        {
            OpenRoutingEditForm(null);
        }

        private void btnEditRouting_Click(object sender, EventArgs e)
        {
            EditSelectedRouting();
        }

        private void EditSelectedRouting()
        {
            try
            {
                // 获取选中的行
                if (dataGridViewRouting.CurrentRow != null)
                {
                    // 获取工艺路线ID
                    int routingId = Convert.ToInt32(dataGridViewRouting.CurrentRow.Cells["工艺ID"].Value);
                    
                    // 查找对应的工艺路线对象
                    ProductRouting selectedRouting = _routingList.FirstOrDefault(r => r.RoutingId == routingId);
                    
                    if (selectedRouting != null)
                    {
                        OpenRoutingEditForm(selectedRouting);
                    }
                }
                else
                {
                    MessageBox.Show("请先选择一条工艺路线", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "编辑工艺路线时出错");
                MessageBox.Show($"编辑工艺路线时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenRoutingEditForm(ProductRouting routing)
        {
            try
            {
                // 创建并显示工艺路线编辑窗体
                using (var routingEditForm = Program.ServiceProvider.GetRequiredService<RoutingEditForm>())
                {
                    routingEditForm.SetRouting(routing, _productId, _productName);
                    if (routingEditForm.ShowDialog() == DialogResult.OK)
                    {
                        // 刷新工艺路线列表
                        LoadRoutingData();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "打开工艺路线编辑窗体时出错");
                MessageBox.Show($"打开工艺路线编辑窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteRouting_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取选中的行
                if (dataGridViewRouting.CurrentRow != null)
                {
                    // 获取工艺路线ID和名称
                    int routingId = Convert.ToInt32(dataGridViewRouting.CurrentRow.Cells["工艺ID"].Value);
                    string routingName = dataGridViewRouting.CurrentRow.Cells["工艺名称"].Value.ToString();
                    
                    // 确认删除
                    DialogResult result = MessageBox.Show($"确定要从产品 [{_productName}] 的工艺路线中删除 [{routingName}] 吗？\n\n删除工艺路线将同时删除所有关联的工序信息！", 
                        "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    
                    if (result == DialogResult.Yes)
                    {
                        // 从数据库中删除
                        ProductRouting routingToDelete = _dbContext.ProductRoutings.Find(routingId);
                        if (routingToDelete != null)
                        {
                            // 删除关联的工序
                            var operations = _dbContext.RoutingOperations.Where(o => o.RoutingId == routingId).ToList();
                            foreach (var operation in operations)
                            {
                                _dbContext.RoutingOperations.Remove(operation);
                            }
                            
                            // 删除工艺路线
                            _dbContext.ProductRoutings.Remove(routingToDelete);
                            _dbContext.SaveChanges();

                            // 重新加载数据
                            LoadRoutingData();
                            
                            MessageBox.Show("工艺路线删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("工艺路线不存在或已被删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请先选择一条工艺路线", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除工艺路线时出错");
                MessageBox.Show($"删除工艺路线时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewOperations_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取选中的行
                if (dataGridViewRouting.CurrentRow != null)
                {
                    // 获取工艺路线ID和名称
                    int routingId = Convert.ToInt32(dataGridViewRouting.CurrentRow.Cells["工艺ID"].Value);
                    string routingName = dataGridViewRouting.CurrentRow.Cells["工艺名称"].Value.ToString();
                    
                    // 打开工序管理窗口
                    using (var operationManagementForm = Program.ServiceProvider.GetRequiredService<OperationManagementForm>())
                    {
                        operationManagementForm.SetRouting(routingId, routingName, _productId, _productName);
                        operationManagementForm.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("请先选择一条工艺路线", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查看工序时出错");
                MessageBox.Show($"查看工序时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadRoutingData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        #endregion
    }
} 