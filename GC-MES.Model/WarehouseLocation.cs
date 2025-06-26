using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GC_MES.Model
{
    /// <summary>
    /// 库位实体类
    /// </summary>
    [Table("WarehouseLocation")]
    public class WarehouseLocation
    {
        [Key]
        public int LocationId { get; set; }
        
        // 所属仓库ID
        public int WarehouseId { get; set; }
        
        // 库位编码
        [Required]
        [StringLength(50)]
        public string LocationCode { get; set; }
        
        // 库位名称
        [Required]
        [StringLength(100)]
        public string LocationName { get; set; }
        
        // 库位类型：普通、冷藏、危险品等
        [StringLength(50)]
        public string LocationType { get; set; }
        
        // 区域
        [StringLength(50)]
        public string Area { get; set; }
        
        // 排
        [StringLength(50)]
        public string Row { get; set; }
        
        // 架
        [StringLength(50)]
        public string Rack { get; set; }
        
        // 层
        [StringLength(50)]
        public string Level { get; set; }
        
        // 是否启用
        public bool IsActive { get; set; }
        
        // 备注
        [StringLength(500)]
        public string Remark { get; set; }
        
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
        [ForeignKey("WarehouseId")]
        public virtual Warehouse Warehouse { get; set; }
    }
} 