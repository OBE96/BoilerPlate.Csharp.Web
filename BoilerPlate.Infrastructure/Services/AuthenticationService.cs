using BoilerPlate.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BoilerPlate.Infrastructure.Services
{
    public class AuthenticationService(IHttpContextAccessor httpContextAccessor) : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public Task<Guid> GetCurrentUserAsync()
        {
            //This method retrieves the current authenticated user's ID as a Guid.
            //_httpContextAccessor.HttpContext.User.Identity: Accesses the current user's identity from the HTTP context.
            //ClaimsIdentity identity: Attempts to cast the user's identity to ClaimsIdentity. If the cast fails, it
            //throws an exception because the user identity is not available or is not in the expected format.
            if (_httpContextAccessor.HttpContext.User.Identity is not ClaimsIdentity identity)
                throw new InvalidOperationException("User identity is not available.");

            //identity.Claims: Retrieves the collection of claims associated with the user. Claims are key-value pairs
            //that represent information about the user, such as their ID, name, roles, etc.
            var claim = identity.Claims;


            //ClaimTypes.Sid: This is a standard claim type representing the user's security identifier (SID),
            //which typically corresponds to the user's ID.
            //FirstOrDefault: Searches for the first claim with the type ClaimTypes.Sid. If no such claim exists,
            //loggedInUserId will be null.
            var loggedInUserId = claim.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value;


            //Checks if the loggedInUserId is null or empty. If so, it throws an exception because the user ID
            //is not available in the claims.
            if (string.IsNullOrEmpty(loggedInUserId))
                throw new InvalidOperationException("User ID is not available in the claims.");

            //Attempts to parse the loggedInUserId string into a Guid. If parsing fails,
            //it throws an exception because the user ID is not in the correct format.
            if (!Guid.TryParse(loggedInUserId, out var userId))
                throw new InvalidOperationException("Invalid user ID format.");

            //Returns the parsed userId as a Guid wrapped in a Task. This makes the method asynchronous,
            //even though it doesn't perform any asynchronous operations.
            return Task.FromResult(userId);

        }
    }
}
