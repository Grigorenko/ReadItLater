using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReadItLater.Web.Server.Utils
{
    public class RequestResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RequestResponseOptions options;

        public RequestResponseMiddleware(RequestDelegate next, RequestResponseOptions options)
        {
            _next = next;
            this.options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            IDictionary<string, string>? requestParams = await GetRequestData(context);
            IDictionary<string, string>? responseParams = null;

            if (!options.Response.IsShowed)
                await _next.Invoke(context);

            else 
                responseParams = await GetResponseData(context);

            WriteToConsole(requestParams, responseParams);
        }

        private async Task<IDictionary<string, string>?> GetRequestData(HttpContext context)
        {
            if (!options.Request.IsShowed)
                return null;

            var request = context.Request;
            var requestParams = new Dictionary<string, string>();

            if (options.Request.ShowMethod)
                requestParams.Add(nameof(request.Method), request.Method);

            if (options.Request.ShowPath)
                requestParams.Add(nameof(request.Path), request.Path);

            if (options.Request.ShowQueryString && request.QueryString.HasValue)
                requestParams.Add(nameof(request.QueryString), request.QueryString.Value!);

            if (options.Request.ShowHeaders)
            {
                var all = request.Headers
                    .ToDictionary(a => a.Key, a => a.Value);
                var headers = options.Request.HeadersWhiteList?.Any() ?? false
                    ? all.Where(h => options.Request.HeadersWhiteList.Any(i => string.Equals(h.Key, i)))
                    : all;

                if (headers.Any())
                {
                    var headerStrings = headers
                        .Select(h => $"\t{h.Key}:\t{h.Value}");

                    requestParams.Add(nameof(request.Headers), "\n" + string.Join("\n", headerStrings));
                }
            }

            if (options.Request.ShowBody && options.Request.AllowHttpMethods.Any(m => string.Equals(request.Method, m.Method, StringComparison.OrdinalIgnoreCase)))
            {
                request.EnableBuffering();

                using (var reader = new StreamReader(
                    request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    leaveOpen: true))
                {
                    var json = await reader.ReadToEndAsync();
                    request.Body.Position = 0;

                    if (!string.IsNullOrEmpty(json))
                    {
                        var body = JsonSerializer.Serialize(JsonSerializer.Deserialize<dynamic>(json), new JsonSerializerOptions { WriteIndented = true });
                        requestParams.Add(nameof(request.Body), "\n" + body);
                    }
                }
            }

            return requestParams;
        }

        private async Task<IDictionary<string, string>?> GetResponseData(HttpContext context)
        {
            if (!options.Response.IsShowed)
                return null;

            var response = context.Response;
            var responseParams = new Dictionary<string, string>();
            string? bodyStr = null;

            if (options.Response.ShowBody)
            {
                using (var responseBody = new MemoryStream())
                {
                    var originalBodyStream = response.Body;
                    response.Body = responseBody;

                    await _next.Invoke(context);

                    responseBody.Seek(0, SeekOrigin.Begin);

                    if (responseBody.CanRead)
                    {
                        var json = await new StreamReader(responseBody).ReadToEndAsync();

                        if (!string.IsNullOrEmpty(json) && !json.StartsWith("<!DOCTYPE html>"))
                        {
                            var body = JsonSerializer.Serialize(JsonSerializer.Deserialize<dynamic>(json), new JsonSerializerOptions { WriteIndented = true });
                            bodyStr = "\n" + body;
                        }
                    }

                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                    response.Body = originalBodyStream;
                }
            }

            else
                await _next.Invoke(context);

            if (options.Response.ShowStatusCode)
                responseParams.Add(nameof(response.StatusCode), response.StatusCode.ToString());

            if (options.Response.ShowHeaders)
            {
                var headers = response.Headers
                    .ToDictionary(a => a.Key, a => a.Value);

                if (headers.Any())
                {
                    var headerStrings = headers
                        .Select(h => $"\t{h.Key}:\t{h.Value}");

                    responseParams.Add(nameof(response.Headers), "\n" + string.Join("\n", headers));
                }
            }

            if (options.Response.ShowBody && !string.IsNullOrEmpty(bodyStr))
                responseParams.Add(nameof(response.Body), bodyStr);

            return responseParams;
        }

        private void WriteToConsole(IDictionary<string, string>? requestParams, IDictionary<string, string>? responseParams)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\n" + new string('=', 70));
            Console.ResetColor();

            if (requestParams != null)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("REQUEST");
                Console.ResetColor();

                requestParams.ToList()
                    .ForEach(p => Console.WriteLine($"{p.Key}:\t{p.Value}"));
            }

            if (responseParams != null)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("RESPONSE");
                Console.ResetColor();

                responseParams.ToList()
                    .ForEach(p =>
                    {
                        if (options.Response.HighlightUnsuccessfulStatusCode &&
                            string.Equals(p.Key, "StatusCode") &&
                            !string.Equals(p.Value, "200")
                        )
                            Console.ForegroundColor = ConsoleColor.Red;

                        Console.WriteLine($"{p.Key}:\t{p.Value}");
                        Console.ResetColor();
                    });
            }


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\n" + new string('=', 70));
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    public class RequestResponseOptions
    {
        public RequestOptions Request { get; set; } = new RequestOptions();
        public ResponseOptions Response { get; set; } = new ResponseOptions();

        public class RequestOptions
        {
            public bool IsShowed { get; set; } = true;
            public bool ShowMethod { get; set; } = true;
            public bool ShowPath { get; set; } = true;
            public bool ShowQueryString { get; set; } = true;
            public bool ShowHeaders { get; set; } = true;
            public bool ShowBody { get; set; } = false;
            /// <summary>
            /// Describe which http methods allow write body to log.
            /// By default: HttpMethod.Post & HttpMethod.Put
            /// </summary>
            public HttpMethod[] AllowHttpMethods { get; set; } = new[] { HttpMethod.Post, HttpMethod.Put };
            public string[] HeadersWhiteList { get; set; } = new[] { HeaderNames.Authorization, HeaderNames.ContentType };
        }

        public class ResponseOptions
        {
            public bool IsShowed { get; set; } = false;
            public bool ShowStatusCode { get; set; } = true;
            public bool ShowHeaders { get; set; } = true;
            public bool ShowBody { get; set; } = false;
            public bool HighlightUnsuccessfulStatusCode { get; set; } = true;
        }
    }
}
