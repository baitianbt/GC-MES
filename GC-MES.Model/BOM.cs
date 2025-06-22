using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GC_MES.Model
{
    /// <summary>
    /// 物料清单(BOM)实体类
    /// </summary>
    [Table("BOM")]
    public class BOM
    {
        [Key]
        public int BOMId { get; set; }
        
        // 父产品ID
        public int ProductId { get; set; }
        
        // 组件产品ID
        public int ComponentId { get; set; }
        
        // 用量
        [Required]
        public decimal Quantity { get; set; }
        
        // 单位
        [StringLength(50)]
        public string Unit { get; set; }
        
        // 位置
        [StringLength(100)]
        public string Position { get; set; }
        
        // 废品率(%)
        public decimal? ScrapRate { get; set; }
        
        // 备注
        [StringLength(500)]
        public string Remark { get; set; }
        
        // BOM版本
        [StringLength(20)]
        public string Version { get; set; }
        
        // BOM层级
        public int Level { get; set; }
        
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
        public virtual Product ParentProduct { get; set; }
        
        [ForeignKey("ComponentId")]
        public virtual Product ComponentProduct { get; set; }
    }
} 