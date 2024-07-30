using Newtonsoft.Json.Linq;
using WeatherApi.DataAccess;
using WeatherApi.Models;
using System.Xml;
using System.Xml.Linq;

namespace WeatherApi.Services
{
    public interface IDistrictService
    {
        public List<DistrictModel> Get();
        public void Insert(DistrictModel model);
        public void Insert(List<DistrictModel> models);
        public Task<List<DistrictModel>> DeserializeAsync(IFormFile file);
    }
}