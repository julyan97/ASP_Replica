using System.Reflection;
using System.Text.RegularExpressions;

namespace ASP_Server_Copy.Core.Helpers
{
    public static class MethodHelper
    {
        public static object? ExecuteMethod(string className, MethodInfo method, object?[]? parameters = default)
        {
            Assembly assembly = Server.ExecutingAssembly;
            Type[] types = assembly.GetTypes();

            types = types.Where(x => Regex.IsMatch(x.Name, className)).ToArray();
            Type? type = types.FirstOrDefault();
            object? result = null;

            if(type is not null)
            {
                object? obj = Activator.CreateInstance(type);
                if(parameters.All(x => x == null))
                    parameters = null;

                result = method.Invoke(obj, parameters);
            }
            else
            {
                Console.WriteLine($"Type '{className}' not found.");
            }

            return result;
        }

        public static object GetInstansiatedMethodArgumentOfAMethod(MethodInfo method, Dictionary<string, string> props = default!)
        {
            var parameters = method.GetParameters();
            object result = default!;
            for(int i = 0; i < parameters.Length; i++)
            {
                var typeOfParameter = parameters[i].ParameterType;
                var instance = Activator.CreateInstance(typeOfParameter);

                var instanceProperties = instance.GetType().GetProperties();
                for(int j = 0; j < instanceProperties.Length; j++)
                {
                    var propName = instanceProperties[j].Name.ToLower();
                    if(props is not null && props.ContainsKey(propName))
                    {
                        instanceProperties[j].SetValue(instance, props[propName]);
                    }
                }
                result = instance;
            }

            return result;
        }
    }
}
