using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GC_MES.Model;

namespace GC_MES.DAL.DbContexts
{
    public class MesDbContext : DbContext
    {
        public MesDbContext() : base("name=DbConnectionString")
        {
            // 配置EF以禁用延迟加载
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        
        // 产品管理相关
        public DbSet<Product> Products { get; set; }
        public DbSet<BOM> BOMs { get; set; }
        public DbSet<ProductRouting> ProductRoutings { get; set; }
        public DbSet<RoutingOperation> RoutingOperations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 配置实体关系
            modelBuilder.Entity<UserRole>()
                .HasRequired(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasRequired(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasRequired(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasRequired(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);

            // 处理自引用关系
            modelBuilder.Entity<Permission>()
                .HasOptional(p => p.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(p => p.ParentId);
                
            // 配置产品和BOM关系
            modelBuilder.Entity<BOM>()
                .HasRequired(b => b.ParentProduct)
                .WithMany(p => p.BOMs)
                .HasForeignKey(b => b.ProductId)
                .WillCascadeOnDelete(false);
                
            modelBuilder.Entity<BOM>()
                .HasRequired(b => b.ComponentProduct)
                .WithMany()
                .HasForeignKey(b => b.ComponentId)
                .WillCascadeOnDelete(false);
                
            // 配置产品和工艺路线的关系
            modelBuilder.Entity<ProductRouting>()
                .HasRequired(r => r.Product)
                .WithMany(p => p.Routings)
                .HasForeignKey(r => r.ProductId);
                
            // 配置工艺路线和工序的关系
            modelBuilder.Entity<RoutingOperation>()
                .HasRequired(o => o.Routing)
                .WithMany(r => r.Operations)
                .HasForeignKey(o => o.RoutingId);
        }
    }
}
