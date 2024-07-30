using Newtonsoft.Json.Linq;
using WeatherApi.DataAccess;
using WeatherApi.Models;
using System.Xml;
using System.Xml.Linq;

namespace WeatherApi.Services
{
public class WeatherService(HttpClient client,IConfiguration configuration,WeatherDb context)
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
    public List<ReportModel> Filter(ReportFilter filter,Pagination pagination)
    {
        var query = context.ReportModel.AsQueryable();

            if (filter.WeatherId.HasValue)
                query = query.Where(r => r.WeatherId == filter.WeatherId.Value);

            if (!string.IsNullOrEmpty(filter.Main))
                query = query.Where(r => r.Main == filter.Main);

            if (!string.IsNullOrEmpty(filter.Description))
                query = query.Where(r => r.Description == filter.Description);

            if (filter.TempMin.HasValue)
                query = query.Where(r => r.Temp >= filter.TempMin.Value);

            if (filter.TempMax.HasValue)
                query = query.Where(r => r.Temp <= filter.TempMax.Value);

            if (filter.FeelsLikeMin.HasValue)
                query = query.Where(r => r.FeelsLike >= filter.FeelsLikeMin.Value);

            if (filter.FeelsLikeMax.HasValue)
                query = query.Where(r => r.FeelsLike <= filter.FeelsLikeMax.Value);

            if (filter.PressureMin.HasValue)
                query = query.Where(r => r.Pressure >= filter.PressureMin.Value);

            if (filter.PressureMax.HasValue)
                query = query.Where(r => r.Pressure <= filter.PressureMax.Value);

            if (filter.HumidityMin.HasValue)
                query = query.Where(r => r.Humidity >= filter.HumidityMin.Value);

            if (filter.HumidityMax.HasValue)
                query = query.Where(r => r.Humidity <= filter.HumidityMax.Value);

            if (filter.WindSpeedMin.HasValue)
                query = query.Where(r => r.WindSpeed >= filter.WindSpeedMin.Value);

            if (filter.WindSpeedMax.HasValue)
                query = query.Where(r => r.WindSpeed <= filter.WindSpeedMax.Value);

            if (filter.WindDegreeMin.HasValue)
                query = query.Where(r => r.WindDegree >= filter.WindDegreeMin.Value);

            if (filter.WindDegreeMax.HasValue)
                query = query.Where(r => r.WindDegree <= filter.WindDegreeMax.Value);

            if (filter.CloudsMin.HasValue)
                query = query.Where(r => r.Clouds >= filter.CloudsMin.Value);

            if (filter.CloudsMax.HasValue)
                query = query.Where(r => r.Clouds <= filter.CloudsMax.Value);

            if (filter.DisctrictId.HasValue)
                query = query.Where(r => r.DisctrictId == filter.DisctrictId.Value);

            if (filter.DateTimeStart.HasValue)
                query = query.Where(r => r.DateTime >= filter.DateTimeStart.Value);

            if (filter.DateTimeEnd.HasValue)
                query = query.Where(r => r.DateTime <= filter.DateTimeEnd.Value);


            var paginatedReports = query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            return paginatedReports;
        }
       public List<DistrictModel> Deserialize(Stream xmlStream)
        {
            var districts = new List<DistrictModel>();

            var doc = XDocument.Load(xmlStream);
            XNamespace ss = "urn:schemas-microsoft-com:office:spreadsheet";

            // Skip the first row if it contains headers
            foreach (var row in doc.Descendants(ss + "Row").Skip(1))
            {
                var cells = row.Elements(ss + "Cell").ToList();
                if (cells.Count >= 4) // Ensure there are enough cells to read from
                {
                    var district = new DistrictModel
                    {
                        Id = int.Parse(cells[0].Element(ss + "Data").Value),
                        Name = cells[1].Element(ss + "Data").Value,
                        Lat = float.Parse(cells[2].Element(ss + "Data").Value),
                        Lon = float.Parse(cells[3].Element(ss + "Data").Value)
                    };
                    districts.Add(district);
                }
            }

            return districts;
        }

}
}