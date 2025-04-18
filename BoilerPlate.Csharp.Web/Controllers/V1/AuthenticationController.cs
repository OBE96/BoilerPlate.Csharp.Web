using Asp.Versioning;
using BoilerPlate.Application.Features.UserManagement.Commands;
using BoilerPlate.Application.Features.UserManagement.Dtos;
using BoilerPlate.Application.Features.UserManagement.Queries;
using BoilerPlate.Application.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace BoilerPlate.Web.Controllers.V1
{
    [Route("api/v{version:apiVersion}/Authentication")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthenticationController(IMediator mediator)
        {
            this._mediator = mediator;
        }

    
        [HttpGet("@me")]
        [ProducesResponseType(typeof(SuccessResponseDto<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<SuccessResponseDto<UserDto>>> GetLoggedInUserDetails()
        {
            var query = new GetLoggedInUserDetailsQuery();
            var response = await _mediator.Send(query);
            return Ok(new SuccessResponseDto<UserDto>
            {
                Data = response,
                StatusCode =StatusCodes.Status200OK,
            });
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(UserLoginResponseDto<SignupResponseData>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserLoginResponseDto<object>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserLoginResponseDto<SignupResponseData>>> Login([FromBody] UserLoginRequestDto loginRequest)
        {
            var command = new CreateUserLoginCommand(loginRequest);
            var response = await _mediator.Send(command);
            if (response == null || response.Data == null)
            {
                return Unauthorized(new 
                {
                    message = "Invalid credentials",
                    error ="Invalid email or password",
                    status_code = StatusCodes.Status401Unauthorized
                    
                });

            }
            return Ok(response);
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(SignUpResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<SignUpResponse>> UserSignUp([FromBody] UserSignUpDto Body)
        {
            var command = new UserSignUpCommand(Body);
            var response = await _mediator.Send(command);

            if (response.Data == null)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(UserSignUp), response);
        }

        [HttpPost("google")]
        [ProducesResponseType(typeof(UserLoginResponseDto<SignupResponseData>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserLoginResponseDto<SignupResponseData>>> GoogleLogin([FromBody] GoogleLoginRequestDto googleLoginRequest)
        {
            var command = new GoogleLoginCommand(googleLoginRequest.IdToken);
            var response = await _mediator.Send(command);

            if (response == null || response.Data == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid credentials",
                    error = "Google login failed.",
                    status_code = StatusCodes.Status401Unauthorized
                });
            }

            return Ok(response);
        }

        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(ForgotPasswordResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ForgotPasswordResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgotPasswordRequestDto request)
        {
            var response = await _mediator.Send(new ForgotPasswordDto(request.Email!, false));
            if(response.IsFailure)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new ForgotPasswordResponse() 
                    {
                        Message = response.Error,
                        StatusCode=StatusCodes.Status404NotFound
                    });

            }
            return Ok(response.Value);
        }

        [HttpPost("forgot-password-mobile")]
        [ProducesResponseType(typeof(ForgotPasswordResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ForgotPasswordResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ForgetPasswordMobile([FromBody] ForgotPasswordRequestDto request)
        {
            var response = await _mediator.Send(new ForgotPasswordDto(request.Email!, true));
            if (response.IsFailure)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new ForgotPasswordResponse()
                    {
                        Message = response.Error,
                        StatusCode = StatusCodes.Status404NotFound
                    });

            }
            return Ok(response.Value);
        }

        [HttpPost("verify-code")]
        [ProducesResponseType(typeof(VerifyForgotPasswordCodeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(VerifyForgotPasswordCodeResponse), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> VerifyForgotPasswordCode([FromBody] VerifyForgotPasswordCodeDto request )
        {
            var response = await _mediator.Send(request);
            if (response.IsFailure)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new ForgotPasswordResponse()
                    {
                        Message = response.Error,
                        StatusCode = StatusCodes.Status404NotFound
                    });

            }
            return Ok(response.Value);
        }

        [HttpPut("reset-password")]
        [ProducesResponseType(typeof(PasswordResetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PasswordResetResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetDto request)
        {
            var response = await _mediator.Send(request);

            if (response.IsFailure)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new ForgotPasswordResponse()
                    {
                        Message = response.Error,
                        StatusCode = StatusCodes.Status404NotFound
                    });

            }
            return Ok(response.Value);
        }

        [HttpPut("reset-password-mobile")]
        [ProducesResponseType(typeof(PasswordResetMobileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PasswordResetMobileResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PasswordResetMobile([FromBody] PasswordResetMobileDto request)
        {
            var response = await _mediator.Send(new PasswordResetMobileCommand(request));

            if (response.IsFailure)
                return StatusCode(StatusCodes.Status404NotFound,
                new PasswordResetMobileResponse()
                {
                    Message = response.Error,
                    StatusCode = StatusCodes.Status404NotFound
                });

            return Ok(response.Value);
        }
    }
}