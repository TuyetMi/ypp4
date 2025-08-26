using System;
using System.Net;
using System.Text;

namespace MVC.Models
{
    public class HttpServer
    {
        private readonly Router _router = new Router();

        public async Task StartAsync()
        {
            using var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/");
            listener.Start();
            Console.WriteLine("Server running at http://localhost:5000/");

            while (true)
            {
                var context = await listener.GetContextAsync();
                var request = context.Request;
                var response = context.Response;

                var path = request.Url.AbsolutePath;
                var method = request.HttpMethod;

                var (content, contentType) = _router.Route(path, method);

                var buffer = Encoding.UTF8.GetBytes(content);
                response.ContentType = contentType;
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
        }
    }
}