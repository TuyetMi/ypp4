namespace MVC.Server
{
    public class RouteDefinition
    {
        public string Method { get; }
        public string Template { get; }
        public Func<RouteContext, (string, string)> Handler { get; }

        private readonly string[] _segments;

        public RouteDefinition(string method, string template, Func<RouteContext, (string, string)> handler)
        {
            Method = method;
            Template = template;
            Handler = handler;
            _segments = template.Trim('/').Split('/');
        }

        public Dictionary<string, string>? Match(string method, string path)
        {
            if (!string.Equals(method, Method, StringComparison.OrdinalIgnoreCase))
                return null;

            var parts = path.Trim('/').Split('/');
            if (parts.Length != _segments.Length) return null;

            var routeParams = new Dictionary<string, string>();
            for (int i = 0; i < _segments.Length; i++)
            {
                if (_segments[i].StartsWith("{") && _segments[i].EndsWith("}"))
                {
                    var key = _segments[i].Trim('{', '}');
                    routeParams[key] = parts[i];
                }
                else if (!string.Equals(_segments[i], parts[i], StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }
            }

            return routeParams;
        }
    }
}
