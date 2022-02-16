import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { Router } from '@angular/router';
import { AnalisarLEInsumoFormularioComponent } from '../formulario/formularioInsumo.component';

@Component({
	selector: 'app-analisar-le-insumo-grid',
	templateUrl: './gridInsumo.component.html'
})

export class AnalisarLEInsumoGridComponent {
	servico = 'LEInsumo';
	model: any;

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
		private AnalisarLEInsumoFormularioComponent: AnalisarLEInsumoFormularioComponent,
	) { }

	@Input() lista: any[];
	@Input() total: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() page: number;
	@Input() exportarListagem: boolean;
	@Input() parametros: any = {};
	@Input() somenteLeitura: any = {};
	@Input() isAlteracao: boolean;
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

		this.applicationService.delete(this.servico, id).subscribe(result => {

			this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", '').subscribe(isConfirmado => {

				this.AnalisarLEInsumoFormularioComponent.buscarInsumos();
			});
		}, error => {
		});

	}

	analisar(item){
		this.AnalisarLEInsumoFormularioComponent.analisar(item);
	}

	alterar(item){
		this.AnalisarLEInsumoFormularioComponent.alterar(item);
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
