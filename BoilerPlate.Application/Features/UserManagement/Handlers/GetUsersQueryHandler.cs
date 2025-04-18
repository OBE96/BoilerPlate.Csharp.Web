using AutoMapper;
using BoilerPlate.Application.Features.UserManagement.Dtos;
using BoilerPlate.Application.Features.UserManagement.Queries;
using BoilerPlate.Domain.Entities;
using BoilerPlate.Infrastructure.Repository.Interface;
using MediatR;

namespace BoilerPlate.Application.Features.UserManagement.Handlers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IRepository<User> _userRepo;
        private readonly IMapper _mapper;
        public GetUsersQueryHandler(IRepository<User> userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepo.GetAllAsync(x => x.Products, x => x.Organizations, x => x.Profile!);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
