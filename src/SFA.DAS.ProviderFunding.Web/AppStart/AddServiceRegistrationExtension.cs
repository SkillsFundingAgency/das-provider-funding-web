using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.ProviderFunding.Web.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
        }
    }
}