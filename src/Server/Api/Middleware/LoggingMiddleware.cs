using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Api.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMaskingService _maskingService;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger, IMaskingService maskingService)
        {
            _next = next
                ?? throw new ArgumentNullException(nameof(next));
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            _maskingService = maskingService
                ?? throw new ArgumentNullException(nameof(maskingService));
        }

        public async Task InvokeAsync(HttpContext context, IHttpContextService httpContextService)
        {
            if (context is null) { throw new ArgumentNullException(nameof(context)); }

            var start = Stopwatch.GetTimestamp();
            var responseBodyStream = new MemoryStream();
            var originalResponseBodyStream = context.Response.Body;
            var requestBody = await GetBodyAsync(context.Request);

            try
            {
                var maskedRequestBody = _maskingService.MaskJsonProperties(requestBody, LoggingConstant.MaskProperties);

                ProcessStartLog(maskedRequestBody, context, httpContextService);

                context.Response.Body = responseBodyStream;
                context.Response.Headers.Add(LoggingConstant.ResponseRequestIdKey, context.TraceIdentifier);

                await _next(context);
            }
            finally
            {
                var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());
                await ProcessEndLogAsync(elapsedMs, requestBody, context, responseBodyStream, originalResponseBodyStream, httpContextService);

                await responseBodyStream.DisposeAsync();
                context.Response.Body = originalResponseBodyStream;
            }
        }

        private void ProcessStartLog(string maskedRequestBody, HttpContext httpContext, IHttpContextService httpContextService)
        {
            _logger.LogInformation(LoggingConstant.StartMessageTemplate, GetRequestScheme(httpContext), httpContext.Request.Method, GetPath(httpContext), maskedRequestBody);
        }

        private async Task ProcessEndLogAsync(double elapsedMs, string maskedRequestBody, HttpContext httpContext,
            MemoryStream responseBodyStream, Stream originalResponseBodyStream, IHttpContextService httpContextService, Exception exception = default)
        {
            var responseBody = await GetBodyAsync(httpContext.Response);
            var logLevel = GetLogLevel(httpContext.Response.StatusCode, exception);

            var maskedResponseBody = _maskingService.MaskJsonProperties(responseBody, LoggingConstant.MaskProperties, logLevel);

            _logger.Log(logLevel, exception, LoggingConstant.EndMessageTemplate, GetRequestScheme(httpContext), httpContext.Request.Method,
                GetPath(httpContext), httpContext.Response.StatusCode, elapsedMs, maskedRequestBody, maskedResponseBody);

            await responseBodyStream.CopyToAsync(originalResponseBodyStream);
        }

        private static LogLevel GetLogLevel(int statusCode, Exception exception)
        {
            if (exception is not null) { return LogLevel.Error; }

            return !IsSuccessStatusCode(statusCode)
                ? LogLevel.Error
                : LogLevel.Information;
        }

        private static string GetRequestScheme(HttpContext httpContext)
        {
            return httpContext.Request.IsHttps ? "HTTPS" : "HTTP";
        }

        private static double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }

        private static string GetPath(HttpContext httpContext)
        {
            return httpContext.Features.Get<IHttpRequestFeature>()?.RawTarget ??
                   httpContext.Request.Path.ToString() + httpContext.Request.QueryString;
        }

        private static async Task<string> GetBodyAsync(HttpRequest request)
        {
            request.EnableBuffering();

            using var streamReader = new StreamReader(request.Body, leaveOpen: true);
            var requestBody = await streamReader.ReadToEndAsync();

            _ = request.Body.Seek(0, SeekOrigin.Begin);

            return string.IsNullOrWhiteSpace(requestBody) ? null : requestBody;
        }

        private static async Task<string> GetBodyAsync(HttpResponse response)
        {
            _ = response.Body.Seek(0, SeekOrigin.Begin);

            using var streamReader = new StreamReader(response.Body, leaveOpen: true);
            var responseBody = await streamReader.ReadToEndAsync();

            _ = response.Body.Seek(0, SeekOrigin.Begin);

            return string.IsNullOrWhiteSpace(responseBody) ? null : responseBody;
        }

        private static bool IsSuccessStatusCode(int statusCode)
        {
            return statusCode is >= 200 and <= 299;
        }
    }


}
