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
import { ManterPliFormularioRetificadorComponent } from '../../../views/manter-pli/formulario/formularioRetificador.component';

@Component({
	selector: 'app-modal-adicionar-li',
	templateUrl: './modal-adicionar-li.component.html',
})

export class ModalAddLiComponent implements OnInit {
	model: any;
	adicoes: any;
	formPai: any;
	servico = 'PliAddLi';

	@ViewChild('appModalAddLi') appModalAddLi;
	@ViewChild('appModalAddLiBackground') appModalAddLiBackground;
	@ViewChild('liadicoes') liadicoes;
	@ViewChild('modalManterPliFormularioRetificador') modalManterPliFormularioRetificador;

	limiteArquivo = 20971520; // 20MB
	temArquivo = false;
	filetype: string;
	filesize: number;
	types = ['application/x-zip-compressed'];

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
		let obj = {numeroDI: this.model.numeroDIReferencia, idPliAplicacao: this.model.idPLIAplicacao};
		this.applicationService.get('LiAdicoesDropDown', obj).subscribe(result => {
			this.adicoes = result;
		});
		this.appModalAddLi.nativeElement.style.display = 'block';
		this.appModalAddLiBackground.nativeElement.style.display = 'block';
	}

	salvar(){
		if (this.model.idLiReferencia == null || this.model.idLiReferencia == ""){
			this.modal.alerta(this.msg.CAMPO_$_OBRIGATORIO_NAO_INFORMADO.replace('$','LI - Adições'),'Atenção');
			return false;
		}

		this.applicationService.put<manterPliVM>(this.servico, this.model).subscribe((result:any) => {
			if (result != "" && result != null && result != undefined && result.mensagem == null || result.mensagem == "") {
				this.formPai.selecionar(result.idPLI);
				this.modal.resposta("Operação realizada com sucesso!", "", "");
				this.appModalAddLi.nativeElement.style.display = 'none';
				this.appModalAddLiBackground.nativeElement.style.display = 'none';
				this.model.idLiReferencia = null;
			}
			else if (result != "" && result != null && result != undefined && result.mensagem != null && result.mensagem == null || result.mensagem != "") {
				this.modal.alerta(result.mensagem, "", "");
				this.appModalAddLi.nativeElement.style.display = 'none';
				this.appModalAddLiBackground.nativeElement.style.display = 'none';
				this.model.idLiReferencia = null;
			}
		});
	}

	public fechar() {

		this.modal.confirmacao("Os dados serão descartados. Deseja continuar?", '', '')
			.subscribe(
				isConfirmado => {
					if (isConfirmado) {
						this.model.idLiReferencia = null;
						this.appModalAddLi.nativeElement.style.display = 'none';
						this.appModalAddLiBackground.nativeElement.style.display = 'none';
					}
				}
			);
    }
}
