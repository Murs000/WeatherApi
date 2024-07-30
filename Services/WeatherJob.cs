using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Quartz;
using WeatherApi.DataAccess;
using WeatherApi.Models;
namespace WeatherApi.Services
{
    public class WeatherJob(WeatherService service,IServiceUnitOfWork serviceUnitOfWork) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var models = serviceUnitOfWork.DistrictService.Get();

            foreach(var model in models)
            {
                var weather = service.GetWeather(model.Lat, model.Lon);

                serviceUnitOfWork.ReportService.Insert(weather, model.Id);
            }
        }
    }
}