using BoilerPlate.Application.Features.UserManagement.Commands;
using BoilerPlate.Application.Features.UserManagement.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BoilerPlate.Graphql.Features.Mutations
{
    public partial class Mutations
    {
        public async Task<SignUpResponse> UserSignUp(UserSignUpDto Body, [FromServices] IMediator mediator)
        {
            var command = new UserSignUpCommand(Body);
            return await mediator.Send(command);

        }

    }
}
