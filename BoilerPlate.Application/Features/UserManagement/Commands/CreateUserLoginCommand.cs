using BoilerPlate.Application.Features.UserManagement.Dtos;
using FluentValidation;
using MediatR;

namespace BoilerPlate.Application.Features.UserManagement.Commands
{
    public class CreateUserLoginCommand: AbstractValidator<CreateUserLoginCommand>, IRequest<UserLoginResponseDto<SignupResponseData>>
    {
        public UserLoginRequestDto LoginRequestDto { get; set; }
        public CreateUserLoginCommand(UserLoginRequestDto loginRequestDto)
        {
            LoginRequestDto = loginRequestDto;

            RuleFor(x => x.LoginRequestDto.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.LoginRequestDto.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("Password must include an uppercase letter, a lowercase letter, a number, and a special character.");
        }
    }
}
