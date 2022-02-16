import { Component, OnInit, Injectable, ViewChild } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router } from '@angular/router';
import { ViewImportadorVM } from '../../view-model/ViewImportadorVM';
import { EstruturaPropriaPLIArquivoVM } from '../../view-model/EstruturaPropriaPLIArquivoVM';
import { FileItem } from 'ng2-file-upload';
import { Observable } from 'rxjs';



@Component({
	selector: 'app-estrutura-propria-pli.component',
	templateUrl: './estrutura-propria-pli.component.html'
})


export class EstruturaPropriaPLIComponent implements OnInit {
	parametros: any = {};
	servicoViewImportador = "ViewImportador";
	servicoEstruturaPropria = "EstruturaPropriaPliArquivo";
	model: EstruturaPropriaPLIArquivoVM = new EstruturaPropriaPLIArquivoVM();

	filetype: string;
	filesize: number;

	@ViewChild('inputArquivo') inputArquivo;
	@ViewChild('optionchecked') optionchecked;
	@ViewChild('btnReset') btnReset;
	@ViewChild('appUploadFile') appUploadFile;
	@ViewChild('appModalEstruturaPropriaPli') appModalEstruturaPropriaPli;

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
	
	enviarArquivo(event) {
		//validação se existe arquivo
        if (this.appUploadFile.isFiles())
        {
			let reader = new FileReader();

			reader.onload = function (e) {
				var dataURL = reader.result;
			}
			 
			let file = this.appUploadFile.uploader.queue[0]._file;

			reader.onload = () => {
				this.parametros.Arquivo = reader.result.split(',')[1];

				this.parametros.TecnologiaAssistida = this.optionchecked.nativeElement.checked;
				this.parametros.Nomearquivo = file.name;
				this.applicationService.put<string>(this.servicoEstruturaPropria, this.parametros).subscribe(result => {
					if (parseInt(result) > 0) {
						this.modal.resposta(this.msg.ESTRUTURA_PROPRIA_SUCESSO + result, "", "");
						this.limpar();
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
		this.appModalEstruturaPropriaPli.abrir();
	}
}
