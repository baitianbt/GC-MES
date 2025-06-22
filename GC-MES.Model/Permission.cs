using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GC_MES.Model
{
    /// <summary>
    /// 权限实体类
    /// </summary>
    [Table("Permissions")]
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }

        [Required]
        [StringLength(50)]
        public string PermissionName { get; set; }

        [Required]
        [StringLength(100)]
        public string PermissionCode { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public int? ParentId { get; set; }

        [StringLength(50)]
        public string ModuleName { get; set; }

        [StringLength(100)]
        public string ActionUrl { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsMenu { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateTime { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }

        public DateTime? UpdateTime { get; set; }

        [StringLength(50)]
        public string UpdateBy { get; set; }

        // 导航属性
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        
        [ForeignKey("ParentId")]
        public virtual Permission Parent { get; set; }
        
        public virtual ICollection<Permission> Children { get; set; }
    }
} 