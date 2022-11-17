using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.ProviderFunding.Web.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
        }
    }
}