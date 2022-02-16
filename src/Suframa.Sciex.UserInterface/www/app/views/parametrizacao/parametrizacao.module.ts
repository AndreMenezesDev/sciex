import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { ParametrizacaoAnalistasComponent } from './analistas/analistas.component';
import { ParametrizacaoComponent } from './parametrizacao.component';
import { ParametrizacaoDistribuicaoAutomaticaComponent } from './distribuicao-automatica/distribuicao-automatica.component';
import { ParametrizacaoServicosComponent } from './servicos/servicos.component';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { ViewsComponentsModule } from '../../views-components/views-components.module';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule,
		ViewsComponentsModule,
	],
	declarations: [
		ParametrizacaoAnalistasComponent,
		ParametrizacaoComponent,
		ParametrizacaoDistribuicaoAutomaticaComponent,
		ParametrizacaoServicosComponent,
	],
	exports: [
		ParametrizacaoAnalistasComponent,
		ParametrizacaoComponent,
		ParametrizacaoDistribuicaoAutomaticaComponent,
		ParametrizacaoServicosComponent,
	]
})
export class ParametrizacaoModule { }
