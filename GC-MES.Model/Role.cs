using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GC_MES.Model
{
    /// <summary>
    /// 角色实体类
    /// </summary>
    [Table("Roles")]
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateTime { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }

        public DateTime? UpdateTime { get; set; }

        [StringLength(50)]
        public string UpdateBy { get; set; }

        // 导航属性
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
} 