using Asp.Versioning;
using AutoReels.Studio.Identity.API.Middlewares;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using NSwag;
using System.Text.Json;
using System.Text.Json.Serialization;
using ZymLabs.NSwag.FluentValidation;
using NSwag.Generation.AspNetCore;

namespace AutoReels.Studio.Identity.API
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.AddHttpContextAccessor();
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            services.AddScoped(provider =>
            {
                var validationRules = provider.GetService<IEnumerable<FluentValidationRule>>();
                var loggerFactory = provider.GetService<ILoggerFactory>();

                return new FluentValidationSchemaProcessor(provider, validationRules, loggerFactory);
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.InvalidModelStateResponseFactory = context =>
                {
                    return new ObjectResult(new { error = "Invalid request media type." })
                    {
                        StatusCode = StatusCodes.Status415UnsupportedMediaType
                    };
                };
            });

            services
                .AddOpenApiDocument((options, provider) => Configure(options, provider, "v1"))
                .AddOpenApiDocument((options, provider) => Configure(options, provider, "v2"));

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;

                options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("x-api-version"),
                    new MediaTypeApiVersionReader("x-api-version"));
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddHttpLogging(logs =>
            {
                logs.LoggingFields = HttpLoggingFields.All;
                logs.RequestBodyLogLimit = 4096;
                logs.ResponseBodyLogLimit = 4096;
                logs.CombineLogs = true;
            });

            return services;
        }

        private static void Configure(AspNetCoreOpenApiDocumentGeneratorSettings configure, IServiceProvider provider, string version)
        {
            var fluentValidationSchemaProcessor = provider.CreateScope().ServiceProvider.GetService<FluentValidationSchemaProcessor>();
            configure.SchemaSettings.SchemaProcessors.Add(fluentValidationSchemaProcessor!);

            configure.DocumentName = version;
            configure.ApiGroupNames = new[] { version };

            configure.PostProcess = document =>
            {
                document.Info = new OpenApiInfo
                {
                    Title = "AutoReels Identity Service",
                    Description = "No description",
                    Version = version
                };
            };
        }
    }
}
