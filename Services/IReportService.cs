using Newtonsoft.Json.Linq;
using WeatherApi.DataAccess;
using WeatherApi.Models;
using System.Xml;
using System.Xml.Linq;

namespace WeatherApi.Services
{
    public interface IReportService
    {
        public List<ReportModel> Filter(ReportFilter filter,Pagination pagination);
    
        public List<ReportModel> Get();
        public int Insert(ReportModel model, int districtId);
    }
}
