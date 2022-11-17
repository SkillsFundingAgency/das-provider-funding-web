using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderFunding.Web.Services
{
    public class ProviderEarningsService : IProviderEarningsService
    {
        private readonly HttpClient _client;

        public ProviderEarningsService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ProviderEarningsSummaryDto> GetSummary(long ukprn)
        {
            var url = OuterApiRoutes.Provider.GetEarningsSummary(ukprn);

            using var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var data = await JsonSerializer.DeserializeAsync<ProviderEarningsSummaryDto>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return data;
        }
    }
}
