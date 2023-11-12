using CapstoneGVC.BusinessCore.Domain;
using CapstoneGVC.BusinessCore.UnitOfWork;
using CapstoneGVC.Contracts.DomainServices;

namespace CapstoneGVC.Modules.Injection
{
    /// <summary>
    /// Clase que se encarga de realizar la inyección de dependencia de las interfaces y sus respectivas implementaciones
    /// </summary>
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEnmascarador, Enmascarador>();
            services.AddScoped<IEncriptador, Encriptador>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
