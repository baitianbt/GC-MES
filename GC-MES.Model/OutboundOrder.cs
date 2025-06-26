using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GC_MES.Model
{
    /// <summary>
    /// 出库单实体类
    /// </summary>
    [Table("OutboundOrder")]
    public class OutboundOrder
    {
        [Key]
        public int OutboundId { get; set; }
        
        // 出库单号
        [Required]
        [StringLength(50)]
        public string OutboundCode { get; set; }
        
        // 出库类型：生产领料、销售出库、其他出库等
        [Required]
        [StringLength(50)]
        public string OutboundType { get; set; }
        
        // 关联业务单号（可选，如销售订单号、生产工单号等）
        [StringLength(50)]
        public string RelatedCode { get; set; }
        
        // 仓库ID
        public int WarehouseId { get; set; }
        
        // 出库日期
        public DateTime OutboundDate { get; set; }
        
        // 出库人员
        [StringLength(50)]
        public string Operator { get; set; }
        
        // 领用部门
        [StringLength(50)]
        public string Department { get; set; }
        
        // 领用人
        [StringLength(50)]
        public string Recipient { get; set; }
        
        // 出库状态：待出库、部分出库、已完成
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
        
        public virtual ICollection<OutboundOrderItem> OutboundItems { get; set; }
    }
} 