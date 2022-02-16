import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterCodigoUtilizacaoComponent } from './manter-codigo-utilizacao.component';
import { ManterCodigoUtilizacaoFormularioComponent } from './formulario/formulario.component';
import { ManterCodigoUtilizacaoGridComponent } from './grid/grid.component';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule,
	],
	declarations: [
		ManterCodigoUtilizacaoComponent,
		ManterCodigoUtilizacaoFormularioComponent,
		ManterCodigoUtilizacaoGridComponent
	],
})
export class ManterCodigoUtilizacaoModule { }
