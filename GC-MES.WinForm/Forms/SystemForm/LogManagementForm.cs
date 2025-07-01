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
    public partial class LogManagementForm : Form
    {
        private readonly ISys_LogService _logService;
        private int _pageIndex = 1;
        private int _pageSize = 20;
        private int _totalCount = 0;

        public LogManagementForm()
        {
            InitializeComponent();
            _logService = Program.ServiceProvider.GetService(typeof(ISys_LogService)) as ISys_LogService;
        }

        private void LogManagementForm_Load(object sender, EventArgs e)
        {
            // 应用主题
            Common.ThemeManager.Instance.ApplyTheme(this);

            // 初始化日志类型下拉框
            InitLogTypes();

            // 加载数据
            LoadData();

            // 绑定事件
            BindEvents();
        }

        private void InitLogTypes()
        {
            cmbLogType.Items.Clear();
            cmbLogType.Items.Add("全部");
            cmbLogType.Items.Add("操作日志");
            cmbLogType.Items.Add("异常日志");
            cmbLogType.Items.Add("登录日志");
            cmbLogType.SelectedIndex = 0;
        }

        private void BindEvents()
        {
            btnSearch.Click += BtnSearch_Click;
            btnClear.Click += BtnClear_Click;
            btnViewDetail.Click += BtnViewDetail_Click;
            btnExport.Click += BtnExport_Click;
            btnDelete.Click += BtnDelete_Click;
            btnFirstPage.Click += BtnFirstPage_Click;
            btnPrevPage.Click += BtnPrevPage_Click;
            btnNextPage.Click += BtnNextPage_Click;
            btnLastPage.Click += BtnLastPage_Click;
        }

        private void LoadData()
        {
            try
            {
                // 获取查询条件
                string userName = txtUserName.Text.Trim();
                string logType = cmbLogType.SelectedIndex > 0 ? cmbLogType.SelectedItem.ToString() : null;
                DateTime? startDate = null;
                DateTime? endDate = null;

                if (dtpStartDate.Checked)
                    startDate = dtpStartDate.Value;

                if (dtpEndDate.Checked)
                    endDate = dtpEndDate.Value.AddDays(1).AddSeconds(-1); // 设置为当天的最后一秒

                // 构建查询条件
                List<Sys_Log> result;
                if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(logType) && !startDate.HasValue && !endDate.HasValue)
                {
                    // 无条件查询
                    result = _logService.QueryPage(_pageIndex, _pageSize, out _totalCount, null, l => l.BeginDate, false);
                }
                else
                {
                    // 条件查询
                    result = _logService.QueryPage(
                        _pageIndex,
                        _pageSize,
                        out _totalCount,
                        l => (string.IsNullOrEmpty(userName) || l.UserName.Contains(userName)) &&
                             (string.IsNullOrEmpty(logType) || l.LogType == logType) &&
                             (!startDate.HasValue || l.BeginDate >= startDate) &&
                             (!endDate.HasValue || l.BeginDate <= endDate),
                        l => l.BeginDate,
                        false);
                }

                // 绑定数据到DataGridView
                dgvLogs.DataSource = result;

                // 更新分页信息
                int totalPages = (_totalCount + _pageSize - 1) / _pageSize;
                lblPageInfo.Text = $"第 {_pageIndex}/{totalPages} 页，共 {_totalCount} 条";

                // 更新分页按钮状态
                btnFirstPage.Enabled = btnPrevPage.Enabled = _pageIndex > 1;
                btnNextPage.Enabled = btnLastPage.Enabled = _pageIndex < totalPages;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载日志数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtUserName.Text = string.Empty;
            cmbLogType.SelectedIndex = 0;
            dtpStartDate.Checked = false;
            dtpEndDate.Checked = false;
            _pageIndex = 1;
            LoadData();
        }

        private void BtnViewDetail_Click(object sender, EventArgs e)
        {
            if (dgvLogs.CurrentRow == null)
            {
                MessageBox.Show("请先选择一条记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedLog = dgvLogs.CurrentRow.DataBoundItem as Sys_Log;
            if (selectedLog == null) return;

            var detailForm = new LogDetailForm(selectedLog);
            detailForm.ShowDialog();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            // 导出Excel功能
            MessageBox.Show("导出Excel功能待实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvLogs.CurrentRow == null)
            {
                MessageBox.Show("请先选择一条记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedLog = dgvLogs.CurrentRow.DataBoundItem as Sys_Log;
            if (selectedLog == null) return;

            if (MessageBox.Show($"确定要删除选中的日志记录吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    bool result = _logService.Delete(selectedLog);
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
        #endregion
    }
}
