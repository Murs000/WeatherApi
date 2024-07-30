using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Quartz;
using WeatherApi.DataAccess;
using WeatherApi.Models;
namespace WeatherApi.Services
{
    public class WeatherJob(WeatherService service, WeatherDb DBcontext) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var models = DBcontext.DisrtictModel.ToList();

            foreach(var model in models)
            {
                var weather = service.GetWeather(model.Lat, model.Lon);

                weather.DisctrictId = model.Id;
                weather.DateTime = DateTime.Now.ToUniversalTime();

                DBcontext.ReportModel.Add(weather);
                DBcontext.SaveChanges();
            }
        }
    }
}