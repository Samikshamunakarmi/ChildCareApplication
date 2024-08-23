// Presentation/Controllers/AuthController.cs
using ChildCareApplication.Domain;
using ChildCareApplication.Presentation.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("[controller]")]
public class LoginController : Controller
{
    private readonly IConfiguration _configuration;
    public IMediator _mediator;
    public LoginController(IConfiguration configuration, IMediator mediator)
    {
        _configuration = configuration;
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginEntities login)
    {
        try
        {
            var result = await _mediator.Send(login);

            if(result)
            {
                var token = GenerateJwtToken(login.Email);
                return Ok(new { token });
            }
            
        }
        catch (Exception)
        {
            throw new Exception("The password or email is not correct");
        }

        return Unauthorized();



    }

    [AllowAnonymous]
    [HttpPost("resetPassword")]

    public async Task<IActionResult> ResetPassword([FromBody] ReSetPassword resetPassword)
    {
        if (resetPassword.NewPassword != resetPassword.ConfirmPassword)
        {
            return BadRequest(new { message = "The passwords didn't match" });
        }

        if(resetPassword.Email == null)
        {
            return BadRequest(new { message = "Please enter the email address" });
        }

        var result = await _mediator.Send(resetPassword);

        if(result)

        {
            return Ok(new { message = "Password reset successful. Please log in with your new password." });
        }

        return Unauthorized();
    }



    private string GenerateJwtToken(string userId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, userId),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    


}


