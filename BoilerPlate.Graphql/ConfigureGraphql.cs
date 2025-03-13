using Microsoft.Extensions.DependencyInjection;

namespace BoilerPlate.Graphql
{
    public static class ConfigureGraphql
    {
        public static IServiceCollection AddGraphql(this IServiceCollection services)
        {
            services.AddGraphQLServer()
                .AddQueryType<Queries>()
                .AddMutationType<Mutations>()
                .AddAuthorization();

            return services;
        }
    }
}
