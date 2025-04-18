using Asp.Versioning;
using BoilerPlate.Application.Features.UserManagement.Commands;
using BoilerPlate.Application.Features.UserManagement.Dtos;
using BoilerPlate.Application.Features.UserManagement.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoilerPlate.Web.Controllers.V1
{

    [Route("api/v{version:apiVersion}/User")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly IMediator? _mediator;
        public UserController(IMediator? mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersById(Guid id)
        {
            var query = new GetUserByIdQuery(id);
            var response = await _mediator!.Send(query);
            return Ok(response);
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _mediator!.Send(new GetUsersQuery());
            return Ok(users);
        }

    }
}
