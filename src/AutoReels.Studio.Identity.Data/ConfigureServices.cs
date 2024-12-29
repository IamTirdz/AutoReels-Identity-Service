using AutoReels.Studio.Identity.Common.Constants;
using AutoReels.Studio.Identity.Common.Entities;
using AutoReels.Studio.Identity.Data.Contexts;
using AutoReels.Studio.Identity.Data.Repositories;
using AutoReels.Studio.Identity.Data.Services;
using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutoReels.Studio.Identity.Data
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Audience = ApiResource.Module;
                    options.Authority = configuration["Authority"];

                    options.TokenValidationParameters.ValidateLifetime = true;
                    options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policy.ReadAccess, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(JwtClaimTypes.Scope, ApiScope.Read);
                });

                options.AddPolicy(Policy.WriteAccess, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(JwtClaimTypes.Scope, ApiScope.Write);
                });

                options.AddPolicy(Policy.UpdateAccess, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(JwtClaimTypes.Scope, ApiScope.Update);
                });

                options.AddPolicy(Policy.DeleteAccess, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(JwtClaimTypes.Scope, ApiScope.Delete);
                });

                options.AddPolicy(Policy.UpdatePasswordAccess, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(JwtClaimTypes.Scope, ApiScope.Update);
                    policy.RequireClaim(JwtClaimTypes.Scope, ApiScope.UpdatePassword);
                });

                var authorizationPolicy = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme,
                    IdentityServerConstants.DefaultCookieAuthenticationScheme,
                    IdentityServerConstants.ExternalCookieAuthenticationScheme
                );

                options.DefaultPolicy = authorizationPolicy.RequireAuthenticatedUser().Build();
            });

            services.AddHostedService<TokenCleanupHost>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            //var migrationAssembly = typeof(ConfigureServices).Assembly.FullName;
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddDbContext<PersistedGrantDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = true;

                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireDigit = true;
                })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromDays(2);
            });

            services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;

                    options.Authentication.CookieLifetime = TimeSpan.FromDays(30);
                    options.Authentication.CookieSlidingExpiration = true;
                    options.IssuerUri = configuration["IssuerUri"];

                    options.UserInteraction.LoginUrl = IdentityDefault.LoginUrl;
                    options.UserInteraction.LogoutUrl = IdentityDefault.LogoutUrl;
                })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = (context) => context.UseNpgsql(connectionString);
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = (context) => context.UseNpgsql(connectionString);

                    options.EnableTokenCleanup = true;
                })
                .AddProfileService<ProfileService>()
                .AddAspNetIdentity<ApplicationUser>()
                .AddDeveloperSigningCredential();

            services.AddScoped<IIdentityRepository, IdentityRepository>();

            return services;
        }
    }
}
