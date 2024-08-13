using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Api.Middleware
{
    public class MaskingService : IMaskingService
    {
        private readonly ILogger<MaskingService> _logger;

        public MaskingService(ILogger<MaskingService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string MaskJsonProperties(string body, IReadOnlyCollection<string> maskProperties, LogLevel logLevel = LogLevel.Information)
        {
            if (logLevel > LogLevel.Information || string.IsNullOrWhiteSpace(body)) { return body; }

            // Logging is best effort, any error that occurs shall be absorbed and not bubbled up.
            try
            {
                if (!TryParse(body, out var jObject)) { return body; }

                var jProperties = jObject.Properties().ToList();

                if (!jProperties.Any()) { return body; }

                foreach (var jProperty in jProperties)
                {
                    if (jProperty.Value.Type == JTokenType.Object)
                    {
                        jProperty.Value = JToken.Parse(MaskJsonProperties(jProperty.Value.ToString(), maskProperties));
                    }

                    var isAnyObjectInArray = jProperty.Value.Children().Any(x => x.Type == JTokenType.Object);

                    if (jProperty.Value.Type == JTokenType.Array && isAnyObjectInArray)
                    {
                        var arrayObjects = JsonConvert.DeserializeObject<object[]>(jProperty.Value.ToString());
                        var jArray = new JArray();
                        foreach (var arrayObject in arrayObjects)
                        {
                            jArray.Add(JToken.Parse(MaskJsonProperties(arrayObject.ToString(), maskProperties)));
                        }

                        jProperty.Value = jArray;
                    }

                    // To skip masking for null values.
                    var isCurrentKeyWithNonNullValue = !string.IsNullOrEmpty(jProperty.Value.ToString());
                    var isSensitiveKey =
                        maskProperties.Any(p => p.Equals(jProperty.Name, StringComparison.OrdinalIgnoreCase)) ||
                        maskProperties.Any(p => jProperty.Name.Contains(p, StringComparison.OrdinalIgnoreCase));

                    if (!isCurrentKeyWithNonNullValue || !isSensitiveKey) { continue; }

                    if (jProperty.Value.Type != JTokenType.Array)
                    {
                        jProperty.Value = LoggingConstant.MaskValue;
                        continue;
                    }

                    // Simple array of primitive data types.
                    if (jProperty.Value.Type == JTokenType.Array && !isAnyObjectInArray)
                    {
                        jProperty.Value = JToken.Parse(GetMaskedArrayString(jProperty.Value.ToString()));
                    }
                }

                return Regex.Unescape(Regex.Replace(jObject.ToString(), @"\r\n?|\n", string.Empty));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error masking request/response body!");
                return body;
            }
        }

        private static bool TryParse(string body, out JObject jObject)
        {
            try
            {
                jObject = JObject.Parse(body);
                return true;
            }
            catch
            {
                jObject = null;
                return false;
            }
        }

        private static string GetMaskedArrayString(string arrayString)
        {
            return JsonConvert.SerializeObject(
                (JsonConvert.DeserializeObject<string[]>(arrayString) ?? Enumerable.Empty<string>())
                .Select(s => !string.IsNullOrEmpty(s) ? LoggingConstant.MaskValue : s));
        }
    }
}
