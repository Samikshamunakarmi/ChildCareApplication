using ChildCareApplication.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

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
                var result = await _mediator.Send(accountDetail);

                return Ok(result);
            }

            catch (InvalidOperationException ex)
            {
                // Return the error to the frontend
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                return BadRequest(new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }


    }
}
