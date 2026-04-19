import { Component, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PackageList } from '../../components/package-list/package-list';
import { PackageFormComponent } from '../../components/package-form/package-form';

@Component({
    selector: 'app-dashboard',
    imports: [PackageList, PackageFormComponent],
    templateUrl: './dashboard.html'
})
export class Dashboard {
}
