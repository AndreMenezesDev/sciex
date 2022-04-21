import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';

import { RelatorioHistoricoInsumosComponent} from './relatorio-historico-insumos.component';
import { RelatorioComponent } from './relatorio/relatorio.component';


@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule
	],
	declarations: [
		RelatorioHistoricoInsumosComponent,
		RelatorioComponent
	],
})
export class RelatorioHistoricoInsumosModule { }
