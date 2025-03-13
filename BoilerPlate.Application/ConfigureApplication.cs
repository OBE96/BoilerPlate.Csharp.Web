using BoilerPlate.Infrastructure.Services.Interfaces;
using BoilerPlate.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using BoilerPlate.Infrastructure.EmailTemplates;
using BoilerPlate.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;



namespace BoilerPlate.Application
{
    public static class ConfigureApplication
    {
        public static IServiceCollection AddApplicationConfig(this IServiceCollection services, IConfiguration configurations)
        {
            services.AddMediatR(cf => cf.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IPasswordService, PasswordService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtOptions =>
            {
                jwtOptions.TokenValidationParameters = TokenService.GetTokenValidationParameters(configurations.GetSection("Jwt").Get<Jwt>()!.SecretKey!);
            });

            services.AddAuthorization();

            services.AddHttpContextAccessor();

            //The purpose of this code is to enable your application to handle cross-origin HTTP requests,
            //meaning requests made from a different domain, protocol, or port than the one your API is hosted on.
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()  // Allows any origin (domain, protocol, port).
                               .AllowAnyMethod()  // Allows any HTTP method (GET, POST, PUT, DELETE, etc.).
                               .AllowAnyHeader();  // Allows any headers in the request.
                    });
            });

            services.AddSingleton(configurations.GetSection("SmtpCredentials").Get<SmtpCredentials>());

            services.AddSingleton(configurations.GetSection("EmailTemplateDirectory").Get<TemplateDir>());
            services.AddSingleton(configurations.GetSection("Jwt").Get<Jwt>());

            return services;
        }
    }
}