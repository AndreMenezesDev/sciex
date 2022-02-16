import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterCodigoContaComponent } from './manter-codigo-conta.component';
import { ManterCodigoContaFormularioComponent } from './formulario/formulario.component';
import { ManterCodigoContaGridComponent } from './grid/grid.component';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule,
	],
	declarations: [
		ManterCodigoContaComponent,
		ManterCodigoContaFormularioComponent,
		ManterCodigoContaGridComponent
	],
})
export class ManterCodigoContaModule { }
