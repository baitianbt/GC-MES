using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GC_MES.Model
{
    /// <summary>
    /// 库存事务实体类（库存收发记录）
    /// </summary>
    [Table("InventoryTransaction")]
    public class InventoryTransaction
    {
        [Key]
        public int TransactionId { get; set; }
        
        // 事务类型：入库、出库、调拨、盘点等
        [Required]
        [StringLength(20)]
        public string TransactionType { get; set; }
        
        // 关联单据类型：入库单、出库单、调拨单、盘点单等
        [StringLength(50)]
        public string DocumentType { get; set; }
        
        // 关联单据编号
        [StringLength(50)]
        public string DocumentCode { get; set; }
        
        // 关联单据明细ID（可选）
        public int? DocumentItemId { get; set; }
        
        // 产品ID
        public int ProductId { get; set; }
        
        // 仓库ID
        public int WarehouseId { get; set; }
        
        // 库位ID（可选）
        public int? LocationId { get; set; }
        
        // 批次号
        [StringLength(50)]
        public string BatchNo { get; set; }
        
        // 变动前数量
        public decimal BeforeQuantity { get; set; }
        
        // 变动数量（正数为增加，负数为减少）
        public decimal TransactionQuantity { get; set; }
        
        // 变动后数量
        public decimal AfterQuantity { get; set; }
        
        // 单位
        [StringLength(20)]
        public string Unit { get; set; }
        
        // 交易日期时间
        public DateTime TransactionDate { get; set; }
        
        // 操作人
        [StringLength(50)]
        public string Operator { get; set; }
        
        // 备注
        [StringLength(500)]
        public string Remark { get; set; }
        
        // 创建者
        [StringLength(50)]
        public string CreateBy { get; set; }
        
        // 创建时间
        public DateTime CreateTime { get; set; }
        
        // 导航属性
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        
        [ForeignKey("WarehouseId")]
        public virtual Warehouse Warehouse { get; set; }
        
        [ForeignKey("LocationId")]
        public virtual WarehouseLocation Location { get; set; }
    }
} 