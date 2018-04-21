using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Sphinx.Application.Users;
using Sphinx.Application.WeatherForecasts;

namespace Sphinx.Application
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddSphinxApplication(this IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddTransient<IUserApplicationService, UserApplicationService>();
            services.AddTransient<IWeatherForecastApplicationService, WeatherForecastApplicationService>();

            return services;
        }
    }
}
