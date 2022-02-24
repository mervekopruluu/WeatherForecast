using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecast.Models;
using WeatherForecast.ViewModels;
using WeatherForecast.Operations;

namespace WeatherForecast.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWeatherOperations _weatherOperation;

        public HomeController(
            ILogger<HomeController> logger,
            IWeatherOperations weatherOperation)
        {
            _logger = logger;
            _weatherOperation = weatherOperation;
        }

        public IActionResult Index()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<JsonResult> Get(string city, DateTime date)
        {
            var result = await _weatherOperation.GetWeathers(city, date);
            return Json(result);
        }
    }
}
