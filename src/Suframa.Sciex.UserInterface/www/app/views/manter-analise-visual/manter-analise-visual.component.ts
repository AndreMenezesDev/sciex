import { Component, OnInit, Injectable, ViewChild, Output, EventEmitter, AfterViewInit } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router } from '@angular/router';
import { ValidationService } from "../../shared/services/validation.service";
import { manterPliVM } from '../../view-model/ManterPliVM';
import { forEach } from '@angular/router/src/utils/collection';
import { ManterAnaliseVisualGridComponent } from './grid/grid.component';

import { EnumPerfil } from '../../shared/enums/EnumPerfil';

@Component({
	selector: 'app-manter-analise-visual',
	templateUrl: './manter-analise-visual.component.html',
	providers: [ManterAnaliseVisualGridComponent]
})

@Injectable()
export class ManterAnaliseVisualComponent implements OnInit {
	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	ocultarbotaocheck: boolean = false;
	ordenarGrid: boolean = true;
	servicoConsultarGrid= 'ConsultarAnaliseVisualGrid';
	inscricaoSuframa = '';
	razaoSocialEmpresa = '';
	isUsuarioImportador: boolean = false;

	@ViewChild('inscricaoCadastral') inscricaoCadastral;
	@ViewChild('empresa') empresa;
	@ViewChild('npli') npli;
	@ViewChild('dataEnvioInicial') dataEnvioInicial;
	@ViewChild('dataEnvioFinal') dataEnvioFinal;
	@ViewChild('grid') grid1;

	isModificouPesquisa: boolean = false;
	model: manterPliVM = new manterPliVM();
	isBuscaSalva: boolean = false;
	carregarGrid: boolean = false;
	opcaoStatus: number;
	isAnaliseVisual: boolean;

	constructor(
		private applicationService: ApplicationService,
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
		private ManterAnaliseVisualGridComponent: ManterAnaliseVisualGridComponent
	) {

		if (localStorage.getItem(this.router.url) == null && localStorage.length > 0) {
			localStorage.clear();
		}
	 
		localStorage.removeItem("GridStatusPli");
	}

	ngOnInit(): void {
		this.isUsuarioImportador = false;

		if (this.model.statusPliSelecionado == undefined) {
			this.parametros.statusPliSelecionado = 0;
		}

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
			this.parametros.servico = this.servicoConsultarGrid;
			this.parametros.titulo = "CONSULTAR ANÁLISE VISUAL";
			this.parametros.width = { 0: { columnWidth: 80 }, 1: { columnWidth: 120 }, 2: { columnWidth: 250 }, 3: { columnWidth: 120 }, 4:{ columnWidth: 200} };
			this.parametros.columns = ["Nº PLI", "Inscrição Cadastral", "Empresa", "Data de Entrega", "Status"  ];
			this.parametros.fields = ["numeroPliConcatenado", "inscricaoCadastral", "razaoSocial", "dataEnvioPliFormatada", "statusPliAnaliseFormatado"  ];

			let dat = new Date();
			this.parametros.dataInicio = dat.toLocaleDateString('en-CA');
			this.parametros.dataFim = dat.toLocaleDateString('en-CA');
		}

