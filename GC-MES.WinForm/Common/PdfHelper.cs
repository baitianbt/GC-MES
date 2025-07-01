using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Font = iTextSharp.text.Font;

namespace GC_MES.WinForm.Common
{
    /// <summary>
    /// PDF导出帮助类
    /// </summary>
    public static class PdfHelper
    {
        private static readonly string DefaultChineseFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "simsun.ttc,0");

        /// <summary>
        /// 导出 List 数据到 PDF
        /// </summary>
        public static bool ExportToPdf<T>(List<T> data, string filePath, string title,
            string[] columnHeaders, string[] columnProperties, float[] columnWidths = null)
        {
            if (data == null || data.Count == 0 || columnHeaders == null || columnProperties == null)
                return false;

            try
            {
                DataTable dt = ConvertToDataTable(data, columnHeaders, columnProperties);
                return ExportToPdf(dt, filePath, title, columnWidths);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"导出PDF失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 导出 DataTable 到 PDF
        /// </summary>
        public static bool ExportToPdf(DataTable dt, string filePath, string title, float[] columnWidths = null)
        {
            if (dt == null || dt.Rows.Count == 0)
                return false;

            try
            {
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    Document document = new Document(PageSize.A4, 36f, 36f, 36f, 36f);
                    PdfWriter.GetInstance(document, fs);
                    document.Open();

                    // 加载中文字体
                    BaseFont baseFont = BaseFont.CreateFont(DefaultChineseFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    Font titleFont = new Font(baseFont, 16, Font.BOLD);
                    Font normalFont = new Font(baseFont, 10);
                    Font headerFont = new Font(baseFont, 10, Font.BOLD);
                    Font cellFont = new Font(baseFont, 9);

                    // 标题
                    document.Add(new Paragraph(title, titleFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 20f
                    });

                    // 导出时间
                    document.Add(new Paragraph($"导出时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}", normalFont)
                    {
                        Alignment = Element.ALIGN_RIGHT,
                        SpacingAfter = 20f
                    });

                    PdfPTable table = new PdfPTable(dt.Columns.Count)
                    {
                        WidthPercentage = 100
                    };

                    if (columnWidths != null && columnWidths.Length == dt.Columns.Count)
                        table.SetWidths(columnWidths);

                    // 表头
                    foreach (DataColumn col in dt.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(col.ColumnName, headerFont))
                        {
                            BackgroundColor = new BaseColor(220, 220, 220),
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            Padding = 5f
                        };
                        table.AddCell(cell);
                    }

                    // 数据
                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (object item in row.ItemArray)
                        {
                            string text = item switch
                            {
                                DBNull => "",
                                DateTime dtVal => dtVal.ToString("yyyy-MM-dd HH:mm:ss"),
                                bool boolVal => boolVal ? "是" : "否",
                                _ => item.ToString()
                            };

                            PdfPCell cell = new PdfPCell(new Phrase(text, cellFont))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                VerticalAlignment = Element.ALIGN_MIDDLE,
                                Padding = 4f
                            };
                            table.AddCell(cell);
                        }
                    }

                    document.Add(table);

                    // 页脚
                    document.Add(new Paragraph($"共 {dt.Rows.Count} 条记录", normalFont)
                    {
                        Alignment = Element.ALIGN_RIGHT,
                        SpacingBefore = 20f
                    });

                    document.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"导出PDF失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 将 List 转换为 DataTable（用于统一处理）
        /// </summary>
        private static DataTable ConvertToDataTable<T>(List<T> data, string[] columnHeaders, string[] columnProperties)
        {
            DataTable table = new DataTable();

            for (int i = 0; i < columnHeaders.Length; i++)
            {
                table.Columns.Add(columnHeaders[i]);
            }

            foreach (var item in data)
            {
                DataRow row = table.NewRow();
                for (int i = 0; i < columnProperties.Length; i++)
                {
                    PropertyInfo prop = typeof(T).GetProperty(columnProperties[i]);
                    if (prop != null)
                    {
                        row[i] = prop.GetValue(item) ?? DBNull.Value;
                    }
                }
                table.Rows.Add(row);
            }

            return table;
        }
    }
}
