using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

    ///<inheritdoc cref="ITrainingProviderAuthorizationHandler"/>
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

            // Condition to check if the Provider Details has permission to access Apprenticeship Services based on the property value "CanAccessApprenticeshipService" set to True.
            return providerDetails is { CanAccessApprenticeshipService: true };
        }

        #region "Private Methods"
        private long GetProviderId()
        {
            return long.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst(c => c.Type.Equals(ProviderClaims.ProviderUkprn))?.Value, out var providerId) 
                ? providerId 
                : 0;
        }
        #endregion
    }
}
