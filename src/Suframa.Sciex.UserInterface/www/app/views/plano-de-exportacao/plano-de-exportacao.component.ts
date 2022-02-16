import { Component, OnInit, Injectable, ViewChild } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router } from '@angular/router';
import { AuthGuard } from '../../shared/guards/auth-guard.service';
import { ValidationService } from '../../shared/services/validation.service';
import { Location } from '@angular/common';
@Component({
	selector: 'app-plano-de-exportacao',
	templateUrl: './plano-de-exportacao.component.html'
})

@Injectable()
export class PlanoDeExportacaoComponent implements OnInit {
	formPai = this;

	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = true;
	result: boolean = false;
	isModificouPesquisa: boolean = false;
	isBuscaSalva: boolean = false;
	dataValida = false;
	isUsuarioInterno: boolean = false;

	servico = 'PlanoDeExportacao';

	@ViewChild('dataInicio') dataInicio;
	@ViewChild('dataFim') dataFim;
	@ViewChild('formBusca') formBusca;
	@ViewChild('situacao') situacao;

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private validationService: ValidationService,
		private msg: MessagesService,
		private router: Router,
		private Location: Location,
		private authguard: AuthGuard
	) {
		//let a = this.Location.back();
		let sameScreen = sessionStorage.getItem('sameScreen');
		if (sameScreen != 'plano-de-exportacao' 
				|| sameScreen == null 
				|| sameScreen == undefined
			){
			sessionStorage.removeItem(`/`+sameScreen);
			sessionStorage.removeItem('sameScreen');
		}

		if (sessionStorage.getItem(this.router.url) == null && sessionStorage.length > 0) {
			sessionStorage.clear();
		}
	}

	ngOnInit(): void {		
		this.authguard.active = false;
		this.retornaValorSessao();
		

		if (this.parametros != undefined || this.parametros != null) {

			this.grid.page = this.parametros.page;
			this.grid.size = this.parametros.size;
			this.grid.sort.field = this.parametros.sort;
			this.grid.sort.reverse = this.parametros.reverse;
			this.grid.lista = 0;
			this.grid.total = 0;

			if (this.parametros.isClear)
				this.limpar();

			this.listar();
		}
		else {
			this.parametros = {};			
			this.limpar();
			this.limparPaginacao();
			let dat = new Date();
			this.parametros.dataInicio = dat.toLocaleDateString('en-CA');
			this.parametros.dataFim = dat.toLocaleDateString('en-CA');
		}
	}
	limparPaginacao(){
		this.parametros.page = 1;
		this.parametros.size = 10;
		this.grid.page = 1;
		this.grid.size = 10;
	}

	onChangeSort($event) {
		this.grid.sort = $event;
	}

	onChangeSize($event) {
		this.grid.size = $event;
	}

	onChangePage($event) {
		this.grid.page = $event;
		this.listar();
	}

	buscar(exibirMensagem) {
		this.validarData();

		if (exibirMensagem) {
			this.isModificouPesquisa = true;
			this.limparPaginacao();
		}
		else {
			this.isModificouPesquisa = false;
			this.isBuscaSalva = true;
		}
		this.listar();

	}

	listar() {
		// if (!this.isBuscaSalva || this.isModificouPesquisa) {

		// 	if (this.isModificouPesquisa) {
		// 		this.parametros.page = 1;
		// 		this.parametros.size = 10;
		// 		this.grid.page = 1;
		// 		this.grid.size = 10;
		// 	}
		// 	else {
		// 		this.parametros.page = this.grid.page;
		// 		this.parametros.size = this.grid.size;
		// 	}

		// 	this.parametros.sort = this.grid.sort.field;
		// 	this.parametros.reverse = this.grid.sort.reverse;
		// 	this.parametros.isClear = false;

		// 	if (this.dataInicio.nativeElement.value == "" && this.dataFim.nativeElement.value == null) {
		// 		this.parametros.dataFim = this.parametros.dataInicio = null;
		// 	}
		// 	else {
		// 		this.parametros.dataInicio = this.dataInicio.nativeElement.value;
		// 		this.parametros.dataFim = this.dataFim.nativeElement.value;
		// 	}

		// }

		this.parametros.idAnalistaDesignado ?
			this.parametros.idAnalistaDesignado = Number(this.parametros.idAnalistaDesignado) :
			'';

		this.parametros.status ?
			this.parametros.status = Number(this.parametros.status) :
			'';

		this.parametros.exportarListagem = true;
		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;

		this.applicationService.get(this.servico, this.parametros).subscribe((result: PagedItems) => {
			this.grid.lista = result.items;
			this.grid.total = result.total;

			if(result.total != null) {
				this.carregarExportacao();
			}

			this.gravarBusca();
		});
	}

	carregarExportacao() {
		this.parametros.servico = this.servico;
		this.parametros.titulo = "Listagem Plano de Exportação"

		this.parametros.width = {
			0: { columnWidth: 70 }, 1: { columnWidth: 90 }, 2: { columnWidth: 80 },
			3: { columnWidth: 100 }, 4: { columnWidth: 80 }, 5: { columnWidth: 70 },
			6: { columnWidth: 70 }, 7: { columnWidth: 70 }, 8: { columnWidth: 70 },
			9: { columnWidth: 40 }, 10: { columnWidth: 40 }
		};

		this.parametros.columns = [
			"N° Plano", "Empresa", "Modalidade",
			"Tipo", "Status", "Analista",
			"Dt Status", "Dt Recebimento", "Processo",
			"Fluxo < 70%", "Perda > 2%"
		];

		this.parametros.fields = [
			"numeroAnoPlanoFormatado", "razaoSocial", "tipoModalidadeString",
			"tipoExportacaoString", "situacaoString", "nomeResponsavel",
			"dataStatusFormatada", "dataEnvioFormatada", "numeroAnoProcessoFormatado",
			"qtdFluxoMenor70porcentoString", "qtdPerdaMaior2porcentoString"
		];
	}

	limpar() {
		this.parametros.numeroAnoPlanoConcat = null;
		this.parametros.situacao = "0";
		this.parametros.dataInicio = null;
		this.parametros.dataFim = null;
		this.parametros.numeroInscricaoCadastral = null;
		this.parametros.razaoSocial = null;
		this.parametros.idAnalistaDesignado = 0;
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

	ExibeOcultaFiltros() {
		this.ocultarFiltro ?
			this.ocultarFiltro = false :
			this.ocultarFiltro = true;
	}

	gravarBusca() {

		sessionStorage.removeItem(this.router.url);
		sessionStorage.setItem(this.router.url, JSON.stringify(this.parametros));
	}

	retornaValorSessao() {
		if (sessionStorage.getItem(this.router.url) != null &&
		sessionStorage.getItem(this.router.url) != "" &&
		sessionStorage.getItem(this.router.url) != undefined) {
			this.parametros = JSON.parse(sessionStorage.getItem(this.router.url));
			this.isBuscaSalva = true;
		} else {
			this.parametros = undefined;
		}
	}


}
