using FlashLogistic.Domain.Enums;

namespace FlashLogistic.Domain.Entities;

public class Paquete
{
    public const int DescripcionMinLength = 20;
    public const int DescripcionMaxLength = 300;
    public const int CodigoMaxLength = 20;

    public Guid Id { get; private set; }
    public string Descripcion { get; set; }
    public double Peso { get; set; }
    public string Codigo { get; set; }
    public EstadoPaquete Estado { get; set; }
    public PrioridadPaquete Prioridad { get; private set; }
    public Guid? RepartidorId { get; private set; }
    public virtual Repartidor? Repartidor { get; private set; }

    public Paquete(string descripcion, double peso, string codigo, PrioridadPaquete prioridad)
    {
        Id = Guid.NewGuid();
        Descripcion = descripcion;
        Peso = peso;
        Codigo = codigo;
        Estado = EstadoPaquete.Bodega;
        Prioridad = prioridad;

        Validate();
    }

    public Paquete(Guid id, string descripcion, double peso, string codigo, EstadoPaquete estadoPaquete, PrioridadPaquete prioridad, Guid? repartidorId)
    {
        Id = id;
        Descripcion = descripcion;
        Peso = peso;
        Codigo = codigo;
        Estado = estadoPaquete;
        RepartidorId = repartidorId;

        Validate();
    }

    public void AsignarRepartidor(Repartidor repartidor)
    {
        if (Estado == EstadoPaquete.Entregado)
            throw new DomainException("No se puede asignar un paquete ya entregado.");

        Repartidor = repartidor;
        RepartidorId = repartidor.Id;
        Estado = EstadoPaquete.Asignado;
    }

    public void AsignarPrioridad(PrioridadPaquete prioridad)
    {
        if (Estado == EstadoPaquete.Entregado)
            throw new DomainException("No se puede cambiar la prioridad de un paquete entregado");

        Prioridad = prioridad;
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Descripcion) || Descripcion.Length < DescripcionMinLength || Descripcion.Length > DescripcionMaxLength)
            throw new DomainException($"La descripción tiene que tener una longitud entre {DescripcionMinLength} y {DescripcionMaxLength} caracteres");

        if (Peso <= 0)
            throw new DomainException("El peso debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(Codigo))
            throw new DomainException("El código es obligatorio.");

        if (Codigo.Length > CodigoMaxLength)
            throw new DomainException($"El código no puede tener mas de {CodigoMaxLength} caracteres");
    }
}
