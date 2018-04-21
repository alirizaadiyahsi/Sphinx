using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sphinx.Application.WeatherForecasts
{
    public interface IWeatherForecastApplicationService
    {
        Task<List<WeatherForecastDto>> GetAllAsync();
    }
}
