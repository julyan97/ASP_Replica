using ASP_Server_Copy.Core.Helpers;
using ASP_Server_Copy.Core.Models.ControllerReturnTypes;

namespace ASP_Server_Copy.Core.Controller_Logic
{
    public abstract class ControllerBase
    {
        public ActionResult View(string viewName)
        {
            var assembly = Server.ExecutingAssembly;
            var executingController = this.GetType().Name;

            string binDebugPath = Path.GetDirectoryName(assembly.Location);
            var path = $@"{binDebugPath}\View\{executingController.Replace("Controller", "")}\{viewName}";

            var result = File.ReadAllText(path + ".html");
            Console.WriteLine(result);
            return new ActionResult { Content = result, ShouldBeRaw = true };
        }

        public ActionResult Content(string content, int status = 200)
        {
            var response = ResponseHelper.Create(content, status);
            return new ActionResult() { Content = response, ShouldBeRaw = false };
        }

        public ActionResult Status(int status = 200)
        {
            var response = ResponseHelper.Create("", status);
            return new ActionResult() { Content = response, ShouldBeRaw = false };
        }
    }
}
