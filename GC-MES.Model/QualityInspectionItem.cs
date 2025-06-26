using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GC_MES.Model
{
    /// <summary>
    /// 质量检验项目明细表实体类
    /// </summary>
    [Table("QualityInspectionItem")]
    public class QualityInspectionItem
    {
        [Key]
        public int ItemId { get; set; }
        
        // 关联的检验单ID
        public int InspectionId { get; set; }
        
        // 检验项目编号
        [Required]
        [StringLength(50)]
        public string ItemCode { get; set; }
        
        // 检验项目名称
        [Required]
        [StringLength(100)]
        public string ItemName { get; set; }
        
        // 检验方法
        [StringLength(200)]
        public string InspectionMethod { get; set; }
        
        // 检验工具
        [StringLength(100)]
        public string InspectionTool { get; set; }
        
        // 标准值
        [StringLength(100)]
        public string StandardValue { get; set; }
        
        // 上限值
        public decimal? UpperLimit { get; set; }
        
        // 下限值
        public decimal? LowerLimit { get; set; }
        
        // 单位
        [StringLength(20)]
        public string Unit { get; set; }
        
        // 实测值
        [StringLength(100)]
        public string ActualValue { get; set; }
        
        // 检验结果：合格、不合格
        [StringLength(20)]
        public string Result { get; set; }
        
        // 不合格原因
        [StringLength(200)]
        public string DefectReason { get; set; }
        
        // 不合格类型
        [StringLength(50)]
        public string DefectType { get; set; }
        
        // 检验顺序
        public int SequenceNo { get; set; }
        
        // 是否关键项目
        public bool IsCritical { get; set; }
        
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
    }
} 