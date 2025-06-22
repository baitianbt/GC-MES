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
    public partial class RoleManagementForm : Form
    {
        private readonly ILogger<RoleManagementForm> _logger;
        private readonly IConfiguration _configuration;
        private readonly MesDbContext _dbContext;

        // 分页相关变量
        private int _currentPageIndex = 0;
        private int _pageSize = 10;
        private int _totalPages = 0;
        private List<Role> _allRoles = new List<Role>();

        public RoleManagementForm(ILogger<RoleManagementForm> logger, IConfiguration configuration, MesDbContext dbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
            
            InitializeComponent();
            
            // 应用当前主题
            ThemeManager.ApplyTheme(this);
        }

        private void RoleManagementForm_Load(object sender, EventArgs e)
        {
            // 初始化数据网格视图
            InitializeDataGridView();
            
            // 加载角色数据
            LoadRoleData();
        }

        private void InitializeDataGridView()
        {
            // 设置列宽
            dataGridViewRoles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            // 禁止添加行
            dataGridViewRoles.AllowUserToAddRows = false;
            
            // 整行选择
            dataGridViewRoles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            // 只读
            dataGridViewRoles.ReadOnly = true;
            
            // 交替行颜色
            dataGridViewRoles.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            
            // 双击编辑角色
            dataGridViewRoles.CellDoubleClick += DataGridViewRoles_CellDoubleClick;
        }

        private void LoadRoleData()
        {
            try
            {
                // 清空现有数据
                _allRoles.Clear();

                try
                {
                    // 从数据库加载数据
                    _allRoles = _dbContext.Roles.OrderBy(r => r.RoleId).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "从数据库加载角色数据时发生错误");
                    
                    // 如果数据库加载失败，则使用模拟数据
                    _allRoles = new List<Role>
                    {
                        new Role { RoleId = 1, RoleName = "系统管理员", Description = "系统管理员，拥有所有权限", IsActive = true, CreateTime = DateTime.Now.AddMonths(-6), CreateBy = "admin" },
                        new Role { RoleId = 2, RoleName = "生产主管", Description = "生产部门主管", IsActive = true, CreateTime = DateTime.Now.AddMonths(-5), CreateBy = "admin" },
                        new Role { RoleId = 3, RoleName = "质检员", Description = "负责质量检测", IsActive = true, CreateTime = DateTime.Now.AddMonths(-4), CreateBy = "admin" },
                        new Role { RoleId = 4, RoleName = "仓库管理员", Description = "负责仓库管理", IsActive = true, CreateTime = DateTime.Now.AddMonths(-3), CreateBy = "admin" },
                        new Role { RoleId = 5, RoleName = "设备维护员", Description = "负责设备维护", IsActive = true, CreateTime = DateTime.Now.AddMonths(-2), CreateBy = "admin" },
                        new Role { RoleId = 6, RoleName = "普通用户", Description = "基本操作权限", IsActive = true, CreateTime = DateTime.Now.AddMonths(-1), CreateBy = "admin" }
                    };
                }

                // 更新总页数
                _totalPages = (_allRoles.Count + _pageSize - 1) / _pageSize;
                if (_totalPages == 0) _totalPages = 1;

                // 设置当前页为第一页
                _currentPageIndex = 0;

                // 显示第一页数据
                UpdatePageDisplay();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载角色数据时出错");
                MessageBox.Show($"加载角色数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdatePageDisplay()
        {
            try
            {
                // 清空当前绑定数据
                dataGridViewRoles.DataSource = null;

                // 创建当前页数据表
                DataTable currentPageTable = new DataTable();
                currentPageTable.Columns.Add("角色ID", typeof(int));
                currentPageTable.Columns.Add("角色名称", typeof(string));
                currentPageTable.Columns.Add("描述", typeof(string));
                currentPageTable.Columns.Add("状态", typeof(string));
                currentPageTable.Columns.Add("创建时间", typeof(DateTime));
                currentPageTable.Columns.Add("创建者", typeof(string));

                // 计算当前页起始和结束索引
                int startIndex = _currentPageIndex * _pageSize;
                int endIndex = Math.Min(startIndex + _pageSize, _allRoles.Count);

                // 填充当前页数据
                for (int i = startIndex; i < endIndex; i++)
                {
                    var role = _allRoles[i];
                    currentPageTable.Rows.Add(
                        role.RoleId,
                        role.RoleName,
                        role.Description,
                        role.IsActive ? "启用" : "禁用",
                        role.CreateTime,
                        role.CreateBy
                    );
                }

                // 更新数据源
                dataGridViewRoles.DataSource = currentPageTable;

                // 更新页码显示
                lblPageInfo.Text = $"第 {_currentPageIndex + 1} 页，共 {_totalPages} 页";

                // 启用/禁用分页按钮
                btnFirstPage.Enabled = _currentPageIndex > 0;
                btnPrevPage.Enabled = _currentPageIndex > 0;
                btnNextPage.Enabled = _currentPageIndex < _totalPages - 1;
                btnLastPage.Enabled = _currentPageIndex < _totalPages - 1;

                // 更新记录数量显示
                lblRecordCount.Text = $"共 {_allRoles.Count} 条记录";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新分页显示时出错");
                MessageBox.Show($"更新分页显示时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterRoles()
        {
            try
            {
                // 获取搜索条件
                string searchText = txtSearch.Text.Trim().ToLower();
                string status = cmbStatus.SelectedItem?.ToString() ?? string.Empty;

                // 清空列表并重新筛选
                List<Role> filteredRoles = new List<Role>();

                // 从数据库或缓存获取所有角色
                var allRolesSource = _dbContext.Roles.ToList();  // 实际应用中可以优化为缓存

                foreach (var role in allRolesSource)
                {
                    bool matchSearch = string.IsNullOrEmpty(searchText) ||
                                      role.RoleName.ToLower().Contains(searchText) ||
                                      (role.Description?.ToLower()?.Contains(searchText) ?? false);

                    bool matchStatus = string.IsNullOrEmpty(status) ||
                                     (status == "启用" && role.IsActive) ||
                                     (status == "禁用" && !role.IsActive);

                    if (matchSearch && matchStatus)
                    {
                        filteredRoles.Add(role);
                    }
                }

                // 更新角色列表
                _allRoles = filteredRoles;

                // 更新总页数
                _totalPages = (_allRoles.Count + _pageSize - 1) / _pageSize;
                if (_totalPages == 0) _totalPages = 1;

                // 重置为第一页
                _currentPageIndex = 0;

                // 更新显示
                UpdatePageDisplay();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "筛选角色数据时出错");
                MessageBox.Show($"筛选角色数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region 事件处理

        private void DataGridViewRoles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 忽略列头和无效行
            if (e.RowIndex < 0) return;
            
            EditSelectedRole();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FilterRoles();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // 清空搜索条件
            txtSearch.Text = string.Empty;
            cmbStatus.SelectedIndex = -1;
            
            // 重新加载所有数据
            LoadRoleData();
        }

        private void btnAddRole_Click(object sender, EventArgs e)
        {
            OpenRoleEditForm(null);
        }

        private void btnEditRole_Click(object sender, EventArgs e)
        {
            EditSelectedRole();
        }

        private void EditSelectedRole()
        {
            try
            {
                // 获取选中的行
                if (dataGridViewRoles.CurrentRow != null)
                {
                    // 获取角色ID
                    int roleId = Convert.ToInt32(dataGridViewRoles.CurrentRow.Cells["角色ID"].Value);
                    
                    // 查找对应的角色对象
                    Role selectedRole = _allRoles.FirstOrDefault(r => r.RoleId == roleId);
                    
                    if (selectedRole != null)
                    {
                        OpenRoleEditForm(selectedRole);
                    }
                }
                else
                {
                    MessageBox.Show("请先选择一个角色", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "编辑角色时出错");
                MessageBox.Show($"编辑角色时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenRoleEditForm(Role role)
        {
            try
            {
                // 创建并显示角色编辑窗体
                using (var roleEditForm = Program.ServiceProvider.GetRequiredService<RoleEditForm>())
                {
                    roleEditForm.SetRole(role);
                    if (roleEditForm.ShowDialog() == DialogResult.OK)
                    {
                        // 刷新角色列表
                        LoadRoleData();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "打开角色编辑窗体时出错");
                MessageBox.Show($"打开角色编辑窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteRole_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取选中的行
                if (dataGridViewRoles.CurrentRow != null)
                {
                    // 获取角色ID和名称
                    int roleId = Convert.ToInt32(dataGridViewRoles.CurrentRow.Cells["角色ID"].Value);
                    string roleName = dataGridViewRoles.CurrentRow.Cells["角色名称"].Value.ToString();
                    
                    // 确认删除
                    DialogResult result = MessageBox.Show($"确定要删除角色 [{roleName}] 吗？", 
                        "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    
                    if (result == DialogResult.Yes)
                    {
                        // 从数据库中删除
                        Role roleToDelete = _dbContext.Roles.Find(roleId);
                        if (roleToDelete != null)
                        {
                            // 检查是否有关联的用户
                            int userCount = _dbContext.UserRoles.Count(ur => ur.RoleId == roleId);
                            if (userCount > 0)
                            {
                                MessageBox.Show($"无法删除角色 [{roleName}]，该角色下有 {userCount} 个关联用户。", 
                                    "删除失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            // 删除关联的权限
                            var permissions = _dbContext.RolePermissions.Where(rp => rp.RoleId == roleId).ToList();
                            foreach (var permission in permissions)
                            {
                                _dbContext.RolePermissions.Remove(permission);
                            }

                            // 删除角色
                            _dbContext.Roles.Remove(roleToDelete);
                            _dbContext.SaveChanges();

                            // 重新加载数据
                            LoadRoleData();
                            
                            MessageBox.Show("角色删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("角色不存在或已被删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请先选择一个角色", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除角色时出错");
                MessageBox.Show($"删除角色时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPermission_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取选中的行
                if (dataGridViewRoles.CurrentRow != null)
                {
                    // 获取角色ID和名称
                    int roleId = Convert.ToInt32(dataGridViewRoles.CurrentRow.Cells["角色ID"].Value);
                    string roleName = dataGridViewRoles.CurrentRow.Cells["角色名称"].Value.ToString();
                    
                    // 打开权限设置窗口
                    using (var rolePermissionForm = Program.ServiceProvider.GetRequiredService<RolePermissionForm>())
                    {
                        rolePermissionForm.SetRole(roleId, roleName);
                        if (rolePermissionForm.ShowDialog() == DialogResult.OK)
                        {
                            // 刷新角色列表
                            LoadRoleData();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请先选择一个角色", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "设置角色权限时出错");
                MessageBox.Show($"设置角色权限时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                FilterRoles();
                e.SuppressKeyPress = true;
            }
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterRoles();
        }

        #endregion
    }
} 