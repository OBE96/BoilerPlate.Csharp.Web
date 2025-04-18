using Asp.Versioning;
using BoilerPlate.Application.Features.SuperAdmin.Dto;
using BoilerPlate.Application.Features.SuperAdmin.Queries;
using BoilerPlate.Application.Shared.Dtos;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoilerPlate.Web.Controllers.V1
{
    [Route("api/v{version:apiVersion}/Admin")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

    

        [HttpGet("users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult> GetUserBySearch([FromQuery] UsersQueryParameters parameters)
        {
            var users = await _mediator.Send(new GetUsersBySearchQuery(parameters));
            return Ok(new PaginatedResponseDto<PagedListDto<UserSuperDto>>
            {
                Data = users,
                Metadata = users.MetaData,
            });
        }
    }
}
