import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ConsultarEntradaDiComponent } from './consultar-entrada-di.component';
import { ConsultarEntradaDiGridComponent } from './grid/grid.component';
import { ConsultarEntradaDiProcessadoFormularioComponent } from './formulario/consultar-entrada-di-processado.component';
import { ConsultarEntradaDIProcessadoGridComponent } from './grid/grid-entrada-di-processado.component';
import { ConsultarEntradaDiErrosFormularioComponent } from './formulario/consultar-entrada-di-erros.component';
import { ConsultarEntradaDiErrosGridComponent } from './grid/grid-entrada-di-erros.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
		SharedModule
    ],
    declarations: [
        ConsultarEntradaDiComponent,
        ConsultarEntradaDiGridComponent,
        ConsultarEntradaDiProcessadoFormularioComponent,
        ConsultarEntradaDIProcessadoGridComponent,
        ConsultarEntradaDiErrosFormularioComponent,
        ConsultarEntradaDiErrosGridComponent
	],
})
export class ConsultarEntradaDiModule { }
