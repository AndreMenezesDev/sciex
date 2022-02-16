import { Component, OnInit, Injectable, ViewChild} from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { viewClassName } from '@angular/compiler';
import { Router } from '@angular/router';

@Component({
    selector: 'app-manter-codigo-utilizacao',
    templateUrl: './manter-codigo-utilizacao.component.html'
})

@Injectable()
export class ManterCodigoUtilizacaoComponent implements OnInit {
    grid: any = { sort: {} };
    parametros: any = {};
    ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	servicoCodigoUtilizacaoGrid = 'CodigoUtilizacaoGrid';
	@ViewChild('codigo') codigo;
	@ViewChild('descricao') descricao;
	@ViewChild('ativo') ativo;
	@ViewChild('todos') todos;
	@ViewChild('inativo') inativo;
	isModificouPesquisa: boolean = false;
	isBuscaSalva: boolean = false;
	carregarGrid: boolean = false;

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
			this.parametros.servico = this.servicoCodigoUtilizacaoGrid;
			this.parametros.titulo = "MANTER CÓDIGO UTILIZAÇÃO";
			this.parametros.width = { 0: { columnWidth: 80 }, 1: { columnWidth: 600 }, 2: { columnWidth: 80 } };
			this.parametros.columns = ["Código", "Descrição", "Status"];
			this.parametros.fields = ["codigo", "descricao", "status"];
		}
    }

	retornaValorSessao() {
		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;
		}
	}

	buscar(exibirMensagem) {
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
		this.buscar(false);
    }

	limpar() {
		//this.parametros = {};
		this.descricao.nativeElement.value = this.codigo.nativeElement.value = "";
		this.codigo.nativeElement.value = "";
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

			if (this.ativo.nativeElement.checked == false && this.inativo.nativeElement.checked == false) {
				this.parametros.status = 2;
			} else if (this.ativo.nativeElement.checked == true && this.inativo.nativeElement.checked == false) {
				this.parametros.status = 1;
			} else {
				this.parametros.status = 0;
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
		this.applicationService.get(this.servicoCodigoUtilizacaoGrid, this.parametros).subscribe((result: PagedItems) => {
			this.grid.lista = result.items;
			this.grid.total = result.total;

			this.isModificouPesquisa = false;
			this.isBuscaSalva = true;

			if (result.total > 0) {
				this.parametros.exportarListagem = true;
				this.parametros.sort = this.grid.sort;
			}

			this.gravarBusca();
        });
    }

	gravarBusca() {
		localStorage.removeItem(this.router.url);
		localStorage.setItem(this.router.url, JSON.stringify(this.parametros));
	}
}
