import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';

import { RelatoriErrosoDueComponent } from './relatorio-erro-due.component';
import { FormularioRelatorioComponent } from './formulario-relatorio/formulario-relatorio.component';


@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule
	],
	declarations: [
		RelatoriErrosoDueComponent,
		FormularioRelatorioComponent
	],
})
export class RelatorioErrosDueModule { }
