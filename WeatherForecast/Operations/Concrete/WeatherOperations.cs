
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecast.Models;

namespace WeatherForecast.Operations
{
    public class WeatherOperations : IWeatherOperations
    {
        public async Task<List<WeatherForecastDtoModel>> GetWeathers(string city, DateTime date)
        {
            var cityId = await GetByCity(city);
            var weathers = await GetByCityAndDate(cityId, date);

            return weathers;
        }

        private async Task<int> GetByCity(string city)
        {
            var client = new RestClient("https://www.metaweather.com/api/location/search/?query=" + city);
            var request = new RestRequest();
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var cities = JsonConvert.DeserializeObject<List<CityJsonModel>>(response.Content);
                if (!cities.Any())
                {
                    throw new InvalidOperationException("City is not found!");
                }
                return cities[0].woeid;
            }
            throw new InvalidOperationException(response.ErrorMessage);
        }

        private async Task<List<WeatherForecastDtoModel>> GetByCityAndDate(int cityId, DateTime date)
        {
            var client = new RestClient(
                "https://www.metaweather.com/api/location/" + cityId + "/" +
                date.ToString("yyyy-MM-dd").Replace('-', '/'));
            var request = new RestRequest();
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var cities = JsonConvert.DeserializeObject<List<CityAndDateJsonModel>>(response.Content);
                if (!cities.Any())
                {
                    throw new InvalidOperationException("City and date is not found!");
                }

                return cities
                    .GroupBy(x => x.created.Date)
                    .Select(x => new WeatherForecastDtoModel
                    {
                        Date = x.Key,
                        MinDegree = cities.FirstOrDefault(c => c.created.Date == x.Key.Date).min_temp,
                        MaxDegree = cities.FirstOrDefault(c => c.created.Date == x.Key.Date).max_temp,
                    }).OrderByDescending(x => x.Date).Take(5).ToList();
            }
            throw new InvalidOperationException(response.ErrorMessage);
        }

    }


}
