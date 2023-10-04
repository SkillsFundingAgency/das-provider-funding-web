using SFA.DAS.ProviderFunding.Web.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderFunding.Web.Services
{
    public class TrainingProviderService : ITrainingProviderService
    {
        private readonly HttpClient _httpClient;

        public TrainingProviderService(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<bool> CanProviderAccessService(long ukprn)
        {
            var url = OuterApiRoutes.Provider.GetProviderDetails(ukprn);

            using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

            var data = await JsonSerializer.DeserializeAsync<GetProviderSummaryResult>(
                await response.Content.ReadAsStreamAsync(),
                options: new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return data.CanAccessService;
        }
    }
}
