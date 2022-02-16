import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { PagedItems } from '../../../view-model/PagedItems';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import {Location} from '@angular/common';
import { ThrowStmt } from '@angular/compiler';
import { PRCInsumoVM } from '../../../view-model/PRCInsumoVM';

declare var $: any;

@Component({
	selector: 'app-consultar-formulario-quadros-insumos',
	templateUrl: './formulario-quadros-insumos.component.html'
})

export class ConsultarFormularioQuadrosInsumosComponent implements OnInit {
	titulo: string;
	subtitulo: string;
	isQuadroNacional: boolean;
	grid: any = { sort: {} };
	dadosSolicitacao: any = {};
	path: string;
	desabilitado: boolean;
	servico = "ProcessoProduto";
	servicoListarInsumos = "ListarProcessoInsumosNacionalOuImportadoPorIdProduto";
	servicoValidacaoExistenciaAlteracaoInsumo = "ValidaExistenciAlteracaoInsumo";
	servicoProcessoSolicitacao = "SolicitacaoProcesso";
	exibirCamposSolicitacoes: boolean = false;
	formPai = this;
	modelProduto: any = {};
	exibirFiltros : boolean = true;
	modelProcesso: any = {};
	parametros: any = {};
	listaPais = [];
	totalpais: number = 0;
	codigoNCM : number;
	@ViewChild('appModalIncluirInsumo') appModalIncluirInsumo;
	@ViewChild('appModalSolicitarInclusaoInsumo') appModalSolicitarInclusaoInsumo;
	@ViewChild('formBusca') formBusca;
	@ViewChild('codigoNCM2') codigoNCM2;
	@ViewChild('codigoInsumo2') codigoInsumo2;

	idProcessoProduto: any;
	idProcesso : number;
	exibirFiltroPesquisa : boolean = true;
	habilitarCombobox : boolean = false;
	url : string;
	resultConsultaProduto: any = {};
	flagAlterouGrid : boolean = false;
	flagIndicadoraAlterouInsumo : string = "";
	idNCM: any;

	flagInsumoNovoVazio: boolean;

	constructor(
		private validationService: ValidationService,
		private router: Router,
		private modal: ModalService,
		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private Location: Location,
		private msg: MessagesService
	) {
		this.url = this.router.url;
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.idProcessoProduto = this.route.snapshot.params['idProcesso'];
		sessionStorage.removeItem("routeMinhasSolicitacoes");
	}

	onChangeSort($event) {
		this.grid.sort = $event;
	}

	onChangeSize($event) {
		this.grid.size = $event;
	}

	onChangePage($event) {
		this.grid.page = $event;
		this.carregarInsumos(this.idProcessoProduto);
	}

	carregarOpcoesPaginacaoInicial(){
		this.parametros.page = 1;
		this.parametros.size = 10;
		this.grid.page = 1;
		this.grid.size = 10;
	}

	ngOnInit() {
		this.parametros = {};
		this.modelProduto = {};
		this.listaPais = [];
		this.verificarRota();
		// console.log(this.flagInsumoNovoVazio)
	}
	receberPedido(valor){
		this.flagInsumoNovoVazio= valor;
	}

	public verificarRota() {
		this.carregarOpcoesPaginacaoInicial();
		this.titulo = "-";
		this.subtitulo = "-";
		this.isQuadroNacional = false;

		if (this.path == 'visualizar-quadro-nacional') {
			this.selecionarProduto(this.idProcessoProduto);
			this.titulo = "Nacionais e Regionais - Quadro II";
			this.subtitulo = "Nacional e Regional";
			this.isQuadroNacional = true;
		}
		else if (this.path == 'visualizar-quadro-importado') {
			this.selecionarProduto(this.idProcessoProduto);
			this.titulo = "Importados - Quadro III";
			this.subtitulo = "Padrão e Extra Padrão";
			this.isQuadroNacional = false;
		}
	}

	public abrirModalIncluirInsumo(){
		this.appModalIncluirInsumo.abrir(this,this.isQuadroNacional,this.idProcessoProduto);
	}

	public selecionarProduto(id: number) {
		if (!id) { return; }
		this.applicationService.get(this.servico, id).subscribe((result: any) => {
			this.modelProduto = this.resultConsultaProduto = result;
			this.idProcesso = result.idProcesso;
			this.modelProcesso = result.processo;

			this.verificaExistenciaAlteracaoInsumo(this.idProcessoProduto);
			this.carregarSolicitacoes(result.idProcesso);			
			this.carregarInsumos(this.idProcessoProduto);
		 });
	}

