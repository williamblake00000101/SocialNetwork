using DAL.Context;
using DAL.Interfaces;
using DAL.Specifications;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    public readonly SocialNetworkContext _context;
    public readonly DbSet<T> _set;

    public GenericRepository(SocialNetworkContext context)
    {
        _context = context;
        _set = _context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _set.FindAsync(id).AsTask();
    }

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await _set.AsNoTracking().ToListAsync();
    }

    public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).CountAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(_set.AsQueryable(), spec);
    }

    public void Add(T entity)
    {
        _set.Add(entity);
    }
}