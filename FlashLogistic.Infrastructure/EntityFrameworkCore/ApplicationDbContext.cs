using FlashLogistic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlashLogistic.Infrastructure.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    public DbSet<Paquete> Paquetes => Set<Paquete>();
    public DbSet<Repartidor> Repartidor => Set<Repartidor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureModels();
    }
}