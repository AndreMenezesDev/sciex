import { Component, ViewChild, Input, EventEmitter, Output, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { manterPliDetalheMercadoriaVM } from '../../../view-model/ManterPliDetalheMercadoriaVM';
import { manterPliMercadoriaVM } from '../../../view-model/ManterPliMercadoriaVM';
import { Location } from '@angular/common';

@Component({
	selector: 'app-consultar-pli-mercadorias-detalhe-formulario',
	templateUrl: './formulario-mercadorias-detalhe.component.html',
})

export class ManterConsultarPliMercadoriasDetalheFormularioComponent implements OnInit {

	path: string;
	titulo: string;
	tituloPanel: string;

	modelPLI: manterPliVM = new manterPliVM();
	modelPLIMercadoria: manterPliMercadoriaVM = new manterPliMercadoriaVM();
	modelDetalheMercadoria: manterPliDetalheMercadoriaVM = new manterPliDetalheMercadoriaVM();

	servicoPLI = 'Pli';
	servicoPliMercadoria = 'PliMercadoria';
	servicoPliDetalheMercadoria = 'PliDetalheMercadoria';
	servicoPliDetalheMercadoriaGrid = 'PliDetalheMercadoriaGrid';

	isModificouPesquisa: boolean = false;

	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	ordenarGrid: boolean = true;
	isBuscaSalva: boolean = false;
	notComercializacao: boolean = true;

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
		this.verificarRota();
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

		}
		else {
			this.parametros = {};
			this.parametros.servico = this.servicoPliDetalheMercadoriaGrid;
			this.parametros.titulo = "ITENS DA MERCADORIA DO PLI"
			this.parametros.width = { 0: { columnWidth: 150 }, 1: { columnWidth: 150 }, 2: { columnWidth: 150 }, 3: { columnWidth: 150 }, 4: { columnWidth: 160 } };
			this.parametros.columns = ["ITEM", "QUANTIDADE", "VALOR UNIT. ORIG.", "VALOR UNIT. USD", "VALOR DO ITEM USD"];
			this.parametros.fields = ["codigoDetalheMercadoria", "quantidadeComercializadaFormatada", "valorUnitarioCondicaoVendaFormatada", "valorUnitarioCondicaoVendaFormatada", "valorTotalCondicaoVendaDolarFormatada"];

		}	
	}

	retornaValorSessao() {

		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;
		}
	}

		gravarBusca() {
		localStorage.removeItem(this.router.url);
		localStorage.setItem(this.router.url, JSON.stringify(this.parametros));
	}

	public verificarRota() {
		this.tituloPanel = 'Detalhe da Mercadoria';
		if (this.path == 'visualizar-mercadoria-detalhe') {
			this.tituloPanel = 'Detalhe da Mercadoria';

			this.selecionarPLI(this.route.snapshot.params['id']);
			this.selecionarMercadoria(this.route.snapshot.params['idmercadoria']);
		}
	}

	selecionarPLI(id) {
		if (!id) { return; }

		this.applicationService.get<manterPliVM>(this.servicoPLI, id).subscribe(result => {
			this.modelPLI = result;
			this.notComercializacao = this.modelPLI.idPLIAplicacao != 1 ? true : false;
		});
	}

	selecionarMercadoria(idMercadoria) {
		this.applicationService.get<manterPliMercadoriaVM>(this.servicoPliMercadoria, idMercadoria).subscribe(result => {
			this.modelPLIMercadoria = result;
			this.listar(idMercadoria);
		});
	}

	listar(idmercadoria) {

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

		this.parametros.IdPliMercadoria = idmercadoria;
		this.parametros.exportarListagem = false;
		this.applicationService.get(this.servicoPliDetalheMercadoriaGrid, this.parametros).subscribe((result: PagedItems) => {

			this.isModificouPesquisa = false;
			this.isBuscaSalva = true;
			this.grid.lista = result.items;
			this.grid.total = result.total;

			if (result.total > 0) {
				this.parametros.exportarListagem = true;
			}

			this.gravarBusca();
		});
	}

	voltar() {
		this._location.back();
	}
}
