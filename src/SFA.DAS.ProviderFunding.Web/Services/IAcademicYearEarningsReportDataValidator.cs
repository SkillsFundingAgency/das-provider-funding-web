using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderFunding.Web.Services
{
    public interface IAcademicYearEarningsReportDataValidator
    {
        Task<bool> Validate(AcademicYearEarningsDto earningsData, IEnumerable<ApprenticeshipDto> apprenticeshipsData);
    }
}

