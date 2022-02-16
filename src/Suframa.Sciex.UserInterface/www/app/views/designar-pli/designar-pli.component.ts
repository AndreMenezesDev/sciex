import { Component, OnInit, Injectable, ViewChild, AfterViewInit, AfterContentChecked } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ValidationService } from "../../shared/services/validation.service";
import { manterPliVM } from '../../view-model/ManterPliVM';
import { DesignarPliGridComponent } from './grid/grid.component';

@Component({
	selector: 'app-designar-pli',
	templateUrl: './designar-pli.component.html',
	providers: [DesignarPliGridComponent]
})

@Injectable()
export class DesignarPliComponent implements OnInit {
	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	ocultarbotaocheck: boolean = false;
	ordenarGrid: boolean = true;
	servicoConsultarGrid= 'ConsultarDesignarPliGrid';
	servicoConsultarGridLe= 'ConsultarDesignarLeGrid';
	servicoConsultarGridPlano= 'ConsultarDesignarPlanoGrid';
	servicoConsultarSolicitacaoGrid = 'ConsultarDesignarSolicitacaoGrid';

	inscricaoSuframa = '';
	razaoSocialEmpresa = '';
	path: any;
	id: any;

	@ViewChild('pli') pli;
	@ViewChild('le') le;
	@ViewChild('pe') pe;
	@ViewChild('solic') solic;
	@ViewChild('plano') plano;
	isPli: boolean = true;
	isLe: boolean = false;
	isPlano: boolean = false;
	isSolic: boolean = false;

	@ViewChild('appModalDesignarAnalista') appModalDesignarAnalista;

	@ViewChild('inscricaoCadastral') inscricaoCadastral;
	@ViewChild('empresa') empresa;
	@ViewChild('codigoProduto') codigoProduto;
	@ViewChild('npli') npli;
	@ViewChild('dataEnvioInicial') dataEnvioInicial;
	@ViewChild('dataEnvioFinal') dataEnvioFinal;
	@ViewChild('analista') analista;
	@ViewChild('grid') grid1;

	isModificouPesquisa: boolean = false;
	model: manterPliVM = new manterPliVM();
	isBuscaSalva: boolean = false;
	carregarGrid: boolean = false;
	opcaoStatus: number;
	isAnaliseVisual: boolean;

