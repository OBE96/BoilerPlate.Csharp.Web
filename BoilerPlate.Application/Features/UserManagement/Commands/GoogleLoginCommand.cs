using BoilerPlate.Application.Features.UserManagement.Dtos;
using MediatR;


namespace BoilerPlate.Application.Features.UserManagement.Commands
{
    public class GoogleLoginCommand : IRequest<UserLoginResponseDto<SignupResponseData>>
    {
        public string IdToken { get; }

        public GoogleLoginCommand(string idToken)
        {
            IdToken = idToken;
        }
    }
}
