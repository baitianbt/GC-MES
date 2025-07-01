using GC_MES.DAL;
using GC_MES.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GC_MES.WinForm.Common
{
    public class CodeGeneratorHelper
    {

        #region 生成Repository
        /// <summary>
        /// 生成IRepository代码
        /// </summary>
        private static void  GenerateIRepository(string entityName, string namespaceName,string outputDir)
        {


            StringBuilder code = new StringBuilder();


            // 添加命名空间
            code.AppendLine($"using {namespaceName}.Model;");
            code.AppendLine($"using {namespaceName}.DAL;");
            code.AppendLine("using System;");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine("using System.Linq;");
            code.AppendLine("using System.Text;");
            code.AppendLine("using System.Threading.Tasks;");
            code.AppendLine();

            // 添加命名空间声明
            code.AppendLine($"namespace {namespaceName}.DAL.IRepository");
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

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            File.WriteAllText(Path.Combine(outputDir, $"I{entityName}Repository.cs"), code.ToString());
        }

        /// <summary>
        /// 生成Repository代码
        /// </summary>
        private static void GenerateRepository(string entityName, string namespaceName, string outputDir)
        {
            

            StringBuilder code = new StringBuilder();
          
            // 添加命名空间
            code.AppendLine($"using {namespaceName}.DAL;");
            code.AppendLine($"using {namespaceName};");
            code.AppendLine("using System;");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine("using System.Linq;");
            code.AppendLine("using System.Text;");
            code.AppendLine("using System.Threading.Tasks;");
            code.AppendLine();

            // 添加命名空间声明
            code.AppendLine($"namespace {namespaceName}.DAL.Repository");
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

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            File.WriteAllText(Path.Combine(outputDir, $"I{entityName}Repository.cs"), code.ToString());
        }




        #endregion

        #region 生成Service
        /// <summary>
        /// 生成IService代码
        /// </summary>
        private void GenerateIService(string entityName, string namespaceName, string outputDir)
        {
         

            StringBuilder code = new StringBuilder();
         

            // 添加命名空间
            code.AppendLine($"using {namespaceName}.BLL;");
            code.AppendLine($"using {namespaceName};");
            code.AppendLine("using System;");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine("using System.Linq;");
            code.AppendLine("using System.Text;");
            code.AppendLine("using System.Threading.Tasks;");
            code.AppendLine();

            // 添加命名空间声明
            code.AppendLine($"namespace {namespaceName}.BLL.IService");
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

            if (!Directory.Exists(outputDir)) 
                Directory.CreateDirectory(outputDir);

            File.WriteAllText(Path.Combine(outputDir, $"I{entityName}Service.cs"), code.ToString());

        }

        /// <summary>
        /// 生成Service代码
        /// </summary>
        private void GenerateService(string entityName, string namespaceName, string outputDir)
        {
            

            StringBuilder code = new StringBuilder();


            // 添加命名空间
            code.AppendLine($"using {namespaceName}.BLL;");
            code.AppendLine($"using {namespaceName};");
            code.AppendLine("using GC_MES.DAL;");
            code.AppendLine("using GC_MES.DAL.System.IRepository;");
            code.AppendLine("using System;");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine("using System.Linq;");
            code.AppendLine("using System.Text;");
            code.AppendLine("using System.Threading.Tasks;");
            code.AppendLine();

            // 添加命名空间声明
            code.AppendLine($"namespace {namespaceName}.BLL.Service");
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

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            File.WriteAllText(Path.Combine(outputDir, $"{entityName}Service.cs"), code.ToString());
        }
        #endregion




        #region 生成UI界面

        public void GenerateForm(string entityName, string namespaceName, string outputDir)
        {
            StringBuilder code = new StringBuilder();
            // 添加命名空间
            code.AppendLine($"using {namespaceName}.Model;");
            code.AppendLine($"using {namespaceName}.BLL.IService;");
            code.AppendLine("using System;");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine("using System.Linq;");
            code.AppendLine("using System.Text;");
            code.AppendLine("using System.Threading.Tasks;");
            code.AppendLine("using System.Windows.Forms;");
            code.AppendLine();
            // 添加命名空间声明
            code.AppendLine($"namespace {namespaceName}.WinForm.Forms.SystemForm.SubForm");
            code.AppendLine("{");
            // 添加类声明
            code.AppendLine($"    public partial class {entityName}EditForm : Form");
            code.AppendLine("    {");
            // 添加私有字段和构造函数等代码
            // ...
            // 结束类定义
            code.AppendLine("    }");
            code.AppendLine("}");
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);
            File.WriteAllText(Path.Combine(outputDir, $"{entityName}EditForm.cs"), code.ToString());
        }
        public void GenerateFormDesign(string entityName, string namespaceName, string outputDir)
        {
            StringBuilder code = new StringBuilder();
            // 添加命名空间
            code.AppendLine($"using {namespaceName}.Model;");
            code.AppendLine($"using {namespaceName}.BLL.IService;");
            code.AppendLine("using System;");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine("using System.Linq;");
            code.AppendLine("using System.Text;");
            code.AppendLine("using System.Threading.Tasks;");
            code.AppendLine("using System.Windows.Forms;");
            code.AppendLine();
            // 添加命名空间声明
            code.AppendLine($"namespace {namespaceName}.WinForm.Forms.SystemForm.SubForm");
            code.AppendLine("{");
            // 添加类声明
            code.AppendLine($"    public partial class {entityName}EditForm : Form");
            code.AppendLine("    {");
            // 添加私有字段和构造函数等代码
            // ...
            // 结束类定义
            code.AppendLine("    }");
            code.AppendLine("}");
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);
            File.WriteAllText(Path.Combine(outputDir, $"{entityName}EditForm.cs"), code.ToString());
        }
        #endregion
    }
}
