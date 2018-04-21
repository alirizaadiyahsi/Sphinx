using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Sphinx.Application.WeatherForecasts;
using Sphinx.Domain;
using Sphinx.EntityFramework;
using Xunit;

namespace Sphinx.Application.Tests
{
    public class WeatherForecastApplicationServiceTests : TestBase
    {
        private readonly IWeatherForecastApplicationService _weatherForecastApplicationService;

        public WeatherForecastApplicationServiceTests()
        {
            //todo: try move following lines to TestBase.cs
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ApplicationMappingProfile>();
            });

            var weatherForecastRepository = Substitute.For<IRepository<WeatherForecast>>();
            weatherForecastRepository.GetAllAsync()
                .Returns(GetInitializedDbContext().WeatherForecasts.ToListAsync());
            _weatherForecastApplicationService = new WeatherForecastApplicationService(weatherForecastRepository);
        }

        [Fact]
        public async void TestGetAll()
        {
            var weatherForecasts = await _weatherForecastApplicationService.GetAllAsync();
            Assert.NotNull(weatherForecasts);
            Assert.Equal(3, weatherForecasts.Count);
        }
    }
}
