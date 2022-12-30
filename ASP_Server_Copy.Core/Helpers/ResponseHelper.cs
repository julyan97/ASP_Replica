using System.Text;

namespace ASP_Server_Copy.Core.Helpers
{
    public static class ResponseHelper
    {
        public static string Create(string content, int code, Dictionary<string, string> headers = default!)
        {
            var resultLength = Encoding.UTF8.GetBytes($"{content}").Length;
            var defaultHeaders = $"Content-Type: text/html; charset=UTF-8\r\nContent-Length: {resultLength}";

            if(headers is null)
            {
                headers = new Dictionary<string, string>();
            }

            headers.Add("Content-Type", "text/html; charset=UTF-8");
            headers.Add("Content-Length", $"{resultLength}");

            StringBuilder sb = new StringBuilder();

            sb.Append("HTTP/1.1 ")
                .AppendLine($"{code}");

            foreach(var header in headers)
            {
                sb.AppendLine($"{header.Key}: {header.Value}");
            }

            sb.AppendLine()
              .Append(content);

            return sb.ToString();
        }
    }
}
