import { Component, OnInit } from "@angular/core";
import * as html2pdf from 'html2pdf.js';
import {Location} from '@angular/common';
import { MonthPipe } from "../../../shared/pipes/month.pipe";
import { CertificadoRegistroVM } from "../../../view-model/CertificadoRegistroVM";
import { ApplicationService } from "../../../shared/services/application.service";
import { ActivatedRoute } from "@angular/router";

enum TipoCertificado {
	APROVADO = 1,
	ALTERADO = 2,
	CANCELADO = 3,
	PRORROGADO = 4,
	PRORROGADO_ESPECIAL = 5
}

@Component({
	selector: 'app-relatorio-certificado-registro',
	templateUrl: './relatorio-certificado-registro.component.html',
	styleUrls:['./relatorio-certificado-registro.component.css']
})

export class RelatorioCertificadoRegistroComponent {

	ocultarPdf = 0;
	dataEmissao: string;
	model = new CertificadoRegistroVM();
	servico="CertificadoRegistro";
	idStatus : number;
	path: string;

	constructor(
		private location: Location,
		private applicationService: ApplicationService,
		private route: ActivatedRoute
	) {
		//this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.idStatus = this.route.snapshot.params['idStatus'];		
	}

    emitirCertificado(idStatus){
		this.idStatus = idStatus;
		let carregaDados = new Promise (resolve=>{
			setTimeout(() => {
				this.buscarDados(this.idStatus);
			}, 1000);
			resolve(null);
		}).then(()=>{
			setTimeout(() => {				
				this.gerarCertificadoPDF();
			}, 4000);
		});

		// let relatorioPDF = new Promise (resolve=>{
		// 	setTimeout(() => {				
		// 		this.gerarCertificadoPDF();
		// 	}, 4000);
		// 	resolve(null);
		// });
    }

	gerarCertificadoPDF(){

		let renderizarHtml = new Promise (resolve=>{
			let rel = this.ocultarPdf == 1 ? 'certificadoPDF1' :
						this.ocultarPdf == 2 ? 'certificadoPDF2':
						this.ocultarPdf == 3 ? 'certificadoPDF3' :
						this.ocultarPdf == 4 ? 'certificadoPDF4' :
						this.ocultarPdf == 5 ? 'certificadoPDF5' : '-';

			const elements = document.getElementById(rel);
				const options = {

				margin: [0.03, 0.03, 0.10, 0.03], // [top, left, bottom, right]
				filename: 'relatorio-certificado-registro.pdf',
				image: { type: 'jpeg', quality: 0.98 },
				html2canvas: {
					scale: 2,
					dpi: 300,
					letterRendering: true,
					useCORS: true
				},
				jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' }
				};
				html2pdf().from(elements).set(options).toPdf().get('pdf').save();

				resolve(null);
		});

		let liberarTela = new Promise (resolve=>{
			setTimeout(() => {
				this.ocultarPdf = 0;
				//this.location.back();
			}, 5000);
			resolve(null);
		});

		Promise.all([renderizarHtml,liberarTela]);		
	}

	public buscarDados(id: number) {

		// data de emissão do relatorio
		var dateObj = new Date();
		var month = dateObj.getUTCMonth()+1;
		var day = dateObj.getUTCDate().toString().length <= 1 ? '0' +dateObj.getUTCDate().toString() : dateObj.getUTCDate().toString();
		var year = dateObj.getUTCFullYear();

		var monthExt = new MonthPipe().transform(Number(month));

		this.dataEmissao = day + " de " + monthExt + " de " + year;
		//
				
		this.applicationService.get(this.servico,id).subscribe((result:any) => {
			this.model = result;
			switch (result.tipo) {
				case 'AP':
					this.ocultarPdf = Number(TipoCertificado.APROVADO)
					break;

				case 'AL':
					this.ocultarPdf = Number(TipoCertificado.ALTERADO)
					break;

				case 'CA':
					this.ocultarPdf = Number(TipoCertificado.CANCELADO)
					break;

				case 'PR':
					this.ocultarPdf = Number(TipoCertificado.PRORROGADO)
					break;

				case 'PE':
					this.ocultarPdf = Number(TipoCertificado.PRORROGADO_ESPECIAL)
					break;
			}
		});
	}
}