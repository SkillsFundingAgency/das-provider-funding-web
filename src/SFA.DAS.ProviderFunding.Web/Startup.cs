using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.Provider.Shared.UI.Startup;
using SFA.DAS.ProviderFunding.Infrastructure.Configuration;
using SFA.DAS.ProviderFunding.Web.AppStart;
using SFA.DAS.ProviderFunding.Web.Infrastructure;
using SFA.DAS.ProviderFunding.Web.Infrastructure.Authorization;

namespace SFA.DAS.ProviderFunding.Web
{
    [ExcludeFromCodeCoverage]
    public class ApplicationStartup
    {
        private readonly IConfiguration _configuration;

        public ApplicationStartup(IConfiguration configuration, IWebHostEnvironment environment)
        {

            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddConfigurationOptions(_configuration);

            services.AddSingleton<IAuthorizationHandler, ProviderAuthorizationHandler>();

            services.AddProviderUiServiceRegistration(_configuration);

            services.AddAuthorizationServicePolicies();

            if (_configuration["StubProviderAuth"] != null && _configuration["StubProviderAuth"].Equals("true", StringComparison.CurrentCultureIgnoreCase))
            {
                services.AddProviderStubAuthentication();
            }
            else
            {
                var providerConfig = _configuration
                    .GetSection(nameof(ProviderIdams))
                    .Get<ProviderIdams>();
                services.AddAndConfigureProviderAuthentication(providerConfig);
            }


            services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });

            services.Configure<RouteOptions>(options =>
                {
                    options.LowercaseUrls = true;
                }).AddMvc(options =>
                {
                    if (!_configuration.IsDev())
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }

                })
                .SetDefaultNavigationSection(NavigationSection.Home)
                .EnableGoogleAnalytics()
                .SetZenDeskConfiguration(_configuration.GetSection("ProviderZenDeskSettings").Get<ZenDeskConfiguration>());

            if (!_configuration.IsDev() && !_configuration.IsLocal())
            {
                services.AddHealthChecks();
            }

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.IsEssential = true;
            });

            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

            services.AddLogging();
#if DEBUG
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
#endif
            services.AddOuterApiServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHealthChecks();
                app.UseExceptionHandler("/Error/500");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.Use(async (context, next) =>
            {
                if (context.Response.Headers.ContainsKey("X-Frame-Options"))
                {
                    context.Response.Headers.Remove("X-Frame-Options");
                }

                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");

                await next();

                if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
                {
                    //Re-execute the request so the user gets the error page
                    var originalPath = context.Request.Path.Value;
                    context.Items["originalPath"] = originalPath;
                    context.Request.Path = "/error/404";
                    await next();
                }
            });
            
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(builder =>
            {
                builder.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }


    }
}