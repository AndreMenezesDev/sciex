import { Component, Injectable, OnInit, ViewChild } from "@angular/core";
import { Router } from "@angular/router";
import { ApplicationService } from "../../shared/services/application.service";
import { MessagesService } from "../../shared/services/messages.service";
import { ModalService } from "../../shared/services/modal.service";
import { ValidationService } from "../../shared/services/validation.service";
import { manterPliVM } from "../../view-model/ManterPliVM";
import { PagedItems } from "../../view-model/PagedItems";

@Component({
	selector: 'app-consultar-entrada-di',
	templateUrl: './consultar-entrada-di.component.html'
})

@Injectable()
export class ConsultarEntradaDiComponent implements OnInit{
	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	ocultarbotaocheck: boolean = false;
	ordenarGrid: boolean = true;
	servicoConsultarDIEntradaArquivoGrid = 'ConsultarDIEntradaArquivoGrid';

	@ViewChild('dataRecebimentoInicial') dataRecebimentoInicial;
	@ViewChild('dataRecebimentoFinal') dataRecebimentoFinal;
	@ViewChild('numeroIdentificador') numeroIdentificador;

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
		private router: Router
	){
		if (localStorage.getItem(this.router.url) == null && localStorage.length > 0) {
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

			this.listar();
		}
		else {
			this.parametros = {};
			let dat = new Date();
			this.parametros.dataInicio = dat.toLocaleDateString('en-CA');
			this.parametros.dataFim = dat.toLocaleDateString('en-CA');
		}
	}

	buscar(){

		this.validarData();

		if (!this.validationService.form('formBusca')) { return ; }

		if ((this.dataRecebimentoInicial.nativeElement.value == '') &&
		(this.dataRecebimentoFinal.nativeElement.value == '') &&
		(this.numeroIdentificador.nativeElement.value == '' || this.numeroIdentificador.nativeElement.value == undefined)) {

			this.modal.alerta(this.msg.INFORME_OS_FILTROS_DE_PESQUISA, 'Informação');
		}else{
			this.listar();
		}
	}

	listar(){

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

			if (this.numeroIdentificador.nativeElement.value == "")
				this.parametros.Identificador = null;
			else
				this.parametros.Identificador = this.numeroIdentificador.nativeElement.value;

			if (this.dataRecebimentoInicial.nativeElement.value == "") {
				this.parametros.dataInicio = null;
			} else {
				this.parametros.dataInicio = this.dataRecebimentoInicial.nativeElement.value;
			}

			if (this.dataRecebimentoFinal.nativeElement.value == "") {
				this.parametros.dataFim = null;
			} else {
				this.parametros.dataFim = this.dataRecebimentoFinal.nativeElement.value;
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

		// this.parametros.page = this.grid.page;
		// this.parametros.size = this.grid.size;
		// this.parametros.sort = this.grid.sort.field;
		// this.parametros.reverse = this.grid.sort.reverse;

		// this.parametros.dataInicio = this.dataRecebimentoInicial.nativeElement.value;
		// this.parametros.dataFim = this.dataRecebimentoFinal.nativeElement.value;
		// this.parametros.Identificador = this.numeroIdentificador.nativeElement.value;

		this.applicationService.get(this.servicoConsultarDIEntradaArquivoGrid, this.parametros).subscribe((result: PagedItems) => {
			if (result.total > 0) {
				this.parametros.exportarListagem = true;
				this.grid.lista = JSON.parse(JSON.stringify(result.items));
				this.grid.total = result.total;

				this.parametros.servico = this.servicoConsultarDIEntradaArquivoGrid;
				this.parametros.titulo = "CONSULTAR ENTRADA DI"
				this.parametros.columns = ["Identificador", "Nome Arquivo", "QT DI", "Dt. Recebido", "Status da Validação do Arquivo"];
				this.parametros.fields = ["id", "nomeArquivo", "quantidadeDiConcatenado", "dataRecepcaoFormatada", "descricaoSituacaoLeitura"];

			}else{
				this.grid.lista = null;
				this.grid.total = 0;
				this.parametros.exportarListagem = false;
			}
			this.gravarBusca();
		});
	}

	retornaValorSessao() {
	
		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;
		}
	}

	gravarBusca() {

		if (this.numeroIdentificador.nativeElement.value == "")
			this.parametros.Identificador = null;
		else
			this.parametros.Identificador = this.numeroIdentificador.nativeElement.value;

		if (this.dataRecebimentoInicial.nativeElement.value == "") {
			this.parametros.dataInicio = null;
		} else {
			this.parametros.dataInicio = this.dataRecebimentoInicial.nativeElement.value;
		}

		if (this.dataRecebimentoFinal.nativeElement.value == "") {
			this.parametros.dataFim = null;
		} else {
			this.parametros.dataFim = this.dataRecebimentoFinal.nativeElement.value;
		}

		localStorage.removeItem(this.router.url);
		localStorage.setItem(this.router.url, JSON.stringify(this.parametros));

	}

	limpar() {
		this.dataRecebimentoInicial.nativeElement.value = "";
		this.dataRecebimentoFinal.nativeElement.value = "";
		this.parametros.dataFim = "";
		this.parametros.dataInicio = "";
		this.numeroIdentificador.nativeElement.value = "";
		this.grid = { sort: {} };
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
		this.buscar();
	}

	validarData() {

		try {

			this.dataRecebimentoInicial.nativeElement.setCustomValidity('');
			this.dataRecebimentoFinal.nativeElement.setCustomValidity('');

			if (this.dataRecebimentoInicial.nativeElement.value.length > 0 || this.dataRecebimentoFinal.nativeElement.value.length > 0) {

				if (this.dataRecebimentoInicial.nativeElement.value.length >= 0 && this.dataRecebimentoInicial.nativeElement.value.length < 10) {

					this.dataRecebimentoInicial.nativeElement.setCustomValidity('Para pesquisar arquivos dentro de um período de recebimento, deve-se informar a data inicial E a data final.');

				} else if (this.dataRecebimentoFinal.nativeElement.value.length >= 0 && this.dataRecebimentoFinal.nativeElement.value.length < 10) {

					this.dataRecebimentoFinal.nativeElement.setCustomValidity('Para pesquisar arquivos dentro de um período de recebimento, deve-se informar a data inicial E a data final.');

				} else if (new Date(this.parametros.dataInicio) > new Date(this.parametros.dataFim)) {

					this.dataRecebimentoFinal.nativeElement.setCustomValidity('Para pesquisar arquivos dentro de um período de recebimento, a data inicial NÃO pode ser maior que a data final.');

				}
			}
		} catch (e) {

			this.dataRecebimentoInicial.nativeElement.setCustomValidity('Informe uma data válida.');

		}

	}

}