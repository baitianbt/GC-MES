using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GC_MES.Model
{
    /// <summary>
    /// 工艺工序实体类
    /// </summary>
    [Table("RoutingOperation")]
    public class RoutingOperation
    {
        [Key]
        public int OperationId { get; set; }
        
        // 工艺路线ID
        public int RoutingId { get; set; }
        
        // 工序代码
        [Required]
        [StringLength(50)]
        public string OperationCode { get; set; }
        
        // 工序名称
        [Required]
        [StringLength(100)]
        public string OperationName { get; set; }
        
        // 工作中心
        [StringLength(50)]
        public string WorkCenter { get; set; }
        
        // 工序序号
        public int SequenceNo { get; set; }
        
        // 标准工时（分钟）
        public decimal? StandardTime { get; set; }
        
        // 准备时间（分钟）
        public decimal? SetupTime { get; set; }
        
        // 等待时间（分钟）
        public decimal? WaitTime { get; set; }
        
        // 移动时间（分钟）
        public decimal? MoveTime { get; set; }
        
        // 质检要求
        [StringLength(500)]
        public string QCRequirement { get; set; }
        
        // 工艺描述
        [StringLength(1000)]
        public string ProcessDescription { get; set; }
        
        // 操作指导
        [StringLength(1000)]
        public string OperationInstruction { get; set; }
        
        // 需要质检
        public bool RequireQC { get; set; }
        
        // 是否关键工序
        public bool IsCritical { get; set; }
        
        // 是否外协
        public bool IsOutsourced { get; set; }
        
        // 外协供应商
        [StringLength(100)]
        public string OutsourcedVendor { get; set; }
        
        // 备注
        [StringLength(500)]
        public string Remark { get; set; }
        
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
        [ForeignKey("RoutingId")]
        public virtual ProductRouting Routing { get; set; }
    }
} 