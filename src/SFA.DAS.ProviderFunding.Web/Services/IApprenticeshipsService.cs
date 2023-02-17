using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderFunding.Web.Services;

public interface IApprenticeshipsService
{
    Task<IEnumerable<ApprenticeshipDto>> GetAll(long ukprn);
}