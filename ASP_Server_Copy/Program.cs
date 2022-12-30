using ASP_Server_Copy.Core;
using System.Net;

namespace ASP_Server_Copy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var ip = IPAddress.Parse("127.0.0.1");
            var port = 8000;

            Server server = new Server(ip, port, typeof(Program));
            server.Start();
        }
    }
}