﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sphinx.Application.WeatherForecasts;
using Sphinx.Web.Host.AppConsts;

namespace Sphinx.Web.Host.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IWeatherForecastApplicationService _weatherForecastApplicationService;

        public ValuesController(IWeatherForecastApplicationService weatherForecastApplicationService)
        {
            _weatherForecastApplicationService = weatherForecastApplicationService;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<WeatherForecastDto>> WeatherForecasts()
        {
            return await _weatherForecastApplicationService.GetAllAsync();
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET api/values/5
        [Authorize(Policy = SphinxPolicies.ApiUser)]
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
