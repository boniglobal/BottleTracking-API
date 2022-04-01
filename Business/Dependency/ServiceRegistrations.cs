using Business.Services;
using Business.ValidationRules.FluentValidation;
using Core.Utilities.JWT;
using Data.Abstract;
using Data.Concrete.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using static Core.DTOs.Bottle;
using static Core.DTOs.Station;
using static Core.DTOs.User;

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
            services.AddScoped<IBottleService, BottleService>();
            services.AddScoped<IStationService, StationService>();
            services.AddScoped<IStationLogService, StationLogService>();

            //Repositories
            services.AddScoped<IPanelUserRepository, PanelUserRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IBottleRepository, BottleRepository>();
            services.AddScoped<IStationRepository, StationRepository>();
            services.AddScoped<IStationLogRepository, StationLogRepository>();

            //Validators
            services.AddTransient<IValidator<StationAdd>, StationAddValidator>();
            services.AddTransient<IValidator<StationUpdate>, StationUpdateValidator>();
            services.AddTransient<IValidator<BottleAdd>, BottleAddValidator>();
            services.AddTransient<IValidator<BottleUpdate>, BottleUpdateValidator>();
            services.AddTransient<IValidator<PanelUserAddRequest>, PanelUserAddValidator>();
            services.AddTransient<IValidator<PanelUserUpdateRequest>, PanelUserUpdateValidator>();
            services.AddTransient<IValidator<ResetPassword>, PanelUserResetPasswordValidator>();
        }
    }
}
