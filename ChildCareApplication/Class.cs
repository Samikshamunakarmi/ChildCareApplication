using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

public class JwtValidationLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JwtValidationLoggingMiddleware> _logger;

    public JwtValidationLoggingMiddleware(RequestDelegate next, ILogger<JwtValidationLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            var token = context.Request.Headers["Authorization"].ToString().Split(" ").Last();
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(context.RequestServices.GetRequiredService<IConfiguration>()["JWT:Key"]);

            try
            {
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = context.RequestServices.GetRequiredService<IConfiguration>()["JWT:Issuer"],
                    ValidAudience = context.RequestServices.GetRequiredService<IConfiguration>()["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                _logger.LogInformation("Token validated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Token validation failed: {ex.Message}");
            }
        }

        await _next(context);
    }
}

public static class JwtValidationLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseJwtValidationLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<JwtValidationLoggingMiddleware>();
    }
}
