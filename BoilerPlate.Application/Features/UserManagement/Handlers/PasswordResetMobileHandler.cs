using BoilerPlate.Application.Features.UserManagement.Dtos;
using BoilerPlate.Domain.Entities;
using BoilerPlate.Infrastructure.Repository.Interface;
using BoilerPlate.Infrastructure.Services.Interfaces;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Application.Features.UserManagement.Handlers
{
    public class PasswordResetMobileHandler : IRequestHandler<PasswordResetMobileCommand, Result<PasswordResetMobileResponse>>
    {
        private readonly IRepository<User> _userRepo;
        private readonly IPasswordService _passwordService;

        public PasswordResetMobileHandler(IRepository<User> userRepo, IPasswordService passwordService)
        {
            _userRepo = userRepo;
            _passwordService = passwordService;
        }

        public async Task<Result<PasswordResetMobileResponse>> Handle(PasswordResetMobileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetBySpec(u => u.Email == request.Command.Email);

            if (user == null)
                return Result.Failure<PasswordResetMobileResponse>("User Not Found!");

            (user.PasswordSalt, user.Password) = _passwordService.GeneratePasswordSaltAndHash(request.Command.NewPassword);

            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveChanges();

            return Result.Success(new PasswordResetMobileResponse()
            {
                Message = "successful",
                StatusCode = StatusCodes.Status200OK,
                Data = new PasswordResetMobileData()
                {
                    Message = "success"
                }
            });
        }
    }
}
