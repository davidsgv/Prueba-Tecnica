using FlashLogistic.Domain.Enums;

namespace FlashLogistic.Application.DTOs.Paquetes;

public class PaquetesInputDTO : PagedRequest
{
    public EstadoPaquete? EstadoPaquete { get; set; }
}
