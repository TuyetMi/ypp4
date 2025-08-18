//namespace MVC.Router
//{
//    public class Router
//    {
//        private readonly Dictionary<(string Method, string Path), Func<object?, Task>> _routes
//            = new();

//        public void Register(string method, string path, Func<object?, Task> action)
//        {
//            _routes[(method.ToUpper(), path.ToLower())] = action;
//        }

//        public async Task RouteAsync(string method, string path, object? body = null)
//        {
//            if (_routes.TryGetValue((method.ToUpper(), path.ToLower()), out var action))
//            {
//                await action(body);
//            }
//            else
//            {
//                Console.WriteLine("404 Not Found");
//            }
//        }
//    }

//}
