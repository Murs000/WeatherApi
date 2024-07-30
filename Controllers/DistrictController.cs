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
public class DistrictController(WeatherDb context, WeatherService service) : ControllerBase
{
    [HttpGet]
    public ActionResult<List<DistrictModel>> GetDistricts()
    {
        return Ok(context.DisrtictModel.ToList());
    }
    [HttpPost]
    public IActionResult InsertDistrict([FromBody] DistrictModel model)
    {
        context.DisrtictModel.Add(model);
        context.SaveChanges();

        return Ok(model);
    }
    [HttpPost("Upload")]
    public async Task<ActionResult<List<DistrictModel>>> UploadDistrictAsync(IFormFile file)
    {
        List<DistrictModel> models = [];
        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            stream.Position = 0;
            models = service.Deserialize(stream);
        }

        return Ok(models);
    }
}
