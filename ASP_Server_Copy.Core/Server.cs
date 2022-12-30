using ASP_Server_Copy.Core.Helpers;
using ASP_Server_Copy.Core.Models.ControllerReturnTypes;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace ASP_Server_Copy.Core
{
    public class Server
    {
        private TcpListener serverListener;
        private Dictionary<string, Dictionary<string, List<MethodInfo>>> endpoints;
        internal static Assembly ExecutingAssembly;

        public Server(IPAddress ipAddres, int port, Type executingAssembly)
        {
            serverListener = new TcpListener(ipAddres, port);
            endpoints = new Dictionary<string, Dictionary<string, List<MethodInfo>>>();
            ExecutingAssembly = executingAssembly.Assembly;
        }

        public void Start()
        {
            serverListener.Start();
            ControllerHelper.BuildEndpoints(endpoints, ExecutingAssembly);

            Console.WriteLine("Just started");

            while(true)
            {
                using TcpClient client = serverListener.AcceptTcpClient();
                using NetworkStream stream = client.GetStream();

                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string incomingData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    Dictionary<string, string>? meyhodParamsAsDictoinary;

                    //Bind the Controller, Method and Request Method
                    BindIncomingDataRequestLine(incomingData,
                        out string requestLine,
                        out string requestMethod, // Incoming RequestMethod : Get, Post, Put, Delete
                        out string controller, // Controller
                        out string controllerMethod, // Method in  Controller
                        out meyhodParamsAsDictoinary);// Method Parameters

                    Console.WriteLine($"{requestMethod}, {controller}, {controllerMethod}");

                    // From the requestMethod get the Attribute Get -> HttpGetAttribute that the data is bound to
                    var bindingAttributeToARequestMethod = ControllerHelper.GetAttributeName(requestMethod);

                    //Get Method fom the end points
                    var methodToInvoke = endpoints[bindingAttributeToARequestMethod][controller].FirstOrDefault(x => x.Name == controllerMethod);
                    
                    // Create instances of the parameters of the method and initialize their values from the query string
                    var methodParameters = MethodHelper.GetInstansiatedMethodArgumentOfAMethod(methodToInvoke, meyhodParamsAsDictoinary);


                    var result = (ActionResult)MethodHelper.ExecuteMethod(controller, methodToInvoke, new[] { methodParameters })!;
                    var response = result.Content;

                    // if the flag ShouldBeRaw is true, it mean we shourd create a new request and we should insert our content in its body
                    // if not, then we sould return the original result.conttent because its content already contains the created response 
                    if(result.ShouldBeRaw)
                    {
                        response = ResponseHelper.Create(result.Content, 200);
                    }

                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    stream.Write(responseBytes, 0, responseBytes.Length);
                    Console.WriteLine(response);
                    Console.WriteLine("---------------");
                    Console.WriteLine(requestLine);

                }
                catch
                {
                    continue;
                }
            }
        }

        private void BindIncomingDataRequestLine(string incomingData, out string requestLine, out string requestMethod, out string controller, out string controllerMethod, out Dictionary<string, string>? controllersParamsAsDictoinary)
        {
            string[] lines = incomingData.Split(new[] { "\r\n" }, StringSplitOptions.None);

            requestLine = lines[0];
            var requestLineParts = requestLine.Split();

            requestMethod = requestLineParts[0];
            controller = requestLineParts[1].Split("/")[1];
            var controllerMethodAndParamsBase = requestLineParts[1].Split("/")[2];
            var controllerMethodAndParams = controllerMethodAndParamsBase.Split("?");

            controllerMethod = controllerMethodAndParams[0];
            var controllerParams = controllerMethodAndParams.Length > 1 ? controllerMethodAndParams[1] : null;
            controllersParamsAsDictoinary = controllerParams?
                .Split("&")
                .ToDictionary(x => x.Split("=")[0], x => x.Split("=")[1]);
        }
    }
}
