using GC_MES.BLL.System.IService;
using GC_MES.Model.System;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GC_MES.WinForm.Forms.SystemForm.SubForm
{
    public partial class RoleEditForm : Form
    {
        private readonly ISys_RoleService _roleService;
        private readonly ISys_DeptService _deptService;
        private readonly ISys_MenuService _menuService;

        private Sys_Role _role;
        private bool _isNew;
        private int _roleId;

        // 用于新增角色的构造函数
        public RoleEditForm(ISys_RoleService roleService, ISys_DeptService deptService, ISys_MenuService menuService)
        {
            _roleService = roleService;
            _deptService = deptService;
            _menuService = menuService;

            _isNew = true;
            _role = new Sys_Role
            {
                Id = 0,
                CreateTime = DateTime.Now,
                Enabled = true,
                Order_ID = 0
            };

            InitializeComponent();

            // 设置窗体标题
            lblTitle.Text = "添加角色";

            // 绑定事件
            Load += RoleEditForm_Load;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        // 用于编辑角色的构造函数
        public RoleEditForm(int roleId, ISys_RoleService roleService, ISys_DeptService deptService, ISys_MenuService menuService)
        {
            _roleService = roleService;
            _deptService = deptService;
            _menuService = menuService;

            _isNew = false;
            _roleId = roleId;

            InitializeComponent();

            // 设置窗体标题
            lblTitle.Text = "编辑角色";

            // 绑定事件
            Load += RoleEditForm_Load;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        private void RoleEditForm_Load(object sender, EventArgs e)
        {
            // 加载部门和父级角色下拉框数据
            LoadComboBoxData();

            // 如果是编辑模式，加载角色数据
            if (!_isNew)
            {
                LoadRoleData();
            }
            else
            {
                // 新增角色时的默认值
                chkEnabled.Checked = true;
                numOrder.Value = 0;
            }

            // 加载权限树
            LoadPermissionTree();
        }

        // 加载下拉框数据
        private void LoadComboBoxData()
        {
            try
            {
                // 加载部门数据
                var departments = _deptService.Query();

                cmbDepartment.DataSource = departments;
                cmbDepartment.DisplayMember = "DeptName";
                cmbDepartment.ValueMember = "Id";

                if (departments.Count > 0)
                {
                    cmbDepartment.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载部门数据失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 加载角色数据
        private void LoadRoleData()
        {
            try
            {
                // 根据ID查询角色信息
                _role = _roleService.QueryById(_roleId);

                if (_role != null)
                {
                    // 填充表单控件
                    txtRoleName.Text = _role.RoleName;
                    txtRoleDesc.Text = _role.RoleDesc;
                    numOrder.Value = _role.Order_ID ?? 0;
                    chkEnabled.Checked = _role.Enabled ?? true;

                    // 设置部门下拉框
                    if (_role.Dept_Id.HasValue && _role.Dept_Id.Value > 0)
                    {
                        cmbDepartment.SelectedValue = _role.Dept_Id;
                    }

                    // 加载角色权限
                    LoadRolePermissions(_role.Id);
                }
                else
                {
                    MessageBox.Show("未找到指定角色数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载角色数据失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 加载权限树
        private void LoadPermissionTree()
        {
            try
            {
                // 获取所有菜单
                var menus = _menuService.Query();

                // 清空现有节点
                trvPermissions.Nodes.Clear();

                // 构建菜单树
                var rootMenus = menus.Where(m => m.ParentId == 0 || m.ParentId == null).OrderBy(m => m.Order_ID).ToList();

                foreach (var rootMenu in rootMenus)
                {
                    TreeNode rootNode = new TreeNode(rootMenu.MenuName)
                    {
                        Tag = rootMenu.Id,
                        Checked = false
                    };

                    // 递归添加子菜单
                    AddChildNodes(rootNode, menus, rootMenu.Id);

                    trvPermissions.Nodes.Add(rootNode);
                }

                // 如果是编辑模式，标记已有的权限
                if (!_isNew && _role != null)
                {
                    MarkRolePermissions(_role.Id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载权限树失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 递归添加子节点
        private void AddChildNodes(TreeNode parentNode, List<Sys_Menu> menus, int parentMenuId)
        {
            var childMenus = menus.Where(m => m.ParentId == parentMenuId).OrderBy(m => m.Order_ID).ToList();

            foreach (var childMenu in childMenus)
            {
                TreeNode childNode = new TreeNode(childMenu.MenuName)
                {
                    Tag = childMenu.Id,
                    Checked = false
                };

                AddChildNodes(childNode, menus, childMenu.Id);
                parentNode.Nodes.Add(childNode);
            }
        }

        // 标记角色已有的权限
        private void MarkRolePermissions(int roleId)
        {
            try
            {
                // 获取角色权限
                var permissions = _roleService.GetRolePermissions(roleId);

                if (permissions != null && permissions.Count > 0)
                {
                    // 遍历权限树，标记已有的权限
                    foreach (TreeNode node in trvPermissions.Nodes)
                    {
                        MarkNodePermissions(node, permissions);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载角色权限失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 递归标记节点权限
        private void MarkNodePermissions(TreeNode node, List<int> permissions)
        {
            if (node.Tag != null && permissions.Contains((int)node.Tag))
            {
                node.Checked = true;
            }

            foreach (TreeNode childNode in node.Nodes)
            {
                MarkNodePermissions(childNode, permissions);
            }
        }

        // 获取选中的权限ID列表
        private List<int> GetSelectedPermissions()
        {
            List<int> permissions = new List<int>();

            foreach (TreeNode node in trvPermissions.Nodes)
            {
                GetNodePermissions(node, permissions);
            }

            return permissions;
        }

        // 递归获取节点权限
        private void GetNodePermissions(TreeNode node, List<int> permissions)
        {
            if (node.Checked && node.Tag != null)
            {
                permissions.Add((int)node.Tag);
            }

            foreach (TreeNode childNode in node.Nodes)
            {
                GetNodePermissions(childNode, permissions);
            }
        }

        // 加载角色权限
        private void LoadRolePermissions(int roleId)
        {
            // 在LoadPermissionTree中已经实现了标记角色权限
        }

        // 保存按钮事件
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // 表单验证
            if (string.IsNullOrWhiteSpace(txtRoleName.Text))
            {
                MessageBox.Show("角色名称不能为空", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRoleName.Focus();
                return;
            }

            try
            {
                // 收集表单数据
                _role.RoleName = txtRoleName.Text.Trim();
                _role.RoleDesc = txtRoleDesc.Text.Trim();
                _role.Order_ID = (int)numOrder.Value;
                _role.Enabled = chkEnabled.Checked;
                _role.Dept_Id = (int)cmbDepartment.SelectedValue;

                if (_isNew)
                {
                    _role.CreateTime = DateTime.Now;
                    _role.CreateBy = Program.CurrentUser?.UserName;
                }
                else
                {
                    _role.UpdateTime = DateTime.Now;
                    _role.UpdateBy = Program.CurrentUser?.UserName;
                }

                // 保存角色信息
                bool saveResult;
                if (_isNew)
                {
                    saveResult = _roleService.Add(_role);
                }
                else
                {
                    saveResult = _roleService.Update(_role);
                }

                if (saveResult)
                {
                    // 获取选中的权限
                    var selectedPermissions = GetSelectedPermissions();

                    // 保存角色权限
                    bool permissionResult = _roleService.SaveRolePermissions(_role.Id, selectedPermissions);

                    if (permissionResult)
                    {
                        MessageBox.Show("保存成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("角色权限保存失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("角色信息保存失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 取消按钮事件
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}