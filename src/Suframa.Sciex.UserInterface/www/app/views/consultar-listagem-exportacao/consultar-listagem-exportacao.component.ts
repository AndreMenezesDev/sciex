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
import { EnumPerfil } from '../../shared/enums/EnumPerfil';

@Component({
	selector: 'app-consultar-listagem-exportacao',
	templateUrl: './consultar-listagem-exportacao.component.html'
})

@Injectable()
export class ConsultarListagemExportacaoComponent implements OnInit {
	grid: any = { sort: {} };
	parametros: any = {};
	parametros1: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = true;
	servicoGrid = 'LEConsultarProdutoGrid';
	somenteLeitura: boolean;
	@ViewChild('dataInicio') dataInicio;
    @ViewChild('dataFim') dataFim;
	@ViewChild('formBusca') formBusca;
	@ViewChild('btnlimpar') btnlimpar;
	isModificouPesquisa: boolean = false;
	isBuscaSalva: boolean = false;
	dataValida = false;
	isUsuarioInterno: boolean = false;


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
		this.buscarUsuario();
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
			this.parametros.titulo = "Listagem de Exportação"
			this.parametros.width = { 0: { columnWidth: 50 }, 1: { columnWidth: 55 }, 2: { columnWidth: 235 }, 3: { columnWidth: 250 }, 4: { columnWidth: 80 }, 5: { columnWidth: 125 } };
			this.parametros.servico = this.servicoGrid;
			this.parametros.columns = ["Código Produto", "NCM","Cód. Prod. Suf.","Tipo do Produto", "Data do Cadastro", "Status"];
			this.parametros.fields = ["codigoProduto", "codigoNCM", "descCodigoProdutoSuframa", "descCodigoTipoProduto", "dataCadastroFormatada", "descStatusLE"];

			let dat = new Date();
			this.parametros.dataInicio = dat.toLocaleDateString('en-CA');
			this.parametros.dataFim = dat.toLocaleDateString('en-CA');
		}
	}

	buscarUsuario(){
		this.applicationService.get("UsuarioLogado").subscribe((result: any) => {
			if(result) {
				if (result.perfis.includes(EnumPerfil.pessoaJuridica)) {
					this.isUsuarioInterno = false;
				}
				else{
					this.isUsuarioInterno = true;
				}
			}
		});
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
		this.parametros.inscricaoCadastral = null;
		this.parametros.razaoSocial = null;
		this.parametros.codigoProduto = null;
		this.parametros.dataInicio = null;
		this.parametros.dataFim = null;
		this.parametros.statusLE = undefined;
		this.parametros.idAnalistaDesignado = 0;
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

			if (result.total > 0){
				this.grid.lista = result.items;
				this.grid.total = result.total;
				this.grid.isUsuarioInterno = result.total > 0 ? result.items[0].isUsuarioInterno : false;
			}else{
				this.grid.total = 0
			}

			this.gravarBusca();
		});
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
}

