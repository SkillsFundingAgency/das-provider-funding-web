using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SFA.DAS.ProviderFunding.Infrastructure.Enums;
using SFA.DAS.ProviderFunding.Web.Infrastructure.Authorization;
using SFA.DAS.ProviderFunding.Web.Services;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderFunding.Web.Infrastructure.Authentication
{
    /// <summary>
    /// Interface to define contracts related to Training Provider Authorization Handlers.
    /// </summary>
    public interface ITrainingProviderAuthorizationHandler
    {
        /// <summary>
        /// Contract to check is the logged in Provider is a valid Training Provider. 
        /// </summary>
        /// <param name="context">AuthorizationHandlerContext.</param>
        /// <param name="allowAllUserRoles">boolean.</param>
        /// <returns>boolean.</returns>
        Task<bool> IsProviderAuthorized(AuthorizationHandlerContext context, bool allowAllUserRoles);
    }
    public class TrainingProviderAuthorizationHandler : ITrainingProviderAuthorizationHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITrainingProviderService _trainingProviderService;

        public TrainingProviderAuthorizationHandler(
            IHttpContextAccessor httpContextAccessor,
            ITrainingProviderService trainingProviderService)
        {
            _httpContextAccessor = httpContextAccessor;
            _trainingProviderService = trainingProviderService;
        }

        public async Task<bool> IsProviderAuthorized(AuthorizationHandlerContext context, bool allowAllUserRoles)
        {
            var ukprn = GetProviderId();

            if (ukprn <= 0) return false;

            var providerDetails = await _trainingProviderService.GetProviderDetails(ukprn);

            // Logic to check if the provider is a valid
            // Condition 1: is the provider's profile a Main or Employer Profile.
            // Condition 2: is the provider's status Active or On-boarding.
            return providerDetails is
            {
                ProviderTypeId: (int)ProviderTypeIdentifier.MainProvider or (int)ProviderTypeIdentifier.EmployerProvider,
                StatusId: (int)ProviderStatusType.Active or (int)ProviderStatusType.Onboarding
            };
        }

        private long GetProviderId()
        {
            return long.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst(c => c.Type.Equals(ProviderClaims.ProviderUkprn))?.Value, out var providerId) ? providerId : 0;
        }
    }
}
