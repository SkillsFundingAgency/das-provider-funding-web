using System.Threading.Tasks;

namespace SFA.DAS.ProviderFunding.Web.Services;

public interface IProviderEarningsService
{
    Task<ProviderEarningsSummaryDto> GetSummary(long ukprn);

    Task<AcademicYearEarningsDto> GenerateCSV(long ukprn);
}