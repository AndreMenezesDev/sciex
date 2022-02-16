import { Component, OnInit, Injectable, ViewChild } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router } from '@angular/router';

import * as $ from 'jquery';


@Component({
	selector: 'app-manter-aladi',
	templateUrl: './manter-aladi.component.html'
})

@Injectable()
export class ManterAladiComponent implements OnInit {
	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	servicoAladiGrid = 'AladiGrid';
	@ViewChild('codigo') codigo;
	@ViewChild('descricao') descricao;
	isModificouPesquisa: boolean = false;
	isBuscaSalva: boolean = false;

	public barChartOptions: any = {
		scaleShowVerticalLines: false,
		responsive: true
	};
	public barChartLabels: string[] = ['2006', '2007', '2008', '2009', '2010', '2011', '2012'];
	public barChartType: string = 'bar';
	public barChartLegend: boolean = true;

	public barChartData: any[] = [
		{ data: [65, 59, 80, 81, 56, 55, 40], label: 'Series A' },
		{ data: [28, 48, 40, 19, 86, 27, 90], label: 'Series B' }
	];

	// events
	public chartClicked(e: any): void {
	}

	public chartHovered(e: any): void {
	}

	public randomize(): void {
		// Only Change 3 values
		let data = [
			Math.round(Math.random() * 100),
			59,
			80,
			(Math.random() * 100),
			56,
			(Math.random() * 100),
			40];
		let clone = JSON.parse(JSON.stringify(this.barChartData));
		clone[0].data = data;
		this.barChartData = clone;
		/**
		 * (My guess), for Angular to recognize the change in the dataset
		 * it has to change the dataset variable directly,
		 * so one way around it, is to clone the data, change it and then
		 * assign it;
		 */
	}


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
			this.parametros.servico = this.servicoAladiGrid;
			this.parametros.titulo = "ALADI"
			this.parametros.width = { 0: { columnWidth: 100 }, 1: { columnWidth: 660 } };
			this.parametros.columns = ["Código", "Descrição"];
			this.parametros.fields = ["codigo|formatCode:3", "descricao"];
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
		this.applicationService.get(this.servicoAladiGrid, this.parametros).subscribe((result: PagedItems) => {
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

