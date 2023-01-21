using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories;
public interface IGenericRepository<T> where T : class
{
    void Dispose();
    Task<int> CountAsync();

    Task<T> GetByIdAsync(Guid id);
    Task<T> GetByIntIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize);
    Task<ICollection<T>> GetAllIncludingAsync(bool noTrack = false, params Expression<Func<T, object>>[] include);

    Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match, bool noTrack = false);
    Task<T> FindIncludingAsync(Expression<Func<T, bool>> match, bool noTrack = false, params Expression<Func<T, object>>[] include);
    Task<ICollection<T>> FindAllIncludingAsync(Expression<Func<T, bool>> match, bool noTrack = false, params Expression<Func<T, object>>[] include);
    Task<T> FindAsync(Expression<Func<T, bool>> match, bool noTrack = false);


    Task<T> AddAsync(T entity);
    Task<ICollection<T>> AddRangeAsync(ICollection<T> t);
    Task UpdateAsync(T entity);
    Task<T> UpdateAsync(T t, long id);

    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(ICollection<T> entity);

    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsAsync(int id);
    Task<bool> AnyAsync(Expression<Func<T, bool>> match);
}