import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { AnalisarListagemExportacaoComponent } from './analisar-listagem-exportacao.component'
import { AnalisarListagemExportacaoGridComponent } from './grid/grid.component';
import { AnalisarLEInsumoFormularioComponent } from './formulario/formularioInsumo.component';
import { AnalisarLEInsumoGridComponent } from './grid/gridInsumo.component';
import { ModalAnalisarInsumoComponent } from './modal/modal-analisar-insumo.component';
@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule
	],
	declarations: [
		AnalisarListagemExportacaoComponent,
		AnalisarListagemExportacaoGridComponent,
		AnalisarLEInsumoFormularioComponent,
		AnalisarLEInsumoGridComponent,
		ModalAnalisarInsumoComponent
	],
})
export class AnalisarListagemExportacaoModule { }
