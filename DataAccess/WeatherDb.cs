using WeatherApi.Models;
using Microsoft.EntityFrameworkCore;

namespace WeatherApi.DataAccess
{
    public class WeatherDb : DbContext
{
    public WeatherDb(DbContextOptions<WeatherDb> options) : base(options) {}
    public DbSet<DistrictModel> DisrtictModel => Set<DistrictModel>();
    public DbSet<ReportModel> ReportModel => Set<ReportModel>();
}
}