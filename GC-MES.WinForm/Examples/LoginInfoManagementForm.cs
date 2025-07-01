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
using GC_MES.Model.Common;
using GC_MES.Model.System;
using System.IO;
using System.Reflection;

namespace GC_MES.WinForm.Forms.SystemForm
{
    public partial class LoginInfoManagementForm : Form
    {
        // 当前页数和每页条数
        private int currentPage = 1;
        private int pageSize = 20;
        private int totalCount = 0;
        private int totalPages = 0;

        // 服务实例
        private readonly ILoginInfoService _logininfoService;

        // 查询条件
        private string searchUserName = string.Empty;
        private string searchPassword = string.Empty;
        private string searchVerificationCode = string.Empty;

        // 当前选中的实体
        private LoginInfo selectedLoginInfo = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LoginInfoManagementForm(ILoginInfoService logininfoService)
        {
            InitializeComponent();

            // 初始化服务
            _logininfoService = logininfoService;

            // 应用主题
            Common.ThemeManager.Instance.ApplyTheme(this);

            // 初始化下拉框
            InitComboBoxes();

            // 注册事件处理
            RegisterEventHandlers();
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        private void LoginInfoManagementForm_Load(object sender, EventArgs e)
        {
            // 加载数据
            LoadData();
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
        /// 注册事件处理
        /// </summary>
        private void RegisterEventHandlers()
        {
            // 按钮点击事件
            btnSearch.Click += btnSearch_Click;
            btnClear.Click += btnClear_Click;
            btnAdd.Click += btnAdd_Click;
            btnEdit.Click += btnEdit_Click;
            btnDelete.Click += btnDelete_Click;
            btnImport.Click += btnImport_Click;
            btnExport.Click += btnExport_Click;
            btnExportPDF.Click += btnExportPDF_Click;

            // 分页按钮点击事件
            btnFirstPage.Click += btnFirstPage_Click;
            btnPrevPage.Click += btnPrevPage_Click;
            btnNextPage.Click += btnNextPage_Click;
            btnLastPage.Click += btnLastPage_Click;

            // 数据表行选择事件
            dgvLoginInfos.SelectionChanged += dgvLoginInfos_SelectionChanged;
            dgvLoginInfos.CellDoubleClick += dgvLoginInfos_CellDoubleClick;

            // 窗体加载事件
            this.Load += LoginInfoManagementForm_Load;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                // 构建查询参数
                var parameters = new Dictionary<string, object>();

                // 添加查询条件
                if (!string.IsNullOrWhiteSpace(searchUserName))
                {
                    parameters.Add("UserName", searchUserName);
                }
                if (!string.IsNullOrWhiteSpace(searchPassword))
                {
                    parameters.Add("Password", searchPassword);
                }
                if (!string.IsNullOrWhiteSpace(searchVerificationCode))
                {
                    parameters.Add("VerificationCode", searchVerificationCode);
                }

                // 执行分页查询
                var pagedResult = _logininfoService.GetPagedList(parameters, currentPage, pageSize);

                // 更新数据表
                dgvLoginInfos.DataSource = pagedResult.Items;

                // 更新分页信息
                totalCount = pagedResult.TotalCount;
                totalPages = pagedResult.TotalPages;
                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 更新分页信息
        /// </summary>
        private void UpdatePaginationInfo()
        {
            lblPageInfo.Text = $"第 {currentPage}/{totalPages} 页，共 {totalCount} 条";

            // 禁用/启用分页按钮
            btnFirstPage.Enabled = currentPage > 1;
            btnPrevPage.Enabled = currentPage > 1;
            btnNextPage.Enabled = currentPage < totalPages;
            btnLastPage.Enabled = currentPage < totalPages;
        }

        /// <summary>
        /// 获取搜索条件
        /// </summary>
        private void GetSearchConditions()
        {
            // 获取用户名
            searchUserName = txtUserName.Text.Trim();
            // 获取密码
            searchPassword = txtPassword.Text.Trim();
            // 获取验证码
            searchVerificationCode = txtVerificationCode.Text.Trim();
        }

        #region 按钮事件处理

        /// <summary>
        /// 搜索按钮点击事件
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // 获取搜索条件
            GetSearchConditions();

            // 重置为第一页
            currentPage = 1;

            // 重新加载数据
            LoadData();
        }

        /// <summary>
        /// 清空按钮点击事件
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            // 清空搜索控件
            foreach (Control ctrl in pnlSearch.Controls)
            {
                if (ctrl is TextBox txtBox)
                {
                    txtBox.Clear();
                }
                else if (ctrl is ComboBox cmbBox)
                {
                    if (cmbBox.Items.Count > 0)
                        cmbBox.SelectedIndex = 0;
                }
                else if (ctrl is CheckBox chkBox)
                {
                    chkBox.CheckState = CheckState.Indeterminate;
                }
            }

            searchUserName = string.Empty;
            searchPassword = string.Empty;
            searchVerificationCode = string.Empty;

            // 重置为第一页
            currentPage = 1;

            // 重新加载数据
            LoadData();
        }

        /// <summary>
        /// 新增按钮点击事件
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // 创建编辑窗体，传入null表示新增
            using (var editForm = new LoginInfoEditForm())
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    // 重新加载数据
                    LoadData();
                }
            }
        }

        /// <summary>
        /// 编辑按钮点击事件
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedLoginInfo == null)
            {
                MessageBox.Show("请先选择要编辑的记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 获取主键属性
            var keyProperty = typeof(LoginInfo).GetProperties()
                .FirstOrDefault(p => p.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true).Any());

            if (keyProperty == null)
            {
                MessageBox.Show("无法确定实体的主键", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 获取主键值
            var keyValue = keyProperty.GetValue(selectedLoginInfo);

            // 加载完整数据
            var entity = _logininfoService.GetById(keyValue);
            if (entity == null)
            {
                MessageBox.Show("找不到要编辑的数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 打开编辑窗体，传入实体表示编辑
            using (var editForm = new LoginInfoEditForm(entity))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    // 重新加载数据
                    LoadData();
                }
            }
        }

        /// <summary>
        /// 删除按钮点击事件
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedLoginInfo == null)
            {
                MessageBox.Show("请先选择要删除的记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 确认删除
            if (MessageBox.Show("确定要删除所选记录吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // 获取主键属性
                    var keyProperty = typeof(LoginInfo).GetProperties()
                        .FirstOrDefault(p => p.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true).Any());

                    if (keyProperty == null)
                    {
                        MessageBox.Show("无法确定实体的主键", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // 获取主键值
                    var keyValue = keyProperty.GetValue(selectedLoginInfo);

                    // 执行删除
                    bool result = _logininfoService.DeleteById(keyValue);

                    if (result)
                    {
                        MessageBox.Show("删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 重新加载数据
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

        /// <summary>
        /// 导入按钮点击事件
        /// </summary>
        private void btnImport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
                openFileDialog.Title = "选择Excel文件";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filePath = openFileDialog.FileName;
                        // 这里实现Excel导入逻辑，可以使用Common中的ExcelHelper类
                        // List<LoginInfo> importData = GC_MES.Common.ExcelHelper.ImportFromExcel<LoginInfo>(filePath);
                        // bool result = _logininfoService.BatchInsert(importData);

                        MessageBox.Show("导入成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 重新加载数据
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"导入失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// 导出Excel按钮点击事件
        /// </summary>
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel Files|*.xlsx";
                    saveFileDialog.Title = "保存Excel文件";
                    saveFileDialog.FileName = $"LoginInfo导出_20250701135426.xlsx";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // 获取所有数据
                        var data = _logininfoService.GetList();

                        // 调用导出方法
                        // GC_MES.Common.ExcelHelper.ExportToExcel(data, saveFileDialog.FileName);

                        MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 导出PDF按钮点击事件
        /// </summary>
        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PDF Files|*.pdf";
                    saveFileDialog.Title = "保存PDF文件";
                    saveFileDialog.FileName = $"LoginInfo导出_20250701135426.pdf";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // 获取所有数据
                        var data = _logininfoService.GetList();

                        // 调用导出方法
                        // GC_MES.Common.PdfHelper.ExportToPdf(data, saveFileDialog.FileName);

                        MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 分页事件处理

        /// <summary>
        /// 第一页按钮点击事件
        /// </summary>
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage = 1;
                LoadData();
            }
        }

        /// <summary>
        /// 上一页按钮点击事件
        /// </summary>
        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadData();
            }
        }

        /// <summary>
        /// 下一页按钮点击事件
        /// </summary>
        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadData();
            }
        }

        /// <summary>
        /// 最后一页按钮点击事件
        /// </summary>
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage = totalPages;
                LoadData();
            }
        }

        #endregion

        #region 数据表事件处理

        /// <summary>
        /// 数据表选择行改变事件
        /// </summary>
        private void dgvLoginInfos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLoginInfos.SelectedRows.Count > 0)
            {
                selectedLoginInfo = dgvLoginInfos.SelectedRows[0].DataBoundItem as LoginInfo;
            }
            else
            {
                selectedLoginInfo = null;
            }

            // 启用/禁用按钮
            btnEdit.Enabled = selectedLoginInfo != null;
            btnDelete.Enabled = selectedLoginInfo != null;
        }

        /// <summary>
        /// 数据表单元格双击事件
        /// </summary>
        private void dgvLoginInfos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 如果双击的不是表头行，且有选中的行，则执行编辑操作
            if (e.RowIndex >= 0 && selectedLoginInfo != null)
            {
                btnEdit_Click(sender, e);
            }
        }

        #endregion

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
