import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterFornecedorComponent } from './manter-fornecedor.component';
import { ManterFornecedorFormularioComponent } from './formulario/formulario.component';
import { ManterFornecedorGridComponent } from './grid/grid.component';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule,
	],
	declarations: [
		ManterFornecedorComponent,
		ManterFornecedorFormularioComponent,
		ManterFornecedorGridComponent,
	]
})
export class ManterFornecedorModule { }
