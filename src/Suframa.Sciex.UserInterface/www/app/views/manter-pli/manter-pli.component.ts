import { Component, OnInit, Injectable, ViewChild, AfterContentInit } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { ValidationService } from '../../shared/services/validation.service';
import { Router } from '@angular/router';
import { AuthGuard } from '../../shared/guards/auth-guard.service';
import { manterPliVM } from '../../view-model/ManterPliVM';
import { take } from 'rxjs/operator/take';

@Component({
	selector: 'app-manter-pli',
	templateUrl: './manter-pli.component.html'
})

@Injectable()
export class ManterPliComponent implements OnInit {
	grid: any = { sort: {} };
	parametros: any = {};
	parametros1: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	servicoPliGrid = 'PliGrid';
	@ViewChild('npli') NumeroPLI;
	@ViewChild('descricao') descricao;
	@ViewChild('dataInicio') dataInicio;
    @ViewChild('dataFim') dataFim;
    @ViewChild('tipoPLI') tipoPLI;
	@ViewChild('aplicacaoPLI') aplicacaoPLI;
	@ViewChild('statusPLI') statusPLI;
	@ViewChild('formBusca') formBusca;
	@ViewChild('appModalNovoPli') appModalNovoPli;
	@ViewChild('btnlimpar') btnlimpar;
	isModificouPesquisa: boolean = false;
	isBuscaSalva: boolean = false;
	dataValida = false;


	constructor(
		private applicationService: ApplicationService,
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
		private authguard: AuthGuard,
	) {
		if (  localStorage.getItem(this.router.url) == null	&& localStorage.length > 0) {
			localStorage.clear();
		}

		this.parametros.isClear = false;
		this.parametros.exportarListagem = false;

	}

	ngOnInit(): void {

		this.authguard.active = false;
		this.retornaValorSessao();

		if (this.parametros != undefined || this.parametros != null) {

			this.grid.page = this.parametros.page;
			this.grid.size = this.parametros.size;
			this.grid.sort.field = this.parametros.sort;
			this.grid.sort.reverse = this.parametros.reverse;

			if (this.parametros.isClear)
				this.limpar();

			if (this.parametros.NumeroPli != "-1")
				localStorage.removeItem(this.router.url);
			this.listar();
		}
		else {
			this.parametros = {};

            this.parametros.idPliAplicacao = 0;
            this.parametros.tipoDocumento = 0;
			this.parametros.statusPli = 0;
			this.parametros.NumeroPli = -1;
			this.parametros.Ano = -1;

			this.parametros.titulo = "MANTER PLI"
			this.parametros.width = { 0: { columnWidth: 150 }, 1: { columnWidth: 200 }, 2: { columnWidth: 150 }, 3: { columnWidth: 150 }, 4: { columnWidth: 260 } };
			this.parametros.servico = this.servicoPliGrid;
			this.parametros.columns = ["Nº PLI", "Tipo de Aplicação","Tipo de PLI","Data de Cadastro", "Status"];
			this.parametros.fields = ["numeroPliConcatenado", "descricaoAplicacao", "descricaoTipoDocumento", "dataPliFormatada", "descricaoStatus"];

			let dat = new Date();
			this.parametros.dataInicio = dat.toLocaleDateString('en-CA');
			this.parametros.dataFim = dat.toLocaleDateString('en-CA');
		}
	}

