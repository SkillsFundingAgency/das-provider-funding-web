﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderFunding.Web.Configuration;
using SFA.DAS.ProviderFunding.Web.Configuration.Routing;

namespace SFA.DAS.ProviderFunding.Web.Controllers
{
    public class ExternalLinksController : Controller
    {
        private readonly ProviderApprenticeshipsLinkHelper _linkHelper;

        public ExternalLinksController(ProviderApprenticeshipsLinkHelper linkHelper)
        {
            _linkHelper = linkHelper;
        }

        [AllowAnonymous]
        [HttpGet("account-home", Name = RouteNames.Dashboard_Account_Home)]
        public IActionResult AccountHome()
        {
            return Redirect(_linkHelper.AccountHome);
        }
        
        [HttpGet("{ukprn:length(8)}/provider-recruitment-api", Name = RouteNames.Dashboard_ProviderRecruitmentApi)]
        public IActionResult ProviderRecruitmentApi([FromRoute]long ukprn)
        {
            var url = string.Format(_linkHelper.ProviderRecruitmentApi, ukprn);
            return Redirect(url);
        }
    }
}