		if (this.parametros.statusPli == null)
			this.parametros.statusPli = 0;
	}

	retornaValorSessao() {
	
		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;
		}
	}

	validarData() {

		if (this.parametros != null) {

			try {

				this.dataEnvioInicial.nativeElement.setCustomValidity('');
				this.dataEnvioFinal.nativeElement.setCustomValidity('');

				var dataFim = new Date(this.parametros.dataFim);
				var dataInicio = new Date(this.parametros.dataInicio);


				if (this.dataEnvioInicial.nativeElement.value.length > 0 || this.dataEnvioFinal.nativeElement.value.length > 0) {
					if (this.dataEnvioInicial.nativeElement.value.length >= 0 && this.dataEnvioInicial.nativeElement.value.length < 10) {
						this.dataEnvioInicial.nativeElement.setCustomValidity('Campo inválido');
					} else if (this.dataEnvioFinal.nativeElement.value.length >= 0 && this.dataEnvioFinal.nativeElement.value.length < 10) {
						this.dataEnvioFinal.nativeElement.setCustomValidity('Campo inválido');

					} else if (new Date(this.parametros.dataFim) < new Date(this.parametros.dataInicio)) {
						this.dataEnvioFinal.nativeElement.setCustomValidity('Campo inválido. Data inicio maior que data final');
					}
				}
			} catch (e) {

				this.dataEnvioInicial.nativeElement.setCustomValidity('Informe uma data válida.');
			}

		}

	}

	buscar(exibirMensagem) {

		this.validarData();
		if (!this.validationService.form('formBusca')) { return; }

		if ((this.inscricaoCadastral.nativeElement.value == '') &&
			(this.empresa.nativeElement.value == '') &&
			(this.npli.nativeElement.value == '') &&
			(this.dataEnvioInicial.nativeElement.value == '') &&
			(this.dataEnvioFinal.nativeElement.value == '') &&
			(this.parametros.statusPliSelecionado == 0)) {

			if (exibirMensagem) {
				this.modal.alerta(this.msg.INFORME_OS_FILTROS_DE_PESQUISA, 'Informação');
			} else {
				this.validarData()
				if (this.isBuscaSalva) {
					this.listar();
				}
			}
		}
		else {

			this.validarData();

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
		if (!this.isUsuarioImportador) {
			this.inscricaoCadastral.nativeElement.value = "";
			this.empresa.nativeElement.value = "";
			this.parametros.razaoSocial = "";
			this.parametros.inscricaoCadastral = "";

		}
		this.parametros.dataFim = "";
		this.parametros.dataInicio = "";
		this.npli.nativeElement.value = "";
		this.parametros.statusPliSelecionado = undefined;
	}

	listar() {
		this.parametros.consultarPli = 1;
		this.ocultarbotaocheck = true;


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

			if (this.inscricaoCadastral.nativeElement.value == "")
				this.parametros.inscricaoCadastral = null;
			else
				this.parametros.inscricaoCadastral = this.inscricaoCadastral.nativeElement.value;

			if (this.empresa.nativeElement.value == "" || this.empresa.nativeElement.value == undefined)
				this.parametros.razaoSocial = null;
			else
				this.parametros.razaoSocial = this.empresa.nativeElement.value;

			if (this.npli.nativeElement.value == "") {
				this.parametros.NumeroPli = -1;
				this.parametros.Ano = -1;
				this.parametros.Numero = "";
			} else {
				this.parametros.Ano = this.npli.nativeElement.value.split("/")[0];
				this.parametros.NumeroPli = +this.npli.nativeElement.value.split("/")[1];
				this.parametros.Numero = this.npli.nativeElement.value;
			}

			if (this.dataEnvioInicial.nativeElement.value == "") {
				this.parametros.dataInicio = null;
			} else {
				this.parametros.dataInicio = this.dataEnvioInicial.nativeElement.value;
			}

			if (this.dataEnvioFinal.nativeElement.value == "") {
				this.parametros.dataFim = null;
			} else {
				this.parametros.dataFim = this.dataEnvioFinal.nativeElement.value;
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


		this.parametros.exportarListagem = true;
		this.applicationService.get(this.servicoConsultarGrid, this.parametros).subscribe((result: PagedItems) => {
			
			this.isModificouPesquisa = false;
			this.isBuscaSalva = true;
			this.grid.lista = JSON.parse(JSON.stringify(result.items));
			this.grid.total = result.total;

			this.gravarBusca();
		});

	}

	gravarBusca() {


		if (this.inscricaoCadastral.nativeElement.value == "")
			this.parametros.inscricaoCadastral = null;
		else
			this.parametros.inscricaoCadastral = this.inscricaoCadastral.nativeElement.value;

		if (this.empresa.nativeElement.value == "" || this.empresa.nativeElement.value == undefined)
			this.parametros.razaoSocial = null;
		else
			this.parametros.razaoSocial = this.empresa.nativeElement.value;

		if (this.npli.nativeElement.value == "") {
			this.parametros.NumeroPli = -1;
			this.parametros.Ano = -1;
			this.parametros.Numero = "";
		} else {
			this.parametros.Ano = this.npli.nativeElement.value.split("/")[0];
			this.parametros.NumeroPli = +this.npli.nativeElement.value.split("/")[1];
			this.parametros.Numero = this.npli.nativeElement.value;
		}

		if (this.dataEnvioInicial.nativeElement.value == "") {
			this.parametros.dataInicio = null;
		} else {
			this.parametros.dataInicio = this.dataEnvioInicial.nativeElement.value;
		}

		if (this.dataEnvioFinal.nativeElement.value == "") {
			this.parametros.dataFim = null;
		} else {
			this.parametros.dataFim = this.dataEnvioFinal.nativeElement.value;
		}

		localStorage.removeItem(this.router.url);
		localStorage.setItem(this.router.url, JSON.stringify(this.parametros));

	}

}
