import { Component, ViewChild, Input, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { EstruturaPropriaPLIVM } from '../../../view-model/EstruturaPropriaPLIVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { Location } from '@angular/common';


@Component({

	selector: 'app-consultar-protocolo-envio-pli',
	templateUrl: './consultar-protocolo-envio-pli.component.html',

})

export class ManterConsultarProtocoloEnvioPliFormularioComponent {

	path: string;
	titulo: string;
	tituloPanel: string;
	parametros: any = {};
	servico = 'ConsultarProtocoloEnvio';
	servicoGrid = 'ConsultarProtocoloEnvioGrid';
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
	servicoSolicitacaoPliGrid = 'SolicitacaoPliGrid';
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
			this.parametros.servico = this.servicoSolicitacaoPliGrid;
			this.parametros.titulo = "CONSULTAR PROTOCOLO DE ENVIO"
			//this.parametros.width = {
			//	0: { columnWidth: 100 }, 1: { columnWidth: 80 }, 2: { columnWidth: 280 }, 3: { columnWidth: 83 }, 4: { columnWidth: 70 }, 5: { columnWidth: 200 } };
			this.parametros.columns = ["Nº PLI Importador", "Nº PLI Suframa","Data de Validação", "Status de Validação", "QTD de Erros"];
			this.parametros.fields = ["numeroPliImportador", "numeroPliSuframa", "dataValidacao", "statusSolicitacaoNome", "qtdErrosPli"];
			this.parametros.width = { 0: { columnWidth: 120 }, 1: { columnWidth: 120 }, 2: { columnWidth: 100 }, 3: { columnWidth: 180 }, 4: { columnWidth: 180 } };
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

		console.log('consultando');

		if (!id) { return; }

		if (this.parametros == null) this.parametros = {};

		this.parametros.idEstruturaPropriaPli = id;
		this.parametros.IdSolicitacaoPli = -1;
		this.parametros.exportarListagem = false;
		this.parametros.statusSolicitacao = status;

		this.applicationService.get(this.servicoSolicitacaoPliGrid, this.parametros).subscribe((result: PagedItems) => {
			
			this.grid.lista = result.items;
			this.grid.total = result.total;

			if (result.total > 0) {

				this.parametros.statusSolicitacao = status;
				this.parametros.idEstruturaPropriaPli = id;
				this.parametros.IdSolicitacaoPli = -1;
				this.parametros.exportarListagem = true;
				this.cnpjEmpresa = result.items[0].cnpjEmpresa;
				this.cnpjRazaoSocial = result.items[0].razaoSocialEmpresa;

				this.gravarBusca();
			}
	
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
