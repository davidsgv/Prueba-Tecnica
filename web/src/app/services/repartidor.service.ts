import { inject, Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { catchError, throwError, tap } from 'rxjs';

export interface Repartidor {
    id: string;
    nombre: string;
}

@Injectable({ providedIn: 'root' })
export class RepartidorService {
    private http = inject(HttpClient);
    private apiUrl = 'https://localhost:7455/api/repartidores';

    repartidores = signal<Repartidor[]>([]);
    error = signal<string | null>(null);

    obtenerRepartidores(page: number = 1, size: number = 10) {
        const params = new HttpParams()
            .set('PageNumber', page.toString())
            .set('PageSize', size.toString());

        this.http.get<Repartidor[]>(this.apiUrl, { params })
            .pipe(
                catchError(err => {
                    this.error.set("Error al cargar repartidores.");
                    return throwError(() => err);
                })
            )
            .subscribe(data => this.repartidores.set(data));
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
}