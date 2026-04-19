using FlashLogistic.Domain.Enums;

namespace FlashLogistic.Application.DTOs.Paquetes;

public class UpdatePaqueteDTO
{
    public string Descripcion { get; set; } = default!;
    public double Peso { get; set; }
    public EstadoPaquete Estado { get; set; }
    public PrioridadPaquete Prioridad { get; set; }
}
