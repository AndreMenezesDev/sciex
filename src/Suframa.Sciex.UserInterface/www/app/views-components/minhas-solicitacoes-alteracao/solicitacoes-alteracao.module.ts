import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { SolicitacoesAlteracaoComponent } from './solicitacoes-alteracao.component';
import { GridSolicitacoesAlteracaoComponent } from './grid/grid.component';
import { DetalheSolicitacaoComponent } from './detalhes-solicitacao/detalhe-solicitacao.component';
import { GridDetalheSolicitacaoComponent } from './detalhes-solicitacao/grid/grid.component';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule
	],
	declarations: [
		SolicitacoesAlteracaoComponent,
		GridSolicitacoesAlteracaoComponent,
		DetalheSolicitacaoComponent,
		GridDetalheSolicitacaoComponent
	],
})
export class SolicitacoesAlteracaoModule { }
