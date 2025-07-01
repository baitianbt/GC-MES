using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace GC_MES.Common.Export
{
    /// <summary>
    /// Excel导入导出帮助类
    /// </summary>
    public class ExcelHelper
    {
        #region Excel导出

        /// <summary>
        /// 将数据导出到Excel
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据列表</param>
        /// <param name="filePath">保存路径</param>
        /// <param name="sheetName">工作表名称</param>
        /// <param name="columnHeaders">列标题（可选）</param>
        /// <param name="columnProperties">列属性名（可选，与columnHeaders一一对应）</param>
        /// <returns>是否成功</returns>
        public static bool ExportToExcel<T>(List<T> data, string filePath, string sheetName = "Sheet1", 
            string[] columnHeaders = null, string[] columnProperties = null)
        {
            try
            {
                if (data == null || data.Count == 0)
                    return false;

                // 创建工作簿
                IWorkbook workbook;
                if (filePath.EndsWith(".xls"))
                    workbook = new HSSFWorkbook();
                else
                    workbook = new XSSFWorkbook();

                // 创建工作表
                ISheet sheet = workbook.CreateSheet(sheetName);

                // 创建标题行样式
                ICellStyle headerStyle = workbook.CreateCellStyle();
                IFont headerFont = workbook.CreateFont();
                headerFont.IsBold = true;
                headerStyle.SetFont(headerFont);
                headerStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                headerStyle.FillPattern = FillPattern.SolidForeground;

                // 如果没有指定列属性，则获取所有公共属性
                if (columnProperties == null || columnProperties.Length == 0)
                {
                    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    columnProperties = properties.Select(p => p.Name).ToArray();
                    
                    // 如果没有指定列标题，则使用属性名作为标题
                    if (columnHeaders == null || columnHeaders.Length == 0)
                    {
                        columnHeaders = columnProperties;
                    }
                }

                // 创建标题行
                IRow headerRow = sheet.CreateRow(0);
                for (int i = 0; i < columnHeaders.Length; i++)
                {
                    ICell cell = headerRow.CreateCell(i);
                    cell.SetCellValue(columnHeaders[i]);
                    cell.CellStyle = headerStyle;
                }

                // 填充数据行
                int rowIndex = 1;
                foreach (var item in data)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex++);
                    for (int i = 0; i < columnProperties.Length; i++)
                    {
                        var property = typeof(T).GetProperty(columnProperties[i]);
                        if (property != null)
                        {
                            var value = property.GetValue(item);
                            ICell cell = dataRow.CreateCell(i);
                            
                            // 根据属性类型设置单元格值
                            if (value == null)
                            {
                                cell.SetCellValue("");
                            }
                            else if (value is DateTime)
                            {
                                cell.SetCellValue(((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                            else if (value is bool)
                            {
                                cell.SetCellValue((bool)value ? "是" : "否");
                            }
                            else
                            {
                                cell.SetCellValue(value.ToString());
                            }
                        }
                    }
                }

                // 调整列宽
                for (int i = 0; i < columnHeaders.Length; i++)
                {
                    sheet.AutoSizeColumn(i);
                }

                // 保存文件
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fs);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"导出Excel失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 将DataTable导出到Excel
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="filePath">保存路径</param>
        /// <param name="sheetName">工作表名称</param>
        /// <returns>是否成功</returns>
        public static bool ExportToExcel(DataTable dt, string filePath, string sheetName = "Sheet1")
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0)
                    return false;

                // 创建工作簿
                IWorkbook workbook;
                if (filePath.EndsWith(".xls"))
                    workbook = new HSSFWorkbook();
                else
                    workbook = new XSSFWorkbook();

                // 创建工作表
                ISheet sheet = workbook.CreateSheet(sheetName);

                // 创建标题行样式
                ICellStyle headerStyle = workbook.CreateCellStyle();
                IFont headerFont = workbook.CreateFont();
                headerFont.IsBold = true;
                headerStyle.SetFont(headerFont);
                headerStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                headerStyle.FillPattern = FillPattern.SolidForeground;

                // 创建标题行
                IRow headerRow = sheet.CreateRow(0);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ICell cell = headerRow.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                    cell.CellStyle = headerStyle;
                }

                // 填充数据行
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow dataRow = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = dataRow.CreateCell(j);
                        var value = dt.Rows[i][j];
                        
                        // 根据数据类型设置单元格值
                        if (value == null || value == DBNull.Value)
                        {
                            cell.SetCellValue("");
                        }
                        else if (value is DateTime)
                        {
                            cell.SetCellValue(((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else if (value is bool)
                        {
                            cell.SetCellValue((bool)value ? "是" : "否");
                        }
                        else
                        {
                            cell.SetCellValue(value.ToString());
                        }
                    }
                }

                // 调整列宽
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sheet.AutoSizeColumn(i);
                }

                // 保存文件
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fs);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"导出Excel失败: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Excel导入

        /// <summary>
        /// 从Excel导入数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="columnMappings">列映射（Excel列名 -> 属性名）</param>
        /// <param name="startRow">开始行（0为第一行）</param>
        /// <returns>数据列表</returns>
        public static List<T> ImportFromExcel<T>(string filePath, Dictionary<string, string> columnMappings = null, int startRow = 1) where T : new()
        {
            try
            {
                List<T> result = new List<T>();
                
                // 打开Excel文件
                IWorkbook workbook;
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    // 根据文件扩展名选择适当的工作簿类型
                    if (filePath.EndsWith(".xls"))
                        workbook = new HSSFWorkbook(file);
                    else
                        workbook = new XSSFWorkbook(file);
                }

                // 获取第一个工作表
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet == null)
                    return result;

                // 获取标题行
                IRow headerRow = sheet.GetRow(0);
                if (headerRow == null)
                    return result;

                // 解析标题行，建立列索引到属性名的映射
                Dictionary<int, string> columnToProperty = new Dictionary<int, string>();
                for (int i = 0; i < headerRow.LastCellNum; i++)
                {
                    ICell cell = headerRow.GetCell(i);
                    if (cell != null)
                    {
                        string columnName = cell.StringCellValue;
                        if (!string.IsNullOrEmpty(columnName))
                        {
                            // 如果提供了列映射，则使用映射的属性名
                            if (columnMappings != null && columnMappings.ContainsKey(columnName))
                            {
                                columnToProperty[i] = columnMappings[columnName];
                            }
                            else
                            {
                                // 否则尝试直接使用列名作为属性名
                                columnToProperty[i] = columnName;
                            }
                        }
                    }
                }

                // 获取类型的所有属性
                var properties = typeof(T).GetProperties();

                // 读取数据行
                for (int i = startRow; i <= sheet.LastRowNum; i++)
                {
                    IRow dataRow = sheet.GetRow(i);
                    if (dataRow == null)
                        continue;

                    // 创建新对象
                    T item = new T();

                    // 填充对象属性
                    foreach (var colIndex in columnToProperty.Keys)
                    {
                        if (colIndex >= dataRow.LastCellNum)
                            continue;

                        ICell cell = dataRow.GetCell(colIndex);
                        if (cell == null)
                            continue;

                        string propertyName = columnToProperty[colIndex];
                        var property = properties.FirstOrDefault(p => p.Name == propertyName);
                        
                        if (property != null && property.CanWrite)
                        {
                            try
                            {
                                // 根据属性类型和单元格类型设置属性值
                                object value = null;
                                switch (cell.CellType)
                                {
                                    case CellType.Numeric:
                                        if (DateUtil.IsCellDateFormatted(cell))
                                        {
                                            value = cell.DateCellValue;
                                        }
                                        else
                                        {
                                            value = cell.NumericCellValue;
                                        }
                                        break;
                                    case CellType.String:
                                        value = cell.StringCellValue;
                                        break;
                                    case CellType.Boolean:
                                        value = cell.BooleanCellValue;
                                        break;
                                    case CellType.Formula:
                                        switch (cell.CachedFormulaResultType)
                                        {
                                            case CellType.Numeric:
                                                value = cell.NumericCellValue;
                                                break;
                                            case CellType.String:
                                                value = cell.StringCellValue;
                                                break;
                                            default:
                                                value = cell.ToString();
                                                break;
                                        }
                                        break;
                                    default:
                                        value = cell.ToString();
                                        break;
                                }

                                // 转换值类型以匹配属性类型
                                if (value != null)
                                {
                                    Type targetType = property.PropertyType;
                                    
                                    // 处理可空类型
                                    if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                    {
                                        targetType = Nullable.GetUnderlyingType(targetType);
                                    }

                                    // 转换值
                                    if (targetType == typeof(int) || targetType == typeof(int?))
                                    {
                                        property.SetValue(item, Convert.ToInt32(value));
                                    }
                                    else if (targetType == typeof(double) || targetType == typeof(double?))
                                    {
                                        property.SetValue(item, Convert.ToDouble(value));
                                    }
                                    else if (targetType == typeof(decimal) || targetType == typeof(decimal?))
                                    {
                                        property.SetValue(item, Convert.ToDecimal(value));
                                    }
                                    else if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
                                    {
                                        if (value is DateTime)
                                            property.SetValue(item, value);
                                        else
                                            property.SetValue(item, Convert.ToDateTime(value));
                                    }
                                    else if (targetType == typeof(bool) || targetType == typeof(bool?))
                                    {
                                        if (value is bool)
                                            property.SetValue(item, value);
                                        else
                                        {
                                            string strValue = value.ToString().ToLower();
                                            property.SetValue(item, strValue == "true" || strValue == "是" || strValue == "1");
                                        }
                                    }
                                    else if (targetType == typeof(byte) || targetType == typeof(byte?))
                                    {
                                        property.SetValue(item, Convert.ToByte(value));
                                    }
                                    else
                                    {
                                        property.SetValue(item, Convert.ChangeType(value, targetType));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"导入第{i+1}行，属性{propertyName}时出错: {ex.Message}");
                            }
                        }
                    }

                    result.Add(item);
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"从Excel导入数据失败: {ex.Message}");
                return new List<T>();
            }
        }

        /// <summary>
        /// 从Excel导入数据到DataTable
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="hasHeader">是否有标题行</param>
        /// <returns>数据表</returns>
        public static DataTable ImportFromExcel(string filePath, bool hasHeader = true)
        {
            try
            {
                DataTable dt = new DataTable();
                
                // 打开Excel文件
                IWorkbook workbook;
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    // 根据文件扩展名选择适当的工作簿类型
                    if (filePath.EndsWith(".xls"))
                        workbook = new HSSFWorkbook(file);
                    else
                        workbook = new XSSFWorkbook(file);
                }

                // 获取第一个工作表
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet == null)
                    return dt;

                // 获取第一行（可能是标题行）
                IRow firstRow = sheet.GetRow(0);
                if (firstRow == null)
                    return dt;

                // 创建列
                int cellCount = firstRow.LastCellNum;
                for (int i = 0; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn();
                    if (hasHeader)
                    {
                        ICell cell = firstRow.GetCell(i);
                        column.ColumnName = cell?.ToString() ?? $"Column{i}";
                    }
                    else
                    {
                        column.ColumnName = $"Column{i}";
                    }
                    dt.Columns.Add(column);
                }

                // 读取数据行
                int startRow = hasHeader ? 1 : 0;
                for (int i = startRow; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null)
                        continue;

                    DataRow dataRow = dt.NewRow();
                    for (int j = 0; j < cellCount; j++)
                    {
                        ICell cell = row.GetCell(j);
                        if (cell != null)
                        {
                            switch (cell.CellType)
                            {
                                case CellType.Numeric:
                                    if (DateUtil.IsCellDateFormatted(cell))
                                    {
                                        dataRow[j] = cell.DateCellValue;
                                    }
                                    else
                                    {
                                        dataRow[j] = cell.NumericCellValue;
                                    }
                                    break;
                                case CellType.String:
                                    dataRow[j] = cell.StringCellValue;
                                    break;
                                case CellType.Boolean:
                                    dataRow[j] = cell.BooleanCellValue;
                                    break;
                                case CellType.Formula:
                                    switch (cell.CachedFormulaResultType)
                                    {
                                        case CellType.Numeric:
                                            dataRow[j] = cell.NumericCellValue;
                                            break;
                                        case CellType.String:
                                            dataRow[j] = cell.StringCellValue;
                                            break;
                                        default:
                                            dataRow[j] = cell.ToString();
                                            break;
                                    }
                                    break;
                                default:
                                    dataRow[j] = cell.ToString();
                                    break;
                            }
                        }
                    }
                    dt.Rows.Add(dataRow);
                }

                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"从Excel导入数据失败: {ex.Message}");
                return new DataTable();
            }
        }

        #endregion
    }
} 