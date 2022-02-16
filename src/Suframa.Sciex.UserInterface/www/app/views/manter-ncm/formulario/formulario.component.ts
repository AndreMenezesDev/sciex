import { Component, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { manterNcmVM } from '../../../view-model/ManterNcmVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';

enum StatusNcm{
	ATIVO = 1,
	INATIVO = 0
}

enum IsAmazoniaOcidental{
	SIM = 1,
	NAO = 0
}
@Component({
	selector: 'app-manter-ncm-formulario',
	templateUrl: './formulario.component.html'
})

export class ManterNcmFormularioComponent {
	habilitarCampoCodigo: boolean;
	isCancelarVisible: boolean;
	isCampoStatus: boolean;
	titulo: string;
	path: string;
	servico = 'Ncm';
	model = new manterNcmVM();
	parametros: any = {};
	somenteLeitura: boolean;
	ischecked: boolean;

	@ViewChild('formulario') formulario;
	@ViewChild('statusNcm') statusNcm;
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

	public verificarRota() {
		if (this.path == 'cadastrar') {
			this.titulo = 'Cadastrar'
			this.somenteLeitura = false;
			this.model.isEditStatus = 0;
			this.model.acaoTela = 1;
		}

		if (this.path == 'editar') {
			this.selecionar(this.route.snapshot.params['id']);
			this.titulo = 'Alterar';
			this.somenteLeitura = true;

		}
	}

	public selecionar(id: number) {

		if (!id) { return; }
		let that = this;
		this.applicationService.get<manterNcmVM>(this.servico, id).subscribe(result => {
			that.model = result;
			that.parametros.radioCheckAmazoniaOcidental = result.isAmazoniaOcidental;
			that.model.isEditStatus = 1;
			that.model.acaoTela = 2;
			if (result.status == StatusNcm.ATIVO ){
				that.ischecked = true;
			}else{
				that.ischecked = false;
			}
		});
	}

	public salvar() {
		if (this.statusNcm.nativeElement.checked){
			this.model.status = StatusNcm.ATIVO;
		}else{
			this.model.status = StatusNcm.INATIVO;
		}

		if (this.parametros.radioCheckAmazoniaOcidental === undefined){
			this.model.isAmazoniaOcidental = 0;
		}else{
			this.model.isAmazoniaOcidental = Number(this.parametros.radioCheckAmazoniaOcidental);
		}

		if (this.model.codigoNCM === undefined){
			this.modal.alerta("Campo código NCM é obrigatório","Atenção")
			return;
		}

		if (this.model.descricao === undefined){
			this.modal.alerta("Campo descrição é obrigatório","Atenção")
			return;
		}
		if (this.somenteLeitura && (this.model.justificativa === undefined || this.model.justificativa == null)){
			this.modal.alerta("Campo justificativa é obrigatório","Atenção")
			return;
		}

		if (!this.validationService.form('formulario')) { return; }
		if (!this.formulario.valid) { return; }

		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
		.subscribe(isConfirmado => {
			if (isConfirmado) {
				this.salvarRegistro();
			}
		});


	}

	salvarRegistro() {
		this.applicationService.put(this.servico, this.model).subscribe((result:any={}) => {
			if (result.mensagemErro != null)
				this.modal.alerta(this.msg.REGISTRO_JA_CADASTRADO, "Alerta", "/manter-ncm");
			else {
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "/manter-ncm");
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
					this.router.navigate(['/manter-ncm']);
				}
			});
	}

}
