using Asp.Versioning;
using BoilerPlate.Application.Features.UserManagement.Commands;
using BoilerPlate.Application.Features.UserManagement.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoilerPlate.Web.Controllers.V1
{
    [Route("api/v{version:apiVersion}/Authentication")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthenticationController(IMediator mediator)
        {
            this._mediator = mediator;
        }


        [HttpPost("register")]
        [ProducesResponseType(typeof(SignUpResponse),StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<SignUpResponse>> UserSignUp([FromBody] UserSignUpDto Body)
        {
            var command = new UserSignUpCommand (Body);
            var response = await _mediator.Send(command);

            if(response.Data == null)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(UserSignUp), response);
        }
    }
}
