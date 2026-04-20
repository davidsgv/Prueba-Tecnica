import { Component, inject, input, isSignal, OnInit, signal } from '@angular/core';
import { RepartidorService } from '../../services/repartidor.service';
import { Package } from '../../core/models/package.models';
import { PaqueteService } from '../../services/paquete.service';

@Component({
    selector: 'app-repartidor-select',
    templateUrl: './repartidor-select.html'
})
export class RepartidorSelectComponent implements OnInit{
    private paqueteService = inject(PaqueteService);
    private repartidorService = inject(RepartidorService);

    repartidores = this.repartidorService.repartidores;
    selectedRepartidorId = signal<string>('');
    paquete = input.required<Package>();
    isLoading = signal<boolean>(false);

    ngOnInit() {
        if(this.repartidores().length == 0)
            this.repartidorService.obtenerRepartidores();
    }

    asignar(repartidorId: string) {
        const paqueteId = this.paquete().id;

        if (!repartidorId) return;
        this.isLoading.set(true);

        this.paqueteService.asignarRepartidor(paqueteId, repartidorId).subscribe({
            next: ()=> this.paqueteService.obtenerPaquetes(),
            complete: ()=> this.isLoading.set(false)
        })
    }

    isEmpty(value: string){
        return value === ""
    }
}
