using System.Net;
using System.Text;

namespace MVC.Server
{
    public class HttpServer
    {
        private readonly Router _router;

        public HttpServer(Router router)
        {
            _router = router;
        }

        public async Task StartAsync(string prefix)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(prefix);
            listener.Start();
            Console.WriteLine($"Server running at {prefix}");
            Console.WriteLine("Press CTRL+C to stop...");

            while (true)
            {
                var context = await listener.GetContextAsync();
                await HandleRequest(context);
            }
        }

        private async Task HandleRequest(HttpListenerContext context)
        {
            var response = context.Response;

            try
            {
                string responseString = await _router.RouteAsync(context.Request);
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);

                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                var error = Encoding.UTF8.GetBytes($"{{\"error\":\"{ex.Message}\"}}");
                await response.OutputStream.WriteAsync(error, 0, error.Length);
            }
            finally
            {
                response.OutputStream.Close();
            }
        }
    }
}
