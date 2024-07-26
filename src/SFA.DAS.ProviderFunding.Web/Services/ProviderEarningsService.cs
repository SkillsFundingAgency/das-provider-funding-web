using Azure.Core;
using Microsoft.AspNetCore.Http;
using SFA.DAS.ProviderFunding.Web.Extensions;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderFunding.Web.Services;

public class ProviderEarningsService : IProviderEarningsService
{
    private readonly HttpClient _client;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProviderEarningsService(HttpClient client, IHttpContextAccessor httpContextAccessor)
    {
        _client = client;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ProviderEarningsSummaryDto> GetSummary(long ukprn)
    {
        var url = OuterApiRoutes.Provider.GetEarningsSummary(ukprn);

        using var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        var data = await JsonSerializer.DeserializeAsync<GetProviderEarningsSummaryResponse>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return data.Summary;
    }


    public async Task<AcademicYearEarningsDto> GetDetails(long ukprn)
    {
        var url = OuterApiRoutes.Provider.GetAcademicYearEarnings(ukprn);

        var token = _httpContextAccessor.HttpContext!.GetBearerToken();
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        requestMessage.Headers.Add("Authorization", $"Bearer {token}");

        using var response = await _client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        var data = await JsonSerializer.DeserializeAsync<GetAcademicEarningsResponse>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return data.AcademicYearEarnings;
    }



}
