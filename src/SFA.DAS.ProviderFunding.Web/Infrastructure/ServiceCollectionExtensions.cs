using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.Http;
using SFA.DAS.ProviderFunding.Infrastructure.Configuration;
using SFA.DAS.ProviderFunding.Web.Services;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SFA.DAS.ProviderFunding.Web.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOuterApiServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddClient<IProviderEarningsService>((client, _) => new ProviderEarningsService(client));
            serviceCollection.AddOuterApiClient<ITrainingProviderService>((client, _) => new TrainingProviderService(client));
            return serviceCollection;
        }

        public static IServiceCollection AddOtherServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAcademicYearEarningsReportBuilder, AcademicYearEarningsReportBuilder>();

            return serviceCollection;
        }

        private static IServiceCollection AddClient<T>(
            this IServiceCollection serviceCollection,
            Func<HttpClient, IServiceProvider, T> instance) where T : class
        {
            serviceCollection.AddTransient(s =>
            {
                var settings = s.GetService<IOptions<FundingOuterApiOptions>>()?.Value;

                var clientBuilder = new HttpClientBuilder()
                    .WithDefaultHeaders()
                    .WithApimAuthorisationHeader(settings)
                    .WithLogging(s.GetService<ILoggerFactory>());

                var httpClient = clientBuilder.Build();

                if (!settings!.ApiBaseUrl.EndsWith("/"))
                {
                    settings.ApiBaseUrl += "/";
                }

                httpClient.BaseAddress = new Uri(settings.ApiBaseUrl);
                httpClient.DefaultRequestHeaders.Remove("X-Version");
                httpClient.DefaultRequestHeaders.Add("X-Version", settings.ApiVersion);

                return instance.Invoke(httpClient, s);
            });

            return serviceCollection;
        }

        private static IServiceCollection AddOuterApiClient<T>(
            this IServiceCollection serviceCollection,
            Func<HttpClient, IServiceProvider, T> instance) where T : class
        {
            serviceCollection.AddTransient(s =>
            {
                var settings = s.GetService<IOptions<TrainingProviderApiClientConfiguration>>()?.Value;
                var configuration = s.GetService<IConfiguration>();

                var clientBuilder = new HttpClientBuilder()
                    .WithDefaultHeaders()
                    .WithLogging(s.GetService<ILoggerFactory>());

                var httpClient = clientBuilder.Build();

                if (!settings!.ApiBaseUrl.EndsWith("/"))
                {
                    settings.ApiBaseUrl += "/";
                }

                if (!configuration.IsLocal())
                {
                    var azureServiceTokenProvider = new AzureServiceTokenProvider();
                    var accessToken = azureServiceTokenProvider.GetAccessTokenAsync(settings.IdentifierUri).GetAwaiter().GetResult();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }

                httpClient.BaseAddress = new Uri(settings.ApiBaseUrl);
                httpClient.DefaultRequestHeaders.Remove("X-Version");
                httpClient.DefaultRequestHeaders.Add("X-Version", "1.0");

                return instance.Invoke(httpClient, s);
            });

            return serviceCollection;
        }
    }
}