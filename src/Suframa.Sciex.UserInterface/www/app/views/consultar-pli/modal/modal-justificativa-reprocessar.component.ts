import { Component, ViewChild, Output, EventEmitter } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { MessagesService } from '../../../shared/services/messages.service';
import { Router } from '@angular/router';
import { forEach } from '@angular/router/src/utils/collection';
import { ManterConsultarPliComponent } from '../manter-consultar-pli.component';
import { ManterConsultarPliGridComponent } from '../grid/grid.component';


@Component({
	selector: 'app-modal-justificativa-reprocessar',
	templateUrl: './modal-justificativa-reprocessar.component.html',
})

export class ModalJustificativaReprocessarComponent {
	isDisplay: boolean = false;
	servicoConsultarPli = 'ConsultarPli';
	parametros: any = {};
	listaPli: Array<number>;
	
	model: manterPliVM = new manterPliVM();

	@ViewChild('formularioB') formularioB;
	@ViewChild('appModalJustificativaReprocessarBackground') appModalJustificativaReprocessarBackground;
	@ViewChild('appModalJustificativaReprocessar') appModalJustificativaReprocessar;
	@ViewChild('justificativa') justificativa;
	formPai: any;


	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private router: Router,
		private manterConsultarPliComponent: ManterConsultarPliComponent,
		private ManterConsultarPliGridComponent: ManterConsultarPliGridComponent
	) { }



	public abrir(formPai, pli: Array<number>) {

		this.model.listaPli = pli;
		this.formPai = formPai;
		this.appModalJustificativaReprocessarBackground.nativeElement.style.display = 'block';
		this.appModalJustificativaReprocessar.nativeElement.style.display = 'block';
	}

	isSetorTrue() {
		this.isDisplay = true;
	}

	public fechar() {

		this.modal.confirmacao("Os dados serão descartados. Deseja continuar?", '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.model = new manterPliVM();
					this.justificativa.nativeElement.value = '';
					this.appModalJustificativaReprocessarBackground.nativeElement.style.display = 'none';
					this.appModalJustificativaReprocessar.nativeElement.style.display = 'none';
					this.manterConsultarPliComponent.listar();
				}
			});
	}


	public salvar() {

		if (!this.validationService.form('formularioB')) { return; }
		if (!this.formularioB.valid) { return; }

		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.parametros.observacao = this.parametros.descricaoHistoricoPli;
					this.model.observacao = this.parametros.observacao;

					this.applicationService.put(this.servicoConsultarPli, this.model).subscribe(result => {
						this.parametros = result;

						if (this.parametros.mensagemErro != null) {

							this.modal.alerta(this.parametros.mensagemErro, "Informação");
							this.appModalJustificativaReprocessarBackground.nativeElement.style.display = 'none';
							this.appModalJustificativaReprocessar.nativeElement.style.display = 'none';
							this.manterConsultarPliComponent.buscar(true);
						}
						else {
							this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "/consultar-pli");
							this.parametros = result;

							this.appModalJustificativaReprocessarBackground.nativeElement.style.display = 'none';
							this.appModalJustificativaReprocessar.nativeElement.style.display = 'none';
							this.manterConsultarPliComponent.buscar(true);
						}
	
					});
				}
			});

	}

}

