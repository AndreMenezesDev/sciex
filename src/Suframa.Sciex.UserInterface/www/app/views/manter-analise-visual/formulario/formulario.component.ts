import { Component, Input, ViewChild, AfterViewInit, AfterContentInit, AfterViewChecked, AfterContentChecked} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import {Location} from '@angular/common';
import { manterCodigoUtilizacaoVM } from '../../../view-model/ManterCodigoUtilizacaoVM';
import { ValidationService } from '../../../shared/services/validation.service';
import * as html2pdf from 'html2pdf.js';

@Component({

	selector: 'app-analisevisual-formulario',
	templateUrl: './formulario.component.html',
})

export class ManterAnaliseVisualFormularioComponent {
	token: string;
	path: string;
	titulo: string;
	tituloPanel: string;
	id: number;
	listaRelatorio : any;
	model: manterPliVM = new manterPliVM();
	parametros: any = {};
	listaCodigoUtilizacao: any;
	listaCodigoConta: any;
	servico = 'PliAnaliseVisual';
	servicoCodigoUtilizacao = 'CodigoUtilizacaoAnaliseDropDown';
	servicoCodigoConta= 'CodigoContaAnaliseDropDown';
	ocultarConteudoPdf: boolean = true;
	servicoRelatorio = "RelatorioAnaliseVisualRetificacoes";
	rotaRecebida: any;
	rotaVoltar: string;
	isRetificador: boolean = false;
	isPendencia: boolean = false;

	@ViewChild('formularioB') formulario;
	@ViewChild('situacao') situacao;
	@ViewChild('codigoUtilizacao') codigoUtilizacao;
	@ViewChild('motivo') motivo;
	@ViewChild('codigoConta') codigoConta;

	telaAtual: number;

