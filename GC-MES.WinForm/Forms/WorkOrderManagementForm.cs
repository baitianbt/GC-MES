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

namespace GC_MES.WinForm.Forms
{
    public partial class WorkOrderManagementForm : Form
    {
        private readonly ILogger<WorkOrderManagementForm> _logger;
        private readonly IConfiguration _configuration;

        // 模拟工单数据
        private DataTable _workOrdersDataTable;

        // 分页相关变量
        private int _currentPageIndex = 0;
        private int _pageSize = 10;
        private int _totalPages = 0;
        private List<DataRow> _allWorkOrders = new List<DataRow>();

        // 工单类型和状态列表（实际应从数据库加载）
        private readonly List<string> _orderTypes = new List<string> 
        { "生产工单", "维修工单", "质检工单", "返工工单", "物料申请单", "成品入库单" };
        
        private readonly List<string> _orderStatus = new List<string> 
        { "待处理", "进行中", "已完成", "已取消", "已暂停", "已延期" };
        
        private readonly List<string> _priorities = new List<string>
        { "紧急", "高", "中", "低" };

        public WorkOrderManagementForm(ILogger<WorkOrderManagementForm> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            
            InitializeComponent();
            InitializeDataTable();
            LoadWorkOrderData();
            
            // 设置下拉框数据源
            cmbOrderType.Items.AddRange(_orderTypes.ToArray());
            cmbOrderStatus.Items.AddRange(_orderStatus.ToArray());
            cmbPriority.Items.AddRange(_priorities.ToArray());
            
            // 应用当前主题
            ThemeManager.ApplyTheme(this);
        }

        /// <summary>
        /// 初始化数据表格式
        /// </summary>
        private void InitializeDataTable()
        {
            _workOrdersDataTable = new DataTable();
            _workOrdersDataTable.Columns.Add("工单编号", typeof(string));
            _workOrdersDataTable.Columns.Add("工单类型", typeof(string));
            _workOrdersDataTable.Columns.Add("工单标题", typeof(string));
            _workOrdersDataTable.Columns.Add("优先级", typeof(string));
            _workOrdersDataTable.Columns.Add("状态", typeof(string));
            _workOrdersDataTable.Columns.Add("创建人", typeof(string));
            _workOrdersDataTable.Columns.Add("创建时间", typeof(DateTime));
            _workOrdersDataTable.Columns.Add("计划开始时间", typeof(DateTime));
            _workOrdersDataTable.Columns.Add("计划完成时间", typeof(DateTime));
            _workOrdersDataTable.Columns.Add("实际完成时间", typeof(DateTime));
            _workOrdersDataTable.Columns.Add("负责人", typeof(string));
            _workOrdersDataTable.Columns.Add("备注", typeof(string));
        }

