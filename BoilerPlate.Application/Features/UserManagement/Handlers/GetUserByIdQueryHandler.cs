using AutoMapper;
using BoilerPlate.Application.Features.UserManagement.Dtos;
using BoilerPlate.Application.Features.UserManagement.Queries;
using BoilerPlate.Domain.Entities;
using BoilerPlate.Infrastructure.Repository.Interface;
using MediatR;

namespace BoilerPlate.Application.Features.UserManagement.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IRepository<User> _userRepo;
        private readonly IMapper _mapper;
        public GetUserByIdQueryHandler(IRepository<User> userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }
        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetBySpec(
                u => u.Id == request.UserId,
                u => u.Products,
                u => u.Organizations,
                u => u.Profile!
            );
            if( user == null )
            {
                return null;
            }
            return _mapper.Map<UserDto>( user );
        }
    }
}
