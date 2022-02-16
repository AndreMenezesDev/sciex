import { Component, Output, Input, OnInit, EventEmitter, ViewChild, Injectable, OnChanges, SimpleChanges } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { EstruturaPropriaPLIVM } from '../../../view-model/EstruturaPropriaPLIVM';

@Component({
	selector: 'app-consultar-protocolo-envio-grid',
	templateUrl: './grid.component.html'
})

@Injectable()
export class ManterConsultarProtocoloEnvioGridComponent {
	servicoBaixarAnexo = "ConsultarArquivoEstruturaPropria";

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
	) { }

	@ViewChild('appModalConsultarProtocoloEnvioInformacao') appModalConsultarProtocoloEnvioInformacao;

	@Input() lista: any[];
	@Input() total: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() page: number;
	@Input() exportarListagem: boolean;
	@Input() parametros: any = {};

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();

	changeSize($event) {		
		this.onChangeSize.emit(+$event);
	}

	changeSort($event) {
		this.sorted = $event.field;
		this.onChangeSort.emit($event);
		this.changePage(this.page);
	}

	changePage($event) {
		this.page = $event;
		this.onChangePage.emit($event);
	}

	public abrir() {
		this.appModalConsultarProtocoloEnvioInformacao.abrir();
	}

	downloadArquivo(registro){

		let param = {
			idEstruturaPropria: registro.idEstruturaPropria,
			nomeArquivoEnvio: registro.nomeArquivoEnvio
		};

		this.applicationService.get(this.servicoBaixarAnexo,param)
		.subscribe((result:any)=>{
			if (result.arquivo) {
				const hashPDF = result.arquivo;
				const linkSource = 'data:' + 'application/zip' + ';base64,' + hashPDF;
				const downloadLink = document.createElement('a');
				const fileName = result.nomeArquivoEnvio;
	
				document.body.appendChild(downloadLink);
	
				downloadLink.href = linkSource;
				downloadLink.download = fileName;
	
				downloadLink.target = '_self';
	
				downloadLink.click();
			} else {
				this.modal.alerta('Erro ao baixar arquivo.', 'Informação');
			}
		});

		
	}

}
