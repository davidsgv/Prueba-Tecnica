import { inject, Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, catchError, throwError, tap, switchMap } from 'rxjs';
import { Package } from '../core/models/package.models';
import { PagedResponse } from '../core/DTOs/pagedResponse.dto';
import { CreatePackageDto, PackageDto, UpdatePaqueteDTO } from '../core/DTOs/paquete.dto';
import { EstadoPaquete } from '../core/enums/estado-paquete.enum';

@Injectable({ providedIn: 'root' })
export class PaqueteService {
    private http = inject(HttpClient);
    private apiUrl = 'https://localhost:7455/api/paquetes';

    paquetes = signal<Package[]>([]);
    error = signal<string | null>(null);
    asignarError = signal<string | null>(null);

    private currentPage = signal<number>(1);
    private pageSize = signal<number>(10);
    private currentEstado = signal<EstadoPaquete | undefined>(undefined);

    private _cargarPaquetes() { // Helper que retorna el observable
        let params = new HttpParams()
            .set('PageNumber', this.currentPage().toString())
            .set('PageSize', this.pageSize().toString());

        const estadoFiltrado = this.currentEstado();
        if (estadoFiltrado !== undefined) {
            params = params.set('EstadoPaquete', estadoFiltrado);
        }
        
        return this.http.get<PagedResponse<PackageDto>>(this.apiUrl, { params }).pipe(
            map(response => response.items.map(dto => this.mapDtoToModel(dto))),
            tap(data => this.paquetes.set(data))
        );
    }

    // GET: Obtener todos
    obtenerPaquetes(page?: number, size?: number, estado?: EstadoPaquete) {
        if (page !== undefined) this.currentPage.set(page);
        if (size !== undefined) this.pageSize.set(size);
        this.currentEstado.set(estado);

        this._cargarPaquetes().subscribe(data => {
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
    actualizarPaquete(id: string, paquete: UpdatePaqueteDTO) {
        return this.http.put(`${this.apiUrl}/${id}`, paquete).pipe(
            switchMap(() => this._cargarPaquetes()), 
            tap(() => this.error.set(null)),
            catchError(err => this.handleError(err))
        )
    }

    // PUT: Asignar Repartidor (Acción de negocio clave)
    asignarRepartidor(paqueteId: string, repartidorId: string) {
        return this.http.put(`${this.apiUrl}/${paqueteId}/repartidor`, { repartidorId }).pipe(
            tap(() => this.obtenerPaquetes()),
            catchError(err => {
                const msg = err.error?.detail || "Error en la asignación de paquetes.";
                this.asignarError.set(msg);
                return throwError(() => err);
            })
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