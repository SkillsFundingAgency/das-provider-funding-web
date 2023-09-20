using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.Http;
using SFA.DAS.Http.Configuration;
using SFA.DAS.ProviderFunding.Infrastructure.Configuration;
using SFA.DAS.ProviderFunding.Web.Services;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace SFA.DAS.ProviderFunding.Web.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOuterApiServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddClient<IProviderEarningsService, FundingOuterApiOptions>((client, _) => new ProviderEarningsService(client));
            serviceCollection.AddClient<ITrainingProviderService, RecruitApiConfiguration>((client, _) => new TrainingProviderService(client));
            return serviceCollection;
        }

        public static IServiceCollection AddOtherServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAcademicYearEarningsReportBuilder, AcademicYearEarningsReportBuilder>();

            return serviceCollection;
        }

        private static IServiceCollection AddClient<T, TU>(
            this IServiceCollection serviceCollection,
            Func<HttpClient, IServiceProvider, T> instance) 
            where T : class
            where TU : class
        {
            serviceCollection.AddTransient(s =>
            {
                var settings = (IApimClientConfiguration)s.GetService<IOptions<TU>>()?.Value;

                var baseUrl = settings?.ApiBaseUrl;
                var clientBuilder = new HttpClientBuilder()
                    .WithDefaultHeaders()
                    .WithApimAuthorisationHeader(settings)
                    .WithLogging(s.GetService<ILoggerFactory>());

                var httpClient = clientBuilder.Build();

                if (!settings!.ApiBaseUrl.EndsWith("/"))
                {
                    baseUrl += "/";
                }

                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Remove("X-Version");
                httpClient.DefaultRequestHeaders.Add("X-Version", settings.ApiVersion);

                return instance.Invoke(httpClient, s);
            });

            return serviceCollection;
        }
    }
}