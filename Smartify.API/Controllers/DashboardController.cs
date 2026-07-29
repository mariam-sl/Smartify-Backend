using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smartify.Application.IService;
using Smartify.Domain.Constants;

namespace Smartify.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var dashboard = await _dashboardService.GetDashboardSummaryAsync();
            return Ok(dashboard);
        }
    }
}
