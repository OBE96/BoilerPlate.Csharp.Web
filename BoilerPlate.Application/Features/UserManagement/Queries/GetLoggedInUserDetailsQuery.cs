using BoilerPlate.Application.Features.UserManagement.Dtos;
using MediatR;


namespace BoilerPlate.Application.Features.UserManagement.Queries
{
    public class GetLoggedInUserDetailsQuery:IRequest<UserDto>
    {
    }
}
