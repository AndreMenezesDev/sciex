import { Component, OnInit, Injectable, ViewChild } from '@angular/core';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router } from '@angular/router';
import { EstruturaPropriaPLIArquivoVM } from '../../view-model/EstruturaPropriaPLIArquivoVM';
import { FileItem } from 'ng2-file-upload';
import { Observable } from 'rxjs';



@Component({
	selector: 'app-estrutura-propria-pe.component',
	templateUrl: './estrutura-propria-pe.component.html'
})


export class EstruturaPropriaPEComponent implements OnInit {
	parametros: any = {};
	servicoViewImportador = "ViewImportador";
	servicoEstruturaPropria = "EstruturaPropriaPEArquivo";
	model: EstruturaPropriaPLIArquivoVM = new EstruturaPropriaPLIArquivoVM();
	maximoTamanhoArquivo: number = 1 * 1024 * 1024;
	filetype: string;
	filesize: number;

	// #region campos Anexo
	@ViewChild('arquivo') arquivo;
	ocultarInputAnexo = false;
	limiteArquivo = 10485760; // 10MB
	temArquivo = false;
	types = ['application/x-zip-compressed','application/pdf'];
	// #endregion

	@ViewChild('inputArquivo') inputArquivo;
	@ViewChild('btnReset') btnReset;
	@ViewChild('appUploadFile') appUploadFile;
	@ViewChild('appModalEstruturaPropriaPE') appModalEstruturaPropriaPE;

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router
	) {
		if (localStorage.getItem(this.router.url) == null && localStorage.length > 0) {
			localStorage.clear();
		}
	}

	ngOnInit(): void {
	}
	
	onFileChange(event) {

		let reader = new FileReader();

		if (event.target.files && event.target.files.length > 0) {
			let file = event.target.files[0];

			reader.readAsDataURL(file);
			this.filetype = file.type;
			this.filesize = file.size;


			if(this.types.includes(this.filetype)) {
				if(file.name.length <= 50){
					if(this.filesize < this.limiteArquivo){
						this.temArquivo = true;
	
						reader.onload = () => {
							this.ocultarInputAnexo = true;
							this.parametros.nomeAnexo = file.name;
							this.parametros.anexo = (reader.result as string).split(',')[1];
						};
					}else{
					this.modal.alerta(this.msg.ANEXO_ACIMA_DO_LIMITE.replace('($)','10'),'Atenção');
					this.limparAnexo();
					}
				} else {
					this.modal.alerta("O nome do arquivo ultrapassou o limite de 50 caracteres",'Atenção');
					this.limparAnexo();
				}
			} else {
				this.modal.alerta(this.msg.FAVOR_SELECIONAR_UM_ARQUIVO_NO_FORMATO_ZIP,'Atenção');
				this.limparAnexo();
			}

		}else{
			this.temArquivo = false;
			this.parametros.nomeAnexo = null;
			this.parametros.anexo = null;
		}

	}


	limparAnexo() {
		this.temArquivo = false;
		if (this.arquivo != undefined)
			this.arquivo.nativeElement.value = '';
		this.ocultarInputAnexo = false;
		this.parametros.anexo = null;
		this.parametros.nomeAnexo = null;
	}

	enviarArquivo(event) {
		if (this.parametros.anexo == null || this.parametros.anexo == undefined){
			this.modal.alerta("Não foi anexado o arquivo do documento necessário para o envio do Plano de Exportação.")
			.subscribe(()=>{
				this.arquivo.nativeElemente.focus();
			});
			return;
		}
		//validação se existe arquivo
        if (this.appUploadFile.isFiles())
        {
			let reader = new FileReader();

			reader.onload = function (e) {
				var dataURL = reader.result;
			}
			 
			let file = this.appUploadFile.uploader.queue[0]._file;

			reader.onload = () => {
				this.parametros.Arquivo = (reader.result as string).split(',')[1];
				
				this.parametros.Nomearquivo = file.name;
				this.applicationService.put<string>(this.servicoEstruturaPropria, this.parametros).subscribe(result => {
					if (parseInt(result) > 0) {
						this.modal.resposta(this.msg.ESTRUTURA_PROPRIA_SUCESSO + result, "", "");
						this.limpar();
						this.limparAnexo();
					} else
						if (result != "" && result != null && result != undefined) {
							this.modal.alerta(result, "", "");
						}
				});
			};
			reader.readAsDataURL(file);
        } else
        {
			this.modal.alerta(this.msg.ESTRUTURA_PROPRIA_FALTA_ARQUIVO, "", "");
			return
		}
	}

	limpar() {
		this.appUploadFile.removerTodos();
	}

	public abrir() {
		this.appModalEstruturaPropriaPE.abrir();
	}
}
