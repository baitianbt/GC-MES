﻿using Newtonsoft.Json;
using SqlSugar;


namespace GC_MES.Model.System
{
    [Table("Sys_Role")]

    public class Sys_Role 
    {
        /// <summary>
        ///Id
        /// </summary>
        [Key]
        [Display(Name = "Id")]
        [Column(TypeName = "int")]
        [Required(AllowEmptyStrings = false)]
        public int Role_Id { get; set; }

        /// <summary>
        ///父级ID
        /// </summary>
        [Display(Name = "父级ID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int ParentId { get; set; }

        /// <summary>
        ///角色名称
        /// </summary>
        [Display(Name = "角色名称")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string RoleName { get; set; }

        /// <summary>
        ///部门ID
        /// </summary>
        [Display(Name = "部门ID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? Dept_Id { get; set; }

        /// <summary>
        ///部门名称
        /// </summary>
        [Display(Name = "部门名称")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string DeptName { get; set; }

        /// <summary>
        ///排序
        /// </summary>
        [Display(Name = "排序")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? OrderNo { get; set; }

        /// <summary>
        ///创建人
        /// </summary>
        [Display(Name = "创建人")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string Creator { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        ///修改人
        /// </summary>
        [Display(Name = "修改人")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string Modifier { get; set; }

        /// <summary>
        ///修改时间
        /// </summary>
        [Display(Name = "修改时间")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "DeleteBy")]
        [MaxLength(50)]
        [JsonIgnore]
        [Column(TypeName = "nvarchar(50)")]
        public string DeleteBy { get; set; }

        /// <summary>
        ///是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        public byte? Enable { get; set; }


        [SugarColumn(IsIgnore = true)]
        [ForeignKey("Role_Id")]
        public List<Sys_RoleAuth> RoleAuths { get; set; }

    }
}

