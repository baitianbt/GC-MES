using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GC_MES.Model
{
    /// <summary>
    /// 质量检验主表实体类
    /// </summary>
    [Table("QualityInspection")]
    public class QualityInspection
    {
        [Key]
        public int InspectionId { get; set; }
        
        // 检验单号
        [Required]
        [StringLength(50)]
        public string InspectionCode { get; set; }
        
        // 检验类型：IQC(来料检验)、IPQC(过程检验)、FQC(成品检验)
        [Required]
        [StringLength(20)]
        public string InspectionType { get; set; }
        
        // 关联的工单ID（可选）
        public int? WorkOrderId { get; set; }
        
        // 关联的产品ID
        public int ProductId { get; set; }
        
        // 关联的工序ID（可选，过程检验时使用）
        public int? OperationId { get; set; }
        
        // 检验批次
        [StringLength(50)]
        public string BatchNo { get; set; }
        
        // 检验数量
        public decimal Quantity { get; set; }
        
        // 合格数量
        public decimal PassedQuantity { get; set; }
        
        // 不合格数量
        public decimal FailedQuantity { get; set; }
        
        // 检验结果：合格、不合格、让步接收
        [StringLength(20)]
        public string Result { get; set; }
        
        // 检验状态：待检验、检验中、已完成
        [StringLength(20)]
        public string Status { get; set; }
        
        // 检验日期
        public DateTime InspectionDate { get; set; }
        
        // 检验人员
        [StringLength(50)]
        public string Inspector { get; set; }
        
        // 检验备注
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
        
        [ForeignKey("OperationId")]
        public virtual RoutingOperation Operation { get; set; }
        
        public virtual ICollection<QualityInspectionItem> InspectionItems { get; set; }
    }
} 