	@Input() rota: any;

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private router: Router,
		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private _location: Location
	){
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.id = this.route.snapshot.params['id'];
		this.verificarRota();

		if (sessionStorage.getItem('token') && !this.token) {
			this.token = sessionStorage.getItem('token');
		}

	}
	
	ngOnInit() {
		let a = 2;
	}

	public verificarRota() {
		this.tituloPanel = 'Detalhamento do PLI';
		if (this.path == 'analisar') {
			this.tituloPanel = 'Detalhamento do PLI';
			this.selecionarPli(this.route.snapshot.params['id']);
			this.telaAtual = this.route.snapshot.params['tela'];
		}
		else if (this.path == 'visualizar-detalhamento-pli') {
			this.tituloPanel = 'Detalhamento do PLI';
			this.selecionarPli(this.route.snapshot.params['id']);
			this.telaAtual = this.route.snapshot.params['tela'];
		}
	}

	alterarSituacao(){
		let model = { 'idAplicacaoPli': this.model.idPLIAplicacao};

		this.parametros.idUtilizacao = null;
		this.parametros.idConta = null;
		this.parametros.motivo = null;

		if(this.parametros.statusPliAnalise == 7){
			this.applicationService.get(this.servicoCodigoUtilizacao, model).subscribe((result:any) => {
				if(result != null && result.length > 0){
					this.listaCodigoUtilizacao = result;
					this.listaCodigoConta = null;
				}
			});
		}
		else
		{
			this.listaCodigoUtilizacao = null;
			this.listaCodigoConta = null;
		}
	}

	alterarCodigoUtilizacao(){
		let model = { 
						'idAplicacaoPli': this.model.idPLIAplicacao,
						'idCodigoUtilizacao': this.parametros.idUtilizacao
					};
		this.listaCodigoConta = null;
		this.parametros.idConta = null;
		if(this.parametros.idUtilizacao != null && this.parametros.idUtilizacao > 0 ){
			this.applicationService.get(this.servicoCodigoConta, model).subscribe((result:any) => {
				if(result != null && result.length > 0){
					this.listaCodigoConta = result;
				}
			});
		}
	
		
	}

	public selecionarPli(id: number) {
		if (!id) { return; }
		this.applicationService.get<manterPliVM>(this.servico, id).subscribe(result => {
			this.model = result;

			if(result.tipoDocumento == 3){
				this.isRetificador = true;
			}

			if(result.descricaoResposta != null){
				this.isPendencia = true;
			}
		});

	}

	buscarRelatorio(){
		let buscarRelatorio = new Promise(resolve=>{
			this.ocultarConteudoPdf = false;
				this.applicationService.get(this.servicoRelatorio, this.model.idPLI).subscribe((result) => {
					if (result != null){
						this.listaRelatorio = result;
						let renderizarHtml = new Promise (resolve=>{
							const elements = document.getElementById('conteudoPdf');
							const options = {
								margin: [0.03, 0.03, 0.10, 0.03], // [top, left, bottom, right]
								filename: 'relatorio-retificacoes.pdf',
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
				
								  var dateObj = new Date();
								  var month = dateObj.getUTCMonth().toString().length <= 1 ? '0' + (dateObj.getUTCMonth() + 1).toString() : dateObj.getUTCMonth() + 1;
								  var day = dateObj.getUTCDate();
								  var year = dateObj.getUTCFullYear();
				
								  var hh = dateObj.getHours();
								  var mm = dateObj.getMinutes();
								  var ss = dateObj.getSeconds();
				
								  var newhr = hh + ":" + mm + ":" + ss;
				
								  var newdate = day + "/" + month + "/" + year;
				
								  pdf.text('Data/Hora de Emissão: '+ newdate + " " + newhr
								  + '                                                                                                           Página ' + i + ' de ' + (totalPages -1), .2, 11.5);
								}
								pdf.deletePage(totalPages);
							  }).save();
							resolve(null);
						});
				
						let liberarTela = new Promise (resolve=>{
							setTimeout(() => {
								this.ocultarConteudoPdf = true;
							}, 5000);
							resolve(null);
						});
				
						Promise.all([renderizarHtml,liberarTela]);
					}
				});
				resolve(null);
		})
	}

	downloadFileAnaliseVisual(){
		if (this.model.analiseVisualAnexo) {
			const hashPDF = this.model.analiseVisualAnexo;
			const linkSource = 'data:' + 'application/zip' + ';base64,' + hashPDF;
			const downloadLink = document.createElement('a');
			const fileName = this.model.analiseVisualNomeAnexo;

			document.body.appendChild(downloadLink);

			downloadLink.href = linkSource;
			downloadLink.download = fileName;

			downloadLink.target = '_self';

			downloadLink.click();
		} else {
			this.modal.alerta('Erro ao baixar arquivo.', 'Informação');
		}
	}

	downloadFile(){
		if (this.model.anexo) {
			const hashPDF = this.model.anexo;
			const linkSource = 'data:' + 'application/zip' + ';base64,' + hashPDF;
			const downloadLink = document.createElement('a');
			const fileName = this.model.nomeAnexo;

			document.body.appendChild(downloadLink);

			downloadLink.href = linkSource;
			downloadLink.download = fileName;

			downloadLink.target = '_self';

			downloadLink.click();
		} else {
			this.modal.alerta('Erro ao baixar arquivo.', 'Informação');
		}
	}

	voltar(){
		this._location.back();
	}

	visualizarMercadorias(idPLI){
		let d = idPLI;

		const url = this.router.serializeUrl(
			this.router.createUrlTree([`/consultar-pli/${idPLI}/visualizar-mercadoria-pli`])
		  );

		window.open(url, '_blank');
	}
	
	salvar(){
		this.parametros.idPLI = this.model.idPLI;
		
		if(this.parametros.statusPliAnalise == null)
			this.situacao.nativeElement.setCustomValidity('Selecione a situação');
		else{
			this.situacao.nativeElement.setCustomValidity('');
			if(this.parametros.statusPliAnalise == 7){

				if(this.parametros.idUtilizacao == null)
					this.codigoUtilizacao.nativeElement.setCustomValidity('Selecione o Codigo de Utilização');
				else
					this.codigoUtilizacao.nativeElement.setCustomValidity('');

				if(this.parametros.idConta == null)
					this.codigoConta.nativeElement.setCustomValidity('Selecione o Codigo Conta');
				else
					this.codigoConta.nativeElement.setCustomValidity('');
			}
			if(this.parametros.statusPliAnalise == 8 || this.parametros.statusPliAnalise == 9 || this.parametros.statusPliAnalise == 12){
				if(this.parametros.motivo == null || this.parametros.motivo == "")
					this.motivo.nativeElement.setCustomValidity('Descreva o motivo');
				else
					this.motivo.nativeElement.setCustomValidity('');
			}
		}


		if (!this.validationService.form('formularioB')) { return; }
		if (!this.formulario.valid) { return; }

		this.applicationService.put(this.servico, this.parametros).subscribe(result => {
			let res = result;		
			if (result != null) {
				this.modal.resposta("Salvo com Sucesso!", "Informação", "").subscribe( result => {
					this.voltar();
				});
				return;
			}
		});
	}

}
