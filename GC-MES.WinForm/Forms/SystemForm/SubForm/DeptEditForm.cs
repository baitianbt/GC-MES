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
    public partial class DeptEditForm : Form
    {
        private readonly ISys_DeptService _deptService;
        private Sys_Dept _dept;
        private bool _isAdd = true;

        public DeptEditForm()
        {
            InitializeComponent();
            _deptService = Program.Services.GetService(typeof(ISys_DeptService)) as ISys_DeptService;
            _dept = new Sys_Dept();
            _isAdd = true;
            Text = "新增部门";
            lblTitle.Text = "新增部门";
        }

        public DeptEditForm(Sys_Dept dept)
        {
            InitializeComponent();
            _deptService = Program.Services.GetService(typeof(ISys_DeptService)) as ISys_DeptService;
            _dept = dept;
            _isAdd = false;
            Text = "编辑部门";
            lblTitle.Text = "编辑部门";
        }

        private void DeptEditForm_Load(object sender, EventArgs e)
        {
            // 应用主题
            Common.ThemeManager.Instance.ApplyTheme(this);

            // 如果是编辑模式，加载部门数据
            if (!_isAdd)
            {
                LoadDeptData();
            }
        }

        private void LoadDeptData()
        {
            txtDeptName.Text = _dept.DeptName;
            txtDeptCode.Text = _dept.DeptCode;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 验证输入
            if (string.IsNullOrWhiteSpace(txtDeptName.Text))
            {
                MessageBox.Show("部门名称不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDeptName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDeptCode.Text))
            {
                MessageBox.Show("部门编码不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDeptCode.Focus();
                return;
            }

            try
            {
                // 检查部门编码是否已存在
                var existingDept = _deptService.QueryByClause(d => d.DeptCode == txtDeptCode.Text && d.Dept_Id != _dept.Dept_Id);
                if (existingDept != null)
                {
                    MessageBox.Show("部门编码已存在，请更换", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDeptCode.Focus();
                    return;
                }

                // 设置部门属性
                _dept.DeptName = txtDeptName.Text.Trim();
                _dept.DeptCode = txtDeptCode.Text.Trim();

                bool result;
                if (_isAdd)
                {
                    // 添加新部门
                    _dept.CreateDate = DateTime.Now;
                    _dept.CreateID = AppInfo.CurrentUser?.User_Id ?? 0;
                    _dept.Creator = AppInfo.CurrentUser?.UserName ?? "admin";
                    result = _deptService.Insert(_dept) == 0;
                }
                else
                {
                    // 更新部门
                    _dept.ModifyDate = DateTime.Now;
                    _dept.ModifyID = AppInfo.CurrentUser?.User_Id ?? 0;
                    _dept.Modifier = AppInfo.CurrentUser?.UserName ?? "admin";
                    result = _deptService.Update(_dept);
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
