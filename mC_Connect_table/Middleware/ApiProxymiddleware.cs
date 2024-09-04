using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


public class ApiProxyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HttpClient _httpClient;
    private readonly string _baseApiUrl = "https://mc-connect-manager.smcs.io/api/v1";
    private readonly string _apiKey = "00a7d7db-37e9-4737-8400-5c778d6cc05c";
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
            // Construct the full URL by appending the remaining path to the base API URL
            var targetUrl = $"{_baseApiUrl}{remainingPath}";

            var externalRequest = new HttpRequestMessage(new HttpMethod(context.Request.Method), targetUrl);
            externalRequest.Headers.Add("Star-Api-Key", _apiKey);


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
            _logger.LogInformation("Index action invoked.\n\n\n\n\n\n\n\n\n\n\n\n");
            await _next(context);
        }
    }
}
