using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GC_MES.Common.Export
{
    /// <summary>
    /// PDF导出帮助类
    /// </summary>
    public class PdfHelper
    {
        #region PDF导出

        /// <summary>
        /// 导出数据到PDF
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据列表</param>
        /// <param name="filePath">保存路径</param>
        /// <param name="title">文档标题</param>
        /// <param name="columnHeaders">列标题</param>
        /// <param name="columnProperties">列属性名（与columnHeaders一一对应）</param>
        /// <param name="columnWidths">列宽度比例（可选）</param>
        /// <returns>是否成功</returns>
        public static bool ExportToPdf<T>(List<T> data, string filePath, string title,
            string[] columnHeaders, string[] columnProperties, float[] columnWidths = null)
        {
            try
            {
                if (data == null || data.Count == 0 || columnHeaders == null || columnProperties == null)
                    return false;

                // 创建文档
                Document document = new Document(PageSize.A4, 36f, 36f, 36f, 36f);
                
                // 创建PDF写入器
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                
                // 打开文档
                document.Open();
                
                // 添加标题
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                Paragraph titleParagraph = new Paragraph(title, titleFont);
                titleParagraph.Alignment = Element.ALIGN_CENTER;
                titleParagraph.SpacingAfter = 20f;
                document.Add(titleParagraph);
                
                // 添加导出时间
                Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                Paragraph exportTime = new Paragraph($"导出时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}", normalFont);
                exportTime.Alignment = Element.ALIGN_RIGHT;
                exportTime.SpacingAfter = 20f;
                document.Add(exportTime);
                
                // 创建表格
                PdfPTable table = new PdfPTable(columnHeaders.Length);
                table.WidthPercentage = 100;
                
                // 设置列宽
                if (columnWidths != null && columnWidths.Length == columnHeaders.Length)
                {
                    table.SetWidths(columnWidths);
                }
                
                // 表格标题样式
                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                BaseColor headerBgColor = new BaseColor(220, 220, 220);
                
                // 添加表头
                foreach (var header in columnHeaders)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header, headerFont));
                    cell.BackgroundColor = headerBgColor;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.Padding = 5f;
                    table.AddCell(cell);
                }
                
                // 添加数据行
                Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                foreach (var item in data)
                {
                    for (int i = 0; i < columnProperties.Length; i++)
                    {
                        var property = typeof(T).GetProperty(columnProperties[i]);
                        if (property != null)
                        {
                            var value = property.GetValue(item);
                            string displayValue = "";
                            
                            if (value != null)
                            {
                                if (value is DateTime)
                                {
                                    displayValue = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else if (value is bool)
                                {
                                    displayValue = (bool)value ? "是" : "否";
                                }
                                else
                                {
                                    displayValue = value.ToString();
                                }
                            }
                            
                            PdfPCell cell = new PdfPCell(new Phrase(displayValue, cellFont));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.Padding = 4f;
                            table.AddCell(cell);
                        }
                    }
                }
                
                // 添加表格到文档
                document.Add(table);
                
                // 添加页脚
                Paragraph footer = new Paragraph($"共 {data.Count} 条记录", normalFont);
                footer.Alignment = Element.ALIGN_RIGHT;
                footer.SpacingBefore = 20f;
                document.Add(footer);
                
                // 关闭文档
                document.Close();
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"导出PDF失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 导出DataTable到PDF
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="filePath">保存路径</param>
        /// <param name="title">文档标题</param>
        /// <param name="columnWidths">列宽度比例（可选）</param>
        /// <returns>是否成功</returns>
        public static bool ExportToPdf(DataTable dt, string filePath, string title, float[] columnWidths = null)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0)
                    return false;

                // 创建文档
                Document document = new Document(PageSize.A4, 36f, 36f, 36f, 36f);
                
                // 创建PDF写入器
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                
                // 打开文档
                document.Open();
                
                // 添加标题
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                Paragraph titleParagraph = new Paragraph(title, titleFont);
                titleParagraph.Alignment = Element.ALIGN_CENTER;
                titleParagraph.SpacingAfter = 20f;
                document.Add(titleParagraph);
                
                // 添加导出时间
                Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                Paragraph exportTime = new Paragraph($"导出时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}", normalFont);
                exportTime.Alignment = Element.ALIGN_RIGHT;
                exportTime.SpacingAfter = 20f;
                document.Add(exportTime);
                
                // 创建表格
                PdfPTable table = new PdfPTable(dt.Columns.Count);
                table.WidthPercentage = 100;
                
                // 设置列宽
                if (columnWidths != null && columnWidths.Length == dt.Columns.Count)
                {
                    table.SetWidths(columnWidths);
                }
                
                // 表格标题样式
                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                BaseColor headerBgColor = new BaseColor(220, 220, 220);
                
                // 添加表头
                foreach (DataColumn column in dt.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, headerFont));
                    cell.BackgroundColor = headerBgColor;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.Padding = 5f;
                    table.AddCell(cell);
                }
                
                // 添加数据行
                Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                foreach (DataRow row in dt.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        string displayValue = "";
                        
                        if (item != null && item != DBNull.Value)
                        {
                            if (item is DateTime)
                            {
                                displayValue = ((DateTime)item).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else if (item is bool)
                            {
                                displayValue = (bool)item ? "是" : "否";
                            }
                            else
                            {
                                displayValue = item.ToString();
                            }
                        }
                        
                        PdfPCell cell = new PdfPCell(new Phrase(displayValue, cellFont));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.Padding = 4f;
                        table.AddCell(cell);
                    }
                }
                
                // 添加表格到文档
                document.Add(table);
                
                // 添加页脚
                Paragraph footer = new Paragraph($"共 {dt.Rows.Count} 条记录", normalFont);
                footer.Alignment = Element.ALIGN_RIGHT;
                footer.SpacingBefore = 20f;
                document.Add(footer);
                
                // 关闭文档
                document.Close();
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"导出PDF失败: {ex.Message}");
                return false;
            }
        }

        #endregion
    }
} 