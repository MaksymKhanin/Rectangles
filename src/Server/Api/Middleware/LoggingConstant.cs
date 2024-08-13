using System.Collections.Generic;

namespace Api.Middleware
{
    public static class LoggingConstant
    {
        public const string MaskValue = "***MASKED***";
        public const string ResponseRequestIdKey = "X-RequestId";

        private const string RequestTemplate =
            "{RequestScheme} {RequestMethod} {RequestPath} requested\n" +
            "AcceptLanguage: {AcceptLanguage}\n" +
            "RequestBody: {RequestBody}\n" +
            "CustomerId: {@CustomerId}\n";

        private const string ResponseTemplate =
            "{RequestScheme} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms\n" +
            "AcceptLanguage: {AcceptLanguage}\n" +
            "RequestBody: {RequestBody}\n" +
            "ResponseBody: {ResponseBody}\n" +
            "CustomerId: {@CustomerId}\n";

        public const string StartMessageTemplate = "Log " + RequestTemplate;
        public const string EndMessageTemplate = "Log " + ResponseTemplate;

        public const string CustomerDetailsNotAvailable = "N/A";
        public const string GenericErrorMessage =
            "Internal server error, please use the RequestId to query for error details on App Insights.";

        public const string CustomerInfoMessageTemplate =
            "Attempt to get {CustomerInfo} for AllowAnonymous endpoint";

        private static List<string> _maskProperties => new()
    {
        "FirstName", "MiddleName", "LastName", "DateOfBirth", "DocumentNumber",
        "Address","Email", "Phone", "MobileNumber"
    };

        public static IReadOnlyCollection<string> MaskProperties => _maskProperties;
    }
}
