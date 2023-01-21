using AspNetCoreRateLimit;

using WebApi.Middlewares;

namespace WebApi.Extensions;

public static class AppExtensions
{
    public static void UseSwaggerExtension(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerAuthorized();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api");
            c.RoutePrefix = "swagger";
        });
    }
    public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();
    }

    public static void UseDefaultPage(this IApplicationBuilder app)
    {
        app.UseMiddleware<DefaultPageMiddleware>();
    }

    public static void UseRateLimit(this IApplicationBuilder app)
    {
        app.UseIpRateLimiting();
    }

    public static void UseCorsRules(this IApplicationBuilder app)
    {
        app.UseCors("localhost");
    }

    public static IApplicationBuilder UseSwaggerAuthorized(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SwaggerAuthorizedMiddleware>();
    }
}