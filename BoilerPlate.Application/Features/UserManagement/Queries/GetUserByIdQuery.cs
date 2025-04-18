using BoilerPlate.Application.Features.UserManagement.Dtos;
using MediatR;

namespace BoilerPlate.Application.Features.UserManagement.Queries
{
    public class GetUserByIdQuery: IRequest<UserDto>
    {
        public Guid UserId { get; set; }
        public GetUserByIdQuery(Guid id)
        {
            UserId = id;
        }
    }
}