	verificaExistenciaAlteracaoInsumo (IdProcessoProduto : number){
		var objeto : any = {};
		objeto.idProcessoProduto = Number(IdProcessoProduto);
		this.applicationService.get(this.servicoValidacaoExistenciaAlteracaoInsumo, objeto).subscribe((result: any) => {
			this.flagIndicadoraAlterouInsumo = result;
		});
	}

	carregarSolicitacoes(idProcesso:number){
		let id = idProcesso;
		this.applicationService.get(this.servicoProcessoSolicitacao, id).subscribe((result: PagedItems) => {
			this.dadosSolicitacao = result;
			this.exibirCamposSolicitacoes = true;
		});
	}

	criarSolicitacao(){
		let dados = {idProcesso:this.idProcesso};
		this.applicationService.post(this.servicoProcessoSolicitacao, dados).subscribe((result:any) => {
			if (result.resultado){
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informação", '')
				.subscribe(()=>{
					this.ngOnInit();
				});
			}
			else{
				this.modal.resposta(result.mensagem, "Atenção", "");
			}
		});
	}

	excluirSolicitacao(dados){
		if(!this.flagIndicadoraAlterouInsumo){
			this.modal.alerta("");
		}	

		let id = dados.id;
		this.applicationService.delete(this.servicoProcessoSolicitacao, id).subscribe((result:any) => {
			if (result){
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informação", '')
				.subscribe(()=>{
					this.ngOnInit();
				});
			}
			else{
				this.modal.resposta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Atenção", "");
			}
		});
	}

