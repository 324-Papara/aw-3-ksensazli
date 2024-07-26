using System.Text;

namespace Para.Api.Middleware;

public class RequestResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseMiddleware> _logger;

    public RequestResponseMiddleware(RequestDelegate next, ILogger<RequestResponseMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestLog = await LogRequestAsync(context.Request);
        _logger.LogInformation(requestLog);

        var originalBodyStream = context.Response.Body;
        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            await _next(context);

            var responseLog = await LogResponseAsync(context.Response);
            _logger.LogInformation(responseLog);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private async Task<string> LogRequestAsync(HttpRequest request)
    {
        request.EnableBuffering();

        var bodyAsText = await ReadStreamInChunksAsync(request.Body);

        request.Body.Position = 0;

        var requestLog = new StringBuilder();
        requestLog.AppendLine("HTTP Request Information:");
        requestLog.AppendLine($"Schema: {request.Scheme}");
        requestLog.AppendLine($"Host: {request.Host}");
        requestLog.AppendLine($"Path: {request.Path}");
        requestLog.AppendLine($"QueryString: {request.QueryString}");
        requestLog.AppendLine($"Request Body: {bodyAsText}");

        return requestLog.ToString();
    }

    private async Task<string> LogResponseAsync(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        var responseLog = new StringBuilder();
        responseLog.AppendLine("HTTP Response Information:");
        responseLog.AppendLine($"Status Code: {response.StatusCode}");
        responseLog.AppendLine($"Response Body: {text}");

        return responseLog.ToString();
    }

    private async Task<string> ReadStreamInChunksAsync(Stream stream)
    {
        const int bufferLength = 4096;
        var buffer = new byte[bufferLength];
        var readStringBuilder = new StringBuilder();

        int bytesRead;
        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            readStringBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
        }

        return readStringBuilder.ToString();
    }
}