namespace ObservableVersionedApi.Models.v2
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        // Renamed and changed type from int to double
        public double TempC { get; set; }

        // Removed TemperatureF property

        public string? Summary { get; set; }
    }
}
