using System.Linq.Expressions;
using ToDoList.Data;
using ToDoList.Interfaces;

namespace ToDoList.Repositories;

public class Repository<T> (DataContext context) : IRepository<T> where T : class
{
    public ICollection<T> Find(Expression<Func<T, bool>> predicate)
    {
        return context.Set<T>().Where(predicate).ToArray();
    }
    
    public ICollection<T> GetAll()
    {
        return context.Set<T>().ToArray();
    }

    public T? GetById(int id)
    {
        return context.Set<T>().Find(id);
    }

    public void Create(T entity)
    {
        context.Set<T>().Add(entity);
        context.SaveChanges();
    }

    public void Update(T entity)
    {
        context.Set<T>().Update(entity);
        context.SaveChanges();
    }

    public void Remove(T entity)
    {
        context.Set<T>().Remove(entity);
        context.SaveChanges();
    }
}