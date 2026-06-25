using EduRequestSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EduRequestSystemAPI.Controllers
{
    /// <summary>
    /// Контроллер для работы с аналитикой и дашбордами приложения.
    /// </summary>
    public class AnalyticsController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        /// <summary>
        /// Возвращает сводные данные аналитики (всего заявок, закрытые заявки, процент конверсии).
        /// </summary>
        /// <returns>Объект AnalyticsSummaryDto с ключевыми показателями.</returns>
        [HttpGet]
        [Route("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _analyticsService.GetSummaryAsync();
            return Ok(result);
        }

        /// <summary>
        /// Возвращает данные динамики поступления новых заявок, сгруппированные по дням.
        /// </summary>
        /// <returns>Коллекция объектов AnalyticsDynamicsDto (дата и количество).</returns>
        [HttpGet]
        [Route("dynamics")]
        public async Task<IActionResult> GetDynamics()
        {
            var result = await _analyticsService.GetDynamicsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Возвращает статистику по самым востребованным направлениям (для построения столбчатой диаграммы).
        /// </summary>
        /// <returns>Коллекция объектов PopularDirectionDto.</returns>
        [HttpGet]
        [Route("popular-directions")]
        public async Task<IActionResult> GetPopularDirections()
        {
            var result = await _analyticsService.GetPopularDirectionsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Возвращает текущее распределение всех заявок по статусам (для круговой диаграммы).
        /// </summary>
        /// <returns>Коллекция объектов StatusDistributionDto.</returns>
        [HttpGet]
        [Route("statuses-distribution")]
        public async Task<IActionResult> GetStatusesDistribution()
        {
            var result = await _analyticsService.GetStatusesDistributionAsync();
            return Ok(result);
        }
    }
}