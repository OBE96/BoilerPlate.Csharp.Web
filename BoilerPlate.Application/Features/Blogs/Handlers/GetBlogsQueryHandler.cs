using AutoMapper;
using BoilerPlate.Application.Features.Blogs.Dtos;
using BoilerPlate.Application.Features.Blogs.Queries;
using BoilerPlate.Domain.Entities;
using BoilerPlate.Infrastructure.Repository.Interface;
using MediatR;

namespace BoilerPlate.Application.Features.Blogs.Handlers
{
    public class GetBlogsQueryHandler : IRequestHandler<GetBlogsQuery, IEnumerable<BlogDto>>
    {
        private readonly IRepository<Blog> _blogRepository;
        private readonly IMapper _mapper;
        public GetBlogsQueryHandler(IRepository<Blog> blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BlogDto>> Handle(GetBlogsQuery request, CancellationToken cancellationToken)
        {
            var blogs = _blogRepository.GetAllAsync();
            if (blogs == null)
            {
                return null;
            }

            return _mapper.Map<IEnumerable<BlogDto>>(blogs);
        }
    }
}
