import { Component, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { manterControleImportacaoVM } from '../../../view-model/ManterControleImportacaoVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { isNullOrUndefined } from 'util';

@Component({
	selector: 'app-manter-controle-importacao-formulario',
	templateUrl: './formulario.component.html'
})

export class ManterControleImportacaoFormularioComponent implements OnInit {
	path: string;
	titulo: string;
	tituloPanel: string;
	desabilitado: boolean;
	servico = 'ControleImportacao';
	servicoGrid = 'ControleImportacaoGrid';
	habilitarCampoCodigo: boolean;
	isCampoStatus: boolean;
	validar: boolean = false;
	model: manterControleImportacaoVM = new manterControleImportacaoVM();
	codigo1: string;
	status: boolean;
	isCancelarVisible: boolean;

	@ViewChild('formulario') formulario;
	@ViewChild('field2') field2;

	constructor(
		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private arrayService: ArrayService,
		private modal: ModalService,
		private validationService: ValidationService,
		private router: Router
	) {
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.verificarRota();
	}

	ngOnInit() {
		this.model.status = 1;
	}

	public verificarRota() {
		this.habilitarCampoCodigo = false;
		this.tituloPanel = 'Formulário';
		this.isCancelarVisible = true;
		this.isCampoStatus = true;

	}

	public onBlurEvent() {
		
	}

	public desabilitarTela() {
		this.desabilitado = true;
	}

	getSelectedOptionText(event: Event) {
		let selectedOptions = event.target['options'];
		let selectedIndex = selectedOptions.selectedIndex;
		let selectElementText = selectedOptions[selectedIndex].text;
		this.model.descricaoSetor = selectElementText;
	}

	public salvar() {

		if (!this.validationService.form('formulario')) { return; }
		if (!this.formulario.valid) { return; }

		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.salvarRegistro();
				}
			});
	}

	private salvarRegistro() {
		
		this.applicationService.put<manterControleImportacaoVM>(this.servico, this.model).subscribe(result => {
			if (result.mensagemErro != null)
				this.modal.alerta(this.msg.REGISTRO_JA_CADASTRADO, "Alerta", "/manter-controle-importacao");

			else {
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "/manter-controle-importacao");
				this.model = result;
			}
			if (this.path != "editar")
				localStorage.clear();
		});

	}

	public cancelarOperacao() {
		this.modal.confirmacao(this.msg.CONFIRMAR_CONFIRMACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.router.navigate(['/manter-controle-importacao']);
				}
			});
	}
}
