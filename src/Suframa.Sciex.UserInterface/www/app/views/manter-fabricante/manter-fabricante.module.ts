import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterFabricanteComponent } from './manter-fabricante.component';
import { ManterFabricanteFormularioComponent } from './formulario/formulario.component';
import { ManterFabricanteGridComponent } from './grid/grid.component';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule,
	],
	declarations: [
		ManterFabricanteComponent,
		ManterFabricanteFormularioComponent,
		ManterFabricanteGridComponent
	],
	providers: []
})
export class ManterFabricanteModule { }
