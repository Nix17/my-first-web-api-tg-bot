using Application.Helpers;
using Application.Interfaces.Services;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using System.Reflection;

//using WebApi.Services;

namespace WebApi.Extensions;

public static class ServiceExtensions
{
    public static void AddSwaggerExtension(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "AMRguide.Api",
                Description = "",
                Contact = new OpenApiContact
                {
                    Name = "Ivan Trushin",
                    Email = "ivan.v.trushin@yandex.ru",
                }
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
        });
    }
    public static void AddApiVersioningExtension(this IServiceCollection services)
    {
        services.AddApiVersioning(config =>
        {
            // Specify the default API Version as 1.0
            config.DefaultApiVersion = new ApiVersion(1, 0);
            // If the client hasn't specified the API version in the request, use the default API version number
            config.AssumeDefaultVersionWhenUnspecified = true;
            // Advertise the API versions supported for the particular endpoint
            config.ReportApiVersions = true;
            config.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"));
        });
    }

    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
        services.AddHttpContextAccessor();
    }

    public static void AddExtendedMemoryCache(this IServiceCollection services)
    {
        services.AddResponseCaching();
        services.AddMemoryCache();
        services.AddSingleton<IMemoryCacheExtended, MemoryCacheExtended>();
    }

    public static void AddApiLimits(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddInMemoryRateLimiting();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    }

    public static void AddCorsRules(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(opt => opt.AddPolicy("localhost", p => { p.WithOrigins("http://127.0.0.1:5000", "http://127.0.0.1:4200", "http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials(); }));
        services.AddCors(opt => opt.AddDefaultPolicy(p => { p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }));
    }
}