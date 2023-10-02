using Newtonsoft.Json;
using SFA.DAS.ProviderFunding.Web.Models;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderFunding.Web.Services
{
    /// <inheritdoc />
    public class TrainingProviderService : ITrainingProviderService
    {
        private readonly HttpClient _client;

        public TrainingProviderService(HttpClient client)
        {
            _client = client;
        }

        /// <inheritdoc />
        public async Task<GetProviderSummaryResult> GetProviderDetails(long ukprn)
        {
            var url = OuterApiRoutes.Provider.GetProviderDetails(ukprn);

            using var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<GetProviderSummaryResult>(json);
                case HttpStatusCode.NotFound:
                default:
                    return default;
            }
        }
    }
}
