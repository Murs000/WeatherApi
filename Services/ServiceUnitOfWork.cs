using Newtonsoft.Json.Linq;
using WeatherApi.DataAccess;
using WeatherApi.Models;
using System.Xml;
using System.Xml.Linq;

namespace WeatherApi.Services
{
    public class ServiceUnitOfWork(WeatherDb context) :IServiceUnitOfWork
    {
        public IReportService ReportService => new ReportService(context);
        public IDistrictService DistrictService => new DistrictService(context);
    }
}
