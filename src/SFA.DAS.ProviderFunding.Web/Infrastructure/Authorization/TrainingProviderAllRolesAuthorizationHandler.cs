using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using SFA.DAS.ProviderFunding.Web.Infrastructure.Authentication;
using Microsoft.Extensions.Configuration;
using SFA.DAS.Provider.Shared.UI.Models;

namespace SFA.DAS.ProviderFunding.Web.Infrastructure.Authorization
{
    public class TrainingProviderAllRolesAuthorizationHandler : AuthorizationHandler<TrainingProviderAllRolesRequirement>
    {
        private readonly ITrainingProviderAuthorizationHandler _handler;
        private readonly IConfiguration _configuration;
        private readonly ProviderSharedUIConfiguration _providerSharedUiConfiguration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TrainingProviderAllRolesAuthorizationHandler(
            ITrainingProviderAuthorizationHandler handler,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ProviderSharedUIConfiguration providerSharedUiConfiguration)
        {
            _handler = handler;
            _httpContextAccessor = httpContextAccessor;
            _providerSharedUiConfiguration = providerSharedUiConfiguration;
            _configuration = configuration;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TrainingProviderAllRolesRequirement requirement)
        {
            var isStubProviderValidationEnabled = GetUseStubProviderValidationSetting();

            // check if the stub is activated to by-pass the validation. Mostly used for local development purpose.
            // logic to check if the provider is authorized if not redirect the user to PAS 401 un-authorized page.
            if (!isStubProviderValidationEnabled && !(await _handler.IsProviderAuthorized(context, true)))
            {
                var httpContext = _httpContextAccessor.HttpContext;
                httpContext?.Response.Redirect($"{_providerSharedUiConfiguration.DashboardUrl}/error/401");
            }

            context.Succeed(requirement);
        }

        #region "Private Methods"
        private bool GetUseStubProviderValidationSetting()
        {
            var value = _configuration.GetSection("UseStubProviderValidation").Value;

            return value != null && bool.TryParse(value, out var result) && result;
        }
        #endregion
    }
}
