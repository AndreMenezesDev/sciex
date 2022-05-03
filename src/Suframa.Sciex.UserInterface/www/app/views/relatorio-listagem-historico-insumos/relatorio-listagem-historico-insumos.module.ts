import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';

import { RelatoriListagensHistoricoInsumoComponent } from './relatorio-listagem-historico-insumos.component';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule
	],
	declarations: [
		RelatoriListagensHistoricoInsumoComponent
	],
})
export class RelatorioListagemHistoricoInsumosModule { }
