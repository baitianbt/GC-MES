using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GC_MES.Model
{
    /// <summary>
    /// 用户实体类
    /// </summary>
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(50)]
        public string RealName { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastLoginTime { get; set; }

        public DateTime CreateTime { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }

        public DateTime? UpdateTime { get; set; }

        [StringLength(50)]
        public string UpdateBy { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }

        // 导航属性
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
} 