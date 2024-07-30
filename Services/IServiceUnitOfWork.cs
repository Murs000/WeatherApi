using Newtonsoft.Json.Linq;
using WeatherApi.DataAccess;
using WeatherApi.Models;
using System.Xml;
using System.Xml.Linq;

namespace WeatherApi.Services
{
    public interface IServiceUnitOfWork
    {
        public IReportService ReportService {get;}
        public IDistrictService DistrictService {get;}
    }
}
