using Application.Extensions;
using Application.Interfaces.Services;

using Shared.Tools;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Extensions;

namespace Shared.Services;

public class ExcelService : IExcelService
{
    public List<DataTable> FromExcelFile(byte[] data)
    {
        return ExcelTool.ReadExcelFileContent(data);
    }

    public DataTable ToDataTable<T>(IEnumerable<T> list)
    {
        return list.ToDataTable();
    }

    public async Task<byte[]> ToExcelAsync<T>(IEnumerable<T> items)
    {
        return await ExcelTool.ToExcelAsync<T>(items);
    }

    public async Task<byte[]> ToExcelAsync(Dictionary<string, DataTable> tables)
    {
        var book = ExcelTool.NewExcel();

        foreach (var table in tables)
        {
            book = ExcelTool.ToExcelWorksheet(book, table.Value, table.Key);
        }

        var res = await ExcelTool.ToExcelAsync(book);
        return res;
    }

    public async Task<byte[]> ToExcelAsync(string name, DataTable data)
    {
        var book = ExcelTool.NewExcel();
        book = ExcelTool.ToExcelWorksheet(book, data, name);
        var res = await ExcelTool.ToExcelAsync(book);
        return res;
    }
}