using AutoMapper;
using BoilerPlate.Application.Features.Subcriptions.Dtos.Responses;
using BoilerPlate.Application.Features.UserManagement.Commands;
using BoilerPlate.Application.Features.UserManagement.Dtos;
using BoilerPlate.Domain.Entities;
using BoilerPlate.Domain.Enums;
using BoilerPlate.Infrastructure.Repository.Interface;
using BoilerPlate.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace BoilerPlate.Application.Features.UserManagement.Handlers
{
    public class UserSignUpCommandHandler : IRequestHandler<UserSignUpCommand, SignUpResponse>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserSignUpCommandHandler> _logger;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        public UserSignUpCommandHandler(IRepository<User> userRepository, IRepository<Role> roleRepository, 
            IMapper mapper, ILogger<UserSignUpCommandHandler> logger, IPasswordService passwordService,
            ITokenService tokenService, IEmailService emailService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logger = logger;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _emailService = emailService;
        }
        public async Task<SignUpResponse> Handle(UserSignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //checks if the email is a unique email 
                //if the email is not null i.e the mail is present in database . it returns an error message  
                var isUnique = await _userRepository.GetBySpec(u => u.Email == request.SignUpBody.Email);
                if (isUnique is not null)
                {
                    return new SignUpResponse
                    {
                        Message = "Email already exists",

                    };
                }
                //If the email is not taken, the method proceeds to create a new user.
                //It uses a mapper to convert the SignUpBody (which likely contains the user-provided data) into a User object.
                //A new unique Guid is assigned to the user's Id.
                //The password is hashed and salted using _passwordService.GeneratePasswordSaltAndHash() before being stored.

                var createdUser = _mapper.Map<User>(request.SignUpBody);
                createdUser.Id = Guid.NewGuid();
                (createdUser.PasswordSalt, createdUser.Password) = _passwordService.GeneratePasswordSaltAndHash(request.SignUpBody.Password!);

                var userOrg = new Organization
                {
                    Name = $"{createdUser.FirstName}'s Org",
                    OwnerId = createdUser.Id,
                    Email = createdUser.Email,
                    CreatedAt = DateTime.UtcNow,
                    Id = Guid.NewGuid()
                };
                createdUser.Organizations.Add(userOrg);

                var sub = new Subscription
                {
                    Frequency = SubscriptionFrequency.Annually,
                    IsActive = true,
                    Plan = SubscriptionPlan.Free,
                    StartDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddYears(1),
                    UserId = createdUser.Id,
                    OrganizationId = userOrg.Id,
                    Amount = 0
                };
                createdUser.Subscriptions.Add(sub);

                //The method creates a role for the user within the organization (in this case, an "Admin" role).
                //A UserRole is also created, linking the user to the role in the organization.

                var role = new Role
                {
                    Id = Guid.NewGuid(),
                    OrganizationId = userOrg.Id,
                    Name = "Admin",
                    IsActive = true,
                };
                role.UsersRoles.Add(new UserRole
                {
                    Id = Guid.NewGuid(),
                    UserId = createdUser.Id,
                    OrganizationId = userOrg.Id
                });

                await _userRepository.AddAsync(createdUser);
                await _roleRepository.AddAsync(role);
                await _userRepository.SaveChanges();

                var emailMessage = Message.CreateEmail(
                   createdUser.Email!,
                   "Welcome to Our Service",
                   $"Hi {createdUser.FirstName},\n\nThank you for signing up! We're excited to have you on board.",
                   createdUser.FirstName!
               );

                await _emailService.SendEmailMessage(emailMessage);

                var token = _tokenService.GenerateJwt(createdUser);
                SignupResponseData signUpResponseData = GetUserDetails(createdUser);

                return new SignUpResponse
                {
                    Message = "User registered successfully",
                    Data = signUpResponseData,
                    Token = token,
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user");
                return new SignUpResponse
                {
                    Message = "An error occurred while processing your request",
                };
            }
        }

        private SignupResponseData GetUserDetails(User createdUser)
        {
            var user = _mapper.Map<UserResponseDto>(createdUser);
            var orgs = createdUser.Organizations.Select(o => new OrganisationDto
            {
                Id = o.Id,
                Name = o.Name,
                Role = o.UsersRoles.Where(x => x.User == createdUser && x.Orgainzation == o).FirstOrDefault()?.Role!.Name,
                IsOwner = o.OwnerId == createdUser.Id,
            }).ToList();
            var subs = createdUser.Subscriptions.Select(r => new SubscribeFreePlanResponse
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
            var signUpResponseData = new SignupResponseData { User = user, Organization = orgs, Subscription = subs };
            return signUpResponseData;
        }

    }
}
