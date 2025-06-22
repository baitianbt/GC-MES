using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GC_MES.Model
{
    /// <summary>
    /// 产品工艺路线实体类
    /// </summary>
    [Table("ProductRouting")]
    public class ProductRouting
    {
        [Key]
        public int RoutingId { get; set; }
        
        // 产品ID
        public int ProductId { get; set; }
        
        // 工艺路线名称
        [Required]
        [StringLength(100)]
        public string RoutingName { get; set; }
        
        // 版本号
        [StringLength(20)]
        public string Version { get; set; }
        
        // 是否默认工艺路线
        public bool IsDefault { get; set; }
        
        // 描述
        [StringLength(500)]
        public string Description { get; set; }
        
        // 备注
        [StringLength(500)]
        public string Remark { get; set; }
        
        // 是否启用
        public bool IsActive { get; set; }
        
        // 创建者
        [StringLength(50)]
        public string CreateBy { get; set; }
        
        // 创建时间
        public DateTime CreateTime { get; set; }
        
        // 更新者
        [StringLength(50)]
        public string UpdateBy { get; set; }
        
        // 更新时间
        public DateTime? UpdateTime { get; set; }
        
        // 导航属性
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        
        public virtual ICollection<RoutingOperation> Operations { get; set; }
    }
} 