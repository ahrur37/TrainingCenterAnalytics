using EduRequestSystemAPI.DatabaseContext;
using EduRequestSystemAPI.Interfaces;
using EduRequestSystemAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EduRequestSystemAPI.Services.Implementations
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly ContextDb _context;

        public AnalyticsService(ContextDb context)
        {
            _context = context;
        }

        public async Task<AnalyticsSummaryDto> GetSummaryAsync()
        {
            var totalRequests = await _context.Requests.CountAsync();
            
            var closedRequests = await _context.Requests
                .CountAsync(r => r.StatusId == 4 || r.StatusId == 5);

            var closureRate = totalRequests > 0 
                ? Math.Round((double)closedRequests / totalRequests * 100, 1) 
                : 0;

            return new AnalyticsSummaryDto(totalRequests, closedRequests, closureRate);
        }

        public async Task<IEnumerable<AnalyticsDynamicsDto>> GetDynamicsAsync()
        {
            var rawDynamics = await _context.Requests
                .Select(r => new { RequestDate = r.CreatedAt.Date })
                .GroupBy(x => x.RequestDate)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(d => d.Date)
                .ToListAsync();

            var formattedDynamics = rawDynamics.Select(d => new AnalyticsDynamicsDto(
                d.Date.ToString("yyyy-MM-dd"),
                d.Count
            ));

            return formattedDynamics;
        }

        public async Task<IEnumerable<PopularDirectionDto>> GetPopularDirectionsAsync()
        {
            var rawData = await _context.Requests
                .Include(r => r.Direction)
                .GroupBy(r => r.Direction.Name)
                .Select(g => new 
                {
                    DirectionName = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(d => d.Count)
                .ToListAsync();

            return rawData.Select(d => new PopularDirectionDto(d.DirectionName, d.Count));
        }

        public async Task<IEnumerable<StatusDistributionDto>> GetStatusesDistributionAsync()
        {
            var rawData = await _context.Requests
                .Include(r => r.Status)
                .GroupBy(r => r.Status.Name)
                .Select(g => new 
                {
                    StatusName = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(s => s.Count)
                .ToListAsync();

            return rawData.Select(s => new StatusDistributionDto(s.StatusName, s.Count));
        }
    }
}