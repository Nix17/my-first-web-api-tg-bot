using System.Net;
using System.Text;

namespace WebApi.Middlewares;

public class DefaultPageMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<DefaultPageMiddleware> _logger;

    public DefaultPageMiddleware(RequestDelegate next, ILogger<DefaultPageMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context, IConfiguration config)
    {
        if (
            !context.Request.Path.StartsWithSegments("/swagger") &&
            !context.Request.Path.StartsWithSegments("/info") &&
            !context.Request.Path.StartsWithSegments("/health") &&
            !context.Request.Path.StartsWithSegments("/api")
            )
        {
            await ReturnIndexPage(context);
            return;
        }
        await _next.Invoke(context);
    }

    private static async Task ReturnIndexPage(HttpContext context)
    {
        var file = new FileInfo(@"wwwroot\index.html");
        byte[] buffer;
        if (file.Exists)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.ContentType = "text/html";

            buffer = await File.ReadAllBytesAsync(file.FullName);
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.ContentType = "text/html";
            buffer = Encoding.UTF8
                .GetBytes(@"<!DOCTYPE html><html><head>    <meta charset='utf-8' />    <title>AMRguide</title></head><body>    <h1>AMRguide</h1> </body></html>");
        }

        context.Response.ContentLength = buffer.Length;

        using (var stream = context.Response.Body)
        {
            await stream.WriteAsync(buffer, 0, buffer.Length);
            await stream.FlushAsync();
        }
    }

    private bool IsLocalRequest(HttpContext context)
    {
        if (context.Connection.RemoteIpAddress == null && context.Connection.LocalIpAddress == null)
        {
            return true;
        }
        if (context.Connection.RemoteIpAddress.Equals(context.Connection.LocalIpAddress))
        {
            return true;
        }
        if (IPAddress.IsLoopback(context.Connection.RemoteIpAddress))
        {
            return true;
        }
        return false;
    }
}