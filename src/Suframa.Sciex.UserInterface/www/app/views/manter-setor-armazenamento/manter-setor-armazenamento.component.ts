import { Component, OnInit, Injectable, ViewChild } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { viewClassName } from '@angular/compiler';
import { Router } from '@angular/router';

@Component({
	selector: 'app-manter-setor-armazenamento',
	templateUrl: './manter-setor-armazenamento.component.html'
})

@Injectable()

export class ManterSetorArmazenamentoComponent implements OnInit {

	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	isModificouPesquisa: boolean = false;
	isBuscaSalva: boolean = false;
	carregarGrid: boolean = false;
	
	servico = "SetorArmazenamentoGrid";	

	@ViewChild('codigo') codigo;
	@ViewChild('descricao') descricao;
	@ViewChild('todos') todos;
	@ViewChild('ativo') ativo;
	@ViewChild('inativo') inativo;

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

			if (this.parametros.codigo != "-1"){				
				localStorage.removeItem(this.router.url);
			} else{
				this.parametros.codigo = null;
			}			
			
			this.listar();	
		}
		else {
		
			this.parametros = {};
			this.parametros.exportarListagem = false;
			this.parametros.servico = this.servico;
			this.parametros.titulo = "MANTER RECINTO SETOR ARMAZENAMENTO";
			this.parametros.width = { 0: { columnWidth: 100 }, 1: { columnWidth: 100 }, 2: { columnWidth: 680 } };
			this.parametros.columns = ["Código Recinto", "Código Setor", "Descrição"];
			this.parametros.fields = ["codigoRecintoAlfandega" , "codigo", "descricao"];
		}

	}

	retornaValorSessao() {

		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;
		}
	}

	buscar(exibirMensagem) {

		if (this.codigo.nativeElement.value == "" && this.descricao.nativeElement.value == "") {
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
		this.parametros = {};
		this.descricao.nativeElement.value = this.codigo.nativeElement.value = "";
		this.codigo.nativeElement.value = "";
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
				this.parametros.codigo = -1;
			else
				this.parametros.codigo = this.codigo.nativeElement.value;

			if (this.descricao.nativeElement.value == "")
				this.parametros.descricao = "";
			else
				this.parametros.descricao = this.descricao.nativeElement.value;
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

		this.codigo.nativeElement.value ? 
			this.parametros.codigo = Number(this.codigo.nativeElement.value) : 
				this.parametros.codigo = -1;		

		if (this.ativo.nativeElement.checked == false && this.inativo.nativeElement.checked == false) {
			this.parametros.status = 2;
		} else if (this.ativo.nativeElement.checked == true && this.inativo.nativeElement.checked == false) {
			this.parametros.status = 1;
		} else {
			this.parametros.status = 0
		}

		this.parametros.idRecintoAlfandega ? 
			this.parametros.idRecintoAlfandega = Number(this.parametros.idRecintoAlfandega) : 
				'';
		
		this.parametros.servico = this.servico;

		this.applicationService.get(this.servico, this.parametros).subscribe((result: PagedItems) => {
			if(result.total > 0){				
				this.isModificouPesquisa = false;
				this.isBuscaSalva = true;
				this.grid.lista = result.items;
				this.grid.total = result.total;			 	
				this.gravarBusca();

				this.parametros.codigo == -1 ? this.codigo.nativeElement.value = "" : '';

				this.parametros.width = { 0: { columnWidth: 100 }, 1: { columnWidth: 100 }, 2: { columnWidth: 680 } };
				this.parametros.columns = ["Código Recinto", "Código Setor", "Descrição"];
				this.parametros.fields = ["codigoRecintoAlfandega" , "codigo", "descricao"];
				this.parametros.exportarListagem = true;
			} else {
				this.parametros.exportarListagem = false;
				this.grid = { sort: {} };
			}			
		});
	}

	gravarBusca() {
		localStorage.removeItem(this.router.url);
		localStorage.setItem(this.router.url, JSON.stringify(this.parametros));
    }
    
    getSelectedOptionTextSetor(event){
        let selectedOptions = event.target['options'];
		let selectedIndex = selectedOptions.selectedIndex;
		let selectElementText = selectedOptions[selectedIndex].text;
		this.parametros.codigoRecintoAlfandega = Number(selectElementText.substr(0, 6));
    }


}