        /// <summary>
        /// 加载工单数据（模拟数据）
        /// </summary>
        private void LoadWorkOrderData()
        {
            try
            {
                // 清空现有数据
                _workOrdersDataTable.Clear();
                _allWorkOrders.Clear();

                // 添加示例数据（实际应从数据库加载）
                _allWorkOrders.Add(_workOrdersDataTable.Rows.Add("WO-20230001", "生产工单", "A型产品生产批次001", "高", "已完成", "张三", DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-9), DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-6), "李四", "按时完成"));
                _allWorkOrders.Add(_workOrdersDataTable.Rows.Add("WO-20230002", "质检工单", "A型产品质检批次001", "高", "已完成", "王五", DateTime.Now.AddDays(-9), DateTime.Now.AddDays(-9), DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-8), "赵六", "全部合格"));
                _allWorkOrders.Add(_workOrdersDataTable.Rows.Add("WO-20230003", "生产工单", "B型产品生产批次001", "中", "已完成", "张三", DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-7), DateTime.Now.AddDays(-3), DateTime.Now.AddDays(-4), "李四", "提前完成"));
                _allWorkOrders.Add(_workOrdersDataTable.Rows.Add("WO-20230004", "维修工单", "注塑机1号维护", "紧急", "已完成", "孙七", DateTime.Now.AddDays(-7), DateTime.Now.AddDays(-7), DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-6), "周八", "更换传感器"));
                _allWorkOrders.Add(_workOrdersDataTable.Rows.Add("WO-20230005", "质检工单", "B型产品质检批次001", "高", "已完成", "王五", DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-6), "赵六", "发现3个不良品"));
                _allWorkOrders.Add(_workOrdersDataTable.Rows.Add("WO-20230006", "返工工单", "B型产品返工批次001", "高", "已完成", "吴九", DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-5), "郑十", "返工合格"));
                _allWorkOrders.Add(_workOrdersDataTable.Rows.Add("WO-20230007", "生产工单", "C型产品生产批次001", "中", "已完成", "张三", DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-2), "李四", "按时完成"));
                _allWorkOrders.Add(_workOrdersDataTable.Rows.Add("WO-20230008", "质检工单", "C型产品质检批次001", "中", "已完成", "王五", DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-4), "赵六", "全部合格"));
                _allWorkOrders.Add(_workOrdersDataTable.Rows.Add("WO-20230009", "物料申请单", "D型产品原料申请", "低", "已完成", "黄十一", DateTime.Now.AddDays(-3), DateTime.Now.AddDays(-3), DateTime.Now.AddDays(-3), DateTime.Now.AddDays(-3), "林十二", ""));
                _allWorkOrders.Add(_workOrdersDataTable.Rows.Add("WO-20230010", "生产工单", "D型产品生产批次001", "中", "进行中", "张三", DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-2), DateTime.Now.AddDays(1), null, "李四", "按计划进行中"));
                _allWorkOrders.Add(_workOrdersDataTable.Rows.Add("WO-20230011", "维修工单", "包装线3号维护", "中", "进行中", "孙七", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1), DateTime.Now.AddDays(0), null, "周八", ""));
                _allWorkOrders.Add(_workOrdersDataTable.Rows.Add("WO-20230012", "生产工单", "E型产品生产批次001", "低", "待处理", "张三", DateTime.Now.AddDays(0), DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), null, "李四", ""));
                _allWorkOrders.Add(_workOrdersDataTable.Rows.Add("WO-20230013", "质检工单", "D型产品质检批次001", "中", "待处理", "王五", DateTime.Now.AddDays(0), DateTime.Now.AddDays(1), DateTime.Now.AddDays(1), null, "赵六", ""));
                _allWorkOrders.Add(_workOrdersDataTable.Rows.Add("WO-20230014", "物料申请单", "E型产品原料申请", "紧急", "待处理", "黄十一", DateTime.Now.AddDays(0), DateTime.Now.AddDays(0), DateTime.Now.AddDays(0), null, "林十二", "紧急补充原料"));

                // 更新总页数
                _totalPages = (_allWorkOrders.Count + _pageSize - 1) / _pageSize;
                if (_totalPages == 0) _totalPages = 1;

                // 设置当前页为第一页
                _currentPageIndex = 0;

                // 显示第一页数据
                UpdatePageDisplay();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载工单数据时出错");
                MessageBox.Show($"加载工单数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据当前页码更新数据显示
        /// </summary>
        private void UpdatePageDisplay()
        {
            try
            {
                // 清空当前绑定数据
                dataGridViewWorkOrders.DataSource = null;

                // 创建当前页数据表
                DataTable currentPageTable = _workOrdersDataTable.Clone();

                // 计算当前页起始和结束索引
                int startIndex = _currentPageIndex * _pageSize;
                int endIndex = Math.Min(startIndex + _pageSize, _allWorkOrders.Count);

                // 填充当前页数据
                for (int i = startIndex; i < endIndex; i++)
                {
                    currentPageTable.ImportRow(_allWorkOrders[i]);
                }

                // 更新数据源
                dataGridViewWorkOrders.DataSource = currentPageTable;

                // 更新页码显示
                lblPageInfo.Text = $"第 {_currentPageIndex + 1} 页，共 {_totalPages} 页";

                // 启用/禁用分页按钮
                btnFirstPage.Enabled = _currentPageIndex > 0;
                btnPrevPage.Enabled = _currentPageIndex > 0;
                btnNextPage.Enabled = _currentPageIndex < _totalPages - 1;
                btnLastPage.Enabled = _currentPageIndex < _totalPages - 1;

                // 更新记录数量显示
                lblRecordCount.Text = $"共 {_allWorkOrders.Count} 条记录";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新分页显示时出错");
                MessageBox.Show($"更新分页显示时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据搜索条件筛选工单
        /// </summary>
        private void FilterWorkOrders()
        {
            try
            {
                // 获取搜索条件
                string searchText = txtSearch.Text.Trim().ToLower();
                string orderType = cmbOrderType.SelectedItem?.ToString() ?? string.Empty;
                string status = cmbOrderStatus.SelectedItem?.ToString() ?? string.Empty;
                string priority = cmbPriority.SelectedItem?.ToString() ?? string.Empty;

                // 清空列表并重新筛选
                _allWorkOrders.Clear();

                foreach (DataRow row in _workOrdersDataTable.Rows)
                {
                    bool matchSearch = string.IsNullOrEmpty(searchText) ||
                                      row["工单编号"].ToString().ToLower().Contains(searchText) ||
                                      row["工单标题"].ToString().ToLower().Contains(searchText) ||
                                      row["创建人"].ToString().ToLower().Contains(searchText) ||
                                      row["负责人"].ToString().ToLower().Contains(searchText);

                    bool matchOrderType = string.IsNullOrEmpty(orderType) ||
                                         row["工单类型"].ToString() == orderType;

                    bool matchStatus = string.IsNullOrEmpty(status) ||
                                     row["状态"].ToString() == status;

                    bool matchPriority = string.IsNullOrEmpty(priority) ||
                                       row["优先级"].ToString() == priority;

                    if (matchSearch && matchOrderType && matchStatus && matchPriority)
                    {
                        _allWorkOrders.Add(row);
                    }
                }

                // 更新总页数
                _totalPages = (_allWorkOrders.Count + _pageSize - 1) / _pageSize;
                if (_totalPages == 0) _totalPages = 1;

                // 重置为第一页
                _currentPageIndex = 0;

                // 更新显示
                UpdatePageDisplay();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "筛选工单数据时出错");
                MessageBox.Show($"筛选工单数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region 事件处理

        private void WorkOrderManagementForm_Load(object sender, EventArgs e)
        {
            // 初始化数据网格视图
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            // 设置列宽
            dataGridViewWorkOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            // 禁止添加行
            dataGridViewWorkOrders.AllowUserToAddRows = false;
            
            // 整行选择
            dataGridViewWorkOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            // 只读
            dataGridViewWorkOrders.ReadOnly = true;
            
            // 交替行颜色
            dataGridViewWorkOrders.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            
            // 双击编辑工单
            dataGridViewWorkOrders.CellDoubleClick += DataGridViewWorkOrders_CellDoubleClick;
        }

        private void DataGridViewWorkOrders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 忽略列头和无效行
            if (e.RowIndex < 0) return;
            
            EditSelectedWorkOrder();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FilterWorkOrders();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // 清空搜索条件
            txtSearch.Text = string.Empty;
            cmbOrderType.SelectedIndex = -1;
            cmbOrderStatus.SelectedIndex = -1;
            cmbPriority.SelectedIndex = -1;
            
            // 重新加载所有数据
            LoadWorkOrderData();
        }

        private void btnAddWorkOrder_Click(object sender, EventArgs e)
        {
            // 打开新增工单表单
            MessageBox.Show("此功能尚未实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // OpenWorkOrderEditForm(null);
        }

        private void btnEditWorkOrder_Click(object sender, EventArgs e)
        {
            EditSelectedWorkOrder();
        }

        private void EditSelectedWorkOrder()
        {
            try
            {
                // 获取选中的行
                if (dataGridViewWorkOrders.CurrentRow != null)
                {
                    // 获取工单信息
                    DataRowView rowView = (DataRowView)dataGridViewWorkOrders.CurrentRow.DataBoundItem;
                    DataRow selectedWorkOrder = rowView.Row;
                    
                    // 打开编辑表单
                    MessageBox.Show("此功能尚未实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // OpenWorkOrderEditForm(selectedWorkOrder);
                }
                else
                {
                    MessageBox.Show("请先选择一个工单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "编辑工单时出错");
                MessageBox.Show($"编辑工单时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteWorkOrder_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取选中的行
                if (dataGridViewWorkOrders.CurrentRow != null)
                {
                    // 获取工单信息
                    DataRowView rowView = (DataRowView)dataGridViewWorkOrders.CurrentRow.DataBoundItem;
                    string workOrderId = rowView["工单编号"].ToString();
                    string workOrderTitle = rowView["工单标题"].ToString();
                    
                    // 确认删除
                    DialogResult result = MessageBox.Show($"确定要删除工单 [{workOrderId}] {workOrderTitle} 吗？", 
                        "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    
                    if (result == DialogResult.Yes)
                    {
                        // 从数据源中删除（实际应从数据库删除）
                        rowView.Delete();
                        
                        // 重新加载数据
                        LoadWorkOrderData();
                        
                        MessageBox.Show("工单删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("请先选择一个工单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除工单时出错");
                MessageBox.Show($"删除工单时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                FilterWorkOrders();
                e.SuppressKeyPress = true;
            }
        }

        private void cmbOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterWorkOrders();
        }

        private void cmbOrderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterWorkOrders();
        }

        private void cmbPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterWorkOrders();
        }
        
        #endregion
    }
} 