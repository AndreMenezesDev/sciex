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
	selector: 'app-modal-editar-documento-due-correcao',
	templateUrl: './modal-editar-documento-due-correcao.component.html',
})

export class ModalEditarDocumentoDueCorrecaoComponent implements OnInit {
	model: any;
	formPai: any;
	parametros: any = {};
	servicoCorrigirDue = "CorrigirDUE";
	somenteLeitura = true;


	@ViewChild('appModalEditarDocumentoDueCorrecao') appModalEditarDocumentoDueCorrecao;
	@ViewChild('appModalEditarDocumentoDueCorrecaoBackground') appModalEditarDocumentoDueCorrecaoBackground;
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
		this.appModalEditarDocumentoDueCorrecao.nativeElement.style.display = 'block';
		this.appModalEditarDocumentoDueCorrecaoBackground.nativeElement.style.display = 'block';
		this.editarDocumentosDUE(item);
	}

	EditarItemLista(){
		if(this.parametros.codigoPais == null || this.parametros.codigoPais == ""){
			this.modal.alerta("Preencha o campo 'País'", "Informação");
			return;
		}else if (this.parametros.numero == null || this.parametros.numero == ""){
			this.modal.alerta("Preencha o campo 'DU-E'", "Informação");
			return;
		}
		else if (this.parametros.quantidade == null || this.parametros.quantidade == ""){
			this.modal.alerta("Preencha o campo 'Quantidade Total'", "Informação");
			return;
		}
		else if (this.parametros.valorDolar == null || this.parametros.valorDolar == ""){
			this.modal.alerta("Preencha o campo 'Valor Total (US$)''", "Informação");
			return;
		}
		else if (this.parametros.quantidade <= 0){
			this.modal.alerta("Informe uma quantidade válida.", "Informação");
			return;
		}
		else if (this.parametros.valorDolar <= 0){
			this.modal.alerta("Informe um valor válido.", "Informação");
			return;
		}
		else if (this.parametros.numero <= 0){
			this.modal.alerta("informe um numero DUE válido.", "Informação");
			return;
		}
		this.parametros.quantidade = Number(this.parametros.quantidade);
		this.parametros.valorDolar = Number(this.parametros.valorDolar);

		if(this.parametros.idDue != null){
			this.applicationService.put(this.servicoCorrigirDue, this.parametros).subscribe((result:any) =>{
				if (result.sucesso){
					this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, 'Sucesso', '');
					this.parametros.idDue = null;
					this.limpar();
					this.formPai.selecionarProduto();
					this.fechar();
				}else{
					this.modal.alerta(result.retornoString, 'Atenção', '');
					console.log(result.retornoString);

				}
			});
		}

	}
	editarDocumentosDUE(item){
		this.parametros.idPEProdutoPais = item.idPEProdutoPais;
		this.parametros.codigoPais = item.codigoPais;
		this.parametros.idDue = item.idDue;
		this.parametros.numero = item.numero;
		this.parametros.dataAverbacao = (item.dataAverbacao as string).substring(0,10);
		this.parametros.quantidade = item.quantidade;
		this.parametros.valorDolar = item.valorDolar;
		this.parametros.idPEProduto = this.model.idPEProduto;
	}

	limpar() {
		this.parametros.codigoPais = null;
		this.parametros.numero = null;
		this.parametros.quantidade = null;
		this.parametros.valorDolar = null;
		this.parametros.dataAverbacao = null;
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
		this.appModalEditarDocumentoDueCorrecao.nativeElement.style.display = 'none';
		this.appModalEditarDocumentoDueCorrecaoBackground.nativeElement.style.display = 'none';
    }
}
