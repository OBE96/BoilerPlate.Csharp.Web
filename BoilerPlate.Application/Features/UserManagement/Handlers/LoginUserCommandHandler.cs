﻿using AutoMapper;
using BoilerPlate.Application.Features.Subcriptions.Dtos.Responses;
using BoilerPlate.Application.Features.UserManagement.Commands;
using BoilerPlate.Application.Features.UserManagement.Dtos;
using BoilerPlate.Domain.Entities;
using BoilerPlate.Infrastructure.Repository.Interface;
using BoilerPlate.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BoilerPlate.Application.Features.UserManagement.Handlers
{
    public class LoginUserCommandHandler : IRequestHandler<CreateUserLoginCommand, UserLoginResponseDto<SignupResponseData>>
    {
        private readonly IRepository<User>? _userRepo;
        private readonly IRepository<LastLogin>? _loginLast;
        private readonly IMapper? _mapper;
        private readonly IPasswordService? _passwordService;
        private readonly ITokenService? _tokenService;
        private readonly IHttpContextAccessor? _httpcontextAccessor;

        public LoginUserCommandHandler(
            IRepository<User>? userRepo, 
            IRepository<LastLogin>? loginLast, 
            IMapper? mapper,
            IPasswordService? passwordService,
            ITokenService? tokenService,
            IHttpContextAccessor? httpcontextAccessor)
        {
            _userRepo = userRepo;
            _loginLast = loginLast;
            _mapper = mapper;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _httpcontextAccessor = httpcontextAccessor;
        }
        public async Task<UserLoginResponseDto<SignupResponseData>> Handle(CreateUserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo!
                .GetQueryableBySpec(u => u.Email == request.LoginRequestDto.Email)
                .Include(u => u.Organizations)
                .ThenInclude(u => u.UsersRoles)
                .ThenInclude(u => u.Role)
                .Include(u => u.Subscriptions)
                .FirstOrDefaultAsync();

            if(user == null || !_passwordService!.IsPasswordEqual(request.LoginRequestDto.Password! , user.PasswordSalt!, user.Password!))
            {
                return new UserLoginResponseDto<SignupResponseData>
                {
                    Data =  null,
                    AccessToken = null,
                    Message = "Invalid credentials",
                    StatusCode = StatusCodes.Status401Unauthorized,
                };
            }
            if(user.Status == "Inative")
            {
                return new UserLoginResponseDto<SignupResponseData>
                {
                    Data = null,
                    AccessToken = null,
                    Message = "Your account is inactive. Please contact support",
                    StatusCode = StatusCodes.Status403Forbidden,
                };
            }
            if (user.Status == "Deleted")
            {
                return new UserLoginResponseDto<SignupResponseData>
                {
                    Data = null,
                    AccessToken = null,
                    Message = "Your account does not exist or has been deleted",
                    StatusCode = StatusCodes.Status403Forbidden,
                };
            }

            var token = _tokenService!.GenerateJwt(user);

            var lastLogin = new LastLogin
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                LoginTime = DateTime.UtcNow,
                IPAddress = _httpcontextAccessor?.HttpContext?.Connection.RemoteIpAddress?.ToString(),
            };
            await _loginLast!.AddAsync(lastLogin);
            await _loginLast.SaveChanges();

            return new UserLoginResponseDto<SignupResponseData>
            {
                Data = GetUserDetails(user),
                AccessToken = token,
                Message = "login successful"
            };
        }

        private SignupResponseData GetUserDetails(User user)
        {
            var userResponse = _mapper!.Map<UserResponseDto>(user);
            var orgs = user.Organizations.Select(o => new OrganisationDto
            {
                Id = o.Id,
                Name = o.Name,
                Role = o.UsersRoles.FirstOrDefault()?.Role!.Name,
                IsOwner = o.OwnerId == user.Id,
            }).ToList();
            var subs = user.Subscriptions.Select(r => new SubscribeFreePlanResponse
            {
                SubscriptionId = r.Id,
                Frequency = r.Frequency.ToString(),
                IsActive = r.IsActive,
                Plan = r.Plan.ToString(),
                StartDate = r.StartDate,
                UserId = r.UserId,
                OrganizationId = r.OrganizationId,
                Amount = r.Amount,
            }).ToList();
            var signUpResponseData = new SignupResponseData { User = userResponse, Organization = orgs, Subscription = subs };
            return signUpResponseData;
        }
    }
}
