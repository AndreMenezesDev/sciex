import { Component, Output, Input, OnInit, EventEmitter, Injectable} from '@angular/core';
import { ModalService } from '../../../shared/services/modal.service';

@Component({
	selector: 'app-consultar-entrada-di-grid',
	templateUrl: './grid.component.html'
})

@Injectable()
export class ConsultarEntradaDiGridComponent {

	constructor(
		private modal: ModalService,
	) { }

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

	downloadArquivo(arquivo){

		if (arquivo) {
			const hashPDF =arquivo;
			const linkSource = 'data:' + 'application/txt' + ';base64,' + hashPDF;
			const downloadLink = document.createElement('a');
			const fileName = "SERPRODI.FILE";

			document.body.appendChild(downloadLink);

			downloadLink.href = linkSource;
			downloadLink.download = fileName;

			downloadLink.target = '_self';

			downloadLink.click();
		} else {
			this.modal.alerta('Erro ao baixar arquivo.', 'Informação');
		}
	}

}