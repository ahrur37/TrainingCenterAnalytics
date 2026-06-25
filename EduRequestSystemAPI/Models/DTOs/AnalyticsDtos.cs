namespace EduRequestSystemAPI.Models.DTOs
{
    public record AnalyticsSummaryDto(int Total, int Closed, double ClosureRate);
    
    public record AnalyticsDynamicsDto(string Date, int Count);
    
    public record PopularDirectionDto(string DirectionName, int Count);
    
    public record StatusDistributionDto(string StatusName, int Count);
}