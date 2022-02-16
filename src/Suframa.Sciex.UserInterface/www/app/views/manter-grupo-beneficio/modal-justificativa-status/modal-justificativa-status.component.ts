import { Component, ViewChild, Injectable } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../shared/guards/auth-guard.service';
import { manterNcmVM } from '../../../view-model/ManterNcmVM';
import { MessagesService } from '../../../shared/services/messages.service';
import { TaxaGrupoBeneficioVM } from '../../../view-model/TaxaGrupoBeneficioVM';
import { ManterGrupoBeneficioComponent } from '../manter-grupo-beneficio.component';

@Component({
	selector: 'app-modal-justificativa-status-grupo-beneficio',
	templateUrl: './modal-justificativa-status.component.html',
})

@Injectable()
export class ModalJustificativaStatusGrupoBeneficioComponent {
	isDisplay: boolean = false;
	cpfDigitado: string = '';
    opcaoSelecionada: any;
	servicoPli = 'Pli';
	parametros: any = {};
	model: TaxaGrupoBeneficioVM = new TaxaGrupoBeneficioVM();
	tipoPli: string;
	servico = 'Beneficio';
	formPai: ManterGrupoBeneficioComponent;

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private applicationService: ApplicationService,
		private router: Router,
		private authguard : AuthGuard,
		private msg: MessagesService,
	) { }

	@ViewChild('appModalJustificativaStatusBeneficio') appModalJustificativaStatusBeneficio;
	@ViewChild('appModalJustificativaStatusBeneficioBackground') appModalJustificativaStatusBeneficioBackground;
	@ViewChild('formularioB') formulario;
	@ViewChild('cpf') cpf;

	public abrir(item: TaxaGrupoBeneficioVM, formPai: ManterGrupoBeneficioComponent) {
		this.model = item;
		this.formPai = formPai;
		this.appModalJustificativaStatusBeneficio.nativeElement.style.display = 'block';
		this.appModalJustificativaStatusBeneficioBackground.nativeElement.style.display = 'block';
	}

	public fechar() {
		this.modal.confirmacao("Os dados serão descartados. Deseja continuar?", '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.model.justificativa = undefined;
					this.appModalJustificativaStatusBeneficio.nativeElement.style.display = 'none';
					this.appModalJustificativaStatusBeneficioBackground.nativeElement.style.display = 'none';
				}
			});
    }

	salvar() {

		if (this.model.justificativa == undefined || this.model.justificativa == ''){
			this.modal.alerta("Campo justificativa é obrigatório","Atenção")
			return;
		}

		if (this.model.statusBeneficio == 0) {
			this.model.statusBeneficio = 1;
		} else {
			this.model.statusBeneficio = 0;
		}
		//his.model.acaoTela = 2;
		this.applicationService.put(this.servico, this.model).subscribe((result:any={}) => {
			if (result.mensagemErro != null)
				this.modal.alerta(this.msg.REGISTRO_JA_CADASTRADO, "Alerta");
			else {
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso",'').subscribe(()=>{

					this.model.justificativa = undefined;
					this.formPai.listar();
					this.appModalJustificativaStatusBeneficio.nativeElement.style.display = 'none';
					this.appModalJustificativaStatusBeneficioBackground.nativeElement.style.display = 'none';
				});
			}

		});

	}



}
