using blazchat.Domain.Entities;
using blazchat.Infra.Data.Context;
using blazchat.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace blazchat.Infra.Data.Repositories;

public class EFRepository<T> : IRepository<T> where T : EntityBase
{
    private readonly AplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    protected EFRepository(AplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<List<T>> GetAll()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<T> GetById(Guid id)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Entity not found");
    }

    public async Task<T> Add(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Update(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task Delete(Guid id)
    {
        _dbSet.Remove(await GetById(id));
        await _context.SaveChangesAsync();
    }
}