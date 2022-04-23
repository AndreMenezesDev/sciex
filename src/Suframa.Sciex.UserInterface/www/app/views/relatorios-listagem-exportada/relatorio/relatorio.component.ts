import { Component, OnInit, Injectable, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { PagedItems } from '../../../view-model/PagedItems';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../shared/guards/auth-guard.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { Location } from '@angular/common';
import { AssignHour } from '../../../shared/services/assignHour.service';
import * as html2pdf from 'html2pdf.js';
@Component({
	selector: 'app-relatorio',
	templateUrl: './relatorio.component.html'
})

@Injectable()
export class RelatorioComponent implements OnInit {

	formPai = this;
	grid: any = { sort: {} };
	parametros: any = {};
	result: boolean = false;
	servico = '';
	@Input() lista: any = {};
	@Input() tipoRelatorio: any;
	@Output() DownloadFinalizado: EventEmitter<any> = new EventEmitter();
	exibeRelatorio: boolean = false;
	dataImpressao: Date;
	arquivoRelatorio: any;
	hashPDF: any;
	linkSource: any;
	downloadLink: any;
	fileName: any;
	dataInicio: string;
	dataFim: string;
	constructor(
		private assignHour: AssignHour,
		private applicationService: ApplicationService,
		private modal: ModalService,
		private validationService: ValidationService,
		private msg: MessagesService,
		private router: Router,
		private Location: Location,
		private authguard: AuthGuard
		) {

		}
		ngOnInit(): void {
		this.exportPDF()
	}
	exportPDF()
	{
		this.tipoRelatorio;
		this.dataInicio = new Date(this.lista.dataInicio).toLocaleDateString();
		this.dataFim = new Date(this.lista.dataFim).toLocaleDateString();
		this.exibeRelatorio = true;
		const assign = this.assignHour;
		let renderizarHtml = new Promise(resolve => {
			setTimeout(() => {
				const elements = document.getElementById('relatorio');
				const options = {
					margin: [0.03, 0.03, 0.5, 0.03], // [top, left, bottom, right]
					filename: "Relatório ",
					image: { type: 'jpeg', quality: 0.98 },
					html2canvas: {
						scale: 2,
						dpi: 300,
						letterRendering: true,
						useCORS: true
					},
					jsPDF: { unit: 'in', format: 'a4', orientation: 'landscape' },
					pagebreak: { before: ['#quebraPaginaAnalises'], after: ['#quebraPagina'] }
				};
				this.arquivoRelatorio = html2pdf().from(elements).set(options).toPdf().get('pdf').then(function (pdf) {
					console.log("height page: " + pdf.internal.pageSize.height);
					var totalPages = pdf.internal.getNumberOfPages();
					for (let i = 1; i <= totalPages; i++) {
					pdf.setPage(i);
					pdf.setFontSize(10);
					pdf.setTextColor(150);
					pdf.text((pdf.internal.pageSize.width/2), 7.9, '                                                                                                                                                                                                                                                                          '+
											'página '+i+' de '+totalPages);
					}
				}).outputPdf();
			}, 3000);

			resolve(null);
		});
		let liberarTela = new Promise(resolve => {
			setTimeout(() => {
				this.salvarArquivoRelatorio();
			}, 3000);
			resolve(null);
		});

		Promise.all([renderizarHtml, liberarTela]);
	}
	salvarArquivoRelatorio() {
		new Promise(resolve => {
			btoa(JSON.stringify(this.arquivoRelatorio.then(pdf => {
				resolve(btoa(pdf));
			})));
		}).then((data) => {
			this.linkSource = 'data:' + 'application/pdf' + ';base64,' + data;
			this.downloadLink = document.createElement('a');
			this.fileName = "Relatório de Listagem de Exportação Aprovada por Empresa";
			document.body.appendChild(this.downloadLink);
			this.downloadLink.href = this.linkSource;
			this.downloadLink.download = this.fileName;
			this.downloadLink.target = '_self';
			this.downloadLink.click();
			this.DownloadFinalizado.emit(true);
			this.exibeRelatorio = false;
		});
	}
}
