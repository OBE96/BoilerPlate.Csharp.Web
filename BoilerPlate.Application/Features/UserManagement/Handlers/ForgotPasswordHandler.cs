
using BoilerPlate.Application.Features.UserManagement.Dtos;
using BoilerPlate.Domain.Entities;
using BoilerPlate.Infrastructure.Repository.Interface;
using BoilerPlate.Infrastructure.Services.Interfaces;
using BoilerPlate.Infrastructure.Utilities;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.Xml;
using static System.Net.WebRequestMethods;

namespace BoilerPlate.Application.Features.UserManagement.Handlers
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordDto, Result<ForgotPasswordResponse>>
    {
        private readonly IRepository<User>? _userRepo;
        private readonly ITokenService? _tokenService;
        private readonly IOptions<FrontendUrl> _options;
        private readonly IMessageQueueService _queueService;
        private readonly ILogger<ForgotPasswordHandler>? _logger;
        public ForgotPasswordHandler(
            IRepository<User>? userRepo,
            ITokenService? tokenService,
            ILogger<ForgotPasswordHandler>? logger,
            IMessageQueueService queueService,
            IOptions<FrontendUrl> options
            )
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _logger = logger;
            _queueService = queueService;
            _options = options;
        }
        public async Task<Result<ForgotPasswordResponse>> Handle(ForgotPasswordDto request, CancellationToken cancellationToken)
        {
            string code= null!;
                var user = await _userRepo!.GetBySpec(u => u.Email == request.Email);
            if (user == null)
                return Result.Failure<ForgotPasswordResponse>("User with email does not exist");
            try
            { 
                if(!request.isMobile)
                {
                    code = Guid.NewGuid().ToString().Replace("-", "");

                    user.PasswordResetToken = code;
                    var accessToken = _tokenService!.GenerateJwt(user, 10);
                    var pageLink = $"{_options.Value.Path}/api/v1/Authentication/reset-password?access_token={Uri.EscapeDataString(accessToken)}";
                    //{_options.Value.Path}
                    //send email
                    await _queueService.SendForgotPasswordEmailAsync(
                        user.FirstName ?? user.LastName!,
                        user.Email!,
                        "Brand BiolerPlate",
                        pageLink,
                        DateTime.UtcNow.Year.ToString());
                }
                else
                {
                    code = GenerateSixDigitCode();
                    user.PasswordResetToken = code;

                    //send email
                    await _queueService.SendForgotPasswordEmailMobileAsync(
                        user.FirstName ?? user.LastName!,
                        user.Email!,
                        "Brand BoilerPlate",
                        code,
                        DateTime.UtcNow.Year.ToString());

                }
               

                user.PasswordResetTokenTime = DateTime.UtcNow;

                await _userRepo.UpdateAsync( user );
                await _userRepo.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger!.LogError("Forget Password Error {error}",ex);
            }
            return Result.Success(new ForgotPasswordResponse()
            {
                Message ="successful",
                StatusCode = StatusCodes.Status200OK,
                Data = new ForgotPasswordData()
                {
                    Message = "A mail has been sent to your email address"
                }
            });
        }

        public static string GenerateSixDigitCode()
        {
            Random random = new Random();
            int number = random.Next(100000, 999999);
            return number.ToString("D6");
        }
    }

}
