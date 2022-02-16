import { Component, OnInit, Injectable, ViewChild } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { viewClassName } from '@angular/compiler';
import { Router } from '@angular/router';

@Component({
	selector: 'app-manter-ncm',
	templateUrl: './manter-ncm.component.html'
})

@Injectable()
export class ManterNcmComponent implements OnInit {
	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	servicoGrid = 'NcmGrid';
	@ViewChild('codigo') codigo;
	@ViewChild('descricao') descricao;
	@ViewChild('ativo') ativo;
	@ViewChild('inativo') inativo;
	@ViewChild('todos') todos;
	@ViewChild('checkbox') checkbox;
	isModificouPesquisa: boolean = false;
	isBuscaSalva: boolean = false;
	carregarGrid: boolean = false;
	formPai = this;


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
			this.parametros.servico = this.servicoGrid;
			this.parametros.titulo = "MANTER NCM";
			this.parametros.width = { 0: { columnWidth: 100 }, 1: { columnWidth: 540 }, 2: { columnWidth: 80 }, 3: { columnWidth: 80 } };
			this.parametros.columns = ["Código", "Descrição","Amazônia Ocidental", "Status"];
			this.parametros.fields = ["codigoNCM", "descricao","isAmazoniaOcidentalString", "status"];
		}
	}

	retornaValorSessao() {
		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;
		}
	}

	buscar(exibirMensagem, grid) {

		if (grid)
			return;

		if (this.codigo.nativeElement.value == "" &&
			this.descricao.nativeElement.value == "" &&
			(this.ativo.nativeElement.checked != true && this.inativo.nativeElement.checked != true && this.todos.nativeElement.checked != true)) {

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
		this.buscar(false, (this.grid.lista == undefined || this.grid.lista.lenght == 0));
	}

	limpar() {

		this.codigo.nativeElement.value = "";
		this.descricao.nativeElement.value = "";
		this.checkbox.nativeElement.checked = false;
		this.todos.nativeElement.checked = true;
		this.ativo.nativeElement.checked = false;
		this.inativo.nativeElement.checked = false;

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
				this.parametros.CodigoNCM = "";
			else {
				this.parametros.CodigoNCM = this.removerCaractere(this.codigo.nativeElement.value);;
			}

			if (this.descricao.nativeElement.value == "")
				this.parametros.Descricao = "";
			else
				this.parametros.Descricao = this.descricao.nativeElement.value;

			if (this.ativo.nativeElement.checked == false && this.inativo.nativeElement.checked == false) {
				this.parametros.status = 2;
			} else if (this.ativo.nativeElement.checked == true && this.inativo.nativeElement.checked == false) {
				this.parametros.status = 1;
			} else {
				this.parametros.status = 0;
			}

			if (this.checkbox.nativeElement.checked == true){
				this.parametros.checkboxSomenteAmazoniaOcidental = true;
			}else{
				this.parametros.checkboxSomenteAmazoniaOcidental = false;
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
		this.applicationService.get(this.servicoGrid, this.parametros).subscribe((result: PagedItems) => {
			this.isModificouPesquisa = false;
			this.isBuscaSalva = true;
			if (result.total > 0) {
				this.parametros.exportarListagem = true;
			}
			this.grid.lista = result.items;
			this.grid.total = result.total;
			this.gravarBusca();

		});
	}

	gravarBusca() {
		localStorage.removeItem(this.router.url);
		localStorage.setItem(this.router.url, JSON.stringify(this.parametros));
	}

	removerCaractere(documento) {
		var nomeDocumento = "";
		for (var i = 0; i < documento.length; i++) {
			if (documento[i] != "." && documento[i] != "-" && documento[i] != "/") {
				nomeDocumento = nomeDocumento + documento[i];
			}
		}
		return nomeDocumento;
	}
}
