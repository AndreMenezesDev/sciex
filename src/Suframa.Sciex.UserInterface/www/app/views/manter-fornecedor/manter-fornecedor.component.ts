import { Component, OnInit, Injectable, Input, ViewChild } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router } from '@angular/router';

@Component({
	selector: 'app-manter-fornecedor',
	templateUrl: './manter-fornecedor.component.html'
})

@Injectable()
export class ManterFornecedorComponent implements OnInit {
	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	servicoFornecedorGrid = 'FornecedorGrid';
	@ViewChild('idFornecedor') idFornecedor;
	@ViewChild('codigo') codigo;
	@ViewChild('razaoSocial') razaoSocial;
	isBuscaSalva: boolean = false;
	isModificouPesquisa: boolean = false;
	CNPJ: string;

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

			if (this.parametros.idFornecedor != "-1")
				localStorage.removeItem(this.router.url);
			this.listar();
		}
		else {
			this.parametros = {};
			this.parametros.titulo = "FORNECEDOR"
			this.parametros.width = { 0: { columnWidth: 80 }, 1: { columnWidth: 300 }, 2: { columnWidth: 100 }, 3: { columnWidth: 120 }, 4: { columnWidth: 160 } };
			this.parametros.servico = this.servicoFornecedorGrid;
			this.parametros.columns = ["Código", "Razão Social", "Cidade", "Estado", "País"];
			this.parametros.fields = ["codigo", "razaoSocial", "cidade", "estado", "descricaoPais"];
		}


		this.applicationService.get("UsuarioLogado", { cnpj: true }).subscribe((result: any) => {
			if (result != null) {
				this.CNPJ = result;
				this.parametros.CNPJImportador = this.CNPJ;
			}
		});		
	}

	retornaValorSessao() {

		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;
		}
	}

	buscar(exibirMensagem) {

		if (this.codigo.nativeElement.value == "" && this.razaoSocial.nativeElement.value == "") {
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
		this.razaoSocial.nativeElement.value = this.codigo.nativeElement.value = "";
		this.codigo.nativeElement.value = "";
	}

	validarCodigo(valor) {
		
		return true;
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
				this.parametros.codigo = Number(this.codigo.nativeElement.value);

			if (this.razaoSocial.nativeElement.value == "")
				this.parametros.razaoSocial = "";
			else
				this.parametros.razaoSocial = this.razaoSocial.nativeElement.value;

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
		this.applicationService.get(this.servicoFornecedorGrid, this.parametros).subscribe((result: PagedItems) => {
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
}
