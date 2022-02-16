import { Component, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { Location } from '@angular/common';

@Component({
	selector: 'app-consultar-relatorio-status-pli-formulario',
	templateUrl: './formulario-relatorio-status.component.html',
})

export class ManterConsultarRelatorioStatusPliFormularioComponent implements OnInit {

	path: string;
	titulo: string;
	tituloPanel: string;
	model: manterPliVM = new manterPliVM();
	servico = 'Pli';
	servicoRelatorioAnaliseAliGrid = 'RelatorioAnaliseAliGrid';
	isModificouPesquisa: boolean = false;
	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	ordenarGrid: boolean = true;
	isBuscaSalva: boolean = false;
	quantidadeErroAli: number;

	@ViewChild('gridStatusPli') gridStatusPli;

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
		this.retornaValorSessao();
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

			this.listar(this.parametros.idPli);
		}
		else {
			this.parametros = {};

			this.parametros.page = this.grid.page;
			this.parametros.size = this.grid.size;
			this.parametros.sort = this.grid.sort.field;
			this.parametros.reverse = this.grid.sort.reverse;

			this.parametros.titulo = "RELATÓRIO DE ANÁLISE DO PLI"
			this.parametros.width = { 0: { columnWidth: 120 }, 1: { columnWidth: 80 }, 2: { columnWidth: 100 }, 3: { columnWidth: 100 }, 4: { columnWidth: 100 }, 5: { columnWidth: 100 }, 6: { columnWidth: 160 } };
			this.parametros.servico = this.servicoRelatorioAnaliseAliGrid;
			this.parametros.columns = ["Nº ALI", "Nº LI", "NCM", "PRODUTO", "TIPO", "MODELO", "STATUS"];
			this.parametros.fields = ["numeroAli", "numeroLi", "nomenclaturaComumMercosul", "codigoProduto", "tipoProduto", "codigoModeloProduto", "descricaoStatus"];

		}
	}

	retornaValorSessao() {

		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;
		}

		if (localStorage.getItem("GridStatusPli") != null) {
			this.parametros = JSON.parse(localStorage.getItem("GridStatusPli"));
		}
	}

	listar(id: number) {

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

		this.parametros.IdPli = id;
		this.parametros.exportarListagem = false;

		this.applicationService.get(this.servicoRelatorioAnaliseAliGrid, this.parametros).subscribe((result: PagedItems) => {

			this.isModificouPesquisa = false;
			this.isBuscaSalva = true;
			this.grid.lista = result.items;
			this.grid.total = result.total;

			this.gridStatusPli.notComercializacao = this.model.idPLIAplicacao != 1 ? true : false;

			if (result.total > 0) {
				this.parametros.exportarListagem = true;
				if (result.items[0].quantidadeErroAli != undefined) {
					this.quantidadeErroAli = result.items[0].quantidadeErroAli;
				}
			}
			this.gravarBusca();

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

	public verificarRota() {

		this.tituloPanel = 'Relatório de Análise do Pli';

		if (this.path == 'relatorio-status') {

			this.tituloPanel = 'Relatório de Análise do Pli';
			this.selecionar(this.route.snapshot.params['id']);

		}

	}

	public selecionar(id: number) {

		if (!id) { return; }

		this.applicationService.get<manterPliVM>(this.servico, id).subscribe(result => {

			this.model = result;
			this.listar(id);

			this.gravarBusca();

		});

	}

	voltar() {
		this._location.back();
	}
}
