using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GC_MES.Model
{
    /// <summary>
    /// 入库单实体类
    /// </summary>
    [Table("InboundOrder")]
    public class InboundOrder
    {
        [Key]
        public int InboundId { get; set; }
        
        // 入库单号
        [Required]
        [StringLength(50)]
        public string InboundCode { get; set; }
        
        // 入库类型：采购入库、生产入库、退货入库、其他入库等
        [Required]
        [StringLength(50)]
        public string InboundType { get; set; }
        
        // 关联业务单号（可选，如采购订单号、生产工单号等）
        [StringLength(50)]
        public string RelatedCode { get; set; }
        
        // 仓库ID
        public int WarehouseId { get; set; }
        
        // 入库日期
        public DateTime InboundDate { get; set; }
        
        // 入库人员
        [StringLength(50)]
        public string Operator { get; set; }
        
        // 入库状态：待入库、部分入库、已完成
        [StringLength(20)]
        public string Status { get; set; }
        
        // 审核状态：未审核、已审核
        [StringLength(20)]
        public string ApprovalStatus { get; set; }
        
        // 审核人
        [StringLength(50)]
        public string Approver { get; set; }
        
        // 审核日期
        public DateTime? ApprovalDate { get; set; }
        
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
        
        public virtual ICollection<InboundOrderItem> InboundItems { get; set; }
    }
} 