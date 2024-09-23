using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace SFA.DAS.ProviderFunding.Web.AppStart;

public static class AddDataProtectionExtensions
{
    public static void AddDataProtection(this IServiceCollection services, IConfiguration configuration)
    {
            
        var dataProtectionKeysDatabase = configuration["DataProtectionKeysDatabase"];
        var redisConnectionString = configuration["RedisConnectionString"];

        if (!string.IsNullOrEmpty(dataProtectionKeysDatabase) 
            && !string.IsNullOrEmpty(redisConnectionString))
        {
            var redis = ConnectionMultiplexer
                .Connect($"{redisConnectionString},{dataProtectionKeysDatabase}");

            services.AddDataProtection()
                .SetApplicationName("das-provider")
                .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
        }
    }
}