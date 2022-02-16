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
import { manterFornecedorVM } from '../../../view-model/manterFornecedorVM';
import { manterFabricanteVM } from '../../../view-model/ManterFabricanteVM';
import { validarFornecedorFabricanteMercadoriaVM } from '../../../view-model/ValidarFornecedorFabricanteMercadoriaVM';
import { manterPliFornecedorFabricanteVM } from '../../../view-model/ManterPliFornecedorFabricanteVM';
import { Location } from '@angular/common';

@Component({
	selector: 'app-consultar-pli-mercadorias-fornecedor-formulario',
	templateUrl: './formulario-mercadorias-fornecedor.component.html',
})

export class ManterConsultarPliMercadoriasFornecedorFormularioComponent implements OnInit {

	path: string;
	titulo: string;
	tituloPanel: string;
	tipoOrigem: number;

	modelPLI: manterPliVM = new manterPliVM();
	modelPLIMercadoria: manterPliMercadoriaVM = new manterPliMercadoriaVM();
	modelPliFornecedorFabricante: manterPliFornecedorFabricanteVM = new manterPliFornecedorFabricanteVM();
	modelDetalheMercadoria: manterPliDetalheMercadoriaVM = new manterPliDetalheMercadoriaVM();
	modelFornecedor: manterFornecedorVM = new manterFornecedorVM();
	modelFabricante: manterFabricanteVM = new manterFabricanteVM();

	servicoPLIFornecedorFabricante = 'PliFornecedorFabricante';
	servicoPLI = 'Pli';
	servicoPliMercadoria = 'PliMercadoria';
	servicoFornecedor = "Fornecedor";
	servicoFabricante = "Fabricante";
	servicoProcessoAnuenteGrid = "PliProcessoAnuenteGrid";

	isModificouPesquisa: boolean = false;

	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	ordenarGrid: boolean = true;
	isBuscaSalva: boolean = false;

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
			this.parametros.titulo = "LISTAGEM DE PROCESSOS ANUENTES"
			this.parametros.width = { 0: { columnWidth: 250 }, 1: { columnWidth: 510 } };
			this.parametros.servico = this.servicoProcessoAnuenteGrid;
			this.parametros.columns = ["Número Processo", "Órgão Anuente"];
			this.parametros.fields = ["numeroProcesso", "descricao"];

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
		this.tituloPanel = 'Detalhe do Fornecedor';
		if (this.path == 'visualizar-mercadoria-fornecedor') {
			this.tituloPanel = 'Detalhe do Fornecedor';

			this.selecionarPLI(this.route.snapshot.params['id']);
			this.selecionarMercadoria(this.route.snapshot.params['idmercadoria']);
		}
	}

	selecionarPLI(id: number) {
		if (!id) { return; }
		this.applicationService.get<manterPliVM>(this.servicoPLI, id).subscribe(result => {
			this.modelPLI = result;

			var idMercadoria = this.route.snapshot.params['idmercadoria'];

			if (this.modelPLI.tipoOrigem == 1 || this.modelPLI.tipoOrigem == 3) {
				this.applicationService.get<manterPliFornecedorFabricanteVM>(this.servicoPLIFornecedorFabricante, idMercadoria).subscribe(result => {
					this.modelPliFornecedorFabricante = result;
				});
			}
		});
	}

	selecionarMercadoria(idMercadoria: number) {		
		this.applicationService.get<manterPliMercadoriaVM>(this.servicoPliMercadoria, idMercadoria).subscribe(result => {
			this.modelPLIMercadoria = result;
			this.listar(idMercadoria);
		});
	}

	selecionarFornecedor(idFornecedor: number) {
		this.applicationService.get<manterFornecedorVM>(this.servicoFornecedor, idFornecedor).subscribe(result => {
			this.modelFornecedor = result;
		});
	}

	selecionarFabricante(idFabricante: number) {
		this.applicationService.get<manterFabricanteVM>(this.servicoFabricante, idFabricante).subscribe(result => {
			this.modelFabricante = result;
		});
	}

	listar(idmercadoria: number) {

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

		this.parametros.IdPliProcessoAnuente = -1;
		this.parametros.IdPliMercadoria = idmercadoria;
		this.parametros.exportarListagem = false;

		this.applicationService.get(this.servicoProcessoAnuenteGrid, this.parametros).subscribe((result: PagedItems) => {

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
