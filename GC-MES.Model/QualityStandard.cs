using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GC_MES.Model
{
    /// <summary>
    /// 质量检验标准模板实体类
    /// </summary>
    [Table("QualityStandard")]
    public class QualityStandard
    {
        [Key]
        public int StandardId { get; set; }
        
        // 标准编号
        [Required]
        [StringLength(50)]
        public string StandardCode { get; set; }
        
        // 标准名称
        [Required]
        [StringLength(100)]
        public string StandardName { get; set; }
        
        // 适用产品ID（可选，如果为空则为通用标准）
        public int? ProductId { get; set; }
        
        // 适用工序ID（可选）
        public int? OperationId { get; set; }
        
        // 检验类型：IQC(来料检验)、IPQC(过程检验)、FQC(成品检验)
        [Required]
        [StringLength(20)]
        public string InspectionType { get; set; }
        
        // 版本号
        [StringLength(20)]
        public string Version { get; set; }
        
        // 描述
        [StringLength(500)]
        public string Description { get; set; }
        
        // AQL值（接收质量限）
        public decimal? AQL { get; set; }
        
        // 抽样方案
        [StringLength(100)]
        public string SamplingPlan { get; set; }
        
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
        
        [ForeignKey("OperationId")]
        public virtual RoutingOperation Operation { get; set; }
        
        public virtual ICollection<QualityStandardItem> StandardItems { get; set; }
    }
} 