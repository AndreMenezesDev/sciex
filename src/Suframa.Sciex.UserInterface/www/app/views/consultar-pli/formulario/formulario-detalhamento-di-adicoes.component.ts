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
	selector: 'app-consultar-di-adicoes',
	templateUrl: './formulario-detalhamento-di-adicoes.component.html',
})

export class ManterDetalhamentoDiAdicoesFormularioComponent implements OnInit {

	path: string;
	titulo: string;
	tituloPanel: string;

	model: manterPliVM = new manterPliVM();

	servicoDi = 'Di';
	servicoDiLiGrid = 'DiLiGrid';

	isModificouPesquisa: boolean = false;

	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	ordenarGrid: boolean = true;
	isBuscaSalva: boolean = false;
	@Input() lista: any[];
	@Input() total: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() page: number;
	@Input() exportarListagem: boolean = false;
	@Input() isShowPanel: boolean = false;
	@Input() somenteLeitura: boolean = false;

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
		this.model.di = {};
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
			this.parametros.servico = this.servicoDiLiGrid;
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
		this.tituloPanel = 'Detalhamento da DI';
		if (this.path == 'visualizar-detalhamento-di-adicoes') {
			this.tituloPanel = 'Detalhamento da DI';

			this.selecionarPLI(this.route.snapshot.params['id']);
		}
	}

	selecionarPLI(id) {
		if (!id) { return; }

		this.applicationService.get<manterPliVM>(this.servicoDi, id).subscribe(result => {
			this.model = result;
			this.selecionarDiLi(result.di.idDi);
		});
	}

	selecionarDiLi(id) {
		this.parametros.page = this.page;
		this.parametros.total = this.total;
		this.parametros.size = this.size;
		this.parametros.sort = this.sorted;
		this.parametros.idDi = id;

		this.applicationService.get(this.servicoDiLiGrid, this.parametros).subscribe((result: PagedItems) => {
			this.total = result.total;
			this.lista = result.items;
		});
	}

	changeSize($event) {
		this.size = $event;
	}

	changeSort($event) {
		this.sorted = $event.field;
		if (this.parametros == undefined) {
			this.parametros.reverse = true;
		} else {
			this.parametros.reverse = (this.parametros.reverse ? false : true);
		}
		
		this.changePage(this.page);
	}

	changePage($event) {
		this.page = $event;
		this.selecionarDiLi(this.model.di.idDi);
	}

	voltar() {
		this._location.back();
	}
}