	retornaValorSessao() {

		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			if (this.parametros != undefined) {

				if (this.parametros.Numero != undefined || this.parametros.Numero != null)
					this.parametros1.Numero = this.parametros.Numero != undefined ? this.parametros.Numero : null;

				if (this.parametros.idPliAplicacao != undefined || this.parametros.idPliAplicacao != null)
					this.parametros1.idPliAplicacao = this.parametros.idPliAplicacao != undefined ? this.parametros.idPliAplicacao : null;

				if (this.parametros.statusPli != undefined || this.parametros.statusPli != null)
					this.parametros1.statusPli = this.parametros.statusPli != null ? this.parametros.statusPli : null;

				if (this.parametros.DataInicio != undefined || this.parametros.DataInicio != null)
					this.parametros1.DataInicio = this.parametros.DataInicio != undefined ? this.parametros.statusPli : null;

				if (this.parametros.DataFim != undefined || this.parametros.DataFim != null)
					this.parametros1.DataFim = this.parametros.DataFim != undefined ? this.parametros.statusPli : null;
			}
			this.isBuscaSalva = true;
		}
	}

	buscar(exibirMensagem) {
		this.validarData();
		if (!this.validationService.form('formBusca')) { return; }
		if (!this.formBusca.valid) { return; }

		if (!this.NumeroPLI.nativeElement.value
            && (!this.aplicacaoPLI.select.nativeElement.value || this.aplicacaoPLI.select.nativeElement.value == "0")
            && (!this.tipoPLI.select.nativeElement.value || this.tipoPLI.select.nativeElement.value == "0")
			&& (!this.statusPLI.select.nativeElement.value || this.statusPLI.select.nativeElement.value == "0")
			&& !this.dataInicio.nativeElement.value
			&& !this.dataFim.nativeElement.value
		) {
			if (exibirMensagem) {
				this.modal.alerta(this.msg.INFORME_OS_FILTROS_DE_PESQUISA, 'Informação');
			} else {
				if (!this.validationService.form('formBusca')) {
					return;
				}
				if (!this.formBusca.valid) {
					return;
				}

				if (this.isBuscaSalva) {
					this.listar();
				}
			}
		}
		else {
			if (!this.validationService.form('formBusca')) { return; }
			if (!this.formBusca.valid) { return; }

			if (exibirMensagem) {
				this.isModificouPesquisa = true;
			}
			else {
				this.isModificouPesquisa = false;
				this.isBuscaSalva = true;
			}
			this.listar();
		}
	}

	validarData() {

		try {
			this.dataInicio.nativeElement.setCustomValidity('');
			this.dataFim.nativeElement.setCustomValidity('');

			var dataFim = new Date(this.parametros.dataFim);
			var dataInicio = new Date(this.parametros.dataInicio);

			var dias = ((dataFim.getTime() - dataInicio.getTime()) / 86400000);
			if (dias > 30) {
				this.dataInicio.nativeElement.setCustomValidity('Período não pode ser maior que 30 dias');
			}
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
		this.parametros.isClear = true;
		this.gravarBusca();
		this.dataInicio.nativeElement.value = this.dataFim.nativeElement.value = "";
		this.NumeroPLI.nativeElement.value = "";
		this.aplicacaoPLI.clear();
		this.statusPLI.clear();
		this.aplicacaoPLI.clear();
		this.statusPLI.clear();
		this.tipoPLI.clear();
		this.parametros1.Numero = null;
		this.parametros1.idPliAplicacao = null;
		this.parametros1.statusPli = null;
		this.parametros1.DataInicio = null;
		this.parametros1.DataFim = null;

		this.parametros.idPliAplicacao = 0;
		this.parametros.statusPli = 0;
		this.parametros.NumeroPli = -1;
		this.parametros.Ano = -1;
		this.parametros.dataFim = null;
		this.parametros.dataInicio = null;


		this.dataInicio.nativeElement.setCustomValidity('');
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

			this.parametros.isClear = false;

			if (this.NumeroPLI.nativeElement.value == "") {
				this.parametros.NumeroPli = -1;
				this.parametros.Ano = -1;
			}
			else {
				this.parametros.Ano = this.NumeroPLI.nativeElement.value.split("/")[0];
				this.parametros.NumeroPli = +this.NumeroPLI.nativeElement.value.split("/")[1];
			}

			if (this.dataInicio.nativeElement.value == "" && this.dataFim.nativeElement.value == null) {
				this.parametros.dataFim  = this.parametros.dataInicio = null;
			}
			else {
				this.parametros.dataInicio = this.dataInicio.nativeElement.value;
				this.parametros.dataFim = this.dataFim.nativeElement.value;
			}


			if (this.aplicacaoPLI.select.nativeElement.value == "") {
				this.parametros.idPliAplicacao = 0;
			}
			else {
				this.parametros.idPliAplicacao = this.aplicacaoPLI.select.nativeElement.value;
			}

			if (this.statusPLI.select.nativeElement.value == "") {
				this.parametros.statusPli = 0;
			}
			else {
				this.parametros.statusPli = this.statusPLI.select.nativeElement.value;
			}

		}
		else {

			// Recuperar dados do localStorage
			if (this.parametros.Numero != undefined && this.parametros.Numero != null && this.parametros.Numero != "") {
				this.NumeroPLI.nativeElement.value = this.parametros.Numero;
				this.parametros.Ano = this.parametros.Numero.split("/")[0];
				this.parametros.NumeroPli = this.parametros.Numero.split("/")[1];
			}

			if (this.parametros.dataInicio != undefined && this.parametros.dataInicio != null) {
				this.dataInicio.nativeElement.value = this.parametros.dataInicio;
			}

			if (this.parametros.dataFim != undefined && this.parametros.dataFim != null) {
				this.dataFim.nativeElement.value = this.parametros.dataFim;
			}

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

		this.applicationService.get(this.servicoPliGrid, this.parametros).subscribe((result: PagedItems) => {
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
		this.parametros.DataInicio = this.parametros1.DataInicio;
		this.parametros.DataFim = this.parametros1.DataFim;

		if (this.NumeroPLI.nativeElement.value == "") {
			this.parametros.NumeroPli = -1;
			this.parametros.Ano = -1;
			this.parametros.Numero = "";
		}
		else {
			this.parametros.Ano = this.NumeroPLI.nativeElement.value.split("/")[0];
			this.parametros.NumeroPli = +this.NumeroPLI.nativeElement.value.split("/")[1];
			this.parametros.Numero = this.NumeroPLI.nativeElement.value;
		}

		if (this.dataInicio.nativeElement.value == "" && this.dataFim.nativeElement.value == null) {
			this.parametros.dataFim = this.parametros.dataInicio = null;
		}
		else {
			this.parametros.dataInicio = this.dataInicio.nativeElement.value;
			this.parametros.dataFim = this.dataFim.nativeElement.value;
		}

		if (this.aplicacaoPLI.select.nativeElement.value == "") {
			this.parametros.idPliAplicacao = 0;
		}
		else {
			this.parametros.idPliAplicacao = this.aplicacaoPLI.select.nativeElement.value;
		}

		if (this.statusPLI.select.nativeElement.value == "") {
			this.parametros.statusPli = 0;
		}
		else {
			this.parametros.statusPli = this.statusPLI.select.nativeElement.value;
		}

		localStorage.removeItem(this.router.url);
		localStorage.setItem(this.router.url, JSON.stringify(this.parametros));
	}

	cadastrar() {
		this.appModalNovoPli.abrir();
	}

}

