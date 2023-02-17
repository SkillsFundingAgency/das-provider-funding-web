using System.Collections.Generic;

namespace SFA.DAS.ProviderFunding.Web.Services
{
    public class GetApprenticeshipsResponse
    {
        public IEnumerable<ApprenticeshipDto> Apprenticeships { get; set; }
    }
}
