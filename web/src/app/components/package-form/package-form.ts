import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { PaqueteService } from '../../services/paquete.service';
import { Prioridad } from '../../core/enums/prioridad.enum';
import { CreatePackageDto } from '../../core/DTOs/paquete.dto';
import { LucidePackage, LucideSave } from '@lucide/angular';

@Component({
    selector: 'app-package-form',
    imports: [CommonModule, ReactiveFormsModule, LucidePackage, LucideSave],
    templateUrl: './package-form.html',
})
export class PackageFormComponent {
    private fb = inject(FormBuilder);
    private paqueteService = inject(PaqueteService);

    public PrioridadEnum = Prioridad;
    serverError = signal<string | null>(null);

    packageForm = this.fb.group({
        codigo: ['', [
            Validators.required, 
            Validators.maxLength(20), // CodigoMaxLength
            Validators.pattern(/^[a-zA-Z0-9-]+$/)
        ]],
        descripcion: ['', [
            Validators.required, 
            Validators.minLength(20), // DescripcionMinLength
            Validators.maxLength(300) // DescripcionMaxLength
        ]],
        peso: [0, [Validators.required, Validators.min(0.1)]], // Peso > 0
        prioridad: [Prioridad.Media, [Validators.required]]
    });

    onSubmit() {
        this.serverError.set(null);

        if (this.packageForm.valid) {
            const nuevoPaquete = this.packageForm.value as CreatePackageDto;

            this.paqueteService.crearPaquete(nuevoPaquete).subscribe({
                next: () => {
                    this.packageForm.reset({ 
                        prioridad: Prioridad.Media, 
                        peso: 0,
                        codigo: '',
                        descripcion: ''
                    });
                },
                error: (err) => {
                    if (err.status === 400 && err.error?.detail) {
                        this.serverError.set(err.error.detail);
                    } else {
                        this.serverError.set("Ocurrió un error inesperado al guardar.");
                    }
                }
            });
        } else {
            this.packageForm.markAllAsTouched();
        }
    }

    isInvalid(field: string) {
        const control = this.packageForm.get(field);
        return control && control.invalid && (control.dirty || control.touched);
    }

    setPrioridad(valor: Prioridad) {
        this.packageForm.patchValue({ prioridad: valor });
    }
}
