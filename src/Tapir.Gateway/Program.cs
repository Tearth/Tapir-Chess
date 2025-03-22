using System.Net.Http.Headers;
using Yarp.ReverseProxy.Transforms;

namespace Tapir.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services
                .AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
                .AddTransforms(transformBuilderContext =>
                {
                    transformBuilderContext.AddRequestTransform(async transformContext =>
                    {
                        var accessToken = transformContext.HttpContext.Request.Cookies["access_token"];
                        var refreshToken = transformContext.HttpContext.Request.Cookies["refresh_token"];

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            transformContext.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        }

                        if (!string.IsNullOrEmpty(refreshToken))
                        {
                            transformContext.ProxyRequest.Headers.Add("X-Refresh-Token", "Basic " + refreshToken);
                        }
                    });
                });

            var app = builder.Build();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.MapReverseProxy();
            app.Run();
        }
    }
}
