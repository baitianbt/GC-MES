using GC_MES.BLL.System.IService;
using GC_MES.Model.System;
using GC_MES.WinForm.Common;
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
    public partial class MenuEditForm : Form
    {
        private readonly ISys_MenuService _menuService;
        private Sys_Menu _menu;
        private bool _isAdd = true;

        public MenuEditForm()
        {
            InitializeComponent();
            _menuService = Program.Services.GetService(typeof(ISys_MenuService)) as ISys_MenuService;
            _menu = new Sys_Menu();
            _isAdd = true;
            Text = "新增菜单";
            lblTitle.Text = "新增菜单";
        }

        public MenuEditForm(Sys_Menu menu)
        {
            InitializeComponent();
            _menuService = Program.Services.GetService(typeof(ISys_MenuService)) as ISys_MenuService;
            _menu = menu;
            _isAdd = false;
            Text = "编辑菜单";
            lblTitle.Text = "编辑菜单";
        }

        private void MenuEditForm_Load(object sender, EventArgs e)
        {
            // 应用主题
            Common.ThemeManager.Instance.ApplyTheme(this);

            // 加载父级菜单
            LoadParentMenus();

            // 初始化菜单类型
            InitMenuTypes();

            // 如果是编辑模式，加载菜单数据
            if (!_isAdd)
            {
                LoadMenuData();
            }
            else
            {
                // 默认值
                nudOrderNo.Value = 100;
                chkEnable.Checked = true;
                cmbMenuType.SelectedIndex = 0;
            }
        }

        private void LoadParentMenus()
        {
            try
            {
                // 获取所有菜单
                var allMenus = _menuService.Query();

                // 添加一个"顶级菜单"选项
                var menuList = new List<Sys_Menu>
                {
                    new Sys_Menu { Menu_Id = 0, MenuName = "顶级菜单" }
                };

                // 添加其他菜单作为可选父级
                if (!_isAdd)
                {
                    // 编辑模式下，排除自身及其子菜单
                    menuList.AddRange(allMenus.Where(m => m.Menu_Id != _menu.Menu_Id && !IsChildMenu(m.Menu_Id, _menu.Menu_Id)));
                }
                else
                {
                    // 新增模式下，添加所有菜单
                    menuList.AddRange(allMenus);
                }

                cmbParent.DataSource = menuList;
                cmbParent.DisplayMember = "MenuName";
                cmbParent.ValueMember = "Menu_Id";

                // 默认选择顶级菜单
                cmbParent.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载父级菜单失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitMenuTypes()
        {
            cmbMenuType.Items.Clear();
            cmbMenuType.Items.Add("PC端");
            cmbMenuType.Items.Add("移动端");
        }

        private void LoadMenuData()
        {
            txtMenuName.Text = _menu.MenuName;
            txtTableName.Text = _menu.TableName;
            txtUrl.Text = _menu.Url;
            txtIcon.Text = _menu.Icon;
            txtDescription.Text = _menu.Description;
            txtAuth.Text = _menu.Auth;
            nudOrderNo.Value = _menu.OrderNo ?? 100;
            chkEnable.Checked = _menu.Enable == 1;
            cmbMenuType.SelectedIndex = _menu.MenuType ?? 0;

            // 设置父级菜单
            foreach (Sys_Menu item in cmbParent.Items)
            {
                if (item.Menu_Id == _menu.ParentId)
                {
                    cmbParent.SelectedItem = item;
                    break;
                }
            }
        }

        private bool IsChildMenu(int menuId, int parentId)
        {
            // 检查menuId是否是parentId的子菜单
            var menu = _menuService.QueryById(menuId);
            if (menu == null) return false;

            if (menu.ParentId == parentId) return true;

            return IsChildMenu(menu.ParentId, parentId);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 验证输入
            if (string.IsNullOrWhiteSpace(txtMenuName.Text))
            {
                MessageBox.Show("菜单名称不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMenuName.Focus();
                return;
            }

            try
            {
                // 设置菜单属性
                _menu.MenuName = txtMenuName.Text.Trim();
                _menu.TableName = txtTableName.Text.Trim();
                _menu.Url = txtUrl.Text.Trim();
                _menu.Icon = txtIcon.Text.Trim();
                _menu.Description = txtDescription.Text.Trim();
                _menu.Auth = txtAuth.Text.Trim();
                _menu.OrderNo = (int)nudOrderNo.Value;
                _menu.Enable = chkEnable.Checked ? (byte)1 : (byte)0;
                _menu.MenuType = cmbMenuType.SelectedIndex;
                _menu.ParentId = (int)cmbParent.SelectedValue;

                bool result;
                if (_isAdd)
                {
                    // 添加新菜单
                    _menu.CreateDate = DateTime.Now;
                    _menu.Creator = AppInfo.CurrentUser?.UserName ?? "admin";
                    result = _menuService.Insert(_menu) == 1;
                }
                else
                {
                    // 更新菜单
                    _menu.ModifyDate = DateTime.Now;
                    _menu.Modifier = AppInfo.CurrentUser?.UserName ?? "admin";
                    result = _menuService.Update(_menu);
                }

                if (result)
                {
                    MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
