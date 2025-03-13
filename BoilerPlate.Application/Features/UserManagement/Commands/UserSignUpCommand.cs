using BoilerPlate.Application.Features.UserManagement.Dtos;
using MediatR;

namespace BoilerPlate.Application.Features.UserManagement.Commands
{
    public class UserSignUpCommand : IRequest<SignUpResponse>
    {
        public UserSignUpDto SignUpBody { get; }
        public UserSignUpCommand(UserSignUpDto signUpDto)
        {
            SignUpBody = signUpDto;
        }

    }
}