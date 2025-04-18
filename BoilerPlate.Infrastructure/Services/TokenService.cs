using BoilerPlate.Domain.Entities;
using BoilerPlate.Infrastructure.Services.Interfaces;
using BoilerPlate.Infrastructure.Utilities;
using BoilerPlate.Infrastructure.Utilities.EmailQueue;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BoilerPlate.Infrastructure.Services
{
    public class TokenService(IHttpContextAccessor context, Jwt jwtKeys) : ITokenService
    {
        private readonly IHttpContextAccessor _context = context;
        private readonly Jwt _jwtKeys = jwtKeys;

        public static TokenValidationParameters GetTokenValidationParameters(string secretKey) => new()
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = GetSecurityKey(secretKey),
        };

        public string GenerateJwt(User userData, int expireInMinutes = 0)
        {
            SymmetricSecurityKey securityKey = GetSecurityKey(_jwtKeys.SecretKey!);

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = [
                new(ClaimTypes.Sid, userData.Id.ToString()),
                new(ClaimTypes.Email, userData.Email!),
                new(ClaimTypes.Name, userData.FirstName!),
                new(ClaimTypes.NameIdentifier,
                    !string.IsNullOrWhiteSpace(userData.PasswordResetToken) ? userData.PasswordResetToken : "")
            ];

            expireInMinutes = expireInMinutes == 0 ? _jwtKeys.ExpireInMinute : expireInMinutes;
            var tokenObject = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(expireInMinutes)
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenObject);
        }

        public string GetCurrentUserEmail()
        {
            var identity = _context.HttpContext.User.Identity as ClaimsIdentity;

            var claim = identity!.Claims;

            var loggedInUSerEmail = claim
                .First(x => x.Type == ClaimTypes.Email).Value;

            if (string.IsNullOrWhiteSpace(loggedInUSerEmail))
            {
                throw new InvalidOperationException("User email not found in claims");
            }

            return loggedInUSerEmail;
        }

        public string GetForgotPasswordToken()
        {
            var identity = _context.HttpContext.User.Identity as ClaimsIdentity;

            var claim = identity!.Claims;

            var forgotPasswordToken = claim
                .First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            return forgotPasswordToken;
        }

        private static SymmetricSecurityKey GetSecurityKey(string secretKey)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            return securityKey;
        }
    }
}