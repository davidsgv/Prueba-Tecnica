using AutoMapper;
using FlashLogistic.Application.DTOs.Paquetes;
using FlashLogistic.Application.DTOs.Repartidor;
using FlashLogistic.Domain.Entities;

namespace FlashLogistic.Application;

internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Repartidor, RepartidorDTO>();
        CreateMap<Paquete, PaqueteDTO>();
    }
}
