
using System.Net;
using System.Text;
using System.Text.Json;
using MVC.Controllers;
using MVC.Models;
using MVC.Helpers;
using MVC.Data; // Giả sử DIScope, DI config, DBHelper ở đây

class Program
{
    static async Task Main()
    {
        // ===== Khởi tạo database =====
        TestDatabaseHelper.InitDatabase();

        // ===== Khởi tạo DI =====
        var diConfig = AppDependencyInjectionConfig.CreateConfig();
        using var scope = new DIScope(diConfig);

        // Resolve controller từ DI
        var controller = scope.Resolve<AccountController>();

        // ===== Khởi tạo HTTP server =====
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:5000/");
        listener.Start();
        Console.WriteLine("Server running at http://localhost:5000/");
        Console.WriteLine("Press CTRL+C to stop...");

        while (true)
        {
            var context = await listener.GetContextAsync();
            var request = context.Request;
            var response = context.Response;

            try
            {
                string responseString = "";

                // ===== Router cơ bản =====
                if (request.Url.AbsolutePath == "/account/create" && request.HttpMethod == "POST")
                {
                    using var reader = new System.IO.StreamReader(request.InputStream);
                    var json = await reader.ReadToEndAsync();
                    var account = JsonSerializer.Deserialize<Account>(json);
                    var id = await controller.CreateAccount(account!);
                    responseString = $"{{\"id\":{id}}}";
                }
                else if (request.Url.AbsolutePath == "/account/all" && request.HttpMethod == "GET")
                {
                    var accounts = await controller.GetAllAccount();
                    responseString = JsonSerializer.Serialize(accounts);
                }
                else if (request.Url.AbsolutePath.StartsWith("/account/") && request.HttpMethod == "GET")
                {
                    var parts = request.Url.AbsolutePath.Split('/');
                    if (parts.Length == 3 && int.TryParse(parts[2], out int id))
                    {
                        var acc = await controller.GetAccountById(id);
                        responseString = JsonSerializer.Serialize(acc);
                    }
                }
                
                else
                {
                    response.StatusCode = 404;
                    responseString = "{\"error\":\"Not found\"}";
                }

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
