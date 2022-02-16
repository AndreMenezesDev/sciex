import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterRegimeTributarioTesteComponent } from './manter-regime-tributario-teste.component';
import { ManterRegimeTributarioTesteFormularioComponent } from './formulario/formulario.component';
import { ManterRegimeTributarioTesteGridComponent } from './grid/grid.component';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule,
	],
	declarations: [
		ManterRegimeTributarioTesteComponent,
		ManterRegimeTributarioTesteFormularioComponent,
		ManterRegimeTributarioTesteGridComponent,
	]
})
export class ManterRegimeTributarioTesteModule { }
