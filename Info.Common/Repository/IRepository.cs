using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Info.Common.Repository;

public interface IRepository<T> where T : EntityBase
{
    IReadOnlyCollection<T> GetAll();
    IReadOnlyCollection<T> GetAll(Expression<Func<T, bool>> filter);
    T Get(Expression<Func<T, bool>> filter);
    T Get(int id);
    bool EntityExist(Expression<Func<T, bool>> filter);
    bool EntityExist(int id);
    void Create(T entity);
    void Update(T entity);
    void Remove(T entity);

    bool SaveChanges();
}
