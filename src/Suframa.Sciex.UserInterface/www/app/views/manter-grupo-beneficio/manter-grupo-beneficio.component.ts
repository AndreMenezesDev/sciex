import { Component, OnInit, Injectable, ViewChild, Output, EventEmitter } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router } from '@angular/router';
import { ValidationService } from "../../shared/services/validation.service";
import { manterPliVM } from '../../view-model/ManterPliVM';
import { forEach } from '@angular/router/src/utils/collection';
import { EnumPerfil } from '../../shared/enums/EnumPerfil';
import { ManterGupoBeneficioGridComponent } from './grid/grid-manter-grupo-beneficio.component';
import { TaxaGrupoBeneficioVM } from '../../view-model/TaxaGrupoBeneficioVM';

@Component({
	selector: 'app-manter-grupo-beneficio-pli',
	templateUrl: './manter-grupo-beneficio.component.html',
	providers: [ManterGupoBeneficioGridComponent]
})

@Injectable()
export class ManterGrupoBeneficioComponent implements OnInit {


	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	ocultarbotaocheck: boolean = false;
	ordenarGrid: boolean = true;
	manterBeneficioGrid = 'ManterBeneficioGrid';
	inscricaoSuframa = '';
	razaoSocialEmpresa = '';
	isUsuarioImportador: boolean = false;
	formPai = this;

	@ViewChild('inscricaoCadastral') inscricaoCadastral;
	@ViewChild('empresa') empresa;
	@ViewChild('descricao') descricao;
	@ViewChild('situacao') situacao;
	@ViewChild('codigo') codigo;
	@ViewChild('tipobeneficio') tipobeneficio;

	isModificouPesquisa: boolean = false;
	model: TaxaGrupoBeneficioVM = new TaxaGrupoBeneficioVM();
	isBuscaSalva: boolean = false;
	carregarGrid: boolean = false;
	opcaoStatus: number;
	isGeracaoDebito: boolean;
	isAnaliseVisual: boolean;
	isAguardandoProcessamento: boolean;
	isProcessado: boolean;
	ocultarBotaoReprocessar: boolean = true;
	selectOpcao: boolean = true;
	selectTipoBeneficio: boolean = true;

	constructor(
		private applicationService: ApplicationService,
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
		private ManterGupoBeneficioGridComponent: ManterGupoBeneficioGridComponent
	) {

		if (localStorage.getItem(this.router.url) == null && localStorage.length > 0) {
			localStorage.clear();
		}

		localStorage.removeItem("GridStatusPli");
	}

	ngOnInit(): void {

		this.isUsuarioImportador = false;

		this.ocultarBotaoReprocessar = true;

		/*this.applicationService.get("UsuarioLogado").subscribe((result: any) => {
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
			}
		});*/

		this.retornaValorSessao();

		if (this.parametros != undefined || this.parametros != null) {

			this.grid.page = this.parametros.page;
			this.grid.size = this.parametros.size;
			this.grid.sort.field = this.parametros.sort;
			this.grid.sort.reverse = this.parametros.reverse;

			if (this.parametros.codigo != "" &&
				this.parametros.descricao != "" &&
				this.parametros.statusBeneficio == "99" &&
				this.parametros.tipoBeneficio == "99"){

				localStorage.removeItem(this.router.url);
			}else{

				this.codigo.nativeElement.value = this.parametros.codigo;
				this.descricao.nativeElement.value = this.parametros.descricao;
				this.situacao.nativeElement.value = parseInt(this.parametros.statusBeneficio);
				this.tipobeneficio.nativeElement.value = parseInt(this.parametros.tipoBeneficio);
			}

				this.listar();

		}else {

			this.parametros = {};
			this.parametros.servico = this.manterBeneficioGrid;
			this.parametros.titulo = "MANTER BENEFICIO"
			this.parametros.width = { 0: { columnWidth: 80 }, 1: { columnWidth: 360 }, 2: { columnWidth: 80 }, 3: { columnWidth: 80 }, 4: { columnWidth: 150 } };
			this.parametros.columns = ["Código", "Descrição", "Tipo Benefício", "% de Reducao", "Data Cadastro" ] ;
			this.parametros.fields = ["codigo", "descricao", "tipoBeneficioConcatenado", "percentualConcatenado", "dataConcatenada" ];
		}

	}

	retornaValorSessao() {

		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;
		}
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

		if (this.codigo.nativeElement.value == ""
		 && this.descricao.nativeElement.value == ""
		 && this.situacao.nativeElement.value == "undefined"
		 && this.tipobeneficio.nativeElement.value == "TODAS") {
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


	listar(){

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
				this.parametros.codigo = "";
			else
				this.parametros.codigo = this.codigo.nativeElement.value;

			if (this.descricao.nativeElement.value == "")
				this.parametros.descricao = "";
			else
				this.parametros.descricao = this.descricao.nativeElement.value;

			if (this.situacao.nativeElement.value == "")
				this.parametros.statusBeneficio = 0;
			else
				this.parametros.statusBeneficio = this.situacao.nativeElement.value;

			if (this.tipobeneficio.nativeElement.value == "")
				this.parametros.tipoBeneficio = -1;
			else
				this.parametros.tipoBeneficio = this.tipobeneficio.nativeElement.value;

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

		// this.parametros.page = this.grid.page;
        // this.parametros.size = this.grid.size;
        // this.parametros.sort = this.grid.sort.field;
        // this.parametros.reverse = this.grid.sort.reverse;

		this.parametros.exportarListagem = false;

		this.applicationService.get(this.manterBeneficioGrid, this.parametros).subscribe((result: PagedItems) => {

			if(result.total > 0){

				this.grid.lista = result.items;
				this.grid.total = result.total;

				this.isModificouPesquisa = false;
				this.isBuscaSalva = true;
				this.parametros.exportarListagem = true;
			} else{

				this.modal.alerta("Registro não encontrado", 'Alerta');
				this.grid = { sort: {} };

			}
			this.gravarBusca();

		});

	}

	gravarBusca() {

		if (this.codigo.nativeElement.value == "")
				this.parametros.codigo = "";
		else
			this.parametros.codigo = this.codigo.nativeElement.value;

		if (this.descricao.nativeElement.value == "")
			this.parametros.descricao = "";
		else
			this.parametros.descricao = this.descricao.nativeElement.value;

		if (this.situacao.nativeElement.value == "")
			this.parametros.statusBeneficio = 0;
		else
			this.parametros.statusBeneficio = this.situacao.nativeElement.value;

		if (this.tipobeneficio.nativeElement.value == "")
			this.parametros.tipoBeneficio = -1;
		else
			this.parametros.tipoBeneficio = this.tipobeneficio.nativeElement.value;

		localStorage.removeItem(this.router.url);
		localStorage.setItem(this.router.url, JSON.stringify(this.parametros));
	}

	ocultar(){

		if (this.ocultarFiltro === false)
		{
			this.ocultarFiltro = true;
		}
		else{
			this.ocultarFiltro = false;
		}

	}

	limpar(){

		this.selectOpcao = true;
		this.selectTipoBeneficio = true;
		this.parametros.descricao = '';
		this.parametros.codigo = '';
		this.situacao.nativeElement.value = 99;
		this.tipobeneficio.nativeElement.value = 99;
	}

}
