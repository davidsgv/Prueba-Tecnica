using FlashLogistic.Domain.Repositories;
using FlashLogistic.Infrastructure.EntityFrameworkCore;
using FlashLogistic.Infrastructure.repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FlashLogistic.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddSqlServerDbContext<ApplicationDbContext>("sqldata", null, options =>
        {
            options.UseSqlServer(opt =>
                opt.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
        });

        builder.Services.AddScoped<IPaqueteRepository, PaqueteRepository>();
        builder.Services.AddScoped<IRepartidorRepository, RepartidorRepository>();
        return builder;
    }
}
