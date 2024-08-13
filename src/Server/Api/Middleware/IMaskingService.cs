using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Api.Middleware
{
    public interface IMaskingService
    {
        string MaskJsonProperties(string body, IReadOnlyCollection<string> maskProperties, LogLevel logLevel = LogLevel.Information);
    }
}
