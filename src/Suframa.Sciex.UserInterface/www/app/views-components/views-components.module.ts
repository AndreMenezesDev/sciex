import { NgModule, LOCALE_ID } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MaskModule } from 'soft-angular-mask';
import { SharedModule } from '../shared/shared.module';
import { FiltroComponent } from './filtro/filtro.component';
import { ModalServicosComponent } from './modal-servicos/modal-servicos.component';
import { DropListRepresentacaoComponent } from './drop-list-representacao/drop-list-representacao.component';
import { SolicitacoesAlteracaoModule } from './minhas-solicitacoes-alteracao/solicitacoes-alteracao.module';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		MaskModule,
		RouterModule,
		SharedModule,
		SolicitacoesAlteracaoModule
	],
	declarations: [
		DropListRepresentacaoComponent,
		FiltroComponent,
		ModalServicosComponent
	],
	exports: [
		DropListRepresentacaoComponent,
		FiltroComponent,
		ModalServicosComponent
	],
	providers: [
	],
	// Don't forget to add the component to entryComponents section
	entryComponents: [
	]
})
export class ViewsComponentsModule { }
