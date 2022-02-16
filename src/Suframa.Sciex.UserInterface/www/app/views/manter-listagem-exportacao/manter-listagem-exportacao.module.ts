import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterListagemExportacaoComponent } from './manter-listagem-exportacao.component'
import { ManterListagemExportacaoGridComponent } from './grid/grid.component';
import { ManterLEProdutoFormularioComponent } from './formulario/formulario.component';
import { ManterLEInsumoFormularioComponent } from './formulario/formularioInsumo.component';
import { ManterLEInsumoGridComponent } from './grid/gridInsumo.component';
import { ModalNovoInsumoComponent } from './modal/modal-novo-insumo.component';
@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule
	],
	declarations: [
		ManterListagemExportacaoComponent,
		ManterListagemExportacaoGridComponent,
		ManterLEProdutoFormularioComponent,
		ManterLEInsumoFormularioComponent,
		ManterLEInsumoGridComponent,
		ModalNovoInsumoComponent
	],
})
export class ManterListagemExportacaoModule { }
