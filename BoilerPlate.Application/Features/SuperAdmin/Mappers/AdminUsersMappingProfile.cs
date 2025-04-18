using BoilerPlate.Application.Features.SuperAdmin.Dto;
using BoilerPlate.Domain.Entities;

namespace BoilerPlate.Application.Features.SuperAdmin.Mappers
{
    public class AdminUsersMappingProfile : AutoMapper.Profile
    {
        public AdminUsersMappingProfile()
        {
            CreateMap<User, UserSuperDto>().ReverseMap();
        }
    }
}
