import { Component, OnInit, Injectable, ViewChild } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { viewClassName } from '@angular/compiler';
import { Router } from '@angular/router';

@Component({
	selector: 'app-manter-fundamento-legal',
	templateUrl: './manter-fundamento-legal.component.html'
})

@Injectable()
export class ManterFundamentoLegalComponent implements OnInit {
	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	servicoFundamentoLegalGrid = 'FundamentoLegalGrid';
	@ViewChild('codigo') codigo;
	@ViewChild('descricao') descricao;
	@ViewChild('tipoAreaBeneficio') tipoAreaBeneficio;
	isModificouPesquisa: boolean = false;
	isBuscaSalva: boolean = false;
	carregarGrid: boolean = false;
	listaBeneficios = [];

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router
	) {
		if (localStorage.getItem(this.router.url) == null && localStorage.length > 0) {
			localStorage.clear();
		}
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
			this.listar();
		}
		else {
			this.parametros = {};
			this.parametros.servico = this.servicoFundamentoLegalGrid;
			this.parametros.titulo = "MANTER FUNDAMENTO LEGAL";
			this.parametros.columns = ["Código", "Descrição", "Área de Benefício"];
			this.parametros.width = { 0: { columnWidth: 50 }, 1: { columnWidth: 540 }, 2: { columnWidth: 170}  };
			this.parametros.fields = ["codigo|formatCode:2", "descricao", "descricaoArea"];

			//this.parametros.columns = ["Código", "Descrição"];
			//this.parametros.fields = ["codigo|formatCode:2","descricao"];
		}

		this.inicializarDadosAreabeneficios();
	}

	retornaValorSessao() {

		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;
		}
	}

	buscar(exibirMensagem) {

		if (this.codigo.nativeElement.value == "" && this.descricao.nativeElement.value == "" && (this.tipoAreaBeneficio.nativeElement.value == "0")) {
			if (exibirMensagem) {
				this.modal.alerta(this.msg.INFORME_OS_FILTROS_DE_PESQUISA, 'Informação');
			} else

				if (this.isBuscaSalva) {
					this.listar();
				}
		}
		else {

			if (exibirMensagem) {
				this.isModificouPesquisa = true;
			}
			else {
				this.isBuscaSalva = true;
			}
			this.listar();
		}
	}

	ocultar() {
		if (this.ocultarFiltro === false)
			this.ocultarFiltro = true;
		else
			this.ocultarFiltro = false;
	}

	onChangeSort($event) {
		this.grid.sort = $event;

	}

	onChangeSize($event) {
		this.grid.size = $event;
	}

	onChangePage($event) {
		this.grid.page = $event;
		this.buscar(false);
	}

	limpar() {
		//this.parametros = {};
		this.descricao.nativeElement.value = this.codigo.nativeElement.value = "";
		this.codigo.nativeElement.value = "";
		this.tipoAreaBeneficio.nativeElement.value = "0";
	}

	listar() {

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

			if (this.codigo.nativeElement.value == "")
				this.parametros.codigo = -1;
			else
				this.parametros.codigo = this.codigo.nativeElement.value;

			if (this.descricao.nativeElement.value == "")
				this.parametros.descricao = "";
			else
				this.parametros.descricao = this.descricao.nativeElement.value;

			if (this.tipoAreaBeneficio.nativeElement.value == "" || this.tipoAreaBeneficio.nativeElement.value == "0") {
				this.parametros.tipoAreaBeneficio = null;
			} else {
				this.parametros.tipoAreaBeneficio = this.tipoAreaBeneficio.nativeElement.value;;
			}
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

		this.parametros.exportarListagem = false;
		this.applicationService.get(this.servicoFundamentoLegalGrid, this.parametros).subscribe((result: PagedItems) => {
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

	gravarBusca() {
		localStorage.removeItem(this.router.url);
		localStorage.setItem(this.router.url, JSON.stringify(this.parametros));
	}

	inicializarDadosAreabeneficios() {
		this.listaBeneficios = [
			{ idAreaBeneficio: 0, descricao: "Selecione uma opção" },
			{ idAreaBeneficio: 1, descricao: "ZFM – Zona Franca de Manaus" },
			{ idAreaBeneficio: 2, descricao: "ALC – Área de Livre Comércio" },
			{ idAreaBeneficio: 3, descricao: "AO – Amazônia Ocidental" }
		];
	}


}

