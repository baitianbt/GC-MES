using GC_MES.Model.SystemModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GC_MES.DAL.DbContexts
{
    public abstract class BaseDbContext : DbContext
    {
        /// <summary>
        /// 数据库连接字符串，由子类提供
        /// </summary>
        protected abstract string ConnectionString { get; }

        /// <summary>
        /// 构造函数：初始化连接字符串
        /// </summary>
        public BaseDbContext() : base()
        {
        }

        public BaseDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        /// <summary>
        /// 是否启用对象跟踪（EF6 默认启用）
        /// </summary>
        private bool _queryTracking = true;

        public bool QueryTracking
        {
            get { return _queryTracking; }
            set { _queryTracking = value; }
        }

        /// <summary>
        /// 重写 Set<T>() 控制是否启用跟踪
        /// </summary>
        public new DbSet<T> Set<T>() where T : class
        {
            var set = base.Set<T>();
            return set;
        }

        /// <summary>
        /// 手动注册实体类型，约定所有实体都继承指定基类
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="baseEntityType"></param>
        protected void RegisterEntities(DbModelBuilder modelBuilder, Type baseEntityType)
        {
            try
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic && a.FullName.StartsWith("GC-MES")) // 替换为你的项目前缀
                    .ToList();

                foreach (var assembly in assemblies)
                {
                    var entityTypes = assembly.GetTypes()
                        .Where(t => t.IsClass && !t.IsAbstract && t.BaseType == baseEntityType);

                    foreach (var type in entityTypes)
                    {
                        modelBuilder.RegisterEntityType(type);
                    }
                }
            }
            catch (Exception ex)
            {
                var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);
                var filePath = Path.Combine(logPath, $"syslog_{DateTime.Now:yyyyMMddHHmmss}.txt");
                File.WriteAllText(filePath, ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        /// <summary>
        /// EF6 OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            RegisterEntities(modelBuilder, typeof(BaseEntity)); // 你自己的基类
            base.OnModelCreating(modelBuilder);
        }
    }
}
