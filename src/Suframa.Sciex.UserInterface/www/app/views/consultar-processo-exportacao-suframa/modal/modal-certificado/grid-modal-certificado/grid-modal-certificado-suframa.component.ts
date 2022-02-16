import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../../../shared/services/application.service';
import { MessagesService } from '../../../../../shared/services/messages.service';
import { ModalService } from '../../../../../shared/services/modal.service';
import { Router } from '@angular/router';

@Component({
	selector: 'app-grid-modal-certificado-suframa',
	templateUrl: './grid-modal-certificado-suframa.component.html'
})

export class ModalGridCertificadoSuframaComponent {
	servico = 'PlanoExportacao';
	ocultarTituloGrid = true;
	checkAll1: any;
	exibirCabecalhoGrid = false;

	@ViewChild('relatorio') relatorio;

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
	) { }

	@Input() lista: any[];
	@Input() total: number;
	@Input() size: number;
	@Input() page: number;
	@Input() sorted: string;
	@Input() exportarListagem: boolean;
	@Input() parametros: any = {};
	@Input() formPai: any;

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

	emitirCertificado(item){
		this.relatorio.emitirCertificado(item.idStatus);
	}

}
