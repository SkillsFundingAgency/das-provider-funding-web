using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderFunding.Web.Services
{
    public class ApprenticeshipsService : IApprenticeshipsService
    {
        private readonly HttpClient _client;

        public ApprenticeshipsService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<ApprenticeshipDto>> GetAll(long ukprn)
        {
            var url = OuterApiRoutes.Provider.GetApprenticeships(ukprn);

            using var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var data = await JsonSerializer.DeserializeAsync<GetApprenticeshipsResponse>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return data.Apprenticeships;
        }
    }
}