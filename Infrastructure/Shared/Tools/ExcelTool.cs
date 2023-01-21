using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Tools;

public static class ExcelTool
{
    private static readonly Dictionary<string, string> ColumnFormats = new Dictionary<string, string>
            {
                            { "Created", "yyyy-mm-dd HH:mm:ss"},
                            { "Updated", "yyyy-mm-dd HH:mm:ss"},
            };

    public static List<DataTable> ReadExcelFileContent(byte[] content)
    {
        var res = new List<DataTable>();

        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        using (var stream = new MemoryStream(content))
        using (var excel = new ExcelPackage(stream))
        {
            foreach (var ws in excel.Workbook.Worksheets)
            {
                var dt = new DataTable();

                int colCount = ws.Dimension.End.Column;
                int rowCount = ws.Dimension.End.Row;
                var columns = new Dictionary<int, string>();

                for (int col = 1; col <= colCount; col++)
                {
                    var column = (ws.Cells[1, col].Value ?? string.Empty).ToString().Trim();
                    if (!string.IsNullOrEmpty(column))
                    {
                        if (!dt.Columns.Contains(column))
                        {
                            columns.Add(col, column);
                            dt.Columns.Add(column);
                        }
                    }
                }

                for (int row = 2; row <= rowCount; row++)
                {
                    var newrow = dt.NewRow();
                    for (int col = 1; col <= colCount; col++)
                    {
                        if (columns.TryGetValue(col, out string column))
                        {
                            newrow[column] = (ws.Cells[row, col].Value ?? string.Empty).ToString().Trim();
                        }
                    }
                    dt.Rows.Add(newrow);
                }
                dt.AcceptChanges();

                res.Add(dt);
            }
        }

        return res;
    }
    public static ExcelWorksheet ToExcelWorksheet<T>(IEnumerable<T> items, string sheetName = "NewSheet")
    {
        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        var pck = new ExcelPackage();
        var ws = pck.Workbook.Worksheets.Add(sheetName);
        var orderedProperties = (from property in typeof(T).GetProperties()
                                 where Attribute.IsDefined(property, typeof(DisplayAttribute))
                                 orderby ((DisplayAttribute)property
                                          .GetCustomAttributes(typeof(DisplayAttribute), false)
                                          .Single()).Order
                                 select property);

        ws.Cells["A1"].LoadFromCollection(
            Collection: items, PrintHeaders: true, TableStyle: TableStyles.Light1,
            memberFlags: System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
            Members: orderedProperties.ToArray());
        return ws;
    }

    public static ExcelPackage ToExcelWorksheet(ExcelPackage pck, DataTable dt, string sheetName)
    {
        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

        var ws = pck.Workbook.Worksheets.Add(sheetName);
        FillInfo(ws, dt);

        return pck;
    }

    public static ExcelPackage NewExcel()
    {
        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        var pck = new ExcelPackage();

        return pck;
    }

    public static async Task<byte[]> ToExcelAsync(ExcelPackage pck)
    {
        return await pck.GetAsByteArrayAsync();
    }

    public static async Task<byte[]> ToExcelAsync<T>(IEnumerable<T> items)
    {
        byte[] excel;
        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        using (var pck = new ExcelPackage())
        {
            var ws = pck.Workbook.Worksheets.Add("Data");
            var orderedProperties = (from property in typeof(T).GetProperties()
                                     where Attribute.IsDefined(property, typeof(DisplayAttribute))
                                     orderby ((DisplayAttribute)property
                                              .GetCustomAttributes(typeof(DisplayAttribute), false)
                                              .Single()).Order
                                     select property);

            ws.Cells["A1"].LoadFromCollection(
                Collection: items, PrintHeaders: true, TableStyle: TableStyles.Light1,
                memberFlags: System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
                Members: orderedProperties.ToArray());
            excel = await pck.GetAsByteArrayAsync();
        }
        return excel;
    }


