namespace juniorcalcmiles_serve;

public class WeatherForecast
{
    private static int forecastIndex = 0;
    public WeatherForecast() {
        ForecastIndex = forecastIndex++;
    }

    public int ForecastIndex { get; }

    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}
