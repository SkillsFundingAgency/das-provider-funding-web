using Microsoft.AspNetCore.Authorization;
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
        /// <returns>boolean.</returns>
        Task<bool> IsProviderAuthorized(AuthorizationHandlerContext context);
    }

    ///<inheritdoc cref="ITrainingProviderAuthorizationHandler"/>
    public class TrainingProviderAuthorizationHandler : ITrainingProviderAuthorizationHandler
    {
        private readonly ITrainingProviderService _trainingProviderService;

        public TrainingProviderAuthorizationHandler(ITrainingProviderService trainingProviderService)
        {
            _trainingProviderService = trainingProviderService;
        }

        public async Task<bool> IsProviderAuthorized(AuthorizationHandlerContext context)
        {
            var ukprn = GetProviderId(context);

            if (ukprn <= 0)
            {
                return false;
            }

            var providerDetails = await _trainingProviderService.CanProviderAccessService(ukprn);

            return providerDetails;
        }

        private long GetProviderId(AuthorizationHandlerContext authorizationHandlerContext)
        {
            return long.TryParse(authorizationHandlerContext.User.FindFirst(c => c.Type.Equals(ProviderClaims.ProviderUkprn))?.Value, out var providerId) 
                ? providerId 
                : 0;
        }
    }
}
