using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GC_MES.Model
{
    /// <summary>
    /// 库存余额实体类
    /// </summary>
    [Table("Inventory")]
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }
        
        // 关联产品ID
        public int ProductId { get; set; }
        
        // 仓库ID
        public int WarehouseId { get; set; }
        
        // 库位ID
        public int? LocationId { get; set; }
        
        // 批次号
        [StringLength(50)]
        public string BatchNo { get; set; }
        
        // 数量
        public decimal Quantity { get; set; }
        
        // 单位
        [StringLength(20)]
        public string Unit { get; set; }
        
        // 库存状态：正常、冻结、待检验
        [StringLength(20)]
        public string Status { get; set; }
        
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
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
} 