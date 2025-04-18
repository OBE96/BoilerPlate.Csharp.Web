
namespace BoilerPlate.Infrastructure.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Guid> GetCurrentUserAsync();
    }
}