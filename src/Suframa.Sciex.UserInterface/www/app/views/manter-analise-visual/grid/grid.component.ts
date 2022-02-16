import { Component, Output, Input, OnInit, EventEmitter, ViewChild, Injectable, OnChanges, SimpleChanges } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterNcmExcecaoVM } from "../../../view-model/ManterNcmExcecaoVM";
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { forEach } from '@angular/router/src/utils/collection';
import { ActivatedRoute, Router } from '@angular/router';
import { ManterAnaliseVisualFormularioComponent } from '../formulario/formulario.component';
import { ManterAnaliseVisualComponent } from '../manter-analise-visual.component';
import { PagedItems } from '../../../view-model/PagedItems';
import { ExcelService } from '../../../shared/services/excel.service';
import { PDFService } from '../../../shared/services/pdf.service';
import { FormatCodeService } from '../../../shared/services/format-code.service';


@Component({

	selector: 'app-consultar-analisevisual-grid',
	templateUrl: './grid.component.html',
})

@Injectable()
export class ManterAnaliseVisualGridComponent implements OnChanges {

	servicoNCM = 'consultarPLI';

	constructor(
		private router: Router,
		private applicationService: ApplicationService,
		private excelService: ExcelService,
		private pdfService: PDFService,
		private formatCodeService: FormatCodeService,
		private modal: ModalService,
		private msg: MessagesService
	) { }

	@Input() isHideCabecalho = false;
	@Input() isHidePaginacao = false;
	@Input() isShowPanel: boolean = true;
	@Input() lista: any[];
	@Input() total: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() page: number;
	@Input() exportarListagem: boolean;
	@Input() parametros: any = {};
	
	@ViewChild('appModalAnaliseVisual') appModalAnaliseVisual;

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();


	ngOnChanges(changes: SimpleChanges) {
		
	}

	abrirModalVisualizar(item){
		this.appModalAnaliseVisual.abrir(item);
	}

	changeSize($event) {		
		this.onChangeSize.emit(+$event);
		this.onChangeSize.emit($event);
		this.changePage(1)
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

	exportar(tipo) {
		this.parametros.page = 1;
		this.parametros.size = 1000000;

		this.applicationService.get(this.parametros.servico, this.parametros).subscribe((result: PagedItems) => {

			var file = window.location.href.split("#")[1].replace("/", "");
			this.lista = result.items;

			let rows = Array<any>();
			for (var i = 0; i < this.lista.length; i++) {
				let r = Array<any>();
				let valor: any;

				for (var j = 0; j < this.parametros.fields.length; j++) {

					var item = this.parametros.fields[j].split("|");

					valor = item.length > 0 ? Object.values(this.lista)[i][item[0].trim()] : Object.values(this.lista)[i][this.parametros.fields[j].trim()];

					if (item.length == 2) {
						if (item[1].split(":")[0].trim() == "formatCode") {
							r[j] = this.formatCodeService.transform(valor, item[1].split(":")[1]);
						}
					}
					else if (this.parametros.fields[j].trim() == "status") 
						r[j] = (valor == 1 ? "ATIVO" : "INATIVO");
					else {
						r[j] = valor;
					}

				}
				rows.push(r);
			}
			if (tipo == 1) {
				this.pdfService.exportAsPDFFile(this.parametros.columns, rows, this.parametros.width, file, this.parametros.titulo);
			}
			else if (tipo == 2) {
				var excel: any = [];

				excel.push(this.parametros.columns);

				for (var i = 0; i < rows.length; i++) {
					excel.push(rows[i]);
				}

				this.excelService.exportAsExcelFile(excel, file, this.parametros.titulo);
			}
		});
	}
}
