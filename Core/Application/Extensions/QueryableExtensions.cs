using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Application.Helpers;

// TODO: MAke external project
namespace Application.Extensions;

public interface IQueryObject
{
    string FilterText { get; set; }
    string SortBy { get; set; }
    bool IsSortAsc { get; set; }
    int Page { get; set; }
    int PageSize { get; set; }
}

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, IQueryObject queryObj, Dictionary<string, Expression<Func<T, object>>> columnsMap)
    {
        if (string.IsNullOrEmpty(queryObj.SortBy) || !columnsMap.ContainsKey(queryObj.SortBy))
            return query;
        if (queryObj.IsSortAsc)
            return query = query.OrderBy(columnsMap[queryObj.SortBy]);
        else
            return query = query.OrderByDescending(columnsMap[queryObj.SortBy]);
    }

    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, IQueryObject queryObj)
    {
        if (queryObj.PageSize <= 0)
            queryObj.PageSize = 10;
        if (queryObj.Page <= 0)
            queryObj.Page = 1;
        return query = query.Skip((queryObj.Page - 1) * queryObj.PageSize).Take(queryObj.PageSize);
    }

    public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
    {
        foreach (var i in ie)
        {
            action(i);
        }
    }


    public static IOrderedQueryable<TSource> OrderByNew<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector,
        bool descending) => descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);

    public static IQueryable<T> ApplyOrderingDataTable<T>(this IQueryable<T> query,
        ICollection<KeyValuePair<string, string>> sortings,
      Dictionary<string, string> propertyMap)
    {
        if (sortings.First().Value.Equals("asc"))
            query = query.OrderBy(propertyMap[sortings.First().Key]);
        else
            query = query.OrderBy(propertyMap[sortings.First().Key] + " descending");

        if (sortings.Count > 1)
        {
            foreach (var sort in sortings)
            {
                if (sort.Value.Equals("asc"))
                    query = query.ThenByProperty(propertyMap[sort.Key]);
                else
                    query = query.ThenByPropertyDescending(propertyMap[sort.Key]);
            }

        }
        return query;
    }

    public static IQueryable<T> ApplyOrderingDataTable<T>(this IQueryable<T> query,
     ICollection<KeyValuePair<string, int>> sortings,
        Dictionary<string, string> propertyMap)
    {
        if (QueryHelper.PropertyExists<T>(propertyMap[sortings.First().Key]))
        {
            if (sortings.First().Value == 1)
                query = query.OrderByProperty(propertyMap[sortings.First().Key]);
            else
                query = query.OrderByPropertyDescending(propertyMap[sortings.First().Key]);
        }


        if (sortings.Count > 1)
        {
            foreach (var sort in sortings)
            {
                if (sort.Value == 1)
                    query = query.ThenByProperty(propertyMap[sort.Key]);
                else
                    query = query.ThenByPropertyDescending(propertyMap[sort.Key]);
            }

        }
        return query;
    }

    public static IQueryable<T> ApplyPagingDataTable<T>(this IQueryable<T> query, int page, int pageSize)
    {
        if (pageSize <= 0)
            pageSize = 10;
        if (page <= 0)
            page = 1;
        return query = query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    public static IQueryable<T> Search<T>(this IQueryable<T> source, string property, object value)
    {
        IQueryable<T> values =
            source.Where(t => t.GetType().GetProperty(property).GetValue(t, null) == value);
        return values;
    }

    public static IQueryable<T> SearchContain<T>(this IQueryable<T> query, string property, string value)
    {
        query =
           query.Where(t => t.GetType().GetProperty(property).GetValue(t, null).ToString().Contains(value));
        return query;
    }
}