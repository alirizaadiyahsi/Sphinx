using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Sphinx.Domain;
using Sphinx.EntityFramework;

namespace Sphinx.Application.Tests
{
    public class TestBase
    {
        public SphinxDbContext GetEmptyDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SphinxDbContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            var inMemoryContext = new SphinxDbContext(optionsBuilder.Options);

            return inMemoryContext;
        }

        public SphinxDbContext GetInitializedDbContext()
        {
            var inMemoryContext = GetEmptyDbContext();

            var testUsers = new List<ApplicationUser>
            {
                new ApplicationUser {UserName = "A"},
                new ApplicationUser {UserName = "B"},
                new ApplicationUser {UserName = "C"},
                new ApplicationUser {UserName = "D"},
                new ApplicationUser {UserName = "E"},
                new ApplicationUser {UserName = "F"}
            };

            var testWeatherForecasts = new List<WeatherForecast>
            {
                new WeatherForecast {DateFormatted = "01.06.2018", Summary = "Hot", TemperatureC = 40},
                new WeatherForecast {DateFormatted = "01.03.2018", Summary = "Warm", TemperatureC = 4},
                new WeatherForecast {DateFormatted = "01.01.2018", Summary = "Freezing", TemperatureC = -40}
            };

            inMemoryContext.AddRange(testUsers);
            inMemoryContext.AddRange(testWeatherForecasts);
            inMemoryContext.SaveChanges();

            return inMemoryContext;
        }
    }
}
