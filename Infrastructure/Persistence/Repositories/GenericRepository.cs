using Application.Interfaces.Repositories;
using Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;

    public GenericRepository(ApplicationDbContext dbContext)
    {
        _context = dbContext;
    }

    private bool disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            this.disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public virtual async Task<T> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public virtual async Task<T> GetByIntIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public virtual async Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize)
    {
        return await _context
            .Set<T>()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    public virtual async Task<T> UpdateAsync(T t, long id)
    {
        if (t == null)
            return null;
        T exist = await _context.Set<T>().FindAsync(id);
        if (exist != null)
        {
            _context.Entry(exist).CurrentValues.SetValues(t);
            _context.Entry(exist).State = EntityState.Modified;
        }
        await _context.SaveChangesAsync();
        return exist;
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _context
             .Set<T>()
             .AsNoTracking()
             .ToListAsync();
    }

    public virtual async Task<int> CountAsync()
    {
        return await _context.Set<T>().AsNoTracking().CountAsync();
    }

    public virtual async Task<ICollection<T>> GetAllIncludingAsync(bool noTrack = false, params Expression<Func<T, object>>[] include)
    {
        IQueryable<T> queryable = _context.Set<T>();
        foreach (Expression<Func<T, object>> includeProperty in include)
        {
            queryable = queryable.Include<T, object>(includeProperty);
        }

        if (noTrack) await queryable.AsNoTracking().ToListAsync();
        return await queryable.ToListAsync();
    }

    public virtual async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match, bool noTrack = false)
    {
        if (noTrack) return await _context.Set<T>().Where(match).AsNoTracking().ToListAsync();

        return await _context.Set<T>().Where(match).ToListAsync();
    }

    public virtual async Task<T> FindIncludingAsync(Expression<Func<T, bool>> match, bool noTrack = false, params Expression<Func<T, object>>[] include)
    {
        IQueryable<T> queryable = _context.Set<T>();
        foreach (Expression<Func<T, object>> includeProperty in include)
        {
            queryable = queryable.Include<T, object>(includeProperty);
        }

        if (noTrack) return await queryable.AsNoTracking().FirstOrDefaultAsync(match);
        return await queryable.FirstOrDefaultAsync(match);
    }

    public virtual async Task<ICollection<T>> FindAllIncludingAsync(Expression<Func<T, bool>> match, bool noTrack = false, params Expression<Func<T, object>>[] include)
    {
        IQueryable<T> queryable = _context.Set<T>();
        foreach (Expression<Func<T, object>> includeProperty in include)
        {
            queryable = queryable.Include<T, object>(includeProperty);
        }

        if (noTrack) return await queryable.Where(match).AsNoTracking().ToListAsync();
        return await queryable.Where(match).ToListAsync();
    }

    public virtual async Task<T> FindAsync(Expression<Func<T, bool>> match, bool noTrack = false)
    {
        if (noTrack) return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(match);
        return await _context.Set<T>().FirstOrDefaultAsync(match);
    }

    public virtual async Task<ICollection<T>> AddRangeAsync(ICollection<T> t)
    {
        await _context.Set<T>().AddRangeAsync(t);
        await _context.SaveChangesAsync();
        return t;
    }

    public virtual async Task DeleteRangeAsync(ICollection<T> entity)
    {
        _context.Set<T>().RemoveRange(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id) != null;
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id) != null;
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> match)
    {
        return await _context.Set<T>().Where(match).AsNoTracking().AnyAsync();
    }


}