	constructor(
		private applicationService: ApplicationService,
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private route: ActivatedRoute,
		private router: Router,
	) {

		this.path= this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.id = this.route.snapshot.params['id'];

		if(this.path == "consultarPli"){
			this.isPli = true;
			this.isLe = false;
			this.isPlano = false;
			this.isSolic = false;
		}
		else if(this.path == "consultarLe"){
			this.isPli = false;
			this.isLe = true;
			this.isPlano = false;
			this.isSolic = false;
		}
		else if(this.path == "consultarPe"){
			this.isPli = false;
			this.isLe = false;
			this.isPlano = true;
			this.isSolic = false;
		}
		else if(this.path == "consultarSolic"){
			this.isPli = false;
			this.isLe = false;
			this.isPlano = false;
			this.isSolic = true;
		}

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

			if (this.parametros.codigo != "-1")
				localStorage.removeItem(this.router.url);

			this.listar();
		}
		else {
			this.resetarAtributos();

			if(this.path == "designar-pli"){
				let dat = new Date();
				this.parametros.dataInicio = dat.toLocaleDateString('en-CA');
				this.parametros.dataFim = dat.toLocaleDateString('en-CA');
			}
			else if(this.path == "consultarPli"){
				if(!this.pli.nativeElement.checked)
					this.pli.nativeElement.checked = "checked";
				this.parametros.idAnalistaDesignado = this.id;
				this.buscar(true);
			}
			else if(this.path == "consultarLe"){
				if(!this.le.nativeElement.checked)
					this.le.nativeElement.checked = "checked";
				this.parametros.idAnalistaDesignado = this.id;
				this.buscar(true);
			}
			else if(this.path == "consultarPe"){
				if(!this.pe.nativeElement.checked)
					this.pe.nativeElement.checked = "checked";
				this.parametros.idAnalistaDesignado = this.id;
				this.buscar(true);
			}
			else if(this.path == "consultarSolic"){
				if(!this.solic.nativeElement.checked)
					this.solic.nativeElement.checked = "checked";
				this.parametros.idAnalistaDesignado = this.id;
				this.buscar(true);
			}
		}
	}

	resetarAtributos() {
		if (this.isPli) {
			this.parametros = {};
			this.parametros.servico = this.servicoConsultarGrid;
			this.parametros.titulo = "Listagem de PLI";
			this.parametros.width = { 0: { columnWidth: 80 }, 1: { columnWidth: 120 }, 2: { columnWidth: 180 }, 3: { columnWidth: 120 }, 4: { columnWidth: 120 }, 5: { columnWidth: 140 } };
			this.parametros.columns = ["Nº PLI", "Inscrição Cadastral", "Empresa", "Analista Designado", "Data de Entrega", "Status"];
			this.parametros.fields = ["numeroPliConcatenado", "inscricaoCadastral", "razaoSocial", "analistaDesignado", "dataEnvioPliFormatada", "analiseVisualStatusFormatado"];
		}
		else if (this.isLe) {
			this.parametros = {};
			this.parametros.servico = this.servicoConsultarGridLe;
			this.parametros.titulo = "Listagem de LE";
			this.parametros.width = { 0: { columnWidth: 80 }, 1: { columnWidth: 120 }, 2: { columnWidth: 180 }, 3: { columnWidth: 120 }, 4: { columnWidth: 120 }, 5: { columnWidth: 140 } };
			this.parametros.columns = ["Código Produto", "Inscrição Cadastral", "Empresa", "Analista Designado", "Data de Entrega", "Status"];
			this.parametros.fields = ["codigoProduto", "inscricaoCadastral", "razaoSocial", "nomeResponsavel", "dataEnvioFormatada", "descStatusLE"];
		}
		else if (this.isPlano) {
			this.parametros = {};
			this.parametros.servico = this.servicoConsultarGridPlano;
			this.parametros.titulo = "Listagem de Plano";
			this.parametros.width = { 0: { columnWidth: 80 }, 1: { columnWidth: 120 }, 2: { columnWidth: 180 }, 3: { columnWidth: 120 }, 4: { columnWidth: 120 }, 5: { columnWidth: 140 } };
			this.parametros.columns = ["N° Plano", "Inscrição Cadastral", "Empresa", "Analista Designado", "Data de Entrega", "Status"];
			this.parametros.fields = ["numeroPlano", "inscricaoCadastral", "razaoSocial", "nomeResponsavel", "dataEnvioFormatada", "Situacao"];
		}
		else if (this.isSolic) {
			this.parametros = {};
			this.parametros.servico = this.servicoConsultarSolicitacaoGrid;
			this.parametros.titulo = "Listagem de Solicitação";
			this.parametros.width = { 0: { columnWidth: 80 }, 1: { columnWidth: 120 }, 2: { columnWidth: 180 }, 3: { columnWidth: 120 }, 4: { columnWidth: 120 }, 5: { columnWidth: 140 } };
			this.parametros.columns = ["N° Solicitacao", "Inscrição Cadastral", "Empresa", "Analista Designado", "Data de Entrega", "Status"];
			this.parametros.fields = ["numeroSolicitacao", "inscricaoCadastral", "razaoSocial", "nomeResponsavel", "dataEnvioFormatada", "Situacao"];
		}
		else{

		}
	}

	public getSelectedOption() {

		if (this.pli.nativeElement.checked == true) {
			this.isPli = true;
			this.isLe = false;
			this.isPlano = false;
			this.isSolic = false;
			this.resetarAtributos()

			this.parametros.codigoProduto = null;
		}

		if (this.le.nativeElement.checked == true) {
			this.isPli = false;
			this.isLe = true;
			this.isPlano = false;
			this.isSolic = false;
			this.resetarAtributos()

			this.parametros.numero = null;
		}

		if (this.pe.nativeElement.checked == true) {
			this.isPli = false;
			this.isLe = false;
			this.isPlano = true;
			this.isSolic = false;
			this.resetarAtributos()

			this.parametros.numeroPlano = null;
		}
		
		if (this.solic.nativeElement.checked == true) {
			this.isPli = false;
			this.isLe = false;
			this.isPlano = false;
			this.isSolic = true;
			this.resetarAtributos()

			this.parametros.numeroPlano = null;
		}


		if(this.isPli){
			this.parametros.servico = this.servicoConsultarGrid;
			this.parametros.titulo = "Listagem de PLI";
			this.parametros.width = { 0: { columnWidth: 80 }, 1: { columnWidth: 120 }, 2: { columnWidth: 180 }, 3: { columnWidth: 120 }, 4:{ columnWidth: 120} , 5:{ columnWidth: 140} };
			this.parametros.columns = ["Nº PLI", "Inscrição Cadastral", "Empresa", "Analista Designado", "Data de Entrega", "Status"  ];
			this.parametros.fields = ["numeroPliConcatenado", "inscricaoCadastral", "razaoSocial", "analistaDesignado", "dataEnvioPliFormatada", "analiseVisualStatusFormatado"  ];
		}
		else if(this.isLe){
			this.parametros.servico = this.servicoConsultarGridLe;
			this.parametros.titulo = "Listagem de LE";
			this.parametros.width = { 0: { columnWidth: 80 }, 1: { columnWidth: 120 }, 2: { columnWidth: 180 }, 3: { columnWidth: 120 }, 4:{ columnWidth: 120} , 5:{ columnWidth: 140} };
			this.parametros.columns = ["Código Produto", "Inscrição Cadastral", "Empresa", "Analista Designado", "Data de Entrega", "Status"  ];
			this.parametros.fields = ["codigoProduto", "inscricaoCadastral", "razaoSocial", "nomeResponsavel", "dataEnvioFormatada", "descStatusLE"  ];
		}
		else if(this.isPlano){
			this.parametros.servico = this.servicoConsultarGridLe;
			this.parametros.titulo = "Listagem de Plano";
			this.parametros.width = { 0: { columnWidth: 80 }, 1: { columnWidth: 120 }, 2: { columnWidth: 180 }, 3: { columnWidth: 120 }, 4:{ columnWidth: 120} , 5:{ columnWidth: 140} };
			this.parametros.columns = ["N° Plano", "Inscrição Cadastral", "Empresa", "Analista Designado", "Data de Entrega", "Status"  ];
			this.parametros.fields = ["numeroPlano", "inscricaoCadastral", "razaoSocial", "nomeResponsavel", "dataEnvioFormatada", "descStatusLE"  ];
		}
		else if(this.isSolic){
			this.parametros.servico = this.servicoConsultarSolicitacaoGrid;
			this.parametros.titulo = "Listagem de Solicitação";
			this.parametros.width = { 0: { columnWidth: 80 }, 1: { columnWidth: 120 }, 2: { columnWidth: 180 }, 3: { columnWidth: 120 }, 4:{ columnWidth: 120} , 5:{ columnWidth: 140} };
			this.parametros.columns = ["N° Solicitação", "Inscrição Cadastral", "Empresa", "Analista Designado", "Data de Entrega", "Status"  ];
			this.parametros.fields = ["numeroAnoSolicitacao", "inscricaoCadastral", "razaoSocial", "nomeResponsavel", "dataEnvioFormatada", "descStatusLE"  ];
		}

		this.grid.lista = null;
		this.grid.total = null;
	}

	retornaValorSessao() {

		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;

			if(this.parametros != null) {
				this.parametros.page = 1;
				this.parametros.size = 10;
				this.parametros.sort = null;
				this.parametros.reverse = false;
			}
		}
	}

	validarData() {

		if (this.parametros != null) {

			try {

				this.dataEnvioInicial.nativeElement.setCustomValidity('');
				this.dataEnvioFinal.nativeElement.setCustomValidity('');

				var dataFim = new Date(this.parametros.dataFim);
				var dataInicio = new Date(this.parametros.dataInicio);


				if (this.dataEnvioInicial.nativeElement.value.length > 0 || this.dataEnvioFinal.nativeElement.value.length > 0) {
					if (this.dataEnvioInicial.nativeElement.value.length >= 0 && this.dataEnvioInicial.nativeElement.value.length < 10) {
						this.dataEnvioInicial.nativeElement.setCustomValidity('Campo inválido');
					} else if (this.dataEnvioFinal.nativeElement.value.length >= 0 && this.dataEnvioFinal.nativeElement.value.length < 10) {
						this.dataEnvioFinal.nativeElement.setCustomValidity('Campo inválido');

					} else if (new Date(this.parametros.dataFim) < new Date(this.parametros.dataInicio)) {
						this.dataEnvioFinal.nativeElement.setCustomValidity('Campo inválido. Data inicio maior que data final');
					}
				}
			} catch (e) {

				this.dataEnvioInicial.nativeElement.setCustomValidity('Informe uma data válida.');
			}

		}

	}

	buscar(exibirMensagem) {

		this.validarData();
		if (!this.validationService.form('formBusca')) { return; }


		if (this.isPli){
			if (exibirMensagem) {
				this.isModificouPesquisa = true;
			}
			else {
				this.isBuscaSalva = true;
			}
			this.listar();
		}

		if(this.isLe){
			if (exibirMensagem) {
				this.isModificouPesquisa = true;
			}
			else {
				this.isBuscaSalva = true;
			}
			this.listarLes();
		}

		if(this.isPlano){
			if (exibirMensagem) {
				this.isModificouPesquisa = true;
			}
			else {
				this.isBuscaSalva = true;
			}
			this.listarPlanos();
		}
		
		if(this.isSolic){
			if (exibirMensagem) {
				this.isModificouPesquisa = true;
			}
			else {
				this.isBuscaSalva = true;
			}
			this.listarSolicitacoes();
		}
	}
	listarSolicitacoes() {
		if (!this.isBuscaSalva || this.isModificouPesquisa) {

			if (this.isModificouPesquisa) {
				this.inicializarOrdenacaoPaginacao();
			}
			else {
				this.parametros.page = this.grid.page;
				this.parametros.size = this.grid.size;
				this.parametros.sort = this.grid.sort.field;
				this.parametros.reverse = this.grid.sort.reverse;
			}

			if (this.inscricaoCadastral.nativeElement.value == ""){
				this.parametros.inscricaoCadastral = null;
			}
			else {
				this.parametros.inscricaoCadastral = this.inscricaoCadastral.nativeElement.value;
			}

			if (this.empresa.nativeElement.value == "" || this.empresa.nativeElement.value == undefined){
				this.parametros.razaoSocial = null;
			}
			else{
				this.parametros.razaoSocial = this.empresa.nativeElement.value;
			}
			
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

		}
		else {

			// Recuperar dados do localStorage
			if (this.parametros.page != this.grid.page){
				this.parametros.page = this.grid.page;
			}
			else {
				this.grid.page = this.parametros.page;
			}

			if (this.grid.size != this.parametros.size) {
				this.parametros.size = this.grid.size;
			}
			else {
				this.grid.size = this.parametros.size;
			}

			if (this.grid.sort.field != this.parametros.sort){
				this.parametros.sort = this.grid.sort.field;
			}
			else {
				this.grid.sort.field = this.parametros.sort;
			}

			if (this.grid.sort.reverse != this.parametros.reverse){
				this.parametros.reverse = this.grid.sort.reverse;
			}
			else{
				this.grid.sort.reverse = this.parametros.reverse;
			}
		}

		if(this.parametros.numeroAnoSolicitacao != "" && this.parametros.numeroAnoSolicitacao != undefined) {
			this.parametros.solicitacao = this.parametros.numeroAnoSolicitacao.split("/")[0];
			this.parametros.anoSolicitacao = this.parametros.numeroAnoSolicitacao.split("/")[1];
		}else{
			this.parametros.numeroAnoSolicitacao = null;
		}

		if(this.parametros.numeroAnoProcesso != "" && this.parametros.numeroAnoProcesso != undefined) {
			this.parametros.numeroProcesso = this.parametros.numeroAnoProcesso.split("/")[0];
			this.parametros.anoProcesso = this.parametros.numeroAnoProcesso.split("/")[1];
		}else{
			this.parametros.numeroAnoSolicitacao = null;
		}

		this.parametros.exportarListagem = true;
		this.applicationService.get(this.servicoConsultarSolicitacaoGrid, this.parametros).subscribe((result: PagedItems) => {

			this.isModificouPesquisa = false;
			this.isBuscaSalva = true;
			this.grid.lista = JSON.parse(JSON.stringify(result.items));
			this.grid.total = result.total;

			this.gravarBuscaSolicitacao();
		});
	}

	inicializarOrdenacaoPaginacao() {
		this.grid.page = 1;
		this.grid.size = 10;
		this.parametros.page = 1;
		this.parametros.size = 10;
		this.parametros.sort = null;
		this.parametros.reverse = false;
	}

	gravarBuscaSolicitacao() {

		if (this.inscricaoCadastral.nativeElement.value == "")
			this.parametros.inscricaoCadastral = null;
		else
			this.parametros.inscricaoCadastral = this.inscricaoCadastral.nativeElement.value;

		if (this.empresa.nativeElement.value == "" || this.empresa.nativeElement.value == undefined)
			this.parametros.razaoSocial = null;
		else
			this.parametros.razaoSocial = this.empresa.nativeElement.value;

		if (this.parametros.numeroAnoSolicitacao == "" || this.parametros.numeroAnoSolicitacao == undefined) {
			this.parametros.numeroAnoSolicitacao = null;
		}
		if (this.parametros.numeroAnoProcesso == "" || this.parametros.numeroAnoProcesso == undefined) {
			this.parametros.numeroAnoProcesso = null;
		}

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

		this.parametros.isPli = this.isPli;
		this.parametros.isLe = this.isLe;
		this.parametros.isPlano = this.isPlano;
		this.parametros.isSolic = this.isSolic;

		localStorage.removeItem(this.router.url);
		localStorage.setItem(this.router.url, JSON.stringify(this.parametros));
	}

	designar(){
		let result  = this.grid.lista.filter(u => u.check == true);

		if(result.length == 0){
			this.modal.alerta('Selecione ao menos um item para Designar!', 'Informação');
			return;
		}

		this.appModalDesignarAnalista.abrir(result, this.isPli, this.isLe, this.isPlano, this.isSolic);
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
		this.parametros.dataFim = "";
		this.parametros.dataInicio = "";
		if(this.npli != null)
			this.npli.nativeElement.value = "";
		if(this.codigoProduto != null)
			this.parametros.codigoProduto = "";
		if(this.plano != null)
			this.parametros.numeroAnoPlanoFormatado = "";
		
		if (this.solic != null){
			this.parametros.numeroAnoSolicitacao = ""
			this.parametros.numeroAnoProcesso = ""

		}
		this.parametros.razaoSocial = "";
		this.parametros.inscricaoCadastral = "";
		this.parametros.idAnalistaDesignado = null;
	}

	listar() {
		this.parametros.consultarPli = 1;
		this.ocultarbotaocheck = true;


		if (!this.isBuscaSalva || this.isModificouPesquisa) {

			if (this.isModificouPesquisa) {
				this.inicializarOrdenacaoPaginacao();
			}
			else {
				this.parametros.page = this.grid.page;
				this.parametros.size = this.grid.size;
				this.parametros.sort = this.grid.sort.field;
				this.parametros.reverse = this.grid.sort.reverse;
			}

			if (this.inscricaoCadastral.nativeElement.value == ""){
				this.parametros.inscricaoCadastral = null;
			}
			else{
				this.parametros.inscricaoCadastral = this.inscricaoCadastral.nativeElement.value;
			}

			if (this.empresa.nativeElement.value == "" || this.empresa.nativeElement.value == undefined){
				this.parametros.razaoSocial = null;
			}
			else{
				this.parametros.razaoSocial = this.empresa.nativeElement.value;
			}

			if (this.npli == null || this.npli.nativeElement.value == "") {
				this.parametros.NumeroPli = -1;
				this.parametros.Ano = -1;
				this.parametros.Numero = "";
			} else {
				this.parametros.Ano = this.npli.nativeElement.value.split("/")[0];
				this.parametros.NumeroPli = +this.npli.nativeElement.value.split("/")[1];
				this.parametros.Numero = this.npli.nativeElement.value;
			}

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

		}
		else {

			// Recuperar dados do localStorage
			if (this.parametros.page != this.grid.page){
				this.parametros.page = this.grid.page;
			}
			else {
				this.grid.page = this.parametros.page;
			}

			if (this.grid.size != this.parametros.size) {
				this.parametros.size = this.grid.size;
			}
			else {
				this.grid.size = this.parametros.size;
			}

			if (this.grid.sort.field != this.parametros.sort){
				this.parametros.sort = this.grid.sort.field;
			}
			else{
				this.grid.sort.field = this.parametros.sort;
			}

			if (this.grid.sort.reverse != this.parametros.reverse){
				this.parametros.reverse = this.grid.sort.reverse;
			}
			else{
				this.grid.sort.reverse = this.parametros.reverse;
			}
		}


		this.parametros.exportarListagem = true;
		this.applicationService.get(this.servicoConsultarGrid, this.parametros).subscribe((result: PagedItems) => {

			this.isModificouPesquisa = false;
			this.isBuscaSalva = true;
			this.grid.lista = JSON.parse(JSON.stringify(result.items));
			this.grid.total = result.total;

			this.gravarBuscaPli();
		});

	}

	listarLes() {
		if (!this.isBuscaSalva || this.isModificouPesquisa) {

			if (this.isModificouPesquisa) {
				this.inicializarOrdenacaoPaginacao();
			}
			else {
				this.parametros.page = this.grid.page;
				this.parametros.size = this.grid.size;
				this.parametros.sort = this.grid.sort.field;
				this.parametros.reverse = this.grid.sort.reverse;
			}

			if (this.inscricaoCadastral.nativeElement.value == ""){
				this.parametros.inscricaoCadastral = null;
			}
			else {
				this.parametros.inscricaoCadastral = this.inscricaoCadastral.nativeElement.value;
			}

			if (this.empresa.nativeElement.value == "" || this.empresa.nativeElement.value == undefined){
				this.parametros.razaoSocial = null;
			}
			else{
				this.parametros.razaoSocial = this.empresa.nativeElement.value;
			}

			if (this.codigoProduto == null || this.codigoProduto.nativeElement.value == "") {
				this.parametros.codigoProduto = null;
			} else {
				this.parametros.codigoProduto = this.codigoProduto.nativeElement.value;
			}
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

		}
		else {

			// Recuperar dados do localStorage
			if (this.parametros.page != this.grid.page){
				this.parametros.page = this.grid.page;
			}
			else {
				this.grid.page = this.parametros.page;
			}

			if (this.grid.size != this.parametros.size) {
				this.parametros.size = this.grid.size;
			}
			else {
				this.grid.size = this.parametros.size;
			}

			if (this.grid.sort.field != this.parametros.sort){
				this.parametros.sort = this.grid.sort.field;
			}
			else {
				this.grid.sort.field = this.parametros.sort;
			}

			if (this.grid.sort.reverse != this.parametros.reverse){
				this.parametros.reverse = this.grid.sort.reverse;
			}
			else{
				this.grid.sort.reverse = this.parametros.reverse;
			}
		}


		this.parametros.exportarListagem = true;
		this.applicationService.get(this.servicoConsultarGridLe, this.parametros).subscribe((result: PagedItems) => {

			this.isModificouPesquisa = false;
			this.isBuscaSalva = true;
			this.grid.lista = JSON.parse(JSON.stringify(result.items));
			this.grid.total = result.total;

			this.gravarBuscaLe();
		});
	}

	listarPlanos() {
		if (!this.isBuscaSalva || this.isModificouPesquisa) {

			if (this.isModificouPesquisa) {
				this.inicializarOrdenacaoPaginacao();
			}
			else {
				this.parametros.page = this.grid.page;
				this.parametros.size = this.grid.size;
				this.parametros.sort = this.grid.sort.field;
				this.parametros.reverse = this.grid.sort.reverse;
			}

			if (this.inscricaoCadastral.nativeElement.value == ""){
				this.parametros.inscricaoCadastral = null;
			}
			else {
				this.parametros.inscricaoCadastral = this.inscricaoCadastral.nativeElement.value;
			}

			if (this.empresa.nativeElement.value == "" || this.empresa.nativeElement.value == undefined){
				this.parametros.razaoSocial = null;
			}
			else{
				this.parametros.razaoSocial = this.empresa.nativeElement.value;
			}

			if (this.plano == null || this.plano.nativeElement.value == "") {
				this.parametros.numeroPlano = null;
			} else {
				this.parametros.numeroPlano = this.plano.nativeElement.value;
			}
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

		}
		else {

			// Recuperar dados do localStorage
			if (this.parametros.page != this.grid.page){
				this.parametros.page = this.grid.page;
			}
			else {
				this.grid.page = this.parametros.page;
			}

			if (this.grid.size != this.parametros.size) {
				this.parametros.size = this.grid.size;
			}
			else {
				this.grid.size = this.parametros.size;
			}

			if (this.grid.sort.field != this.parametros.sort){
				this.parametros.sort = this.grid.sort.field;
			}
			else {
				this.grid.sort.field = this.parametros.sort;
			}

			if (this.grid.sort.reverse != this.parametros.reverse){
				this.parametros.reverse = this.grid.sort.reverse;
			}
			else{
				this.grid.sort.reverse = this.parametros.reverse;
			}
		}	

		this.parametros.exportarListagem = true;
		this.applicationService.get(this.servicoConsultarGridPlano, this.parametros).subscribe((result: PagedItems) => {

			this.isModificouPesquisa = false;
			this.isBuscaSalva = true;
			this.grid.lista = JSON.parse(JSON.stringify(result.items));
			this.grid.total = result.total;

			this.gravarBuscaPlano();
		});
	}

	gravarBuscaPli() {


		if (this.inscricaoCadastral.nativeElement.value == "")
			this.parametros.inscricaoCadastral = null;
		else
			this.parametros.inscricaoCadastral = this.inscricaoCadastral.nativeElement.value;

		if (this.empresa.nativeElement.value == "" || this.empresa.nativeElement.value == undefined)
			this.parametros.razaoSocial = null;
		else
			this.parametros.razaoSocial = this.empresa.nativeElement.value;

		if (this.npli.nativeElement.value == "") {
			this.parametros.NumeroPli = -1;
			this.parametros.Ano = -1;
			this.parametros.Numero = "";
		} else {
			this.parametros.Ano = this.npli.nativeElement.value.split("/")[0];
			this.parametros.NumeroPli = +this.npli.nativeElement.value.split("/")[1];
			this.parametros.Numero = this.npli.nativeElement.value;
		}

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

		this.parametros.isPli = this.isPli;
		this.parametros.isLe = this.isLe;
		this.parametros.isPlano = this.isPlano;
		this.parametros.isSolic = this.isSolic;

		localStorage.removeItem(this.router.url);
		localStorage.setItem(this.router.url, JSON.stringify(this.parametros));

	}

	gravarBuscaLe() {


		if (this.inscricaoCadastral.nativeElement.value == "")
			this.parametros.inscricaoCadastral = null;
		else
			this.parametros.inscricaoCadastral = this.inscricaoCadastral.nativeElement.value;

		if (this.empresa.nativeElement.value == "" || this.empresa.nativeElement.value == undefined)
			this.parametros.razaoSocial = null;
		else
			this.parametros.razaoSocial = this.empresa.nativeElement.value;

		if (this.codigoProduto.nativeElement.value == "") {
			this.parametros.codigoProduto = null;
		} else {
			this.parametros.codigoProduto = this.codigoProduto.nativeElement.value;
		}

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

		this.parametros.isPli = this.isPli;
		this.parametros.isLe = this.isLe;
		this.parametros.isPlano = this.isPlano;

		localStorage.removeItem(this.router.url);
		localStorage.setItem(this.router.url, JSON.stringify(this.parametros));
	}

	gravarBuscaPlano() {


		if (this.inscricaoCadastral.nativeElement.value == "")
			this.parametros.inscricaoCadastral = null;
		else
			this.parametros.inscricaoCadastral = this.inscricaoCadastral.nativeElement.value;

		if (this.empresa.nativeElement.value == "" || this.empresa.nativeElement.value == undefined)
			this.parametros.razaoSocial = null;
		else
			this.parametros.razaoSocial = this.empresa.nativeElement.value;

		if (this.plano.nativeElement.value == "") {
			this.parametros.numeroPlano = null;
		} else {
			this.parametros.numeroPlano = this.plano.nativeElement.value;
		}

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

		this.parametros.isPli = this.isPli;
		this.parametros.isLe = this.isLe;
		this.parametros.isPlano = this.isPlano;
		this.parametros.isSolic = this.isSolic;

		localStorage.removeItem(this.router.url);
		localStorage.setItem(this.router.url, JSON.stringify(this.parametros));
	}

}
