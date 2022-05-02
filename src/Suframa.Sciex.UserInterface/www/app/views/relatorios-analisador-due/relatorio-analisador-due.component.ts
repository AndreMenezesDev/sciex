import { inscricaoCadastralCredenciamentoVM } from './../../view-model/InscricaoCadastralCredenciamentoVM';
import { Component, OnInit, Injectable, ViewChild } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router } from '@angular/router';
import { AuthGuard } from '../../shared/guards/auth-guard.service';
import { ValidationService } from '../../shared/services/validation.service';
import { Location } from '@angular/common';
import { RelatorioAnalisadorDueVM } from '../../view-model/RelatorioAnalisadorDueVM';
import { ExcelService } from '../../shared/services/excel.service';
import { AssignHour } from '../../shared/services/assignHour.service';
import * as html2pdf from 'html2pdf.js';
@Component({
	selector: 'app-plano-de-exportacao',
	templateUrl: './relatorio-analisador-due.component.html'
})

@Injectable()
export class RelatorioAnalisadorDue implements OnInit {
	formPai = this;

	grid: any = { sort: {} };
	result: boolean = false;
	parametros: any = {}
	parametros2: RelatorioAnalisadorDueVM = new RelatorioAnalisadorDueVM()
	lista: any;
	dadosRelatorio: any;
	servico = 'RelatorioAnalisadorDues';
	date: Date = new Date();
	exibeRelatorio: boolean = false;
	arquivoRelatorio: any;
	hashPDF: any;
	linkSource: any;
	downloadLink: any;
	fileName: any;


	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private validationService: ValidationService,
		private msg: MessagesService,
		private router: Router,
		private Location: Location,
		private assignHour: AssignHour,
		private excelService: ExcelService,
		private authguard: AuthGuard
	) {

	}

	ngOnInit(): void {

	}

	exportPDF(isExcel) {
		let isValid = true

		if (this.parametros2.nomeEmpresa == null || this.parametros2.nomeEmpresa == '' && isValid) {
			this.modal.alerta("Empresa não informada");
			isValid = false;
		}

		if (isValid) {
			this.applicationService.get(this.servico, this.parametros2).subscribe((result: any) => {
				if (result) {

					this.dadosRelatorio = result;

					if (isExcel) {

						this.parametros.titulo = "ANALISADOR DUE";
						this.parametros.columns = ["Nº Plano", "Plano Status", "Data Staus", "DUE", "Valor", "Quantidade", "Ano processo", "N Processo"];
						this.parametros.fields = ["numeroPlanoFormated", "planoStatus", "dataStatus", "due", "valorDue", "quantidadeDue", "anoProcesso", "numeroProcesso"];

						var file = window.location.href.split("#")[1].replace("/", "");
						this.lista = this.dadosRelatorio;

						var excel: any = [];
						excel.push(this.parametros.columns);
						for (var x = 0; x < this.lista.length; x++) {
							let rows = Array<any>();
							for (var i = 0; i < this.lista[x].relatoriosAnaliseDue.length; i++) {
								let r = Array<any>();
								let valor: any;

								for (var j = 0; j < this.parametros.fields.length; j++) {

									var item = this.parametros.fields[j].split("|");

									valor = item.length > 0 ? Object.values(this.lista[x].relatoriosAnaliseDue)[i][item[0].trim()] : Object.values(this.lista[x].relatoriosAnaliseDue)[i][this.parametros.fields[j].trim()];

									if (this.parametros.fields[j].trim() == "razaoSocial")
										r[j] = this.lista.razaoSocial;
									else if (this.parametros.fields[j].trim() == "numeroAnoProcessoFormatado")
										r[j] = this.lista.numeroAnoProcessoFormatado;
									else {
										r[j] = valor;
									}

								}
								rows.push(r);
							}

							for (var i = 0; i < rows.length; i++) {
								excel.push(rows[i]);
							}
							this.parametros.columns = ["", "", "", "Total:", this.lista[x].valorDueTotal, this.lista[x].quantidadeDueTotal, "", ""];
							excel.push(this.parametros.columns);
						}
						this.excelService.exportAsExcelFile(excel, file, this.parametros.titulo);
					}
					else {

						this.exibeRelatorio = true;
						const assign = this.assignHour;
						let renderizarHtml = new Promise(resolve => {
							setTimeout(() => {
								const elements = document.getElementById('relatorio');
								const options = {
									margin: [0.03, 0.03, 0.5, 0.03], // [top, left, bottom, right]
									filename: "Relatório Listagem do Histórico de Processo de Exportação",
									image: { type: 'jpeg', quality: 0.98 },
									html2canvas: {
										scale: 2,
										dpi: 300,
										letterRendering: true,
										useCORS: true
									},
									jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' },
									pagebreak: { before: ['#quebraPaginaAnalises'], after: ['#quebraPagina'] }
								};
								this.arquivoRelatorio = html2pdf().from(elements).set(options).toPdf().get('pdf').then(function (pdf) {
									var totalPages = pdf.internal.getNumberOfPages();
									for (let i = 1; i <= totalPages; i++) {
										pdf.setPage(i);
										pdf.setFontSize(10);
										pdf.setTextColor(150);
										pdf.text((pdf.internal.pageSize.width / 2), 7.9, '                                                                                                                                                                                                                                                                          ' +
											'página ' + i + ' de ' + totalPages);
									}
								}).outputPdf();
							}, 5000);
							resolve(null);
						});

						let liberarTela = new Promise(resolve => {
							setTimeout(() => {
								this.salvarArquivoRelatorio();
							}, 5000);
							resolve(null);
						});

						Promise.all([renderizarHtml, liberarTela]);
					}
				} else {
					this.modal.alerta(this.msg.DADO_NAO_ENCONTRADO);
					return false;
				}
			})
		}
	}

	salvarArquivoRelatorio() {
		new Promise(resolve => {
			btoa(JSON.stringify(this.arquivoRelatorio.then(pdf => {
				resolve(btoa(pdf));
			})));
		}).then((data) => {
			this.linkSource = 'data:' + 'application/pdf' + ';base64,' + data;
			this.downloadLink = document.createElement('a');
			this.fileName = "Relatório Listagem do Histórico de Processo de Exportação";
			document.body.appendChild(this.downloadLink);
			this.downloadLink.href = this.linkSource;
			this.downloadLink.download = this.fileName;
			this.downloadLink.target = '_self';
			this.downloadLink.click();
			this.exibeRelatorio = false;
		});
	}


	limpar() {
		this.parametros2.numeroInscricaoCadastral = null;
		this.parametros2.nomeEmpresa = null;
		this.parametros2.numeroPlanoFormated = null;
		this.parametros2.due = null;
	}

}
