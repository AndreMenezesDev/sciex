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
import { ModalNovoPlanoComponent } from './modal/modal-novo-plano.component';

@Component({
	selector: 'app-manter-plano-exportacao',
	templateUrl: './manter-plano-exportacao.component.html'
})

@Injectable()
export class ManterPlanoExportacaoComponent implements OnInit {
	grid: any = { sort: {} };
	parametros: any = {};
	parametros1: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = true;
	servicoGrid = 'PlanoExportacao';
	somenteLeitura: boolean;
	@ViewChild('dataInicio') dataInicio;
    @ViewChild('dataFim') dataFim;
	@ViewChild('formBusca') formBusca;
	@ViewChild('btnlimpar') btnlimpar;
	@ViewChild('appModalNovoPlano') appModalNovoPlano : ModalNovoPlanoComponent;
	isModificouPesquisa: boolean = false;
	isBuscaSalva: boolean = false;
	dataValida = false;
	formPai = this;

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

			this.listar();
		}
		else {
			this.parametros = {};

            // this.parametros.idPliAplicacao = 0;
            // this.parametros.tipoDocumento = 0;
			// this.parametros.statusPli = 0;
			// this.parametros.NumeroPli = -1;
			// this.parametros.Ano = -1;

			// this.parametros.titulo = "MANTER PLI"
			// this.parametros.width = { 0: { columnWidth: 150 }, 1: { columnWidth: 200 }, 2: { columnWidth: 150 }, 3: { columnWidth: 150 }, 4: { columnWidth: 260 } };
			// this.parametros.servico = this.servicoPliGrid;
			// this.parametros.columns = ["Nº PLI", "Tipo de Aplicação","Tipo de PLI","Data de Cadastro", "Status"];
			// this.parametros.fields = ["numeroPliConcatenado", "descricaoAplicacao", "descricaoTipoDocumento", "dataPliFormatada", "descricaoStatus"];

			let dat = new Date();
			this.parametros.dataInicio = dat.toLocaleDateString('en-CA');
			this.parametros.dataFim = dat.toLocaleDateString('en-CA');
		}

	}

	retornaValorSessao() {

		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			if (this.parametros != undefined) {

				if (this.parametros.numeroPlano != undefined || this.parametros.numeroPlano != null)
					this.parametros1.numeroPlano = this.parametros.numeroPlano != undefined ? this.parametros.numeroPlano : null;

				if (this.parametros.statusPlano != undefined || this.parametros.statusPlano != null)
					this.parametros1.statusPlano = this.parametros.statusPlano != undefined ? this.parametros.statusPlano : null;

				if (this.parametros.dataInicio != undefined || this.parametros.dataInicio != null)
					this.parametros1.dataInicio = this.parametros.dataInicio != undefined ? this.parametros.dataInicio : null;

				if (this.parametros.dataFim != undefined || this.parametros.dataFim != null)
					this.parametros1.dataFim = this.parametros.dataFim != undefined ? this.parametros.dataFim : null;
			}
			this.isBuscaSalva = true;
		}
	}

	buscar(exibirMensagem) {
		this.validarData();
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
		this.parametros.isClear = true;
		this.gravarBusca();
		this.parametros.numeroPlano = null;
		this.parametros.statusPlano = undefined;
		this.parametros.dataInicio = null;
		this.parametros.dataFim = null;
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

			if (this.dataInicio.nativeElement.value == "" && this.dataFim.nativeElement.value == null) {
				this.parametros.dataFim  = this.parametros.dataInicio = null;
			}
			else {
				this.parametros.dataInicio = this.dataInicio.nativeElement.value;
				this.parametros.dataFim = this.dataFim.nativeElement.value;
			}
		}
		else {
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

		this.parametros.exportarListagem = true;
		this.ocultarGrid = true;
		this.applicationService.get(this.servicoGrid, this.parametros).subscribe((result: PagedItems) => {
			this.isModificouPesquisa = false;
			this.isBuscaSalva = true;

				this.grid.lista = result.items;
				this.grid.total = result.total;
				console.log(this.grid.lista)
				
			if (result.total > 0){
				this.ocultarGrid = false;

				//this.grid.isUsuarioInterno = result.total > 0 ? result.items[0].isUsuarioInterno : false;
				this.setarDadosExportacao();
			}else{
				this.ocultarGrid = true;
				//this.modal.resposta(this.msg.NENHUM_REGISTRO_ENCONTRADO, "Informação", '')
			}

			this.gravarBusca();
		});
	}
	setarDadosExportacao() {
		this.parametros.titulo = "Plano de Exportação"
		this.parametros.width = { 0: { columnWidth: 180 }, 
									1: { columnWidth: 180 }, 
									2: { columnWidth: 180 }, 
									3: { columnWidth: 80 }, 
									4: { columnWidth: 150 }
								};
		this.parametros.servico = this.servicoGrid;		
		this.parametros.columns = [
			"Nº Plano", 
			"Modalidade", 
			"Tipo",
			"Data Cadastro", 
			"Status"];
		this.parametros.fields = [
			"numeroAnoPlanoFormatado", 
			"tipoModalidadeString", 
			"tipoExportacaoString", 
			"dataCadastroFormatada", 
			"situacaoString"];
	}

	gravarBusca() {
		this.parametros.DataInicio = this.parametros1.DataInicio;
		this.parametros.DataFim = this.parametros1.DataFim;

		if (this.dataInicio.nativeElement.value == "" && this.dataFim.nativeElement.value == null) {
			this.parametros.dataFim = this.parametros.dataInicio = null;
		}
		else {
			this.parametros.dataInicio = this.dataInicio.nativeElement.value;
			this.parametros.dataFim = this.dataFim.nativeElement.value;
		}

		localStorage.removeItem(this.router.url);
		localStorage.setItem(this.router.url, JSON.stringify(this.parametros));
	}
	abrirNovoPlano()
	{
		this.appModalNovoPlano.abrir(this);
	}
}

