using Google.Apis.Auth;


namespace BoilerPlate.Infrastructure.Services.Interfaces
{
    public  interface IGoogleAuthService
    {
        Task<GoogleJsonWebSignature.Payload> ValidateAsync(string idToken);
    }
}
