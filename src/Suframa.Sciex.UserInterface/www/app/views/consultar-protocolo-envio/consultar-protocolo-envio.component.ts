import { Component, OnInit, Injectable, ViewChild, Output, EventEmitter } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router } from '@angular/router';
import { ValidationService } from "../../shared/services/validation.service";
import { manterPliVM } from '../../view-model/ManterPliVM';
import { ManterConsultarProtocoloEnvioGridComponent } from './grid/grid.component';

@Component({
	selector: 'app-consultar-protocolo-envio',
	templateUrl: './consultar-protocolo-envio.component.html',
	providers: [ManterConsultarProtocoloEnvioGridComponent]
})

@Injectable()
export class ManterConsultarProtocoloEnvioComponent implements OnInit {

	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	ocultarbotaocheck: boolean = false;
	ordenarGrid: boolean = true;
	servicoConsultarProtocoloEnvioGrid = 'ConsultarProtocoloEnvioGrid';

	@ViewChild('inscricaoCadastral') inscricaoCadastral;
	@ViewChild('empresa') empresa;
	@ViewChild('npli') npli;
	@ViewChild('dataEnvioInicial') dataEnvioInicial;
	@ViewChild('dataEnvioFinal') dataEnvioFinal;
	@ViewChild('codigoProtocolo') codigoProtocolo;
	@ViewChild('appModalJustificativaReprocessar') appModalJustificativaReprocessar;
	@ViewChild('grid') grid1;
	isModificouPesquisa: boolean = false;
	model: manterPliVM = new manterPliVM();
	isBuscaSalva: boolean = false;
	carregarGrid: boolean = false;
	opcaoStatus: number;
	isGeracaoDebito: boolean;
	isAnaliseVisual: boolean;
	isAguardandoProcessamento: boolean;
	isProcessado: boolean;
	ocultarBotaoReprocessar: boolean = true;
	dataInicio: Date;
	espDhInicio: Date;
	espDhFim: Date;

	constructor(
		private applicationService: ApplicationService,
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
		private ManterConsultarProtocoloEnvioGridComponent: ManterConsultarProtocoloEnvioGridComponent
	) {

		if (
			localStorage.getItem(this.router.url) == null && localStorage.length > 0) {
			localStorage.clear();
		}
	}

	ngOnInit(): void {

		this.retornaValorSessao();

		if (this.parametros != undefined || this.parametros != null) {

			this.grid.page = this.parametros.page;
			this.grid.size = this.parametros.size;
			this.grid.sort.field = this.parametros.sort;
			this.grid.sort.reverse = this.parametros.reverse;

			if (this.parametros.codigo != "-1")
				localStorage.removeItem(this.router.url);
			this.listar();
		}
		else {
			this.parametros = {};
			this.parametros.servico = this.servicoConsultarProtocoloEnvioGrid;
			this.parametros.titulo = "CONSULTAR PROTOCOLO DE ENVIO"
			//this.parametros.width = {
			//	0: { columnWidth: 100 }, 1: { columnWidth: 80 }, 2: { columnWidth: 280 }, 3: { columnWidth: 83 }, 4: { columnWidth: 70 }, 5: { columnWidth: 200 } };
			this.parametros.columns = ["Nº Protocolo", "Inscrição", "Nome Arquivo", "Quantidade", "Dt. Envio", "Status da Validação do Arquivo"];
			this.parametros.fields = ["idEstruturaPropria", "inscricaoCadastral", "nomeArquivoEnvio", "quantidadePliConcatenado", "dataEnvio", "statusValidacaoArquivoConcatenado"];

			let dat = new Date();
			this.parametros.dataInicio = dat.toLocaleDateString('en-CA');
			this.parametros.dataFim = dat.toLocaleDateString('en-CA');
		}

	}

	retornaValorSessao() {

		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {

			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;

		}

	}

	validarData() {

		try {

			this.dataEnvioInicial.nativeElement.setCustomValidity('');
			this.dataEnvioFinal.nativeElement.setCustomValidity('');

			if (this.dataEnvioInicial.nativeElement.value.length > 0 || this.dataEnvioFinal.nativeElement.value.length > 0) {

				if (this.dataEnvioInicial.nativeElement.value.length >= 0 && this.dataEnvioInicial.nativeElement.value.length < 10) {

					this.dataEnvioInicial.nativeElement.setCustomValidity('Para pesquisar protocolos dentro de um período de envio, deve-se informar a data inicial E a data final.');

				} else if (this.dataEnvioFinal.nativeElement.value.length >= 0 && this.dataEnvioFinal.nativeElement.value.length < 10) {

					this.dataEnvioFinal.nativeElement.setCustomValidity('Para pesquisar protocolos dentro de um período de envio, deve-se informar a data inicial E a data final.');

				} else if (new Date(this.parametros.dataFim) < new Date(this.parametros.dataInicio)) {

					this.dataEnvioFinal.nativeElement.setCustomValidity('Para pesquisar protocolos dentro de um período de envio, a data inicial NÃO pode ser maior que a data final.');

				}
			}
		} catch (e) {

			this.dataEnvioInicial.nativeElement.setCustomValidity('Informe uma data válida.');

		}

	}

