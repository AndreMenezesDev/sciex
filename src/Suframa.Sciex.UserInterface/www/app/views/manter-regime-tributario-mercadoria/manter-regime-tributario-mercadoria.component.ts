import { Component, OnInit, Injectable, ViewChild } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { viewClassName } from '@angular/compiler';
import { Router } from '@angular/router';
import { ValidationService } from "../../shared/services/validation.service";

@Component({
	selector: 'app-manter-regime-tributario-mercadoria',
	templateUrl: './manter-regime-tributario-mercadoria.component.html'
})

@Injectable()
export class ManterRegimeTributarioMercadoriaComponent implements OnInit {
	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	servicoGrid = 'RegimeTributarioMercadoriaGrid';
	@ViewChild('ativo') ativo;
	@ViewChild('inativo') inativo;
	@ViewChild('todos') todos;
	@ViewChild('dataInicio') dataInicio;
	@ViewChild('dataFim') dataFim;
	@ViewChild('codigoMunicipio') codigoMunicipio;
	@ViewChild('fundamentoLegal') fundamentoLegal;
	@ViewChild('formBusca') formBusca;
	uf: string;

	isModificouPesquisa: boolean = false;
	isBuscaSalva: boolean = false;
	carregarGrid: boolean = false;

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
		private validationService: ValidationService
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
			this.parametros.titulo = "MANTER REGIME TRIBUTÁRIO DA MERCADORIA";
			this.parametros.width = { 0: { columnWidth: 120 }, 1: { columnWidth: 40 }, 2: { columnWidth: 100 }, 3: { columnWidth: 340 }, 4: { columnWidth: 80 }, 5: { columnWidth: 80 } };
			this.parametros.columns = ["Município", "UF", "Regime Tributário", "Fundamento Legal","Vigência","Status"];
			this.parametros.fields = ["codigoDescricaoMunicipio", "uf", "codigoDescricaoRegimeTributario", "codigoDescricaoFundamentoLegal","dataVigenciaFormatado","status"];
		}
	}

	retornaValorSessao() {
		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;
		}
	}


	validarData() {

			try {
				this.dataInicio.nativeElement.setCustomValidity('');
				this.dataFim.nativeElement.setCustomValidity('');

				var dataFim = new Date(this.parametros.dataFim);
				var dataInicio = new Date(this.parametros.dataInicio);


				if (this.dataInicio.nativeElement.value.length > 0 || this.dataFim.nativeElement.value.length > 0) {
					if (this.dataInicio.nativeElement.value.length >= 0 && this.dataInicio.nativeElement.value.length < 10) {
						this.dataInicio.nativeElement.setCustomValidity('Campo inválido');
					} else if (this.dataFim.nativeElement.value.length >= 0 && this.dataFim.nativeElement.value.length < 10) {
						this.dataFim.nativeElement.setCustomValidity('Campo inválido');

					} else if (new Date(this.parametros.dataFim) < new Date(this.parametros.dataInicio)) {
						this.dataFim.nativeElement.setCustomValidity('Campo inválido. Data inicio maior que data final');
					}
				}
			} catch (e) {

				this.dataInicio.nativeElement.setCustomValidity('Informe uma data válida.');
			}

		}


	buscar(exibirMensagem) {
		
		this.validarData();
		if (!this.validationService.form('formBusca')) { return; }

		if (this.parametros.codigoMunicipio == undefined &&
			this.parametros.uf == undefined &&
			this.parametros.idRegimeTributario == undefined &&
			this.parametros.idFundamentoLegal == undefined &&
			this.dataInicio.nativeElement.value == 0 &&
			this.dataFim.nativeElement.value == 0 &&
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
		this.codigoMunicipio.clearInput = true;
		this.fundamentoLegal.clearInput = true;
		let element: HTMLElement = document.getElementById('valorInput') as HTMLElement;
		element.click();
		this.parametros = {};
		this.codigoMunicipio.clearInput = false;
		this.fundamentoLegal.clearInput = false;
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

			if (this.parametros.codigoMunicipio == undefined || this.parametros.codigoMunicipio == "")
				this.parametros.codigoMunicipio = -1;
			else
				this.parametros.codigoMunicipio = this.parametros.codigoMunicipio;

			if (this.parametros.uf == undefined || this.parametros.uf == "")
				this.parametros.uf = "";
			else
				this.parametros.uf = this.parametros.uf;

			if (this.parametros.idRegimeTributario == undefined || this.parametros.idRegimeTributario == "")
				this.parametros.idRegimeTributario = 0;
			else
				this.parametros.idRegimeTributario = this.parametros.idRegimeTributario;

			if (this.parametros.idFundamentoLegal == undefined || this.parametros.idFundamentoLegal == "")
				this.parametros.idFundamentoLegal = 0;
			else
				this.parametros.idFundamentoLegal = this.parametros.idFundamentoLegal;

			if (this.parametros.descricaoFundamentoLegal == undefined || this.parametros.descricaoFundamentoLegal == null)
				this.parametros.decricaoFundamentoLegal = ""
			else
				this.parametros.descricaoFundamentoLegal = this.parametros.descricaoFundamentoLegal;

			if (this.ativo.nativeElement.checked == false && this.inativo.nativeElement.checked == false) {
				this.parametros.status = 2;
			} else if (this.ativo.nativeElement.checked == true && this.inativo.nativeElement.checked == false) {
				this.parametros.status = 1;
			} else {
				this.parametros.status = 0;
			}
			// if (this.fundamentoLegal.nativeElement.value == undefined) {
			// 	this.parametros.descricaoFundamentoLegal = "";
			// } else {
			// 	this.parametros.descricaoFundamentoLegal = this.fundamentoLegal.nativeElement.value;
			//}
			if(!this.fundamentoLegal.valorSelecionado){
				this.parametros.descricaoFundamentoLegal = "";
			} else{
				this.parametros.descricaoFundamentoLegal = this.fundamentoLegal.valorInput.nativeElement.value.split("|")[1];
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
}
