using Newtonsoft.Json.Linq;
using WeatherApi.DataAccess;
using WeatherApi.Models;
using System.Xml;
using System.Xml.Linq;

namespace WeatherApi.Services
{
    public class WeatherService(HttpClient client,IConfiguration configuration)
    {
        public ReportModel GetWeather(double latitude, double longitude)
        {
            var apiKey = configuration["OpenWeatherMap:ApiKey"];
            var unit = configuration["OpenWeatherMap:Units"];
            var response = client.GetAsync(
                $"weather?lat={latitude}&lon={longitude}&appid={apiKey}&units={unit}").Result;
        
            var content = response.Content.ReadAsStringAsync().Result;
            var jsonResponse = JObject.Parse(content);

            return new ReportModel
            {
                WeatherId = (int)jsonResponse["weather"][0]["id"],
                Main = (string)jsonResponse["weather"][0]["main"],
                Description = (string)jsonResponse["weather"][0]["description"],
                Icon = (string)jsonResponse["weather"][0]["icon"],
                Temp = (float)jsonResponse["main"]["temp"],
                FeelsLike = (float)jsonResponse["main"]["feels_like"],
                TempMin = (float)jsonResponse["main"]["temp_min"],
                TempMax = (float)jsonResponse["main"]["temp_max"],
                Pressure = (float)jsonResponse["main"]["pressure"],
                Humidity = (float)jsonResponse["main"]["humidity"],
                SeaLevel = jsonResponse["main"]["sea_level"] != null ? (float)jsonResponse["main"]["sea_level"] : 0,
                GroundLevel = jsonResponse["main"]["grnd_level"] != null ? (float)jsonResponse["main"]["grnd_level"] : 0,
                WindSpeed = (float)jsonResponse["wind"]["speed"],
                WindDegree = (float)jsonResponse["wind"]["deg"],
                WindGust = jsonResponse["wind"]["gust"] != null ? (float)jsonResponse["wind"]["gust"] : 0,
                Clouds = (float)jsonResponse["clouds"]["all"]
            };
        }
    
    }
}