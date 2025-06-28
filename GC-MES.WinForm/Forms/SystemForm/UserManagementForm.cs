using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GC_MES.Model.System;
using System.Linq;
using SqlSugar;
using GC_MES.WinForm.Forms.SystemForm.SubForm;

namespace GC_MES.WinForm.Forms.SystemForm
{
    public partial class UserManagementForm : Form
    {
      
        private int _pageIndex = 1;
        private int _pageSize = 20;
        private int _totalCount = 0;
        private int _totalPages = 0;

        // 用于编辑或删除的当前选中用户
        private Sys_User _currentUser = null;

        // 查询参数
        private string _searchName = "";
        private string _searchMobile = "";
        private string _searchEmail = "";

        public UserManagementForm()
        {
           

            InitializeComponent();

            // 绑定事件
            Load += UserManagementForm_Load;
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

            dgvUsers.SelectionChanged += DgvUsers_SelectionChanged;
            dgvUsers.CellDoubleClick += DgvUsers_CellDoubleClick;

            // 自定义DataGridView样式
            CustomizeDataGridView();
        }

        private void UserManagementForm_Load(object sender, EventArgs e)
        {
            try
            {
                // 加载用户数据
                LoadUserData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 自定义DataGridView样式
        /// </summary>
        private void CustomizeDataGridView()
        {
            // 设置列头样式
            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 63, 88);
            dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular);

            dgvUsers.ColumnHeadersHeight = 40;

// 设置行样式
dgvUsers.RowsDefaultCellStyle.BackColor = Color.White;
dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
dgvUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(87, 115, 153);
dgvUsers.DefaultCellStyle.SelectionForeColor = Color.White;
dgvUsers.RowTemplate.Height = 36;

// 设置边框和网格线
dgvUsers.BorderStyle = BorderStyle.None;
dgvUsers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
dgvUsers.GridColor = Color.FromArgb(230, 230, 230);

// 设置其他属性
dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
dgvUsers.MultiSelect = false;
dgvUsers.ReadOnly = true;
dgvUsers.AllowUserToAddRows = false;
dgvUsers.AllowUserToDeleteRows = false;
dgvUsers.AllowUserToResizeRows = false;
        }
        
