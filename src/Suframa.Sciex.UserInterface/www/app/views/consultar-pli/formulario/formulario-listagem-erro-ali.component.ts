import { Component, ViewChild, Input, EventEmitter, Output, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { manterPliMercadoriaVM } from '../../../view-model/ManterPliMercadoriaVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { aliVM } from '../../../view-model/AliVM';
import { Location } from '@angular/common';

@Component({
	selector: 'app-relatorio-listagem-erro-ali-formulario',
	templateUrl: './formulario-listagem-erro-ali.component.html',
})

export class ManterConsultarListagemErroAliComponent implements OnInit {

	path: string;
	titulo: string;
	tituloPanel: string;
	model: manterPliMercadoriaVM = new manterPliMercadoriaVM();
	modelAli: aliVM = new aliVM();
	manterPLI: manterPliVM = new manterPliVM();
	servico = 'PliMercadoria';
	servicoErroProcessamentoAliGrid = 'ErroProcessamentoAliGrid'
	isModificouPesquisa: boolean = false;
	quantidadeErroAli: number;

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
			this.parametros.servico = this.servicoErroProcessamentoAliGrid;
			this.parametros.titulo = "RELATÓRIO DE ANÁLISE DO PLI LISTAGEM DE ERROS"
			this.parametros.width = { 0: { columnWidth: 120 }, 1: { columnWidth: 520 }, 2: { columnWidth: 120 } };
			this.parametros.columns = ["Local do Erro", "Mensagem do Erro", "Origem do Erro"];
			this.parametros.fields = ["localErro", "descricao", "origemErro"];

		}
	}

	retornaValorSessao() {

		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;
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

		this.parametros.idPliMercadoriaOuPliDetalheMercadoria = id
		this.parametros.exportarListagem = false;

		this.applicationService.get(this.servicoErroProcessamentoAliGrid, this.parametros).subscribe((result: PagedItems) => {

			this.isModificouPesquisa = false;
			this.isBuscaSalva = true;
			this.grid.lista = result.items;
			this.grid.total = result.total;

			if (result.total > 0) {
				this.parametros.exportarListagem = true;
				this.quantidadeErroAli = result.total;
			}
			this.gravarBusca();

		});
	}

	gravarBusca() {
		localStorage.removeItem(this.router.url);
		localStorage.setItem(this.router.url, JSON.stringify(this.parametros));
	}

	public verificarRota() {

		this.tituloPanel = 'Relatório de Análise do PLI Listagem de Erros';
		if (this.path == 'relatorio-status-ali') {
			this.tituloPanel = 'Relatório de Análise do PLI Listagem de Erros';
			this.selecionar(this.route.snapshot.params['id']);
		}
	}

	public selecionar(id: number) {
		if (!id) { return; }
		this.applicationService.get<manterPliMercadoriaVM>(this.servico, id).subscribe(result => {
			this.model = result;
			this.listar(id);
		});
	}

	voltar() {
		this._location.back();
	}
}
