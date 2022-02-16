import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { PagedItems } from '../../../view-model/PagedItems';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import {Location} from '@angular/common';

declare var $: any;

@Component({
	selector: 'app-consultar-formulario-quadros-insumos-suframa',
	templateUrl: './formulario-quadros-insumos-suframa.component.html'
})

export class ConsultarFormularioQuadrosInsumosSuframaComponent implements OnInit {
	selecionarPrimeiraSolicitacao = true;
	exibirGrid: boolean = false;
	exibirFiltros: boolean = true;
	titulo: string;
	subtitulo: string;
	isQuadroNacional: boolean;
	grid: any = { sort: {}, total: 0};
	path: string;
	desabilitado: boolean;
	servico = "ProcessoProduto";
	servicoListarInsumos = "ListarProcessoInsumosNacionalOuImportadoPorIdProdutoSuframa";
	formPai = this;
	modelProduto: any = {};
	modelProcesso: any = {};
	parametros: any = {};
	listaPais = [];
	totalpais: number = 0;
	@ViewChild('appModalIncluirInsumo') appModalIncluirInsumo;
	idSolicitacaoAnalise: any;
	codigoInsumo : any;
	codigoNCM : any;
	tipoStatusAnalise : any;
	tipoAlteracao : any;
	existeSolicAlteracaoEmAnalise: boolean = false;
	idProduto: any;
	idProcesso: any;
	url : string;

	constructor(
		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private Location: Location,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router
	) {
		this.exibirFiltros = true;
		this.exibirGrid = false;
		this.url = this.router.url;
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.idProduto = this.route.snapshot.params['idProduto'];
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
		this.carregarInsumos(this.idProduto);
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
	}

	public verificarRota() {
		this.carregarOpcoesPaginacaoInicial();
		this.titulo = "-";
		this.subtitulo = "-";
		this.isQuadroNacional = false;

		if (this.path == 'visualizar-quadro-nacional') {
			this.selecionarProduto(this.idProduto);
			this.titulo = "Nacionais e Regionais - Quadro II";
			this.subtitulo = "Nacional e Regional";
			this.isQuadroNacional = true;
		}
		else if (this.path == 'visualizar-quadro-importado') {
			this.selecionarProduto(this.idProduto);
			this.titulo = "Importados - Quadro III";
			this.subtitulo = "Padrão e Extra Padrão";
			this.isQuadroNacional = false;
		}
	}

	public abrirTelaMinhasSolicitacaoes(){
		let idProcesso = this.idProcesso;

		let url = `/minhas-solicitacoes-alteracao/${idProcesso}/visualizar-quadro-importado`;
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

	public abrirModalIncluirInsumo(){
		this.appModalIncluirInsumo.abrir(this,this.isQuadroNacional,this.idProduto);
	}

	public entregarSolicitacao(){
		let obj = {
			idSolicitacao: this.parametros.idSolicitacaoAnalise
		}
		this.applicationService.post(this.servico, obj).subscribe((result: any) => {
			if (result.mensagemErro == null || result.mensagemErro == undefined)
			{
				this.modal.resposta(`Foi gerado parecer técnico: ${result.dadosParecer.numeroControle}/${result.dadosParecer.anoControle}`, "Sucesso", "").
				subscribe(()=>{
					this.router.navigate(['/consultar-processo-exportacao-suframa']);
				});
			}
			else
			{
				this.modal.alerta("Existem registros não analisados, não é possível finalizar a Análise", "Atenção", "");
				console.log(result.mensagemErro)
			}
		});
	}

	public selecionarProduto(id: number): void {
		if (!id) { return; }
		this.applicationService.get(this.servico, id).subscribe((result: any) => {
			this.modelProduto = result;
			this.modelProcesso = result.processo;
			this.existeSolicAlteracaoEmAnalise = result.existeSolicAlteracaoEmAnalise;
			this.idProcesso = result.idProcesso;

			if (!this.existeSolicAlteracaoEmAnalise){
				this.carregarInsumos(this.idProduto);
			}
		});
	}

	carregarInsumos(id?:number){

		id = id || this.idProduto;

		if (!id) { return; }

		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;

		this.parametros.isQuadroNacional = this.isQuadroNacional;
		this.parametros.idProduto = this.idProduto;
		this.parametros.idProcesso = this.idProcesso;
		this.parametros.existeSolicAlteracaoEmAnalise = this.existeSolicAlteracaoEmAnalise

		if(this.existeSolicAlteracaoEmAnalise) {
			if (this.parametros.idSolicitacaoAnalise == undefined || this.parametros.idSolicitacaoAnalise == null){
				this.modal.alerta(this.msg.CAMPO_$_OBRIGATORIO_NAO_INFORMADO.replace('$','solicitação'));
				return;
			}
		}

		// this.parametros.idSolicitacaoAnalise = this.idSolicitacaoAnalise;
		// this.parametros.codigoInsumo = this.codigoInsumo;
		// this.parametros.codigoNCM = this.codigoNCM;
		// this.parametros.tipoStatusAnalise = this.tipoStatusAnalise;
        // this.parametros.tipoAlteracao = this.tipoAlteracao;

		this.applicationService.get(this.servicoListarInsumos, this.parametros).subscribe((result: PagedItems) => {

			this.grid.lista = result.items;
			this.grid.total = result.total;

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
	limpar(){
		this.parametros.codigoInsumo = null;
		this.parametros.idNcm = null;
		this.parametros.tipoStatusAnalise = undefined;
		this.parametros.tipoAlteracao = undefined;

	}

}
