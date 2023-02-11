using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Info.Common.Repository;

public class Repository<T> : IRepository<T> where T : EntityBase
{
    private readonly DbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public void Create(T entity)
    {
        _dbSet.Add(entity);
    }

    public bool EntityExist(Expression<Func<T, bool>> filter)
    {
        return _dbSet.Any(filter);
    }

    public bool EntityExist(int id)
    {
        return _dbSet.Any(c => c.Id == id);
    }

    public T Get(Expression<Func<T, bool>> filter)
    {
        return _dbSet.FirstOrDefault(filter);
    }

    public T Get(int id)
    {
        return _dbSet.FirstOrDefault(c => c.Id == id);
    }

    public IReadOnlyCollection<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public IReadOnlyCollection<T> GetAll(Expression<Func<T, bool>> filter)
    {
        return _dbSet
            .Where(filter)
            .ToList();
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public bool SaveChanges()
    {
        return (_dbContext.SaveChanges() >= 0);
    }
}