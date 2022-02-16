import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ConsultarListagemExportacaoComponent } from './consultar-listagem-exportacao.component';
import { ConsultarListagemExportacaoGridComponent } from './grid/grid.component';
import { ConsultarLEInsumoFormularioComponent } from './formulario/formularioInsumo.component';
import { ConsultarLEInsumoGridComponent } from './grid/gridInsumo.component';
import { ModalConsultarInsumoComponent } from './modal/modal-consultar-insumo.component';
@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule
	],
	declarations: [
		ConsultarListagemExportacaoComponent,
		ConsultarListagemExportacaoGridComponent,
		ConsultarLEInsumoFormularioComponent,
		ConsultarLEInsumoGridComponent,
		ModalConsultarInsumoComponent
	],
})
export class ConsultarListagemExportacaoModule { }
