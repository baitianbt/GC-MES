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
    public partial class UserManagementForm : Form
    {
        private readonly ILogger<UserManagementForm> _logger;
        private readonly IConfiguration _configuration;

        // 模拟用户数据
        private DataTable _usersDataTable;

        // 分页相关变量
        private int _currentPageIndex = 0;
        private int _pageSize = 10;
        private int _totalPages = 0;
        private List<DataRow> _allUsers = new List<DataRow>();

        // 部门和角色列表（实际应从数据库加载）
        private readonly List<string> _departments = new List<string> 
        { "IT部门", "生产部", "质检部", "仓库", "设备部", "人力资源部", "财务部" };
        
        private readonly List<string> _roles = new List<string> 
        { "管理员", "主管", "操作员", "质检员", "仓管员", "维修工程师", "普通用户" };

        public UserManagementForm(ILogger<UserManagementForm> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            
            InitializeComponent();
            InitializeDataTable();
            LoadUserData();
            
            // 设置下拉框数据源
            cmbDepartment.Items.AddRange(_departments.ToArray());
            cmbRole.Items.AddRange(_roles.ToArray());
            
            // 应用当前主题
            ThemeManager.ApplyTheme(this);
        }

        /// <summary>
        /// 初始化数据表格式
        /// </summary>
        private void InitializeDataTable()
        {
            _usersDataTable = new DataTable();
            _usersDataTable.Columns.Add("用户ID", typeof(string));
            _usersDataTable.Columns.Add("用户名", typeof(string));
            _usersDataTable.Columns.Add("姓名", typeof(string));
            _usersDataTable.Columns.Add("部门", typeof(string));
            _usersDataTable.Columns.Add("角色", typeof(string));
            _usersDataTable.Columns.Add("状态", typeof(string));
            _usersDataTable.Columns.Add("创建时间", typeof(DateTime));
            _usersDataTable.Columns.Add("最后登录", typeof(DateTime));
            _usersDataTable.Columns.Add("备注", typeof(string));
        }

        /// <summary>
        /// 加载用户数据（模拟数据）
        /// </summary>
        private void LoadUserData()
        {
            try
            {
                // 清空现有数据
                _usersDataTable.Clear();
                _allUsers.Clear();

                // 添加示例数据（实际应从数据库加载）
                _allUsers.Add(_usersDataTable.Rows.Add("001", "admin", "系统管理员", "IT部门", "管理员", "启用", DateTime.Now.AddMonths(-6), DateTime.Now.AddDays(-1), "系统内置管理员账户"));
                _allUsers.Add(_usersDataTable.Rows.Add("002", "zhangsan", "张三", "生产部", "主管", "启用", DateTime.Now.AddMonths(-3), DateTime.Now.AddDays(-3), "生产部总负责人"));
                _allUsers.Add(_usersDataTable.Rows.Add("003", "lisi", "李四", "仓库", "仓管员", "启用", DateTime.Now.AddMonths(-2), DateTime.Now.AddDays(-2), "负责原材料仓库"));
                _allUsers.Add(_usersDataTable.Rows.Add("004", "wangwu", "王五", "质检部", "质检员", "停用", DateTime.Now.AddMonths(-1), DateTime.Now.AddDays(-15), ""));
                _allUsers.Add(_usersDataTable.Rows.Add("005", "zhaoliu", "赵六", "设备部", "维修工程师", "启用", DateTime.Now.AddDays(-15), DateTime.Now.AddHours(-12), ""));
                _allUsers.Add(_usersDataTable.Rows.Add("006", "sunqi", "孙七", "生产部", "操作员", "启用", DateTime.Now.AddDays(-10), DateTime.Now.AddHours(-8), ""));
                _allUsers.Add(_usersDataTable.Rows.Add("007", "zhouba", "周八", "质检部", "质检员", "启用", DateTime.Now.AddDays(-8), DateTime.Now.AddHours(-24), ""));
                _allUsers.Add(_usersDataTable.Rows.Add("008", "wujiu", "吴九", "仓库", "仓管员", "启用", DateTime.Now.AddDays(-7), DateTime.Now.AddHours(-36), "负责成品仓库"));
                _allUsers.Add(_usersDataTable.Rows.Add("009", "zhengshi", "郑十", "人力资源部", "主管", "启用", DateTime.Now.AddMonths(-4), DateTime.Now.AddDays(-4), ""));
                _allUsers.Add(_usersDataTable.Rows.Add("010", "huangsw", "黄十一", "财务部", "主管", "启用", DateTime.Now.AddMonths(-5), DateTime.Now.AddDays(-5), ""));
                _allUsers.Add(_usersDataTable.Rows.Add("011", "linse", "林十二", "IT部门", "操作员", "启用", DateTime.Now.AddDays(-25), DateTime.Now.AddHours(-48), ""));
                _allUsers.Add(_usersDataTable.Rows.Add("012", "chenst", "陈十三", "设备部", "操作员", "停用", DateTime.Now.AddMonths(-1), DateTime.Now.AddDays(-30), "已离职"));

                // 更新总页数
                _totalPages = (_allUsers.Count + _pageSize - 1) / _pageSize;
                if (_totalPages == 0) _totalPages = 1;

                // 设置当前页为第一页
                _currentPageIndex = 0;

                // 显示第一页数据
                UpdatePageDisplay();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载用户数据时出错");
                MessageBox.Show($"加载用户数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                dataGridViewUsers.DataSource = null;

                // 创建当前页数据表
                DataTable currentPageTable = _usersDataTable.Clone();

                // 计算当前页起始和结束索引
                int startIndex = _currentPageIndex * _pageSize;
                int endIndex = Math.Min(startIndex + _pageSize, _allUsers.Count);

                // 填充当前页数据
                for (int i = startIndex; i < endIndex; i++)
                {
                    currentPageTable.ImportRow(_allUsers[i]);
                }

                // 更新数据源
                dataGridViewUsers.DataSource = currentPageTable;

                // 更新页码显示
                lblPageInfo.Text = $"第 {_currentPageIndex + 1} 页，共 {_totalPages} 页";

                // 启用/禁用分页按钮
                btnFirstPage.Enabled = _currentPageIndex > 0;
                btnPrevPage.Enabled = _currentPageIndex > 0;
                btnNextPage.Enabled = _currentPageIndex < _totalPages - 1;
                btnLastPage.Enabled = _currentPageIndex < _totalPages - 1;

                // 更新记录数量显示
                lblRecordCount.Text = $"共 {_allUsers.Count} 条记录";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新分页显示时出错");
                MessageBox.Show($"更新分页显示时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据搜索条件筛选用户
        /// </summary>
        private void FilterUsers()
        {
            try
            {
                // 获取搜索条件
                string searchText = txtSearch.Text.Trim().ToLower();
                string department = cmbDepartment.SelectedItem?.ToString() ?? string.Empty;
                string role = cmbRole.SelectedItem?.ToString() ?? string.Empty;
                string status = cmbStatus.SelectedItem?.ToString() ?? string.Empty;

                // 清空列表并重新筛选
                _allUsers.Clear();

                foreach (DataRow row in _usersDataTable.Rows)
                {
                    bool matchSearch = string.IsNullOrEmpty(searchText) ||
                                       row["用户ID"].ToString().ToLower().Contains(searchText) ||
                                       row["用户名"].ToString().ToLower().Contains(searchText) ||
                                       row["姓名"].ToString().ToLower().Contains(searchText);

                    bool matchDepartment = string.IsNullOrEmpty(department) ||
                                          row["部门"].ToString() == department;

                    bool matchRole = string.IsNullOrEmpty(role) ||
                                    row["角色"].ToString() == role;

                    bool matchStatus = string.IsNullOrEmpty(status) ||
                                      row["状态"].ToString() == status;

                    if (matchSearch && matchDepartment && matchRole && matchStatus)
                    {
                        _allUsers.Add(row);
                    }
                }

                // 更新总页数
                _totalPages = (_allUsers.Count + _pageSize - 1) / _pageSize;
                if (_totalPages == 0) _totalPages = 1;

                // 重置为第一页
                _currentPageIndex = 0;

                // 更新显示
                UpdatePageDisplay();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "筛选用户数据时出错");
                MessageBox.Show($"筛选用户数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region 事件处理

        private void UserManagementForm_Load(object sender, EventArgs e)
        {
            // 初始化状态下拉框
            cmbStatus.Items.AddRange(new string[] { "启用", "停用" });
            
            // 初始化数据网格视图
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            // 设置列宽
            dataGridViewUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            // 禁止添加行
            dataGridViewUsers.AllowUserToAddRows = false;
            
            // 整行选择
            dataGridViewUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            // 只读
            dataGridViewUsers.ReadOnly = true;
            
            // 交替行颜色
            dataGridViewUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            
            // 双击编辑用户
            dataGridViewUsers.CellDoubleClick += DataGridViewUsers_CellDoubleClick;
        }

        private void DataGridViewUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 忽略列头和无效行
            if (e.RowIndex < 0) return;
            
            EditSelectedUser();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FilterUsers();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // 清空搜索条件
            txtSearch.Text = string.Empty;
            cmbDepartment.SelectedIndex = -1;
            cmbRole.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
            
            // 重新加载所有用户
            LoadUserData();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            OpenUserEditForm(null);
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            EditSelectedUser();
        }

        private void EditSelectedUser()
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                DataRowView selectedRow = dataGridViewUsers.SelectedRows[0].DataBoundItem as DataRowView;
                if (selectedRow != null)
                {
                    OpenUserEditForm(selectedRow.Row);
                }
            }
            else
            {
                MessageBox.Show("请先选择要编辑的用户", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OpenUserEditForm(DataRow selectedUser)
        {
            using (UserEditForm editForm = new UserEditForm(_logger, _departments, _roles))
            {
                // 应用当前主题
                ThemeManager.ApplyTheme(editForm);
                
                // 设置编辑模式和数据
                editForm.SetUserData(selectedUser);
                
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    // 获取编辑后的数据
                    DataRow userData = editForm.GetUserData();
                    
                    if (selectedUser == null)
                    {
                        // 添加新用户
                        DataRow newRow = _usersDataTable.NewRow();
                        foreach (DataColumn col in _usersDataTable.Columns)
                        {
                            if (userData.Table.Columns.Contains(col.ColumnName))
                            {
                                newRow[col.ColumnName] = userData[col.ColumnName];
                            }
                        }
                        
                        // 默认用户ID为当前最大ID+1
                        int maxId = 0;
                        foreach (DataRow row in _usersDataTable.Rows)
                        {
                            int id;
                            if (int.TryParse(row["用户ID"].ToString(), out id))
                            {
                                maxId = Math.Max(maxId, id);
                            }
                        }
                        newRow["用户ID"] = (maxId + 1).ToString("D3");
                        
                        // 设置创建时间
                        newRow["创建时间"] = DateTime.Now;
                        
                        _usersDataTable.Rows.Add(newRow);
                        _allUsers.Add(newRow);
                    }
                    else
                    {
                        // 更新现有用户
                        foreach (DataColumn col in _usersDataTable.Columns)
                        {
                            if (userData.Table.Columns.Contains(col.ColumnName) && 
                                col.ColumnName != "用户ID" && col.ColumnName != "创建时间")
                            {
                                selectedUser[col.ColumnName] = userData[col.ColumnName];
                            }
                        }
                    }
                    
                    // 更新显示
                    UpdatePageDisplay();
                }
            }
        }

        //private void btnDeleteUser_Click(object sender, EventArgs e)
        //{
        //    if (dataGridViewUsers.SelectedRows.Count > 0)
        //    {
        //        DataRowView selectedRow = dataGridViewUsers.SelectedRows[0].DataBoundItem as DataRowView;
        //        if (selectedRow != null)
        //        {
        //            string userId = selectedRow["用户ID"].ToString();
        //            string username = selectedRow["用户名"].ToString();
                    
        //            if (userId == "001" && username == "admin")
        //            {
        //                MessageBox.Show("系统管理员账户不能删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                return;
        //            }
                    
        //            if (MessageBox.Show($"确定要删除用户"{username}"吗？\n删除后将无法恢复！", 
        //                "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //            {
        //                // 在全局数据和当前视图中删除
        //                DataRow rowToDelete = null;
        //                foreach (DataRow row in _usersDataTable.Rows)
        //                {
        //                    if (row["用户ID"].ToString() == userId)
        //                    {
        //                        rowToDelete = row;
        //                        break;
        //                    }
        //                }
                        
        //                if (rowToDelete != null)
        //                {
        //                    _allUsers.Remove(rowToDelete);
        //                    rowToDelete.Delete();
        //                }
                        
        //                // 刷新显示
        //                UpdatePageDisplay();
                        
        //                _logger.LogInformation($"已删除用户: {userId} - {username}");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("请先选择要删除的用户", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}

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
                btnSearch_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterUsers();
        }

        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterUsers();
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterUsers();
        }

        #endregion
    }
}