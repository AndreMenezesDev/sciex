import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { Router } from '@angular/router';

@Component({
	selector: 'app-manter-listagem-exportacao-grid',
	templateUrl: './grid.component.html'
})

export class ManterListagemExportacaoGridComponent {
	servico = 'LEEntregarProduto';
	servicoLe = 'LEProduto';
	servicoLeAlteracao = 'LESolicitarAlteracao';
	model: any;

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
	) { }

	@Input() lista: any[];
	@Input() total: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() page: number;
	@Input() exportarListagem: boolean;
	@Input() parametros: any = {};

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();

	changeSize($event) {
		this.onChangeSize.emit($event);
	}

	changeSort($event) {
		this.sorted = $event.field;
		this.onChangeSort.emit($event);
		this.changePage(this.page);
	}

	changePage($event) {
		this.page = $event;
		this.onChangePage.emit($event);
		console.log($event);
	}

	deletar(id) {

		this.applicationService.delete(this.servicoLe, id).subscribe(result => {

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

	solicitarAlteracao(item) {
		this.parametros.idLe = item.idLe;
		this.applicationService.put(this.servicoLeAlteracao, this.parametros).subscribe((result: any) => {
			this.modal.resposta(result.mensagem, "Sucesso", '').subscribe(isConfirmado => {
				if (this.lista.length == 1)
					this.changePage(this.page - 1);
				else
					this.changePage(this.page);
			});
		});
	}

	confirmarExclusao(id) {
		this.modal.confirmacao("Deseja realmente excluir este produto? Todos os insumos vinculados a este serão excluídos.", '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.deletar(id);
				}
			});
	}

	confirmarEntrega(item) {
		this.modal.confirmacao('Confirma operação?', '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {

					this.applicationService.put(this.servico, item).subscribe((result: any) => {
						if(result != null && result.mensagem == "Entrega realizada com sucesso!"){
							this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", '').subscribe(isConfirmado => {

								if (this.lista.length == 1)
									this.changePage(this.page - 1);
								else
									this.changePage(this.page);
							});
						}else{
							this.modal.alerta(result.mensagem, "Atenção", '')
						}
					});
				}
			});
	}
}
