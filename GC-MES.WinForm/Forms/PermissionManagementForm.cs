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
    public partial class PermissionManagementForm : Form
    {
        private readonly ILogger<PermissionManagementForm> _logger;
        private readonly IConfiguration _configuration;
        private readonly MesDbContext _dbContext;

        // 树形权限列表
        private List<Permission> _allPermissions = new List<Permission>();
        private Dictionary<int, TreeNode> _permissionNodes = new Dictionary<int, TreeNode>();

        public PermissionManagementForm(ILogger<PermissionManagementForm> logger, IConfiguration configuration, MesDbContext dbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
            
            InitializeComponent();
            
            // 应用当前主题
            ThemeManager.ApplyTheme(this);
        }

        private void PermissionManagementForm_Load(object sender, EventArgs e)
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
                _permissionNodes.Clear();

                try
                {
                    // 从数据库加载权限数据
                    _allPermissions = _dbContext.Permissions
                        .Include(p => p.Parent)
                        .OrderBy(p => p.ModuleName)
                        .ThenBy(p => p.DisplayOrder)
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
                        
                        new Permission { PermissionId = 13, PermissionName = "权限管理", PermissionCode = "system.permission", ModuleName = "系统管理", IsMenu = true, ParentId = 1, DisplayOrder = 3, IsActive = true },
                        new Permission { PermissionId = 14, PermissionName = "查看权限", PermissionCode = "system.permission.view", ModuleName = "系统管理", IsMenu = false, ParentId = 13, DisplayOrder = 1, IsActive = true },
                        new Permission { PermissionId = 15, PermissionName = "添加权限", PermissionCode = "system.permission.add", ModuleName = "系统管理", IsMenu = false, ParentId = 13, DisplayOrder = 2, IsActive = true },
                        new Permission { PermissionId = 16, PermissionName = "编辑权限", PermissionCode = "system.permission.edit", ModuleName = "系统管理", IsMenu = false, ParentId = 13, DisplayOrder = 3, IsActive = true },
                        new Permission { PermissionId = 17, PermissionName = "删除权限", PermissionCode = "system.permission.delete", ModuleName = "系统管理", IsMenu = false, ParentId = 13, DisplayOrder = 4, IsActive = true },
                        
                        // 生产管理模块
                        new Permission { PermissionId = 20, PermissionName = "生产管理", PermissionCode = "production", ModuleName = "生产管理", IsMenu = true, ParentId = null, DisplayOrder = 2, IsActive = true },
                        new Permission { PermissionId = 21, PermissionName = "工单管理", PermissionCode = "production.workorder", ModuleName = "生产管理", IsMenu = true, ParentId = 20, DisplayOrder = 1, IsActive = true },
                        new Permission { PermissionId = 22, PermissionName = "查看工单", PermissionCode = "production.workorder.view", ModuleName = "生产管理", IsMenu = false, ParentId = 21, DisplayOrder = 1, IsActive = true },
                        
                        // 质量管理模块
                        new Permission { PermissionId = 30, PermissionName = "质量管理", PermissionCode = "quality", ModuleName = "质量管理", IsMenu = true, ParentId = null, DisplayOrder = 3, IsActive = true }
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
                }

                // 构建树形结构
                BuildPermissionTree();

                // 更新当前权限数量显示
                UpdatePermissionCount();
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
                _permissionNodes.Clear();

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
                        Tag = rootPermission,
                        ImageIndex = rootPermission.IsMenu ? 0 : 1,  // 0 为目录图标，1为功能图标
                        SelectedImageIndex = rootPermission.IsMenu ? 0 : 1
                    };
                    
                    // 将节点添加到字典中
                    _permissionNodes[rootPermission.PermissionId] = rootNode;
                    
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
                    Tag = childPermission,
                    ImageIndex = childPermission.IsMenu ? 0 : 1,  // 0 为目录图标，1为功能图标
                    SelectedImageIndex = childPermission.IsMenu ? 0 : 1
                };
                
                // 将节点添加到字典中
                _permissionNodes[childPermission.PermissionId] = childNode;
                
                // 递归添加子节点
                AddChildPermissionNodes(childNode, childPermission, allPermissions);
                
                // 将子节点添加到父节点
                parentNode.Nodes.Add(childNode);
            }
        }

        private void UpdatePermissionCount()
        {
            int totalCount = 0;
            CountTreeNodes(treeViewPermissions.Nodes, ref totalCount);
            lblPermissionCount.Text = $"共 {totalCount} 条权限记录";
        }

        private void CountTreeNodes(TreeNodeCollection nodes, ref int count)
        {
            foreach (TreeNode node in nodes)
            {
                count++;
                CountTreeNodes(node.Nodes, ref count);
            }
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

        #region 事件处理

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FilterPermissions();
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

        private void btnAddPermission_Click(object sender, EventArgs e)
        {
            // 获取选中的父节点
            TreeNode selectedNode = treeViewPermissions.SelectedNode;
            Permission parentPermission = null;
            
            if (selectedNode != null)
            {
                parentPermission = selectedNode.Tag as Permission;
            }
            
            OpenPermissionEditForm(null, parentPermission);
        }

        private void btnEditPermission_Click(object sender, EventArgs e)
        {
            // 获取选中的节点
            TreeNode selectedNode = treeViewPermissions.SelectedNode;
            
            if (selectedNode != null)
            {
                Permission selectedPermission = selectedNode.Tag as Permission;
                if (selectedPermission != null)
                {
                    OpenPermissionEditForm(selectedPermission, selectedPermission.Parent);
                }
            }
            else
            {
                MessageBox.Show("请先选择一个权限", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OpenPermissionEditForm(Permission permission, Permission parentPermission)
        {
            MessageBox.Show("此功能正在开发中", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // 实际实现中，这里应该调用PermissionEditForm
            // var permissionEditForm = Program.ServiceProvider.GetRequiredService<PermissionEditForm>();
            // permissionEditForm.SetPermission(permission, parentPermission);
            // if (permissionEditForm.ShowDialog() == DialogResult.OK)
            // {
            //     LoadPermissionData();
            // }
        }

        private void btnDeletePermission_Click(object sender, EventArgs e)
        {
            // 获取选中的节点
            TreeNode selectedNode = treeViewPermissions.SelectedNode;
            
            if (selectedNode == null)
            {
                MessageBox.Show("请先选择一个权限", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            Permission selectedPermission = selectedNode.Tag as Permission;
            if (selectedPermission == null)
                return;
            
            // 检查是否有子节点
            if (selectedNode.Nodes.Count > 0)
            {
                MessageBox.Show("无法删除包含子权限的权限，请先删除所有子权限", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // 确认删除
            DialogResult result = MessageBox.Show($"确定要删除权限 [{selectedPermission.PermissionName}] 吗？\n这可能会影响已分配此权限的角色。", 
                "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                try
                {
                    // 从数据库中删除
                    var permissionToDelete = _dbContext.Permissions.Find(selectedPermission.PermissionId);
                    if (permissionToDelete != null)
                    {
                        // 删除关联的角色权限
                        var rolePermissions = _dbContext.RolePermissions
                            .Where(rp => rp.PermissionId == selectedPermission.PermissionId)
                            .ToList();
                            
                        foreach (var rolePermission in rolePermissions)
                        {
                            _dbContext.RolePermissions.Remove(rolePermission);
                        }
                        
                        // 删除权限
                        _dbContext.Permissions.Remove(permissionToDelete);
                        _dbContext.SaveChanges();
                        
                        // 从树中删除节点
                        selectedNode.Remove();
                        
                        // 更新权限数量
                        UpdatePermissionCount();
                        
                        MessageBox.Show("权限删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "删除权限时出错");
                    MessageBox.Show($"删除权限时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void treeViewPermissions_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null && e.Node.Tag is Permission selectedPermission)
            {
                // 更新权限详情显示
                ShowPermissionDetails(selectedPermission);
            }
        }

        private void ShowPermissionDetails(Permission permission)
        {
            if (permission == null)
            {
                // 清空详情
                txtPermissionName.Text = string.Empty;
                txtPermissionCode.Text = string.Empty;
                txtDescription.Text = string.Empty;
                txtModuleName.Text = string.Empty;
                chkIsMenu.Checked = false;
                chkIsActive.Checked = false;
                return;
            }
            
            // 显示权限详情
            txtPermissionName.Text = permission.PermissionName;
            txtPermissionCode.Text = permission.PermissionCode;
            txtDescription.Text = permission.Description ?? string.Empty;
            txtModuleName.Text = permission.ModuleName ?? string.Empty;
            chkIsMenu.Checked = permission.IsMenu;
            chkIsActive.Checked = permission.IsActive;
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FilterPermissions();
                e.SuppressKeyPress = true;
            }
        }

        #endregion
    }
}