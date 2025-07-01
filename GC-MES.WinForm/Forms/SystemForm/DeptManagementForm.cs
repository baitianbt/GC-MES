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
    public partial class DeptManagementForm : Form
    {
        private readonly ISys_DeptService _deptService;
        private int _pageIndex = 1;
        private int _pageSize = 20;
        private int _totalCount = 0;

        public DeptManagementForm()
        {
            InitializeComponent();
            _deptService = Program.ServiceProvider.GetService(typeof(ISys_DeptService)) as ISys_DeptService;
        }

        private void DeptManagementForm_Load(object sender, EventArgs e)
        {
            // 应用主题
            Common.ThemeManager.Instance.ApplyTheme(this);

            // 加载数据
            LoadData();

            // 绑定事件
            BindEvents();
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
                string deptName = txtSearchName.Text.Trim();
                string deptCode = txtSearchCode.Text.Trim();

                // 构建查询条件
                List<Sys_Dept> result;
                if (string.IsNullOrEmpty(deptName) && string.IsNullOrEmpty(deptCode))
                {
                    // 无条件查询
                    result = _deptService.QueryPage(_pageIndex, _pageSize, out _totalCount, null, d => d.Dept_Id, true);
                }
                else
                {
                    // 条件查询
                    result = _deptService.QueryPage(
                        _pageIndex,
                        _pageSize,
                        out _totalCount,
                        d => (string.IsNullOrEmpty(deptName) || d.DeptName.Contains(deptName)) &&
                             (string.IsNullOrEmpty(deptCode) || d.DeptCode.Contains(deptCode)),
                        d => d.Dept_Id,
                        true);
                }

                // 绑定数据到DataGridView
                dgvDepts.DataSource = result;

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
            txtSearchCode.Text = string.Empty;
            _pageIndex = 1;
            LoadData();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var editForm = new DeptEditForm();
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDepts.CurrentRow == null)
            {
                MessageBox.Show("请先选择一条记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedDept = dgvDepts.CurrentRow.DataBoundItem as Sys_Dept;
            if (selectedDept == null) return;

            var editForm = new DeptEditForm(selectedDept);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDepts.CurrentRow == null)
            {
                MessageBox.Show("请先选择一条记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedDept = dgvDepts.CurrentRow.DataBoundItem as Sys_Dept;
            if (selectedDept == null) return;

            if (MessageBox.Show($"确定要删除部门 [{selectedDept.DeptName}] 吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    bool result = _deptService.Delete(selectedDept);
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
