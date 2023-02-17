using SFA.DAS.ProviderFunding.Web.Models;
using System.Collections.Generic;

namespace SFA.DAS.ProviderFunding.Web.Services
{
    public interface IAcademicYearEarningsReportBuilder
    {
        IEnumerable<AcademicYearEarningsReport> Build(AcademicYearEarningsDto earningsData);
    }
}