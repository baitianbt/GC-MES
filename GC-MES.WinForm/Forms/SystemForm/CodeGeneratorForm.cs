using GC_MES.Model.System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GC_MES.WinForm.Common;

namespace GC_MES.WinForm.Forms.SystemForm
{
    public partial class CodeGeneratorForm : Form
    {
        // 存储所有实体类型
        private List<Type> entityTypes = new List<Type>();
        // 当前选中的实体类型
        private Type selectedEntityType = null;
        // 当前选中实体的属性列表
        private List<PropertyInfo> entityProperties = new List<PropertyInfo>();
        // 代码生成助手
        private CodeGeneratorHelper codeGeneratorHelper = new CodeGeneratorHelper();

        public CodeGeneratorForm()
        {
            InitializeComponent();
            LoadEntityTypes();
        }

        private void CodeGeneratorForm_Load(object sender, EventArgs e)
        {
            // 应用主题
            Common.ThemeManager.Instance.ApplyTheme(this);
        }

        /// <summary>
        /// 加载所有实体类型
        /// </summary>
        private void LoadEntityTypes()
        {
            try
            {
                // 加载GC-MES.Model程序集
                Assembly modelAssembly = Assembly.Load("GC-MES.Model");

                // 获取所有类型
                var allTypes = modelAssembly.GetTypes();

                // 过滤出实体类（非抽象类，非接口，非枚举）
                entityTypes = allTypes
                    .Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface && !t.IsEnum)
                    .ToList();

                // 绑定到实体列表
                lstEntities.DataSource = null;
                lstEntities.DisplayMember = "Name";
                lstEntities.DataSource = entityTypes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载实体类型失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 当选择实体类型时
        /// </summary>
        private void lstEntities_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstEntities.SelectedItem == null) return;

            selectedEntityType = lstEntities.SelectedItem as Type;
            if (selectedEntityType != null)
            {
                LoadEntityProperties(selectedEntityType);
                UpdateEntityInfo();
            }
        }

        /// <summary>
        /// 加载实体属性
        /// </summary>
        private void LoadEntityProperties(Type entityType)
        {
            entityProperties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .OrderBy(p => p.MetadataToken) // 保持原始顺序
                .ToList();

            // 绑定到属性DataGridView
            BindPropertiesToGrid();
        }

        /// <summary>
        /// 绑定属性到DataGridView
        /// </summary>
        private void BindPropertiesToGrid()
        {
            var propertyList = new List<EntityPropertyViewModel>();

            foreach (var prop in entityProperties)
            {
                var viewModel = new EntityPropertyViewModel
                {
                    PropertyName = prop.Name,
                    PropertyType = prop.PropertyType.Name,
                    IsKey = prop.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true).Any(),
                    DisplayName = GetDisplayName(prop),
                    IsRequired = IsRequired(prop),
                    MaxLength = GetMaxLength(prop),
                    IsEditable = IsEditable(prop)
                };

                propertyList.Add(viewModel);
            }

