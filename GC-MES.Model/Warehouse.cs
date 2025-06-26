using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GC_MES.Model
{
    /// <summary>
    /// 仓库实体类
    /// </summary>
    [Table("Warehouse")]
    public class Warehouse
    {
        [Key]
        public int WarehouseId { get; set; }
        
        // 仓库编码
        [Required]
        [StringLength(50)]
        public string WarehouseCode { get; set; }
        
        // 仓库名称
        [Required]
        [StringLength(100)]
        public string WarehouseName { get; set; }
        
        // 仓库类型：原料仓、成品仓、半成品仓、备件仓等
        [StringLength(50)]
        public string WarehouseType { get; set; }
        
        // 地址
        [StringLength(200)]
        public string Address { get; set; }
        
        // 负责人
        [StringLength(50)]
        public string Manager { get; set; }
        
        // 联系电话
        [StringLength(20)]
        public string ContactPhone { get; set; }
        
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
    }
} 