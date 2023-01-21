using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Tools;

public static class TableTool
{
    public static DataTable ToDataTable<T>(this IEnumerable<T> list)
    {
        Type elementType = typeof(T);
        DataTable t = new DataTable();

        //add a column to table for each public property on T
        foreach (var propInfo in elementType.GetProperties())
        {
            Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;
            var dc = new DataColumn(propInfo.Name, ColType);
            var displayName = propInfo.Name;
            int? displayOrder = 0;
            var attribute = propInfo.GetCustomAttributes(typeof(DisplayAttribute), true);
            if (attribute.Length != 0)
            {
                var attrValue = attribute.Cast<DisplayAttribute>().Single();
                displayName = attrValue?.Name;
                displayOrder = attrValue?.Order;
            }
            dc.Caption = displayName;
            t.Columns.Add(dc);
            var columnOrdinal = 0;
            if (displayOrder.Value >= t.Columns.Count)
            {
                columnOrdinal = t.Columns.Count - 1;
            }
            else if (displayOrder.Value > 0)
            {
                columnOrdinal = displayOrder.Value;
            }
            dc.SetOrdinal(columnOrdinal);
        }

        //go through each property on T and add each value to the table
        foreach (T item in list)
        {
            DataRow row = t.NewRow();

            foreach (var propInfo in elementType.GetProperties())
            {
                row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
            }

            t.Rows.Add(row);
        }

        return t;
    }

    public static DataTable JoinDataTables(DataTable t1, DataTable t2, params Func<DataRow, DataRow, bool>[] joinOn)
    {
        DataTable result = new DataTable();
        foreach (DataColumn col in t1.Columns)
        {
            if (result.Columns[col.ColumnName] == null)
                result.Columns.Add(col.ColumnName, col.DataType);
        }
        foreach (DataColumn col in t2.Columns)
        {
            if (result.Columns[col.ColumnName] == null)
                result.Columns.Add(col.ColumnName, col.DataType);
        }
        foreach (DataRow row1 in t1.Rows)
        {

            var joinRows = t2.AsEnumerable().Where(row2 =>
            {
                foreach (var parameter in joinOn)
                {
                    if (!parameter(row1, row2)) return false;
                }
                return true;
            });
            foreach (DataRow fromRow in joinRows)
            {
                DataRow insertRow = result.NewRow();
                foreach (DataColumn col1 in t1.Columns)
                {
                    insertRow[col1.ColumnName] = row1[col1.ColumnName];
                }
                foreach (DataColumn col2 in t2.Columns)
                {
                    insertRow[col2.ColumnName] = fromRow[col2.ColumnName];
                }
                result.Rows.Add(insertRow);
            }
        }
        return result;
    }

    public static DataTable LeftJoinDataTables(DataTable t1, DataTable t2, params Func<DataRow, DataRow, bool>[] joinOn)
    {
        DataTable result = new DataTable();
        foreach (DataColumn col in t1.Columns)
        {
            if (result.Columns[col.ColumnName] == null)
                result.Columns.Add(col.ColumnName, col.DataType);
        }
        foreach (DataColumn col in t2.Columns)
        {
            if (result.Columns[col.ColumnName] == null)
                result.Columns.Add(col.ColumnName, col.DataType);
        }
        foreach (DataRow row1 in t1.Rows)
        {
            DataRow insertRow = result.NewRow();
            foreach (DataColumn col1 in t1.Columns)
            {
                insertRow[col1.ColumnName] = row1[col1.ColumnName];
            }

            var joinRows = t2.AsEnumerable().Where(row2 =>
            {
                foreach (var parameter in joinOn)
                {
                    if (!parameter(row1, row2)) return false;
                }
                return true;
            });
            foreach (DataRow fromRow in joinRows)
            {
                foreach (DataColumn col2 in t2.Columns)
                {
                    insertRow[col2.ColumnName] = fromRow[col2.ColumnName];
                }
            }
            result.Rows.Add(insertRow);
        }
        return result;
    }
}