    public static async Task<byte[]> ToExcelAsync(DataTable dt, int colIndex = 0, int rowIndex = 0)
    {
        byte[] excel;
        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        using (var pck = new ExcelPackage())
        {
            var ws = pck.Workbook.Worksheets.Add("Data");
            FillInfo(ws, dt);
            excel = await pck.GetAsByteArrayAsync();
        }
        return excel;
    }

    private static void SetWorkbookProperties(ExcelPackage p, string name)
    {
        //Here setting some document properties
        p.Workbook.Properties.Author = "Excel";
        p.Workbook.Properties.Created = DateTime.UtcNow;
        p.Workbook.Properties.Title = name;
    }

    private static ExcelWorksheet CreateSheet(ExcelPackage p, string sheetName)
    {
        ExcelWorksheet ws = p.Workbook.Worksheets.Add(sheetName);
        ws.Name = sheetName; //Setting Sheet's name
        ws.Cells.Style.Font.Size = 8; //Default font size for whole sheet
        ws.Cells.Style.Font.Name = "Verdana"; //Default Font name for whole sheet
        return ws;
    }

    private static void FillInfo<T>(ExcelWorksheet ws, IEnumerable<T> items)
    {
        var orderedProperties = (from property in typeof(T).GetProperties()
                                 where Attribute.IsDefined(property, typeof(DisplayAttribute))
                                 orderby ((DisplayAttribute)property
                                          .GetCustomAttributes(typeof(DisplayAttribute), false)
                                          .Single()).Order
                                 select property);

        ws.Cells["A1"].LoadFromCollection(
            Collection: items, PrintHeaders: true, TableStyle: TableStyles.Light1,
            memberFlags: System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
            Members: orderedProperties.ToArray());

        var tableHeader = ws.Cells[1, 1, 1, orderedProperties.Count()];
        tableHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
        tableHeader.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
        tableHeader.Style.Border.Bottom.Style
        = tableHeader.Style.Border.Top.Style
        = tableHeader.Style.Border.Left.Style
        = tableHeader.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        tableHeader.Style.Font.Size = 8;
        tableHeader.Style.Font.Name = "Verdana";
    }
    private static void FillInfo(ExcelWorksheet ws, DataTable dt)
    {
        var colIndex = 1;
        var rowIndex = 1;
        foreach (DataColumn dc in dt.Columns) //Creating Headings
        {
            var cell = ws.Cells[rowIndex, colIndex];

            //Setting the background color of header cells to Gray
            var fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(Color.LightGray);

            //Setting Top/left,right/bottom borders.
            var border = cell.Style.Border;
            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            //Setting Value in cell
            cell.Value = dc.Caption;

            colIndex++;
        }
        // INsert data
        foreach (DataRow dr in dt.Rows) // Adding Data into rows
        {
            colIndex = 1;
            rowIndex++;

            foreach (DataColumn dc in dt.Columns)
            {
                var cell = ws.Cells[rowIndex, colIndex];

                //Setting Value in cell
                double numVal;
                int intVal;
                if (int.TryParse(dr[dc.ColumnName].ToString(), out intVal))
                {
                    cell.Value = intVal;
                    cell.Style.Numberformat.Format = "0";
                }
                else if (double.TryParse(dr[dc.ColumnName].ToString(), out numVal))
                {
                    int decimalCount = numVal.ToString(CultureInfo.InvariantCulture).
                    Substring(numVal.ToString(CultureInfo.InvariantCulture).
                    IndexOf(".") + 1).Length;
                    string decimalPlace = new String('0', decimalCount);

                    cell.Value = numVal;
                    cell.Style.Numberformat.Format = "0." + decimalPlace;
                }
                else
                {
                    cell.Value = dr[dc.ColumnName];
                    cell.Style.Numberformat.Format = GetStringFormatForColumn(dc.ColumnName);
                }
                //  http://stackoverflow.com/questions/40209636/epplus-number-format
                colIndex++;
            }
        }
        ws.Cells.AutoFitColumns();
    }

    private static string GetStringFormatForColumn(string columnName)
    {
        var strFormat = string.Empty;
        if (ColumnFormats.ContainsKey(columnName))
        {
            return ColumnFormats[columnName];
        }
        else
        {
            return strFormat;
        }
    }
}