using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.ProviderFunding.Infrastructure.Configuration;

namespace SFA.DAS.ProviderFunding.Web.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<ProviderFunding.Infrastructure.Configuration.ProviderFundingApiOptions>(configuration.GetSection("ProviderFundingApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderFundingApiOptions>>()!.Value);
            services.Configure<ProviderIdams>(configuration.GetSection("ProviderIdams"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderIdams>>().Value);
        }
    }
}