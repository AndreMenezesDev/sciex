import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterControleImportacaoComponent } from './manter-controle-importacao.component';
import { ManterControleImportacaoFormularioComponent } from './formulario/formulario.component';
import { ManterControleImportacaoGridComponent } from './grid/grid.component';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule,
	],
	declarations: [
		ManterControleImportacaoComponent,
		ManterControleImportacaoFormularioComponent,
		ManterControleImportacaoGridComponent
	],
})
export class ManterControleImportacaoModule { }
