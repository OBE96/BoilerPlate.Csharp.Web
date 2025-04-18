using BoilerPlate.Application.Features.Blogs.Dtos;
using BoilerPlate.Domain.Entities;
using Profile = AutoMapper.Profile;


namespace BoilerPlate.Application.Features.Blogs.Mappers
{
    public class BlogMapperProfile: Profile
    {
        public BlogMapperProfile()
        {
            CreateMap<CreateBlogDto ,Blog>().ReverseMap();
            CreateMap<Blog, BlogDto>().ReverseMap();
        }
    }
}
