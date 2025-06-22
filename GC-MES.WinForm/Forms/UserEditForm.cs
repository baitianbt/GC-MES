using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;

namespace GC_MES.WinForm.Forms
{
    public partial class UserEditForm : Form
    {
        private readonly ILogger _logger;
        private readonly List<string> _departments;
        private readonly List<string> _roles;
        
        private bool _isAddMode = true;
        private DataTable _userDataTable = new DataTable();
        private DataRow _userData;
        
        public UserEditForm(ILogger logger, List<string> departments, List<string> roles)
        {
            _logger = logger;
            _departments = departments;
            _roles = roles;
            
            InitializeComponent();
            InitializeUserDataTable();
            
            // 设置下拉框数据源
            cmbDepartment.Items.AddRange(departments.ToArray());
            cmbRole.Items.AddRange(roles.ToArray());
            cmbStatus.Items.AddRange(new string[] { "启用", "停用" });
        }
        
        /// <summary>
        /// 初始化用户数据表
        /// </summary>
        private void InitializeUserDataTable()
        {
            _userDataTable = new DataTable();
            _userDataTable.Columns.Add("用户ID", typeof(string));
            _userDataTable.Columns.Add("用户名", typeof(string));
            _userDataTable.Columns.Add("姓名", typeof(string));
            _userDataTable.Columns.Add("密码", typeof(string));
            _userDataTable.Columns.Add("部门", typeof(string));
            _userDataTable.Columns.Add("角色", typeof(string));
            _userDataTable.Columns.Add("状态", typeof(string));
            _userDataTable.Columns.Add("最后登录", typeof(DateTime));
            _userDataTable.Columns.Add("备注", typeof(string));
            
            // 添加默认行
            _userData = _userDataTable.NewRow();
            _userData["状态"] = "启用";
            _userDataTable.Rows.Add(_userData);
        }
        
        /// <summary>
        /// 设置用户数据
        /// </summary>
        public void SetUserData(DataRow userData)
        {
            if (userData != null)
            {
                _isAddMode = false;
                
                // 清空现有数据
                _userDataTable.Clear();
                
                // 创建新的用户数据行
                _userData = _userDataTable.NewRow();
                
                // 复制列的值
                foreach (DataColumn col in _userDataTable.Columns)
                {
                    if (userData.Table.Columns.Contains(col.ColumnName))
                    {
                        _userData[col.ColumnName] = userData[col.ColumnName];
                    }
                }
                
                // 添加到数据表
                _userDataTable.Rows.Add(_userData);
                
                // 密码显示为空（安全考虑）
                _userData["密码"] = string.Empty;
            }
            else
            {
                _isAddMode = true;
            }
        }
        
        /// <summary>
        /// 获取用户数据
        /// </summary>
        public DataRow GetUserData()
        {
            return _userData;
        }
        
        private void UserEditForm_Load(object sender, EventArgs e)
        {
            // 设置窗体标题
            this.Text = _isAddMode ? "添加用户" : "编辑用户";
            lblFormTitle.Text = this.Text;
            
            // 绑定数据
            if (_userData != null)
            {
                txtUserID.Text = _userData["用户ID"].ToString();
                txtUsername.Text = _userData["用户名"].ToString();
                txtDisplayName.Text = _userData["姓名"].ToString();
                txtPassword.Text = _userData["密码"].ToString();
                txtConfirmPassword.Text = _userData["密码"].ToString();
                
                // 设置下拉框选择
                if (!string.IsNullOrEmpty(_userData["部门"].ToString()))
                {
                    cmbDepartment.SelectedItem = _userData["部门"].ToString();
                }
                
                if (!string.IsNullOrEmpty(_userData["角色"].ToString()))
                {
                    cmbRole.SelectedItem = _userData["角色"].ToString();
                }
                
                if (!string.IsNullOrEmpty(_userData["状态"].ToString()))
                {
                    cmbStatus.SelectedItem = _userData["状态"].ToString();
                }
                
                txtRemark.Text = _userData["备注"].ToString();
            }
            
            // 用户ID在编辑模式下不可修改
            txtUserID.Enabled = _isAddMode;
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 验证输入
            if (!ValidateInput())
                return;
            
            try
            {
                // 更新数据
                _userData["用户名"] = txtUsername.Text.Trim();
                _userData["姓名"] = txtDisplayName.Text.Trim();
                
                // 仅在添加模式下或密码有修改时更新密码
                if (_isAddMode || !string.IsNullOrEmpty(txtPassword.Text))
                {
                    _userData["密码"] = txtPassword.Text;
                }
                
                _userData["部门"] = cmbDepartment.SelectedItem?.ToString() ?? string.Empty;
                _userData["角色"] = cmbRole.SelectedItem?.ToString() ?? string.Empty;
                _userData["状态"] = cmbStatus.SelectedItem?.ToString() ?? string.Empty;
                _userData["备注"] = txtRemark.Text.Trim();
                
                // 新增用户时设置用户ID
                if (_isAddMode)
                {
                    _userData["用户ID"] = txtUserID.Text.Trim();
                }
                
                // 返回成功
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存用户数据时出错");
                MessageBox.Show($"保存用户数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        
        private bool ValidateInput()
        {
            // 验证必填字段
            if (string.IsNullOrEmpty(txtUsername.Text.Trim()))
            {
                MessageBox.Show("请输入用户名", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }
            
            if (string.IsNullOrEmpty(txtDisplayName.Text.Trim()))
            {
                MessageBox.Show("请输入姓名", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDisplayName.Focus();
                return false;
            }
            
            // 在添加模式下必须输入密码
            if (_isAddMode && string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("请输入密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }
            
            // 如果输入了密码，验证确认密码
            if (!string.IsNullOrEmpty(txtPassword.Text) && txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("两次输入的密码不一致", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return false;
            }
            
            // 验证部门和角色
            if (cmbDepartment.SelectedIndex == -1)
            {
                MessageBox.Show("请选择部门", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbDepartment.Focus();
                return false;
            }
            
            if (cmbRole.SelectedIndex == -1)
            {
                MessageBox.Show("请选择角色", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbRole.Focus();
                return false;
            }
            
            // 验证状态
            if (cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("请选择状态", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbStatus.Focus();
                return false;
            }
            
            return true;
        }
        
        private void txtUserID_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 只允许输入数字、字母和退格
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
        
        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 只允许输入数字、字母、下划线和退格
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '_' && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
} 