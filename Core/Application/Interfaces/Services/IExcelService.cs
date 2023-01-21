using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services;

public interface IExcelService
{
    Task<byte[]> ToExcelAsync<T>(IEnumerable<T> items);
    Task<byte[]> ToExcelAsync(Dictionary<string, DataTable> tables);
    Task<byte[]> ToExcelAsync(string name, DataTable data);

    List<DataTable> FromExcelFile(byte[] data);

    DataTable ToDataTable<T>(IEnumerable<T> list);

}