import { Component, Output, Input, OnInit, EventEmitter, ViewChild, Injectable, OnChanges, SimpleChanges } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterNcmExcecaoVM } from "../../../view-model/ManterNcmExcecaoVM";
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { forEach } from '@angular/router/src/utils/collection';
import { TaxaGrupoBeneficioVM } from '../../../view-model/TaxaGrupoBeneficioVM';
import { ModalJustificativaStatusGrupoBeneficioComponent } from '../modal-justificativa-status/modal-justificativa-status.component';
import { ModalHistoricoBeneficioComponent } from '../modal-historico/modal-historico-beneficio.component';
import { ModalEmpresaPDIComponent } from '../modal-empresa-pdi/modal-empresa-pdi.component';

@Component({
	selector: 'app-manter-grupo-beneficio-grid',
	templateUrl: './grid-manter-grupo-beneficio.component.html'
})

@Injectable()
export class ManterGupoBeneficioGridComponent implements OnChanges {

	servicoNCM = 'consultarPLI';
	model: TaxaGrupoBeneficioVM = new TaxaGrupoBeneficioVM();


	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService
	) { }

	@Input() lista: any[];
	@Input() total: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() page: number;
	@Input() exportarListagem: boolean;
	@Input() parametros: any = {};
	@Input() formPai: any;

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();

	servicoGrupoBeneficio = "Beneficio";

	@ViewChild('appModalAmparoLegalBeneficio') appModalAmparoLegalBeneficio;
	@ViewChild('appModalJustificativa') modalJustificativa: ModalJustificativaStatusGrupoBeneficioComponent;
	@ViewChild('appModalBeneficioHistorico') appModalHistorico: ModalHistoricoBeneficioComponent;
	@ViewChild('appModalEmpresaPDI') appModalEmpresaPDI: ModalEmpresaPDIComponent;

	public VariavelMokada: any = 123;
	public valores: any;
	public check = [];
	public marcado: boolean;

	checkAll1: any;
	@ViewChild("checkedpli") checkedpli;
	@ViewChild("optionchecked") optionchecked;
	listaFiltrada: Array<manterPliVM>;

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

	limparCheck() {

		if (this.optionchecked != undefined) {

			this.check = [false];
			this.check = [true];
			this.check = [false];
			this.check = [false];
		}
	}

	confirmarAtivarStatus(item) {
		item.isEditStatus = 1;
		item.ativar = 1;
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

	abrirJustificativa(item:TaxaGrupoBeneficioVM){
		this.modalJustificativa.abrir(item, this.formPai);
	}

	abrirHistorico(item){
		this.appModalHistorico.abrir(item);
	}

	ativarStatus(item) {
		if (item.statusBeneficio == 0)
		{
			item.statusBeneficio = 1;
		} else
		{
			item.statusBeneficio = 0;
		}

		this.applicationService.put<TaxaGrupoBeneficioVM>(this.servicoGrupoBeneficio, item).subscribe(result => {

			if (result){
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "/grupo-beneficio");
				this.model = result;
				this.changePage(this.page);
			}
		});
	}

	removeCheckAll() {
		this.checkedpli.nativeElement.checked = false;
	}

	onChangeCheckAllGridPLI() {

			for (var i = 0; i < this.lista.length; i++) {

				//Se o checkAll estiver marcado...
				if (this.checkedpli.nativeElement.checked == true) {
					//na lista quando o status for reprovado
					if (this.lista[i].statusPliProcessamento == 3) {
						this.lista[i].checkbox = true;
						this.checkAll1 = true;
					}

				} else {
					this.lista[i].checkbox = false;
					this.checkAll1 = false;
				}
			}
	}

	deletar(id) {
		this.applicationService.delete(this.servicoNCM, id).subscribe(result => {

			this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", 'Informação').subscribe(isConfirmado => {

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

	AbrirModal(descricaoAmparoLegal, percentual, descricao, tipobeneficio, codigo) {
		this.appModalAmparoLegalBeneficio.abrir(descricaoAmparoLegal, percentual, descricao, tipobeneficio, codigo);
	}

	AbrirModalPDI(){
		this.appModalEmpresaPDI.abrir();
	}

}
