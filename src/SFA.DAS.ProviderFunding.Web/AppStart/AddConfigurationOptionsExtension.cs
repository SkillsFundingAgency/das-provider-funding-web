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
            services.Configure<FundingOuterApiOptions>(configuration.GetSection("FundingOuterApiConfiguration"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<FundingOuterApiOptions>>()!.Value);
            services.Configure<TrainingProviderApiClientConfiguration>(configuration.GetSection("TrainingProviderApiConfiguration"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<TrainingProviderApiClientConfiguration>>()!.Value);
            services.Configure<ProviderIdams>(configuration.GetSection("ProviderIdams"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderIdams>>().Value);
        }
    }
}