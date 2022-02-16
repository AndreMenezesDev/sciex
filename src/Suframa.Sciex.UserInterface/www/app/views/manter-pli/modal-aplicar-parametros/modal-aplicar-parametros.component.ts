import { Component, ViewChild  } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../shared/guards/auth-guard.service';
import { manterPliMercadoriaVM } from '../../../view-model/ManterPliMercadoriaVM';

@Component({
	selector: 'app-modal-aplicar-parametros',
	templateUrl: './modal-aplicar-parametros.component.html',
})

export class ModalAplicarParametrosComponent {
	isDisplay: boolean = false;
	servicoPliMercadoria = 'PliMercadoria';
	idPliMercadoria: number;
	idParametro: number;
	model: manterPliMercadoriaVM = new manterPliMercadoriaVM();
	CNPJ: string;
	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private applicationService: ApplicationService,
		private router: Router,
		private authguard: AuthGuard,

	) { }

	@ViewChild('appModalAplicarParametros') appModalAplicarParametros;
	@ViewChild('appModalAplicarParametrosBackground') appModalAplicarParametrosBackground;
	//@ViewChild('formularioA') formulario;
	@ViewChild('aplicarParametro') aplicarParametro;

	public abrir(codigoPliMercadoria, codigoPliProduto, codigoPli) {

		this.applicationService.get("UsuarioLogado", { cnpj: true }).subscribe((result: any) => {

			if (result != null)
				this.CNPJ = result;

		});

		this.idPliMercadoria = this.model.idPliMercadoria = codigoPliMercadoria;
		this.model.idPliProduto = codigoPliProduto;
		this.model.idPLI = codigoPli;

		this.appModalAplicarParametros.nativeElement.style.display = 'block';
		this.appModalAplicarParametrosBackground.nativeElement.style.display = 'block';
	}

	validarParametro() {
		this.aplicarParametro.valorInput.nativeElement.setCustomValidity('');

		if (this.aplicarParametro.valorInput.nativeElement.value == "" || this.aplicarParametro.valorInput.nativeElement.value.length == 0) {
			this.aplicarParametro.valorInput.nativeElement.setCustomValidity('Preencha este campo.');			
		}
	}

	isSetorTrue() {
		this.isDisplay = true;
	}

	isSetorFalse() {
		this.isDisplay = false;
	}

	public fechar() {
		this.modal.confirmacao("Os dados serão descartados. Deseja continuar?", '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.limpar();
					this.appModalAplicarParametros.nativeElement.style.display = 'none';
					this.appModalAplicarParametrosBackground.nativeElement.style.display = 'none';
				}
			});
	}

	limpar() {
		this.aplicarParametro.clearInput = true;
		let element: HTMLElement = document.getElementById('valorInput') as HTMLElement;
		element.click();
		this.aplicarParametro.clearInput = false;	
	}

	public salvar() {		
		this.validarParametro();
		var nome = "formulario" + this.idPliMercadoria.toString();

		if (!this.validationService.form(nome)) { return; }

		this.modal.confirmacao("Os dados da mercadoria serão sobrescritos. Deseja continuar?", '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.aplicarParametros();
					this.limpar();
				}
			});
	}

	aplicarParametros() {
		this.model.aplicarParametros = true;
		this.model.confirmacaoClienteParametro = false;
		this.idParametro = this.model.idParametro;

		this.applicationService.put<manterPliMercadoriaVM>(this.servicoPliMercadoria, this.model).subscribe(result => {
			if (result.parametroAplicado) {
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "");
				this.appModalAplicarParametros.nativeElement.style.display = 'none';
				this.appModalAplicarParametrosBackground.nativeElement.style.display = 'none';
				this.limpar();
			}
			else {				
				this.modal.confirmacao("Fornecedor já existente para a mesma mercadoria. Deseja aplicar os demais dados, exceto fornecedor ?", '', '')
					.subscribe(isConfirmado => {
						if (isConfirmado) {
							console.log(this.idParametro);
							this.model.idParametro = this.idParametro;
							this.model.confirmacaoClienteParametro = true;
							this.applicationService.put<manterPliMercadoriaVM>(this.servicoPliMercadoria, this.model).subscribe(result => {
								if (result.parametroAplicado) {
									this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "");
									this.appModalAplicarParametros.nativeElement.style.display = 'none';
									this.appModalAplicarParametrosBackground.nativeElement.style.display = 'none';
									this.limpar();
								} else {
									this.modal.alerta(result.mensagemErro, "Informação", "");
									this.appModalAplicarParametros.nativeElement.style.display = 'none';
									this.appModalAplicarParametrosBackground.nativeElement.style.display = 'none';
									this.limpar();
								}
							});
						}
					});
			}
		});
	}

	
}
