using log4net;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Net;

namespace CRD.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private static readonly string messageCode = "ERROR";
        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(ErrorHandlingMiddleware));

        private readonly IConfiguration configuration;

        public ErrorHandlingMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            this.next = next;
            this.configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (SqlException ex)
            {
                log.Error(ex);
                await HandleExceptionAsync(context, ex, configuration.GetValue<bool>("LogFullInfo"));

            }
            catch (System.Exception ex)
            {
                log.Error(ex);
                await HandleExceptionAsync(context, ex, configuration.GetValue<bool>("LogFullInfo"));
            }
        }
        private static Task WriteSQLExceptionAsResponseAsync(HttpContext context, SqlException exception, bool logFullInfo = false)
        {
            var code = HttpStatusCode.BadRequest;

            return WriteSQLExceptionAsResponseAsync(context, exception, code, logFullInfo);
        }
        private static Task HandleExceptionAsync(HttpContext context, System.Exception exception, bool logFullInfo = false)
        {
            var code = HttpStatusCode.BadRequest;

            return WriteExceptionAsync(context, exception, code, logFullInfo);
        }


        private async static Task WriteSQLExceptionAsResponseAsync(HttpContext context, SqlException exception, HttpStatusCode code, bool logFullInfo = false)
        {
            log.Error(exception);
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)code;

            if (logFullInfo)
            {
                await response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    error = new
                    {
                        message = messageCode,
                        exception = exception
                    }
                }));
            }

            if (exception is Exception)
            {

                await response.WriteAsync(JsonConvert.SerializeObject(exception));

                return;
            }

            if (!context.Request.Path.Value.Contains("/swagger"))
            {
                await response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    error = new
                    {
                        message = messageCode
                    }
                }));
            }
        }
        private async static Task WriteExceptionAsync(HttpContext context, System.Exception exception, HttpStatusCode code, bool logFullInfo = false)
        {
            log.Error(exception);
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)code;

            if (logFullInfo)
            {
                await response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    error = new
                    {
                        message = messageCode,
                        exception = exception
                    }
                }));
            }

            if (exception is Exception)
            {
                await response.WriteAsync(JsonConvert.SerializeObject(exception));

                return;
            }

            if (!context.Request.Path.Value.Contains("/swagger"))
            {
                await response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    error = new
                    {
                        message = messageCode
                    }
                }));
            }
        }

    }
}
