﻿/*
 *Author：COCO
 * 此代码由框架生成，请勿随意更改
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GC_MES.Model;


namespace GC_MES.Model.System
{
  
    public class vSys_Dictionary
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="Dic_ID")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int Dic_ID { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="DicValue")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string DicValue { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="DicList_ID")]
       [Column(TypeName="int")]
       public int DicList_ID { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="DicName")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string DicName { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Enable")]
       [Column(TypeName="tinyint")]
       public byte? Enable { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="DicNo")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Required(AllowEmptyStrings=false)]
       public string DicNo { get; set; }

       
    }
}
