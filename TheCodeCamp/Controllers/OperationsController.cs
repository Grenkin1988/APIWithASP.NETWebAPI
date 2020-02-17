using System.Configuration;
using System.Web.Http;

namespace TheCodeCamp.Controllers {
    public class OperationsController : BaseApiController {
        [Route("api/refreshconfig")]
        [HttpOptions]
        public IHttpActionResult RefreshAppSettings() {
            IHttpActionResult Function() {
                ConfigurationManager.RefreshSection("AppSettings");
                return Ok();
            }
            return SafeExecute(Function);
        }
    }
}
