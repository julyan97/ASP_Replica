using ASP_Server_Copy.Core.Controller_Logic;
using ASP_Server_Copy.Core.CustomAttributes;
using ASP_Server_Copy.Core.Models.ControllerReturnTypes;

namespace ASP_Server_Copy.Web.Controllers
{
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetResponse()
        {
            return Content("hi");
        }
    }
}
