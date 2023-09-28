using Microsoft.AspNetCore.Mvc;
using SolarWatch.Context;

namespace SolarWatch.RepositoryPattern;

public class Repository<T> : IRepository<T> where T : EntityBase
{
    private readonly SolarWatchContext _dbContext;
    public Repository(SolarWatchContext dbContext)
    {
        _dbContext = dbContext;
    }
    public virtual T? GetByID(int id)
    {
        return _dbContext.Set<T>().FirstOrDefault(item => item.Id == id);
    }

    public virtual void Add(T entity)
    {
        _dbContext.Add(entity);
        _dbContext.SaveChanges();
    }
}