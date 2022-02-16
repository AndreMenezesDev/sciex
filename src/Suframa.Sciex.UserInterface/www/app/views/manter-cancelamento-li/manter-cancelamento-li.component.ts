import { Component, OnInit, Injectable, ViewChild, AfterContentInit } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { ValidationService } from '../../shared/services/validation.service';
import { Router } from '@angular/router';
import { take } from 'rxjs/operator/take';
import { manterLiVM } from '../../view-model/ManterLiVM';
import { ManterCancelamentoLiGridComponent } from './grid/grid.component';

@Component({
	selector: 'app-manter-cancelamento-li',
	templateUrl: './manter-cancelamento-li.component.html',
	providers: [ManterCancelamentoLiGridComponent]
})

@Injectable()
export class ManterCancelamentoLiComponent implements OnInit {
	grid: any = { sort: {} };
	parametros: any = {};
	parametros1: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	servicoLiGrid = 'LiGrid';
	servicoLi = 'Li';
	@ViewChild('npli') NumeroPLI;
	@ViewChild('nli') NumeroLI;
	@ViewChild('descricao') descricao;
	@ViewChild('formBusca') formBusca;
	@ViewChild('btnlimpar') btnlimpar;

	model: manterLiVM = new manterLiVM();

	constructor(
		private applicationService: ApplicationService,
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
		private gridLI: ManterCancelamentoLiGridComponent
	) {
	}

	ngOnInit(): void {

	}

	buscar() {

		if (!this.validationService.form('formBusca')) { return; }
		if (!this.formBusca.valid) { return; }


		if (!this.NumeroPLI.nativeElement.value
			&& !this.NumeroLI.nativeElement.value
		) {

			this.modal.alerta(this.msg.INFORME_OS_FILTROS_DE_PESQUISA, 'Informação');

		}
		else {
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
		this.buscar();
	}

	limpar() {
		this.NumeroPLI.nativeElement.value = "";
		this.NumeroLI.nativeElement.value = "";

		this.parametros1.NumeroLi = null;
		this.parametros1.NumeroPli = null;
	}

	listar() {

		this.limparDadosGrid();

		if (this.NumeroPLI.nativeElement.value == "") {
			this.parametros.NumeroPli = -1;
			this.parametros.AnoPli = -1;
		}
		else {
			this.parametros.AnoPli = this.NumeroPLI.nativeElement.value.split("/")[0];
			this.parametros.NumeroPli = +this.NumeroPLI.nativeElement.value.split("/")[1];
		}

		if (this.NumeroLI.nativeElement.value == "") {
			this.parametros.NumeroLi = 0;
		}
		else {
			this.parametros.NumeroLi = this.NumeroLI.nativeElement.value;
		}

		// Recuperar dados do localStorage
		if (this.parametros.page != this.grid.page)
			this.parametros.page = this.grid.page;
		else
			this.grid.page = this.parametros.page;


		// Regra de negocio limite maximo de registro na grid 400
		this.parametros.size = 400;
		this.grid.size = 400;


		if (this.grid.sort.field != this.parametros.sort)
			this.parametros.sort = this.grid.sort.field;
		else
			this.grid.sort.field = this.parametros.sort;

		if (this.grid.sort.reverse != this.parametros.reverse)
			this.parametros.reverse = this.grid.sort.reverse;
		else
			this.grid.sort.reverse = this.parametros.reverse;

		this.applicationService.get(this.servicoLiGrid, this.parametros).subscribe((result: PagedItems) => {

			this.grid.lista = result.items;
			this.grid.total = result.total;

		});
	}

	cancelarLi() {

		if (this.grid.lista != undefined) {

			this.model.listaSelecionados = new Array<number>();

			for (var i = 0; i < this.grid.lista.length; i++) {

				if (this.grid.lista[i].checkbox) {
					this.model.listaSelecionados.push(this.grid.lista[i].idPliMercadoria);
				}
			}

			if (this.model.listaSelecionados.length == 0) {
				this.modal.alerta("Nenhum LI selecionado para serem cancelados.", 'Informação');
			}
			else {
				this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
					.subscribe(isConfirmado => {
						if (isConfirmado) {

							this.salvarRegistro();
						}
					});
			}
		}
	}

	private salvarRegistro() {
		this.applicationService.put<manterLiVM>(this.servicoLi, this.model).subscribe(result => {
			this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "");
			this.limpar();
			this.limparDadosGrid();
		});

	}

	private limparDadosGrid() {
		this.grid.lista = null;
		this.grid.total = 0;		

	}
}

