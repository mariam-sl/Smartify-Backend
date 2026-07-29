using Smartify.Application.DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.IRepository
{
    public interface IDashboardRepository
    {
        Task<AdminDashboardDTO> GetDashboardSummaryAsync();
    }
}
