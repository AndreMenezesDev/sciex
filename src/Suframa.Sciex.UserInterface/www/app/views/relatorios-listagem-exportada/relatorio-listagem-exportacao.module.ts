import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';

import { RelatorioListagemExportacaoComponent } from './relatorio-listagem-exportacao.component';


@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule
	],
	declarations: [
		RelatorioListagemExportacaoComponent
	],
})
export class RelatorListagemExportacaoModule { }
