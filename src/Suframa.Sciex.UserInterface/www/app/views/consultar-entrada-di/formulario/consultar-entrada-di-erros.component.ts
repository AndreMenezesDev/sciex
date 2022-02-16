import { Component } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { ApplicationService } from "../../../shared/services/application.service";
import { ErrorDIProcessamentoVM } from "../../../view-model/ErrorDIProcessamentoVM";
import { PagedItems } from "../../../view-model/PagedItems";
import { Location } from '@angular/common';

@Component({

	selector: 'app-consultar-entrada-di-erros',
	templateUrl: './consultar-entrada-di-erros.component.html',

})

export class ConsultarEntradaDiErrosFormularioComponent {

	path: string;
	parametros: any = {};
	servicoErroProcessamentoDIGrid = 'ErroProcessamentoGrid';
	grid: any = { sort: {} };

	modelErros: ErrorDIProcessamentoVM = new ErrorDIProcessamentoVM();

	constructor(

		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private _location: Location,
	) {
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.verificarRota();
	}

	onChangeSort($event) {

		this.grid.sort = $event;

	}

	onChangeSize($event) {

		this.grid.size = $event;

	}

	onChangePage($event) {

		this.grid.page = $event;
		this.listarErros(this.parametros.id);

	}

	public verificarRota() {

		if (this.path == 'visualizar-erros') {
			this.selecionar((this.route.snapshot.params['id']));
			this.modelErros = JSON.parse(localStorage.getItem("TelaErrosProcessamento"));
		}
	}

	public selecionar(id: number) {

		if (id == undefined || id == 0) { return; }

		this.listarErros(id);

	}

	voltar() {
		this._location.back();
	}

	listarErros(id) {

		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;
		this.parametros.idDiEntrada = id;
		this.parametros.exportarListagem = false;

		if (id == undefined || id == null) {
			this.parametros.idDiEntrada = this.route.snapshot.params['id'];
		}

		this.applicationService.get(this.servicoErroProcessamentoDIGrid, this.parametros).subscribe((result: PagedItems) => {

			if (result.total > 0) {
				this.grid.lista = result.items;
				this.grid.total = result.total;
			}
		});
	}
}
