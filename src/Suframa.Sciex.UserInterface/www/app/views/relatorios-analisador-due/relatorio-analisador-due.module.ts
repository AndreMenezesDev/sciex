import { RelatorioComponent } from './relatorio/relatorio.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';

import { RelatorioAnalisadorDue } from './relatorio-analisador-due.component';


@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule
	],
	declarations: [
		RelatorioAnalisadorDue,
		RelatorioComponent
	],
})
export class RelatorioAnalisadorDueModule { }
