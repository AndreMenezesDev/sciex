import { Component, Output, Input, OnInit, EventEmitter } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterRegimeTributarioMercadoriaVM } from "../../../view-model/ManterRegimeTributarioMercadoriaVM";

@Component({
	selector: 'app-manter-regime-tributario-mercadoria-grid',
	templateUrl: './grid.component.html'
})

export class ManterRegimeTributarioMercadoriaGridComponent {
	servicoRegimeTributarioMercadoria = 'RegimeTributarioMercadoria';
	model: manterRegimeTributarioMercadoriaVM = new manterRegimeTributarioMercadoriaVM();

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
	) { }

	@Input() lista: any[];
	@Input() total: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() page: number;
	@Input() exportarListagem: boolean;
	@Input() parametros: any = {};

	public valores: any;

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();

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

	confirmarAtivarStatus(item) {
		item.isEditStatus = 1;
		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, 'Ativar Status', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.ativarStatus(item);
				}
			});
	}

	confirmarInativarStatus(item) {
		item.isEditStatus = 1;
		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, 'Inativar Status', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.ativarStatus(item);
				}
			});
	}

	ativarStatus(item) {
		if (item.status == 0)
		{
			item.status = 1;
		} else
		{
			item.status = 0;
		}

		this.applicationService.put<manterRegimeTributarioMercadoriaVM>(this.servicoRegimeTributarioMercadoria, item).subscribe(result => {
			this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "/manter-regime-tributario-mercadoria");
			this.model = result;
			this.changePage(this.page);
		});
	}

	deletar(id) {
		this.applicationService.delete(this.servicoRegimeTributarioMercadoria, id).subscribe(result => {
			this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", '').subscribe(isConfirmado => {
				if (this.lista.length == 1)
					this.changePage(this.page - 1);
				else
					this.changePage(this.page);
			});
		}, error => {
			if (this.lista.length == 1)
				this.changePage(this.page - 1);
			else
				this.changePage(this.page);
		});
	}

	confirmarExclusao(id) {
		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.deletar(id);
				}
			});
	}
}
