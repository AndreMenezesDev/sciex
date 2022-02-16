import { Component, ViewChild, OnInit} from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../shared/guards/auth-guard.service';
import { ToastrService } from 'toastr-ng2/toastr';
import { AuthenticationService } from '../../../shared/services/authentication.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';

@Component({
	selector: 'app-modal-alterar-pais',
	templateUrl: './modal-alterar-pais.component.html',
})

export class ModalAlterarPaisComponent implements OnInit {
	model: any;
	formPai: any;
	servico = 'PEPais';

	@ViewChild('appModalAlterarPais') appModalAlterarPais;
	@ViewChild('appModalAlterarPaisBackground') appModalAlterarPaisBackground;
	@ViewChild('valorUS') valorUS;
	@ViewChild('quantidade') quantidade;

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private applicationService: ApplicationService,
		private toastrService: ToastrService,
		private router: Router,
		private authguard: AuthGuard,
		private authenticationService: AuthenticationService,
	) {

	}

	ngOnInit() {
		this.model = {};
	}

	public abrir(formPai, item) {
		this.formPai = formPai
		this.model = item;
		this.model.quantidade = parseFloat(item.quantidade).toFixed(5);
		this.model.valorDolar = parseFloat(item.valorDolar).toFixed(2);
		this.appModalAlterarPais.nativeElement.style.display = 'block';
		this.appModalAlterarPaisBackground.nativeElement.style.display = 'block';
	}

	confirmaSalvar(){
		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.salvar();
				}
			});
	}
	salvar(){
		if (this.model.quantidade == null || this.model.quantidade == ""){
			this.modal.alerta(this.msg.CAMPO_$_OBRIGATORIO_NAO_INFORMADO.replace('$','"Quantidade Total"'),'Atenção');
			return;
		}
		else if (this.model.valorDolar == null || this.model.valorDolar == ""){
			this.modal.alerta(this.msg.CAMPO_$_OBRIGATORIO_NAO_INFORMADO.replace('$','"Valor Total (US$)"'),'Atenção');
			return;	
		}

		this.model.quantidade = Number(this.model.quantidade);
		this.model.valorDolar = Number(this.model.valorDolar);

		this.applicationService.put(this.servico, this.model).subscribe((result:any) => {
			if (result != "" && result != null && result != undefined && result.mensagem == null || result.mensagem == "") {
				this.formPai.selecionarProduto(result.idPEProduto);
				this.modal.resposta("Operação realizada com sucesso!", "", "");
				this.appModalAlterarPais.nativeElement.style.display = 'none';
				this.appModalAlterarPaisBackground.nativeElement.style.display = 'none';
				this.model = {};
			}
			else if (result != "" && result != null && result != undefined && result.mensagem != null && result.mensagem == null || result.mensagem != "") {
				this.modal.alerta(result.mensagem, "", "");
				this.appModalAlterarPais.nativeElement.style.display = 'none';
				this.appModalAlterarPaisBackground.nativeElement.style.display = 'none';
				this.model = {};
			}
		});
	}

	onBlurQtTotal(event){
		if(event.target.value !== '')
		 this.model.quantidade = parseFloat(this.replaceVirgula(event.target.value)).toFixed(5)
	}

	onBlurVlrTotal(event){
		if(event.target.value !== '')
		 this.model.valorDolar = parseFloat(this.replaceVirgula(event.target.value)).toFixed(2)
	}

	replaceVirgula(value): string{
		return value.replace(',','.');
	}

	public fechar() {

		this.modal.confirmacao(this.msg.CONFIRMAR_CONFIRMACAO, '', '')
			.subscribe(
				isConfirmado => {
					if (isConfirmado) {
						this.formPai.selecionarProduto(this.model.idPEProduto);
						this.model = {};
						this.appModalAlterarPais.nativeElement.style.display = 'none';
						this.appModalAlterarPaisBackground.nativeElement.style.display = 'none';
					}
				}
			);
    }
}
