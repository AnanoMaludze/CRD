using log4net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Http.Extensions;

namespace CRD.Middleware
{

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class CheckMiddleware : ActionFilterAttribute
    {
        private readonly IConfiguration Configuration;
        private readonly RequestDelegate _next;
        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(CheckMiddleware));

        public CheckMiddleware(IConfiguration configuration, RequestDelegate next)
        {
            this.Configuration = configuration;
            _next = next;

        }


        public async Task Invoke(HttpContext httpContext)
        {

            var exclUrls = new List<string> { "/swagger/index.html", "/swagger/v1/swagger.json" };

            if (exclUrls.Contains(httpContext.Request.Path.Value))
            {
                await _next.Invoke(httpContext);
            }
            else
            {
                log.Debug($"Method: {httpContext.Request.Method}");

                if (!string.IsNullOrWhiteSpace(httpContext.Request.Path.Value))
                {

                    log.Debug( $"Path: {httpContext.Request.Path.Value}");
                }


                httpContext.Request.EnableBuffering();

                string requestBody = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();

                httpContext.Request.Body.Position = 0;

                if (!string.IsNullOrWhiteSpace(requestBody))
                {
                    log.Debug($"Body: {requestBody}");
                }

                await _next.Invoke(httpContext);
            }

        }
    }
}
