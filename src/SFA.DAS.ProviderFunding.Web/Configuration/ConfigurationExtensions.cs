using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Startup;
using SFA.DAS.ProviderFunding.Web.Configuration.Routing;
using SFA.DAS.ProviderFunding.Web.Extensions;
using SFA.DAS.ProviderFunding.Web.Filters;
using SFA.DAS.ProviderFunding.Web.Middleware;

namespace SFA.DAS.ProviderFunding.Web.Configuration
{
    public static class ConfigurationExtensions
    {
        private const int SessionTimeoutMinutes = 30;

        public static void AddAuthorizationService(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyNames.ProviderPolicyName, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ProviderRecruitClaims.IdamsUserUkprnClaimsTypeIdentifier);
                    policy.RequireClaim(ProviderRecruitClaims.IdamsUserServiceTypeClaimTypeIdentifier);
                    policy.Requirements.Add(new ProviderAccountRequirement());
                });

                options.AddPolicy(PolicyNames.HasContributorOrAbovePermission, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ProviderRecruitClaims.IdamsUserUkprnClaimsTypeIdentifier);
                    policy.RequireClaim(ProviderRecruitClaims.IdamsUserServiceTypeClaimTypeIdentifier);
                    policy.Requirements.Add(new MinimumServiceClaimRequirement(ServiceClaim.DAC));
                });

                options.AddPolicy(PolicyNames.HasContributorWithApprovalOrAbovePermission, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ProviderRecruitClaims.IdamsUserUkprnClaimsTypeIdentifier);
                    policy.RequireClaim(ProviderRecruitClaims.IdamsUserServiceTypeClaimTypeIdentifier);
                    policy.Requirements.Add(new MinimumServiceClaimRequirement(ServiceClaim.DAB));
                });

                options.AddPolicy(PolicyNames.HasAccountOwnerPermission, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ProviderRecruitClaims.IdamsUserUkprnClaimsTypeIdentifier);
                    policy.RequireClaim(ProviderRecruitClaims.IdamsUserServiceTypeClaimTypeIdentifier);
                    policy.Requirements.Add(new MinimumServiceClaimRequirement(ServiceClaim.DAA));
                });
                
                options.AddPolicy(PolicyNames.IsTraineeshipWeb, policy =>
                {
                    policy.Requirements.Add(new VacancyTypeRequirement(VacancyType.Traineeship));
                });
                
                options.AddPolicy(PolicyNames.IsApprenticeshipWeb, policy =>
                {
                    policy.Requirements.Add(new VacancyTypeRequirement(VacancyType.Apprenticeship));
                });
            });

            services.AddTransient<IAuthorizationHandler, ProviderAccountHandler>();
            services.AddTransient<IAuthorizationHandler, MinimumServiceClaimRequirementHandler>();
            services.AddTransient<IAuthorizationHandler, VacancyTypeRequirementHandler>();
        }

        public static void AddMvcService(this IServiceCollection services, IWebHostEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = CookieNames.AntiForgeryCookie;
                options.FormFieldName = "_csrfToken";
                options.HeaderName = "X-XSRF-TOKEN";
            });
            services.Configure<CookieTempDataProviderOptions>(options => options.Cookie.Name = CookieNames.RecruitTempData);
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

            services.AddMvc(opts =>
                {
                    opts.EnableEndpointRouting = false;
                    opts.Filters.Add(new AuthorizeFilter(PolicyNames.ProviderPolicyName));

                    var jsonInputFormatters = opts.InputFormatters.OfType<NewtonsoftJsonInputFormatter>();
                    foreach (var formatter in jsonInputFormatters)
                    {
                        formatter.SupportedMediaTypes
                            .Add(MediaTypeHeaderValue.Parse("application/csp-report"));
                    }

                    opts.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());

                    opts.Filters.AddService<PlannedOutageResultFilter>();
                    opts.Filters.AddService<GoogleAnalyticsFilter>();
                    opts.Filters.AddService<ZendeskApiFilter>();

                    opts.AddTrimModelBinderProvider(loggerFactory);
                }
            ).AddNewtonsoftJson()
            .EnableCookieBanner()
            .EnableGoogleAnalytics()
            .EnableCsp()
            .SetDefaultNavigationSection(NavigationSection.Recruit);
            services.AddFluentValidationAutoValidation();
        }

        public static void AddAuthenticationService(this IServiceCollection services, AuthenticationConfiguration authConfig)
        {
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignOutScheme = WsFederationDefaults.AuthenticationScheme;
            })
            .AddWsFederation(options =>
            {
                options.Wtrealm = authConfig.WtRealm;
                options.MetadataAddress = authConfig.MetaDataAddress;
                options.UseTokenLifetime = false;
                
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = CookieNames.RecruitData;
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                options.CookieManager = new ChunkingCookieManager() { ChunkSize = 3000 };
                options.AccessDeniedPath = RoutePaths.AccessDeniedPath;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(SessionTimeoutMinutes);
            });
            services
                .AddOptions<WsFederationOptions>(WsFederationDefaults.AuthenticationScheme)
                .Configure<IRecruitVacancyClient>((options, recruitVacancyClient) =>
                {
                    options.Events.OnSecurityTokenValidated = async (ctx) =>
                    {
                        await HandleUserSignedIn(ctx, recruitVacancyClient);
                    };
                });
        }

        private static async Task HandleUserSignedIn(SecurityTokenValidatedContext ctx, IRecruitVacancyClient vacancyClient)
        {
            var user = ctx.Principal.ToVacancyUser();           
            await vacancyClient.UserSignedInAsync(user, UserType.Provider);
        }
    }
}