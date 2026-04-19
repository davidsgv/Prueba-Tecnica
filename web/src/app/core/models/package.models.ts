import { EstadoPaquete } from "../enums/estado-paquete.enum";
import { Prioridad } from "../enums/prioridad.enum";

export interface Package {
    id: string;
    idCorto: string;
    descripcion: string;
    peso: number;
    codigo: string;
    estado: EstadoPaquete;
    prioridad: Prioridad;
    nombreRepartidor: string;
}