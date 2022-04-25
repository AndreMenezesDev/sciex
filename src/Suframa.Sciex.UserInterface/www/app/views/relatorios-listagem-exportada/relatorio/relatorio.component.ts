import { forEach } from '@angular/router/src/utils/collection';
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
import { ExcelService } from '../../../shared/services/excel.service';
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
	dataImpressao: Date;
	arquivoRelatorio: any;
	hashPDF: any;
	linkSource: any;
	downloadLink: any;
	fileName: any;
	dataInicio: string;
	dataFim: string;
	listaExcel: Array<any>
	blockRelatorio: boolean = false;
	constructor(
		private assignHour: AssignHour,
		private applicationService: ApplicationService,
		private modal: ModalService,
		private validationService: ValidationService,
		private msg: MessagesService,
		private router: Router,
		private Location: Location,
		private authguard: AuthGuard,
		private excelService : ExcelService
		) {}

	toExportFileName(excelFileName: string): string {
		return `${excelFileName}_listagem_${new Date().getTime()}.pdf`;
	}
	ngOnInit(): void {
	this.selecionaTipoExportacao(this.tipoRelatorio)
	}
	selecionaTipoExportacao(tipo){
		if(tipo == 1){
			this.exportPDF();
		}else if(tipo == 2){
			this.exportExcel();
		}
	}

	exportPDF()
	{
		this.tipoRelatorio;
		this.dataInicio = new Date(this.lista.dataInicio).toLocaleDateString();
		this.dataFim = new Date(this.lista.dataFim).toLocaleDateString();
		const assign = this.assignHour;
		let renderizarHtml = new Promise(resolve => {
			setTimeout(() => {
				const elements = document.getElementById('relatorio');
				const options = {
					margin: [0.3, 0.2, 0.5, 0.2], // [top, left, bottom, right]
					filename: "Relatório ",
					image: { type: 'jpeg', quality: 0.98 },
					html2canvas: {
						scale: 3,
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
			}, 1000);

			resolve(null);
		});
		let liberarTela = new Promise(resolve => {
			setTimeout(() => {
				this.salvarArquivoRelatorio();
			}, 1000);
			resolve(null);
		});

		Promise.all([renderizarHtml, liberarTela]);
	}

	exportExcel(){
		let nomeRelatorio = "Relatório de Listagem de Exportação Aprovada por Empresa";
		var excel : any = [];
		var jsonExcel : any = {};
		this.listaExcel = this.lista.listaLEProduto;

		jsonExcel.LinhaVazia = [""]
		jsonExcel.Cabecalho1 = ["Periodo de Aprovação: " + this.dataInicio, " até " + this.dataFim, "", "", "", "", "", ""]
		jsonExcel.Cabecalho2 = ["Empresa: " + this.lista.inscricaoCadastral, " - " + this.lista.razaoSocial, "", "", "", "", "", ""]

		if(this.dataInicio != null || this.dataFim != null){
			excel.push(jsonExcel.Cabecalho1);
		}
		excel.push(jsonExcel.Cabecalho2);
		excel.push(jsonExcel.LinhaVazia);

		jsonExcel.CabecalhoRelatorio = [
			"Cód. Produto PEXPAM",
			"NCM",
			"Cód. Produto Suframa",
			"Modelo",
		];

		if(!this.listaExcel)
		{
			jsonExcel.Na = ["NÃO HÁ REGISTROS DE LISTAGEM DE EXPORTAÇÃO"]
			excel.push(jsonExcel.Na);
		}
		else
		{
			excel.push(jsonExcel.CabecalhoRelatorio);
			this.listaExcel.forEach(item => {
				jsonExcel.itemListagemExportacao = [
					item.codigoProduto,
					item.codigoNCM,
					item.codigoProdutoSuframa,
					item.descricaoModelo
				];
				excel.push(jsonExcel.itemListagemExportacao);
			});
		}
		excel.push(jsonExcel.LinhaVazia);
		excel.push(jsonExcel.LinhaVazia);

		this.excelService.exportAsExcelFile(excel, nomeRelatorio, nomeRelatorio);
		let liberarTela = new Promise(resolve => {
			setTimeout(() => {
				this.DownloadFinalizado.emit(true);
			}, 1000);
			resolve(null);
		});
		Promise.all([liberarTela]);

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
		});
	}
}
