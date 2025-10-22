using System.Diagnostics.Metrics;

namespace ObservableVersionedApi.Metrics;

public class ApiMetrics : IDisposable
{
    public const string MeterName = "ObservableVersionedApi.Custom";
    public const string MeterVersion = "1.0.0";
    
    private readonly Meter _meter;
    private readonly Counter<int> _requestCounter;

    public ApiMetrics()
    {
        _meter = new Meter(MeterName, MeterVersion);
        _requestCounter = _meter.CreateCounter<int>("api_requests_total", 
            description: "Total API requests by version and route");
    }

    public void RecordRequest(string apiVersion, string route, string method, int statusCode)
    {
        _requestCounter.Add(1, 
            new KeyValuePair<string, object?>("api_version", apiVersion),
            new KeyValuePair<string, object?>("method", method),
            new KeyValuePair<string, object?>("route", route),
            new KeyValuePair<string, object?>("status_code", statusCode.ToString()));
    }

    public void Dispose()
    {
        _meter?.Dispose();
    }
}