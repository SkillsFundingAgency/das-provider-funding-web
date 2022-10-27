using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.ProviderFunding.Infrastructure.Configuration;

namespace SFA.DAS.ProviderFunding.Web.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<ProviderFunding.Infrastructure.Configuration.ProviderFunding>(configuration.GetSection("ProviderFunding"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderFunding.Infrastructure.Configuration.ProviderFunding>>().Value);
            services.Configure<ProviderIdams>(configuration.GetSection("ProviderIdams"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderIdams>>().Value);
        }
    }
}