            dgvProperties.DataSource = propertyList;
        }

        /// <summary>
        /// 获取属性的Display特性的Name值
        /// </summary>
        private string GetDisplayName(PropertyInfo prop)
        {
            var displayAttr = prop.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), true)
                .FirstOrDefault() as System.ComponentModel.DataAnnotations.DisplayAttribute;
            return displayAttr?.Name ?? prop.Name;
        }

        /// <summary>
        /// 判断属性是否必填
        /// </summary>
        private bool IsRequired(PropertyInfo prop)
        {
            return prop.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), true).Any();
        }

        /// <summary>
        /// 获取属性的最大长度
        /// </summary>
        private int GetMaxLength(PropertyInfo prop)
        {
            var maxLengthAttr = prop.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.MaxLengthAttribute), true)
                .FirstOrDefault() as System.ComponentModel.DataAnnotations.MaxLengthAttribute;
            return maxLengthAttr?.Length ?? 0;
        }

        /// <summary>
        /// 判断属性是否可编辑
        /// </summary>
        private bool IsEditable(PropertyInfo prop)
        {
            var editableAttr = prop.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.EditableAttribute), true)
                .FirstOrDefault() as System.ComponentModel.DataAnnotations.EditableAttribute;
            return editableAttr?.AllowEdit ?? true;
        }

        /// <summary>
        /// 更新实体信息
        /// </summary>
        private void UpdateEntityInfo()
        {
            if (selectedEntityType == null) return;

            txtEntityName.Text = selectedEntityType.Name;
            txtNamespace.Text = selectedEntityType.Namespace;

            // 尝试获取Table特性
            var tableAttr = selectedEntityType.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.Schema.TableAttribute), true)
                .FirstOrDefault() as System.ComponentModel.DataAnnotations.Schema.TableAttribute;
            txtTableName.Text = tableAttr?.Name ?? selectedEntityType.Name;
        }
        private void btnGenerateRepository_Click(object sender, EventArgs e)
        {
            if (selectedEntityType == null)
            {
                MessageBox.Show("请先选择一个实体类", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string entityName = selectedEntityType.Name;
                string namespaceName = selectedEntityType.Namespace;

                // 选择输出目录
                using (var folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "选择Repository输出目录";
                    folderDialog.ShowNewFolderButton = true;

                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string outputDir = folderDialog.SelectedPath;

                        // 生成Repository代码
                        string code = GenerateRepository_Click();
                        string filePath = Path.Combine(outputDir, $"{entityName}Repository.cs");
                        File.WriteAllText(filePath, code);

                        MessageBox.Show($"{entityName}Repository已生成到：\n{filePath}", "生成成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成Repository失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 生成Repository
        /// </summary>
        private string GenerateRepository_Click()
        {
            if (selectedEntityType == null) return string.Empty;

            StringBuilder code = new StringBuilder();
            string entityName = selectedEntityType.Name;
            string namespaceName = selectedEntityType.Namespace;

            // 添加命名空间
            code.AppendLine("using GC_MES.DAL;");
            code.AppendLine($"using {namespaceName};");
            code.AppendLine("using System;");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine("using System.Linq;");
            code.AppendLine("using System.Text;");
            code.AppendLine("using System.Threading.Tasks;");
            code.AppendLine();

            // 添加命名空间声明
            code.AppendLine("namespace GC_MES.DAL.System.Repository");
            code.AppendLine("{");

            // 添加类声明
            code.AppendLine($"    public class {entityName}Repository : BaseRepository<{entityName}>, I{entityName}Repository");
            code.AppendLine("    {");

            // 添加构造函数
            code.AppendLine($"        public {entityName}Repository(UnitOfWork.IUnitOfWork unitOfWork) : base(unitOfWork)");
            code.AppendLine("        {");
            code.AppendLine("        }");

            // 添加自定义方法
            code.AppendLine();
            code.AppendLine("        // 在这里添加自定义的Repository方法");
            code.AppendLine();

            // 结束类定义
            code.AppendLine("    }");
            code.AppendLine("}");

            return code.ToString();
        }


        /// <summary>
        /// 生成IRepository
        /// </summary>
        private void btnGenerateIRepository_Click(object sender, EventArgs e)
        {
            if (selectedEntityType == null)
            {
                MessageBox.Show("请先选择一个实体类", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string entityName = selectedEntityType.Name;
                string namespaceName = selectedEntityType.Namespace;

                // 选择输出目录
                using (var folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "选择IRepository输出目录";
                    folderDialog.ShowNewFolderButton = true;

                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string outputDir = folderDialog.SelectedPath;

                        // 生成IRepository代码
                        StringBuilder code = new StringBuilder();

                        // 添加命名空间
                        code.AppendLine($"using {namespaceName};");
                        code.AppendLine("using GC_MES.DAL;");
                        code.AppendLine("using System;");
                        code.AppendLine("using System.Collections.Generic;");
                        code.AppendLine("using System.Linq;");
                        code.AppendLine("using System.Text;");
                        code.AppendLine("using System.Threading.Tasks;");
                        code.AppendLine();

                        // 添加命名空间声明
                        code.AppendLine("namespace GC_MES.DAL.System.IRepository");
                        code.AppendLine("{");

                        // 添加接口声明
                        code.AppendLine($"    public interface I{entityName}Repository : IBaseRepository<{entityName}>");
                        code.AppendLine("    {");

                        // 添加自定义方法
                        code.AppendLine("        // 在这里添加自定义的Repository接口方法");
                        code.AppendLine();

                        // 结束接口定义
                        code.AppendLine("    }");
                        code.AppendLine("}");

                        string filePath = Path.Combine(outputDir, $"I{entityName}Repository.cs");
                        File.WriteAllText(filePath, code.ToString());

                        MessageBox.Show($"I{entityName}Repository已生成到：\n{filePath}", "生成成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成IRepository失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 生成Service
        /// </summary>
        private void btnGenerateService_Click(object sender, EventArgs e)
        {
            if (selectedEntityType == null)
            {
                MessageBox.Show("请先选择一个实体类", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string entityName = selectedEntityType.Name;
                string namespaceName = selectedEntityType.Namespace;

                // 选择输出目录
                using (var folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "选择Service输出目录";
                    folderDialog.ShowNewFolderButton = true;

                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string outputDir = folderDialog.SelectedPath;

                        // 生成Service代码
                        StringBuilder code = new StringBuilder();

                        // 添加命名空间
                        code.AppendLine($"using {namespaceName};");
                        code.AppendLine("using GC_MES.BLL;");
                        code.AppendLine("using GC_MES.DAL;");
                        code.AppendLine("using GC_MES.DAL.System.IRepository;");
                        code.AppendLine("using System;");
                        code.AppendLine("using System.Collections.Generic;");
                        code.AppendLine("using System.Linq;");
                        code.AppendLine("using System.Text;");
                        code.AppendLine("using System.Threading.Tasks;");
                        code.AppendLine();

                        // 添加命名空间声明
                        code.AppendLine("namespace GC_MES.BLL.System.Service");
                        code.AppendLine("{");

                        // 添加类声明
                        code.AppendLine($"    public class {entityName}Service : BaseServices<{entityName}>, I{entityName}Service");
                        code.AppendLine("    {");

                        // 添加私有字段
                        code.AppendLine($"        private readonly I{entityName}Repository _{entityName.ToLower()}Repository;");

                        // 添加构造函数
                        code.AppendLine();
                        code.AppendLine($"        public {entityName}Service(I{entityName}Repository {entityName.ToLower()}Repository)");
                        code.AppendLine("        {");
                        code.AppendLine($"            this.BaseDal = {entityName.ToLower()}Repository;");
                        code.AppendLine($"            _{entityName.ToLower()}Repository = {entityName.ToLower()}Repository;");
                        code.AppendLine("        }");

                        // 添加自定义方法
                        code.AppendLine();
                        code.AppendLine("        // 在这里添加自定义的Service方法");
                        code.AppendLine();

                        // 结束类定义
                        code.AppendLine("    }");
                        code.AppendLine("}");

                        string filePath = Path.Combine(outputDir, $"{entityName}Service.cs");
                        File.WriteAllText(filePath, code.ToString());

                        MessageBox.Show($"{entityName}Service已生成到：\n{filePath}", "生成成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成Service失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 生成IService
        /// </summary>
        private void btnGenerateIService_Click(object sender, EventArgs e)
        {
            if (selectedEntityType == null)
            {
                MessageBox.Show("请先选择一个实体类", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string entityName = selectedEntityType.Name;
                string namespaceName = selectedEntityType.Namespace;

                // 选择输出目录
                using (var folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "选择IService输出目录";
                    folderDialog.ShowNewFolderButton = true;

                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string outputDir = folderDialog.SelectedPath;

                        // 生成IService代码
                        StringBuilder code = new StringBuilder();

                        // 添加命名空间
                        code.AppendLine($"using {namespaceName};");
                        code.AppendLine("using GC_MES.BLL;");
                        code.AppendLine("using System;");
                        code.AppendLine("using System.Collections.Generic;");
                        code.AppendLine("using System.Linq;");
                        code.AppendLine("using System.Text;");
                        code.AppendLine("using System.Threading.Tasks;");
                        code.AppendLine();

                        // 添加命名空间声明
                        code.AppendLine("namespace GC_MES.BLL.System.IService");
                        code.AppendLine("{");

                        // 添加接口声明
                        code.AppendLine($"    public interface I{entityName}Service : IBaseServices<{entityName}>");
                        code.AppendLine("    {");

                        // 添加自定义方法
                        code.AppendLine("        // 在这里添加自定义的Service接口方法");
                        code.AppendLine();

                        // 结束接口定义
                        code.AppendLine("    }");
                        code.AppendLine("}");

                        string filePath = Path.Combine(outputDir, $"I{entityName}Service.cs");
                        File.WriteAllText(filePath, code.ToString());

                        MessageBox.Show($"I{entityName}Service已生成到：\n{filePath}", "生成成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成IService失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 生成窗体
        /// </summary>
        private void btnGenerateForm_Click(object sender, EventArgs e)
        {
            if (selectedEntityType == null)
            {
                MessageBox.Show("请先选择一个实体类", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string entityName = selectedEntityType.Name;
                string namespaceName = selectedEntityType.Namespace;

                // 生成窗体代码
                StringBuilder code = new StringBuilder();

                // 添加命名空间声明
                code.AppendLine("namespace GC_MES.WinForm.Forms.SystemForm");
                code.AppendLine("{");

                // 添加类声明
                code.AppendLine($"    partial class {entityName}ManagementForm");
                code.AppendLine("    {");
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 必需的设计器变量");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private System.ComponentModel.IContainer components = null;");
                code.AppendLine();
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 清理所有正在使用的资源");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        /// <param name=\"disposing\">如果应释放托管资源，为 true；否则为 false</param>");
                code.AppendLine("        protected override void Dispose(bool disposing)");
                code.AppendLine("        {");
                code.AppendLine("            if (disposing && (components != null))");
                code.AppendLine("            {");
                code.AppendLine("                components.Dispose();");
                code.AppendLine("            }");
                code.AppendLine("            base.Dispose(disposing);");
                code.AppendLine("        }");
                code.AppendLine();
                code.AppendLine("        #region Windows 窗体设计器生成的代码");
                code.AppendLine();
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 设计器支持所需的方法 - 不要修改");
                code.AppendLine("        /// 此方法的内容。");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void InitializeComponent()");
                code.AppendLine("        {");

                // 声明控件
                code.AppendLine("            this.pnlSearch = new System.Windows.Forms.Panel();");
                code.AppendLine("            this.btnClear = new System.Windows.Forms.Button();");
                code.AppendLine("            this.btnSearch = new System.Windows.Forms.Button();");

                // 动态生成查询条件控件声明
                int searchFieldCount = Math.Min(entityProperties.Count, 3); // 最多显示3个查询字段
                for (int i = 0; i < searchFieldCount; i++)
                {
                    var prop = entityProperties[i];
                    string controlName = $"txt{prop.Name}";

                    if (prop.PropertyType == typeof(bool))
                    {
                        code.AppendLine($"            this.chk{prop.Name} = new System.Windows.Forms.CheckBox();");
                    }
                    else if (prop.PropertyType.IsEnum || prop.Name.EndsWith("Id"))
                    {
                        code.AppendLine($"            this.cmb{prop.Name} = new System.Windows.Forms.ComboBox();");
                    }
                    else
                    {
                        code.AppendLine($"            this.txt{prop.Name} = new System.Windows.Forms.TextBox();");
                    }

                    code.AppendLine($"            this.lbl{prop.Name} = new System.Windows.Forms.Label();");
                }

                code.AppendLine("            this.pnlToolbar = new System.Windows.Forms.Panel();");
                code.AppendLine("            this.btnExportPDF = new System.Windows.Forms.Button();");
                code.AppendLine("            this.btnExport = new System.Windows.Forms.Button();");
                code.AppendLine("            this.btnImport = new System.Windows.Forms.Button();");
                code.AppendLine("            this.btnDelete = new System.Windows.Forms.Button();");
                code.AppendLine("            this.btnEdit = new System.Windows.Forms.Button();");
                code.AppendLine("            this.btnAdd = new System.Windows.Forms.Button();");
                code.AppendLine($"            this.dgv{entityName}s = new System.Windows.Forms.DataGridView();");

                // 动态生成DataGridView列声明
                foreach (var prop in entityProperties)
                {
                    string columnName = $"col{prop.Name}";
                    if (prop.PropertyType == typeof(bool))
                    {
                        code.AppendLine($"            this.{columnName} = new System.Windows.Forms.DataGridViewCheckBoxColumn();");
                    }
                    else
                    {
                        code.AppendLine($"            this.{columnName} = new System.Windows.Forms.DataGridViewTextBoxColumn();");
                    }
                }

                code.AppendLine("            this.pnlPager = new System.Windows.Forms.Panel();");
                code.AppendLine("            this.lblPageInfo = new System.Windows.Forms.Label();");
                code.AppendLine("            this.btnLastPage = new System.Windows.Forms.Button();");
                code.AppendLine("            this.btnNextPage = new System.Windows.Forms.Button();");
                code.AppendLine("            this.btnPrevPage = new System.Windows.Forms.Button();");
                code.AppendLine("            this.btnFirstPage = new System.Windows.Forms.Button();");
                code.AppendLine("            this.pnlSearch.SuspendLayout();");
                code.AppendLine("            this.pnlToolbar.SuspendLayout();");
                code.AppendLine($"            ((System.ComponentModel.ISupportInitialize)(this.dgv{entityName}s)).BeginInit();");
                code.AppendLine("            this.pnlPager.SuspendLayout();");
                code.AppendLine("            this.SuspendLayout();");
                code.AppendLine("            // ");
                code.AppendLine("            // pnlSearch");
                code.AppendLine("            // ");
                code.AppendLine("            this.pnlSearch.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);");
                code.AppendLine("            this.pnlSearch.Controls.Add(this.btnClear);");
                code.AppendLine("            this.pnlSearch.Controls.Add(this.btnSearch);");

                // 动态添加查询控件到pnlSearch
                for (int i = 0; i < searchFieldCount; i++)
                {
                    var prop = entityProperties[i];
                    string controlName;

                    if (prop.PropertyType == typeof(bool))
                    {
                        controlName = $"chk{prop.Name}";
                    }
                    else if (prop.PropertyType.IsEnum || prop.Name.EndsWith("Id"))
                    {
                        controlName = $"cmb{prop.Name}";
                    }
                    else
                    {
                        controlName = $"txt{prop.Name}";
                    }

                    code.AppendLine($"            this.pnlSearch.Controls.Add(this.{controlName});");
                    code.AppendLine($"            this.pnlSearch.Controls.Add(this.lbl{prop.Name});");
                }

                code.AppendLine("            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;");
                code.AppendLine("            this.pnlSearch.Location = new System.Drawing.Point(10, 10);");
                code.AppendLine("            this.pnlSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                code.AppendLine("            this.pnlSearch.Name = \"pnlSearch\";");
                code.AppendLine("            this.pnlSearch.Size = new System.Drawing.Size(1473, 113);");
                code.AppendLine("            this.pnlSearch.TabIndex = 1;");
                code.AppendLine("            // ");
                code.AppendLine("            // btnClear");
                code.AppendLine("            // ");
                code.AppendLine("            this.btnClear.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);");
                code.AppendLine("            this.btnClear.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(160, 160, 160);");
                code.AppendLine("            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;");
                code.AppendLine("            this.btnClear.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
                code.AppendLine("            this.btnClear.Location = new System.Drawing.Point(688, 42);");
                code.AppendLine("            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                code.AppendLine("            this.btnClear.Name = \"btnClear\";");
                code.AppendLine("            this.btnClear.Size = new System.Drawing.Size(88, 40);");
                code.AppendLine("            this.btnClear.TabIndex = 5;");
                code.AppendLine("            this.btnClear.Text = \"清空\";");
                code.AppendLine("            this.btnClear.UseVisualStyleBackColor = false;");
                code.AppendLine("            // ");
                code.AppendLine("            // btnSearch");
                code.AppendLine("            // ");
                code.AppendLine("            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);");
                code.AppendLine("            this.btnSearch.FlatAppearance.BorderSize = 0;");
                code.AppendLine("            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;");
                code.AppendLine("            this.btnSearch.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
                code.AppendLine("            this.btnSearch.ForeColor = System.Drawing.Color.White;");
                code.AppendLine("            this.btnSearch.Location = new System.Drawing.Point(583, 42);");
                code.AppendLine("            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                code.AppendLine("            this.btnSearch.Name = \"btnSearch\";");
                code.AppendLine("            this.btnSearch.Size = new System.Drawing.Size(88, 40);");
                code.AppendLine("            this.btnSearch.TabIndex = 4;");
                code.AppendLine("            this.btnSearch.Text = \"搜索\";");
                code.AppendLine("            this.btnSearch.UseVisualStyleBackColor = false;");

                // 动态生成查询控件的属性设置
                int labelX = 23;
                int controlX = 128;
                for (int i = 0; i < searchFieldCount; i++)
                {
                    var prop = entityProperties[i];
                    string displayName = GetDisplayName(prop);

                    // 添加标签
                    code.AppendLine("            // ");
                    code.AppendLine($"            // lbl{prop.Name}");
                    code.AppendLine("            // ");
                    code.AppendLine($"            this.lbl{prop.Name}.AutoSize = true;");
                    code.AppendLine($"            this.lbl{prop.Name}.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
                    code.AppendLine($"            this.lbl{prop.Name}.Location = new System.Drawing.Point({labelX}, {42 + i * 30});");
                    code.AppendLine($"            this.lbl{prop.Name}.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);");
                    code.AppendLine($"            this.lbl{prop.Name}.Name = \"lbl{prop.Name}\";");
                    code.AppendLine($"            this.lbl{prop.Name}.Size = new System.Drawing.Size({displayName.Length * 10}, 17);");
                    code.AppendLine($"            this.lbl{prop.Name}.TabIndex = {i * 2};");
                    code.AppendLine($"            this.lbl{prop.Name}.Text = \"{displayName}：\";");

                    // 添加控件
                    if (prop.PropertyType == typeof(bool))
                    {
                        code.AppendLine("            // ");
                        code.AppendLine($"            // chk{prop.Name}");
                        code.AppendLine("            // ");
                        code.AppendLine($"            this.chk{prop.Name}.AutoSize = true;");
                        code.AppendLine($"            this.chk{prop.Name}.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
                        code.AppendLine($"            this.chk{prop.Name}.Location = new System.Drawing.Point({controlX}, {42 + i * 30});");
                        code.AppendLine($"            this.chk{prop.Name}.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                        code.AppendLine($"            this.chk{prop.Name}.Name = \"chk{prop.Name}\";");
                        code.AppendLine($"            this.chk{prop.Name}.Size = new System.Drawing.Size(60, 21);");
                        code.AppendLine($"            this.chk{prop.Name}.TabIndex = {i * 2 + 1};");
                        code.AppendLine($"            this.chk{prop.Name}.Text = \"\";");
                        code.AppendLine($"            this.chk{prop.Name}.UseVisualStyleBackColor = true;");
                    }
                    else if (prop.PropertyType.IsEnum || prop.Name.EndsWith("Id"))
                    {
                        code.AppendLine("            // ");
                        code.AppendLine($"            // cmb{prop.Name}");
                        code.AppendLine("            // ");
                        code.AppendLine($"            this.cmb{prop.Name}.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;");
                        code.AppendLine($"            this.cmb{prop.Name}.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
                        code.AppendLine($"            this.cmb{prop.Name}.FormattingEnabled = true;");
                        code.AppendLine($"            this.cmb{prop.Name}.Location = new System.Drawing.Point({controlX}, {42 + i * 30});");
                        code.AppendLine($"            this.cmb{prop.Name}.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                        code.AppendLine($"            this.cmb{prop.Name}.Name = \"cmb{prop.Name}\";");
                        code.AppendLine($"            this.cmb{prop.Name}.Size = new System.Drawing.Size(174, 25);");
                        code.AppendLine($"            this.cmb{prop.Name}.TabIndex = {i * 2 + 1};");
                    }
                    else
                    {
                        code.AppendLine("            // ");
                        code.AppendLine($"            // txt{prop.Name}");
                        code.AppendLine("            // ");
                        code.AppendLine($"            this.txt{prop.Name}.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;");
                        code.AppendLine($"            this.txt{prop.Name}.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
                        code.AppendLine($"            this.txt{prop.Name}.Location = new System.Drawing.Point({controlX}, {42 + i * 30});");
                        code.AppendLine($"            this.txt{prop.Name}.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                        code.AppendLine($"            this.txt{prop.Name}.Name = \"txt{prop.Name}\";");
                        code.AppendLine($"            this.txt{prop.Name}.Size = new System.Drawing.Size(175, 23);");
                        code.AppendLine($"            this.txt{prop.Name}.TabIndex = {i * 2 + 1};");
                    }

                    // 更新下一组控件的位置
                    if (i % 2 == 0)
                    {
                        labelX = 327;
                        controlX = 373;
                    }
                    else
                    {
                        labelX = 23;
                        controlX = 128;
                    }
                }

                // 工具栏面板
                code.AppendLine("            // ");
                code.AppendLine("            // pnlToolbar");
                code.AppendLine("            // ");
                code.AppendLine("            this.pnlToolbar.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);");
                code.AppendLine("            this.pnlToolbar.Controls.Add(this.btnExportPDF);");
                code.AppendLine("            this.pnlToolbar.Controls.Add(this.btnExport);");
                code.AppendLine("            this.pnlToolbar.Controls.Add(this.btnImport);");
                code.AppendLine("            this.pnlToolbar.Controls.Add(this.btnDelete);");
                code.AppendLine("            this.pnlToolbar.Controls.Add(this.btnEdit);");
                code.AppendLine("            this.pnlToolbar.Controls.Add(this.btnAdd);");
                code.AppendLine("            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;");
                code.AppendLine("            this.pnlToolbar.Location = new System.Drawing.Point(10, 123);");
                code.AppendLine("            this.pnlToolbar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                code.AppendLine("            this.pnlToolbar.Name = \"pnlToolbar\";");
                code.AppendLine("            this.pnlToolbar.Size = new System.Drawing.Size(1473, 71);");
                code.AppendLine("            this.pnlToolbar.TabIndex = 2;");

                // 按钮配置
                code.AppendLine("            // ");
                code.AppendLine("            // btnExportPDF");
                code.AppendLine("            // ");
                code.AppendLine("            this.btnExportPDF.BackColor = System.Drawing.Color.FromArgb(80, 80, 85);");
                code.AppendLine("            this.btnExportPDF.FlatAppearance.BorderSize = 0;");
                code.AppendLine("            this.btnExportPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;");
                code.AppendLine("            this.btnExportPDF.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
                code.AppendLine("            this.btnExportPDF.ForeColor = System.Drawing.Color.White;");
                code.AppendLine("            this.btnExportPDF.Location = new System.Drawing.Point(595, 17);");
                code.AppendLine("            this.btnExportPDF.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                code.AppendLine("            this.btnExportPDF.Name = \"btnExportPDF\";");
                code.AppendLine("            this.btnExportPDF.Size = new System.Drawing.Size(105, 40);");
                code.AppendLine("            this.btnExportPDF.TabIndex = 5;");
                code.AppendLine("            this.btnExportPDF.Text = \"导出PDF\";");
                code.AppendLine("            this.btnExportPDF.UseVisualStyleBackColor = false;");
                code.AppendLine("            // ");
                code.AppendLine("            // btnExport");
                code.AppendLine("            // ");
                code.AppendLine("            this.btnExport.BackColor = System.Drawing.Color.FromArgb(80, 80, 85);");
                code.AppendLine("            this.btnExport.FlatAppearance.BorderSize = 0;");
                code.AppendLine("            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;");
                code.AppendLine("            this.btnExport.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
                code.AppendLine("            this.btnExport.ForeColor = System.Drawing.Color.White;");
                code.AppendLine("            this.btnExport.Location = new System.Drawing.Point(478, 17);");
                code.AppendLine("            this.btnExport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                code.AppendLine("            this.btnExport.Name = \"btnExport\";");
                code.AppendLine("            this.btnExport.Size = new System.Drawing.Size(105, 40);");
                code.AppendLine("            this.btnExport.TabIndex = 4;");
                code.AppendLine("            this.btnExport.Text = \"导出Excel\";");
                code.AppendLine("            this.btnExport.UseVisualStyleBackColor = false;");
                code.AppendLine("            // ");
                code.AppendLine("            // btnImport");
                code.AppendLine("            // ");
                code.AppendLine("            this.btnImport.BackColor = System.Drawing.Color.FromArgb(80, 80, 85);");
                code.AppendLine("            this.btnImport.FlatAppearance.BorderSize = 0;");
                code.AppendLine("            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;");
                code.AppendLine("            this.btnImport.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
                code.AppendLine("            this.btnImport.ForeColor = System.Drawing.Color.White;");
                code.AppendLine("            this.btnImport.Location = new System.Drawing.Point(362, 17);");
                code.AppendLine("            this.btnImport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                code.AppendLine("            this.btnImport.Name = \"btnImport\";");
                code.AppendLine("            this.btnImport.Size = new System.Drawing.Size(105, 40);");
                code.AppendLine("            this.btnImport.TabIndex = 3;");
                code.AppendLine("            this.btnImport.Text = \"导入Excel\";");
                code.AppendLine("            this.btnImport.UseVisualStyleBackColor = false;");
                code.AppendLine("            // ");
                code.AppendLine("            // btnDelete");
                code.AppendLine("            // ");
                code.AppendLine("            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(159, 68, 74);");
                code.AppendLine("            this.btnDelete.FlatAppearance.BorderSize = 0;");
                code.AppendLine("            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;");
                code.AppendLine("            this.btnDelete.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
                code.AppendLine("            this.btnDelete.ForeColor = System.Drawing.Color.White;");
                code.AppendLine("            this.btnDelete.Location = new System.Drawing.Point(245, 17);");
                code.AppendLine("            this.btnDelete.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                code.AppendLine("            this.btnDelete.Name = \"btnDelete\";");
                code.AppendLine("            this.btnDelete.Size = new System.Drawing.Size(105, 40);");
                code.AppendLine("            this.btnDelete.TabIndex = 2;");
                code.AppendLine("            this.btnDelete.Text = \"删除\";");
                code.AppendLine("            this.btnDelete.UseVisualStyleBackColor = false;");
                code.AppendLine("            // ");
                code.AppendLine("            // btnEdit");
                code.AppendLine("            // ");
                code.AppendLine("            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(67, 67, 70);");
                code.AppendLine("            this.btnEdit.FlatAppearance.BorderSize = 0;");
                code.AppendLine("            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;");
                code.AppendLine("            this.btnEdit.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
                code.AppendLine("            this.btnEdit.ForeColor = System.Drawing.Color.White;");
                code.AppendLine("            this.btnEdit.Location = new System.Drawing.Point(128, 17);");
                code.AppendLine("            this.btnEdit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                code.AppendLine("            this.btnEdit.Name = \"btnEdit\";");
                code.AppendLine("            this.btnEdit.Size = new System.Drawing.Size(105, 40);");
                code.AppendLine("            this.btnEdit.TabIndex = 1;");
                code.AppendLine("            this.btnEdit.Text = \"编辑\";");
                code.AppendLine("            this.btnEdit.UseVisualStyleBackColor = false;");
                code.AppendLine("            // ");
                code.AppendLine("            // btnAdd");
                code.AppendLine("            // ");
                code.AppendLine("            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);");
                code.AppendLine("            this.btnAdd.FlatAppearance.BorderSize = 0;");
                code.AppendLine("            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;");
                code.AppendLine("            this.btnAdd.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
                code.AppendLine("            this.btnAdd.ForeColor = System.Drawing.Color.White;");
                code.AppendLine("            this.btnAdd.Location = new System.Drawing.Point(12, 17);");
                code.AppendLine("            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                code.AppendLine("            this.btnAdd.Name = \"btnAdd\";");
                code.AppendLine("            this.btnAdd.Size = new System.Drawing.Size(105, 40);");
                code.AppendLine("            this.btnAdd.TabIndex = 0;");
                code.AppendLine("            this.btnAdd.Text = \"新增\";");
                code.AppendLine("            this.btnAdd.UseVisualStyleBackColor = false;");

                // DataGridView
                code.AppendLine("            // ");
                code.AppendLine($"            // dgv{entityName}s");
                code.AppendLine("            // ");
                code.AppendLine($"            this.dgv{entityName}s.AllowUserToAddRows = false;");
                code.AppendLine($"            this.dgv{entityName}s.AllowUserToDeleteRows = false;");
                code.AppendLine($"            this.dgv{entityName}s.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;");
                code.AppendLine($"            this.dgv{entityName}s.BackgroundColor = System.Drawing.Color.White;");
                code.AppendLine($"            this.dgv{entityName}s.BorderStyle = System.Windows.Forms.BorderStyle.None;");
                code.AppendLine($"            this.dgv{entityName}s.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;");

                // 添加列
                code.Append($"            this.dgv{entityName}s.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {{ ");
                for (int i = 0; i < entityProperties.Count; i++)
                {
                    code.Append($"this.col{entityProperties[i].Name}");
                    if (i < entityProperties.Count - 1) code.Append(", ");
                }
                code.AppendLine(" });");

                code.AppendLine($"            this.dgv{entityName}s.Dock = System.Windows.Forms.DockStyle.Fill;");
                code.AppendLine($"            this.dgv{entityName}s.Location = new System.Drawing.Point(10, 194);");
                code.AppendLine($"            this.dgv{entityName}s.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                code.AppendLine($"            this.dgv{entityName}s.MultiSelect = false;");
                code.AppendLine($"            this.dgv{entityName}s.Name = \"dgv{entityName}s\";");
                code.AppendLine($"            this.dgv{entityName}s.ReadOnly = true;");
                code.AppendLine($"            this.dgv{entityName}s.RowHeadersVisible = false;");
                code.AppendLine($"            this.dgv{entityName}s.RowTemplate.Height = 30;");
                code.AppendLine($"            this.dgv{entityName}s.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;");
                code.AppendLine($"            this.dgv{entityName}s.Size = new System.Drawing.Size(1473, 717);");
                code.AppendLine($"            this.dgv{entityName}s.TabIndex = 3;");
                code.AppendLine("");
                code.AppendLine("            // 设置列头样式");
                code.AppendLine($"            this.dgv{entityName}s.EnableHeadersVisualStyles = false;");
                code.AppendLine($"            this.dgv{entityName}s.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);");
                code.AppendLine($"            this.dgv{entityName}s.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;");
                code.AppendLine($"            this.dgv{entityName}s.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 9F, System.Drawing.FontStyle.Regular);");
                code.AppendLine($"            this.dgv{entityName}s.ColumnHeadersHeight = 40;");
                code.AppendLine("");
                code.AppendLine("            // 设置行样式");
                code.AppendLine($"            this.dgv{entityName}s.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;");
                code.AppendLine($"            this.dgv{entityName}s.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);");
                code.AppendLine($"            this.dgv{entityName}s.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(67, 67, 70);");
                code.AppendLine($"            this.dgv{entityName}s.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;");
                code.AppendLine($"            this.dgv{entityName}s.RowTemplate.Height = 36;");
                code.AppendLine("");
                code.AppendLine("            // 设置边框和网格线");
                code.AppendLine($"            this.dgv{entityName}s.BorderStyle = System.Windows.Forms.BorderStyle.None;");
                code.AppendLine($"            this.dgv{entityName}s.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;");
                code.AppendLine($"            this.dgv{entityName}s.GridColor = System.Drawing.Color.FromArgb(230, 230, 230);");
                code.AppendLine("");

                // 列配置
                foreach (var prop in entityProperties)
                {
                    string columnName = $"col{prop.Name}";
                    string displayName = GetDisplayName(prop);

                    code.AppendLine("            // ");
                    code.AppendLine($"            // {columnName}");
                    code.AppendLine("            // ");
                    code.AppendLine($"            this.{columnName}.DataPropertyName = \"{prop.Name}\";");
                    code.AppendLine($"            this.{columnName}.HeaderText = \"{displayName}\";");
                    code.AppendLine($"            this.{columnName}.Name = \"{columnName}\";");
                    code.AppendLine($"            this.{columnName}.ReadOnly = true;");

                    // 如果是主键，默认隐藏
                    if (prop.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true).Any())
                    {
                        code.AppendLine($"            this.{columnName}.Visible = false;");
                    }
                }

                // 分页面板
                code.AppendLine("            // ");
                code.AppendLine("            // pnlPager");
                code.AppendLine("            // ");
                code.AppendLine("            this.pnlPager.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);");
                code.AppendLine("            this.pnlPager.Controls.Add(this.lblPageInfo);");
                code.AppendLine("            this.pnlPager.Controls.Add(this.btnLastPage);");
                code.AppendLine("            this.pnlPager.Controls.Add(this.btnNextPage);");
                code.AppendLine("            this.pnlPager.Controls.Add(this.btnPrevPage);");
                code.AppendLine("            this.pnlPager.Controls.Add(this.btnFirstPage);");
                code.AppendLine("            this.pnlPager.Dock = System.Windows.Forms.DockStyle.Bottom;");
                code.AppendLine("            this.pnlPager.Location = new System.Drawing.Point(10, 911);");
                code.AppendLine("            this.pnlPager.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                code.AppendLine("            this.pnlPager.Name = \"pnlPager\";");
                code.AppendLine("            this.pnlPager.Size = new System.Drawing.Size(1473, 71);");
                code.AppendLine("            this.pnlPager.TabIndex = 4;");
                code.AppendLine("            // ");
                code.AppendLine("            // lblPageInfo");
                code.AppendLine("            // ");
                code.AppendLine("            this.lblPageInfo.AutoSize = true;");
                code.AppendLine("            this.lblPageInfo.AutoSize = true;");
                code.AppendLine("            this.lblPageInfo.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
                code.AppendLine("            this.lblPageInfo.Location = new System.Drawing.Point(233, 24);");
                code.AppendLine("            this.lblPageInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);");
                code.AppendLine("            this.lblPageInfo.Name = \"lblPageInfo\";");
                code.AppendLine("            this.lblPageInfo.Size = new System.Drawing.Size(110, 17);");
                code.AppendLine("            this.lblPageInfo.TabIndex = 4;");
                code.AppendLine("            this.lblPageInfo.Text = \"第 1/1 页，共 0 条\";");
                code.AppendLine("            // ");
                code.AppendLine("            // btnLastPage");
                code.AppendLine("            // ");
                code.AppendLine("            this.btnLastPage.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);");
                code.AppendLine("            this.btnLastPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(210, 210, 210);");
                code.AppendLine("            this.btnLastPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;");
                code.AppendLine("            this.btnLastPage.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
                code.AppendLine("            this.btnLastPage.Location = new System.Drawing.Point(183, 17);");
                code.AppendLine("            this.btnLastPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                code.AppendLine("            this.btnLastPage.Name = \"btnLastPage\";");
                code.AppendLine("            this.btnLastPage.Size = new System.Drawing.Size(41, 40);");
                code.AppendLine("            this.btnLastPage.TabIndex = 3;");
                code.AppendLine("            this.btnLastPage.Text = \">|\";");
                code.AppendLine("            this.btnLastPage.UseVisualStyleBackColor = false;");
                code.AppendLine("            // ");
                code.AppendLine("            // btnNextPage");
                code.AppendLine("            // ");
                code.AppendLine("            this.btnNextPage.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);");
                code.AppendLine("            this.btnNextPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(210, 210, 210);");
                code.AppendLine("            this.btnNextPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;");
                code.AppendLine("            this.btnNextPage.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
                code.AppendLine("            this.btnNextPage.Location = new System.Drawing.Point(126, 17);");
                code.AppendLine("            this.btnNextPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                code.AppendLine("            this.btnNextPage.Name = \"btnNextPage\";");
                code.AppendLine("            this.btnNextPage.Size = new System.Drawing.Size(41, 40);");
                code.AppendLine("            this.btnNextPage.TabIndex = 2;");
                code.AppendLine("            this.btnNextPage.Text = \">\";");
                code.AppendLine("            this.btnNextPage.UseVisualStyleBackColor = false;");
                code.AppendLine("            // ");
                code.AppendLine("            // btnPrevPage");
                code.AppendLine("            // ");
                code.AppendLine("            this.btnPrevPage.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);");
                code.AppendLine("            this.btnPrevPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(210, 210, 210);");
                code.AppendLine("            this.btnPrevPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;");
                code.AppendLine("            this.btnPrevPage.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
                code.AppendLine("            this.btnPrevPage.Location = new System.Drawing.Point(69, 17);");
                code.AppendLine("            this.btnPrevPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                code.AppendLine("            this.btnPrevPage.Name = \"btnPrevPage\";");
                code.AppendLine("            this.btnPrevPage.Size = new System.Drawing.Size(41, 40);");
                code.AppendLine("            this.btnPrevPage.TabIndex = 1;");
                code.AppendLine("            this.btnPrevPage.Text = \"<\";");
                code.AppendLine("            this.btnPrevPage.UseVisualStyleBackColor = false;");
                code.AppendLine("            // ");
                code.AppendLine("            // btnFirstPage");
                code.AppendLine("            // ");
                code.AppendLine("            this.btnFirstPage.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);");
                code.AppendLine("            this.btnFirstPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(210, 210, 210);");
                code.AppendLine("            this.btnFirstPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;");
                code.AppendLine("            this.btnFirstPage.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
                code.AppendLine("            this.btnFirstPage.Location = new System.Drawing.Point(12, 17);");
                code.AppendLine("            this.btnFirstPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                code.AppendLine("            this.btnFirstPage.Name = \"btnFirstPage\";");
                code.AppendLine("            this.btnFirstPage.Size = new System.Drawing.Size(41, 40);");
                code.AppendLine("            this.btnFirstPage.TabIndex = 0;");
                code.AppendLine("            this.btnFirstPage.Text = \"|<\";");
                code.AppendLine("            this.btnFirstPage.UseVisualStyleBackColor = false;");
                code.AppendLine("            // ");
                code.AppendLine($"            // {entityName}ManagementForm");
                code.AppendLine("            // ");
                code.AppendLine("            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);");
                code.AppendLine("            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;");
                code.AppendLine("            this.ClientSize = new System.Drawing.Size(1493, 992);");
                code.AppendLine($"            this.Controls.Add(this.dgv{entityName}s);");
                code.AppendLine("            this.Controls.Add(this.pnlPager);");
                code.AppendLine("            this.Controls.Add(this.pnlToolbar);");
                code.AppendLine("            this.Controls.Add(this.pnlSearch);");
                code.AppendLine("            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
                code.AppendLine($"            this.Name = \"{entityName}ManagementForm\";");
                code.AppendLine("            this.Padding = new System.Windows.Forms.Padding(10);");
                code.AppendLine("            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;");
                code.AppendLine($"            this.Text = \"{GetDisplayName(selectedEntityType)}管理\";");
                code.AppendLine("            this.pnlSearch.ResumeLayout(false);");
                code.AppendLine("            this.pnlSearch.PerformLayout();");
                code.AppendLine("            this.pnlToolbar.ResumeLayout(false);");
                code.AppendLine($"            ((System.ComponentModel.ISupportInitialize)(this.dgv{entityName}s)).EndInit();");
                code.AppendLine("            this.pnlPager.ResumeLayout(false);");
                code.AppendLine("            this.pnlPager.PerformLayout();");
                code.AppendLine("            this.ResumeLayout(false);");
                code.AppendLine();
                code.AppendLine("        }");
                code.AppendLine();
                code.AppendLine("        #endregion");

                // 添加字段声明
                code.AppendLine("        private System.Windows.Forms.Panel pnlSearch;");
                code.AppendLine("        private System.Windows.Forms.Button btnClear;");
                code.AppendLine("        private System.Windows.Forms.Button btnSearch;");

                // 动态添加查询控件声明
                 searchFieldCount = Math.Min(entityProperties.Count, 3); // 最多显示3个查询字段
                for (int i = 0; i < searchFieldCount; i++)
                {
                    var prop = entityProperties[i];

                    if (prop.PropertyType == typeof(bool))
                    {
                        code.AppendLine($"        private System.Windows.Forms.CheckBox chk{prop.Name};");
                    }
                    else if (prop.PropertyType.IsEnum || prop.Name.EndsWith("Id"))
                    {
                        code.AppendLine($"        private System.Windows.Forms.ComboBox cmb{prop.Name};");
                    }
                    else
                    {
                        code.AppendLine($"        private System.Windows.Forms.TextBox txt{prop.Name};");
                    }

                    code.AppendLine($"        private System.Windows.Forms.Label lbl{prop.Name};");
                }

                code.AppendLine("        private System.Windows.Forms.Panel pnlToolbar;");
                code.AppendLine("        private System.Windows.Forms.Button btnExportPDF;");
                code.AppendLine("        private System.Windows.Forms.Button btnExport;");
                code.AppendLine("        private System.Windows.Forms.Button btnImport;");
                code.AppendLine("        private System.Windows.Forms.Button btnDelete;");
                code.AppendLine("        private System.Windows.Forms.Button btnEdit;");
                code.AppendLine("        private System.Windows.Forms.Button btnAdd;");
                code.AppendLine($"        private System.Windows.Forms.DataGridView dgv{entityName}s;");

                // 动态添加列声明
                foreach (var prop in entityProperties)
                {
                    string columnName = $"col{prop.Name}";
                    if (prop.PropertyType == typeof(bool))
                    {
                        code.AppendLine($"        private System.Windows.Forms.DataGridViewCheckBoxColumn {columnName};");
                    }
                    else
                    {
                        code.AppendLine($"        private System.Windows.Forms.DataGridViewTextBoxColumn {columnName};");
                    }
                }

                code.AppendLine("        private System.Windows.Forms.Panel pnlPager;");
                code.AppendLine("        private System.Windows.Forms.Label lblPageInfo;");
                code.AppendLine("        private System.Windows.Forms.Button btnLastPage;");
                code.AppendLine("        private System.Windows.Forms.Button btnNextPage;");
                code.AppendLine("        private System.Windows.Forms.Button btnPrevPage;");
                code.AppendLine("        private System.Windows.Forms.Button btnFirstPage;");
                code.AppendLine("    }");
                code.AppendLine("}");

                StringBuilder backCode = new StringBuilder();










                // 选择输出目录
                using (var folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "选择窗体输出目录";
                    folderDialog.ShowNewFolderButton = true;

                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string outputDir = folderDialog.SelectedPath;

                        // 保存Designer.cs文件
                        string filePath = Path.Combine(outputDir, $"{entityName}ManagementForm.Designer.cs");
                        File.WriteAllText(filePath, code.ToString());

                        MessageBox.Show($"{entityName}ManagementForm.Designer.cs已生成到：\n{filePath}", "生成成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                btnGenerateFormCode_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成窗体失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void btnGenerateFormCode_Click(object sender, EventArgs e)
        {
            if (selectedEntityType == null)
            {
                MessageBox.Show("请先选择一个实体类", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string entityName = selectedEntityType.Name;
                string namespaceName = selectedEntityType.Namespace;

                // 生成窗体后台代码
                StringBuilder code = new StringBuilder();

                // 添加命名空间
                code.AppendLine("using System;");
                code.AppendLine("using System.Collections.Generic;");
                code.AppendLine("using System.ComponentModel;");
                code.AppendLine("using System.Data;");
                code.AppendLine("using System.Drawing;");
                code.AppendLine("using System.Linq;");
                code.AppendLine("using System.Text;");
                code.AppendLine("using System.Threading.Tasks;");
                code.AppendLine("using System.Windows.Forms;");
                code.AppendLine("using GC_MES.BLL.System.IService;");
                code.AppendLine("using GC_MES.Model.Common;");
                code.AppendLine($"using {namespaceName};");
                code.AppendLine("using System.IO;");
                code.AppendLine("using System.Reflection;");
                code.AppendLine();

                // 添加命名空间声明
                code.AppendLine("namespace GC_MES.WinForm.Forms.SystemForm");
                code.AppendLine("{");

                // 添加类声明
                code.AppendLine($"    public partial class {entityName}ManagementForm : Form");
                code.AppendLine("    {");

                // 添加私有成员变量
                code.AppendLine("        // 当前页数和每页条数");
                code.AppendLine("        private int currentPage = 1;");
                code.AppendLine("        private int pageSize = 20;");
                code.AppendLine("        private int totalCount = 0;");
                code.AppendLine("        private int totalPages = 0;");
                code.AppendLine();
                code.AppendLine("        // 服务实例");
                code.AppendLine($"        private readonly I{entityName}Service _{entityName.ToLower()}Service;");

                // 如果实体有DeptId属性，添加部门服务
                if (entityProperties.Any(p => p.Name == "DeptId"))
                {
                    code.AppendLine();
                    code.AppendLine("        // 部门服务");
                    code.AppendLine("        private readonly ISys_DeptService _deptService;");
                }

                code.AppendLine();
                code.AppendLine("        // 查询条件");

                // 动态添加查询条件变量
                int searchFieldCount = Math.Min(entityProperties.Count, 3); // 最多显示3个查询字段
                for (int i = 0; i < searchFieldCount; i++)
                {
                    var prop = entityProperties[i];
                    string fieldName = $"search{prop.Name}";

                    if (prop.PropertyType == typeof(string))
                    {
                        code.AppendLine($"        private string {fieldName} = string.Empty;");
                    }
                    else if (prop.PropertyType == typeof(bool))
                    {
                        code.AppendLine($"        private bool? {fieldName} = null;");
                    }
                    else if (prop.PropertyType.IsEnum || prop.Name.EndsWith("Id"))
                    {
                        code.AppendLine($"        private int? {fieldName} = null;");
                    }
                    else if (prop.PropertyType == typeof(DateTime))
                    {
                        code.AppendLine($"        private DateTime? {fieldName} = null;");
                    }
                    else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(long) ||
                             prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(double))
                    {
                        code.AppendLine($"        private {prop.PropertyType.Name}? {fieldName} = null;");
                    }
                }

                code.AppendLine();
                code.AppendLine("        // 当前选中的实体");
                code.AppendLine($"        private {entityName} selected{entityName} = null;");
                code.AppendLine();

                // 添加构造函数
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 构造函数");
                code.AppendLine("        /// </summary>");

                // 根据是否有DeptId生成不同的构造函数参数
                if (entityProperties.Any(p => p.Name == "DeptId"))
                {
                    code.AppendLine($"        public {entityName}ManagementForm(I{entityName}Service {entityName.ToLower()}Service, ISys_DeptService deptService)");
                    code.AppendLine("        {");
                    code.AppendLine("            InitializeComponent();");
                    code.AppendLine();
                    code.AppendLine("            // 初始化服务");
                    code.AppendLine($"            _{entityName.ToLower()}Service = {entityName.ToLower()}Service;");
                    code.AppendLine("            _deptService = deptService;");
                }
                else
                {
                    code.AppendLine($"        public {entityName}ManagementForm(I{entityName}Service {entityName.ToLower()}Service)");
                    code.AppendLine("        {");
                    code.AppendLine("            InitializeComponent();");
                    code.AppendLine();
                    code.AppendLine("            // 初始化服务");
                    code.AppendLine($"            _{entityName.ToLower()}Service = {entityName.ToLower()}Service;");
                }

                code.AppendLine();
                code.AppendLine("            // 应用主题");
                code.AppendLine("            Common.ThemeManager.Instance.ApplyTheme(this);");
                code.AppendLine();
                code.AppendLine("            // 初始化下拉框");
                code.AppendLine("            InitComboBoxes();");
                code.AppendLine();
                code.AppendLine("            // 注册事件处理");
                code.AppendLine("            RegisterEventHandlers();");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加窗体加载事件
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 窗体加载");
                code.AppendLine("        /// </summary>");
                code.AppendLine($"        private void {entityName}ManagementForm_Load(object sender, EventArgs e)");
                code.AppendLine("        {");
                code.AppendLine("            // 加载数据");
                code.AppendLine("            LoadData();");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加初始化下拉框方法
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 初始化下拉框");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void InitComboBoxes()");
                code.AppendLine("        {");
                code.AppendLine("            try");
                code.AppendLine("            {");

                // 检查是否有DeptId属性，如果有则添加部门下拉框初始化代码
                if (entityProperties.Any(p => p.Name == "DeptId"))
                {
                    code.AppendLine("                // 加载部门列表");
                    code.AppendLine("                var depts = _deptService.GetList();");
                    code.AppendLine();
                    code.AppendLine("                // 添加\"全部\"选项");
                    code.AppendLine("                var allItem = new GC_MES.Model.System.Sys_Dept { Dept_Id = -1, DeptName = \"全部\" };");
                    code.AppendLine("                var deptList = new List<GC_MES.Model.System.Sys_Dept> { allItem };");
                    code.AppendLine("                deptList.AddRange(depts);");
                    code.AppendLine();
                    code.AppendLine("                cmbDeptId.DisplayMember = \"DeptName\";");
                    code.AppendLine("                cmbDeptId.ValueMember = \"Dept_Id\";");
                    code.AppendLine("                cmbDeptId.DataSource = deptList;");
                    code.AppendLine("                cmbDeptId.SelectedValue = -1;");
                }

                // 检查是否有枚举类型的属性
                var enumProperties = entityProperties.Where(p => p.PropertyType.IsEnum).ToList();
                if (enumProperties.Any())
                {
                    code.AppendLine();
                    code.AppendLine("                // 初始化枚举下拉框");
                    foreach (var prop in enumProperties)
                    {
                        code.AppendLine($"                // 加载{GetDisplayName(prop)}下拉框");
                        code.AppendLine($"                cmb{prop.Name}.Items.Add(\"全部\");");
                        code.AppendLine($"                foreach (var value in Enum.GetValues(typeof({prop.PropertyType.FullName})))");
                        code.AppendLine("                {");
                        code.AppendLine($"                    cmb{prop.Name}.Items.Add(value);");
                        code.AppendLine("                }");
                        code.AppendLine($"                cmb{prop.Name}.SelectedIndex = 0;");
                        code.AppendLine();
                    }
                }

                code.AppendLine("            }");
                code.AppendLine("            catch (Exception ex)");
                code.AppendLine("            {");
                code.AppendLine("                MessageBox.Show($\"初始化下拉框失败: {ex.Message}\", \"错误\", MessageBoxButtons.OK, MessageBoxIcon.Error);");
                code.AppendLine("            }");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加注册事件处理方法
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 注册事件处理");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void RegisterEventHandlers()");
                code.AppendLine("        {");
                code.AppendLine("            // 按钮点击事件");
                code.AppendLine("            btnSearch.Click += btnSearch_Click;");
                code.AppendLine("            btnClear.Click += btnClear_Click;");
                code.AppendLine("            btnAdd.Click += btnAdd_Click;");
                code.AppendLine("            btnEdit.Click += btnEdit_Click;");
                code.AppendLine("            btnDelete.Click += btnDelete_Click;");
                code.AppendLine("            btnImport.Click += btnImport_Click;");
                code.AppendLine("            btnExport.Click += btnExport_Click;");
                code.AppendLine("            btnExportPDF.Click += btnExportPDF_Click;");
                code.AppendLine();
                code.AppendLine("            // 分页按钮点击事件");
                code.AppendLine("            btnFirstPage.Click += btnFirstPage_Click;");
                code.AppendLine("            btnPrevPage.Click += btnPrevPage_Click;");
                code.AppendLine("            btnNextPage.Click += btnNextPage_Click;");
                code.AppendLine("            btnLastPage.Click += btnLastPage_Click;");
                code.AppendLine();
                code.AppendLine("            // 数据表行选择事件");
                code.AppendLine($"            dgv{entityName}s.SelectionChanged += dgv{entityName}s_SelectionChanged;");
                code.AppendLine($"            dgv{entityName}s.CellDoubleClick += dgv{entityName}s_CellDoubleClick;");
                code.AppendLine();
                code.AppendLine("            // 窗体加载事件");
                code.AppendLine($"            this.Load += {entityName}ManagementForm_Load;");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加加载数据方法
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 加载数据");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void LoadData()");
                code.AppendLine("        {");
                code.AppendLine("            try");
                code.AppendLine("            {");
                code.AppendLine("                // 构建查询参数");
                code.AppendLine("                var parameters = new Dictionary<string, object>();");
                code.AppendLine();
                code.AppendLine("                // 添加查询条件");

                // 为每个搜索字段添加查询条件
                for (int i = 0; i < searchFieldCount; i++)
                {
                    var prop = entityProperties[i];
                    string fieldName = $"search{prop.Name}";

                    if (prop.PropertyType == typeof(string))
                    {
                        code.AppendLine($"                if (!string.IsNullOrWhiteSpace({fieldName}))");
                        code.AppendLine("                {");
                        code.AppendLine($"                    parameters.Add(\"{prop.Name}\", {fieldName});");
                        code.AppendLine("                }");
                    }
                    else if (prop.PropertyType == typeof(bool) || prop.PropertyType.IsEnum ||
                             prop.Name.EndsWith("Id") || prop.PropertyType == typeof(DateTime) ||
                             prop.PropertyType == typeof(int) || prop.PropertyType == typeof(long) ||
                             prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(double))
                    {
                        code.AppendLine($"                if ({fieldName}.HasValue)");
                        code.AppendLine("                {");
                        code.AppendLine($"                    parameters.Add(\"{prop.Name}\", {fieldName}.Value);");
                        code.AppendLine("                }");
                    }
                }

                code.AppendLine();
                code.AppendLine("                // 执行分页查询");
                code.AppendLine($"                var pagedResult = _{entityName.ToLower()}Service.GetPagedList(parameters, currentPage, pageSize);");
                code.AppendLine();
                code.AppendLine("                // 更新数据表");
                code.AppendLine($"                dgv{entityName}s.DataSource = pagedResult.Items;");
                code.AppendLine();
                code.AppendLine("                // 更新分页信息");
                code.AppendLine("                totalCount = pagedResult.TotalCount;");
                code.AppendLine("                totalPages = pagedResult.TotalPages;");
                code.AppendLine("                UpdatePaginationInfo();");
                code.AppendLine("            }");
                code.AppendLine("            catch (Exception ex)");
                code.AppendLine("            {");
                code.AppendLine("                MessageBox.Show($\"加载数据失败: {ex.Message}\", \"错误\", MessageBoxButtons.OK, MessageBoxIcon.Error);");
                code.AppendLine("            }");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加更新分页信息方法
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 更新分页信息");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void UpdatePaginationInfo()");
                code.AppendLine("        {");
                code.AppendLine("            lblPageInfo.Text = $\"第 {currentPage}/{totalPages} 页，共 {totalCount} 条\";");
                code.AppendLine();
                code.AppendLine("            // 禁用/启用分页按钮");
                code.AppendLine("            btnFirstPage.Enabled = currentPage > 1;");
                code.AppendLine("            btnPrevPage.Enabled = currentPage > 1;");
                code.AppendLine("            btnNextPage.Enabled = currentPage < totalPages;");
                code.AppendLine("            btnLastPage.Enabled = currentPage < totalPages;");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加获取搜索条件方法
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 获取搜索条件");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void GetSearchConditions()");
                code.AppendLine("        {");

                // 为每个搜索字段添加获取条件的代码
                for (int i = 0; i < searchFieldCount; i++)
                {
                    var prop = entityProperties[i];
                    string fieldName = $"search{prop.Name}";

                    if (prop.PropertyType == typeof(string))
                    {
                        code.AppendLine($"            // 获取{GetDisplayName(prop)}");
                        code.AppendLine($"            {fieldName} = txt{prop.Name}.Text.Trim();");
                    }
                    else if (prop.PropertyType == typeof(bool))
                    {
                        code.AppendLine($"            // 获取{GetDisplayName(prop)}");
                        code.AppendLine($"            {fieldName} = chk{prop.Name}.CheckState == CheckState.Indeterminate ? null : (bool?)chk{prop.Name}.Checked;");
                    }
                    else if (prop.PropertyType.IsEnum || prop.Name.EndsWith("Id"))
                    {
                        code.AppendLine($"            // 获取{GetDisplayName(prop)}");
                        code.AppendLine($"            if (cmb{prop.Name}.SelectedIndex > 0)");
                        code.AppendLine("            {");
                        if (prop.PropertyType.IsEnum)
                        {
                            code.AppendLine($"                {fieldName} = (int)cmb{prop.Name}.SelectedItem;");
                        }
                        else
                        {
                            code.AppendLine($"                {fieldName} = Convert.ToInt32(cmb{prop.Name}.SelectedValue);");
                        }
                        code.AppendLine("            }");
                        code.AppendLine("            else");
                        code.AppendLine("            {");
                        code.AppendLine($"                {fieldName} = null;");
                        code.AppendLine("            }");
                    }
                }

                code.AppendLine("        }");
                code.AppendLine();

                // 添加按钮事件区域
                code.AppendLine("        #region 按钮事件处理");
                code.AppendLine();

                // 添加搜索按钮点击事件
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 搜索按钮点击事件");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void btnSearch_Click(object sender, EventArgs e)");
                code.AppendLine("        {");
                code.AppendLine("            // 获取搜索条件");
                code.AppendLine("            GetSearchConditions();");
                code.AppendLine();
                code.AppendLine("            // 重置为第一页");
                code.AppendLine("            currentPage = 1;");
                code.AppendLine();
                code.AppendLine("            // 重新加载数据");
                code.AppendLine("            LoadData();");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加清空按钮点击事件
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 清空按钮点击事件");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void btnClear_Click(object sender, EventArgs e)");
                code.AppendLine("        {");
                code.AppendLine("            // 清空搜索控件");
                code.AppendLine("            foreach (Control ctrl in pnlSearch.Controls)");
                code.AppendLine("            {");
                code.AppendLine("                if (ctrl is TextBox txtBox)");
                code.AppendLine("                {");
                code.AppendLine("                    txtBox.Clear();");
                code.AppendLine("                }");
                code.AppendLine("                else if (ctrl is ComboBox cmbBox)");
                code.AppendLine("                {");
                code.AppendLine("                    if (cmbBox.Items.Count > 0)");
                code.AppendLine("                        cmbBox.SelectedIndex = 0;");
                code.AppendLine("                }");
                code.AppendLine("                else if (ctrl is CheckBox chkBox)");
                code.AppendLine("                {");
                code.AppendLine("                    chkBox.CheckState = CheckState.Indeterminate;");
                code.AppendLine("                }");
                code.AppendLine("            }");
                code.AppendLine();

                // 清空搜索条件变量
                for (int i = 0; i < searchFieldCount; i++)
                {
                    var prop = entityProperties[i];
                    string fieldName = $"search{prop.Name}";

                    if (prop.PropertyType == typeof(string))
                    {
                        code.AppendLine($"            {fieldName} = string.Empty;");
                    }
                    else
                    {
                        code.AppendLine($"            {fieldName} = null;");
                    }
                }

                code.AppendLine();
                code.AppendLine("            // 重置为第一页");
                code.AppendLine("            currentPage = 1;");
                code.AppendLine();
                code.AppendLine("            // 重新加载数据");
                code.AppendLine("            LoadData();");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加新增按钮点击事件
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 新增按钮点击事件");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void btnAdd_Click(object sender, EventArgs e)");
                code.AppendLine("        {");
                code.AppendLine($"            // 创建编辑窗体，传入null表示新增");
                code.AppendLine($"            using (var editForm = new {entityName}EditForm())");
                code.AppendLine("            {");
                code.AppendLine("                if (editForm.ShowDialog() == DialogResult.OK)");
                code.AppendLine("                {");
                code.AppendLine("                    // 重新加载数据");
                code.AppendLine("                    LoadData();");
                code.AppendLine("                }");
                code.AppendLine("            }");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加编辑按钮点击事件
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 编辑按钮点击事件");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void btnEdit_Click(object sender, EventArgs e)");
                code.AppendLine("        {");
                code.AppendLine($"            if (selected{entityName} == null)");
                code.AppendLine("            {");
                code.AppendLine("                MessageBox.Show(\"请先选择要编辑的记录\", \"提示\", MessageBoxButtons.OK, MessageBoxIcon.Information);");
                code.AppendLine("                return;");
                code.AppendLine("            }");
                code.AppendLine();
                code.AppendLine("            // 获取主键属性");
                code.AppendLine("            var keyProperty = typeof(" + entityName + ").GetProperties()");
                code.AppendLine("                .FirstOrDefault(p => p.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true).Any());");
                code.AppendLine();
                code.AppendLine("            if (keyProperty == null)");
                code.AppendLine("            {");
                code.AppendLine("                MessageBox.Show(\"无法确定实体的主键\", \"错误\", MessageBoxButtons.OK, MessageBoxIcon.Error);");
                code.AppendLine("                return;");
                code.AppendLine("            }");
                code.AppendLine();
                code.AppendLine("            // 获取主键值");
                code.AppendLine($"            var keyValue = keyProperty.GetValue(selected{entityName});");
                code.AppendLine();
                code.AppendLine("            // 加载完整数据");
                code.AppendLine($"            var entity = _{entityName.ToLower()}Service.GetById(keyValue);");
                code.AppendLine("            if (entity == null)");
                code.AppendLine("            {");
                code.AppendLine("                MessageBox.Show(\"找不到要编辑的数据\", \"错误\", MessageBoxButtons.OK, MessageBoxIcon.Error);");
                code.AppendLine("                return;");
                code.AppendLine("            }");
                code.AppendLine();
                code.AppendLine("            // 打开编辑窗体，传入实体表示编辑");
                code.AppendLine($"            using (var editForm = new {entityName}EditForm(entity))");
                code.AppendLine("            {");
                code.AppendLine("                if (editForm.ShowDialog() == DialogResult.OK)");
                code.AppendLine("                {");
                code.AppendLine("                    // 重新加载数据");
                code.AppendLine("                    LoadData();");
                code.AppendLine("                }");
                code.AppendLine("            }");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加删除按钮点击事件
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 删除按钮点击事件");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void btnDelete_Click(object sender, EventArgs e)");
                code.AppendLine("        {");
                code.AppendLine($"            if (selected{entityName} == null)");
                code.AppendLine("            {");
                code.AppendLine("                MessageBox.Show(\"请先选择要删除的记录\", \"提示\", MessageBoxButtons.OK, MessageBoxIcon.Information);");
                code.AppendLine("                return;");
                code.AppendLine("            }");
                code.AppendLine();
                code.AppendLine("            // 确认删除");
                code.AppendLine("            if (MessageBox.Show(\"确定要删除所选记录吗？\", \"确认删除\", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)");
                code.AppendLine("            {");
                code.AppendLine("                try");
                code.AppendLine("                {");
                code.AppendLine("                    // 获取主键属性");
                code.AppendLine("                    var keyProperty = typeof(" + entityName + ").GetProperties()");
                code.AppendLine("                        .FirstOrDefault(p => p.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true).Any());");
                code.AppendLine();
                code.AppendLine("                    if (keyProperty == null)");
                code.AppendLine("                    {");
                code.AppendLine("                        MessageBox.Show(\"无法确定实体的主键\", \"错误\", MessageBoxButtons.OK, MessageBoxIcon.Error);");
                code.AppendLine("                        return;");
                code.AppendLine("                    }");
                code.AppendLine();
                code.AppendLine("                    // 获取主键值");
                code.AppendLine($"                    var keyValue = keyProperty.GetValue(selected{entityName});");
                code.AppendLine();
                code.AppendLine("                    // 执行删除");
                code.AppendLine($"                    bool result = _{entityName.ToLower()}Service.DeleteById(keyValue);");
                code.AppendLine();
                code.AppendLine("                    if (result)");
                code.AppendLine("                    {");
                code.AppendLine("                        MessageBox.Show(\"删除成功\", \"提示\", MessageBoxButtons.OK, MessageBoxIcon.Information);");
                code.AppendLine();
                code.AppendLine("                        // 重新加载数据");
                code.AppendLine("                        LoadData();");
                code.AppendLine("                    }");
                code.AppendLine("                    else");
                code.AppendLine("                    {");
                code.AppendLine("                        MessageBox.Show(\"删除失败\", \"错误\", MessageBoxButtons.OK, MessageBoxIcon.Error);");
                code.AppendLine("                    }");
                code.AppendLine("                }");
                code.AppendLine("                catch (Exception ex)");
                code.AppendLine("                {");
                code.AppendLine("                    MessageBox.Show($\"删除失败: {ex.Message}\", \"错误\", MessageBoxButtons.OK, MessageBoxIcon.Error);");
                code.AppendLine("                }");
                code.AppendLine("            }");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加导入按钮点击事件
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 导入按钮点击事件");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void btnImport_Click(object sender, EventArgs e)");
                code.AppendLine("        {");
                code.AppendLine("            using (OpenFileDialog openFileDialog = new OpenFileDialog())");
                code.AppendLine("            {");
                code.AppendLine("                openFileDialog.Filter = \"Excel Files|*.xlsx;*.xls\";");
                code.AppendLine("                openFileDialog.Title = \"选择Excel文件\";");
                code.AppendLine();
                code.AppendLine("                if (openFileDialog.ShowDialog() == DialogResult.OK)");
                code.AppendLine("                {");
                code.AppendLine("                    try");
                code.AppendLine("                    {");
                code.AppendLine("                        string filePath = openFileDialog.FileName;");
                code.AppendLine("                        // 这里实现Excel导入逻辑，可以使用Common中的ExcelHelper类");
                code.AppendLine($"                        // List<{entityName}> importData = GC_MES.Common.ExcelHelper.ImportFromExcel<{entityName}>(filePath);");
                code.AppendLine($"                        // bool result = _{entityName.ToLower()}Service.BatchInsert(importData);");
                code.AppendLine();
                code.AppendLine("                        MessageBox.Show(\"导入成功\", \"提示\", MessageBoxButtons.OK, MessageBoxIcon.Information);");
                code.AppendLine();
                code.AppendLine("                        // 重新加载数据");
                code.AppendLine("                        LoadData();");
                code.AppendLine("                    }");
                code.AppendLine("                    catch (Exception ex)");
                code.AppendLine("                    {");
                code.AppendLine("                        MessageBox.Show($\"导入失败: {ex.Message}\", \"错误\", MessageBoxButtons.OK, MessageBoxIcon.Error);");
                code.AppendLine("                    }");
                code.AppendLine("                }");
                code.AppendLine("            }");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加导出按钮点击事件
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 导出Excel按钮点击事件");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void btnExport_Click(object sender, EventArgs e)");
                code.AppendLine("        {");
                code.AppendLine("            try");
                code.AppendLine("            {");
                code.AppendLine("                using (SaveFileDialog saveFileDialog = new SaveFileDialog())");
                code.AppendLine("                {");
                code.AppendLine("                    saveFileDialog.Filter = \"Excel Files|*.xlsx\";");
                code.AppendLine("                    saveFileDialog.Title = \"保存Excel文件\";");
                code.AppendLine($"                    saveFileDialog.FileName = $\"{entityName}导出_{DateTime.Now:yyyyMMddHHmmss}.xlsx\";");
                code.AppendLine();
                code.AppendLine("                    if (saveFileDialog.ShowDialog() == DialogResult.OK)");
                code.AppendLine("                    {");
                code.AppendLine("                        // 获取所有数据");
                code.AppendLine($"                        var data = _{entityName.ToLower()}Service.GetList();");
                code.AppendLine();
                code.AppendLine("                        // 调用导出方法");
                code.AppendLine("                        // GC_MES.Common.ExcelHelper.ExportToExcel(data, saveFileDialog.FileName);");
                code.AppendLine();
                code.AppendLine("                        MessageBox.Show(\"导出成功\", \"提示\", MessageBoxButtons.OK, MessageBoxIcon.Information);");
                code.AppendLine("                    }");
                code.AppendLine("                }");
                code.AppendLine("            }");
                code.AppendLine("            catch (Exception ex)");
                code.AppendLine("            {");
                code.AppendLine("                MessageBox.Show($\"导出失败: {ex.Message}\", \"错误\", MessageBoxButtons.OK, MessageBoxIcon.Error);");
                code.AppendLine("            }");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加导出PDF按钮点击事件
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 导出PDF按钮点击事件");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void btnExportPDF_Click(object sender, EventArgs e)");
                code.AppendLine("        {");
                code.AppendLine("            try");
                code.AppendLine("            {");
                code.AppendLine("                using (SaveFileDialog saveFileDialog = new SaveFileDialog())");
                code.AppendLine("                {");
                code.AppendLine("                    saveFileDialog.Filter = \"PDF Files|*.pdf\";");
                code.AppendLine("                    saveFileDialog.Title = \"保存PDF文件\";");
                code.AppendLine($"                    saveFileDialog.FileName = $\"{entityName}导出_{DateTime.Now:yyyyMMddHHmmss}.pdf\";");
                code.AppendLine();
                code.AppendLine("                    if (saveFileDialog.ShowDialog() == DialogResult.OK)");
                code.AppendLine("                    {");
                code.AppendLine("                        // 获取所有数据");
                code.AppendLine($"                        var data = _{entityName.ToLower()}Service.GetList();");
                code.AppendLine();
                code.AppendLine("                        // 调用导出方法");
                code.AppendLine("                        // GC_MES.Common.PdfHelper.ExportToPdf(data, saveFileDialog.FileName);");
                code.AppendLine();
                code.AppendLine("                        MessageBox.Show(\"导出成功\", \"提示\", MessageBoxButtons.OK, MessageBoxIcon.Information);");
                code.AppendLine("                    }");
                code.AppendLine("                }");
                code.AppendLine("            }");
                code.AppendLine("            catch (Exception ex)");
                code.AppendLine("            {");
                code.AppendLine("                MessageBox.Show($\"导出失败: {ex.Message}\", \"错误\", MessageBoxButtons.OK, MessageBoxIcon.Error);");
                code.AppendLine("            }");
                code.AppendLine("        }");
                code.AppendLine();
                code.AppendLine("        #endregion");
                code.AppendLine();

                // 添加分页事件区域
                code.AppendLine("        #region 分页事件处理");
                code.AppendLine();

                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 第一页按钮点击事件");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void btnFirstPage_Click(object sender, EventArgs e)");
                code.AppendLine("        {");
                code.AppendLine("            if (currentPage > 1)");
                code.AppendLine("            {");
                code.AppendLine("                currentPage = 1;");
                code.AppendLine("                LoadData();");
                code.AppendLine("            }");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加上一页按钮点击事件
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 上一页按钮点击事件");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void btnPrevPage_Click(object sender, EventArgs e)");
                code.AppendLine("        {");
                code.AppendLine("            if (currentPage > 1)");
                code.AppendLine("            {");
                code.AppendLine("                currentPage--;");
                code.AppendLine("                LoadData();");
                code.AppendLine("            }");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加下一页按钮点击事件
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 下一页按钮点击事件");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void btnNextPage_Click(object sender, EventArgs e)");
                code.AppendLine("        {");
                code.AppendLine("            if (currentPage < totalPages)");
                code.AppendLine("            {");
                code.AppendLine("                currentPage++;");
                code.AppendLine("                LoadData();");
                code.AppendLine("            }");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加最后一页按钮点击事件
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 最后一页按钮点击事件");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void btnLastPage_Click(object sender, EventArgs e)");
                code.AppendLine("        {");
                code.AppendLine("            if (currentPage < totalPages)");
                code.AppendLine("            {");
                code.AppendLine("                currentPage = totalPages;");
                code.AppendLine("                LoadData();");
                code.AppendLine("            }");
                code.AppendLine("        }");
                code.AppendLine();

                code.AppendLine("        #endregion");
                code.AppendLine();

                // 添加数据表事件区域
                code.AppendLine("        #region 数据表事件处理");
                code.AppendLine();

                // 添加选择行改变事件
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 数据表选择行改变事件");
                code.AppendLine("        /// </summary>");
                code.AppendLine($"        private void dgv{entityName}s_SelectionChanged(object sender, EventArgs e)");
                code.AppendLine("        {");
                code.AppendLine("            if (dgv" + entityName + "s.SelectedRows.Count > 0)");
                code.AppendLine("            {");
                code.AppendLine($"                selected{entityName} = dgv{entityName}s.SelectedRows[0].DataBoundItem as {entityName};");
                code.AppendLine("            }");
                code.AppendLine("            else");
                code.AppendLine("            {");
                code.AppendLine($"                selected{entityName} = null;");
                code.AppendLine("            }");
                code.AppendLine();
                code.AppendLine("            // 启用/禁用按钮");
                code.AppendLine($"            btnEdit.Enabled = selected{entityName} != null;");
                code.AppendLine($"            btnDelete.Enabled = selected{entityName} != null;");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加单元格双击事件
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 数据表单元格双击事件");
                code.AppendLine("        /// </summary>");
                code.AppendLine($"        private void dgv{entityName}s_CellDoubleClick(object sender, DataGridViewCellEventArgs e)");
                code.AppendLine("        {");
                code.AppendLine("            // 如果双击的不是表头行，且有选中的行，则执行编辑操作");
                code.AppendLine($"            if (e.RowIndex >= 0 && selected{entityName} != null)");
                code.AppendLine("            {");
                code.AppendLine("                btnEdit_Click(sender, e);");
                code.AppendLine("            }");
                code.AppendLine("        }");
                code.AppendLine();

                code.AppendLine("        #endregion");
                code.AppendLine();

         

                // 结束类定义
                code.AppendLine("    }");
                code.AppendLine("}");

                // 选择输出目录
                using (var folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "选择窗体代码输出目录";
                    folderDialog.ShowNewFolderButton = true;

                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string outputDir = folderDialog.SelectedPath;

                        // 保存窗体代码文件
                        string filePath = Path.Combine(outputDir, $"{entityName}ManagementForm.cs");
                        File.WriteAllText(filePath, code.ToString());

                        MessageBox.Show($"{entityName}ManagementForm.cs已生成到：\n{filePath}", "生成成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 询问是否同时生成编辑窗体代码
                        if (MessageBox.Show($"是否同时生成{entityName}EditForm编辑窗体代码？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            GenerateEditForm(entityName, namespaceName, outputDir);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成窗体代码失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        /// <summary>
        /// 生成编辑窗体代码
        /// </summary>
        private void GenerateEditForm(string entityName, string namespaceName, string outputDir)
        {
            try
            {
                // 生成编辑窗体代码
                StringBuilder code = new StringBuilder();

                // 添加命名空间
                code.AppendLine("using System;");
                code.AppendLine("using System.Collections.Generic;");
                code.AppendLine("using System.ComponentModel;");
                code.AppendLine("using System.Data;");
                code.AppendLine("using System.Drawing;");
                code.AppendLine("using System.Linq;");
                code.AppendLine("using System.Text;");
                code.AppendLine("using System.Threading.Tasks;");
                code.AppendLine("using System.Windows.Forms;");
                code.AppendLine("using GC_MES.BLL.System.IService;");
                code.AppendLine($"using {namespaceName};");
                code.AppendLine("using System.Reflection;");
                code.AppendLine();

                // 添加命名空间声明
                code.AppendLine("namespace GC_MES.WinForm.Forms");
                code.AppendLine("{");

                // 添加类声明
                code.AppendLine($"    public partial class {entityName}EditForm : Form");
                code.AppendLine("    {");

                // 添加私有成员变量
                code.AppendLine($"        private {entityName} _entity;");
                code.AppendLine("        private bool _isNew = true;");
                code.AppendLine($"        private readonly I{entityName}Service _{entityName.ToLower()}Service;");

                // 如果实体有DeptId属性，添加部门服务
                if (entityProperties.Any(p => p.Name == "DeptId"))
                {
                    code.AppendLine("        private readonly ISys_DeptService _deptService;");
                }

                code.AppendLine();

                // 添加构造函数（新增）
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 构造函数 - 新增模式");
                code.AppendLine("        /// </summary>");

                if (entityProperties.Any(p => p.Name == "DeptId"))
                {
                    code.AppendLine($"        public {entityName}EditForm(I{entityName}Service {entityName.ToLower()}Service = null, ISys_DeptService deptService = null)");
                    code.AppendLine("        {");
                    code.AppendLine("            InitializeComponent();");
                    code.AppendLine();
                    code.AppendLine("            // 初始化服务，如果未提供则从IoC容器获取");
                    code.AppendLine($"            _{entityName.ToLower()}Service = {entityName.ToLower()}Service ?? Program.ServiceProvider.GetService(typeof(I{entityName}Service)) as I{entityName}Service;");
                    code.AppendLine("            _deptService = deptService ?? Program.ServiceProvider.GetService(typeof(ISys_DeptService)) as ISys_DeptService;");
                }
                else
                {
                    code.AppendLine($"        public {entityName}EditForm(I{entityName}Service {entityName.ToLower()}Service = null)");
                    code.AppendLine("        {");
                    code.AppendLine("            InitializeComponent();");
                    code.AppendLine();
                    code.AppendLine("            // 初始化服务，如果未提供则从IoC容器获取");
                    code.AppendLine($"            _{entityName.ToLower()}Service = {entityName.ToLower()}Service;");
                }

                code.AppendLine();
                code.AppendLine("            // 新增模式");
                code.AppendLine("            _isNew = true;");
                code.AppendLine($"            _entity = new {entityName}();");
                code.AppendLine();
                code.AppendLine("            // 应用主题");
                code.AppendLine("            Common.ThemeManager.Instance.ApplyTheme(this);");
                code.AppendLine("            ");
                code.AppendLine("            // 初始化下拉框");
                code.AppendLine("            InitComboBoxes();");
                code.AppendLine("            ");
                code.AppendLine("            // 设置控件默认值");
                code.AppendLine("            InitControlDefaultValues();");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加构造函数（编辑）
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 构造函数 - 编辑模式");
                code.AppendLine("        /// </summary>");

                if (entityProperties.Any(p => p.Name == "DeptId"))
                {
                    code.AppendLine($"        public {entityName}EditForm({entityName} entity, I{entityName}Service {entityName.ToLower()}Service = null, ISys_DeptService deptService = null)");
                    code.AppendLine("            : this({entityName.ToLower()}Service, deptService)");
                }
                else
                {
                    code.AppendLine($"        public {entityName}EditForm({entityName} entity, I{entityName}Service {entityName.ToLower()}Service = null)");
                    code.AppendLine($"            : this({entityName.ToLower()}Service)");
                }

                code.AppendLine("        {");
                code.AppendLine("            if (entity != null)");
                code.AppendLine("            {");
                code.AppendLine("                // 编辑模式");
                code.AppendLine("                _isNew = false;");
                code.AppendLine("                _entity = entity;");
                code.AppendLine();
                code.AppendLine("                // 加载实体数据到控件");
                code.AppendLine("                LoadEntityToControls();");
                code.AppendLine("            }");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加窗体加载事件
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 窗体加载事件");
                code.AppendLine("        /// </summary>");
                code.AppendLine($"        private void {entityName}EditForm_Load(object sender, EventArgs e)");
                code.AppendLine("        {");
                code.AppendLine("            // 设置窗体标题");
                code.AppendLine($"            this.Text = _isNew ? \"新增{entityName}\" : \"编辑{entityName}\";");
                code.AppendLine();
                code.AppendLine("            // 如果是编辑模式，禁用ID等主键字段");
                code.AppendLine("            DisablePrimaryKeyControls();");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加初始化下拉框方法
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 初始化下拉框");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void InitComboBoxes()");
                code.AppendLine("        {");
                code.AppendLine("            try");
                code.AppendLine("            {");

                // 检查是否有DeptId属性，如果有则添加部门下拉框初始化代码
                if (entityProperties.Any(p => p.Name == "DeptId"))
                {
                    code.AppendLine("                // 加载部门下拉框");
                    code.AppendLine("                var depts = _deptService.GetList();");
                    code.AppendLine();
                    code.AppendLine("                cmbDeptId.DisplayMember = \"DeptName\";");
                    code.AppendLine("                cmbDeptId.ValueMember = \"Dept_Id\";");
                    code.AppendLine("                cmbDeptId.DataSource = depts;");
                }

                // 检查是否有枚举类型的属性
                var enumProperties = entityProperties.Where(p => p.PropertyType.IsEnum).ToList();
                if (enumProperties.Any())
                {
                    code.AppendLine();
                    code.AppendLine("                // 初始化枚举下拉框");
                    foreach (var prop in enumProperties)
                    {
                        code.AppendLine($"                // 加载{GetDisplayName(prop)}下拉框");
                        code.AppendLine($"                cmb{prop.Name}.Items.Clear();");
                        code.AppendLine($"                foreach (var value in Enum.GetValues(typeof({prop.PropertyType.FullName})))");
                        code.AppendLine("                {");
                        code.AppendLine($"                    cmb{prop.Name}.Items.Add(value);");
                        code.AppendLine("                }");
                    }
                }

                code.AppendLine("            }");
                code.AppendLine("            catch (Exception ex)");
                code.AppendLine("            {");
                code.AppendLine("                MessageBox.Show($\"初始化下拉框失败: {ex.Message}\", \"错误\", MessageBoxButtons.OK, MessageBoxIcon.Error);");
                code.AppendLine("            }");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加初始化控件默认值方法
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 初始化控件默认值");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void InitControlDefaultValues()");
                code.AppendLine("        {");
                code.AppendLine("            // 设置默认值");

                // 为布尔类型属性设置默认值
                foreach (var prop in entityProperties.Where(p => p.PropertyType == typeof(bool)))
                {
                    code.AppendLine($"            chk{prop.Name}.Checked = false;");
                }

                // 为整型属性设置默认值
                foreach (var prop in entityProperties.Where(p =>
                    p.PropertyType == typeof(int) || p.PropertyType == typeof(long) ||
                    p.PropertyType == typeof(short) || p.PropertyType == typeof(byte)))
                {
                    if (prop.Name == "OrderNo" || prop.Name.EndsWith("Order"))
                    {
                        code.AppendLine($"            nud{prop.Name}.Value = 0;");
                    }
                }

                // 为日期类型属性设置默认值
                foreach (var prop in entityProperties.Where(p => p.PropertyType == typeof(DateTime)))
                {
                    code.AppendLine($"            dtp{prop.Name}.Value = DateTime.Now;");
                }

                code.AppendLine("        }");
                code.AppendLine();

                // 添加加载实体到控件方法
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 加载实体数据到控件");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void LoadEntityToControls()");
                code.AppendLine("        {");
                code.AppendLine("            if (_entity == null) return;");
                code.AppendLine();
                code.AppendLine("            // 加载属性值到对应控件");

                // 为每种类型的属性添加加载到控件的代码
                foreach (var prop in entityProperties)
                {
                    string controlName = GetControlNameForProperty(prop);

                    if (prop.PropertyType == typeof(string))
                    {
                        code.AppendLine($"            {controlName}.Text = _entity.{prop.Name} ?? string.Empty;");
                    }
                    else if (prop.PropertyType == typeof(bool))
                    {
                        code.AppendLine($"            {controlName}.Checked = _entity.{prop.Name};");
                    }
                    else if (prop.PropertyType.IsEnum)
                    {
                        code.AppendLine($"            {controlName}.SelectedItem = _entity.{prop.Name};");
                    }
                    else if (prop.Name.EndsWith("Id") && prop.PropertyType != typeof(string))
                    {
                        code.AppendLine($"            {controlName}.SelectedValue = _entity.{prop.Name};");
                    }
                    else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(long) ||
                             prop.PropertyType == typeof(short) || prop.PropertyType == typeof(byte))
                    {
                        if (prop.Name == "OrderNo" || prop.Name.EndsWith("Order"))
                        {
                            code.AppendLine($"            {controlName}.Value = _entity.{prop.Name};");
                        }
                        else
                        {
                            code.AppendLine($"            {controlName}.Text = _entity.{prop.Name}.ToString();");
                        }
                    }
                    else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(double) ||
                             prop.PropertyType == typeof(float))
                    {
                        code.AppendLine($"            {controlName}.Text = _entity.{prop.Name}.ToString(\"0.##\");");
                    }
                    else if (prop.PropertyType == typeof(DateTime))
                    {
                        code.AppendLine($"            if (_entity.{prop.Name} != DateTime.MinValue)");
                        code.AppendLine("            {");
                        code.AppendLine($"                {controlName}.Value = _entity.{prop.Name};");
                        code.AppendLine("            }");
                    }
                }

                code.AppendLine("        }");
                code.AppendLine();

                // 添加控件数据收集到实体方法
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 从控件收集数据到实体");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void CollectDataFromControls()");
                code.AppendLine("        {");
                code.AppendLine("            // 收集属性值从对应控件");

                // 为每种类型的属性添加从控件收集值的代码
                foreach (var prop in entityProperties)
                {
                    string controlName = GetControlNameForProperty(prop);

                    // 如果是主键属性且不是新增模式，则跳过
                    if (prop.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true).Any())
                    {
                        code.AppendLine($"            // 如果不是新增模式，跳过主键");
                        code.AppendLine("            if (!_isNew)");
                        code.AppendLine("            {");
                        code.AppendLine($"                // 保留原主键值");
                        code.AppendLine("            }");
                        code.AppendLine("            else");
                        code.AppendLine("            {");

                        if (prop.PropertyType == typeof(string))
                        {
                            code.AppendLine($"                _entity.{prop.Name} = {controlName}.Text.Trim();");
                        }
                        else if (prop.PropertyType == typeof(Guid))
                        {
                            code.AppendLine($"                _entity.{prop.Name} = string.IsNullOrEmpty({controlName}.Text) ? Guid.NewGuid() : Guid.Parse({controlName}.Text);");
                        }
                        else
                        {
                            code.AppendLine($"                _entity.{prop.Name} = Convert.To{prop.PropertyType.Name}({controlName}.Text);");
                        }

                        code.AppendLine("            }");
                        continue;
                    }

                    if (prop.PropertyType == typeof(string))
                    {
                        code.AppendLine($"            _entity.{prop.Name} = {controlName}.Text.Trim();");
                    }
                    else if (prop.PropertyType == typeof(bool))
                    {
                        code.AppendLine($"            _entity.{prop.Name} = {controlName}.Checked;");
                    }
                    else if (prop.PropertyType.IsEnum)
                    {
                        code.AppendLine($"            if ({controlName}.SelectedItem != null)");
                        code.AppendLine("            {");
                        code.AppendLine($"                _entity.{prop.Name} = ({prop.PropertyType.FullName}){controlName}.SelectedItem;");
                        code.AppendLine("            }");
                    }
                    else if (prop.Name.EndsWith("Id") && prop.PropertyType != typeof(string))
                    {
                        code.AppendLine($"            if ({controlName}.SelectedValue != null)");
                        code.AppendLine("            {");
                        code.AppendLine($"                _entity.{prop.Name} = Convert.To{prop.PropertyType.Name}({controlName}.SelectedValue);");
                        code.AppendLine("            }");
                    }
                    else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(long) ||
                             prop.PropertyType == typeof(short) || prop.PropertyType == typeof(byte))
                    {
                        if (prop.Name == "OrderNo" || prop.Name.EndsWith("Order"))
                        {
                            code.AppendLine($"            _entity.{prop.Name} = ({prop.PropertyType.Name}){controlName}.Value;");
                        }
                        else
                        {
                            code.AppendLine($"            if (!string.IsNullOrWhiteSpace({controlName}.Text))");
                            code.AppendLine("            {");
                            code.AppendLine($"                _entity.{prop.Name} = Convert.To{prop.PropertyType.Name}({controlName}.Text);");
                            code.AppendLine("            }");
                        }
                    }
                    else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(double) ||
                             prop.PropertyType == typeof(float))
                    {
                        code.AppendLine($"            if (!string.IsNullOrWhiteSpace({controlName}.Text))");
                        code.AppendLine("            {");
                        code.AppendLine($"                _entity.{prop.Name} = Convert.To{prop.PropertyType.Name}({controlName}.Text, System.Globalization.CultureInfo.InvariantCulture);");
                        code.AppendLine("            }");
                    }
                    else if (prop.PropertyType == typeof(DateTime))
                    {
                        code.AppendLine($"            _entity.{prop.Name} = {controlName}.Value;");
                    }
                    else if (prop.PropertyType == typeof(Guid))
                    {
                        code.AppendLine($"            if (!string.IsNullOrEmpty({controlName}.Text))");
                        code.AppendLine("            {");
                        code.AppendLine($"                _entity.{prop.Name} = Guid.Parse({controlName}.Text);");
                        code.AppendLine("            }");
                    }
                }

                // 设置创建时间和修改时间
                if (entityProperties.Any(p => p.Name == "CreateDate" || p.Name == "CreateTime"))
                {
                    var createDateProp = entityProperties.FirstOrDefault(p => p.Name == "CreateDate" || p.Name == "CreateTime");
                    if (createDateProp != null)
                    {
                        code.AppendLine();
                        code.AppendLine("            // 设置创建时间和修改时间");
                        code.AppendLine("            if (_isNew)");
                        code.AppendLine("            {");
                        code.AppendLine($"                _entity.{createDateProp.Name} = DateTime.Now;");
                        code.AppendLine("            }");
                    }
                }

                if (entityProperties.Any(p => p.Name == "ModifyDate" || p.Name == "UpdateTime"))
                {
                    var modifyDateProp = entityProperties.FirstOrDefault(p => p.Name == "ModifyDate" || p.Name == "UpdateTime");
                    if (modifyDateProp != null)
                    {
                        code.AppendLine("            if (!_isNew)");
                        code.AppendLine("            {");
                        code.AppendLine($"                _entity.{modifyDateProp.Name} = DateTime.Now;");
                        code.AppendLine("            }");
                    }
                }

                code.AppendLine("        }");
                code.AppendLine();

                // 添加禁用主键控件方法
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 禁用主键相关控件");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void DisablePrimaryKeyControls()");
                code.AppendLine("        {");
                code.AppendLine("            if (!_isNew)");
                code.AppendLine("            {");

                // 查找主键属性
                var keyProps = entityProperties.Where(p =>
                    p.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true).Any());

                foreach (var keyProp in keyProps)
                {
                    string controlName = GetControlNameForProperty(keyProp);
                    code.AppendLine($"                // 禁用主键控件");
                    code.AppendLine($"                {controlName}.Enabled = false;");
                }

                code.AppendLine("            }");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加验证数据方法
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 验证数据");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private bool ValidateData()");
                code.AppendLine("        {");

                // 查找必填字段
                var requiredProps = entityProperties.Where(p =>
                    p.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), true).Any());

                if (requiredProps.Any())
                {
                    code.AppendLine("            // 验证必填字段");

                    foreach (var prop in requiredProps)
                    {
                        string controlName = GetControlNameForProperty(prop);
                        string displayName = GetDisplayName(prop);

                        if (prop.PropertyType == typeof(string))
                        {
                            code.AppendLine($"            if (string.IsNullOrWhiteSpace({controlName}.Text))");
                            code.AppendLine("            {");
                            code.AppendLine($"                MessageBox.Show($\"{displayName}不能为空\", \"验证错误\", MessageBoxButtons.OK, MessageBoxIcon.Warning);");
                            code.AppendLine($"                {controlName}.Focus();");
                            code.AppendLine("                return false;");
                            code.AppendLine("            }");
                        }
                        else if (prop.PropertyType.IsEnum || prop.Name.EndsWith("Id"))
                        {
                            code.AppendLine($"            if ({controlName}.SelectedItem == null)");
                            code.AppendLine("            {");
                            code.AppendLine($"                MessageBox.Show($\"请选择{displayName}\", \"验证错误\", MessageBoxButtons.OK, MessageBoxIcon.Warning);");
                            code.AppendLine($"                {controlName}.Focus();");
                            code.AppendLine("                return false;");
                            code.AppendLine("            }");
                        }
                    }
                }

                code.AppendLine();
                code.AppendLine("            return true;");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加保存按钮点击事件
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 保存按钮点击事件");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void btnSave_Click(object sender, EventArgs e)");
                code.AppendLine("        {");
                code.AppendLine("            // 验证数据");
                code.AppendLine("            if (!ValidateData())");
                code.AppendLine("            {");
                code.AppendLine("                return;");
                code.AppendLine("            }");
                code.AppendLine();
                code.AppendLine("            try");
                code.AppendLine("            {");
                code.AppendLine("                // 收集数据");
                code.AppendLine("                CollectDataFromControls();");
                code.AppendLine();
                code.AppendLine("                bool result;");
                code.AppendLine("                if (_isNew)");
                code.AppendLine("                {");
                code.AppendLine("                    // 新增");
                code.AppendLine($"                    result = _{entityName.ToLower()}Service.Add(_entity);");
                code.AppendLine("                }");
                code.AppendLine("                else");
                code.AppendLine("                {");
                code.AppendLine("                    // 编辑");
                code.AppendLine($"                    result = _{entityName.ToLower()}Service.Update(_entity);");
                code.AppendLine("                }");
                code.AppendLine();
                code.AppendLine("                if (result)");
                code.AppendLine("                {");
                code.AppendLine("                    MessageBox.Show(\"保存成功\", \"提示\", MessageBoxButtons.OK, MessageBoxIcon.Information);");
                code.AppendLine("                    DialogResult = DialogResult.OK;");
                code.AppendLine("                    Close();");
                code.AppendLine("                }");
                code.AppendLine("                else");
                code.AppendLine("                {");
                code.AppendLine("                    MessageBox.Show(\"保存失败\", \"错误\", MessageBoxButtons.OK, MessageBoxIcon.Error);");
                code.AppendLine("                }");
                code.AppendLine("            }");
                code.AppendLine("            catch (Exception ex)");
                code.AppendLine("            {");
                code.AppendLine("                MessageBox.Show($\"保存失败: {ex.Message}\", \"错误\", MessageBoxButtons.OK, MessageBoxIcon.Error);");
                code.AppendLine("            }");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加取消按钮点击事件
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 取消按钮点击事件");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private void btnCancel_Click(object sender, EventArgs e)");
                code.AppendLine("        {");
                code.AppendLine("            DialogResult = DialogResult.Cancel;");
                code.AppendLine("            Close();");
                code.AppendLine("        }");
                code.AppendLine();

                // 添加获取Display名称的方法
                code.AppendLine("        /// <summary>");
                code.AppendLine("        /// 获取类型或属性的Display特性的Name值");
                code.AppendLine("        /// </summary>");
                code.AppendLine("        private string GetDisplayName(MemberInfo member)");
                code.AppendLine("        {");
                code.AppendLine("            var displayAttr = member.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), true)");
                code.AppendLine("                .FirstOrDefault() as System.ComponentModel.DataAnnotations.DisplayAttribute;");
                code.AppendLine("            return displayAttr?.Name ?? member.Name;");
                code.AppendLine("        }");

                // 结束类定义
                code.AppendLine("    }");
                code.AppendLine("}");

                // 保存编辑窗体代码
                string filePath = Path.Combine(outputDir, $"{entityName}EditForm.cs");
                File.WriteAllText(filePath, code.ToString());

                MessageBox.Show($"{entityName}EditForm.cs已生成到：\n{filePath}", "生成成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 生成Designer.cs代码
                GenerateEditFormDesigner(entityName, outputDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成编辑窗体代码失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        /// <summary>
        /// 生成编辑窗体
        /// </summary>
        private void btnGenerateEditForm_Click(object sender, EventArgs e)
        {
            if (selectedEntityType == null)
            {
                MessageBox.Show("请先选择一个实体类", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // TODO: 实现编辑窗体生成逻辑
            MessageBox.Show($"即将为 {selectedEntityType.Name} 生成编辑窗体", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 生成编辑窗体的设计器代码
        /// </summary>
        private string GenerateEditFormDesigner(Type entityType)
        {
            if (entityType == null) return string.Empty;

            StringBuilder code = new StringBuilder();
            string entityName = entityType.Name;

            // 添加命名空间声明
            code.AppendLine("namespace GC_MES.WinForm.Forms");
            code.AppendLine("{");

            // 添加类声明
            code.AppendLine($"    partial class {entityName}EditForm");
            code.AppendLine("    {");
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// 必需的设计器变量");
            code.AppendLine("        /// </summary>");
            code.AppendLine("        private System.ComponentModel.IContainer components = null;");
            code.AppendLine();
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// 清理所有正在使用的资源");
            code.AppendLine("        /// </summary>");
            code.AppendLine("        /// <param name=\"disposing\">如果应释放托管资源，为 true；否则为 false</param>");
            code.AppendLine("        protected override void Dispose(bool disposing)");
            code.AppendLine("        {");
            code.AppendLine("            if (disposing && (components != null))");
            code.AppendLine("            {");
            code.AppendLine("                components.Dispose();");
            code.AppendLine("            }");
            code.AppendLine("            base.Dispose(disposing);");
            code.AppendLine("        }");
            code.AppendLine();
            code.AppendLine("        #region Windows 窗体设计器生成的代码");
            code.AppendLine();
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// 设计器支持所需的方法 - 不要修改");
            code.AppendLine("        /// 此方法的内容。");
            code.AppendLine("        /// </summary>");
            code.AppendLine("        private void InitializeComponent()");
            code.AppendLine("        {");

            // 声明控件
            code.AppendLine("            this.pnlTop = new System.Windows.Forms.Panel();");
            code.AppendLine("            this.lblTitle = new System.Windows.Forms.Label();");
            code.AppendLine("            this.pnlBottom = new System.Windows.Forms.Panel();");
            code.AppendLine("            this.btnCancel = new System.Windows.Forms.Button();");
            code.AppendLine("            this.btnSave = new System.Windows.Forms.Button();");
            code.AppendLine("            this.pnlContent = new System.Windows.Forms.Panel();");
            code.AppendLine("            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();");

            // 获取属性列表，排除主键和CreatedTime等自动生成的属性
            var properties = entityType.GetProperties()
                .Where(p => !p.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true).Any() &&
                           !p.Name.Equals("CreateTime") &&
                           !p.Name.Equals("ModifyTime") &&
                           !p.Name.Equals("Creator") &&
                           !p.Name.Equals("Modifier"))
                .OrderBy(p => p.MetadataToken)
                .ToList();

            // 声明控件变量
            int index = 0;
            foreach (var prop in properties)
            {
                code.AppendLine($"            this.lbl{prop.Name} = new System.Windows.Forms.Label();");

                if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                {
                    code.AppendLine($"            this.chk{prop.Name} = new System.Windows.Forms.CheckBox();");
                }
                else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                {
                    code.AppendLine($"            this.dtp{prop.Name} = new System.Windows.Forms.DateTimePicker();");
                }
                else if (prop.PropertyType.IsEnum || prop.Name.EndsWith("Id"))
                {
                    code.AppendLine($"            this.cmb{prop.Name} = new System.Windows.Forms.ComboBox();");
                }
                else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?) ||
                        prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                {
                    code.AppendLine($"            this.num{prop.Name} = new System.Windows.Forms.NumericUpDown();");
                }
                else
                {
                    code.AppendLine($"            this.txt{prop.Name} = new System.Windows.Forms.TextBox();");
                }

                index++;
            }

            // 开始布局设置
            code.AppendLine("            this.pnlTop.SuspendLayout();");
            code.AppendLine("            this.pnlBottom.SuspendLayout();");
            code.AppendLine("            this.pnlContent.SuspendLayout();");
            code.AppendLine("            this.tableLayoutPanel1.SuspendLayout();");

            // 添加NumericUpDown控件的初始化
            foreach (var prop in properties)
            {
                if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?) ||
                    prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                {
                    code.AppendLine($"            ((System.ComponentModel.ISupportInitialize)(this.num{prop.Name})).BeginInit();");
                }
            }

            code.AppendLine("            this.SuspendLayout();");
            code.AppendLine("            // ");
            code.AppendLine("            // pnlTop");
            code.AppendLine("            // ");
            code.AppendLine("            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);");
            code.AppendLine("            this.pnlTop.Controls.Add(this.lblTitle);");
            code.AppendLine("            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;");
            code.AppendLine("            this.pnlTop.Location = new System.Drawing.Point(0, 0);");
            code.AppendLine("            this.pnlTop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
            code.AppendLine("            this.pnlTop.Name = \"pnlTop\";");
            code.AppendLine("            this.pnlTop.Size = new System.Drawing.Size(700, 71);");
            code.AppendLine("            this.pnlTop.TabIndex = 0;");
            code.AppendLine("            // ");
            code.AppendLine("            // lblTitle");
            code.AppendLine("            // ");
            code.AppendLine("            this.lblTitle.AutoSize = true;");
            code.AppendLine("            this.lblTitle.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);");
            code.AppendLine("            this.lblTitle.ForeColor = System.Drawing.Color.White;");
            code.AppendLine("            this.lblTitle.Location = new System.Drawing.Point(23, 21);");
            code.AppendLine("            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);");
            code.AppendLine("            this.lblTitle.Name = \"lblTitle\";");
            code.AppendLine("            this.lblTitle.Size = new System.Drawing.Size(120, 22);");
            code.AppendLine($"            this.lblTitle.Text = \"编辑{GetDisplayName(entityType)}\";");
            code.AppendLine("            this.lblTitle.TabIndex = 0;");
            code.AppendLine("            // ");
            code.AppendLine("            // pnlBottom");
            code.AppendLine("            // ");
            code.AppendLine("            this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);");
            code.AppendLine("            this.pnlBottom.Controls.Add(this.btnCancel);");
            code.AppendLine("            this.pnlBottom.Controls.Add(this.btnSave);");
            code.AppendLine("            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;");
            code.AppendLine("            this.pnlBottom.Location = new System.Drawing.Point(0, 637);");
            code.AppendLine("            this.pnlBottom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
            code.AppendLine("            this.pnlBottom.Name = \"pnlBottom\";");
            code.AppendLine("            this.pnlBottom.Size = new System.Drawing.Size(700, 85);");
            code.AppendLine("            this.pnlBottom.TabIndex = 1;");
            code.AppendLine("            // ");
            code.AppendLine("            // btnCancel");
            code.AppendLine("            // ");
            code.AppendLine("            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);");
            code.AppendLine("            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(160, 160, 160);");
            code.AppendLine("            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;");
            code.AppendLine("            this.btnCancel.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
            code.AppendLine("            this.btnCancel.Location = new System.Drawing.Point(578, 21);");
            code.AppendLine("            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
            code.AppendLine("            this.btnCancel.Name = \"btnCancel\";");
            code.AppendLine("            this.btnCancel.Size = new System.Drawing.Size(105, 42);");
            code.AppendLine("            this.btnCancel.TabIndex = 1;");
            code.AppendLine("            this.btnCancel.Text = \"取消\";");
            code.AppendLine("            this.btnCancel.UseVisualStyleBackColor = false;");
            code.AppendLine("            // ");
            code.AppendLine("            // btnSave");
            code.AppendLine("            // ");
            code.AppendLine("            this.btnSave.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);");
            code.AppendLine("            this.btnSave.FlatAppearance.BorderSize = 0;");
            code.AppendLine("            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;");
            code.AppendLine("            this.btnSave.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
            code.AppendLine("            this.btnSave.ForeColor = System.Drawing.Color.White;");
            code.AppendLine("            this.btnSave.Location = new System.Drawing.Point(461, 21);");
            code.AppendLine("            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
            code.AppendLine("            this.btnSave.Name = \"btnSave\";");
            code.AppendLine("            this.btnSave.Size = new System.Drawing.Size(105, 42);");
            code.AppendLine("            this.btnSave.TabIndex = 0;");
            code.AppendLine("            this.btnSave.Text = \"保存\";");
            code.AppendLine("            this.btnSave.UseVisualStyleBackColor = false;");
            code.AppendLine("            // ");
            code.AppendLine("            // pnlContent");
            code.AppendLine("            // ");
            code.AppendLine("            this.pnlContent.BackColor = System.Drawing.Color.White;");
            code.AppendLine("            this.pnlContent.Controls.Add(this.tableLayoutPanel1);");
            code.AppendLine("            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;");
            code.AppendLine("            this.pnlContent.Location = new System.Drawing.Point(0, 71);");
            code.AppendLine("            this.pnlContent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
            code.AppendLine("            this.pnlContent.Name = \"pnlContent\";");
            code.AppendLine("            this.pnlContent.Padding = new System.Windows.Forms.Padding(23, 28, 23, 28);");
            code.AppendLine("            this.pnlContent.Size = new System.Drawing.Size(700, 566);");
            code.AppendLine("            this.pnlContent.TabIndex = 2;");
            code.AppendLine("            // ");
            code.AppendLine("            // tableLayoutPanel1");
            code.AppendLine("            // ");
            code.AppendLine("            this.tableLayoutPanel1.ColumnCount = 2;");
            code.AppendLine("            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));");
            code.AppendLine("            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));");

            // 添加控件到TableLayoutPanel
            for (int i = 0; i < properties.Count; i++)
            {
                var prop = properties[i];
                code.AppendLine($"            this.tableLayoutPanel1.Controls.Add(this.lbl{ prop.Name}, 0, { i});");

               
            }
            

            for (int i = 0; i < properties.Count; i++)
            {
                var prop = properties[i];
                string controlPrefix;
                if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                    controlPrefix = "chk";
                else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                    controlPrefix = "dtp";
                else if (prop.PropertyType.IsEnum || prop.Name.EndsWith("Id"))
                    controlPrefix = "cmb";
                else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?) ||
                        prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                    controlPrefix = "num";
                else
                    controlPrefix = "txt";
                code.AppendLine($"            this.tableLayoutPanel1.Controls.Add(this.{controlPrefix}{prop.Name}, 1, {i});");

               
            }

            code.AppendLine("            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;");
            code.AppendLine("            this.tableLayoutPanel1.Location = new System.Drawing.Point(23, 28);");
            code.AppendLine("            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
            code.AppendLine("            this.tableLayoutPanel1.Name = \"tableLayoutPanel1\";");
            code.AppendLine($"            this.tableLayoutPanel1.RowCount = {Math.Max(properties.Count, 1) + 1}; // +1 for extra row ");

            // 设置行高
            for (int i = 0; i < properties.Count; i++)
            {
                code.AppendLine($"            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));");
            }
            code.AppendLine("            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));");

            code.AppendLine("            this.tableLayoutPanel1.Size = new System.Drawing.Size(654, 510);");
            code.AppendLine("            this.tableLayoutPanel1.TabIndex = 0;");

            // 创建输入控件
            for (int i = 0; i < properties.Count; i++)
            {
                var prop = properties[i];
                string displayName = GetDisplayName(prop);
                bool isRequired = prop.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), true).Any();
                string labelText = isRequired ? displayName + "*:" : displayName + ":";

                // 标签配置
                code.AppendLine("            // ");
                code.AppendLine($"            // lbl{prop.Name}");
                code.AppendLine("            // ");
                code.AppendLine($"            this.lbl{prop.Name}.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));");
                code.AppendLine($"            this.lbl{prop.Name}.AutoSize = true;");
                code.AppendLine($"            this.lbl{prop.Name}.Location = new System.Drawing.Point(4, {i * 40 + 11});");
                code.AppendLine($"            this.lbl{prop.Name}.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);");
                code.AppendLine($"            this.lbl{prop.Name}.Name = \"lbl{prop.Name}\";");
                code.AppendLine($"            this.lbl{prop.Name}.Size = new System.Drawing.Size(112, 17);");
                code.AppendLine($"            this.lbl{prop.Name}.TabIndex = {i * 2};");
                code.AppendLine($"            this.lbl{prop.Name}.Text = \"{labelText}\";");
                code.AppendLine($"            this.lbl{prop.Name}.TextAlign = System.Drawing.ContentAlignment.MiddleRight;");

                // 输入控件配置
                if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                {
                    code.AppendLine("            // ");
                    code.AppendLine($"            // chk{prop.Name}");
                    code.AppendLine("            // ");
                    code.AppendLine($"            this.chk{prop.Name}.Anchor = System.Windows.Forms.AnchorStyles.Left;");
                    code.AppendLine($"            this.chk{prop.Name}.AutoSize = true;");
                    code.AppendLine($"            this.chk{prop.Name}.Location = new System.Drawing.Point(124, {i * 40 + 11});");
                    code.AppendLine($"            this.chk{prop.Name}.Margin = new System.Windows.Forms.Padding(4);");
                    code.AppendLine($"            this.chk{prop.Name}.Name = \"chk{prop.Name}\";");
                    code.AppendLine($"            this.chk{prop.Name}.Size = new System.Drawing.Size(51, 21);");
                    code.AppendLine($"            this.chk{prop.Name}.TabIndex = {i * 2 + 1};");
                    code.AppendLine($"            this.chk{prop.Name}.Text = \"\";");
                    code.AppendLine($"            this.chk{prop.Name}.UseVisualStyleBackColor = true;");
                }
                else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                {
                    code.AppendLine("            // ");
                    code.AppendLine($"            // dtp{prop.Name}");
                    code.AppendLine("            // ");
                    code.AppendLine($"            this.dtp{prop.Name}.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));");
                    code.AppendLine($"            this.dtp{prop.Name}.CustomFormat = \"yyyy-MM-dd HH:mm:ss\";");
                    code.AppendLine($"            this.dtp{prop.Name}.Format = System.Windows.Forms.DateTimePickerFormat.Custom;");
                    code.AppendLine($"            this.dtp{prop.Name}.Location = new System.Drawing.Point(124, {i * 40 + 8});");
                    code.AppendLine($"            this.dtp{prop.Name}.Margin = new System.Windows.Forms.Padding(4);");
                    code.AppendLine($"            this.dtp{prop.Name}.Name = \"dtp{prop.Name}\";");
                    code.AppendLine($"            this.dtp{prop.Name}.Size = new System.Drawing.Size(526, 23);");
                    code.AppendLine($"            this.dtp{prop.Name}.TabIndex = {i * 2 + 1};");
                }
                else if (prop.PropertyType.IsEnum || prop.Name.EndsWith("Id"))
                {
                    code.AppendLine("            // ");
                    code.AppendLine($"            // cmb{prop.Name}");
                    code.AppendLine("            // ");
                    code.AppendLine($"            this.cmb{prop.Name}.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));");
                    code.AppendLine($"            this.cmb{prop.Name}.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;");
                    code.AppendLine($"            this.cmb{prop.Name}.FormattingEnabled = true;");
                    code.AppendLine($"            this.cmb{prop.Name}.Location = new System.Drawing.Point(124, {i * 40 + 8});");
                    code.AppendLine($"            this.cmb{prop.Name}.Margin = new System.Windows.Forms.Padding(4);");
                    code.AppendLine($"            this.cmb{prop.Name}.Name = \"cmb{prop.Name}\";");
                    code.AppendLine($"            this.cmb{prop.Name}.Size = new System.Drawing.Size(526, 25);");
                    code.AppendLine($"            this.cmb{prop.Name}.TabIndex = {i * 2 + 1};");
                }
                else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?) ||
                        prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                {
                    code.AppendLine("            // ");
                    code.AppendLine($"            // num{prop.Name}");
                    code.AppendLine("            // ");
                    code.AppendLine($"            this.num{prop.Name}.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));");
                    code.AppendLine($"            this.num{prop.Name}.Location = new System.Drawing.Point(124, {i * 40 + 8});");
                    code.AppendLine($"            this.num{prop.Name}.Margin = new System.Windows.Forms.Padding(4);");
                    code.AppendLine($"            this.num{prop.Name}.Name = \"num{prop.Name}\";");
                    code.AppendLine($"            this.num{prop.Name}.Size = new System.Drawing.Size(526, 23);");
                    code.AppendLine($"            this.num{prop.Name}.TabIndex = {i * 2 + 1};");

                    if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                    {
                        code.AppendLine($"            this.num{prop.Name}.DecimalPlaces = 2;");
                    }
                }
                else
                {
                    code.AppendLine("            // ");
                    code.AppendLine($"            // txt{prop.Name}");
                    code.AppendLine("            // ");
                    code.AppendLine($"            this.txt{prop.Name}.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));");
                    code.AppendLine($"            this.txt{prop.Name}.Location = new System.Drawing.Point(124, {i * 40 + 8});");
                    code.AppendLine($"            this.txt{prop.Name}.Margin = new System.Windows.Forms.Padding(4);");
                    code.AppendLine($"            this.txt{prop.Name}.Name = \"txt{prop.Name}\";");
                    code.AppendLine($"            this.txt{prop.Name}.Size = new System.Drawing.Size(526, 23);");
                    code.AppendLine($"            this.txt{prop.Name}.TabIndex = {i * 2 + 1};");

                    // 判断是否有最大长度限制
                    var maxLengthAttr = prop.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.MaxLengthAttribute), true)
                        .FirstOrDefault() as System.ComponentModel.DataAnnotations.MaxLengthAttribute;
                    if (maxLengthAttr != null && maxLengthAttr.Length > 0)
                    {
                        code.AppendLine($"            this.txt{prop.Name}.MaxLength = {maxLengthAttr.Length};");
                    }
                }
            }

            // 窗体设置
            code.AppendLine("            // ");
            code.AppendLine($"            // {entityName}EditForm");
            code.AppendLine("            // ");
            code.AppendLine("            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);");
            code.AppendLine("            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;");
            code.AppendLine("            this.ClientSize = new System.Drawing.Size(700, 722);");
            code.AppendLine("            this.Controls.Add(this.pnlContent);");
            code.AppendLine("            this.Controls.Add(this.pnlBottom);");
            code.AppendLine("            this.Controls.Add(this.pnlTop);");
            code.AppendLine("            this.Font = new System.Drawing.Font(\"Microsoft YaHei UI\", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);");
            code.AppendLine("            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;");
            code.AppendLine("            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);");
            code.AppendLine("            this.MaximizeBox = false;");
            code.AppendLine("            this.MinimizeBox = false;");
            code.AppendLine($"            this.Name = \"{entityName}EditForm\";");
            code.AppendLine("            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;");
            code.AppendLine("            this.Text = _isNew ? \"新增{GetDisplayName(entityType)}\" : \"编辑{GetDisplayName(entityType)}\";");
            code.AppendLine("            this.pnlTop.ResumeLayout(false);");
            code.AppendLine("            this.pnlTop.PerformLayout();");
            code.AppendLine("            this.pnlBottom.ResumeLayout(false);");
            code.AppendLine("            this.pnlContent.ResumeLayout(false);");
            code.AppendLine("            this.tableLayoutPanel1.ResumeLayout(false);");
            code.AppendLine("            this.tableLayoutPanel1.PerformLayout();");

            // 结束NumericUpDown控件的初始化
            foreach (var prop in properties)
            {
                if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?) ||
                    prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                {
                    code.AppendLine($"            ((System.ComponentModel.ISupportInitialize)(this.num{prop.Name})).EndInit();");
                }
            }

            code.AppendLine("            this.ResumeLayout(false);");
            code.AppendLine();
            code.AppendLine("        }");
            code.AppendLine();
            code.AppendLine("        #endregion");

            // 声明字段
            code.AppendLine("        private System.Windows.Forms.Panel pnlTop;");
            code.AppendLine("        private System.Windows.Forms.Label lblTitle;");
            code.AppendLine("        private System.Windows.Forms.Panel pnlBottom;");
            code.AppendLine("        private System.Windows.Forms.Button btnCancel;");
            code.AppendLine("        private System.Windows.Forms.Button btnSave;");
            code.AppendLine("        private System.Windows.Forms.Panel pnlContent;");
            code.AppendLine("        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;");

            // 声明控件字段
            foreach (var prop in properties)
            {
                code.AppendLine($"        private System.Windows.Forms.Label lbl{prop.Name};");

                if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                {
                    code.AppendLine($"        private System.Windows.Forms.CheckBox chk{prop.Name};");
                }
                else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                {
                    code.AppendLine($"        private System.Windows.Forms.DateTimePicker dtp{prop.Name};");
                }
                else if (prop.PropertyType.IsEnum || prop.Name.EndsWith("Id"))
                {
                    code.AppendLine($"        private System.Windows.Forms.ComboBox cmb{prop.Name};");
                }
                else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?) ||
                        prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                {
                    code.AppendLine($"        private System.Windows.Forms.NumericUpDown num{prop.Name};");
                }
                else
                {
                    code.AppendLine($"        private System.Windows.Forms.TextBox txt{prop.Name};");
                }
            }

            code.AppendLine("    }");
            code.AppendLine("}");

            return code.ToString();
        }
        /// <summary>
        /// 生成全部代码
        /// </summary>
        private void btnGenerateAll_Click(object sender, EventArgs e)
        {
            if (selectedEntityType == null)
            {
                MessageBox.Show("请先选择一个实体类", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // TODO: 实现全部代码生成逻辑
            MessageBox.Show($"即将为 {selectedEntityType.Name} 生成全部代码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 实体属性视图模型
        /// </summary>
        public class EntityPropertyViewModel
        {
            public string PropertyName { get; set; }
            public string PropertyType { get; set; }
            public bool IsKey { get; set; }
            public string DisplayName { get; set; }
            public bool IsRequired { get; set; }
            public int MaxLength { get; set; }
            public bool IsEditable { get; set; }
        }

        /// <summary>
        /// 获取属性控件名称前缀
        /// </summary>
        private string GetControlNameForProperty(PropertyInfo prop)
        {
            if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                return $"chk{prop.Name}";
            else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                return $"dtp{prop.Name}";
            else if (prop.PropertyType.IsEnum || prop.Name.EndsWith("Id"))
                return $"cmb{prop.Name}";
            else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?) ||
                    prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                return $"num{prop.Name}";
            else
                return $"txt{prop.Name}";
        }

        /// <summary>
        /// 生成编辑窗体设计器代码到指定路径
        /// </summary>
        private void GenerateEditFormDesigner(string entityName, string outputDir)
        {
            try
            {
                // 获取当前选中的实体类型
                Type entityType = selectedEntityType;
                if (entityType == null) return;

                // 生成设计器代码
                string designerCode = GenerateEditFormDesigner(entityType);

                // 保存设计器代码文件
                string filePath = Path.Combine(outputDir, $"{entityName}EditForm.Designer.cs");
                File.WriteAllText(filePath, designerCode);

                MessageBox.Show($"{entityName}EditForm.Designer.cs已生成到：\n{filePath}", "生成成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成编辑窗体设计器代码失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}