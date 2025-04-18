using Asp.Versioning;
using BoilerPlate.Application.Features.Blogs.Commands;
using BoilerPlate.Application.Features.Blogs.Dtos;
using BoilerPlate.Application.Features.Blogs.Queries;
using BoilerPlate.Application.Shared.Dtos;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoilerPlate.Web.Controllers.V1
{
    [Route("api/v{version:apiVersion}/Blog")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BlogController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BlogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(BlogDto), StatusCodes.Status201Created)]

        public async Task<ActionResult<BlogDto>> CreateBlog([FromBody] CreateBlogDto createBlog)
        {
            var command = new CreateBlogCommand(createBlog);
            var response = _mediator.Send(command);

            return CreatedAtAction(nameof(CreateBlog), response);
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(SuccessResponseDto<IEnumerable<BlogDto>>),StatusCodes.Status200OK)]
        public async Task<ActionResult<SuccessResponseDto<IEnumerable<BlogDto>>>>GetBlogs()
        {
            var blogs = await _mediator.Send(new GetBlogsQuery());
            var response = new SuccessResponseDto<IEnumerable<BlogDto>>
            { 
                
                Data= blogs
            };
            return Ok(response);

        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]

        public async Task<ActionResult> DeleteBlogById(Guid id)
        {
            var command = new DeleteBlogByIdCommand(id);
            var response = await _mediator.Send(command);
            return NoContent(); 

        }
    }
}