	entregarSolicitacao(dados){

		if(this.flagIndicadoraAlterouInsumo == "NAO"){
			this.modal.alerta("Para Entregar a Solicitação, é preciso <b>Solicitar Inclusão de Insumo</b> no Processo, ou Alterar os Detalhes de algum insumo")
			return false;
		}

		let id = dados.id;
		this.modal.confirmacao("Confirma a operação ?", '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.applicationService.put(this.servicoProcessoSolicitacao, dados).subscribe((result:any) => {
						if (result.resultado){
							this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informação", '')
							.subscribe(()=>{
								this.ngOnInit();
							});
						}else if(!result.validacaoListaDetalhesInsumos)
						{
							this.modal.alerta("Existem insumos novos sem o cadastro do detalhe. Não é possível entregar a Solicitação.", "Atenção", "");
						}
						else{
							this.modal.alerta(result.mensagem, "Atenção", "");
						}
					});
				}
			});
	}

	carregarInsumos(id:number){
		if (!id) { return; }
		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;
		this.parametros.isQuadroNacional = this.isQuadroNacional;
		this.parametros.idProcessoProduto = id || this.idProcesso;

		this.applicationService.get<PRCInsumoVM>(this.servicoListarInsumos, this.parametros).subscribe((result: PRCInsumoVM) => {
			this.grid.lista = result.items;
			this.grid.total = result.total;
			console.log(result)
			this.gravarBusca();
		});
	}

	GridOrginal(){
		this.retornaValorSessao();
		this.applicationService.get<PRCInsumoVM>(this.servicoListarInsumos, this.parametros).subscribe((result: PRCInsumoVM) => {
			this.grid.lista = result.items;
			this.grid.total = result.total;
			this.gravarBusca();
		});
	}


	voltar(){
		let arrayUrl = sessionStorage.getItem("arrayUrl");
		let obj = JSON.parse(arrayUrl)
		let url = obj[obj.length - 2]
		obj.pop()
		sessionStorage.removeItem("arrayUrl")
		sessionStorage.setItem("arrayUrl", JSON.stringify(obj))
		
		this.router.navigate([url]);
	}

	onBlurCodigoInsumo(){
		if(this.parametros.codigoInsumo){
			if(Number(this.parametros.codigoInsumo) > 0){
				this.preencheComboboxInsumos();
			} else {
				this.habilitarCombobox = false;
			}
		} else {
			this.habilitarCombobox = false;
		}
	}

	preencheComboboxInsumos(){
		this.habilitarCombobox = true;
	}

	limpar(){
		this.parametros.codigoInsumo = null;
		this.parametros.codigoNCM = null;
		this.codigoNCM2.clear();
		this.codigoInsumo2.clear();
		this.flagAlterouGrid ? this.GridOrginal() : '';
	}

	abrirInclusaoInsumo(){				
		this.appModalSolicitarInclusaoInsumo.abrir(Number(this.idProcessoProduto), this.resultConsultaProduto.codigoProdutoExportacao, this.resultConsultaProduto);
	}

	abrirDetalhes(dadosSolicitacao) {

		let url = `/detalhe-minha-solicitacao/${dadosSolicitacao.id}`;
		this.setHistoryUrl(url)
		this.router.navigate([url])
	}
	
	setHistoryUrl(url){
		let arrayUrl = sessionStorage.getItem("arrayUrl");
		let listArray = JSON.parse( arrayUrl)
		listArray.push(url)
		sessionStorage.removeItem("arrayUrl");
		sessionStorage.setItem("arrayUrl",JSON.stringify(listArray));
	}

	public selecionaInsumo(event) {
		if (event != null) {

			this.parametros.codigoInsumo = Number(event.text.split(" | ")[0].replace(" ", ""));
			
			if (this.parametros.codigoInsumo == null || this.parametros.codigoInsumo == undefined)
				this.codigoInsumo2.clear();
		}
	}
	public selecionaNCM(event) {
		if (event != null) {
			this.idNCM = event.id;
			this.parametros.codigoNCM = Number(event.text.split(" | ")[0].replace(" ", ""));
			
			if (this.parametros.codigoNCM == null || this.parametros.codigoNCM == undefined)
				this.codigoNCM2.clear();

			this.applicationService.get("ViewNcm", event.id).subscribe((result: any) => {
				if (result != null) {
					this.parametros.codigoNCM = Number(result.codigoNCM);
				}
			});
		}
	}

	buscarNoGrid(){

		// if(this.grid.total == 0){
		// 	this.modal.alerta("Lista de Insumos Vazia");
		// 	return false;
		// }
		this.carregarOpcoesPaginacaoInicial();

		if(!this.parametros.codigoNCM && !this.parametros.codigoInsumo){
			this.codigoNCM2.clear();
			this.codigoInsumo2.clear();
			this.carregarInsumos(this.idProcessoProduto)
		}
		else{
			this.carregarInsumos(this.idProcessoProduto)
			// this.parametros.codigoNCM && this.parametros.codigoInsumo ? 
			// this.filtrarPorNcmEInsumo() :
			// this.parametros.codigoNCM && !this.parametros.codigoInsumo ?
			// this.filtrarPorNCM() : 
			// !this.parametros.codigoNCM && this.parametros.codigoInsumo ? 
			// this.filtrarPorInsumo() : '';
		}

		
	}

	filtrarPorNcmEInsumo(){
		var codigoNCM = this.codigoNCM2.model.split(" | ")[0].replace(" ", "");
		var codigoInsumo = this.codigoInsumo2.model.split(" | ")[0].replace(" ", "");
		var selectedDates: any[] = new Array<any>();
		selectedDates = this.grid.lista;
		
		var resultFiltragem = selectedDates.filter(o => o.codigoNCM == String(codigoNCM) && o.codigoInsumo == Number(this.parametros.codigoInsumo));
		this.grid.lista = resultFiltragem;
		this.grid.total = resultFiltragem.length;
			this.flagAlterouGrid = true;		
	}

	filtrarPorNCM(){
		var codigoNCM = this.codigoNCM2.model.split(" | ")[0].replace(" ", "");
		var selectedDates: any[] = new Array<any>();
		selectedDates = this.grid.lista;
		
		var resultFiltragem = selectedDates.filter(o => o.codigoNCM == String(codigoNCM));
		this.grid.lista = resultFiltragem;
		this.grid.total = resultFiltragem.length;
			this.flagAlterouGrid = true;
	}

	filtrarPorInsumo(){
		this.carregarInsumos(this.idProcessoProduto)

		// var selectedDates: any[] = new Array<any>();
		// selectedDates = this.grid.lista;
		
		// var resultFiltragem = selectedDates.filter(o => o.codigoInsumo == Number(this.parametros.codigoInsumo));
		// this.grid.lista = resultFiltragem;
		// this.grid.total = resultFiltragem.length;
		// 	this.flagAlterouGrid = true;
	}

	gravarBusca() {
		sessionStorage.removeItem(this.router.url);
		sessionStorage.setItem(this.router.url, JSON.stringify(this.parametros));
	}

	retornaValorSessao() {
		if (sessionStorage.getItem(this.router.url) != null && sessionStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(sessionStorage.getItem(this.router.url));
		} 
	}

}
