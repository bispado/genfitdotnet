namespace GenFit.API.Middleware;

public class ApiKeyAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ApiKeyAuthenticationMiddleware> _logger;

    public ApiKeyAuthenticationMiddleware(
        RequestDelegate next,
        IConfiguration configuration,
        ILogger<ApiKeyAuthenticationMiddleware> logger)
    {
        _next = next;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Permitir acesso sem API Key para endpoints públicos (health, swagger, etc.)
        if (context.Request.Path.StartsWithSegments("/health") ||
            context.Request.Path.StartsWithSegments("/swagger") ||
            context.Request.Path.StartsWithSegments("/api/v1/wellcome"))
        {
            await _next(context);
            return;
        }

        // Verificar se o endpoint requer autenticação
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.IAuthorizeData>() == null)
        {
            await _next(context);
            return;
        }

        // Obter a API Key do header configurado
        var headerName = _configuration["ApiKey:HeaderName"] ?? "X-API-Key";
        var expectedApiKey = _configuration["ApiKey:Value"];

        if (string.IsNullOrEmpty(expectedApiKey))
        {
            _logger.LogWarning("API Key não configurada. Permite acesso sem validação.");
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(headerName, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key não fornecida.");
            return;
        }

        if (!extractedApiKey.Equals(expectedApiKey))
        {
            _logger.LogWarning("Tentativa de acesso com API Key inválida de {IP}", context.Connection.RemoteIpAddress);
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key inválida.");
            return;
        }

        await _next(context);
    }
}

public static class ApiKeyAuthenticationMiddlewareExtensions
{
    public static IApplicationBuilder UseApiKeyAuthentication(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ApiKeyAuthenticationMiddleware>();
    }
}
