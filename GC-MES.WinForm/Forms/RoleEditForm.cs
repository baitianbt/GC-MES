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

namespace GC_MES.WinForm.Forms
{
    public partial class RoleEditForm : Form
    {
        private readonly ILogger<RoleEditForm> _logger;
        private readonly IConfiguration _configuration;
        private readonly MesDbContext _dbContext;
        
        // 当前编辑的角色
        private Role _currentRole;
        private bool _isNewRole = true;

        public RoleEditForm(ILogger<RoleEditForm> logger, IConfiguration configuration, MesDbContext dbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
            
            InitializeComponent();
            
            // 应用当前主题
            ThemeManager.ApplyTheme(this);
        }

        /// <summary>
        /// 设置当前编辑的角色
        /// </summary>
        /// <param name="role">要编辑的角色，如果为null则表示新增角色</param>
        public void SetRole(Role role)
        {
            if (role == null)
            {
                // 新增角色
                _currentRole = new Role
                {
                    IsActive = true,
                    CreateTime = DateTime.Now,
                    CreateBy = "admin" // 实际应用中应使用当前登录用户
                };
                _isNewRole = true;
                
                // 更新窗体标题
                this.Text = "新增角色";
            }
            else
            {
                // 编辑角色
                _currentRole = role;
                _isNewRole = false;
                
                // 更新窗体标题
                this.Text = $"编辑角色 - {role.RoleName}";
                
                // 填充表单
                txtRoleName.Text = role.RoleName;
                txtDescription.Text = role.Description;
                chkIsActive.Checked = role.IsActive;
            }
        }

        private void RoleEditForm_Load(object sender, EventArgs e)
        {
            // 初始化控件状态
            if (_isNewRole)
            {
                // 新增角色时，默认启用状态
                chkIsActive.Checked = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证输入
                if (string.IsNullOrWhiteSpace(txtRoleName.Text))
                {
                    MessageBox.Show("角色名称不能为空", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRoleName.Focus();
                    return;
                }
                
                // 验证角色名称唯一性
                if (_isNewRole || txtRoleName.Text != _currentRole.RoleName)
                {
                    bool exists = _dbContext.Roles.Any(r => r.RoleName == txtRoleName.Text);
                    if (exists)
                    {
                        MessageBox.Show("角色名称已存在", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtRoleName.Focus();
                        return;
                    }
                }
                
                // 更新角色信息
                _currentRole.RoleName = txtRoleName.Text.Trim();
                _currentRole.Description = txtDescription.Text.Trim();
                _currentRole.IsActive = chkIsActive.Checked;
                
                // 保存到数据库
                if (_isNewRole)
                {
                    _dbContext.Roles.Add(_currentRole);
                }
                else
                {
                    // 更新时间和更新人
                    _currentRole.UpdateTime = DateTime.Now;
                    _currentRole.UpdateBy = "admin"; // 实际应用中应使用当前登录用户
                }
                
                _dbContext.SaveChanges();
                
                // 关闭窗体
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存角色时出错");
                MessageBox.Show($"保存角色时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 取消操作
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
} 