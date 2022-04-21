import { Component, OnInit, Injectable, ViewChild } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router } from '@angular/router';
import { AuthGuard } from '../../shared/guards/auth-guard.service';
import { ValidationService } from '../../shared/services/validation.service';
import { Location } from '@angular/common';
import * as html2pdf from 'html2pdf.js';
import { AssignHour } from '../../shared/services/assignHour.service';
import { FormatCodeService } from '../../shared/services/format-code.service';
import { ExcelService } from '../../shared/services/excel.service';

@Component({
	selector: 'app-plano-de-exportacao',
	templateUrl: './relatorio-historico.component.html'
})

@Injectable()
export class RelatorioHistoricoComponent implements OnInit {
	formPai = this;
	grid: any = { sort: {} };
	parametros: any = {};
	lista: any;
	processo: any;
	empresa: any;
	inscricaoSuframa: any;
	result: boolean = false;
	servico = 'RelatorioHistorico';
	exibeRelatorio: boolean = false;
	arquivoRelatorio: any;
	hashPDF: any;
	linkSource: any;
	downloadLink: any;
	fileName: any;
	dadosRelatorio: any;
	date: Date = new Date();

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private assignHour: AssignHour,
		private excelService: ExcelService,
		private formatCodeService: FormatCodeService,
		private msg: MessagesService
	) {

	}

	ngOnInit(): void {

	}

	limpar(){
		this.processo = '';
	}

	exportPDF(isExcel) {
		if (this.processo == '' || this.processo == null)
			this.modal.alerta("N° do Processo não informado");
		else {
			this.parametros = {
				processo: this.processo,
			}
			this.applicationService.get(this.servico, this.parametros).subscribe((result: any) => {
				this.dadosRelatorio = result;

				if (this.dadosRelatorio) {
					if (isExcel) {

						this.parametros.titulo = "RELATÓRIO LISTAGEM DO HISTÓRICO DO PROCESSO DE EXPORTAÇÃO";
						this.parametros.columns = ["Empresa", "Processo(Núm./Ano)", "Situação", "Data", "Usuário"];
						this.parametros.fields = ["razaoSocial", "numeroAnoProcessoFormatado", "descricaoTipo", "dataSrting", "nomeResponsavel"];

						var file = window.location.href.split("#")[1].replace("/", "");
						this.lista = this.dadosRelatorio;

						let rows = Array<any>();
						for (var i = 0; i < this.lista.listaStatus.length; i++) {
							let r = Array<any>();
							let valor: any;

							for (var j = 0; j < this.parametros.fields.length; j++) {

								var item = this.parametros.fields[j].split("|");

								valor = item.length > 0 ? Object.values(this.lista.listaStatus)[i][item[0].trim()] : Object.values(this.lista)[i][this.parametros.fields[j].trim()];

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
						var excel: any = [];

						excel.push(this.parametros.columns);

						for (var i = 0; i < rows.length; i++) {
							excel.push(rows[i]);
						}

						this.excelService.exportAsExcelFile(excel, file, this.parametros.titulo);
					}
					else {
						this.dadosRelatorio.dataImpressao = `${this.date.getDate()}/${this.date.getMonth()}/${this.date.getFullYear()}`;

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
									console.log("height page: " + pdf.internal.pageSize.height);
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

				}
			});
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
}
