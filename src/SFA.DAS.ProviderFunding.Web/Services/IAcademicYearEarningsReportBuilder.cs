using SFA.DAS.ProviderFunding.Web.Models;
using System.Collections.Generic;

namespace SFA.DAS.ProviderFunding.Web.Services
{
    public interface IAcademicYearEarningsReportBuilder
    {
        List<AcademicYearEarningsReport> Build(AcademicYearEarningsDto earningsData, IEnumerable<ApprenticeshipDto> apprenticeshipsData);
    }
}