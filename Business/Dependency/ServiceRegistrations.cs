using Business.Services;
using Core.Utilities.JWT;
using Data.Abstract;
using Data.Concrete.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Dependency
{
    public static class ServiceRegistrations
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            //Helpers
            services.AddScoped<ITokenHelper, TokenHelper>();

            //Services
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPaneUserService, PanelUserService>();

            //Repositories
            services.AddScoped<IPanelUserRepository, PanelUserRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
        }
    }
}
