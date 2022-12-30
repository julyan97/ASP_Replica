# ASP_Server_Copy replica of ASP.Net 
The replica has most of the functionalities of ASP.Net MVC and Web API

## Functionalities:
# Controllers and Methods:
1. Create a controller
2. Create a method in the Controller
3. Add an attribute to the method to describe the type of the request method
4. Return a View, Content with a status code or just Status Code

```csharp
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
        
        [HttpGet]
        public ActionResult GetResponse3()
        {
            return Status(200);
        }
    }
}
```
