import { Component, OnInit, Injectable, ViewChild } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { viewClassName } from '@angular/compiler';
import { Router } from '@angular/router';

@Component({
	selector: 'app-manter-controle-importacao',
	templateUrl: './manter-controle-importacao.component.html'
})

@Injectable()
export class ManterControleImportacaoComponent implements OnInit {
	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	servicoGrid = 'ControleImportacaoGrid';
	@ViewChild('aplicacao') aplicacao;
	@ViewChild('setor') setor;
	@ViewChild('codigoconta') codigoconta;
	@ViewChild('codigoutilizacao') codigoutilizacao;
	@ViewChild('ativo') ativo;
	@ViewChild('inativo') inativo;
	@ViewChild('todos') todos;
	@ViewChild('formBusca') formBusca;
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
			//this.parametros.descricaoCodigoConta = this.parametros.descricaoCodigoConta + this.parametros.codigoCodigoConta.toString();
			this.parametros = {};
			this.parametros.servico = this.servicoGrid;
			this.parametros.titulo = "MANTER CONTROLE IMPORTAÇÃO";
			this.parametros.width = { 0: { columnWidth: 120 }, 1: { columnWidth: 110 }, 2: { columnWidth: 300 }, 3: { columnWidth: 160 }, 4: { columnWidth: 80 }   };
			this.parametros.columns = ["Aplicação", "Setor", "Código Conta", "Código Utilização", "Status"];
			this.parametros.fields = ["descricaoPliAplicacao", "descricaoSetor", "descricaoCodigoConta", "descricaoCodigoUtilizacao", "status"];
		}
	}

	retornaValorSessao() {
		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;
		}
	}

	buscar(exibirMensagem) {

		if (this.parametros.aplicacao == undefined &&
			this.parametros.codigoSetor == undefined &&
			this.parametros.idCodigoConta == undefined &&
			this.parametros.idCodigoUtilizacao == undefined &&
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
		this.codigoconta.clearInput = true;
		this.codigoutilizacao.clearInput = true;
		let element: HTMLElement = document.getElementById('valorInput') as HTMLElement;
		element.click();
		this.parametros = {};
		this.codigoconta.clearInput = false;
		this.codigoutilizacao.clearInput = false;
		this.inativo.nativeElement.checked = false;
		this.ativo.nativeElement.checked = false;
		this.todos.nativeElement.checked = true;
	}

	//resetField() {
	//	this.parametros.setor = '';
	//}

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

			if (this.parametros.aplicacao == undefined || this.parametros.aplicacao == "")
				this.parametros.aplicacao = -1;
			else
				this.parametros.aplicacao = this.parametros.aplicacao;

			if (this.parametros.setor == undefined || this.parametros.setor == "")
				this.parametros.setor = -1;
			else
				this.parametros.setor = this.parametros.setor;

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
		
		this.applicationService.get(this.servicoGrid, this.parametros).subscribe((result: PagedItems) => {			
			this.isModificouPesquisa = false;
			this.isBuscaSalva = true;
			if (result.total > 0) {
				this.parametros.exportarListagem = true;
			}
			this.grid.lista = result.items;
			this.grid.total = result.total;

			//if (this.parametros.aplicacao == undefined && this.parametros.setor == undefined) {
			//	this.formBusca.reset();
			//} else if (this.parametros.aplicacao != undefined && this.parametros.setor == undefined) {
			//	this.resetField();
			//} else {
			//	this.formBusca.reset();
			//}

			this.gravarBusca();
		});
	}

	//getSelectedOptionTextAplicacao(event: Event) {
	//	let selectedOptions = event.target['options'];
	//	let selectedIndex = selectedOptions.selectedIndex;
	//	let selectElementText = selectedOptions[selectedIndex].text;
	//	this.parametros.aplicacao = selectElementText;
	//}

	getSelectedOptionTextSetor(event: Event) {
		let selectedOptions = event.target['options'];
		let selectedIndex = selectedOptions.selectedIndex;
		let selectElementText = selectedOptions[selectedIndex].text;
		this.parametros.setor = selectElementText;
	}

	gravarBusca() {
		localStorage.removeItem(this.router.url);
		localStorage.setItem(this.router.url, JSON.stringify(this.parametros));
	}
}
