import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterRegimeTributarioMercadoriaComponent } from './manter-regime-tributario-mercadoria.component';
import { ManterRegimeTributarioMercadoriaFormularioComponent } from './formulario/formulario.component';
import { ManterRegimeTributarioMercadoriaGridComponent } from './grid/grid.component';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule,
	],
	declarations: [
		ManterRegimeTributarioMercadoriaComponent,
		ManterRegimeTributarioMercadoriaFormularioComponent,
		ManterRegimeTributarioMercadoriaGridComponent
	],
})
export class ManterRegimeTributarioMercadoriaModule { }
