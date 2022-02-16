import { Component, Output, Input, OnInit, EventEmitter, ViewChild, Injectable, OnChanges, SimpleChanges } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterNcmExcecaoVM } from "../../../view-model/ManterNcmExcecaoVM";
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { forEach } from '@angular/router/src/utils/collection';
import { manterLiVM } from '../../../view-model/ManterLiVM';

@Component({
	selector: 'app-cancelamento-li-grid',
	templateUrl: './grid.component.html'
})

@Injectable()
export class ManterCancelamentoLiGridComponent implements OnChanges {
	servicoLi = "Li";

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
	@Input() parametros: any = {};
	@Input() ocultarbotaocheck: boolean;

	public isHide: boolean = true;
	public isShowPanel: boolean = false;

	public valores: any;
	public check = [];
	public marcado: boolean;

	checkAll1: any;
	@ViewChild("checkedpli") checkedpli;
	@ViewChild("optionchecked") optionchecked;
	listaFiltrada: Array<manterLiVM>;

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();
	@Output() onDiscardChecked: EventEmitter<any> = new EventEmitter();

	ngOnChanges(changes: SimpleChanges) {
		this.limparCheck();
		this.checkedpli.nativeElement.checked = false;
	}

	discardChecked($event) {
		this.removeCheckAll();
	}

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
		this.removeCheckAll();
	}

	limparCheck() {

		if (this.optionchecked != undefined) {

			this.check = [false];
			this.check = [true];
			this.check = [false];
			this.check = [false];
		}
	}


	public removeCheckAll() {
		this.checkedpli.nativeElement.checked = false;
	}

	onChangeCheckAllGridPLI() {

		for (var i = 0; i < this.lista.length; i++) {

			//Se o checkAll estiver marcado...
			if (this.checkedpli.nativeElement.checked == true) {


				this.lista[i].checkbox = true;
				this.checkAll1 = true;


			} else {
				this.lista[i].checkbox = false;
				this.checkAll1 = false;
			}
		}
	}

	confirmarInativarStatus(item) {

		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, 'Inativar Status', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					//this.ativarStatus(item);
				}
			});
	}

}
