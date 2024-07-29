using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace SFA.DAS.ProviderFunding.Web.Extensions
{
    public static class UrlExtensions
    {
        public static HttpRequestMessage ToRequestMessageWithBearerToken(this string url, IHttpContextAccessor httpContextAccessor)
        {
            var token = httpContextAccessor.HttpContext!.GetBearerToken();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Add("Authorization", $"Bearer {token}");
            return requestMessage;
        }
    }
}
