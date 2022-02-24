using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherForecast.Models;

namespace WeatherForecast.Operations
{
    public interface IWeatherOperations
    {
        Task<List<WeatherForecastDtoModel>> GetWeathers(string city, DateTime date);
    }
}
