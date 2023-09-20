using SFA.DAS.ProviderFunding.Web.Models;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderFunding.Web.Services
{
    /// <summary>
    /// Interface to define the contracts related to Training Provider services.
    /// </summary>
    public interface ITrainingProviderService
    {
        /// <summary>
        /// Contract to get the details of Provider from Outer API by given ukprn number.
        /// </summary>
        /// <param name="ukprn">ukprn number.</param>
        /// <returns></returns>
        Task<GetProviderResponseItem> GetProviderDetails(long ukprn);
    }
}
