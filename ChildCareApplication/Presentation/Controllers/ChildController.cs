﻿using ChildCareApplication.Application.CommandHandlers.ChildQueryHandler;
using ChildCareApplication.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChildCareApplication.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildController : ControllerBase
    {

        public IMediator _mediator;

        public ChildController(IMediator mediator )
        {
            _mediator = mediator;
        }

        [HttpPost("childDetail/create")]
        [Authorize]
        public async Task<IActionResult> AddChildDetail([FromBody] ChildInformation childInformation)
        {
            try
            {
                var result = await _mediator.Send(childInformation);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new {error = "An unexpected error occured.", details = ex.Message});
            }
        }

        [HttpGet("getListOfChildDetail")]
        [Authorize]

        public async Task<IActionResult> GetListOfChildDetail()
        {
            try
            {
                var result = _mediator.Send(new GetAllChildDetailsQuery());
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode('500', $"Intern server error:{ex.Message}");
            }
        }
    }
}
