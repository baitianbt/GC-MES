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
    public partial class DictionaryManagementForm : Form
    {
        private readonly ISys_DictionaryService _dictionaryService;
        private readonly ISys_DictionaryListService _dictionaryListService;
        private int _pageIndex = 1;
        private int _pageSize = 20;
        private int _totalCount = 0;
        private Sys_Dictionary _selectedDictionary;

        public DictionaryManagementForm()
        {
            InitializeComponent();
            _dictionaryService = Program.Services.GetService(typeof(ISys_DictionaryService)) as ISys_DictionaryService;
            _dictionaryListService = Program.Services.GetService(typeof(ISys_DictionaryListService)) as ISys_DictionaryListService;
        }

        private void DictionaryManagementForm_Load(object sender, EventArgs e)
        {
            // 应用主题
            Common.ThemeManager.Instance.ApplyTheme(this);

            // 加载数据
            LoadDictionaries();

            // 绑定事件
            BindEvents();
        }

        private void BindEvents()
        {
            // 主表事件
            btnSearch.Click += BtnSearch_Click;
            btnClear.Click += BtnClear_Click;
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnFirstPage.Click += BtnFirstPage_Click;
            btnPrevPage.Click += BtnPrevPage_Click;
            btnNextPage.Click += BtnNextPage_Click;
            btnLastPage.Click += BtnLastPage_Click;

            // 明细表事件
            btnAddItem.Click += BtnAddItem_Click;
            btnEditItem.Click += BtnEditItem_Click;
            btnDeleteItem.Click += BtnDeleteItem_Click;

            // 选择事件
            dgvDictionaries.SelectionChanged += DgvDictionaries_SelectionChanged;
        }

        private void LoadDictionaries()
        {
            try
            {
                // 获取查询条件
                string dicName = txtSearchName.Text.Trim();
                string dicNo = txtSearchCode.Text.Trim();

                // 构建查询条件
                List<Sys_Dictionary> result;
                if (string.IsNullOrEmpty(dicName) && string.IsNullOrEmpty(dicNo))
                {
                    // 无条件查询
                    result = _dictionaryService.QueryPage(_pageIndex, _pageSize, out _totalCount, null, d => d.OrderNo, true);
                }
                else
                {
                    // 条件查询
                    result = _dictionaryService.QueryPage(
                        _pageIndex,
                        _pageSize,
                        out _totalCount,
                        d => (string.IsNullOrEmpty(dicName) || d.DicName.Contains(dicName)) &&
                             (string.IsNullOrEmpty(dicNo) || d.DicNo.Contains(dicNo)),
                        d => d.OrderNo,
                        true);
                }

                // 绑定数据到DataGridView
                dgvDictionaries.DataSource = result;

                // 更新分页信息
                int totalPages = (_totalCount + _pageSize - 1) / _pageSize;
                lblPageInfo.Text = $"第 {_pageIndex}/{totalPages} 页，共 {_totalCount} 条";

                // 更新分页按钮状态
                btnFirstPage.Enabled = btnPrevPage.Enabled = _pageIndex > 1;
                btnNextPage.Enabled = btnLastPage.Enabled = _pageIndex < totalPages;

                // 清空明细表
                dgvDictionaryItems.DataSource = null;
                _selectedDictionary = null;

                // 禁用明细按钮
                EnableItemButtons(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载数据字典失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDictionaryItems(int dictionaryId)
        {
            try
            {
                // 查询明细数据
                var items = _dictionaryListService.Query(d => d.Dic_ID == dictionaryId, d => d.OrderNo, true);

                // 绑定数据到DataGridView
                dgvDictionaryItems.DataSource = items;

                // 启用明细按钮
                EnableItemButtons(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载数据字典明细失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnableItemButtons(bool enabled)
        {
            btnAddItem.Enabled = enabled;
            btnEditItem.Enabled = enabled;
            btnDeleteItem.Enabled = enabled;
        }

        #region 主表事件处理
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            _pageIndex = 1;
            LoadDictionaries();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtSearchName.Text = string.Empty;
            txtSearchCode.Text = string.Empty;
            _pageIndex = 1;
            LoadDictionaries();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var editForm = new DictionaryEditForm();
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadDictionaries();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDictionaries.CurrentRow == null)
            {
                MessageBox.Show("请先选择一条记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedDictionary = dgvDictionaries.CurrentRow.DataBoundItem as Sys_Dictionary;
            if (selectedDictionary == null) return;

            var editForm = new DictionaryEditForm(selectedDictionary);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadDictionaries();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDictionaries.CurrentRow == null)
            {
                MessageBox.Show("请先选择一条记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedDictionary = dgvDictionaries.CurrentRow.DataBoundItem as Sys_Dictionary;
            if (selectedDictionary == null) return;

            // 检查是否有明细数据
            var items = _dictionaryListService.Query(d => d.Dic_ID == selectedDictionary.Dic_ID);
            if (items.Any())
            {
                if (MessageBox.Show($"字典 [{selectedDictionary.DicName}] 包含 {items.Count} 条明细数据，删除将同时删除所有明细数据，确定要删除吗？",
                    "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }
            }
            else
            {
                if (MessageBox.Show($"确定要删除字典 [{selectedDictionary.DicName}] 吗？",
                    "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
            }

            try
            {
                // 删除明细数据
                foreach (var item in items)
                {
                    _dictionaryListService.Delete(item);
                }

                // 删除主表数据
                bool result = _dictionaryService.Delete(selectedDictionary);
                if (result)
                {
                    MessageBox.Show("删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDictionaries();
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

        private void DgvDictionaries_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDictionaries.CurrentRow == null)
            {
                _selectedDictionary = null;
                dgvDictionaryItems.DataSource = null;
                EnableItemButtons(false);
                return;
            }

            _selectedDictionary = dgvDictionaries.CurrentRow.DataBoundItem as Sys_Dictionary;
            if (_selectedDictionary != null)
            {
                LoadDictionaryItems(_selectedDictionary.Dic_ID);
            }
        }

        private void BtnFirstPage_Click(object sender, EventArgs e)
        {
            if (_pageIndex > 1)
            {
                _pageIndex = 1;
                LoadDictionaries();
            }
        }

        private void BtnPrevPage_Click(object sender, EventArgs e)
        {
            if (_pageIndex > 1)
            {
                _pageIndex--;
                LoadDictionaries();
            }
        }

        private void BtnNextPage_Click(object sender, EventArgs e)
        {
            int totalPages = (_totalCount + _pageSize - 1) / _pageSize;
            if (_pageIndex < totalPages)
            {
                _pageIndex++;
                LoadDictionaries();
            }
        }

        private void BtnLastPage_Click(object sender, EventArgs e)
        {
            int totalPages = (_totalCount + _pageSize - 1) / _pageSize;
            if (_pageIndex < totalPages)
            {
                _pageIndex = totalPages;
                LoadDictionaries();
            }
        }
        #endregion

        #region 明细表事件处理
        private void BtnAddItem_Click(object sender, EventArgs e)
        {
            if (_selectedDictionary == null)
            {
                MessageBox.Show("请先选择一个数据字典", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var editForm = new DictionaryItemEditForm(_selectedDictionary.Dic_ID);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadDictionaryItems(_selectedDictionary.Dic_ID);
            }
        }

        private void BtnEditItem_Click(object sender, EventArgs e)
        {
            if (dgvDictionaryItems.CurrentRow == null)
            {
                MessageBox.Show("请先选择一条明细记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedItem = dgvDictionaryItems.CurrentRow.DataBoundItem as Sys_DictionaryList;
            if (selectedItem == null) return;

            var editForm = new DictionaryItemEditForm(selectedItem);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadDictionaryItems(_selectedDictionary.Dic_ID);
            }
        }

        private void BtnDeleteItem_Click(object sender, EventArgs e)
        {
            if (dgvDictionaryItems.CurrentRow == null)
            {
                MessageBox.Show("请先选择一条明细记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedItem = dgvDictionaryItems.CurrentRow.DataBoundItem as Sys_DictionaryList;
            if (selectedItem == null) return;

            if (MessageBox.Show($"确定要删除字典项 [{selectedItem.DicName}] 吗？",
                "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                bool result = _dictionaryListService.Delete(selectedItem);
                if (result)
                {
                    MessageBox.Show("删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDictionaryItems(_selectedDictionary.Dic_ID);
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
        #endregion
    }
}
