using SFA.DAS.ProviderFunding.Web.Models;
using System.Net.Http;
using System.Text.Json;
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
        public async Task<GetProviderResponseItem> GetProviderDetails(long ukprn)
        {
            var url = OuterApiRoutes.Provider.GetProviderDetails(ukprn);

            using var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var providerData = await JsonSerializer.DeserializeAsync<GetProviderResponseItem>(
                await response.Content.ReadAsStreamAsync(), 
                options: new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return providerData;
        }
    }
}
