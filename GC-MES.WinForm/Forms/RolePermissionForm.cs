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
using GC_MES.DAL.DbContexts;
using GC_MES.Model;
using System.Data.Entity;

namespace GC_MES.WinForm.Forms
{
    public partial class RolePermissionForm : Form
    {
        private readonly ILogger<RolePermissionForm> _logger;
        private readonly IConfiguration _configuration;
        private readonly MesDbContext _dbContext;

        // 当前角色
        private int _roleId;
        private string _roleName;
        
        // 权限数据
        private List<Permission> _allPermissions = new List<Permission>();
        private List<int> _rolePermissionIds = new List<int>();

        public RolePermissionForm(ILogger<RolePermissionForm> logger, IConfiguration configuration, MesDbContext dbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
            
            InitializeComponent();
            
            // 应用当前主题
            ThemeManager.ApplyTheme(this);
        }

        public void SetRole(int roleId, string roleName)
        {
            _roleId = roleId;
            _roleName = roleName;
            
            // 更新窗体标题
            this.Text = $"角色权限设置 - {roleName}";
            lblRoleName.Text = roleName;
        }

        private void RolePermissionForm_Load(object sender, EventArgs e)
        {
            // 初始化模块下拉框
            InitializeModuleComboBox();
            
            // 加载权限数据
            LoadPermissionData();
        }

        private void InitializeModuleComboBox()
        {
            try
            {
                // 在实际应用中，这里应该从数据库获取模块列表
                var modules = new List<string>
                {
                    "系统管理",
                    "生产管理",
                    "质量管理",
                    "仓库管理",
                    "设备管理",
                    "报表分析"
                };
                
                cmbModule.Items.Clear();
                cmbModule.Items.Add("所有模块");
                cmbModule.Items.AddRange(modules.ToArray());
                cmbModule.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化模块下拉框时出错");
            }
        }

