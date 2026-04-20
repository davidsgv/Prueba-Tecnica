import { Component, inject, input, OnInit, output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaqueteService } from '../../services/paquete.service';
import { RepartidorService } from '../../services/repartidor.service';
import { EstadoPaquete } from '../../core/enums/estado-paquete.enum';
import { Prioridad } from '../../core/enums/prioridad.enum';
import { LucideListTodo, LucideTruck, LucideCircleAlert, LucideCircleX, LucidePackageCheck } from '@lucide/angular';
import { RepartidorSelectComponent } from '../repartidor-select/repartidor-select';
import { Package } from '../../core/models/package.models';

@Component({
    selector: 'app-package-list',
    imports: [CommonModule, LucideListTodo, LucideTruck, RepartidorSelectComponent, LucideCircleAlert, LucideCircleX, LucidePackageCheck],
    templateUrl: './package-list.html',
})
export class PackageList implements OnInit{
    private paqueteService = inject(PaqueteService);

    paquetes = this.paqueteService.paquetes;
    repartidorError = this.paqueteService.asignarError

    protected readonly EstadoPaquete = EstadoPaquete;
    protected readonly Prioridad = Prioridad;

    filtroActual = signal<EstadoPaquete | undefined>(undefined);
    
    readonly opcionesFiltro = [
        { label: 'Todos', valor: undefined },
        { label: 'En Bodega', valor: EstadoPaquete.Bodega },
        { label: 'Asignados', valor: EstadoPaquete.Asignado },
        { label: 'Entregados', valor: EstadoPaquete.Entregado }
    ];


    ngOnInit() {
        this.paqueteService.obtenerPaquetes(1, 10, undefined);
    }

    setFiltro(valor: EstadoPaquete | undefined) {
        this.paqueteService.obtenerPaquetes(1, 10, valor);
        this.filtroActual.set(valor);
    }

    isAltaPrioridad(prioridad: any): boolean {
        if (prioridad === Prioridad.Alta) return true;
        if (prioridad === Prioridad[Prioridad.Alta]) return true;
        return false;
    }

    closeError(){
        this.repartidorError.set(null);
    }

    markAsDeliered(paquete: Package){
        this.paqueteService.actualizarPaquete(paquete.id,{
            descripcion: paquete.descripcion,
            peso: paquete.peso,
            prioridad: paquete.prioridad, 
            estado: EstadoPaquete.Entregado
        }).subscribe({
            next: ()=> console.log("paquete actualizado")
        });
    }
}
