using AutoMapper;
using BoilerPlate.Application.Features.SuperAdmin.Dto;
using BoilerPlate.Application.Features.SuperAdmin.Queries;
using BoilerPlate.Application.Shared.Dtos;
using BoilerPlate.Domain.Entities;
using BoilerPlate.Infrastructure.Repository.Interface;
using MediatR;
using System.Resources;

namespace BoilerPlate.Application.Features.SuperAdmin.Handlers
{
    public class GetUsersBySearchQueryHandler : IRequestHandler<GetUsersBySearchQuery, PagedListDto<UserSuperDto>>
    {
        private readonly IRepository<User>? _userRepo;
        private readonly IMapper? _mapper;
        public GetUsersBySearchQueryHandler(IRepository<User> userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }
        public async Task<PagedListDto<UserSuperDto>> Handle(GetUsersBySearchQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepo!.GetAllAsync();
            users = users.Where(v => request.userQueryParameters.Email == null || v.Email!.ToLower().Equals(request.userQueryParameters.Email.ToLower())).ToList();
            users = users.Where(V => request.userQueryParameters.FirstName == null || V.FirstName!.ToLower().Equals(request.userQueryParameters.FirstName.ToLower())).ToList();
            users = users.Where(V => request.userQueryParameters.LastName == null || V.LastName!.ToLower().Equals(request.userQueryParameters.LastName.ToLower())).ToList();

            var mappedusers = _mapper!.Map<IEnumerable<UserSuperDto>>(users);
            var userSearchResult = PagedListDto<UserSuperDto>.ToPagedList(mappedusers, request.userQueryParameters.Offset, request.userQueryParameters.Limit);
            return userSearchResult;

        }
    }
}
