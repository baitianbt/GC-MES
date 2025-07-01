using GC_MES.BLL.System.IService;
using GC_MES.Model.System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GC_MES.WinForm.Forms.SystemForm
{
    public partial class MenuManagementForm : Form
    {
        private readonly ISys_MenuService _menuService;
        private List<Sys_Menu> _menuList;
        private int _pageIndex = 1;
        private int _pageSize = 20;
        private int _totalCount = 0;

        public MenuManagementForm()
        {
            InitializeComponent();
            _menuService = Program.Services.GetService(typeof(ISys_MenuService)) as ISys_MenuService;
        }

        private void MenuManagementForm_Load(object sender, EventArgs e)
        {
            // 应用主题
            Common.ThemeManager.Instance.ApplyTheme(this);

            // 初始化父级菜单下拉框
            InitParentMenus();

            // 加载数据
            LoadData();

            // 绑定事件
            BindEvents();
        }

        private void InitParentMenus()
        {
            try
            {
                // 获取所有菜单
                var allMenus = _menuService.Query();

                // 添加一个"全部"选项
                var menuList = new List<Sys_Menu>
                {
                    new Sys_Menu { Menu_Id = -1, MenuName = "全部" }
                };

                // 添加顶级菜单（ParentId = 0）
                menuList.AddRange(allMenus.Where(m => m.ParentId == 0));

                cmbParent.DataSource = menuList;
                cmbParent.DisplayMember = "MenuName";
                cmbParent.ValueMember = "Menu_Id";
                cmbParent.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化父级菜单失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindEvents()
        {
            btnSearch.Click += BtnSearch_Click;
            btnClear.Click += BtnClear_Click;
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnFirstPage.Click += BtnFirstPage_Click;
            btnPrevPage.Click += BtnPrevPage_Click;
            btnNextPage.Click += BtnNextPage_Click;
            btnLastPage.Click += BtnLastPage_Click;
            btnExport.Click += BtnExport_Click;
            btnImport.Click += BtnImport_Click;
        }

        private void LoadData()
        {
            try
            {
                // 获取查询条件
                string menuName = txtSearchName.Text.Trim();
                int parentId = cmbParent.SelectedIndex > 0 ? (int)cmbParent.SelectedValue : -1;

                // 构建查询条件
                List<Sys_Menu> result;
                if (string.IsNullOrEmpty(menuName) && parentId == -1)
                {
                    // 无条件查询
                    result = _menuService.QueryPage(_pageIndex, _pageSize, out _totalCount, null, m => m.OrderNo, true);
                }
                else
                {
                    // 条件查询
                    result = _menuService.QueryPage(
                        _pageIndex,
                        _pageSize,
                        out _totalCount,
                        m => (string.IsNullOrEmpty(menuName) || m.MenuName.Contains(menuName)) &&
                             (parentId == -1 || m.ParentId == parentId),
                        m => m.OrderNo,
                        true);
                }

                _menuList = result;

                // 绑定数据到DataGridView
                dgvMenus.DataSource = result;

                // 更新分页信息
                int totalPages = (_totalCount + _pageSize - 1) / _pageSize;
                lblPageInfo.Text = $"第 {_pageIndex}/{totalPages} 页，共 {_totalCount} 条";

                // 更新分页按钮状态
                btnFirstPage.Enabled = btnPrevPage.Enabled = _pageIndex > 1;
                btnNextPage.Enabled = btnLastPage.Enabled = _pageIndex < totalPages;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region 事件处理
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            _pageIndex = 1;
            LoadData();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtSearchName.Text = string.Empty;
            cmbParent.SelectedIndex = 0;
            _pageIndex = 1;
            LoadData();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var editForm = new MenuEditForm();
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvMenus.CurrentRow == null)
            {
                MessageBox.Show("请先选择一条记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedMenu = dgvMenus.CurrentRow.DataBoundItem as Sys_Menu;
            if (selectedMenu == null) return;

            var editForm = new MenuEditForm(selectedMenu);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvMenus.CurrentRow == null)
            {
                MessageBox.Show("请先选择一条记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedMenu = dgvMenus.CurrentRow.DataBoundItem as Sys_Menu;
            if (selectedMenu == null) return;

            // 检查是否有子菜单
            var childMenus = _menuService.Query(m => m.ParentId == selectedMenu.Menu_Id);
            if (childMenus.Any())
            {
                MessageBox.Show("该菜单下存在子菜单，无法删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"确定要删除菜单 [{selectedMenu.MenuName}] 吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    bool result = _menuService.Delete(selectedMenu);
                    if (result)
                    {
                        MessageBox.Show("删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("删除失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"删除失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnFirstPage_Click(object sender, EventArgs e)
        {
            if (_pageIndex > 1)
            {
                _pageIndex = 1;
                LoadData();
            }
        }

        private void BtnPrevPage_Click(object sender, EventArgs e)
        {
            if (_pageIndex > 1)
            {
                _pageIndex--;
                LoadData();
            }
        }

        private void BtnNextPage_Click(object sender, EventArgs e)
        {
            int totalPages = (_totalCount + _pageSize - 1) / _pageSize;
            if (_pageIndex < totalPages)
            {
                _pageIndex++;
                LoadData();
            }
        }

        private void BtnLastPage_Click(object sender, EventArgs e)
        {
            int totalPages = (_totalCount + _pageSize - 1) / _pageSize;
            if (_pageIndex < totalPages)
            {
                _pageIndex = totalPages;
                LoadData();
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            // 导出Excel功能
            MessageBox.Show("导出Excel功能待实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            // 导入Excel功能
            MessageBox.Show("导入Excel功能待实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
    }
}
