using FlashLogistic.Application.Interfaces;
using FlashLogistic.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FlashLogistic.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        services.AddScoped<IPaqueteService, PaqueteService>();
        services.AddScoped<IRepartidorService, RepartidorService>();
        return services;
    }
}
