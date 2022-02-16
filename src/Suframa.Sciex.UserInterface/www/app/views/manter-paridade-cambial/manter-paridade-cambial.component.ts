import { Component, OnInit, Injectable, ViewChild, EventEmitter, Output } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { manterParidadeCambialVM } from '../../view-model/ManterParidadeCambialVM';
import { Router } from '@angular/router';


@Component({
	selector: 'app-manter-paridade-cambial',
	templateUrl: './manter-paridade-cambial.component.html'
})

@Injectable()
export class ManterParidadeCambialComponent implements OnInit {

	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	servicoParidadeCambialGrid = 'ParidadeCambialGrid';
	paisSelecionado: string;
	isModificouPesquisa: boolean = false;
	isBuscaSalva: boolean = false;
	
	@ViewChild('moeda') moeda;
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
		this.parametros.dataParidade = null;
		this.retornaValorSessao();

		if (this.parametros != undefined || this.parametros != null) {

			this.grid.page = this.parametros.page;
			this.grid.size = this.parametros.size;
			this.grid.sort.field = this.parametros.sort;
			this.grid.sort.reverse = this.parametros.reverse;

			if (this.parametros.dataParidade != "-1")
				localStorage.removeItem(this.router.url);
			this.listar();
		}
		else {
			this.parametros = {};
			this.parametros.servico = this.servicoParidadeCambialGrid;
			this.parametros.titulo = "MANTER PARIDADE CAMBIAL";
			this.parametros.width = { 0: { columnWidth: 170 }, 1: { columnWidth: 100 }, 2: { columnWidth: 90 }, 3: { columnWidth: 110 }, 4: { columnWidth: 80 }, 5: { columnWidth: 210 }};
			this.parametros.columns = ["Moeda", "Valor Paridade R$", "Data Paridade", "Cadastro", "Data Arquivo", "Responsável"];
			this.parametros.fields = ["codDscMoeda", "valor", "dataParidade", "dataCadastro", "dataArquivo", "nomeUsuario"];
		}
	} 

	retornaValorSessao() {
		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;
		}
	}

	buscar(exibirMensagem) {
		
		if (
			!this.parametros.dataParidade) {
			if (exibirMensagem) {
				this.modal.alerta(this.msg.INFORME_OS_FILTROS_DE_PESQUISA, 'Informação');
			} else

				if (this.isBuscaSalva) {
					this.listar();
				}
		} else {
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
		this.moeda.clearInput = true;
		let element: HTMLElement = document.getElementById('valorInput') as HTMLElement;
		element.click();
		this.parametros = {};
		this.moeda.clearInput = false;		
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
		this.applicationService.get(this.servicoParidadeCambialGrid, this.parametros).subscribe((result: PagedItems) => {
			this.grid.lista = result.items;
			this.grid.total = result.total;

			this.isModificouPesquisa = false;
			this.isBuscaSalva = true;

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
