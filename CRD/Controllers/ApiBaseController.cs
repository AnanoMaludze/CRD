using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using log4net;

namespace CRD.Controllers
{
    public class ApiBaseController : Controller
    {
        private readonly IConfiguration configuration;

        private readonly IHttpContextAccessor _httpContentAccessor;

        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(ApiBaseController));

        protected ApiBaseController(IConfiguration configuration, IHttpContextAccessor httpContentAccessor)
        {
            this.configuration = configuration;
            _httpContentAccessor = httpContentAccessor;
        }


        protected int GetUserID()
        {
            var result = string.Empty;

            if (_httpContentAccessor.HttpContext != null)
            {
                result = _httpContentAccessor.HttpContext.User?.Identity?.Name;
            }

            return Convert.ToInt32(result);
        }


        protected IActionResult JsonContent(object message)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var jsonContent = JsonConvert.SerializeObject(message, serializerSettings);

            log.Debug(jsonContent);

            return new ContentResult()
            {
                Content = jsonContent,
                ContentType = "application/json",
                StatusCode = 200
            };

        }
    }
}
