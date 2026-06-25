using System.Text.Json.Serialization;

namespace TCA.Desktop.Models;

public class AnalyticsSummaryDto
{
    [JsonPropertyName("total")]
    public int TotalRequests { get; set; }
    [JsonPropertyName("closed")]
    public int ClosedRequests { get; set; }
    [JsonPropertyName("closureRate")]
    public double ClosureRate { get; set; }
}