using Newtonsoft.Json.Linq;
using WeatherApi.DataAccess;
using WeatherApi.Models;
using System.Xml;
using System.Xml.Linq;

namespace WeatherApi.Services
{
    public class DistrictService(WeatherDb context) : IDistrictService
    {
        public List<DistrictModel> Get()
        {
            return context.DisrtictModel.ToList();
        }
        public void Insert(DistrictModel model)
        {
            var modelFromDB = context.DisrtictModel.First(c=> c.Name == model.Name) ?? null;
            if( modelFromDB == null)
            {
                context.DisrtictModel.Add(model);
                context.SaveChanges();
            }
            else
            {
                modelFromDB.Lat = model.Lat;
                modelFromDB.Lon = model.Lon;
                context.SaveChanges();
            }
        }
        public void Insert(List<DistrictModel> models)
        {
            foreach(var model in models)
            {
                Insert(model);
            }
        }
        public async Task<List<DistrictModel>> DeserializeAsync(IFormFile file)
        {
            var districts = new List<DistrictModel>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                var doc = XDocument.Load(stream);
                XNamespace ss = "urn:schemas-microsoft-com:office:spreadsheet";

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
            }
            return districts;
        }
    }
}