import { Component, ViewChild } from '@angular/core';
import { parametroAnalistaVM } from '../../../view-model/ParametroAnalistaVM';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { MonthPipe } from '../../../shared/pipes/month.pipe';

@Component({
	selector: 'app-modal-bloquear-analista',
	templateUrl: './modal-bloquear-analista.component.html',
})

export class ModalBloquearAnalistaComponent {
	model: parametroAnalistaVM = new parametroAnalistaVM();

	servicoParametroAnalista = 'ParametroAnalista1';
	analistaVisual: boolean;
	horaInicio: Date;
	horaFim: Date;
	tipoSistema: number; // 1: Analise Visual Bloquear // 2: Analise Listagem Bloquear // 3: Analise Visual Ativar // 4: Analise Listagem Ativar
	isRequiredBloquear: boolean;

    descricaoHora_Bloqueio_Atendimento: string;

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private applicationService: ApplicationService,

	) { }

	@ViewChild('appModalBloquearAnalista') appModalBloquearAnalista;
	@ViewChild('appModalBloquearAnalistaBackground') appModalBloquearAnalistaBackground;
	@ViewChild('formulario') formulario;
	@ViewChild('validarHoraFim') validarHoraFim;

	public abrir(model: parametroAnalistaVM, _tipoSistema) {
		this.limparCampos();

		this.appModalBloquearAnalista.nativeElement.style.display = 'block';
		this.appModalBloquearAnalistaBackground.nativeElement.style.display = 'block';

		this.isRequiredBloquear = true;
        this.model = model;
        this.tipoSistema = _tipoSistema;

        if (_tipoSistema == 2 || _tipoSistema == 1) 
            this.descricaoHora_Bloqueio_Atendimento = 'Hora do Bloqueio:';
        else if (_tipoSistema == 4 || _tipoSistema == 3)
            this.descricaoHora_Bloqueio_Atendimento = 'Hora de Atendimento:';
	}

	public fechar() {
		this.isRequiredBloquear = false;
		this.appModalBloquearAnalista.nativeElement.style.display = 'none';
		this.appModalBloquearAnalistaBackground.nativeElement.style.display = 'none';
	}

	public salvar() {
		this.validarHoras();
		if (!this.validationService.form('formulario')) { return; }
		if (!this.formulario.valid) { return; }

		this.fechar();


		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '/parametrizarAnalista')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.salvarRegistro();
				}
			});
	}

	private salvarRegistro() { // 1: Analise Visual Bloquear // 2: Analise Listagem Bloquear // 3: Analise Visual Ativar // 4: Analise Listagem Ativar
		this.model.tipoSistema = this.tipoSistema;
		if (this.tipoSistema == 1) {
			this.model.dataAnaliseVisualBloqueioInicio = new Date();
			this.model.horaAnaliseVisualBloqueioInicio = this.horaInicio;
			this.model.horaAnaliseVisualBloqueioFim = this.horaFim;
			this.model.statusAnaliseVisualBloqueio = 1; //inativo
			this.model.descricaoAnaliseVisualBloqueioFim = this.retornaCodigoStatus(this.model.listaStatusAnaliseVisual);
		}
		else if (this.tipoSistema == 2) {
			this.model.dataAnaliseListagemLoteBloqueioInicio = new Date();
			this.model.horaAnaliseLoteListagemBloqueioInicio = this.horaInicio;
			this.model.horaAnaliseLoteListagemBloqueioFim = this.horaFim;
			this.model.statusAnaliseLoteListagemBloqueio = 1; //inativo
			this.model.descricaoAnaliseLoteListagemBloqueioFim = this.retornaCodigoStatus(this.model.listaStatusAnaliseListagem);
		}
		else if (this.tipoSistema == 3) {
			this.model.dataAnaliseVisualInicio = new Date();
			//this.model.dataAnaliseVisualBloqueioInicio = this.model.dataAnaliseVisualBloqueioInicio;
			//console.log(this.model.dataAnaliseVisualBloqueioInicio);
			this.model.horaAnaliseVisualInicio = this.horaInicio;
			this.model.horaAnaliseVisualFim = this.horaFim;
			this.model.statusAnaliseVisual = 1; //ativo
		}
		else if (this.tipoSistema == 4) {
			this.model.dataAnaliseLoteListagemInicio = new Date();
			this.model.horaAnaliseLoteListagemInicio = this.horaInicio;
			this.model.horaAnaliseLoteListagemFim = this.horaFim;
			this.model.statusAnaliseLoteListagem = 1; //ativo
		}

		this.limparCampos();

		this.applicationService.put<parametroAnalistaVM>(this.servicoParametroAnalista, this.model).subscribe(result => {
			this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "/parametrizarAnalista");
			this.model = result;
		});
	}

	private retornaCodigoStatus(lista) {
		var codigos = '';
		for (var i = 0; i < lista.length; i++) {
			if (lista[i].checked) {
				codigos += codigos + lista[i].idStatusPLI == "" ? '-1' : lista[i].idStatusPLI + ',';
			}
		}
		return codigos;
	}

	private limparCampos() {
		this.horaInicio = this.horaFim = new Date();
	}

	public validarHoras() {
		this.validarHoraFim.nativeElement.setCustomValidity('');
		if (this.horaFim < this.horaInicio) {
			this.validarHoraFim.nativeElement.setCustomValidity('Campo inválido.');
		}
	}

	public atualizaOpcaoCheckedVisual(option, event) {
		this.model.listaStatusAnaliseVisual[option] = event.target.checked;
	}

	public atualizaOpcaoCheckedListagem(option, event) {
		this.model.listaStatusAnaliseListagem[option] = event.target.checked;
	}
}
