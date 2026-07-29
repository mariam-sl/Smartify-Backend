using Smartify.Application.DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.IService
{
    public interface IDashboardService
    {
        Task<AdminDashboardDTO> GetDashboardSummaryAsync();
    }
}
