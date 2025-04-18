using BoilerPlate.Application.Features.Blogs.Commands;
using BoilerPlate.Domain.Entities;
using BoilerPlate.Infrastructure.Repository.Interface;
using MediatR;


namespace BoilerPlate.Application.Features.Blogs.Handlers
{
    public class DeleteBlogByIdCommandHandler : IRequestHandler<DeleteBlogByIdCommand, bool>
    {
        private readonly IRepository<Blog> _blogRepository;
        public DeleteBlogByIdCommandHandler(IRepository<Blog> blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<bool> Handle(DeleteBlogByIdCommand request, CancellationToken cancellationToken)
        {
           var blog = await _blogRepository.GetBySpec(b => b.Id == request.blogId);
            if (blog == null)
            {
                return false;
            }
            await _blogRepository.DeleteAsync(blog);
            await _blogRepository.SaveChanges();
            return true;
        }
    }
}
