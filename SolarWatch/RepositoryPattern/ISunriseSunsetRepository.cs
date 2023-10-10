using Microsoft.AspNetCore.Mvc;
using SolarWatch.Model;

namespace SolarWatch.RepositoryPattern;

public interface ISunriseSunsetRepository
{
    SunriseSunset? GetByIdAndDate(int cityId, DateTime date);
    void Add(SunriseSunset entity);
    // void Delete(T entity);
    void Update(SunriseSunset oldEntity, SunriseSunset newEntity);
}
public abstract class EntityBase
{
    public int Id { get; protected set; }
}