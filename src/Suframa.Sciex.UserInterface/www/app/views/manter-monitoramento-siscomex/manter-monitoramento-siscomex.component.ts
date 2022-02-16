import { Component, OnInit, Injectable, ViewChild, OnChanges, EventEmitter, SimpleChanges } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router } from '@angular/router';
import { ValidationService } from "../../shared/services/validation.service";
import { manterPliVM } from '../../view-model/ManterPliVM';
import { forEach } from '@angular/router/src/utils/collection';
import { manterMonitoramentoSiscomexVM } from '../../view-model/ManterMonitoramentoSiscomexVM';

@Component({
	selector: 'app-manter-monitoramento-siscomex',
	templateUrl: './manter-monitoramento-siscomex.component.html',
})

@Injectable()
export class ManterMonitoramentoSiscomex implements OnInit  {
	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	ocultarbotaocheck: boolean = false;
	ordenarGrid: boolean = true;
	servicoMonitoramentoSiscomexGrid = 'MonitoramentoSiscomexGrid';
	servicoMonitoramentoSiscomex = 'MonitoramentoSiscomex';

	@ViewChild('statusEnvio') statusEnvio;
	@ViewChild('tipoConsulta') tipoConsulta;
	@ViewChild('nAli') nAli;
	@ViewChild('dataEnvioInicial') dataEnvioInicial;
	@ViewChild('dataEnvioFinal') dataEnvioFinal;
	@ViewChild('aliEspecifica') aliEspecifica;
	@ViewChild('envioArquivo') envioArquivo;

	isModificouPesquisa: boolean = false;
	model: manterMonitoramentoSiscomexVM = new manterMonitoramentoSiscomexVM();
	isBuscaSalva: boolean = false;
	carregarGrid: boolean = false;
	opcaoStatus: number;
	isGeracaoDebito: boolean;
	isAnaliseVisual: boolean;
	isAguardandoProcessamento: boolean;
	isProcessado: boolean;
	ocultarBotaoReprocessar: boolean = true;
	desabilitaCampoAli: boolean = true;

	constructor(
		private applicationService: ApplicationService,
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router
	) {

		if (localStorage.getItem(this.router.url) == null && localStorage.length > 0) {
			localStorage.clear();
		}
	}

	public getSelectedOption() {

		if (this.envioArquivo.nativeElement.checked == true) {
			this.nAli.nativeElement.value = "";
			this.nAli.nativeElement.disabled = true;
			this.dataEnvioInicial.nativeElement.disabled = false;
			this.dataEnvioFinal.nativeElement.disabled = false;
			this.tipoConsulta.nativeElement.disabled = false;
			this.statusEnvio.nativeElement.disabled = false;
		}

		if (this.aliEspecifica.nativeElement.checked == true) {

			this.nAli.nativeElement.disabled = false;
			this.dataEnvioFinal.nativeElement.value = "";
			this.dataEnvioInicial.nativeElement.value = "";
			//this.tipoConsulta.nativeElement.value = "";
			//this.statusEnvio.nativeElement.value = "";
			this.dataEnvioInicial.nativeElement.disabled = true;
			this.dataEnvioFinal.nativeElement.disabled = true;
			this.tipoConsulta.nativeElement.disabled = true;
			this.statusEnvio.nativeElement.disabled = true;

		}
	}


	ngOnInit(): void {

		this.nAli.nativeElement.disabled = true

		this.ocultarBotaoReprocessar = false;

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
			this.parametros.servico = this.servicoMonitoramentoSiscomexGrid;
			this.parametros.titulo = "CANCELAR LI";
			this.parametros.width = { 0: { columnWidth: 100 }, 1: { columnWidth: 460 }, 2: { columnWidth: 100 }, 3: { columnWidth: 100 }  };
			this.parametros.columns = ["NCM", "Descrição", "Nº LI", "Data Cadastro"];
			this.parametros.fields = ["numeroNCM", "descricaoNCMMercadoria", "numeroLi", "dataCadastroFormatado"];
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

	public onChangeTipoStatus() {

		if (this.parametros.statusPli == 22) {
			this.isGeracaoDebito = false;
			this.isAnaliseVisual = false;
			this.isAguardandoProcessamento = false;
			this.isProcessado = false;
		} else if (this.parametros.statusPli == 23) {
			this.isGeracaoDebito = false;
			this.isAnaliseVisual = true;
			this.isAguardandoProcessamento = false;
			this.isProcessado = false;
		} else if (this.parametros.statusPli == 24) {
			this.isGeracaoDebito = false;
			this.isAnaliseVisual = false;
			this.isAguardandoProcessamento = true;
			this.isProcessado = false;
		} else if (this.parametros.statusPli == 25) {
			this.isGeracaoDebito = false;
			this.isAnaliseVisual = false;
			this.isAguardandoProcessamento = false;
			this.isProcessado = true;
		} else {
			this.isGeracaoDebito = true;
			this.isAnaliseVisual = true;
			this.isAguardandoProcessamento = true;
			this.isProcessado = true;
		}
	}

	buscar(exibirMensagem) {

		if (!this.validationService.form('formBusca')) { return; }

		if ((this.nAli.nativeElement.value == '' && this.nAli.nativeElement.disabled == false)) {
			if (exibirMensagem) {
				this.modal.alerta(this.msg.INFORME_OS_FILTROS_DE_PESQUISA, 'Informação');
			} else {
				if (this.isBuscaSalva) {
					this.listar();
				}
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
		this.nAli.nativeElement.value = "";
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


			//Busca o numero ALI
			if (this.nAli.nativeElement.value == "") {
				this.parametros.numeroAli = -1;
			} else {
				this.parametros.numeroAli = this.nAli.nativeElement.value;
			}

			//Envia a data inicial e final da geração do Arquivo
			this.parametros.dataEnvioInicial = this.dataEnvioInicial.nativeElement.value;
			this.parametros.dataEnvioFinal = this.dataEnvioFinal.nativeElement.value;

			//Status de Envio
			if (this.statusEnvio.nativeElement.value == 1) {
				this.parametros.codigoStatusEnvioSiscomex = 1;
			}
			if (this.statusEnvio.nativeElement.value == 2) {
				this.parametros.codigoStatusEnvioSiscomex = 2;
			}
			if (this.statusEnvio.nativeElement.value == 3) {
				this.parametros.codigoStatusEnvioSiscomex = 3;
			}

			//Tipo de Consulta
			if (this.tipoConsulta.nativeElement.value == 1) {
				this.parametros.TipoDeConsulta = 1;
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
		this.applicationService.get(this.servicoMonitoramentoSiscomexGrid, this.parametros).subscribe((result: PagedItems) => {
			this.isModificouPesquisa = false;
			this.isBuscaSalva = true;
			this.grid.lista = JSON.parse(JSON.stringify(result.items));
			this.grid.total = result.total;

			if (result.total > 0) {
				this.parametros.exportarListagem = true;
				for (var i = 0; i < result.items.length; i++) {
					this.ocultarBotaoReprocessar = true;
					this.ocultarbotaocheck = true;
				}
			} else {
				this.ocultarBotaoReprocessar = false;
				this.ocultarbotaocheck = false;
			}
			this.gravarBusca();
		});
	}

	gravarBusca() {
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
}
