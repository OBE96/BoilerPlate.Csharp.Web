using BoilerPlate.Application.Features.UserManagement.Dtos;
using BoilerPlate.Domain.Entities;

namespace BoilerPlate.Application.Features.UserManagement.Mappers
{
    public class UserMappingProfile : AutoMapper.Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>()
                //If the User object has FirstName = "John" and LastName = "Doe", the FullName in UserDto will be "John Doe".
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Organizations, opt => opt.MapFrom(src => src.Organizations))
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.Profile))
                .ReverseMap();

            CreateMap<UserSignUpDto, User>()
               .ReverseMap();

            CreateMap<User, UserResponseDto>()
               .ReverseMap();

            CreateMap<User, UserOrganzationDto>();
        }
    }
}