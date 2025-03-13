using BoilerPlate.Infrastructure.Context;
using BoilerPlate.Infrastructure.Repository;
using BoilerPlate.Infrastructure.Repository.Interface;
using BoilerPlate.Infrastructure.Services.Interfaces;
using BoilerPlate.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace BoilerPlate.Infrastructure
{
    public static class ConfigureInfrastructure
    {
        public static IServiceCollection AddInfrastructureConfig(this IServiceCollection services, string connectionString)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}
