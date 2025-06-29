using GC_MES.BLL.System.IService;
using GC_MES.Model.System;
using GC_MES.WinForm.Forms.SystemForm.SubForm;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

namespace GC_MES.WinForm.Forms.SystemForm
{
    public partial class RoleManagementForm : Form
    {
        private readonly ISys_RoleService _roleService;
        private readonly ISys_DeptService _deptService;

        // 分页参数
        private int _pageIndex = 1;
        private int _pageSize = 20;
        private int _totalCount = 0;
        private int _totalPages = 0;

        // 当前选中角色
        private Sys_Role _currentRole = null;

        // 查询参数
        private string _searchName = "";
        private int? _searchDeptId = null;

        public RoleManagementForm(ISys_RoleService roleService, ISys_DeptService deptService)
        {
            _roleService = roleService;
            _deptService = deptService;

            InitializeComponent();

            // 绑定事件处理器
            Load += RoleManagementForm_Load;
            btnSearch.Click += BtnSearch_Click;
            btnClear.Click += BtnClear_Click;
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnImport.Click += BtnImport_Click;
            btnExport.Click += BtnExport_Click;
            btnExportPDF.Click += BtnExportPDF_Click;

            btnFirstPage.Click += BtnFirstPage_Click;
            btnPrevPage.Click += BtnPrevPage_Click;
            btnNextPage.Click += BtnNextPage_Click;
            btnLastPage.Click += BtnLastPage_Click;

            dgvRoles.SelectionChanged += DgvRoles_SelectionChanged;
            dgvRoles.CellDoubleClick += DgvRoles_CellDoubleClick;

            // 自定义样式
            CustomizeDataGridView();
        }

        private void RoleManagementForm_Load(object sender, EventArgs e)
        {
            // 加载部门下拉框
            LoadDepartments();

            // 加载角色数据
            LoadRoleData();
        }

        // 加载部门下拉框
        private void LoadDepartments()
        {
            try
            {
                var departments = _deptService.Query();

                // 添加"全部"选项
                List<Sys_Dept> deptList = new List<Sys_Dept>();
                deptList.Add(new Sys_Dept { Dept_Id = 0, DeptName = "全部" });
                deptList.AddRange(departments);

                cmbDept.DataSource = deptList;
                cmbDept.DisplayMember = "DeptName";
                cmbDept.ValueMember = "Id";
                cmbDept.SelectedValue = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载部门数据失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 自定义DataGridView样式
        private void CustomizeDataGridView()
        {
            // 设置列头样式
            dgvRoles.EnableHeadersVisualStyles = false;
            dgvRoles.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgvRoles.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRoles.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular);
            dgvRoles.ColumnHeadersHeight = 40;

            // 设置行样式
            dgvRoles.RowsDefaultCellStyle.BackColor = Color.White;
            dgvRoles.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvRoles.DefaultCellStyle.SelectionBackColor = Color.FromArgb(67, 67, 70);
            dgvRoles.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvRoles.RowTemplate.Height = 36;

            // 设置边框和网格线
            dgvRoles.BorderStyle = BorderStyle.None;
            dgvRoles.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRoles.GridColor = Color.FromArgb(230, 230, 230);
        }

        // 加载角色数据
        private void LoadRoleData()
        {
            try
            {
                // 构建查询表达式
                var expression = SqlSugar.Expressionable.Create<Sys_Role>()
                    .AndIF(!string.IsNullOrEmpty(_searchName), r => r.RoleName.Contains(_searchName))
                    .AndIF(_searchDeptId.HasValue && _searchDeptId.Value > 0, r => r.Dept_Id == _searchDeptId.Value)
                    .ToExpression();

                // 执行分页查询
                var result = _roleService.QueryPage(expression,pageIndex: _pageIndex,pageSize: _pageSize);

                // 更新总记录数和总页数
                _totalCount = result.TotalCount;
                _totalPages = result.TotalPages;

                // 绑定数据到DataGridView
                dgvRoles.DataSource = result;

                // 配置列显示
                ConfigureDataGridViewColumns();

                // 更新分页信息和按钮状态
                UpdatePagination();
                UpdateButtonStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载角色数据失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 配置DataGridView列显示
        private void ConfigureDataGridViewColumns()
        {
            // 如果已有列，先清空
            if (dgvRoles.Columns.Count > 0)
                return;

            // 配置显示的列
            dgvRoles.Columns["Id"].HeaderText = "编号";
            dgvRoles.Columns["Id"].Width = 60;

            dgvRoles.Columns["RoleName"].HeaderText = "角色名称";
            dgvRoles.Columns["RoleName"].Width = 150;

            dgvRoles.Columns["RoleDesc"].HeaderText = "角色描述";
            dgvRoles.Columns["RoleDesc"].Width = 200;

            dgvRoles.Columns["Order_ID"].HeaderText = "排序";
            dgvRoles.Columns["Order_ID"].Width = 60;

            dgvRoles.Columns["Enabled"].HeaderText = "状态";
            dgvRoles.Columns["Enabled"].Width = 60;

            // 隐藏不需要显示的列
            dgvRoles.Columns["CreateBy"].Visible = false;
            dgvRoles.Columns["CreateTime"].Visible = false;
            dgvRoles.Columns["UpdateBy"].Visible = false;
            dgvRoles.Columns["UpdateTime"].Visible = false;
        }

        // 更新分页信息
        private void UpdatePagination()
        {
            lblPageInfo.Text = $"第 {_pageIndex} 页 / 共 {_totalPages} 页，总记录数：{_totalCount}";

            // 更新分页按钮状态
            btnFirstPage.Enabled = _pageIndex > 1;
            btnPrevPage.Enabled = _pageIndex > 1;
            btnNextPage.Enabled = _pageIndex < _totalPages;
            btnLastPage.Enabled = _pageIndex < _totalPages;
        }

        // 更新按钮状态
        private void UpdateButtonStatus()
        {
            bool hasSelection = dgvRoles.SelectedRows.Count > 0;
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
        }

        // 搜索按钮事件
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            _searchName = txtSearchName.Text.Trim();
            _searchDeptId = cmbDept.SelectedValue != null ? (int?)cmbDept.SelectedValue : null;

            // 重置到第一页
            _pageIndex = 1;

            // 重新加载数据
            LoadRoleData();
        }

        // 清空按钮事件
        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtSearchName.Text = "";
            cmbDept.SelectedValue = 0;

            _searchName = "";
            _searchDeptId = null;

            // 重置到第一页
            _pageIndex = 1;

            // 重新加载数据
            LoadRoleData();
        }

        // 添加按钮事件
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            //// 打开角色编辑窗体
            //var editForm = new RoleEditForm();

            //if (editForm.ShowDialog() == DialogResult.OK)
            //{
            //    // 重新加载数据
            //    LoadRoleData();
            //}
        }

