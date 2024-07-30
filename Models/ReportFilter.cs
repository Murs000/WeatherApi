namespace WeatherApi.Models
{
    public record ReportFilter
{
    public int? WeatherId { get; init; }
    public string? Main { get; init; }
    public string? Description { get; init; }
    public float? TempMin { get; init; }
    public float? TempMax { get; init; }
    public float? FeelsLikeMin { get; init; }
    public float? FeelsLikeMax { get; init; }
    public float? PressureMin { get; init; }
    public float? PressureMax { get; init; }
    public float? HumidityMin { get; init; }
    public float? HumidityMax { get; init; }
    public float? WindSpeedMin { get; init; }
    public float? WindSpeedMax { get; init; }
    public float? WindDegreeMin { get; init; }
    public float? WindDegreeMax { get; init; }
    public float? CloudsMin { get; init; }
    public float? CloudsMax { get; init; }
    public int? DisctrictId { get; init; }
    public DateTime? DateTimeStart { get; init; }
    public DateTime? DateTimeEnd { get; init; }
}
}