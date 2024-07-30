using Microsoft.AspNetCore.Mvc;
using WeatherApi.DataAccess;
using WeatherApi.Models;
using System.Xml.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using WeatherApi.Services;

namespace WeatherApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ReportController(WeatherService service,IServiceUnitOfWork serviceUnitOfWork) : ControllerBase
{
    [HttpGet]
    public ActionResult<List<ReportModel>> GetReports([FromQuery] ReportFilter filter, [FromQuery] Pagination pagination)
    {
        return Ok(serviceUnitOfWork.ReportService.Filter(filter,pagination));
    }
    [HttpPost]
    public IActionResult WeatherCheck([FromBody] DistrictModel model)
    {
        var weather = service.GetWeather(model.Lat, model.Lon);

        serviceUnitOfWork.ReportService.Insert(weather,model.Id);

        return Ok(weather);
    }
}
