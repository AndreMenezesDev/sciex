
import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { PagedItems } from '../../../view-model/PagedItems';
import { ExcelService } from '../../services/excel.service';
import { PDFService } from '../../services/pdf.service';
import { FormatCodeService } from '../../services/format-code.service';
import { ManterCancelarLiComponent } from '../../../views/cancelar-li/cancelar-li.component';


@Component({
	selector: 'app-grid-cancelar-li',
	templateUrl: './grid.component.html',
})
export class GridCancelarLiComponent implements OnInit {
	@Input() isHideCabecalho = false;
	@Input() isHidePaginacao = false;
	@Input() isShowPanel: boolean = true;
	@Input() lista: Array<any>;
	@Input() page: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() total: number;

	@Input() parametros: any = {};

	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();



	constructor(
		private applicationService: ApplicationService,
		private excelService: ExcelService,
		private pdfService: PDFService,
		private formatCodeService: FormatCodeService,
		private ManterCancelarLiComponent: ManterCancelarLiComponent
	) {

	}

	ngOnInit() {
		this.page = this.page || 1;
		this.size = this.size || 10;	
	}

	changeSize($event) {		
		this.onChangeSize.emit($event);
		this.changePage(1);
	}

	changeSort($event) {
		this.parametros.sort = $event;
		this.onChangeSort.emit($event);
		this.changePage(this.page);
	}

	changePage($event) {
		this.page = $event;
		this.onChangePage.emit($event);
	}

	cancelamentos() {
		this.ManterCancelarLiComponent.cancelamentos();
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
