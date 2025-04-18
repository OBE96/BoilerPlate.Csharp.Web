﻿using BoilerPlate.Application.Features.UserManagement.Dtos;
using BoilerPlate.Domain.Entities;
using BoilerPlate.Infrastructure.Repository.Interface;
using BoilerPlate.Infrastructure.Services.Interfaces;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BoilerPlate.Application.Features.UserManagement.Handlers
{
    public record PasswordResetHandler : IRequestHandler<PasswordResetDto, Result<PasswordResetResponse>>
    {
        private readonly IRepository<User>? _userRepo;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        public PasswordResetHandler(IRepository<User>? userRepo, IPasswordService passwordService, ITokenService tokenService)
        {
            _userRepo = userRepo;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        public async  Task<Result<PasswordResetResponse>> Handle(PasswordResetDto request, CancellationToken cancellationToken)
        {
            var email = _tokenService.GetCurrentUserEmail();

            if (string.IsNullOrWhiteSpace(email))
                return Result.Failure<PasswordResetResponse>("User does not exist");

            var forgetPasswordToken = _tokenService.GetForgotPasswordToken();

            if (string.IsNullOrWhiteSpace(forgetPasswordToken))
                return Result.Failure<PasswordResetResponse>("User does not exist");

            var user = await _userRepo.GetBySpec(u => u.Email == email && u.PasswordResetToken == forgetPasswordToken);

            if (user == null)
                return Result.Failure<PasswordResetResponse>("This password reset link has been used!");

            if ((DateTime.UtcNow - user.PasswordResetTokenTime.GetValueOrDefault()).Minutes > 10)
                return Result.Failure<PasswordResetResponse>("Reset Link has expired!");

            (user.PasswordSalt, user.Password) = _passwordService.GeneratePasswordSaltAndHash(request.NewPassword!);
            user.PasswordResetToken = null;

            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveChanges();

            return Result.Success(new PasswordResetResponse()
            {
                Message = "successful",
                StatusCode = StatusCodes.Status200OK,
                Data = new PasswordResetData()
                {
                    Message ="success"
                }

            });

        }
    }
}
