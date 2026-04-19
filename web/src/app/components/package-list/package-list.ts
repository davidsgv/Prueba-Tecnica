import { Component, inject, input, OnInit, output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaqueteService } from '../../services/paquete.service';
import { RepartidorService } from '../../services/repartidor.service';
import { EstadoPaquete } from '../../core/enums/estado-paquete.enum';
import { Prioridad } from '../../core/enums/prioridad.enum';
import { LucidePackageSearch, LucideListTodo, LucideTruck } from '@lucide/angular';

@Component({
    selector: 'app-package-list',
    imports: [CommonModule, LucidePackageSearch, LucideListTodo, LucideTruck],
    templateUrl: './package-list.html',
})
export class PackageList implements OnInit{
    private paqueteService = inject(PaqueteService);
    private repartidorService = inject(RepartidorService);

    paquetes = this.paqueteService.paquetes;
    repartidores = this.repartidorService.repartidores;

    public EstadoPaquete = EstadoPaquete;
    public Prioridad = Prioridad;

    filtroActual = signal<EstadoPaquete | undefined>(undefined);
    
    readonly opcionesFiltro = [
        { label: 'Todos', valor: undefined },
        { label: 'En Bodega', valor: EstadoPaquete.Bodega },
        { label: 'Asignados', valor: EstadoPaquete.Asignado },
        { label: 'Entregados', valor: EstadoPaquete.Entregado }
    ];


    ngOnInit() {
        this.paqueteService.obtenerPaquetes(1, 10, undefined);
        this.repartidorService.obtenerRepartidores();
    }

    setFiltro(valor: EstadoPaquete | undefined) {
        this.paqueteService.obtenerPaquetes(1, 10, valor);
        this.filtroActual.set(valor);
    }

    asignar(paqueteId: string, selectElement: HTMLSelectElement) {
        // const repartidorId = selectElement.value;
        // if (repartidorId) {
        // this.onAssign.emit({ paqueteId, repartidorId });
        // }
    }
}
