using System.ComponentModel.DataAnnotations;

namespace WeatherApi.Models
{
    public class DistrictModel
    {
        [Key]
        public int Id {get; set;}
        public string Name {get; set;} = string.Empty;
        public double Lat {get; set;}
        public double Lon {get; set;}
    }
}