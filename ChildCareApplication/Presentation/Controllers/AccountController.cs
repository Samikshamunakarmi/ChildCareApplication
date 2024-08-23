using ChildCareApplication.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text.RegularExpressions;

namespace ChildCareApplication.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class AccountController : Controller
    {
        public IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto accountDetail)
        {
            try
            {
                
                if (accountDetail.Password != accountDetail.ConfirmPassword)
                {
                    return BadRequest(new { error = "Passwords do not match" });
                }

                
                var emailVerifying = new Regex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
                if (!emailVerifying.IsMatch(accountDetail.Email))
                {
                    return BadRequest(new { error = "Email is not correct" });
                }

                
                var result = await _mediator.Send(accountDetail);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                
                return BadRequest(new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }


    }
}
