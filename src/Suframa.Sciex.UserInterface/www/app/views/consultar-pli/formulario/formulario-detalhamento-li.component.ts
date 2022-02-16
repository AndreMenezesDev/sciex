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
import {Location} from '@angular/common';

@Component({
	selector: 'app-consultar-pli-mercadorias-li-formulario',
	templateUrl: './formulario-detalhamento-li.component.html',
})

export class ManterConsultarPliMercadoriasDetalheLIFormularioComponent implements OnInit {

	path: string;
	titulo: string;
	tituloPanel: string;
	notComercializacao: boolean = true;

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

	constructor(

		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private arrayService: ArrayService,
		private modal: ModalService,
		private validationService: ValidationService,
		private router: Router,
		private Location: Location

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
		this.tituloPanel = 'Detalhamento da LI';
		if (this.path == 'visualizar-detalhamento-li') {
			this.tituloPanel = 'Detalhamento da LI';
			this.selecionarMercadoria(this.route.snapshot.params['id']);
		}
	}

	selecionarPLI(id: number) {
		if (!id) { return; }

		this.applicationService.get<manterPliVM>(this.servicoPLI, id).subscribe(result => {
			this.modelPLI = result;
			this.notComercializacao = this.modelPLI.idPLIAplicacao != 1 ? true : false;
		});
	}

	selecionarMercadoria(idMercadoria: number) {
		this.applicationService.get<manterPliMercadoriaVM>(this.servicoPliMercadoria, idMercadoria).subscribe(result => {
			this.modelPLIMercadoria = result;
			this.selecionarPLI(this.modelPLIMercadoria.idPLI);
		});
	}

	voltar(){
		this.Location.back();
	}

}
