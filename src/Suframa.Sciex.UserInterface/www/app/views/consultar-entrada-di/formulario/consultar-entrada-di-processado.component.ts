import { Component } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { ApplicationService } from "../../../shared/services/application.service";
import { PagedItems } from "../../../view-model/PagedItems";
import {Location} from '@angular/common';

@Component({

	selector: 'app-consultar-entrada-di-processado',
	templateUrl: './consultar-entrada-di-processado.component.html',

})

export class ConsultarEntradaDiProcessadoFormularioComponent {

	path: string;
	parametros: any = {};
	servicoGrid = 'ConsultaEntradaDIProcessadoGrid';

	isBuscaSalva: boolean = false;
	grid: any = { sort: {} };
	identificador: number;

	constructor(

		private route: ActivatedRoute,
		private applicationService: ApplicationService,
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

		this.identificador = this.route.snapshot.params['id'];
		
		if (this.parametros != undefined || this.parametros != null) {

			this.grid.page = this.parametros.page;
			this.grid.size = this.parametros.size;
			this.grid.sort.field = this.parametros.sort;
			this.grid.sort.reverse = this.parametros.reverse;

			if (this.parametros != undefined && this.parametros != null && this.parametros.situacaoLeitura != undefined && this.parametros.situacaoLeitura != null) {
				this.selecionar(this.route.snapshot.params['id'], this.parametros.situacaoLeitura);
			}
			else {
				this.selecionar(this.route.snapshot.params['id'], this.route.snapshot.params['situacaoLeitura']);
			}
			
		}else{
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

		this.selecionar(this.route.snapshot.params['id'], this.route.snapshot.params['situacaoLeitura']);
	}

	voltar() {
		this._location.back();
	}

	public verificarRota() {

		if (this.path == 'visualizar') {

			this.identificador = this.route.snapshot.params['id'];

			if (this.parametros != null && this.parametros != undefined && this.parametros.situacaoLeitura != undefined && this.parametros.situacaoLeitura != null) {
				this.selecionar(this.route.snapshot.params['id'], this.parametros.situacaoLeitura);
			}
			else {
				this.selecionar(this.route.snapshot.params['id'], this.route.snapshot.params['situacaoLeitura']);
			}
			
		}
	}

	public selecionar(id: number, situacaoLeitura: number) {

		if (!id) { return; }

		if (this.parametros == null){
			this.parametros = {};
		}
		
		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;

		this.parametros.id = id;
		this.parametros.exportarListagem = false;
		this.parametros.situacaoLeitura = situacaoLeitura;

		this.applicationService.get(this.servicoGrid, this.parametros).subscribe((result: PagedItems) => {
			
			this.grid.lista = result.items;
			this.grid.total = result.total;

			if (result.total > 0) {

				this.parametros.situacaoLeitura = situacaoLeitura;
				this.parametros.id = id;

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
}
