import { Component, ViewChild, Injectable } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../shared/guards/auth-guard.service';
import { manterNcmVM } from '../../../view-model/ManterNcmVM';
import { MessagesService } from '../../../shared/services/messages.service';
import { ManterNcmComponent } from '../manter-ncm.component';


@Component({
	selector: 'app-modal-justificativa-status',
	templateUrl: './modal-justificativa-status.component.html',
})

@Injectable()
export class ModalJustificativaStatusComponent {
	isDisplay: boolean = false;
	cpfDigitado: string = '';
    opcaoSelecionada: any;
	servicoPli = 'Pli';
	parametros: any = {};
	model: manterNcmVM = new manterNcmVM();
	tipoPli: string;
	servico = 'Ncm';
	formPai: ManterNcmComponent;

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private applicationService: ApplicationService,
		private router: Router,
		private authguard : AuthGuard,
		private msg: MessagesService,
	) { }

	@ViewChild('appModalJustificativaStatus') appModalJustificativaStatus;
	@ViewChild('appModalJustificativaStatusBackground') appModalJustificativaStatusBackground;
	@ViewChild('formularioB') formulario;
	@ViewChild('cpf') cpf;

	public abrir(item: manterNcmVM, formPai: ManterNcmComponent) {
		this.formPai = formPai;
		this.model = item;
		this.tipoPli = "NORMAL";
		this.appModalJustificativaStatus.nativeElement.style.display = 'block';
		this.appModalJustificativaStatusBackground.nativeElement.style.display = 'block';
	}

	public fechar() {
		this.modal.confirmacao("Os dados serão descartados. Deseja continuar?", '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.model.justificativa = undefined;
					this.appModalJustificativaStatus.nativeElement.style.display = 'none';
					this.appModalJustificativaStatusBackground.nativeElement.style.display = 'none';
				}
			});
    }

	salvar() {

		if (this.model.justificativa == undefined || this.model.justificativa == ''){
			this.modal.alerta("Campo justificativa é obrigatório","Atenção")
			return;
		}

		if (this.model.status == 0) {
			this.model.status = 1;
		} else {
			this.model.status = 0;
		}
		this.model.acaoTela = 2;
		this.applicationService.put(this.servico, this.model).subscribe((result:any={}) => {
			if (result.mensagemErro != null)
				this.modal.alerta(this.msg.REGISTRO_JA_CADASTRADO, "Alerta");
			else {
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso",'');
				this.model.justificativa = undefined;
				this.formPai.buscar(true,false);
				this.appModalJustificativaStatus.nativeElement.style.display = 'none';
				this.appModalJustificativaStatusBackground.nativeElement.style.display = 'none';
			}

		});

	}



}
