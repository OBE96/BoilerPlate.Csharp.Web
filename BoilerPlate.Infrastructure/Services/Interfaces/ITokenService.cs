using BoilerPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Infrastructure.Services.Interfaces
{
    public interface ITokenService
    {
        public string GenerateJwt(User userData, int expireInMinutes = 0);

        public string GetCurrentUserEmail();

        public string GetForgotPasswordToken();
    }
}