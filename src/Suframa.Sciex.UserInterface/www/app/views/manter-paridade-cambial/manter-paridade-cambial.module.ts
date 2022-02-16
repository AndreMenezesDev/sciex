import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterParidadeCambialComponent } from './manter-paridade-cambial.component';
import { ManterParidadeCambialFormularioComponent } from './formulario/formulario.component';
import { ManterParidadeCambialGridComponent } from './grid/grid.component';
import { SincronizarParidadeCambialComponent } from './sincronizar-paridade/sincronizar-paridade-cambial.component';
import { ModalAjudaComponent } from './modal/modal-ajuda.component';


@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule,
	],
	declarations: [
		ManterParidadeCambialComponent,
		ManterParidadeCambialFormularioComponent,
		ManterParidadeCambialGridComponent,
		SincronizarParidadeCambialComponent,
		ModalAjudaComponent
	],
})
export class ManterParidadeCambialModule { }
