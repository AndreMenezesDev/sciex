import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterNcmComponent } from './manter-ncm.component';
import { ManterNcmFormularioComponent } from './formulario/formulario.component';
import { ManterNcmGridComponent } from './grid/grid.component';
import { ModalJustificativaStatusComponent } from './modal-justificativa-status/modal-justificativa-status.component'
import { ModalHistoricoNcmComponent } from './modal-historico/modal-historico-ncm.component';
import { NcmHistoricoGridComponent } from './grid-historico/grid-historico-ncm.component';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule,
	],
	declarations: [
		ManterNcmComponent,
		ManterNcmFormularioComponent,
		ManterNcmGridComponent,
		ModalJustificativaStatusComponent,
		ModalHistoricoNcmComponent,
		NcmHistoricoGridComponent
	],
})
export class ManterNcmModule { }
