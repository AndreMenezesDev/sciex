import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ChartsModule } from 'ng2-charts';
import { ManterViaTransporteComponent } from './manter-via-transporte.component';
import { ManterViaTransporteGridComponent } from './grid/grid.component';
import { ManterViaTransporteFormularioComponent } from './formulario/formulario.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
		SharedModule,
		ChartsModule
    ],
    declarations: [
        ManterViaTransporteComponent,
        ManterViaTransporteFormularioComponent,
        ManterViaTransporteGridComponent,
	],
})
export class ManterViaTransporteModule { }
