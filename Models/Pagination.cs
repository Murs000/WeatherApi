namespace WeatherApi.Models
{
    public record Pagination
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}