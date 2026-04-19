import { EstadoPaquete } from "../enums/estado-paquete.enum";
import { Prioridad } from "../enums/prioridad.enum";
import { RepartidorDto } from "./repartidor.dto";

export interface PackageDto {
    id: string;
    descripcion: string;
    peso: number;
    codigo: string;
    estado: EstadoPaquete;
    prioridad: Prioridad;
    repartidor: RepartidorDto | null;
}

export interface CreatePackageDto {
    descripcion: string;
    peso: number;
    codigo: string;
    prioridad: Prioridad;
}