        /// <summary>
        /// 加载用户数据
        /// </summary>
        private void LoadUserData()
{
    try
    {
        // 构建查询条件

        // 添加搜索条件
        if (!string.IsNullOrEmpty(_searchName))
        {
        }

        if (!string.IsNullOrEmpty(_searchMobile))
        {
        }

        if (!string.IsNullOrEmpty(_searchEmail))
        {
        }

        // 获取总数

        // 计算总页数
        _totalPages = (_totalCount + _pageSize - 1) / _pageSize;

        // 确保页码有效
        if (_pageIndex < 1) _pageIndex = 1;
        if (_pageIndex > _totalPages && _totalPages > 0) _pageIndex = _totalPages;

        // 查询当前页数据

        // 绑定数据

        // 更新分页信息
        UpdatePagination();

        // 更新按钮状态
        UpdateButtonStatus();

    }
    catch (Exception ex)
    {
        MessageBox.Show($"加载数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

/// <summary>
/// 更新分页信息显示
/// </summary>
private void UpdatePagination()
{
    lblPageInfo.Text = $"第 {_pageIndex}/{_totalPages} 页，共 {_totalCount} 条";

    // 更新分页按钮状态
    btnFirstPage.Enabled = _pageIndex > 1;
    btnPrevPage.Enabled = _pageIndex > 1;
    btnNextPage.Enabled = _pageIndex < _totalPages;
    btnLastPage.Enabled = _pageIndex < _totalPages;
}

/// <summary>
/// 更新按钮状态
/// </summary>
private void UpdateButtonStatus()
{
    bool hasSelectedUser = _currentUser != null;

    btnEdit.Enabled = hasSelectedUser;
    btnDelete.Enabled = hasSelectedUser;
}

#region 分页控制事件

private void BtnFirstPage_Click(object sender, EventArgs e)
{
    if (_pageIndex != 1)
    {
        _pageIndex = 1;
        LoadUserData();
    }
}

private void BtnPrevPage_Click(object sender, EventArgs e)
{
    if (_pageIndex > 1)
    {
        _pageIndex--;
        LoadUserData();
    }
}

private void BtnNextPage_Click(object sender, EventArgs e)
{
    if (_pageIndex < _totalPages)
    {
        _pageIndex++;
        LoadUserData();
    }
}

private void BtnLastPage_Click(object sender, EventArgs e)
{
    if (_pageIndex != _totalPages)
    {
        _pageIndex = _totalPages;
        LoadUserData();
    }
}

#endregion

#region 搜索相关事件

private void BtnSearch_Click(object sender, EventArgs e)
{
    _searchName = txtSearchName.Text.Trim();
    _searchMobile = txtSearchMobile.Text.Trim();
    _searchEmail = txtSearchEmail.Text.Trim();

    // 重置页码
    _pageIndex = 1;

    // 重新加载数据
    LoadUserData();
}

private void BtnClear_Click(object sender, EventArgs e)
{
    // 清空搜索框
    txtSearchName.Text = "";
    txtSearchMobile.Text = "";
    txtSearchEmail.Text = "";

    // 清空搜索条件
    _searchName = "";
    _searchMobile = "";
    _searchEmail = "";

    // 重置页码
    _pageIndex = 1;

    // 重新加载数据
    LoadUserData();
}

#endregion

#region 数据网格事件

private void DgvUsers_SelectionChanged(object sender, EventArgs e)
{
    if (dgvUsers.SelectedRows.Count > 0)
    {
        // 获取当前选中行的数据
        DataGridViewRow row = dgvUsers.SelectedRows[0];
        int userId = Convert.ToInt32(row.Cells["colUserId"].Value);

        // 获取完整的用户对象

        // 更新按钮状态
        UpdateButtonStatus();
    }
    else
    {
        _currentUser = null;
        UpdateButtonStatus();
    }
}

private void DgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
{
    if (e.RowIndex >= 0)
    {
        // 双击行时编辑用户
        BtnEdit_Click(sender, e);
    }
}

#endregion

#region CRUD操作事件

private void BtnAdd_Click(object sender, EventArgs e)
{
    try
    {
        // 创建用户编辑窗体
    }
    catch (Exception ex)
    {
        MessageBox.Show($"添加用户失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

private void BtnEdit_Click(object sender, EventArgs e)
{
    if (_currentUser == null)
    {
        MessageBox.Show("请先选择要编辑的用户", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
    }

    try
    {
        // 创建用户编辑窗体
    }
    catch (Exception ex)
    {
        MessageBox.Show($"编辑用户失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

private void BtnDelete_Click(object sender, EventArgs e)
{
    if (_currentUser == null)
    {
        MessageBox.Show("请先选择要删除的用户", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
    }

    // 确认删除
    var result = MessageBox.Show($"确定要删除用户 [{_currentUser.UserName}] 吗？", "确认删除",
        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

    if (result == DialogResult.Yes)
    {
        try
        {
            // 执行删除

           
        }
        catch (Exception ex)
        {
            MessageBox.Show($"删除用户失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

#endregion

#region 导入导出功能

private void BtnImport_Click(object sender, EventArgs e)
{
    try
    {
        // 创建打开文件对话框
        using (var dialog = new OpenFileDialog())
        {
            dialog.Filter = "Excel文件|*.xlsx;*.xls";
            dialog.Title = "选择要导入的Excel文件";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // TODO: 实现Excel导入功能
                MessageBox.Show($"导入文件: {dialog.FileName}\r\n此功能尚未实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 导入成功后重新加载数据
                // LoadUserData();
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"导入Excel失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

private void BtnExport_Click(object sender, EventArgs e)
{
    try
    {
        // 创建保存文件对话框
        using (var dialog = new SaveFileDialog())
        {
            dialog.Filter = "Excel文件|*.xlsx";
            dialog.Title = "保存Excel文件";
            dialog.FileName = $"用户数据_{DateTime.Now:yyyyMMdd}.xlsx";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // TODO: 实现Excel导出功能
                MessageBox.Show($"导出文件: {dialog.FileName}\r\n此功能尚未实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"导出Excel失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

private void BtnExportPDF_Click(object sender, EventArgs e)
{
    try
    {
        // 创建保存文件对话框
        using (var dialog = new SaveFileDialog())
        {
            dialog.Filter = "PDF文件|*.pdf";
            dialog.Title = "保存PDF文件";
            dialog.FileName = $"用户数据_{DateTime.Now:yyyyMMdd}.pdf";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // TODO: 实现PDF导出功能
                MessageBox.Show($"导出文件: {dialog.FileName}\r\n此功能尚未实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"导出PDF失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

        #endregion
    }
    
}