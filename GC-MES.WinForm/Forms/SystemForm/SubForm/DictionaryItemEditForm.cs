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
    public partial class DictionaryItemEditForm : Form
    {
        private readonly ISys_DictionaryListService _dictionaryListService;
        private Sys_DictionaryList _dictionaryItem;
        private bool _isAdd = true;
        private int _dictionaryId;

        public DictionaryItemEditForm(int dictionaryId)
        {
            InitializeComponent();
            _dictionaryListService = Program.ServiceProvider.GetService(typeof(ISys_DictionaryListService)) as ISys_DictionaryListService;
            _dictionaryItem = new Sys_DictionaryList { Dic_ID = dictionaryId };
            _dictionaryId = dictionaryId;
            _isAdd = true;
            Text = "新增字典项";
            lblTitle.Text = "新增字典项";
        }

        public DictionaryItemEditForm(Sys_DictionaryList dictionaryItem)
        {
            InitializeComponent();
            _dictionaryListService = Program.ServiceProvider.GetService(typeof(ISys_DictionaryListService)) as ISys_DictionaryListService;
            _dictionaryItem = dictionaryItem;
            _dictionaryId = dictionaryItem.Dic_ID ?? 0;
            _isAdd = false;
            Text = "编辑字典项";
            lblTitle.Text = "编辑字典项";
        }

        private void DictionaryItemEditForm_Load(object sender, EventArgs e)
        {
            // 应用主题
            Common.ThemeManager.Instance.ApplyTheme(this);

            // 如果是编辑模式，加载字典项数据
            if (!_isAdd)
            {
                LoadDictionaryItemData();
            }
            else
            {
                // 默认值
                nudOrderNo.Value = 100;
                chkEnable.Checked = true;
            }
        }

        private void LoadDictionaryItemData()
        {
            txtDicValue.Text = _dictionaryItem.DicValue;
            txtDicName.Text = _dictionaryItem.DicName;
            txtRemark.Text = _dictionaryItem.Remark;
            nudOrderNo.Value = _dictionaryItem.OrderNo ?? 100;
            chkEnable.Checked = _dictionaryItem.Enable == 1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 验证输入
            if (string.IsNullOrWhiteSpace(txtDicValue.Text))
            {
                MessageBox.Show("字典值不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDicValue.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDicName.Text))
            {
                MessageBox.Show("字典文本不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDicName.Focus();
                return;
            }

            try
            {
                // 检查字典值是否已存在
                var existingItem = _dictionaryListService.QueryFirst(d => d.Dic_ID == _dictionaryId && d.DicValue == txtDicValue.Text && d.DicList_ID != _dictionaryItem.DicList_ID);
                if (existingItem != null)
                {
                    MessageBox.Show("当前字典下已存在相同的字典值，请更换", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDicValue.Focus();
                    return;
                }

                // 设置字典项属性
                _dictionaryItem.DicValue = txtDicValue.Text.Trim();
                _dictionaryItem.DicName = txtDicName.Text.Trim();
                _dictionaryItem.Remark = txtRemark.Text.Trim();
                _dictionaryItem.OrderNo = (int)nudOrderNo.Value;
                _dictionaryItem.Enable = chkEnable.Checked ? (byte)1 : (byte)0;

                bool result;
                if (_isAdd)
                {
                    // 添加新字典项
                    _dictionaryItem.CreateID = Program.CurrentUser?.User_Id ?? 0;
                    _dictionaryItem.Creator = Program.CurrentUser?.UserName ?? "admin";
                    _dictionaryItem.CreateDate = DateTime.Now;
                    result = _dictionaryListService.Add(_dictionaryItem);
                }
                else
                {
                    // 更新字典项
                    _dictionaryItem.ModifyID = Program.CurrentUser?.User_Id ?? 0;
                    _dictionaryItem.Modifier = Program.CurrentUser?.UserName ?? "admin";
                    _dictionaryItem.ModifyDate = DateTime.Now;
                    result = _dictionaryListService.Update(_dictionaryItem);
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
