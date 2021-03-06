import { Component, Input, OnInit, ViewChild } from "@angular/core";
import * as html2pdf from 'html2pdf.js';
import {Location} from '@angular/common';
import { MonthPipe } from "../../../../../shared/pipes/month.pipe";
import { ApplicationService } from "../../../../../shared/services/application.service";
import { ActivatedRoute } from "@angular/router";
import { AssignHour } from "../../../../../shared/services/assignHour.service";

enum TipoParecer {
	APROVADO = 1,
	ALTERADO = 2,
	CANCELADO = 3
}
@Component({
	selector: 'app-relatorio-parecer-tecnico',
	templateUrl: './relatorio-parecer-tecnico.component.html',
	styleUrls:['./relatorio-parecer-tecnico.component.css']
})

export class RelatorioParecerTecnicoComponent implements OnInit{

	ocultarPdf = 0;
	dataEmissao: string;
	model:any = {} ;
	servico="ParecerTecnicoRelatorio";
	id : number;
	path: string;

	constructor(
		private location: Location,
		private applicationService: ApplicationService,
		private assignHour: AssignHour,
		private route: ActivatedRoute
	) {
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.id = this.route.snapshot.params['id'];
	}

    ngOnInit(){
		this.model.parecerTecnicoComplementar = {};
		this.model.parecerTecnicoComplementar.length = 0;
    }

	emitirParecer(id){
		this.id = id;
		let carregaDados = new Promise (resolve=>{
			setTimeout(() => {
				this.buscarDados(this.id);
			}, 1000);
			resolve(null);
		}).then(()=>{
			setTimeout(() => {
				this.gerarPDF();
			}, 4000);
		});
	}

	gerarPDF(){
		const assign = this.assignHour;
		let renderizarHtml = new Promise(resolve => {
			let rel = this.ocultarPdf == 1 ? 'relatorioPDF1' :
						this.ocultarPdf == 2 ? 'relatorioPDF2' : 
						this.ocultarPdf == 3 ? 'relatorioPDF3' : '-';

			const elements = document.getElementById(rel);
			const options = {
				margin: [0.03, 0.03, 0.10, 0.03], // [top, left, bottom, right]
				filename: 'relatorio-parecer-tecnico.pdf',
				image: { type: 'jpeg', quality: 0.98 },
				html2canvas: {
					scale: 2,
					dpi: 300,
					letterRendering: true,
					useCORS: true
				},
				jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' },
				pagebreak : { after: ['#grid'] }
				};
				html2pdf().from(elements).set(options).toPdf().get('pdf').then(function (pdf) {
					var totalPages = pdf.internal.getNumberOfPages();

					for (var i = 1; i <= totalPages; i++) {
					  pdf.setPage(i);
					  pdf.setFontSize(10);
					  pdf.setTextColor(150);

					  var {newhr, newdate} = assign.getHourCurrent(new Date())

					  pdf.text('Data/Hora de Emiss??o: '+ newdate + " " + newhr
					  + '                                                                                                           P??gina ' + i + ' de ' + (totalPages), .2, 11.5);
					}
				  }).save();

				resolve(null);
		});

		let liberarTela = new Promise (resolve=>{
			setTimeout(() => {
				this.ocultarPdf = 0;
			}, 5000);
			resolve(null);
		});

		Promise.all([renderizarHtml,liberarTela]);
	}

	public buscarDados(id: number) {

		// data de emiss??o do relatorio
		var dateObj = new Date();
		var month = dateObj.getUTCMonth()+1;
		var day = dateObj.getUTCDate().toString().length <= 1 ? '0' +dateObj.getUTCDate().toString() : dateObj.getUTCDate().toString();
		var year = dateObj.getUTCFullYear();

		var monthExt = new MonthPipe().transform(Number(month));

		this.dataEmissao = day + " de " + monthExt + " de " + year;
		//

		this.applicationService.get(this.servico,id).subscribe((result:any) => {
			this.model = result;
			switch (result.tipoStatus) {
				case 'AP':
					this.ocultarPdf = Number(TipoParecer.APROVADO)
					break;

				case 'AL':
					this.ocultarPdf = Number(TipoParecer.ALTERADO)
					break;

				case 'CA':
					this.ocultarPdf = Number(TipoParecer.CANCELADO)
					break;
			}
		});
	}
}
