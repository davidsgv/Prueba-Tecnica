namespace FlashLogistic.Domain.Entities;

public class Repartidor
{
    public const int NombreMinLength = 3;
    public const int NombreMaxLength = 30;

    public Guid Id { get; private set; }
    public string Nombre { get; private set; }

    private readonly List<Paquete> _paquetes = new();
    public virtual IReadOnlyCollection<Paquete> Paquetes => _paquetes.AsReadOnly();

    public Repartidor(string nombre)
    {
        Id = Guid.NewGuid();
        Nombre = nombre;

        Validate();
    }

    public Repartidor(Guid id, string nombre)
    {
        Id = id;
        Nombre = nombre;

        Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Nombre) || Nombre.Length < NombreMinLength || Nombre.Length > NombreMaxLength)
            throw new DomainException($"La descripción tiene que tener una longitud entre {NombreMinLength} y {NombreMaxLength} caracteres");
    }
}
