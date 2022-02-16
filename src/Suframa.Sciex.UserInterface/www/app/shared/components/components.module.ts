import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { ReCaptchaModule } from 'angular2-recaptcha';

// Importação de componentes customizados devem ser organizados por ordem alfabética
import { ArquivoComponent } from './arquivo/arquivo.component';
import { CalendarComponent } from './calendar/calendar.component';
import { CaptchaComponent } from './captcha/captcha.component';
import { DestacaCampoComponent } from './destaca-campo/destaca-campo.component';
import { DropListComponent } from './drop-list/drop-list.component';
import { GridCabecalhoComponent } from './grid-cabecalho/grid-cabecalho.component';
import { GridComponent } from './grid/grid.component';
import { GridCancelarLiComponent } from './grid-cancelar-li/grid.component';
import { GridReprocessarPliComponent } from './grid-reprocessar-pli/grid.component';
import { ItensPorPaginaComponent } from './itens-por-pagina/itens-por-pagina.component';
import { ModalAlertaComponent } from './modal/modal-alerta/modal-alerta.component';
import { ModalConfirmacaoComponent } from './modal/modal-confirmacao/modal-confirmacao.component';
import { ModalConfirmacaoVisualizacaoComponent } from "./modal/modal-confirmacao-visualizacao/modal-confirmacao-visualizacao.component";
import { ModalPromptComponent } from './modal/modal-prompt/modal-prompt.component';
import { ModalPromptVisualizacaoComponent } from "./modal/modal-prompt-visualizacao/modal-prompt-visualizacao.component";
import { ModalRespostaComponent } from "./modal/modal-resposta/modal-resposta.component";
import { OrdenacaoComponent } from './ordenacao/ordenacao.component';
import { PaginacaoComponent } from './paginacao/paginacao.component';
import { ResultadoPorPaginasComponent } from './resultado-por-paginas/resultado-por-paginas.component';
import { RodapeMenuComponent } from './rodape-menu/rodape-menu.component';
import { SubTituloComponent } from './sub-titulo/sub-titulo.component';
import { TituloComponent } from './titulo/titulo.component';
import { AcoesComponent } from './acoes/acoes.component';

import { Select2CustomComponent } from './select2-custom/select2-custom.component';
import { DropListSelectComponent } from './drop-list/drop-list-select.component';
import { DropListSelect2Component } from './drop-list2/drop-list-select2.component';
import { UploadFileComponent } from './upload/upload-file.component';
import { FileUploadModule } from 'ng2-file-upload/ng2-file-upload';
@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		ReCaptchaModule,
		ReactiveFormsModule,
		FileUploadModule
	],
	declarations: [
		ArquivoComponent,
		CalendarComponent,
		CaptchaComponent,
		DestacaCampoComponent,
		DropListComponent,
		GridCabecalhoComponent,
		GridComponent,
		GridCancelarLiComponent,
		ItensPorPaginaComponent,
		ModalAlertaComponent,
		ModalConfirmacaoComponent,
		ModalConfirmacaoVisualizacaoComponent,
		ModalPromptComponent,
        ModalPromptVisualizacaoComponent,
        ModalRespostaComponent,
		OrdenacaoComponent,
		PaginacaoComponent,
		ResultadoPorPaginasComponent,
		RodapeMenuComponent,
		SubTituloComponent,
		TituloComponent,
        AcoesComponent,   
        Select2CustomComponent,
		DropListSelectComponent,
		DropListSelect2Component,
		UploadFileComponent,
		GridReprocessarPliComponent
	],
	exports: [
		ArquivoComponent,
		CalendarComponent,
		CaptchaComponent,
		DestacaCampoComponent,
		DropListComponent,
		GridCabecalhoComponent,
		GridComponent,
		GridCancelarLiComponent,
		GridReprocessarPliComponent,
		ItensPorPaginaComponent,
		ModalAlertaComponent,
		ModalConfirmacaoComponent,
		ModalConfirmacaoVisualizacaoComponent,
		ModalPromptComponent,
        ModalPromptVisualizacaoComponent,
        ModalRespostaComponent,
		OrdenacaoComponent,
		PaginacaoComponent,
		ResultadoPorPaginasComponent,
		RodapeMenuComponent,
		SubTituloComponent,
        TituloComponent,
        AcoesComponent,
		DropListSelectComponent,
		DropListSelect2Component,
		UploadFileComponent
  
	],
	// Don't forget to add the component to entryComponents section
	entryComponents: [
		ModalAlertaComponent,
		ModalConfirmacaoComponent,
		ModalConfirmacaoVisualizacaoComponent,
		ModalPromptComponent,
        ModalPromptVisualizacaoComponent,
        ModalRespostaComponent
	]
})
export class ComponentsModule { }
