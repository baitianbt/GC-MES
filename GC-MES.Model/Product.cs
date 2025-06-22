using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GC_MES.Model
{
    /// <summary>
    /// 产品实体类
    /// </summary>
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string ProductCode { get; set; }
        
        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }
        
        [StringLength(200)]
        public string Description { get; set; }
        
        [StringLength(100)]
        public string Specification { get; set; }
        
        [StringLength(50)]
        public string Unit { get; set; }
        
        // 类别：原材料、半成品、成品等
        [StringLength(50)]
        public string Category { get; set; }
        
        // 默认仓库
        [StringLength(50)]
        public string DefaultWarehouse { get; set; }
        
        // 默认库位
        [StringLength(50)]
        public string DefaultLocation { get; set; }
        
        // 保质期(天数)
        public int? ShelfLife { get; set; }
        
        // 预警库存
        public decimal? MinStock { get; set; }
        
        // 最大库存
        public decimal? MaxStock { get; set; }
        
        // 标准成本
        public decimal? StandardCost { get; set; }
        
        // 销售价格
        public decimal? SalePrice { get; set; }
        
        // 图片路径
        [StringLength(255)]
        public string ImagePath { get; set; }
        
        // 产品类型: 自制、外购、委外等
        [StringLength(50)]
        public string ProductType { get; set; }
        
        // 物料编码
        [StringLength(50)]
        public string MaterialCode { get; set; }
        
        // 条形码
        [StringLength(100)]
        public string Barcode { get; set; }
        
        // 版本号
        [StringLength(20)]
        public string Version { get; set; }
        
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
        public virtual ICollection<BOM> BOMs { get; set; }
        public virtual ICollection<ProductRouting> Routings { get; set; }
    }
} 