using FlashLogistic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlashLogistic.Infrastructure.EntityFrameworkCore;

public static class DbContextModelCreatingExtension
{
    public static void ConfigureModels(this ModelBuilder builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        builder.Entity<Repartidor>(e =>
        {
            e.ToTable("Repartidores");

            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName(nameof(Repartidor.Id));

            e.Property(x => x.Nombre)
                .IsRequired()
                .HasMaxLength(Repartidor.NombreMaxLength)
                .HasColumnName(nameof(Repartidor.Nombre));

            e.HasIndex(x => x.Nombre).IsUnique();
        });

        builder.Entity<Paquete>(e =>
        {
            e.ToTable("Paquetes");

            e.HasKey(x => x.Id);
            e.Property(x => x.Id)
                .HasColumnName(nameof(Paquete.Id));

            e.Property(x => x.Descripcion)
                .IsRequired()
                .HasMaxLength(Paquete.DescripcionMaxLength)
                .HasColumnName(nameof(Paquete.Descripcion));

            e.Property(x => x.Codigo)
                .IsRequired()
                .HasMaxLength(Paquete.CodigoMaxLength)
                .HasColumnName(nameof(Paquete.Codigo));

            e.HasIndex(x => x.Codigo).IsUnique();

            e.Property(x => x.Peso)
                .IsRequired()
                .HasColumnName(nameof(Paquete.Peso));

            e.Property(x => x.Estado)
                .IsRequired()
                .HasColumnName(nameof(Paquete.Estado));

            e.Property(x => x.Prioridad)
                .IsRequired()
                .HasColumnName(nameof(Paquete.Prioridad));

            e.Property(x => x.RepartidorId)
                .IsRequired(false)
                .HasColumnName(nameof(Paquete.RepartidorId));

            e.HasOne(x => x.Repartidor)
                .WithMany(x => x.Paquetes)
                .HasForeignKey(x => x.RepartidorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
