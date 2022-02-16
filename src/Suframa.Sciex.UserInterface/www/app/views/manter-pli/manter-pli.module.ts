import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterPliComponent } from './manter-pli.component';
import { ManterPliFormularioComponent } from './formulario/formulario.component';
import { ManterPliGridComponent } from './grid/grid.component';
import { ManterPliMercadoriaGridComponent } from './grid/grid-mercadoria.component';
import { ModalNovoPliComponent } from './modal-novo/modal-novo-pli.component';
import { ModalMercadoriaPliComponent } from './modal-mercadoria/modal-mercadoria-pli.component';
import { ModalMercadoriaPliComercializacaoComponent } from './modal-mercadoria-comercializacao/modal-mercadoria-pli-comercializacao.component';
import { ManterPliDetalheMercadoriaGridComponent } from './grid/grid-detalhe-mercadoria.component';
import { ManterPliProcessoAnuenteGridComponent } from './grid/grid-processo-anuente.component';
import { ModalAplicarParametrosComponent } from './modal-aplicar-parametros/modal-aplicar-parametros.component';
import { ManterPliFormularioSubstutivoComponent } from "./formulario/formularioSubstitutivo.component";
import { ManterPliFormularioComercializacaoComponent } from "./formulario/formularioComercializacao.component";
import { ManterPliFormularioComercializacaoSubstitutivoComponent } from "./formulario/formularioComercializacaoSubstitutivo.component";
import { ModalRespostaMotivoComponent } from './modal-reposta/modal-resposta-motivo.component';
import { ManterPliFormularioRetificadorComponent } from './formulario/formularioRetificador.component';
import { ModalAddLiComponent } from './modal-adicionar-li/modal-adicionar-li.component';
import { ManterPliFormularioRetificadorComercializacaoComponent } from './formulario/formularioRetificadorComercializacao.component';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		RouterModule,
		SharedModule
	],
	declarations: [
		ManterPliComponent,
		ManterPliFormularioComponent,
		ManterPliGridComponent,
		ModalNovoPliComponent,
		ModalMercadoriaPliComponent,
		ModalMercadoriaPliComercializacaoComponent,
		ManterPliMercadoriaGridComponent,
		ManterPliDetalheMercadoriaGridComponent,
		ManterPliProcessoAnuenteGridComponent,
        ModalAplicarParametrosComponent,
		ManterPliFormularioSubstutivoComponent,
		ManterPliFormularioComercializacaoComponent,
		ManterPliFormularioComercializacaoSubstitutivoComponent,
		ModalRespostaMotivoComponent,
		ManterPliFormularioRetificadorComponent,
		ModalAddLiComponent,
		ManterPliFormularioRetificadorComercializacaoComponent
	],
})
export class ManterPliModule { }
