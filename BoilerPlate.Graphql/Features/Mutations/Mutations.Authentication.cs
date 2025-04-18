using BoilerPlate.Application.Features.UserManagement.Commands;
using BoilerPlate.Application.Features.UserManagement.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BoilerPlate.Graphql.Features.Mutations
{
    public partial class Mutations
    {

        public async Task<ActionResult<UserLoginResponseDto<SignupResponseData>>> Login(UserLoginRequestDto loginRequest, [FromServices] IMediator mediator)
        { 
            var command  = new CreateUserLoginCommand(loginRequest);
            return await mediator.Send(command);
        }
        public async Task<SignUpResponse> UserSignUp(UserSignUpDto Body, [FromServices] IMediator mediator)
        {
            var command = new UserSignUpCommand(Body);
            return await mediator.Send(command);

        }

        public async Task<ForgotPasswordResponse> ForgetPassword([FromBody] ForgotPasswordRequestDto request, [FromServices] IMediator mediator)
        {
            var response = await mediator.Send(new ForgotPasswordDto(request.Email!, false));
            return response.Value;
        }

    }
}
