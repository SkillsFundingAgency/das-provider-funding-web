using SFA.DAS.ProviderFunding.Web.Models;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderFunding.Web.Services
{
    /// <summary>
    /// Contract to interact with Training Provider(RoATP/APAR) Outer Api.
    /// </summary>
    public interface ITrainingProviderService
    {
        /// <summary>
        /// Contract to get the details of the Provider by given ukprn or provider Id.
        /// </summary>
        /// <param name="providerId">ukprn.</param>
        /// <returns>bool.</returns>
        Task<bool> CanProviderAccessService(long providerId);
    }
}
