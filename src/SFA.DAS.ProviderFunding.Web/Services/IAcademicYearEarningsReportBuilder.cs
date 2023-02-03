using SFA.DAS.ProviderFunding.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderFunding.Web.Services
{
    public interface IAcademicYearEarningsReportBuilder
    {
        Task<List<AcademicYearEarningsReport>> BuildAsync(AcademicYearEarningsDto earningsData, IEnumerable<ApprenticeshipDto> apprenticeshipsData);
    }
}