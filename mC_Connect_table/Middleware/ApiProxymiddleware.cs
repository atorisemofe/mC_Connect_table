using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class ApiProxyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HttpClient _httpClient;
    private readonly string _baseApiUrl = "https://mc-connect-manager.smcs.io/api/v1";
    private readonly ILogger<ApiProxyMiddleware> _logger;

    public ApiProxyMiddleware(RequestDelegate next, ILogger<ApiProxyMiddleware> logger)
    {
        _next = next;
        _httpClient = new HttpClient();
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/proxy", out var remainingPath))
        {
            // Extract the Star-Api-Key from the incoming request headers
            if (!context.Request.Headers.TryGetValue("Star-Api-Key", out var apiKey) || string.IsNullOrEmpty(apiKey))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Star-Api-Key is required.");
                return;
            }

            // Construct the full URL by appending the remaining path to the base API URL
            var targetUrl = $"{_baseApiUrl}{remainingPath}";

            var externalRequest = new HttpRequestMessage(new HttpMethod(context.Request.Method), targetUrl);
            externalRequest.Headers.Add("Star-Api-Key", apiKey.ToString());

            if (context.Request.ContentLength > 0)
            {
                using (var reader = new StreamReader(context.Request.Body))
                {
                    var body = await reader.ReadToEndAsync();
                    externalRequest.Content = new StringContent(body, System.Text.Encoding.UTF8, context.Request.ContentType);
                }
            }

            var response = await _httpClient.SendAsync(externalRequest);
            var content = await response.Content.ReadAsStringAsync();

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;

            await context.Response.WriteAsync(content);
        }
        else
        {
            await _next(context);
        }
    }
}
