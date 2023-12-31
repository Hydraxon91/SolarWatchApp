﻿using Microsoft.AspNetCore.Mvc;
using SolarWatch.Context;
using SolarWatch.Model;

namespace SolarWatch.RepositoryPattern;

public class SunriseSunsetRepository : ISunriseSunsetRepository
{
    private readonly SolarWatchContext _dbContext;
    public SunriseSunsetRepository(SolarWatchContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public SunriseSunset? GetByIdAndDate(int cityId, DateTime date)
    {
        return _dbContext.SunriseSunsets.FirstOrDefault(item => item.CityId == cityId && item.Date == date);
    }
    

    public void Add(SunriseSunset entity)
    {
        _dbContext.Add(entity);
        _dbContext.SaveChanges();
    }

    public void Update(SunriseSunset oldEntity, SunriseSunset newEntity)
    {
        var existingEntity = _dbContext.SunriseSunsets.Find(oldEntity.Id);
        if (existingEntity != null)
        {
            existingEntity.Date = newEntity.Date;
            existingEntity.Sunrise = newEntity.Sunrise;
            existingEntity.Sunset = newEntity.Sunset;
            _dbContext.SaveChanges();
        }
    }

}