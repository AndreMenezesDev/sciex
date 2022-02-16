import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterPliMercadoriaVM } from '../../../view-model/ManterPliMercadoriaVM';
import { manterPliDetalheMercadoriaVM } from '../../../view-model/ManterPliDetalheMercadoriaVM';
import { manterPliProcessoAnuenteVM } from '../../../view-model/ManterPliProcessoAnuenteVM';
import { manterPliProdutoVM } from '../../../view-model/ManterPliProdutoVM';

@Component({
	selector: 'app-manter-pli-processo-anuente-grid',
	templateUrl: './grid-processo-anuente.component.html'
})

export class ManterPliProcessoAnuenteGridComponent {
	servico = 'PliProcessoAnuente';
	servicoGrid = 'PliProcessoAnuenteGrid';

	modelMercadoria: manterPliMercadoriaVM = new manterPliMercadoriaVM();

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
	@Input() isShowPanel: boolean = false;
	@Input() somenteLeitura: boolean = false;


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

	alterarDetalheMercadoria(item: manterPliProcessoAnuenteVM) {
		if (item.idPliProcessoAnuente == undefined) {
			item.index = this.parametros.modal.listaProcessoAnuente.indexOf(item);
		}
		this.parametros.modal.modelProcessoAnuente = item;
	}

	deletar(item: manterPliProcessoAnuenteVM) {

		if (this.lista.length > 0) {
			const index: number = this.lista.indexOf(item);
			if (index !== -1) {
				item.excluir = true;
				this.lista[index] = item;
			}
			
			this.lista = this.lista.filter(
				mer => mer.excluir === false);

			this.parametros.modal.listaProcessoAnuente = [];
			for (var i = 0; i < this.lista.length; i++) {
				this.parametros.modal.listaProcessoAnuente.push(JSON.parse(JSON.stringify(this.lista[i])));
			}						
			this.parametros.modal.limparCamposComplemento();
					
		}


		//if (item.idPliProcessoAnuente == undefined) {
		//	if (this.lista.length > 0) {
		//		const index: number = this.lista.indexOf(item);
		//		if (index !== -1) {
		//			this.lista.splice(index, 1);
		//		}
		//		this.parametros.modal.listaProcessoAnuente = this.lista;
		//		this.parametros.modal.limparCamposComplemento();
		//	}
		//}
		//else {
		//	this.applicationService.delete(this.servico, item.idPliProcessoAnuente).subscribe(result => {

		//		this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", '').subscribe(isConfirmado => {

		//			if (this.lista.length == 1)
		//				this.changePage(this.page - 1);
		//			else
		//				this.changePage(this.page);

		//		});
		//	}, error => {
		//		if (this.lista.length == 1)
		//			this.changePage(this.page - 1);
		//		else
		//			this.changePage(this.page);
		//	});
		//}


	}

	confirmarExclusao(item: manterPliProcessoAnuenteVM) {
		//this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
		//	.subscribe(isConfirmado => {
		//		if (isConfirmado) {
					this.deletar(item);
			//	}
			//});
	}

}
