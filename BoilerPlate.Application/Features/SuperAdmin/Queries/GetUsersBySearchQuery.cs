using BoilerPlate.Application.Features.SuperAdmin.Dto;
using BoilerPlate.Application.Shared.Dtos;
using MediatR;

namespace BoilerPlate.Application.Features.SuperAdmin.Queries
{
    public class GetUsersBySearchQuery: IRequest<PagedListDto<UserSuperDto>>
    {
        public UsersQueryParameters userQueryParameters { get; set; }
        public GetUsersBySearchQuery(UsersQueryParameters queryParameters)
        {
            userQueryParameters = queryParameters;
        }
    }
}
