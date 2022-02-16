import { Component, OnInit, Injectable, ViewChild, Output, EventEmitter, AfterViewInit } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router } from '@angular/router';
import { ValidationService } from "../../shared/services/validation.service";
import { manterPliVM } from '../../view-model/ManterPliVM';
import { forEach } from '@angular/router/src/utils/collection';
import { ManterConsultarPliGridComponent } from './grid/grid.component';

import { EnumPerfil } from '../../shared/enums/EnumPerfil';

@Component({
	selector: 'app-manter-consultar-pli',
	templateUrl: './manter-consultar-pli.component.html',
	providers: [ManterConsultarPliGridComponent]
})

@Injectable()
export class ManterConsultarPliComponent implements OnInit {
	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	ocultarbotaocheck: boolean = false;
	ordenarGrid: boolean = true;
	servicoConsultarPliGrid = 'ConsultarPliGrid';
	inscricaoSuframa = '';
	razaoSocialEmpresa = '';
	isUsuarioImportador: boolean = false;

	@ViewChild('inscricaoCadastral') inscricaoCadastral;
	@ViewChild('empresa') empresa;
	@ViewChild('npli') npli;
	@ViewChild('dataEnvioInicial') dataEnvioInicial;
	@ViewChild('dataEnvioFinal') dataEnvioFinal;
	@ViewChild('appModalJustificativaReprocessar') appModalJustificativaReprocessar;
	@ViewChild('grid') grid1;
	isModificouPesquisa: boolean = false;
	model: manterPliVM = new manterPliVM();
	isBuscaSalva: boolean = false;
	carregarGrid: boolean = false;
	opcaoStatus: number;
	isGeracaoDebito: boolean;
	isAnaliseVisual: boolean;
	isAguardandoProcessamento: boolean;
	isProcessado: boolean;
	ocultarBotaoReprocessar: boolean = true;

	constructor(
		private applicationService: ApplicationService,
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
		private ManterConsultarPliGridComponent: ManterConsultarPliGridComponent
	) {

		if (localStorage.getItem(this.router.url) == null && localStorage.length > 0) {
			localStorage.clear();
		}
	 
		localStorage.removeItem("GridStatusPli");
	}

	ngOnInit(): void {
		this.isUsuarioImportador = false;

		this.ocultarBotaoReprocessar = true;

		this.applicationService.get("UsuarioLogado").subscribe((result: any) => {
			if (result && result.perfis.includes(EnumPerfil.pessoaJuridica)) {
				this.isUsuarioImportador = true;
				this.razaoSocialEmpresa = result.empresaRepresentadaRazaoSocial;
				this.ocultarbotaocheck = true;
				this.ocultarBotaoReprocessar = true;
				this.inscricaoCadastral.nativeElement.value = result.usuInscricaoCadastral;
				this.empresa.nativeElement.value = result.usuNomeEmpresaOuLogado;
				this.parametros.razaoSocial = result.usuNomeEmpresaOuLogado;
				this.parametros.inscricaoCadastral = result.usuInscricaoCadastral;
				this.isModificouPesquisa = true
				//this.cpfcnpj = this.formatarLogin(result.usuarioLogadoCpfCnpj);
			}

		});

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
			this.parametros.servico = this.servicoConsultarPliGrid;
			this.parametros.titulo = "CONSULTAR PLI";
			this.parametros.width = { 0: { columnWidth: 150 }, 1: { columnWidth: 150 }, 2: { columnWidth: 170 }, 3: { columnWidth: 290 } };
			this.parametros.columns = ["PLI Nº", "Data do Envio", "Inscrição Cadastral", "Empresa"];
			this.parametros.fields = ["numeroPliConcatenado", "dataPliFormatada", "inscricaoCadastral", "razaoSocial"];

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

	public onChangeTipoStatus() {

		if (this.parametros.statusPliSelecionado == 2) {
			//Entregue à SUFRAMA
			this.parametros.statusPli = 21
		} else if (this.parametros.statusPliSelecionado == 3) {
			//Débito Gerado
			this.parametros.statusPli = 22
		} else if (this.parametros.statusPliSelecionado == 4) {
			//Processado pela SUFRAMA
			this.parametros.statusPli = 25
		} else {
			//Enviado ao SISCOMEX / Respondido pelo SISCOMEX
			this.parametros.statusPli = 25
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
		this.ocultarBotaoReprocessar = true;


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


		this.parametros.exportarListagem = false;
		this.applicationService.get(this.servicoConsultarPliGrid, this.parametros).subscribe((result: PagedItems) => {
			
			this.isModificouPesquisa = false;
			this.isBuscaSalva = true;
			this.grid.lista = JSON.parse(JSON.stringify(result.items));
			this.grid.total = result.total;

			if (result.total > 0) {
				this.parametros.exportarListagem = true;
				for (var i = 0; i < result.items.length; i++) {
					if (result.items[i].statusPliProcessamento == 3 && !this.isUsuarioImportador) {
						this.ocultarBotaoReprocessar = false;
						this.ocultarbotaocheck = false;

						break;
					}
				}
			}
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

	removerCaractere(documento) {
		var nomeDocumento = "";
		for (var i = 0; i < documento.length; i++) {
			if (documento[i] != "." && documento[i] != "-" && documento[i] != "/") {
				nomeDocumento = nomeDocumento + documento[i];
			}
		}
		return nomeDocumento;
	}

	reprocessarPLI() {
		if (this.grid.lista != undefined) {
			var selecionadoErrado = false;
			this.model.listaSelecionados = new Array<number>();
			for (var i = 0; i < this.grid.lista.length; i++) {
				if (this.grid.lista[i].checkbox)
					this.model.listaSelecionados.push(this.grid.lista[i].idPLI);
			}

			if (this.model.listaSelecionados == null) {
				selecionadoErrado = true;
			}
		}

		if (selecionadoErrado) {
			this.modal.alerta("PLI(s) selecionado(s) que não pode(m) ser reprocessado(s).", 'Informação');
		} else
			if (this.model.listaSelecionados.length > 0) {
				this.appModalJustificativaReprocessar.abrir(this, this.model.listaSelecionados);
			}

			else {
				this.modal.alerta("Nenhum PLI na lista ou selecionado para ser reprocessado.", 'Informação');
			}
	}
}
