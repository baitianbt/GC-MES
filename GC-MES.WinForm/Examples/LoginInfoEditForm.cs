using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GC_MES.BLL.System.IService;
using GC_MES.Model.System;
using System.Reflection;

namespace GC_MES.WinForm.Forms.SystemForm
{
    public partial class LoginInfoEditForm : Form
    {
        private LoginInfo _entity;
        private bool _isNew = true;
        private readonly ILoginInfoService _logininfoService;

        /// <summary>
        /// 构造函数 - 新增模式
        /// </summary>
        public LoginInfoEditForm(ILoginInfoService logininfoService = null)
        {
            InitializeComponent();

            // 初始化服务，如果未提供则从IoC容器获取
            _logininfoService = logininfoService ;

            // 新增模式
            _isNew = true;
            _entity = new LoginInfo();

            // 应用主题
            Common.ThemeManager.Instance.ApplyTheme(this);
            
            // 初始化下拉框
            InitComboBoxes();
            
            // 设置控件默认值
            InitControlDefaultValues();
        }

        /// <summary>
        /// 构造函数 - 编辑模式
        /// </summary>
        public LoginInfoEditForm(LoginInfo entity, ILoginInfoService logininfoService = null)
            : this(logininfoService)
        {
            if (entity != null)
            {
                // 编辑模式
                _isNew = false;
                _entity = entity;

                // 加载实体数据到控件
                LoadEntityToControls();
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void LoginInfoEditForm_Load(object sender, EventArgs e)
        {
            // 设置窗体标题
            this.Text = _isNew ? "新增LoginInfo" : "编辑LoginInfo";

            // 如果是编辑模式，禁用ID等主键字段
            DisablePrimaryKeyControls();
        }

        /// <summary>
        /// 初始化下拉框
        /// </summary>
        private void InitComboBoxes()
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化下拉框失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化控件默认值
        /// </summary>
        private void InitControlDefaultValues()
        {
            // 设置默认值
        }

        /// <summary>
        /// 加载实体数据到控件
        /// </summary>
        private void LoadEntityToControls()
        {
            if (_entity == null) return;

            // 加载属性值到对应控件
            txtUserName.Text = _entity.UserName ?? string.Empty;
            txtPassword.Text = _entity.Password ?? string.Empty;
            txtVerificationCode.Text = _entity.VerificationCode ?? string.Empty;
            txtUUID.Text = _entity.UUID ?? string.Empty;
        }

        /// <summary>
        /// 从控件收集数据到实体
        /// </summary>
        private void CollectDataFromControls()
        {
            // 收集属性值从对应控件
            _entity.UserName = txtUserName.Text.Trim();
            _entity.Password = txtPassword.Text.Trim();
            _entity.VerificationCode = txtVerificationCode.Text.Trim();
            _entity.UUID = txtUUID.Text.Trim();
        }

        /// <summary>
        /// 禁用主键相关控件
        /// </summary>
        private void DisablePrimaryKeyControls()
        {
            if (!_isNew)
            {
            }
        }

        /// <summary>
        /// 验证数据
        /// </summary>
        private bool ValidateData()
        {
            
            // 验证必填字段
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                MessageBox.Show($"用户名不能为空", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show($"密码不能为空", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtVerificationCode.Text))
            {
                MessageBox.Show($"验证码不能为空", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtVerificationCode.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtUUID.Text))
            {
                MessageBox.Show($"UUID不能为空", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUUID.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 保存按钮点击事件
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 验证数据
            if (!ValidateData())
            {
                return;
            }

            try
            {
                // 收集数据
                CollectDataFromControls();

                bool result;
                if (_isNew)
                {
                    // 新增
                    result = _logininfoService.Add(_entity);
                }
                else
                {
                    // 编辑
                    result = _logininfoService.Update(_entity);
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

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// 获取类型或属性的Display特性的Name值
        /// </summary>
        private string GetDisplayName(MemberInfo member)
        {
            var displayAttr = member.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), true)
                .FirstOrDefault() as System.ComponentModel.DataAnnotations.DisplayAttribute;
            return displayAttr?.Name ?? member.Name;
        }
    }
}
