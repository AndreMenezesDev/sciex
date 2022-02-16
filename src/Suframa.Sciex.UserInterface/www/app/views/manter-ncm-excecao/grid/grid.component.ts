import { Component, Output, Input, OnInit, EventEmitter } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterNcmExcecaoVM } from "../../../view-model/ManterNcmExcecaoVM";

@Component({
	selector: 'app-manter-ncm-excecao-grid',
	templateUrl: './grid.component.html'
})

export class ManterNCMExcecaoGridComponent {
	servicoNCM = 'ncmExcecao';

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

		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, 'Ativar Status', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.ativarStatus(item);
				}
			});
	}

	ativarStatus(item) {

		if (item.status == 0) {
			item.status = 1;
		} else {
			item.status = 0;
		}

		this.applicationService.put<manterNcmExcecaoVM>(this.servicoNCM, item).subscribe(result => {

			this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "")
				.subscribe(isConfirmado => {
					if (isConfirmado) {
						this.changePage(this.page);
					}
				});

		}, error => {

			if (this.lista.length == 1)
				this.changePage(1);
			else
				this.changePage(this.page);
		});

	}

	confirmarInativarStatus(item) {

		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, 'Inativar Status', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.ativarStatus(item);
				}
			});
	}



	deletar(id) {
		this.applicationService.delete(this.servicoNCM, id).subscribe(result => {

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
