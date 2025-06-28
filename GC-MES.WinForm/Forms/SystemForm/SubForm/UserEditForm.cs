

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
    /// <summary>
    /// 用户编辑窗体
    /// </summary>
    public partial class UserEditForm : Form
    {
        private readonly GC_MES.Model.System.Sys_User _user;
        private readonly ISqlSugarClient _dbContext;
        private readonly bool _isNewUser;

        public UserEditForm(GC_MES.Model.System.Sys_User user, ISqlSugarClient dbContext)
        {
            _dbContext = dbContext;

            // 如果传入的用户为空，则创建新用户
            if (user == null)
            {
                _user = new GC_MES.Model.System.Sys_User
                {
                    CreateDate = DateTime.Now,
                    Enable = 1,
                    IsRegregisterPhone = 0
                };
                _isNewUser = true;
            }
            else
            {
                _user = user;
                _isNewUser = false;
            }

            InitializeComponent();

            // 绑定事件
            Load += UserEditForm_Load;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        // 这里省略了InitializeComponent方法的实现，它应该在Designer.cs文件中

        private void UserEditForm_Load(object sender, EventArgs e)
        {
            try
            {
                // 设置窗体标题
                Text = _isNewUser ? "添加用户" : "编辑用户";

                // 加载下拉框数据
                LoadComboBoxData();

                // 如果是编辑模式，加载用户数据
                if (!_isNewUser)
                {
                    LoadUserData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化窗体失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadComboBoxData()
        {
            // 加载角色数据
            var roles = _dbContext.Queryable<GC_MES.Model.System.Sys_Role>().Select(r => new { r.Role_Id, r.RoleName }).ToList();
            cmbRole.DataSource = roles;
            cmbRole.DisplayMember = "RoleName";
            cmbRole.ValueMember = "Role_Id";

            // 加载部门数据
            var depts = _dbContext.Queryable<GC_MES.Model.System.Sys_Dept>().Select(d => new { d.Dept_Id, d.DeptName }).ToList();
            cmbDept.DataSource = depts;
            cmbDept.DisplayMember = "DeptName";
            cmbDept.ValueMember = "Dept_Id";
        }

        private void LoadUserData()
        {
            // 填充表单数据
            txtUserName.Text = _user.UserName;
            txtUserName.ReadOnly = true;  // 编辑模式下用户名不可修改
            txtTrueName.Text = _user.UserTrueName;
            txtMobile.Text = _user.Mobile;
            txtEmail.Text = _user.Email;
            txtTel.Text = _user.Tel;
            txtAddress.Text = _user.Address;
            txtRemark.Text = _user.Remark;
            chkEnable.Checked = _user.Enable == 1;

            // 选择角色和部门
            if (_user.Role_Id > 0)
            {
                cmbRole.SelectedValue = _user.Role_Id;
            }

            if (_user.Dept_Id.HasValue && _user.Dept_Id.Value > 0)
            {
                cmbDept.SelectedValue = _user.Dept_Id.Value;
            }

            // 性别选择
            if (_user.Gender.HasValue)
            {
                if (_user.Gender == 1)
                {
                    rbMale.Checked = true;
                }
                else if (_user.Gender == 0)
                {
                    rbFemale.Checked = true;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证表单
                if (!ValidateForm())
                {
                    return;
                }

                // 收集表单数据
                _user.UserName = txtUserName.Text.Trim();
                _user.UserTrueName = txtTrueName.Text.Trim();
                _user.Mobile = txtMobile.Text.Trim();
                _user.Email = txtEmail.Text.Trim();
                _user.Tel = txtTel.Text.Trim();
                _user.Address = txtAddress.Text.Trim();
                _user.Remark = txtRemark.Text.Trim();
                _user.Enable = chkEnable.Checked ? (byte)1 : (byte)0;

                // 角色和部门
                _user.Role_Id = Convert.ToInt32(cmbRole.SelectedValue);
                _user.RoleName = cmbRole.Text;

                if (cmbDept.SelectedValue != null)
                {
                    _user.Dept_Id = Convert.ToInt32(cmbDept.SelectedValue);
                    _user.DeptName = cmbDept.Text;
                }

                // 性别
                if (rbMale.Checked)
                {
                    _user.Gender = 1;
                }
                else if (rbFemale.Checked)
                {
                    _user.Gender = 0;
                }

                // 新增或更新用户
                if (_isNewUser)
                {
                    // 设置默认密码
                    _user.UserPwd = "123456"; // 实际应用中应该使用加密方法

                    // 设置创建信息
                    _user.CreateDate = DateTime.Now;
                    _user.Creator = "Admin"; // 应使用当前登录用户

                    // 插入数据库
                    var result = _dbContext.Insertable(_user).ExecuteReturnIdentity();

                    if (result > 0)
                    {
                        MessageBox.Show("添加用户成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("添加用户失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // 设置修改信息
                    _user.ModifyDate = DateTime.Now;
                    _user.Modifier = "Admin"; // 应使用当前登录用户

                    // 更新数据库
                    var result = _dbContext.Updateable(_user).ExecuteCommand();

                    if (result > 0)
                    {
                        MessageBox.Show("更新用户成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("更新用户失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存用户失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateForm()
        {
            // 用户名验证
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                MessageBox.Show("用户名不能为空", "验证", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Focus();
                return false;
            }

            // 真实姓名验证
            if (string.IsNullOrEmpty(txtTrueName.Text.Trim()))
            {
                MessageBox.Show("真实姓名不能为空", "验证", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTrueName.Focus();
                return false;
            }

            // 角色验证
            if (cmbRole.SelectedIndex == -1)
            {
                MessageBox.Show("请选择角色", "验证", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbRole.Focus();
                return false;
            }

            return true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }

}
