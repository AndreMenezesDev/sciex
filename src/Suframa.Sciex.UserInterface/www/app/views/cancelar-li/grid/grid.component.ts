import { Component, Output, Input, OnInit, EventEmitter, ViewChild, Injectable, OnChanges, SimpleChanges } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterNcmExcecaoVM } from "../../../view-model/ManterNcmExcecaoVM";
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
	selector: 'app-cancelar-li-grid',
	templateUrl: './grid.component.html'
})

@Injectable()
export class ManterCancelarLiGridComponent implements OnChanges {
	servicoNCM = 'cancelarLI';

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
	@Input() ocultarbotaocheck: boolean;

	@ViewChild('appModalHistoricoLI') appModalHistoricoLI;

	public valores: any;
	public check = [];
	public marcado: boolean;

	checkAll1: any;
	@ViewChild("checkedpli") checkedpli;
	@ViewChild("optionchecked") optionchecked;
	listaFiltrada: Array<manterPliVM>;

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();


	ngOnChanges(changes: SimpleChanges) {
		this.limparCheck();
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
	}

	confirmarAtivarStatus(item) {

		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, 'Ativar Status', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.ativarStatus(item);
				}
			});
	}


	limparCheck() {
		if (this.optionchecked != undefined) {
			this.checkedpli.nativeElement.checked = false;
			this.check = [false];
			this.check = [true];
			this.check = [false];
			this.check = [false];
		}
	}

	ativarStatus(item) {

		if (item.status == 0) {
			item.status = 1;
		} else {
			item.status = 0;
		}

		this.applicationService.put<manterNcmExcecaoVM>(this.servicoNCM, item).subscribe(result => {

			this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "");
			this.changePage(this.page);
		}, error => {
			if (this.lista.length == 1)
				this.changePage(1);
			else
				this.changePage(this.page);
		});

	}
	removeCheckAll() {
		this.checkedpli.nativeElement.checked = false;
	}

	onChangeCheckAllGridPLI() {
		for (var i = 0; i < this.lista.length; i++) {
			if (this.checkedpli.nativeElement.checked == true) {
				if (this.lista[i].statusAli == 3) {
					this.lista[i].checkbox = true;
				} else {
					this.lista[i].checkbox = false;
				}
			} else {
				this.lista[i].checkbox = false;
			}
		}
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

	abrirJanelaHistorico(numeroLI) {
		this.appModalHistoricoLI.abrir(numeroLI);
	}


}
