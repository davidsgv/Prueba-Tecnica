using FlashLogistic.Application.DTOs.Repartidor;

namespace FlashLogistic.Application.DTOs.Paquetes;

public class PaqueteDTO
{
    public Guid Id { get; set; }
    public string Descripcion { get; set; } = default!;
    public double Peso { get; set; }
    public string Estado { get; set; }
    public string Prioridad { get; set; }
    public RepartidorDTO? Repartidor { get; set; }
}
