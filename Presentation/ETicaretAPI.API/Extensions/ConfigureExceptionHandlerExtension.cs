using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;

namespace ETicaretAPI.API.Extensions
{
    public static class ConfigureExceptionHandlerExtension
    {
        public static void ConfigureExceptionHandler<T>(this WebApplication webApplication, ILogger<T> logger)
        {
            webApplication.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError(contextFeature.Error.Message);
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            StatusCodeContext = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            AssemblyTitleAttribute = "Hata alındı!"
                        }));
                    }
                });
            });
        }
    }
}
