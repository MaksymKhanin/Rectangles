using System.Collections.Generic;

namespace Api.Middleware
{
    public static class LoggingConstant
    {
        public const string MaskValue = "***MASKED***";
        public const string ResponseRequestIdKey = "X-RequestId";

        private const string RequestTemplate =
            "{RequestScheme} {RequestMethod} {RequestPath} requested\n" +
            "RequestBody: {RequestBody}\n";

        private const string ResponseTemplate =
            "{RequestScheme} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms\n" +
            "RequestBody: {RequestBody}\n" +
            "ResponseBody: {ResponseBody}\n";

        public const string StartMessageTemplate = "Log " + RequestTemplate;
        public const string EndMessageTemplate = "Log " + ResponseTemplate;

        public const string CustomerDetailsNotAvailable = "N/A";
        public const string GenericErrorMessage = "Internal server error, please use the RequestId to query for error details on App Insights.";


        private static List<string> _maskProperties => new()
        {
            "FirstName", "MiddleName", "LastName", "DateOfBirth", "DocumentNumber",
            "Address","Email", "Phone", "MobileNumber"
        };

        public static IReadOnlyCollection<string> MaskProperties => _maskProperties;
    }
}
