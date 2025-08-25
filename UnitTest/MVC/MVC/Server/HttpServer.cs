using System.Net;
using System.Text;

namespace MVC.Server
{
    public class HttpServer
    {
        private readonly Router _router;
        private readonly HttpListener _listener;

        public HttpServer(Router router)
        {
            _router = router;
            _listener = new HttpListener();
        }

        public async Task StartAsync(string prefix)
        {
            _listener.Prefixes.Add(prefix);
            _listener.Start();
            Console.WriteLine($"🚀 Server started at {prefix}");

            while (true)
            {
                var context = await _listener.GetContextAsync();
                _ = ProcessRequestAsync(context);
            }
        }

        private async Task ProcessRequestAsync(HttpListenerContext context)
        {
            try
            {
                string responseJson = await _router.RouteAsync(context.Request);

                context.Response.ContentType = "application/json";
                using var writer = new StreamWriter(context.Response.OutputStream);
                await writer.WriteAsync(responseJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                context.Response.StatusCode = 500;
                using var writer = new StreamWriter(context.Response.OutputStream);
                await writer.WriteAsync("{\"error\":\"Internal server error\"}");
            }
            finally
            {
                context.Response.OutputStream.Close();
            }
        }
    }
}
