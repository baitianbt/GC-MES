using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GC_MES.Model
{
    /// <summary>
    /// 出库单明细实体类
    /// </summary>
    [Table("OutboundOrderItem")]
    public class OutboundOrderItem
    {
        [Key]
        public int ItemId { get; set; }
        
        // 关联出库单ID
        public int OutboundId { get; set; }
        
        // 产品ID
        public int ProductId { get; set; }
        
        // 批次号
        [StringLength(50)]
        public string BatchNo { get; set; }
        
        // 计划出库数量
        public decimal PlanQuantity { get; set; }
        
        // 实际出库数量
        public decimal ActualQuantity { get; set; }
        
        // 单位
        [StringLength(20)]
        public string Unit { get; set; }
        
        // 库位ID
        public int? LocationId { get; set; }
        
        // 出库状态：待出库、已出库
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
        [ForeignKey("OutboundId")]
        public virtual OutboundOrder OutboundOrder { get; set; }
        
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        
        [ForeignKey("LocationId")]
        public virtual WarehouseLocation Location { get; set; }
    }
} 