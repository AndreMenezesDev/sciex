import { Component, Output, Input, OnInit, EventEmitter, ViewChild, ViewContainerRef } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { parametroAnalistaVM } from '../../../view-model/ParametroAnalistaVM';

@Component({
	selector: 'app-parametrizar-analista-grid',
	templateUrl: './grid.component.html'
})

export class ParametrizarAnalistaGridComponent {
	servicoParametrizarAnalista = 'ParametroAnalista1';
	model: parametroAnalistaVM = new parametroAnalistaVM();
	tipoSistema: number;
	modalAtivarAnalista: boolean;
	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
	) {
	}

	@Input() lista: any;
	@Input() total: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() page: number;

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();

	@ViewChild('appModalVisualizarParametrizacao') appModalVisualizarParametrizacao;
	@ViewChild('appModalBloquearAnalista') appModalBloquearAnalista;

	changeSize($event) {
		this.onChangeSize.emit(+$event);
	}

	changeSort($event) {
		this.sorted = $event.field;
		this.onChangeSort.emit($event);
		this.changePage(this.page);
	}

	changePage($event) {
		this.page = $event;
		this.onChangePage.emit($event);
	}

	visualizarParametrizacao(item) {
		this.appModalVisualizarParametrizacao.abrir(item);
	}

    confirmarInativarAnalista(item, _tipoSistema) {
        
		this.tipoSistema = _tipoSistema;
		this.modal.confirmacao(this.msg.CONFIRMAR_INATIVAR_ANALISTA + (item.analista.nome + ". Confirma a operação?"), 'Inativar Analista', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.inativarAnalista(item);
				}
			});
	}

	inativarAnalista(item) {
		if (this.tipoSistema == 3) // 3:Analise Visual Ativo // 4: Analise Listagem Ativo
		{
			item.statusAnaliseVisual = 0;
			item.horaAnaliseVisualInicio = null;
			item.horaAnaliseVisualFim = null;
		} else if (this.tipoSistema == 4) // inativar analista da analise listagem
		{
			item.statusAnaliseLoteListagem = 0;
			item.horaAnaliseLoteListagemInicio = null;
			item.horaAnaliseLoteListagemFim = null;
		}

		this.applicationService.put<parametroAnalistaVM>(this.servicoParametrizarAnalista, item).subscribe(result => {
			this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "/parametrizarAnalista");
			this.model = result;
		});
	}

	confirmarAtivarAnalista(item, _tipoSistema) {
		this.modalAtivarAnalista = true;
		this.appModalBloquearAnalista.abrir(item, _tipoSistema);
	}

	confirmarBloquearAnalista(item, _tipoSistema) {
		this.modalAtivarAnalista = false;
		this.appModalBloquearAnalista.abrir(item, _tipoSistema);
	}

	confirmarDesbloqueioAnalista(item, _tipoSistema) {
		this.tipoSistema = _tipoSistema;

		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '','')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.desbloquearAnalista(item);
				}
			});
	}

	desbloquearAnalista(item) {
		// 1:Analise Visual Bloqueio // 2: Analise Listagem Bloqueio
		if (this.tipoSistema == 1) {
			item.statusAnaliseVisualBloqueio = 0; // ativar analista da analise visual
			item.horaAnaliseVisualBloqueioInicio = null;
			item.horaAnaliseVisualBloqueioFim = null;
		} 
		// ativar analista da analise listagem
		else if (this.tipoSistema == 2)
		{
			item.statusAnaliseLoteListagemBloqueio = 0;
			item.horaAnaliseLoteListagemBloqueioInicio = null;
			item.horaAnaliseLoteListagemBloqueioFim = null;
		}
			

		this.applicationService.put<parametroAnalistaVM>(this.servicoParametrizarAnalista, item).subscribe(result => {
			this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "");
			this.model = result;
		});
	}
}
