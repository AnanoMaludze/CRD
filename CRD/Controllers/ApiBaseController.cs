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

        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(ApiBaseController));

        protected ApiBaseController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        protected IActionResult JsonContent(object message)
        {
            try
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
            catch (Exception)
            {

                throw;
            }
        }
    }
}