	buscar(exibirMensagem) {

		this.validarData();

		if (!this.validationService.form('formBusca')) { return ; }

		if (((this.dataEnvioInicial.nativeElement.value == '') &&
			(this.dataEnvioFinal.nativeElement.value == '') &&
			(this.codigoProtocolo.nativeElement.value == '' || this.codigoProtocolo.nativeElement.value == undefined)) && this.parametros.tipoArquivo == undefined) {

			if (exibirMensagem) {

				this.modal.alerta(this.msg.INFORME_OS_FILTROS_DE_PESQUISA, 'Informação');

			} else {

				this.validarData()

				if (this.isBuscaSalva) {

					this.listar();

				}
			}
		}
		else {

			this.validarData();

			if (exibirMensagem) {

				this.isModificouPesquisa = true;

			}
			else {

				this.isBuscaSalva = true;

			}
			
			this.listar();

		}
	}

	ocultar() {
		if (this.ocultarFiltro === false)
			this.ocultarFiltro = true;
		else
			this.ocultarFiltro = false;
	}

	onChangeSort($event) {
		this.grid.sort = $event;
	}

	onChangeSize($event) {
		this.grid.size = $event;
	}

	onChangePage($event) {
		this.grid.page = $event;
		this.buscar(false);
	}

	limpar() {
		this.dataEnvioInicial.nativeElement.value = "";
		this.dataEnvioFinal.nativeElement.value = "";
		this.parametros.dataFim = "";
		this.parametros.dataInicio = "";
		this.parametros.tipoArquivo = undefined;
		this.codigoProtocolo.nativeElement.value = "";
	}

	listar() {		

		if (!this.isBuscaSalva || this.isModificouPesquisa) {

			if (this.isModificouPesquisa) {
				this.parametros.page = 1;
				this.parametros.size = 10;
				this.grid.page = 1;
				this.grid.size = 10;
			}
			else {
				this.parametros.page = this.grid.page;
				this.parametros.size = this.grid.size;
			}
			this.parametros.sort = this.grid.sort.field;
			this.parametros.reverse = this.grid.sort.reverse;

			if (this.dataEnvioInicial.nativeElement.value == "") {
				this.parametros.dataInicio = null;
			} else {
				this.parametros.dataInicio = this.dataEnvioInicial.nativeElement.value;
			}

			if (this.dataEnvioFinal.nativeElement.value == "") {
				this.parametros.dataFim = null;
			} else {
				this.parametros.dataFim = this.dataEnvioFinal.nativeElement.value;
			}

			if (this.codigoProtocolo.nativeElement.value == "") {
				this.parametros.idEstruturaPropria = -1;
			} else {
				this.parametros.idEstruturaPropria = this.codigoProtocolo.nativeElement.value;
			}
		}
		else {

			// Recuperar dados do localStorage
			if (this.parametros.page != this.grid.page)
				this.parametros.page = this.grid.page;
			else
				this.grid.page = this.parametros.page;

			if (this.grid.size != this.parametros.size) {
				this.parametros.size = this.grid.size;
			}
			else {
				this.grid.size = this.parametros.size;
			}

			if (this.grid.sort.field != this.parametros.sort)
				this.parametros.sort = this.grid.sort.field;
			else
				this.grid.sort.field = this.parametros.sort;

			if (this.grid.sort.reverse != this.parametros.reverse)
				this.parametros.reverse = this.grid.sort.reverse;
			else
				this.grid.sort.reverse = this.parametros.reverse;
		}

		this.parametros.exportarListagem = false;
		
		this.applicationService.get(this.servicoConsultarProtocoloEnvioGrid, this.parametros).subscribe((result: PagedItems) => {

			if (result.total > 0) {

				this.parametros.exportarListagem = true;

			}
			
			this.isModificouPesquisa = false;
			this.isBuscaSalva = true;
			this.grid.lista = JSON.parse(JSON.stringify(result.items));
			this.grid.total = result.total;

			this.gravarBusca();

		});

	}

	gravarBusca() {

		localStorage.removeItem(this.router.url);
		localStorage.setItem(this.router.url, JSON.stringify(this.parametros));

	}

}
