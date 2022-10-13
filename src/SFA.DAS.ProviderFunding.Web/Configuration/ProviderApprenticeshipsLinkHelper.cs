using Microsoft.Extensions.Options;
using SFA.DAS.ProviderFunding.Web.Configuration.Routing;

namespace SFA.DAS.ProviderFunding.Web.Configuration
{
    public class ProviderApprenticeshipsLinkHelper
    {
        private readonly ExternalLinksConfiguration _externalLinks;

        public ProviderApprenticeshipsLinkHelper(IOptions<ExternalLinksConfiguration> externalLinks)
        {
            _externalLinks = externalLinks.Value;
        }
        public string AccountHome => $"{_externalLinks.ProviderApprenticeshipSiteUrl}{ProviderApprenticeshipsRoutes.ProviderApprenticeshipSiteAccountsHomeRoute}";
        public string ProviderRecruitmentApi => $"{_externalLinks.ProviderRecruitmentApiUrl}{ProviderApprenticeshipsRoutes.ProviderRecruitmentApi}";
    }
}