using EduRequestSystemAPI.Models.DTOs;

namespace EduRequestSystemAPI.Interfaces;

public interface IAnalyticsService
{
    Task<AnalyticsSummaryDto> GetSummaryAsync();
    Task<IEnumerable<AnalyticsDynamicsDto>> GetDynamicsAsync();
    Task<IEnumerable<PopularDirectionDto>> GetPopularDirectionsAsync();
    Task<IEnumerable<StatusDistributionDto>> GetStatusesDistributionAsync();
}