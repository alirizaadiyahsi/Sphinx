using AutoMapper;
using Sphinx.Application.WeatherForecasts;
using Sphinx.Domain;

namespace Sphinx.Application
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<WeatherForecast, WeatherForecastDto>();
        }
    }
}