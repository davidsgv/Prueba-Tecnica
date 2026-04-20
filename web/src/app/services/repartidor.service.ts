import { inject, Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { catchError, throwError, tap, map } from 'rxjs';
import { Repartidor } from '../core/models/repartidor.models';
import { PagedResponse } from '../core/DTOs/pagedResponse.dto';
import { RepartidorDto } from '../core/DTOs/repartidor.dto';
import { environment } from '../../environments/environment.development';

@Injectable({ providedIn: 'root' })
export class RepartidorService {
    private http = inject(HttpClient);
    private apiUrl = `${environment.apiUrl}/api/repartidores`;

    repartidores = signal<Repartidor[]>([]);
    error = signal<string | null>(null);

    obtenerRepartidores(page: number = 1, size: number = 50) {
        const params = new HttpParams()
            .set('PageNumber', page.toString())
            .set('PageSize', size.toString());

        this.http.get<PagedResponse<RepartidorDto>>(this.apiUrl, { params })
            .pipe(
                map(response => response.items.map(dto => this.mapDtoToModel(dto))),
                catchError(err => this.handleError(err))
            )
            .subscribe(data => {
                this.repartidores.set(data);
                this.error.set(null);
            });
    }

    crearRepartidor(nombre: string) {
        return this.http.post<Repartidor>(this.apiUrl, { nombre }).pipe(
            tap(() => this.obtenerRepartidores())
        );
    }

    actualizarRepartidor(id: string, nombre: string) {
        return this.http.put(`${this.apiUrl}/${id}`, { nombre }).pipe(
            tap(() => this.obtenerRepartidores())
        );
    }

    eliminarRepartidor(id: string) {
        return this.http.delete(`${this.apiUrl}/${id}`).pipe(
            tap(() => this.obtenerRepartidores())
        );
    }

    private mapDtoToModel(dto: RepartidorDto): Repartidor {
        return {
            id: dto.id,
            nombre: dto.nombre,
        };
    }

    private handleError(err: any) {
        const msg = err.error?.message || "Error en la operación de repartidores.";
        this.error.set(msg);
        return throwError(() => err);
    }
}