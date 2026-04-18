using FlashLogistic.Domain.Enums;

namespace FlashLogistic.Application.DTOs.Paquetes;

public class CreatePaqueteDTO
{
    public string Descripcion { get; set; } = default!;
    public double Peso { get; set; }
    public string Codigo { get; set; }
    public PrioridadPaquete Prioridad { get; set; }
}