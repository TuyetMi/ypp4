using System;
using System.Net;
using System.Text;
using MVC.Models;

namespace MVC.Server
{
    public class HttpServer
    {
        private readonly HttpListener _listener;
        private readonly Router _router;

        public HttpServer(string[] prefixes, Router router)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException("HttpListener không được support trên hệ thống này.");

            _listener = new HttpListener();
            foreach (var prefix in prefixes)
            {
                _listener.Prefixes.Add(prefix);
            }

            _router = router;
        }

        public void Start()
        {
            _listener.Start();
            Console.WriteLine("🚀 Server started...");
            Console.WriteLine("Listening on: " + string.Join(", ", _listener.Prefixes));

            while (true)
            {
                var context = _listener.GetContext(); // blocking
                ProcessRequest(context);
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            Console.WriteLine($"[{DateTime.Now}] {request.HttpMethod} {request.Url?.AbsolutePath}");

            var (content, contentType) = _router.Route(request.Url!.AbsolutePath, request.HttpMethod);

            var buffer = Encoding.UTF8.GetBytes(content);
            response.ContentType = contentType;
            response.ContentLength64 = buffer.Length;

            using var output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    }
}
