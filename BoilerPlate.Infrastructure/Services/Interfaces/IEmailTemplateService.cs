using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Infrastructure.Services.Interfaces
{
    public interface IEmailTemplateService
    {
        public Task<string> GetOrganizationInviteTemplate();

        public Task<string> GetForgotPasswordEmailTemplate();

        Task<string> GetForgotPasswordMobileEmailTemplate();
    }
}