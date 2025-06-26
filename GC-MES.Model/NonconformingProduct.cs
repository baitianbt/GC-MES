using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GC_MES.Model
{
    /// <summary>
    /// 不合格品处理实体类
    /// </summary>
    [Table("NonconformingProduct")]
    public class NonconformingProduct
    {
        [Key]
        public int NonconformingId { get; set; }
        
        // 不合格品单号
        [Required]
        [StringLength(50)]
        public string NonconformingCode { get; set; }
        
        // 关联的检验单ID
        public int? InspectionId { get; set; }
        
        // 关联的产品ID
        public int ProductId { get; set; }
        
        // 不合格数量
        public decimal Quantity { get; set; }
        
        // 不合格原因
        [StringLength(500)]
        public string DefectReason { get; set; }
        
        // 不合格类型
        [StringLength(50)]
        public string DefectType { get; set; }
        
        // 处理方式：返工、报废、让步接收、退回供应商等
        [StringLength(50)]
        public string DispositionType { get; set; }
        
        // 处理说明
        [StringLength(500)]
        public string DispositionDescription { get; set; }
        
        // 处理状态：待处理、处理中、已处理
        [StringLength(20)]
        public string Status { get; set; }
        
        // 处理日期
        public DateTime? DispositionDate { get; set; }
        
        // 处理人员
        [StringLength(50)]
        public string DispositionBy { get; set; }
        
        // 责任部门
        [StringLength(50)]
        public string ResponsibleDepartment { get; set; }
        
        // 是否需要纠正措施
        public bool RequireCorrectiveAction { get; set; }
        
        // 纠正措施编号
        [StringLength(50)]
        public string CorrectiveActionNo { get; set; }
        
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
        [ForeignKey("InspectionId")]
        public virtual QualityInspection Inspection { get; set; }
        
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
} 