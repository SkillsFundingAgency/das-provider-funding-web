using Microsoft.Azure.Services.AppAuthentication;
using Newtonsoft.Json;
using SFA.DAS.ProviderFunding.Web.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SFA.DAS.ProviderFunding.Infrastructure.Configuration;

namespace SFA.DAS.ProviderFunding.Web.Services
{
    /// <inheritdoc />
    public class TrainingProviderService : ITrainingProviderService
    {
        private readonly HttpClient _httpClient;
        private readonly TrainingProviderApiClientConfiguration _configuration;

        public TrainingProviderService(
            HttpClient client,
            TrainingProviderApiClientConfiguration configuration)
        {
            _httpClient = client;
            _configuration = configuration;
        }

        /// <inheritdoc />
        public async Task<GetProviderSummaryResult> GetProviderDetails(long ukprn)
        {
            var url = $"{BaseUrl()}" + OuterApiRoutes.Provider.GetProviderDetails(ukprn);

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            await AddAuthenticationHeader(requestMessage);

            var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

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

        private string BaseUrl()
        {
            if (_configuration.ApiBaseUrl.EndsWith("/"))
            {
                return _configuration.ApiBaseUrl;
            }
            return _configuration.ApiBaseUrl + "/";
        }

        private async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            if (!string.IsNullOrEmpty(_configuration.IdentifierUri))
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(_configuration.IdentifierUri);
                httpRequestMessage.Headers.Remove("X-Version");
                httpRequestMessage.Headers.Add("X-Version", "1.0");
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
    }
}
