namespace mooks.authenticationservice.Installers
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using mooks.authenticationservice.Data.Options;
    using mooks.authenticationservice.Services;
    public class MVCInnstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.GetSection(nameof(JwtSettings)).Bind(jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IStorageService, StorageService>();
        }
    }
}
