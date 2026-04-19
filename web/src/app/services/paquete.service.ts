import { inject, Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, catchError, throwError, tap } from 'rxjs';
import { Package } from '../core/models/package.models';
import { PagedResponse } from '../core/DTOs/pagedResponse.dto';
import { CreatePackageDto, PackageDto } from '../core/DTOs/paquete.dto';
import { EstadoPaquete } from '../core/enums/estado-paquete.enum';

@Injectable({ providedIn: 'root' })
export class PaqueteService {
    private http = inject(HttpClient);
    private apiUrl = 'https://localhost:7455/api/paquetes';

    paquetes = signal<Package[]>([]);
    error = signal<string | null>(null);

    // GET: Obtener todos
    obtenerPaquetes(page: number = 1, size: number = 10, estado?: EstadoPaquete) {
        let params = new HttpParams()
            .set('PageNumber', page.toString())
            .set('PageSize', size.toString());

        if (estado !== undefined) params = params.set('EstadoPaquete', estado);

        this.http.get<PagedResponse<PackageDto>>(this.apiUrl, { params })
            .pipe(
                map(response => response.items.map(dto => this.mapDtoToModel(dto))),
                catchError(err => {
                    this.error.set("Error al cargar los paquetes del servidor.");
                    return throwError(() => err);
                })
            )
            .subscribe(data => {
                this.paquetes.set(data);
                this.error.set(null);
            });
    }

    // GET BY ID
    obtenerPorId(id: string) {
        return this.http.get<PackageDto>(`${this.apiUrl}/${id}`).pipe(
            map(item => this.mapDtoToModel(item))
        );
    }

    // POST: Crear
    crearPaquete(paquete: CreatePackageDto) {
        return this.http.post(this.apiUrl, paquete).pipe(
            tap(() => this.obtenerPaquetes()),
            catchError(err => this.handleError(err))
        );
    }

    // PUT: Actualizar
    actualizarPaquete(id: string, paquete: any) {
        return this.http.put(`${this.apiUrl}/${id}`, paquete).pipe(
            tap(() => this.obtenerPaquetes()),
            catchError(err => this.handleError(err))
        );
    }

    // PUT: Asignar Repartidor (Acción de negocio clave)
    asignarRepartidor(paqueteId: string, repartidorId: string) {
        return this.http.put(`${this.apiUrl}/${paqueteId}/repartidor`, { repartidorId }).pipe(
            tap(() => this.obtenerPaquetes()),
            catchError(err => this.handleError(err))
        );
    }

    // DELETE
    eliminarPaquete(id: string) {
        return this.http.delete(`${this.apiUrl}/${id}`).pipe(
            tap(() => this.obtenerPaquetes()),
            catchError(err => this.handleError(err))
        );
    }

    // Mapeo de datos (Private Helpers)
    private mapDtoToModel(dto: PackageDto): Package {
        return {
            id: dto.id,
            idCorto: dto.id.substring(0, 8).toUpperCase(),
            descripcion: dto.descripcion,
            peso: dto.peso,
            codigo: dto.codigo,
            estado: dto.estado,
            prioridad: dto.prioridad,
            // Si no hay repartidor usamos un texto por defecto
            nombreRepartidor: dto.repartidor ? dto.repartidor.nombre : 'Sin asignar'
        };
    }

    private handleError(err: any) {
        const msg = err.error?.message || "Error en la operación de paquetes.";
        this.error.set(msg);
        return throwError(() => err);
    }
}