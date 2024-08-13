using Microsoft.AspNetCore.Http;

namespace Api.Middleware
{
    public interface IHttpContextService
    {
        string GetTraceId();
    }

    public class HttpContextService : IHttpContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetTraceId()
        {
            return _httpContextAccessor.HttpContext!.TraceIdentifier;
        }
    }
}
