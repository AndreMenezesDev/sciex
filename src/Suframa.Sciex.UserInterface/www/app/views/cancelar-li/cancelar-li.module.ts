import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterCancelarLiGridComponent } from './grid/grid.component';
import { ManterCancelarLiComponent } from './cancelar-li.component';
import { ModalHistoricoLIComponent } from './modal-historico/modal-historico-li.component';
import { ManterCancelarLiHistoricoGridComponent } from './grid/grid-historico.component';
import { CancelaLIFormularioPLIFormularioComponent } from './formulario/formulario-cancelar-consultar-pli.component';
import { CancelaLIFormularioALIFormularioComponent } from './formulario/formulario-cancelar-detalhamento-ali.component';
import { CancelaLIFormularioLIFormularioComponent } from './formulario/formulario-cancelar-detalhamento-li.component';
import { CancelaLIFormularioLISubstitutivoFormularioComponent } from './formulario/formulario-cancelar-detalhamento-li-substitutivo.component';
import { CancelaLIFormularioALISubstitutivoFormularioComponent} from './formulario/formulario-cancelar-detalhamento-ali-substitutivo.component';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule,
	],
	declarations: [
		ManterCancelarLiComponent,
		ManterCancelarLiGridComponent,
		ModalHistoricoLIComponent,
		ManterCancelarLiHistoricoGridComponent,
		CancelaLIFormularioPLIFormularioComponent,
		CancelaLIFormularioALIFormularioComponent,
		CancelaLIFormularioLIFormularioComponent,
		CancelaLIFormularioLISubstitutivoFormularioComponent,
		CancelaLIFormularioALISubstitutivoFormularioComponent
	],
})
export class ManterCancelarLiModule { }