        // 编辑按钮事件
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (_currentRole == null)
            {
                MessageBox.Show("请先选择要编辑的角色", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //    // 打开角色编辑窗体
            //    var editForm = new RoleEditForm(_currentRole.Id);

            //    if (editForm.ShowDialog() == DialogResult.OK)
            //    {
            //        // 重新加载数据
            //        LoadRoleData();
            //    }
        }

        // 删除按钮事件
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_currentRole == null)
            {
                MessageBox.Show("请先选择要删除的角色", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 确认删除
            if (MessageBox.Show($"确认删除角色 \"{_currentRole.RoleName}\" 吗？", "确认删除",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    bool result = _roleService.Delete(_currentRole);
                    if (result)
                    {
                        MessageBox.Show("删除成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 重新加载数据
                        LoadRoleData();
                    }
                    else
                    {
                        MessageBox.Show("删除失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"删除失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 导入按钮事件
        private void BtnImport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("此功能尚未实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 导出Excel按钮事件
        private void BtnExport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("此功能尚未实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 导出PDF按钮事件
        private void BtnExportPDF_Click(object sender, EventArgs e)
        {
            MessageBox.Show("此功能尚未实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 首页按钮事件
        private void BtnFirstPage_Click(object sender, EventArgs e)
        {
            if (_pageIndex != 1)
            {
                _pageIndex = 1;
                LoadRoleData();
            }
        }

        // 上一页按钮事件
        private void BtnPrevPage_Click(object sender, EventArgs e)
        {
            if (_pageIndex > 1)
            {
                _pageIndex--;
                LoadRoleData();
            }
        }

        // 下一页按钮事件
        private void BtnNextPage_Click(object sender, EventArgs e)
        {
            if (_pageIndex < _totalPages)
            {
                _pageIndex++;
                LoadRoleData();
            }
        }

        // 末页按钮事件
        private void BtnLastPage_Click(object sender, EventArgs e)
        {
            if (_pageIndex != _totalPages)
            {
                _pageIndex = _totalPages;
                LoadRoleData();
            }
        }

        // 表格选择变更事件
        private void DgvRoles_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRoles.SelectedRows.Count > 0)
            {
                _currentRole = dgvRoles.SelectedRows[0].DataBoundItem as Sys_Role;
                UpdateButtonStatus();
            }
            else
            {
                _currentRole = null;
                UpdateButtonStatus();
            }
        }

        // 表格双击事件
        private void DgvRoles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && _currentRole != null)
            {
                BtnEdit_Click(this, EventArgs.Empty);
            }
        }
    }
}
