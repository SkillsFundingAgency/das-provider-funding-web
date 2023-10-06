﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using SFA.DAS.ProviderFunding.Web.Infrastructure.Authentication;
using Microsoft.Extensions.Configuration;
using SFA.DAS.Provider.Shared.UI.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SFA.DAS.ProviderFunding.Web.Infrastructure.Authorization
{
    public class TrainingProviderAllRolesAuthorizationHandler : AuthorizationHandler<TrainingProviderAllRolesRequirement>
    {
        private readonly ITrainingProviderAuthorizationHandler _handler;
        private readonly IConfiguration _configuration;
        private readonly ProviderSharedUIConfiguration _providerSharedUiConfiguration;

        public TrainingProviderAllRolesAuthorizationHandler(
            ITrainingProviderAuthorizationHandler handler,
            IConfiguration configuration,
            ProviderSharedUIConfiguration providerSharedUiConfiguration)
        {
            _handler = handler;
            _providerSharedUiConfiguration = providerSharedUiConfiguration;
            _configuration = configuration;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TrainingProviderAllRolesRequirement requirement)
        {
            HttpContext currentContext;
            switch (context.Resource)
            {
                case HttpContext resource:
                    currentContext = resource;
                    break;
                case AuthorizationFilterContext authorizationFilterContext:
                    currentContext = authorizationFilterContext.HttpContext;
                    break;
                default:
                    currentContext = null;
                    break;
            }

            if (!context.User.HasClaim(c => c.Type.Equals(ProviderClaims.ProviderUkprn)))
            {
                context.Fail();
                return;
            }

            var claimValue = context.User.FindFirst(c => c.Type.Equals(ProviderClaims.ProviderUkprn))?.Value;

            if (!int.TryParse(claimValue, out var ukprn))
            {
                context.Fail();
                return;
            }

            var isStubProviderValidationEnabled = GetUseStubProviderValidationSetting();

            // check if the stub is activated to by-pass the validation. Mostly used for local development purpose.
            // logic to check if the provider is authorized if not redirect the user to PAS 401 un-authorized page.
            if (!isStubProviderValidationEnabled && !(await _handler.IsProviderAuthorized(context)))
            {
                currentContext?.Response.Redirect($"{_providerSharedUiConfiguration.DashboardUrl}/error/401");
            }

            context.Succeed(requirement);
        }

        private bool GetUseStubProviderValidationSetting()
        {
            var value = _configuration.GetSection("UseStubProviderValidation").Value;

            return value != null && bool.TryParse(value, out var result) && result;
        }
    }
}
