using BoilerPlate.Infrastructure.Services.Interfaces;
using Google.Apis.Auth;

namespace BoilerPlate.Infrastructure.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        public Task<GoogleJsonWebSignature.Payload> ValidateAsync(string idToken)
        {
            return GoogleJsonWebSignature.ValidateAsync(idToken);
        }
    }
}
