﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.Http;
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
            serviceCollection.AddClient<IProviderEarningsService>((client, _) => new ProviderEarningsService(client));

            return serviceCollection;
        }

        private static IServiceCollection AddClient<T>(
            this IServiceCollection serviceCollection,
            Func<HttpClient, IServiceProvider, T> instance) where T : class
        {
            serviceCollection.AddTransient(s =>
            {
                var settings = s.GetService<IOptions<ProviderFundingApiOptions>>()?.Value;

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

                return instance.Invoke(httpClient, s);
            });

            return serviceCollection;
        }
    }
}