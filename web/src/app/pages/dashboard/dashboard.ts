import { Component, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-dashboard',
    imports: [CommonModule, FormsModule],
    templateUrl: './dashboard.html'
})
export class Dashboard {
    paquetes = signal([
        { id: 1, descripcion: 'Laptop Dell Latitude', peso: 2.5, codigo: 'FLK-8924-XX', prioridad: 'Alta', estado: 'En Bodega' },
        { id: 2, descripcion: 'Teclado Mecánico RGB', peso: 1.0, codigo: 'FLK-5521-ST', prioridad: 'Baja', estado: 'Asignado', repartidor: 'Carlos M.' },
    ]);

    repartidores = signal([
        { id: 1, nombre: 'Carlos M.', paquetesAsignados: 2 },
        { id: 2, nombre: 'Ana P.', paquetesAsignados: 3 }, // Esta llena según regla de negocio
        { id: 3, nombre: 'Juan R.', paquetesAsignados: 0 },
    ]);

    // 2. Estado de filtros y errores
    filtroEstado = signal('Todos');
    mensajeError = signal<string | null>(null);

    // 3. Lista filtrada reactiva (Se actualiza sola cuando cambia el signal de paquetes o el filtro)
    paquetesFiltrados = computed(() => {
        const estadoActual = this.filtroEstado();
        if (estadoActual === 'Todos') return this.paquetes();
        return this.paquetes().filter(p => p.estado === estadoActual);
    });

    // 4. Métodos de acción
    asignarRepartidor(paqueteId: number, repartidorId: string) {
        const repId = parseInt(repartidorId);
        const repartidor = this.repartidores().find(r => r.id === repId);

        // Validación visual (luego vendrá del back)
        if (repartidor && repartidor.paquetesAsignados >= 3) {
        this.mensajeError.set("El Repartidor no puede aceptar más paquetes.");
        return;
        }

        this.mensajeError.set(null); // Limpiar error si todo sale bien
        
        // Actualizar el estado del paquete en el Signal
        this.paquetes.update(prev => prev.map(p => 
        p.id === paqueteId ? { ...p, estado: 'Asignado', repartidor: repartidor?.nombre } : p
        ));
    }
}
