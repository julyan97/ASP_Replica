using System.Reflection;
using System.Text.RegularExpressions;

namespace ASP_Server_Copy.Core.Helpers
{
    public static class ControllerHelper
    {
        private static string[] httpAttributes = { "HttpGetAttribute", "HttpPostAttribute", "HttpPutAttribute", "HttpDeleteAttribute" };


        public static string GetAttributeName(string incomingRequestMethod)
        {
            switch(incomingRequestMethod)
            {
                case "GET":
                {
                    return "HttpGetAttribute";
                }
                case "POST":
                {
                    return "HttpPostAttribute";
                }
                case "PUT":
                {
                    return "HttpPutAttribute";
                }
                case "DELETE":
                {
                    return "HttpDeleteAttribute";
                }
            }
            return null;
        }


        public static void BuildEndpoints(Dictionary<string, Dictionary<string, List<MethodInfo>>> endpoints, Assembly assembly)
        {
            Type[] types = assembly.GetTypes();

            types = types.Where(x => Regex.IsMatch(x.Name, @"(?<name>[A-Za-z])Controller")).ToArray();

            foreach(Type type in types)
            {
                var methods = type.GetMethods().Where(x => x.IsPublic && !x.IsVirtual);
                foreach(var method in methods)
                {
                    var requestMethodAttribute = method.GetCustomAttributes(false)
                        .Where(x => httpAttributes.Contains(x.GetType().Name))
                        .FirstOrDefault()?
                        .GetType()
                        .Name;

                    if(requestMethodAttribute is null)
                        continue;



                    if(!endpoints.ContainsKey(requestMethodAttribute))
                    {
                        endpoints[requestMethodAttribute] = new Dictionary<string, List<MethodInfo>>();
                    }

                    if(!endpoints[requestMethodAttribute].ContainsKey(type.Name))
                    {
                        endpoints[requestMethodAttribute].Add(type.Name, new List<MethodInfo>());
                    }

                    endpoints[requestMethodAttribute][type.Name].Add(method);

                }
            }
        }
    }
}
