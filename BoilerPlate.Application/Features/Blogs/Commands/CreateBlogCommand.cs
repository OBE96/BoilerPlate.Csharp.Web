using BoilerPlate.Application.Features.Blogs.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Application.Features.Blogs.Commands
{
    public class CreateBlogCommand :IRequest<CreateBlogResponseDto>
    {
        public CreateBlogDto blogDto;
        public CreateBlogCommand(CreateBlogDto blogDto)
        {
            this.blogDto = blogDto;
        }
    }
}
