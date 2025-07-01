using GC_MES.BLL.System.IService;
using GC_MES.Model.System;
using GC_MES.WinForm.Common;
using GC_MES.WinForm.Forms.SystemForm.SubForm;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GC_MES.WinForm.Forms.SystemForm
{
    public partial class RoleManagementForm : Form
    {
        #region 字段和属性

        private readonly ISys_RoleService _roleService;
        private readonly ISys_DeptService _deptService;

        // 分页参数
        private int _pageIndex = 1;
        private int _pageSize = 20;
        private int _totalCount = 0;
        private int _totalPages = 0;

        // 当前选中角色
        private Sys_Role _currentRole = null;

        // 查询参数
        private string _searchName = "";
        private int? _searchDeptId = null;

        #endregion

        #region 初始化和窗体加载

        public RoleManagementForm(ISys_RoleService roleService, ISys_DeptService deptService)
        {
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            _deptService = deptService ?? throw new ArgumentNullException(nameof(deptService));

            InitializeComponent();
            InitializeEvents();
           
        }

        private void InitializeEvents()
        {
            // 绑定事件处理器
            Load += RoleManagementForm_Load;
            
            // 按钮事件
            btnSearch.Click += BtnSearch_Click;
            btnClear.Click += BtnClear_Click;
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnImport.Click += BtnImport_Click;
            btnExport.Click += BtnExport_Click;
            btnExportPDF.Click += BtnExportPDF_Click;

            // 分页事件
            btnFirstPage.Click += BtnFirstPage_Click;
            btnPrevPage.Click += BtnPrevPage_Click;
            btnNextPage.Click += BtnNextPage_Click;
            btnLastPage.Click += BtnLastPage_Click;

            // 表格事件
            dgvRoles.SelectionChanged += DgvRoles_SelectionChanged;
            dgvRoles.CellDoubleClick += DgvRoles_CellDoubleClick;
        }

        private void RoleManagementForm_Load(object sender, EventArgs e)
        {
            try
            {
                // 加载部门下拉框
                LoadDepartments();

                // 加载角色数据
                LoadRoleData();
            }
            catch (Exception ex)
            {
                ShowError($"窗体加载失败: {ex.Message}");
            }
        }

        #endregion

        #region 数据加载和绑定

        /// <summary>
        /// 加载部门下拉框数据
        /// </summary>
        private void LoadDepartments()
        {
            try
            {
                var departments = _deptService.Query();

                // 添加"全部"选项
                List<Sys_Dept> deptList = new List<Sys_Dept>
                {
                    new Sys_Dept { Dept_Id = 0, DeptName = "全部" }
                };
                deptList.AddRange(departments);

                cmbDept.DataSource = deptList;
                cmbDept.DisplayMember = "DeptName";
                cmbDept.ValueMember = "Dept_Id";
                cmbDept.SelectedValue = 0;
            }
            catch (Exception ex)
            {
                ShowError($"加载部门数据失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 加载角色数据
        /// </summary>
        private void LoadRoleData()
        {
            try
            {
                // 构建查询表达式
                var expression = BuildQueryExpression();

                // 执行分页查询
                var result = _roleService.QueryPage(expression, pageIndex: _pageIndex, pageSize: _pageSize);

                // 更新总记录数和总页数
                _totalCount = result.TotalCount;
                _totalPages = result.TotalPages;

                // 绑定数据到DataGridView
                dgvRoles.DataSource = result;

              
            
                // 更新分页信息和按钮状态
                UpdatePagination();
            }
            catch (Exception ex)
            {
                ShowError($"加载角色数据失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 构建查询表达式
        /// </summary>
        private Expression<Func<Sys_Role, bool>> BuildQueryExpression()
        {
            return SqlSugar.Expressionable.Create<Sys_Role>()
                .AndIF(!string.IsNullOrEmpty(_searchName), r => r.RoleName.Contains(_searchName))
                .AndIF(_searchDeptId.HasValue && _searchDeptId.Value > 0, r => r.Dept_Id == _searchDeptId.Value)
                .ToExpression();
        }

        #endregion

       

        #region 分页逻辑

        /// <summary>
        /// 更新分页信息和按钮状态
        /// </summary>
        private void UpdatePagination()
        {
            lblPageInfo.Text = $"第 {_pageIndex} 页 / 共 {_totalPages} 页，总记录数：{_totalCount}";

            // 更新分页按钮状态
            btnFirstPage.Enabled = _pageIndex > 1;
            btnPrevPage.Enabled = _pageIndex > 1;
            btnNextPage.Enabled = _pageIndex < _totalPages;
            btnLastPage.Enabled = _pageIndex < _totalPages;

            // 更新按钮状态
            UpdateButtonStatus();
        }

        /// <summary>
        /// 更新按钮状态
        /// </summary>
        private void UpdateButtonStatus()
        {
            bool hasSelection = dgvRoles.SelectedRows.Count > 0;
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
        }

        /// <summary>
        /// 跳转到指定页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        private void GoToPage(int pageIndex)
        {
            if (pageIndex < 1 || pageIndex > _totalPages || pageIndex == _pageIndex)
                return;

            _pageIndex = pageIndex;
            LoadRoleData();
        }

        #endregion

        #region 事件处理 - 查询操作

        /// <summary>
        /// 搜索按钮事件
        /// </summary>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            _searchName = txtSearchName.Text.Trim();
            _searchDeptId = cmbDept.SelectedValue != null ? (int?)cmbDept.SelectedValue : null;

            // 重置到第一页
            _pageIndex = 1;

            // 重新加载数据
            LoadRoleData();
        }

        /// <summary>
        /// 清空按钮事件
        /// </summary>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtSearchName.Text = "";
            cmbDept.SelectedValue = 0;

            _searchName = "";
            _searchDeptId = null;

            // 重置到第一页
            _pageIndex = 1;

            // 重新加载数据
            LoadRoleData();
        }

        #endregion

        #region 事件处理 - 角色CRUD操作

        /// <summary>
        /// 添加按钮事件
        /// </summary>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // 暂时使用消息框代替
                ShowInformation("此功能尚未实现，请先完成RoleEditForm窗体");
                
                // 实际代码应该是：
                // using (var editForm = new RoleEditForm(_roleService))
                // {
                //     if (editForm.ShowDialog() == DialogResult.OK)
                //     {
                //         // 重新加载数据
                //         LoadRoleData();
                //     }
                // }
            }
            catch (Exception ex)
            {
                ShowError($"打开角色编辑窗体失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 编辑按钮事件
        /// </summary>
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (_currentRole == null)
            {
                ShowInformation("请先选择要编辑的角色");
                return;
            }

            try
            {
                // 暂时使用消息框代替
                ShowInformation($"此功能尚未实现，请先完成RoleEditForm窗体，选中的角色ID: {_currentRole.Role_Id}");
                
                // 实际代码应该是：
                // using (var editForm = new RoleEditForm(_roleService, _currentRole.Role_Id))
                // {
                //     if (editForm.ShowDialog() == DialogResult.OK)
                //     {
                //         // 重新加载数据
                //         LoadRoleData();
                //     }
                // }
            }
            catch (Exception ex)
            {
                ShowError($"打开角色编辑窗体失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 删除按钮事件
        /// </summary>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_currentRole == null)
            {
                ShowInformation("请先选择要删除的角色");
                return;
            }

            // 确认删除
            if (MessageBox.Show($"确认删除角色 \"{_currentRole.RoleName}\" 吗？", "确认删除",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    bool result = _roleService.Delete(_currentRole);
                    if (result)
                    {
                        ShowSuccess("删除成功！");

                        // 重新加载数据
                        LoadRoleData();
                    }
                    else
                    {
                        ShowError("删除失败！");
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"删除失败: {ex.Message}");
                }
            }
        }

        #endregion

        #region 事件处理 - 导入导出操作

        /// <summary>
        /// 导入按钮事件
        /// </summary>
        private void BtnImport_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Excel文件|*.xlsx;*.xls";
                    openFileDialog.Title = "选择Excel文件";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;
                        ImportRolesFromExcel(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"导入失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 从Excel导入角色数据
        /// </summary>
        private void ImportRolesFromExcel(string filePath)
        {
            try
            {
                // 使用ExcelHelper导入数据
                DataTable dt = ExcelHelper.ImportFromExcel(filePath);
                if (dt == null || dt.Rows.Count == 0)
                {
                    ShowInformation("没有有效的数据可导入");
                    return;
                }

                // 转换为角色对象列表
                List<Sys_Role> roles = new List<Sys_Role>();
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        Sys_Role role = new Sys_Role
                        {
                            RoleName = row["角色名称"]?.ToString(),
                            DeptName = row["所属部门"]?.ToString(),
                            OrderNo = Convert.ToInt32(row["排序"]?.ToString() ?? "0"),
                            Enable = Convert.ToByte(row["状态"]?.ToString() == "是" ? 1 : 0),
                            Creator = "导入",
                            CreateDate = DateTime.Now
                        };

                        // 根据部门名称查找部门ID
                        if (!string.IsNullOrEmpty(role.DeptName))
                        {
                            var dept = _deptService.Query().FirstOrDefault(d => d.DeptName == role.DeptName);
                            if (dept != null)
                            {
                                role.Dept_Id = dept.Dept_Id;
                            }
                        }

                        roles.Add(role);
                    }
                    catch (Exception ex)
                    {
                        ShowError($"数据格式错误: {ex.Message}");
                    }
                }

                // 批量插入角色
                if (roles.Count > 0)
                {
                    int successCount = 0;
                    foreach (var role in roles)
                    {
                        // 使用Insert方法而不是Add方法

                        successCount += _roleService.Insert(role);
                        
                    }

                    ShowSuccess($"导入完成，成功导入{successCount}条记录，失败{roles.Count - successCount}条");
                    LoadRoleData();
                }
                else
                {
                    ShowInformation("没有有效的数据可导入");
                }
            }
            catch (Exception ex)
            {
                ShowError($"导入Excel失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 导出Excel按钮事件
        /// </summary>
        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取所有角色数据
                var expression = BuildQueryExpression();
                var roles = _roleService.Query().Where(r => expression.Compile().Invoke(r)).ToList();

                if (roles == null || roles.Count == 0)
                {
                    ShowInformation("没有数据可导出");
                    return;
                }

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel文件|*.xlsx";
                    saveFileDialog.Title = "导出Excel";
                    saveFileDialog.FileName = $"角色数据_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // 定义列标题和属性名
                        string[] columnHeaders = { "角色名称", "所属部门", "排序", "状态", "创建人", "创建时间" };
                        string[] columnProperties = { "RoleName", "DeptName", "OrderNo", "Enable", "Creator", "CreateDate" };

                        // 使用ExcelHelper导出数据
                        bool result = ExcelHelper.ExportToExcel(roles, saveFileDialog.FileName, "角色数据", columnHeaders, columnProperties);
                        if (result)
                        {
                            ShowSuccess("导出成功！");
                        }
                        else
                        {
                            ShowError("导出失败！");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"导出失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 导出PDF按钮事件
        /// </summary>
        private void BtnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取所有角色数据
                var expression = BuildQueryExpression();
                var roles = _roleService.Query().Where(r => expression.Compile().Invoke(r)).ToList();

                if (roles == null || roles.Count == 0)
                {
                    ShowInformation("没有数据可导出");
                    return;
                }

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PDF文件|*.pdf";
                    saveFileDialog.Title = "导出PDF";
                    saveFileDialog.FileName = $"角色数据_{DateTime.Now:yyyyMMddHHmmss}.pdf";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // 定义列标题和属性名
                        string[] columnHeaders = { "角色名称", "所属部门", "排序", "状态", "创建人", "创建时间" };
                        string[] columnProperties = { "RoleName", "DeptName", "OrderNo", "Enable", "Creator", "CreateDate" };
                        
                        // 定义列宽比例
                        float[] columnWidths = { 2f, 2f, 1f, 1f, 1.5f, 2f };

                        // 使用PdfHelper导出数据
                        bool result = PdfHelper.ExportToPdf(roles, saveFileDialog.FileName, "角色管理数据", columnHeaders, columnProperties, columnWidths);
                        if (result)
                        {
                            ShowSuccess("导出成功！");
                        }
                        else
                        {
                            ShowError("导出失败！");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"导出失败: {ex.Message}");
            }
        }

        #endregion

        #region 事件处理 - 分页操作

        /// <summary>
        /// 首页按钮事件
        /// </summary>
        private void BtnFirstPage_Click(object sender, EventArgs e)
        {
            GoToPage(1);
        }

        /// <summary>
        /// 上一页按钮事件
        /// </summary>
        private void BtnPrevPage_Click(object sender, EventArgs e)
        {
            GoToPage(_pageIndex - 1);
        }

        /// <summary>
        /// 下一页按钮事件
        /// </summary>
        private void BtnNextPage_Click(object sender, EventArgs e)
        {
            GoToPage(_pageIndex + 1);
        }

        /// <summary>
        /// 末页按钮事件
        /// </summary>
        private void BtnLastPage_Click(object sender, EventArgs e)
        {
            GoToPage(_totalPages);
        }

        #endregion

        #region 事件处理 - 表格操作

        /// <summary>
        /// 表格选择变更事件
        /// </summary>
        private void DgvRoles_SelectionChanged(object sender, EventArgs e)
        {
            _currentRole = dgvRoles.SelectedRows.Count > 0 ? 
                dgvRoles.SelectedRows[0].DataBoundItem as Sys_Role : null;
            
            UpdateButtonStatus();
        }

        /// <summary>
        /// 表格双击事件
        /// </summary>
        private void DgvRoles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && _currentRole != null)
            {
                BtnEdit_Click(this, EventArgs.Empty);
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 显示错误消息
        /// </summary>
        private void ShowError(string message)
        {
            MessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 显示成功消息
        /// </summary>
        private void ShowSuccess(string message)
        {
            MessageBox.Show(message, "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示提示消息
        /// </summary>
        private void ShowInformation(string message)
        {
            MessageBox.Show(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion
    }
}
