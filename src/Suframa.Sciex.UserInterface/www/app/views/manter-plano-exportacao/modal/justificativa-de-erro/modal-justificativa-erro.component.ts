import { Component, ViewChild, Injectable } from '@angular/core';
import { ValidationService } from '../../../../shared/services/validation.service';
import { ModalService } from '../../../../shared/services/modal.service';
import { ApplicationService } from '../../../../shared/services/application.service';
import { manterPliVM } from '../../../../view-model/ManterPliVM';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../../shared/guards/auth-guard.service';
import { manterNcmVM } from '../../../../view-model/ManterNcmVM';
import { MessagesService } from '../../../../shared/services/messages.service';

@Component({
	selector: 'app-modal-justificativa-erro',
	templateUrl: './modal-justificativa-erro.component.html',
})

@Injectable()
export class ModalJustificativaErroComponent {
	isDisplay: boolean = false;
	cpfDigitado: string = '';
	opcaoSelecionada: any;
	servicoPli = 'Pli';
	parametros: any = {};
	model: manterNcmVM = new manterNcmVM();
	tipoPli: string;
	servico = 'Ncm';
	formPai: any;

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private applicationService: ApplicationService,
		private router: Router,
		private authguard: AuthGuard,
		private msg: MessagesService,
	) { }

	@ViewChild('appModalJustificativaErro') appModalJustificativaErro;
	@ViewChild('appModalJustificativaErroBackground') appModalJustificativaErroBackground;
	@ViewChild('formularioB') formulario;
	@ViewChild('cpf') cpf;

	public abrir(itemInsumo) {
		this.model = itemInsumo;
		this.appModalJustificativaErro.nativeElement.style.display = 'block';
		this.appModalJustificativaErroBackground.nativeElement.style.display = 'block';
	}

	public fechar() {
		this.model.justificativa = undefined;
		this.appModalJustificativaErro.nativeElement.style.display = 'none';
		this.appModalJustificativaErroBackground.nativeElement.style.display = 'none';
	}

	salvar() {

		if (this.model.justificativa == undefined || this.model.justificativa == '') {
			this.modal.alerta("Campo justificativa é obrigatório", "Atenção")
			return;
		}

		if (this.model.status == 0) {
			this.model.status = 1;
		} else {
			this.model.status = 0;
		}
		this.model.acaoTela = 2;
		this.applicationService.put(this.servico, this.model).subscribe((result: any = {}) => {
			if (result.mensagemErro != null)
				this.modal.alerta(this.msg.REGISTRO_JA_CADASTRADO, "Alerta");
			else {
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", '');
				this.model.justificativa = undefined;
				this.formPai.buscar(true, false);
				this.appModalJustificativaErro.nativeElement.style.display = 'none';
				this.appModalJustificativaErroBackground.nativeElement.style.display = 'none';
			}

		});

	}



}
