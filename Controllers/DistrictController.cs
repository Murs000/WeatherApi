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
public class DistrictController(IServiceUnitOfWork serviceUnitOfWork, WeatherService service) : ControllerBase
{
    [HttpGet]
    public ActionResult<List<DistrictModel>> GetDistricts()
    {
        return Ok(serviceUnitOfWork.DistrictService.Get());
    }
    [HttpPost]
    public IActionResult InsertDistrict([FromBody] DistrictModel model)
    {
        serviceUnitOfWork.DistrictService.Insert(model);
        return Ok();
    }
    [HttpPost("Upload")]
    public async Task<ActionResult<List<DistrictModel>>> UploadDistrictAsync(IFormFile file)
    {
        var district = await serviceUnitOfWork.DistrictService.DeserializeAsync(file);
        serviceUnitOfWork.DistrictService.Insert(district);
        return Ok(district);
    }
}
