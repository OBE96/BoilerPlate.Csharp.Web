using AutoMapper;
using BoilerPlate.Application.Features.Blogs.Commands;
using BoilerPlate.Application.Features.Blogs.Dtos;
using BoilerPlate.Domain.Entities;
using BoilerPlate.Infrastructure.Repository.Interface;
using BoilerPlate.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BoilerPlate.Application.Features.Blogs.Handlers
{
    public class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommand, CreateBlogResponseDto>
    {
        private readonly IRepository<Blog> _blog;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
        public CreateBlogCommandHandler(IRepository<Blog> blog, IMapper mapper, IAuthenticationService authenticationService)
        {
            _blog = blog;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }
        public async Task<CreateBlogResponseDto> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
        {
            var userId = await _authenticationService.GetCurrentUserAsync();
            var blog = _mapper.Map<Blog>(request.blogDto);
            blog.AuthorId = userId;

            await _blog.AddAsync(blog);
            await _blog.SaveChanges();

            var blogDto = _mapper.Map<BlogDto>(blog);

            return new CreateBlogResponseDto
            {
                StatusCode = StatusCodes.Status201Created,
                Message = "Blog Created Successfully",
                Data = blogDto

            };
        }
    }
}
