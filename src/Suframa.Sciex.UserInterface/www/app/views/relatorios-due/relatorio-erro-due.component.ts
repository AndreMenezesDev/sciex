import { Component, OnInit, Injectable, ViewChild } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { ExcelService } from '../../shared/services/excel.service';
import { Router } from '@angular/router';
import { AuthGuard } from '../../shared/guards/auth-guard.service';
import { ValidationService } from '../../shared/services/validation.service';
import { Location } from '@angular/common';
import * as html2pdf from 'html2pdf.js';
import { AssignHour } from '../../shared/services/assignHour.service';
import { async } from '@angular/core/testing';
import { RelatorioErroDuesVM } from '../../view-model/RelatorioErroDuesVM';

@Component({
	selector: 'app-plano-de-exportacao',
	templateUrl: './relatorio-erro-due.component.html'
})

@Injectable()
export class RelatoriErrosoDueComponent implements OnInit {

	formPai = this;
	grid: any = { sort: {} };
	parametros: any = {};
	result: boolean = false;
	servico = 'RelatorioErroDues';
	objetoRelatorio : Array<RelatorioErroDuesVM> = new Array<RelatorioErroDuesVM>();
	exibeRelatorio: boolean = false;
	arquivoRelatorio: any;
	hashPDF: any;
	linkSource: any;
	downloadLink: any;
	fileName: any;
	filterVm : any = {};

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private validationService: ValidationService,
		private msg: MessagesService,
		private router: Router,
		private Location: Location,
		private authguard: AuthGuard,
		private assignHour: AssignHour,
		private excelService : ExcelService
	) {

	}

	ngOnInit(): void {}

	//1 - pdf / 2 - excel
	validar(tpExportacao){

		if(!this.filterVm.nomeEmpresa && !this.filterVm.anoNumProcesso && !this.filterVm.inscricaoCadastral){
			this.modal.alerta("Informe um filtro para gerar o Relatório!");
			return false;
		}

		let obj : RelatorioErroDuesVM = new RelatorioErroDuesVM();

		if(this.filterVm.anoNumProcesso)
		{
		  let variableSplit = this.filterVm.anoNumProcesso.split("/");
		  obj.numeroPlano = Number(variableSplit[0]);
		  obj.anoPlano = Number(variableSplit[1]);
		}
		else
		{
			obj.numeroPlano = 0;
			obj.anoPlano = 0;
		}

		obj.nomeEmpresa = this.filterVm.nomeEmpresa;
		obj.inscricaoCadastral = this.filterVm.inscricaoCadastral;

		this.applicationService.post("RelatorioErrorDues",obj).subscribe((result:Array<RelatorioErroDuesVM>)=>{
			if(result)
			{
				this.objetoRelatorio = result;
				tpExportacao == 1 ?
					this.exportPDF() :
						this.exportExcel();
			}
			else
			{
				this.modal.alerta("Nenhum registro encontrado", "Erro!", "");
				return false;
			}
		})
	}

	exportPDF()
	{
		this.exibeRelatorio = true;
		const assign = this.assignHour;
		let renderizarHtml = new Promise(resolve => {
			setTimeout(() => {
				const elements = document.getElementById('relatorio-erros-dues');
				const options = {
					margin: [0.03, 0.03, 0.5, 0.03], // [top, left, bottom, right]
					filename: "Relatório de Erros nas DU-E's - Comprovação Processo Exportação",
					image: { type: 'jpeg', quality: 0.98 },
					html2canvas: {
						scale: 2,
						dpi: 300,
						letterRendering: true,
						useCORS: true
					},
					jsPDF: { unit: 'in', format: 'a4', orientation: 'landscape' },
					pagebreak: { before:  [/*'#quebraPaginaAnalises',*/'#novo-plano'], after: [/*'#quebraPagina'*/] }
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

	exportExcel()
	{
		let nomeRelatorio = "Relatório de Erros nas DU-E's - Comprovação Processo Exportação";

		var excel : any = [];

		var jsonExcel : any = {};

		this.objetoRelatorio.forEach(element => {

			jsonExcel.LinhaVazia = [""]

			jsonExcel.Cabecalho = ["Ano/N° do Plano: " + element.anoNumPlano, "Empresa: " + element.nomeEmpresa, "", "", "", "", "", ""]
			excel.push(jsonExcel.Cabecalho);

			jsonExcel.InfoPlanoExportacao = [
				"Modalidade: " + element.modalidade,
				"Tipo: " + element.tipo,
				"Data Status: " + element.dataStatus,
				"Data Receb: " + element.dataRecebimento,
				"Ano/Nº do Processo: " + element.anoNumProcesso
			];
			excel.push(jsonExcel.InfoPlanoExportacao);

			excel.push(jsonExcel.LinhaVazia);

			jsonExcel.CabecalhoRelatorioHistorioAnalise = [
				"Cd. Prod",
				"Nº DU-E",
				"Situação",
				"Responsável",
				"Justificativa"
			];
			excel.push(jsonExcel.CabecalhoRelatorioHistorioAnalise);

			element.relatorios.relatorioHistoricoAnalise.forEach(_historicoAnalise => {
				jsonExcel.itemHistoricoAnalise = [
					_historicoAnalise.codigo,
					_historicoAnalise.numeroDue,
					_historicoAnalise.situacao,
					_historicoAnalise.responsavel,
					_historicoAnalise.justificativa
				];
				excel.push(jsonExcel.itemHistoricoAnalise);
			});

			if(element.relatorios.relatorioDePara.length > 0)
			{
				excel.push(jsonExcel.LinhaVazia);

				jsonExcel.CabecalhoRelatorioDePara = [
					"Cd. Prod",
					"Nº DU-E",
					"Situação",
					"País de Destino",
					"Data Averbação",
					"Quantidade",
					"Valor",
					"Responsável",
					"Justificativa"
				];
				excel.push(jsonExcel.CabecalhoRelatorioDePara);

				element.relatorios.relatorioDePara.forEach(_relatorioDeParaItem => {
					jsonExcel.itemDePara = [
						_relatorioDeParaItem.codigo,
						_relatorioDeParaItem.numeroDue,
						_relatorioDeParaItem.situacao,
						_relatorioDeParaItem.paisDestino,
						_relatorioDeParaItem.dataAverbacao,
						_relatorioDeParaItem.quantidade,
						_relatorioDeParaItem.valor,
						_relatorioDeParaItem.responsavel,
						_relatorioDeParaItem.justificativa
					];
					excel.push(jsonExcel.itemDePara);
				});
			}

			excel.push(jsonExcel.LinhaVazia);
			excel.push(jsonExcel.LinhaVazia);

		});

		this.excelService.exportAsExcelFile(excel, nomeRelatorio, nomeRelatorio);
	}

	salvarArquivoRelatorio() {
		new Promise(resolve => {
			btoa(JSON.stringify(this.arquivoRelatorio.then(pdf => {
				resolve(btoa(pdf));
			})));
		}).then((data) => {
			this.linkSource = 'data:' + 'application/pdf' + ';base64,' + data;
			this.downloadLink = document.createElement('a');
			this.fileName = "Relatório de Erros nas DU-E's - Comprovação Processo Exportação";
			document.body.appendChild(this.downloadLink);
			this.downloadLink.href = this.linkSource;
			this.downloadLink.download = this.fileName;
			this.downloadLink.target = '_self';
			this.downloadLink.click();
			this.exibeRelatorio = false;
		});
	}



}
