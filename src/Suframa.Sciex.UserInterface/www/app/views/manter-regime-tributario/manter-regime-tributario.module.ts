import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterRegimeTributarioComponent } from './manter-regime-tributario.component';
import { ManterRegimeTributarioFormularioComponent } from './formulario/formulario.component';
import { ManterRegimeTributarioGridComponent } from './grid/grid.component';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule,
	],
	declarations: [
		ManterRegimeTributarioComponent,
		ManterRegimeTributarioFormularioComponent,
		ManterRegimeTributarioGridComponent,
	]
})
export class ManterRegimeTributarioModule { }
