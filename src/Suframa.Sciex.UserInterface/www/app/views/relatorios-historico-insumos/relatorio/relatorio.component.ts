import { Component, Input, OnInit, ViewChild } from "@angular/core";
import * as html2pdf from 'html2pdf.js';
import { Location } from '@angular/common';
import { MonthPipe } from "../../../shared/pipes/month.pipe";
import { ApplicationService } from "../../../shared/services/application.service";
import { ActivatedRoute } from "@angular/router";
import { ModalService } from '../../../shared/services/modal.service';


enum TipoHistorico {
	SELECIONADO = 1,
	TODOS = 2
}
@Component({
	selector: 'app-relatorio',
	templateUrl: './relatorio.component.html'
})

export class RelatorioComponent{

	exibirPdf = false;
	exibirHistorico = false;
	dataEmissao: string;
	model: any;
	cabecalho: any;
	servico = "PRCHistoricoInsumoSuframa";
	id: number;
	path: string;

	constructor(
		private location: Location,
		private applicationService: ApplicationService,
		private modal: ModalService,
		private route: ActivatedRoute
	) {
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.id = this.route.snapshot.params['idProduto'];
	}

	emitirHistoricoInsumo(id) {

		if(id != undefined){
			this.id = id;
		}


		this.buscarDados(this.id, false);

	}

	gerarPDF() {
		let renderizarHtml = new Promise(resolve => {
		let rel = 'relatoriohistoricoInsumoSelecionado';

			const elements = document.getElementById(rel);
			const options = {

				margin: [0.03, 0.03, 0.10, 0.03], // [top, left, bottom, right]
				filename: 'relatorio-historico-alteracao-insumos.pdf',
				image: { type: 'jpeg', quality: 0.98 },
				html2canvas: {
					scale: 2,
					dpi: 300,
					letterRendering: true,
					useCORS: true
				},
				jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' },
				pagebreak: { after: ['#grid'] }
			};
			html2pdf().from(elements).set(options).toPdf().get('pdf').then(function (pdf) {
				var totalPages = pdf.internal.getNumberOfPages();

				for (var i = 1; i <= totalPages; i++) {
					pdf.setPage(i);
					pdf.setFontSize(10);
					pdf.setTextColor(150);

					var dateObj = new Date();
					var month = dateObj.getUTCMonth().toString().length <= 1 ? '0' + (dateObj.getUTCMonth() + 1).toString() : dateObj.getUTCMonth() + 1;
					var day = dateObj.getUTCDate();
					var year = dateObj.getUTCFullYear();

					var hh = dateObj.getHours();
					var mm = dateObj.getMinutes();
					var ss = dateObj.getSeconds();

					var newhr = hh + ":" + mm + ":" + ss;

					var newdate = day + "/" + month + "/" + year;

					pdf.text('Data/Hora de Emissão: ' + newdate + " " + newhr
						+ '                                                                                                           Página ' + i + ' de ' + (totalPages), .2, 11.5);
				}
			}).save();

			resolve(null);
		});

		let liberarTela = new Promise(resolve => {
			setTimeout(() => {
				this.exibirPdf = false;
			}, 5000);
			resolve(null);
		});

		Promise.all([renderizarHtml, liberarTela]);
	}

	public buscarDados(id: number, isInsumo : boolean) {

		let parametros = { id, isInsumo};

		// data de emissão do relatorio
		var dateObj = new Date();
		var month = dateObj.getUTCMonth() + 1;
		var day = dateObj.getUTCDate().toString().length <= 1 ? '0' + dateObj.getUTCDate().toString() : dateObj.getUTCDate().toString();
		var year = dateObj.getUTCFullYear();

		var monthExt = new MonthPipe().transform(Number(month));

		this.dataEmissao = day + " de " + monthExt + " de " + year;
		//

		this.applicationService.get(this.servico, parametros).subscribe((result: any) => {

            if(result[0] != null){
				this.cabecalho = result[0];
			    this.model = result;
			    this.exibirPdf = true;
				this.exibirHistorico = true;
				this.gerarPDF();

			}else{
				this.exibirHistorico = false;
				this.modal.alerta( "Este Insumo não possui Histórico vinculado" , 'Informação' );
			}

		});
	}
}
