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
	objetoRelatorio : RelatorioErroDuesVM = new RelatorioErroDuesVM();
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

		this.applicationService.post("RelatorioErrorDues",obj).subscribe((result:RelatorioErroDuesVM)=>{
			if(result.statusCode == 200)
			{
				this.objetoRelatorio = result;
				tpExportacao == 1 ?
					this.exportPDF() :
						this.exportExcel();
			}
			else if(result.statusCode == 404)
			{
				this.modal.alerta("Nenhum Registro Encontrado", "Informação", "");
				return false;
			}
			else
			{
				this.modal.alerta(result.textResponse, "Erro!", "");
				console.log("Falha ao Gerar Relatório de erro nas Dues: " + result.textResponse);
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

	exportExcel(){

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