        private void LoadPermissionData()
        {
            try
            {
                // 清空当前树形结构
                treeViewPermissions.Nodes.Clear();

                // 加载所有权限
                try
                {
                    // 从数据库加载权限数据
                    _allPermissions = _dbContext.Permissions
                        .Include(p => p.Parent)
                        .OrderBy(p => p.ModuleName)
                        .ThenBy(p => p.DisplayOrder)
                        .ToList();
                    
                    // 加载当前角色的权限ID列表
                    _rolePermissionIds = _dbContext.RolePermissions
                        .Where(rp => rp.RoleId == _roleId)
                        .Select(rp => rp.PermissionId)
                        .ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "从数据库加载权限数据时出错");
                    
                    // 如果数据库加载失败，则使用模拟数据
                    _allPermissions = new List<Permission>
                    {
                        // 系统管理模块
                        new Permission { PermissionId = 1, PermissionName = "系统管理", PermissionCode = "system", ModuleName = "系统管理", IsMenu = true, ParentId = null, DisplayOrder = 1, IsActive = true },
                        new Permission { PermissionId = 2, PermissionName = "用户管理", PermissionCode = "system.user", ModuleName = "系统管理", IsMenu = true, ParentId = 1, DisplayOrder = 1, IsActive = true },
                        new Permission { PermissionId = 3, PermissionName = "查看用户", PermissionCode = "system.user.view", ModuleName = "系统管理", IsMenu = false, ParentId = 2, DisplayOrder = 1, IsActive = true },
                        new Permission { PermissionId = 4, PermissionName = "添加用户", PermissionCode = "system.user.add", ModuleName = "系统管理", IsMenu = false, ParentId = 2, DisplayOrder = 2, IsActive = true },
                        new Permission { PermissionId = 5, PermissionName = "编辑用户", PermissionCode = "system.user.edit", ModuleName = "系统管理", IsMenu = false, ParentId = 2, DisplayOrder = 3, IsActive = true },
                        new Permission { PermissionId = 6, PermissionName = "删除用户", PermissionCode = "system.user.delete", ModuleName = "系统管理", IsMenu = false, ParentId = 2, DisplayOrder = 4, IsActive = true },
                        
                        new Permission { PermissionId = 7, PermissionName = "角色管理", PermissionCode = "system.role", ModuleName = "系统管理", IsMenu = true, ParentId = 1, DisplayOrder = 2, IsActive = true },
                        new Permission { PermissionId = 8, PermissionName = "查看角色", PermissionCode = "system.role.view", ModuleName = "系统管理", IsMenu = false, ParentId = 7, DisplayOrder = 1, IsActive = true },
                        new Permission { PermissionId = 9, PermissionName = "添加角色", PermissionCode = "system.role.add", ModuleName = "系统管理", IsMenu = false, ParentId = 7, DisplayOrder = 2, IsActive = true },
                        new Permission { PermissionId = 10, PermissionName = "编辑角色", PermissionCode = "system.role.edit", ModuleName = "系统管理", IsMenu = false, ParentId = 7, DisplayOrder = 3, IsActive = true },
                        new Permission { PermissionId = 11, PermissionName = "删除角色", PermissionCode = "system.role.delete", ModuleName = "系统管理", IsMenu = false, ParentId = 7, DisplayOrder = 4, IsActive = true },
                        new Permission { PermissionId = 12, PermissionName = "分配权限", PermissionCode = "system.role.permission", ModuleName = "系统管理", IsMenu = false, ParentId = 7, DisplayOrder = 5, IsActive = true },
                        
                        // 生产管理模块
                        new Permission { PermissionId = 20, PermissionName = "生产管理", PermissionCode = "production", ModuleName = "生产管理", IsMenu = true, ParentId = null, DisplayOrder = 2, IsActive = true },
                        new Permission { PermissionId = 21, PermissionName = "工单管理", PermissionCode = "production.workorder", ModuleName = "生产管理", IsMenu = true, ParentId = 20, DisplayOrder = 1, IsActive = true },
                        new Permission { PermissionId = 22, PermissionName = "查看工单", PermissionCode = "production.workorder.view", ModuleName = "生产管理", IsMenu = false, ParentId = 21, DisplayOrder = 1, IsActive = true }
                    };
                    
                    // 为了模拟数据能正常显示，需要构建父子关系
                    foreach (var permission in _allPermissions)
                    {
                        if (permission.ParentId.HasValue)
                        {
                            var parent = _allPermissions.FirstOrDefault(p => p.PermissionId == permission.ParentId.Value);
                            permission.Parent = parent;
                        }
                    }
                    
                    // 模拟当前角色的权限
                    _rolePermissionIds = new List<int> { 1, 2, 3, 20, 21 };
                }

                // 构建树形结构
                BuildPermissionTree();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载权限数据时出错");
                MessageBox.Show($"加载权限数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BuildPermissionTree()
        {
            try
            {
                // 清除现有树
                treeViewPermissions.Nodes.Clear();

                // 应用模块过滤
                string selectedModule = cmbModule.SelectedItem?.ToString();
                var filteredPermissions = _allPermissions;
                
                if (!string.IsNullOrEmpty(selectedModule) && selectedModule != "所有模块")
                {
                    filteredPermissions = _allPermissions.Where(p => p.ModuleName == selectedModule).ToList();
                }

                // 获取根权限
                var rootPermissions = filteredPermissions.Where(p => p.ParentId == null).OrderBy(p => p.DisplayOrder).ToList();

                // 为每个根权限创建节点
                foreach (var rootPermission in rootPermissions)
                {
                    // 创建根节点
                    TreeNode rootNode = new TreeNode(rootPermission.PermissionName)
                    {
                        Tag = rootPermission
                    };
                    
                    // 设置复选框状态
                    rootNode.Checked = _rolePermissionIds.Contains(rootPermission.PermissionId);
                    
                    // 添加子节点
                    AddChildPermissionNodes(rootNode, rootPermission, filteredPermissions);
                    
                    // 将根节点添加到树中
                    treeViewPermissions.Nodes.Add(rootNode);
                }

                // 展开所有节点
                treeViewPermissions.ExpandAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "构建权限树时出错");
                throw;
            }
        }

        private void AddChildPermissionNodes(TreeNode parentNode, Permission parentPermission, List<Permission> allPermissions)
        {
            // 获取当前父权限的所有子权限
            var childPermissions = allPermissions.Where(p => p.ParentId == parentPermission.PermissionId)
                                                .OrderBy(p => p.DisplayOrder)
                                                .ToList();
            
            foreach (var childPermission in childPermissions)
            {
                TreeNode childNode = new TreeNode(childPermission.PermissionName)
                {
                    Tag = childPermission
                };
                
                // 设置复选框状态
                childNode.Checked = _rolePermissionIds.Contains(childPermission.PermissionId);
                
                // 递归添加子节点
                AddChildPermissionNodes(childNode, childPermission, allPermissions);
                
                // 将子节点添加到父节点
                parentNode.Nodes.Add(childNode);
            }
        }

        private void UpdatePermissionCount()
        {
            int totalCount = 0;
            CountCheckedNodes(treeViewPermissions.Nodes, ref totalCount);
            lblSelectedCount.Text = $"已选择 {totalCount} 项权限";
        }

        private void CountCheckedNodes(TreeNodeCollection nodes, ref int count)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Checked)
                {
                    count++;
                }
                CountCheckedNodes(node.Nodes, ref count);
            }
        }

        private void treeViewPermissions_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // 阻止事件递归
            treeViewPermissions.AfterCheck -= treeViewPermissions_AfterCheck;
            
            try
            {
                // 更新子节点状态
                UpdateChildNodes(e.Node, e.Node.Checked);
                
                // 更新父节点状态
                UpdateParentNodes(e.Node);
                
                // 更新选中数量
                UpdatePermissionCount();
            }
            finally
            {
                // 恢复事件
                treeViewPermissions.AfterCheck += treeViewPermissions_AfterCheck;
            }
        }

        private void UpdateChildNodes(TreeNode node, bool isChecked)
        {
            foreach (TreeNode childNode in node.Nodes)
            {
                childNode.Checked = isChecked;
                UpdateChildNodes(childNode, isChecked);
            }
        }

        private void UpdateParentNodes(TreeNode node)
        {
            TreeNode parentNode = node.Parent;
            if (parentNode != null)
            {
                bool allChecked = true;
                bool anyChecked = false;
                
                foreach (TreeNode childNode in parentNode.Nodes)
                {
                    if (childNode.Checked)
                    {
                        anyChecked = true;
                    }
                    else
                    {
                        allChecked = false;
                    }
                }
                
                // 如果所有子节点都选中，则父节点选中
                // 如果任一子节点选中，则父节点选中（可根据需求调整）
                parentNode.Checked = anyChecked;
                
                // 递归更新上层节点
                UpdateParentNodes(parentNode);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FilterPermissions();
        }

        private void FilterPermissions()
        {
            try
            {
                // 重新构建树
                BuildPermissionTree();
                
                // 获取搜索条件
                string searchText = txtSearch.Text.Trim().ToLower();
                
                if (!string.IsNullOrEmpty(searchText))
                {
                    // 迭代树中的每个节点，查找匹配项
                    FilterTreeNodes(treeViewPermissions.Nodes, searchText);
                }
                
                // 更新权限数量
                UpdatePermissionCount();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "筛选权限时出错");
                MessageBox.Show($"筛选权限时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool FilterTreeNodes(TreeNodeCollection nodes, string searchText)
        {
            bool anyVisible = false;
            
            foreach (TreeNode node in nodes)
            {
                // 检查当前节点是否匹配搜索条件
                Permission permission = node.Tag as Permission;
                bool isMatch = permission != null && 
                              (permission.PermissionName.ToLower().Contains(searchText) ||
                               permission.PermissionCode.ToLower().Contains(searchText) ||
                               (permission.Description?.ToLower()?.Contains(searchText) ?? false));
                
                // 递归检查子节点
                bool childrenVisible = FilterTreeNodes(node.Nodes, searchText);
                
                // 如果当前节点或任何子节点匹配，则显示此节点
                node.Visible = isMatch || childrenVisible;
                
                // 如果当前节点可见，则其父节点也应可见
                anyVisible = anyVisible || node.Visible;
                
                if (node.Visible)
                {
                    // 展开匹配的节点
                    node.Expand();
                }
            }
            
            return anyVisible;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // 清空搜索条件
            txtSearch.Text = string.Empty;
            cmbModule.SelectedIndex = 0;
            
            // 重新加载所有数据
            LoadPermissionData();
        }

        private void cmbModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 当选择新的模块时，重新加载权限树
            BuildPermissionTree();
            UpdatePermissionCount();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            // 选中所有可见节点
            foreach (TreeNode node in treeViewPermissions.Nodes)
            {
                if (node.Visible)
                {
                    node.Checked = true;
                    SelectAllChildNodes(node, true);
                }
            }
            
            // 更新选中数量
            UpdatePermissionCount();
        }

        private void SelectAllChildNodes(TreeNode node, bool isChecked)
        {
            foreach (TreeNode childNode in node.Nodes)
            {
                if (childNode.Visible)
                {
                    childNode.Checked = isChecked;
                    SelectAllChildNodes(childNode, isChecked);
                }
            }
        }

        private void btnUnselectAll_Click(object sender, EventArgs e)
        {
            // 取消选中所有可见节点
            foreach (TreeNode node in treeViewPermissions.Nodes)
            {
                if (node.Visible)
                {
                    node.Checked = false;
                    SelectAllChildNodes(node, false);
                }
            }
            
            // 更新选中数量
            UpdatePermissionCount();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 收集所有选中的权限ID
                List<int> selectedPermissionIds = new List<int>();
                CollectCheckedPermissionIds(treeViewPermissions.Nodes, selectedPermissionIds);
                
                // 保存到数据库
                SaveRolePermissions(selectedPermissionIds);
                
                // 关闭窗体
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存角色权限时出错");
                MessageBox.Show($"保存角色权限时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CollectCheckedPermissionIds(TreeNodeCollection nodes, List<int> permissionIds)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Checked && node.Tag is Permission permission)
                {
                    permissionIds.Add(permission.PermissionId);
                }
                
                // 递归处理子节点
                CollectCheckedPermissionIds(node.Nodes, permissionIds);
            }
        }

        private void SaveRolePermissions(List<int> selectedPermissionIds)
        {
            // 获取当前角色的所有权限
            var existingPermissions = _dbContext.RolePermissions
                .Where(rp => rp.RoleId == _roleId)
                .ToList();
            
            // 需要删除的权限
            var permissionsToRemove = existingPermissions
                .Where(rp => !selectedPermissionIds.Contains(rp.PermissionId))
                .ToList();
            
            // 需要添加的权限
            var existingPermissionIds = existingPermissions.Select(rp => rp.PermissionId).ToList();
            var permissionsToAdd = selectedPermissionIds
                .Where(id => !existingPermissionIds.Contains(id))
                .Select(id => new RolePermission
                {
                    RoleId = _roleId,
                    PermissionId = id,
                    CreateTime = DateTime.Now,
                    CreateBy = "admin" // 实际应用中应使用当前登录用户
                })
                .ToList();
            
            // 执行数据库操作
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    // 删除不再需要的权限
                    foreach (var permission in permissionsToRemove)
                    {
                        _dbContext.RolePermissions.Remove(permission);
                    }
                    
                    // 添加新的权限
                    foreach (var permission in permissionsToAdd)
                    {
                        _dbContext.RolePermissions.Add(permission);
                    }
                    
                    // 保存更改
                    _dbContext.SaveChanges();
                    
                    // 提交事务
                    transaction.Commit();
                    
                    MessageBox.Show($"角色 [{_roleName}] 权限保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    // 回滚事务
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 取消操作
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FilterPermissions();
                e.SuppressKeyPress = true;
            }
        }
    }
} 