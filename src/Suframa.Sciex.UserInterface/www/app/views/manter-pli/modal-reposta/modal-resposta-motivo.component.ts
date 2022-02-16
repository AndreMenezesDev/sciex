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
import { ManterPliComponent } from '../manter-pli.component';


@Component({
	selector: 'app-modal-resposta-motivo',
	templateUrl: './modal-resposta-motivo.component.html',
})

export class ModalRespostaMotivoComponent implements OnInit {
	model: any;
	servico = 'PliAnaliseVisualResposta';

	@ViewChild('appModalRespostaMotivo') appModalRespostaMotivo;
	@ViewChild('appModalRespostaMotivoBackground') appModalRespostaMotivoBackground;
	@ViewChild('arquivo') arquivo;
	@ViewChild('resposta') resposta;

	@ViewChild('formResp') formResp;

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
		private ManterPliComponent: ManterPliComponent,
	) {

	}

	ngOnInit() {
		this.model = {};
	}

	public abrir(item) {
		this.model = item;
		this.appModalRespostaMotivo.nativeElement.style.display = 'block';
		this.appModalRespostaMotivoBackground.nativeElement.style.display = 'block';

	}

	salvar(){
		if (this.model.descricaoResposta == null || this.model.descricaoResposta== ""){
			this.modal.alerta(this.msg.CAMPO_$_OBRIGATORIO_NAO_INFORMADO.replace('$','Resposta'),'Atenção')
			.subscribe(()=>{
				this.resposta.nativeElement.focus(); 
			});
			return false;

		}

		// if (this.model.nomeAnexo == null && this.model.anexo == null){
		// 	this.modal.alerta(this.msg.CAMPO_OBRIGATORIO_NAO_INFORMADO.replace('$','Anexo'),'Atenção')
		// 	.subscribe(()=>{
		// 		this.arquivo.nativeElement.focus(); 
		// 	});
		// 	return false;

		// }

		this.applicationService.put<manterPliVM>(this.servico, this.model).subscribe((result:any) => {
			if (result != "" && result != null && result != undefined && result.mensagem == "Salvo com Sucesso!") {
				this.modal.resposta("Arquivo enviado com SUCESSO!", "", "");
				this.appModalRespostaMotivo.nativeElement.style.display = 'none';
				this.appModalRespostaMotivoBackground.nativeElement.style.display = 'none';
				this.ManterPliComponent.listar();
			} else if (result != "" && result != null && result != undefined && result.mensagem != null && result.mensagem == "Tipo de compactação não suportada, não é possível realizar envio do arquivo. É necessário enviar arquivo com compactação ZIP") {
					this.modal.alerta(result.mensagem, "", "");
					this.limparAnexoPdf();
			}
			else if (result != "" && result != null && result != undefined && result.mensagem != null && result.mensagem != "") {
				this.modal.alerta(result.mensagem, "", "");
				this.limparAnexoPdf();
				this.model.descricaoResposta = null;
				this.appModalRespostaMotivo.nativeElement.style.display = 'none';
				this.appModalRespostaMotivoBackground.nativeElement.style.display = 'none';
				this.ManterPliComponent.listar();
			}
		});
	}

	onFileChange(event) {

		let reader = new FileReader();

		if (event.target.files && event.target.files.length > 0) {
			//alert("temArquivo");
			let file = event.target.files[0];

			reader.readAsDataURL(file);
			this.filetype = file.type;
			this.filesize = file.size;


			if(this.types.includes(this.filetype)) {
				if(this.filesize < this.limiteArquivo){
					this.temArquivo = true;

					reader.onload = () => {
						this.model.nomeAnexo = file.name;
						this.model.anexo = (reader.result as string).split(',')[1];
					};
				}else{
				this.modal.alerta(this.msg.ANEXO_ACIMA_DO_LIMITE.replace('($)','10'),'Atenção');
				this.limparAnexoPdf();
				}
			} else {
				this.modal.alerta(this.msg.FAVOR_SELECIONAR_UM_ARQUIVO_NO_FORMATO_ZIP,'Atenção');
				this.limparAnexoPdf();
			}

		}else{
			this.temArquivo = false;
			this.model.nomeAnexo = null;
			this.model.anexo = null;
		}

	}

	limparAnexoPdf() {
		this.temArquivo = false;
		if (this.arquivo != undefined)
			this.arquivo.nativeElement.value = '';

		this.model.anexo = null;
		this.model.nomeAnexo = null;
	}

	public fechar() {
		this.modal.confirmacao("Os dados serão descartados. Deseja continuar?", '', '')
			.subscribe(
				isConfirmado => {
					if (isConfirmado) {
						this.limparAnexoPdf();
						this.model.descricaoResposta = null;
						this.appModalRespostaMotivo.nativeElement.style.display = 'none';
						this.appModalRespostaMotivoBackground.nativeElement.style.display = 'none';
					}
				}
			);
    }
}
