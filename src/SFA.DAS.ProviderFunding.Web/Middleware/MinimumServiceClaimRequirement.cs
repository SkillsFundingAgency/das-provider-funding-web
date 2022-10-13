using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ProviderFunding.Web.Configuration;

namespace SFA.DAS.ProviderFunding.Web.Middleware
{
    public class MinimumServiceClaimRequirement : IAuthorizationRequirement
    {
        public ServiceClaim MinimumServiceClaim { get; set; }

        public MinimumServiceClaimRequirement(ServiceClaim minimumServiceClaim)
        {
            MinimumServiceClaim = minimumServiceClaim;
        }
    }
}
