using MediatR;

namespace BoilerPlate.Application.Features.Blogs.Commands
{
    public class DeleteBlogByIdCommand : IRequest<bool>
    {
        public Guid blogId { get; set; }
        public DeleteBlogByIdCommand(Guid id)
        {
            blogId = id;
        }
    }
}
