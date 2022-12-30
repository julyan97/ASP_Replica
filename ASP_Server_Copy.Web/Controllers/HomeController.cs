using ASP_Server_Copy.Core.Controller_Logic;
using ASP_Server_Copy.Core.CustomAttributes;
using ASP_Server_Copy.Core.Models.ControllerReturnTypes;

namespace ASP_Server_Copy.Web.Controllers
{
    public class User
    {
        public string Name { get; set; }
        public string Id { get; set; }

    }
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetResponse()
        {
            return View("Test");
        }

        [HttpGet]
        public ActionResult GetResponse2(User user)
        {
            return Content(user.Name + " " + user.Id);
        }
    }
}
