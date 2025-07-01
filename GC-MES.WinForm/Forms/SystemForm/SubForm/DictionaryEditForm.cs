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

namespace GC_MES.WinForm.Forms.SystemForm.SubForm
{
    public partial class DictionaryEditForm : Form
    {
        private readonly ISys_DictionaryService _dictionaryService;
        private Sys_Dictionary _dictionary;
        private bool _isAdd = true;

        public DictionaryEditForm()
        {
            InitializeComponent();
            _dictionaryService = Program.ServiceProvider.GetService(typeof(ISys_DictionaryService)) as ISys_DictionaryService;
            _dictionary = new Sys_Dictionary();
            _isAdd = true;
            Text = "新增数据字典";
            lblTitle.Text = "新增数据字典";
        }

        public DictionaryEditForm(Sys_Dictionary dictionary)
        {
            InitializeComponent();
            _dictionaryService = Program.ServiceProvider.GetService(typeof(ISys_DictionaryService)) as ISys_DictionaryService;
            _dictionary = dictionary;
            _isAdd = false;
            Text = "编辑数据字典";
            lblTitle.Text = "编辑数据字典";
        }

        private void DictionaryEditForm_Load(object sender, EventArgs e)
        {
            // 应用主题
            Common.ThemeManager.Instance.ApplyTheme(this);

            // 加载父级字典
            LoadParentDictionaries();

            // 如果是编辑模式，加载字典数据
            if (!_isAdd)
            {
                LoadDictionaryData();
            }
            else
            {
                // 默认值
                nudOrderNo.Value = 100;
                chkEnable.Checked = true;
            }
        }

        private void LoadParentDictionaries()
        {
            try
            {
                // 获取所有字典
                var allDictionaries = _dictionaryService.Query();

                // 添加一个"顶级字典"选项
                var dictionaryList = new List<Sys_Dictionary>
                {
                    new Sys_Dictionary { Dic_ID = 0, DicName = "顶级字典" }
                };

                // 添加其他字典作为可选父级
                if (!_isAdd)
                {
                    // 编辑模式下，排除自身
                    foreach (var dic in allDictionaries)
                    {
                        if (dic.Dic_ID != _dictionary.Dic_ID)
                        {
                            dictionaryList.Add(dic);
                        }
                    }
                }
                else
                {
                    // 新增模式下，添加所有字典
                    dictionaryList.AddRange(allDictionaries);
                }

                cmbParent.DataSource = dictionaryList;
                cmbParent.DisplayMember = "DicName";
                cmbParent.ValueMember = "Dic_ID";

                // 默认选择顶级字典
                cmbParent.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载父级字典失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDictionaryData()
        {
            txtDicNo.Text = _dictionary.DicNo;
            txtDicName.Text = _dictionary.DicName;
            txtConfig.Text = _dictionary.Config;
            txtDbSql.Text = _dictionary.DbSql;
            txtRemark.Text = _dictionary.Remark;
            nudOrderNo.Value = _dictionary.OrderNo ?? 100;
            chkEnable.Checked = _dictionary.Enable == 1;

            // 设置父级字典
            foreach (Sys_Dictionary item in cmbParent.Items)
            {
                if (item.Dic_ID == _dictionary.ParentId)
                {
                    cmbParent.SelectedItem = item;
                    break;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 验证输入
            if (string.IsNullOrWhiteSpace(txtDicNo.Text))
            {
                MessageBox.Show("字典编号不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDicNo.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDicName.Text))
            {
                MessageBox.Show("字典名称不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDicName.Focus();
                return;
            }

            try
            {
                // 检查字典编号是否已存在
                var existingDictionary = _dictionaryService.QueryFirst(d => d.DicNo == txtDicNo.Text && d.Dic_ID != _dictionary.Dic_ID);
                if (existingDictionary != null)
                {
                    MessageBox.Show("字典编号已存在，请更换", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDicNo.Focus();
                    return;
                }

                // 设置字典属性
                _dictionary.DicNo = txtDicNo.Text.Trim();
                _dictionary.DicName = txtDicName.Text.Trim();
                _dictionary.ParentId = (int)cmbParent.SelectedValue;
                _dictionary.Config = txtConfig.Text.Trim();
                _dictionary.DbSql = txtDbSql.Text.Trim();
                _dictionary.Remark = txtRemark.Text.Trim();
                _dictionary.OrderNo = (int)nudOrderNo.Value;
                _dictionary.Enable = chkEnable.Checked ? (byte)1 : (byte)0;

                bool result;
                if (_isAdd)
                {
                    // 添加新字典
                    _dictionary.CreateID = Program.CurrentUser?.User_Id ?? 0;
                    _dictionary.Creator = Program.CurrentUser?.UserName ?? "admin";
                    _dictionary.CreateDate = DateTime.Now;
                    result = _dictionaryService.Add(_dictionary);
                }
                else
                {
                    // 更新字典
                    _dictionary.ModifyID = Program.CurrentUser?.User_Id ?? 0;
                    _dictionary.Modifier = Program.CurrentUser?.UserName ?? "admin";
                    _dictionary.ModifyDate = DateTime.Now;
                    result = _dictionaryService.Update(_dictionary);
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
