import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterTipoDeclaracaoComponent } from './manter-tipo-declaracao.component';
import { ManterTipoDeclaracaoGridComponent } from './grid/grid.component';
import { ManterTipoDeclaracaoFormularioComponent } from './formulario/formulario.component';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule,
	],
	declarations: [
		ManterTipoDeclaracaoComponent,
		ManterTipoDeclaracaoGridComponent,
		ManterTipoDeclaracaoFormularioComponent,
	],
})
export class ManterTipoDeclaracaoModule { }
