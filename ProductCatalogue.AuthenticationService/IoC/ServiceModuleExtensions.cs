using ProductCatalogue.AuthenticationService.Interfaces;
using ProductCatalogue.AuthenticationService.Repositories;
using ProductCatalogue.AuthenticationService.Services;

namespace ProductCatalogue.AuthenticationService.IoC
{
    public static class ServiceModuleExtentions
    {
        public static void RegisterCoreServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAuthService, AuthService>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
