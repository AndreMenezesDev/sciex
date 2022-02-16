import { Component, ViewChild, Input, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { EstruturaPropriaPLIVM } from '../../../view-model/EstruturaPropriaPLIVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { ConsultarLEVM } from '../../../view-model/ConsultarLEVM';
import { Location } from '@angular/common';


@Component({

	selector: 'app-consultar-protocolo-envio-plano',
	templateUrl: './consultar-protocolo-envio-plano.component.html',

})

export class ManterConsultarProtocoloEnvioPlanoFormularioComponent {

	path: string;
	titulo: string;
	tituloPanel: string;
	parametros: any = {};
	model = new ConsultarLEVM;
	isModificouPesquisa: boolean = false;
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
	grid: any = { sort: {} };
	servico = 'SolicitacaoPEProduto';
	cnpjEmpresa: string;
	cnpjRazaoSocial: string;
	idEstruturaPropria: string;
	click: string = localStorage.getItem('click');

	constructor(

		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private arrayService: ArrayService,
		private modal: ModalService,
		private validationService: ValidationService,
		private router: Router,
		private _location: Location

	) {
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;

		this.grid.page = 1;
		this.grid.size = 10;

		if (
			localStorage.getItem(this.router.url) == null && localStorage.length > 0) {
			localStorage.removeItem(this.router.url);
		} else {
			this.retornaValorSessao();
			this.verificarRota();
		}
	}

	ngOnInit(): void {

		if (this.parametros != undefined || this.parametros != null) {

			this.grid.page = this.parametros.page;
			this.grid.size = this.parametros.size;
			this.grid.sort.field = this.parametros.sort;
			this.grid.sort.reverse = this.parametros.reverse;

			if (this.parametros != undefined && this.parametros != null && this.parametros.statusSolicitacao != undefined && this.parametros.statusSolicitacao != null) {
				this.selecionar(this.route.snapshot.params['id'], this.parametros.statusSolicitacao);
			}
			else {
				this.selecionar(this.route.snapshot.params['id'], this.route.snapshot.params['status']);
			}
			
		}
		else {			
			this.parametros = {};
		}
	}

	retornaValorSessao() {
		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;

			if (this.parametros != undefined && this.parametros != null && this.parametros.page != undefined && this.parametros.page != null) {
				this.grid.page = this.parametros.page;
				this.grid.size = this.parametros.size;
				this.grid.sort.field = this.parametros.sort;
				this.grid.sort.reverse = this.parametros.reverse;
			}
		}
	}

	onChangeSort($event) {
		this.grid.sort = $event;
	}

	onChangeSize($event) {
		this.grid.size = $event;
	}

	onChangePage($event) {
		this.grid.page = $event;		

		this.selecionar(this.route.snapshot.params['id'], this.route.snapshot.params['status']);
	}

	public verificarRota() {
		this.tituloPanel = 'Consultar Protocolo de Envio';

		if (this.path == 'visualizar') {
			this.tituloPanel = 'Consultar Protocolo de Envio';
			if (this.parametros != null && this.parametros != undefined && this.parametros.statusSolicitacao != undefined && this.parametros.statusSolicitacao != null) {
				this.selecionar(this.route.snapshot.params['id'], this.parametros.statusSolicitacao);
			}
			else {
				this.selecionar(this.route.snapshot.params['id'], this.route.snapshot.params['status']);
			}
			this.idEstruturaPropria = this.route.snapshot.params['id'];
		}
	}

	public selecionar(id: number, status: number) {
		if (!id) { return; }

		if (this.parametros == null) this.parametros = {};
		this.parametros.idEstruturaPropria = id;
		this.parametros.situacaoValidacao = status;


		this.applicationService.get(this.servico, this.parametros).subscribe((result: any) => {
			this.model = result;
			this.parametros.solicitacaoPELote = result.solicitacaoPELote;

			this.consultarGrid();
		});

	}

	consultarGrid(){
		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;
		this.parametros.exportarListagem = true;
		this.parametros.servico = "ConsultarProtocoloEnvioPEGrid";
		this.parametros.titulo = "CONSULTAR PROTOCOLO DE ENVIO"
		this.parametros.columns = ["Produto Exportação", "Produto Suframa","NCM", "Qtd", "Valor", "Data Validação", "Status Validação", "QTD de Erros"];
		this.parametros.fields = ["codigoProdutoPexPam", "codigoProdutoSuframa", "codigoNCM", "quantidadeFormatado", "valorDolarFormatado", "dataValidacao", "situacaoValidacaoDescricao", "qtdErros"];
		this.parametros.width = { 0: { columnWidth: 80 }, 1: { columnWidth: 80 }, 2: { columnWidth: 100 }, 3: { columnWidth: 100 }, 4: { columnWidth: 100 }, 5: { columnWidth: 100 } , 6: { columnWidth: 100 } , 7: { columnWidth: 100 }  };

		this.applicationService.get("ConsultarProtocoloEnvioPEGrid", this.parametros).subscribe((result: any) => {
			this.grid.lista = result.items;
			this.grid.total = result.total;
		});
	}

	gravarBusca() {

		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;

		localStorage.removeItem(this.router.url);
		localStorage.setItem(this.router.url, JSON.stringify(this.parametros));
	}

	voltar() {
		this._location.back();
	}
}
