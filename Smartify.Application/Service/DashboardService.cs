using Smartify.Application.DTO.Dashboard;
using Smartify.Application.IRepository;
using Smartify.Application.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<AdminDashboardDTO> GetDashboardSummaryAsync()
        {
            return await _dashboardRepository.GetDashboardSummaryAsync();
        }
    }
}
