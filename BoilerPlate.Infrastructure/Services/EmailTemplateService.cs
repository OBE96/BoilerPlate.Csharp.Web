using BoilerPlate.Infrastructure.EmailTemplates;
using BoilerPlate.Infrastructure.Services.Interfaces;
using BoilerPlate.Infrastructure.Utilities.StringKeys;
using Microsoft.Extensions.Logging;
using System.Text;

namespace BoilerPlate.Infrastructure.Services
{
    public class EmailTemplateService(TemplateDir templateDir, ILogger<EmailTemplateService> logger) : IEmailTemplateService
    {
        private readonly TemplateDir templateDir = templateDir;
        private readonly ILogger<EmailTemplateService> logger = logger;
        public async Task<string> GetOrganizationInviteTemplate()
        {
            
            logger.LogInformation("Getting organisation invite email template");
            string path = templateDir.Path!;
            path = Path.Combine(path, $"{EmailConstants.inviteEmailTemplate}");
            string template = await File.ReadAllTextAsync(path, Encoding.UTF8);
            return template;
        }

        public async Task<string> GetForgotPasswordEmailTemplate()
        {

            logger.LogInformation("Getting forgot password template");
            string path = templateDir.Path!;
            path = Path.Combine(path, $"{EmailConstants.ForgotPasswordTemplate}");
            string template = await File.ReadAllTextAsync(path, Encoding.UTF8);
            return template;
        }
        
        public async Task<string> GetForgotPasswordMobileEmailTemplate()
        {
            logger.LogInformation("Getting forgot password template");
            string path = templateDir.Path!;
            path = Path.Combine(path, $"{EmailConstants.ForgotPasswordMobileTemplate}");
            string template = await File.ReadAllTextAsync(path, Encoding.UTF8);
            return template;
        }
    }
}