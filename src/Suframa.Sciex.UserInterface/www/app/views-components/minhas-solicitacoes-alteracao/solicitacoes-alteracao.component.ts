import { Component, OnInit, Injectable, ViewChild, AfterContentInit } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { ValidationService } from '../../shared/services/validation.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthGuard } from '../../shared/guards/auth-guard.service';
import { PRCSolicitacaoAlteracaoVM } from '../../view-model/PRCSolicitacaoAlteracaoVM';

@Component({
	selector: 'app-solicitacoes-alteracao',
	templateUrl: './solicitacoes-alteracao.component.html'
})

@Injectable()
export class SolicitacoesAlteracaoComponent implements OnInit {

	grid: any = { sort: {} };
	parametros: PRCSolicitacaoAlteracaoVM = new PRCSolicitacaoAlteracaoVM();
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = true;
	servico = 'SolicitacaoesAlteracao';
	somenteLeitura: boolean;

	@ViewChild('dataInicio') dataInicio;
    @ViewChild('dataFim') dataFim;
	@ViewChild('numeroProcesso') numeroProcesso;
	@ViewChild('numeroSolicitacao') numeroSolicitacao;
	@ViewChild('formBusca') formBusca;

	exibirFiltros : boolean = true;
	isModificouPesquisa: boolean = false;
	isBuscaSalva: boolean = false;
	dataValida = false;
	formPai = this;
	isUsuarioInterno: boolean = false;
	idProcesso : number;
	path : string;
	url : string;
	StorageClear : boolean = true;

	numeroAnoProcesso : string;

	constructor(
		private applicationService: ApplicationService,
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
		private route: ActivatedRoute,
		private authguard: AuthGuard,
	) {
		this.exibirFiltros = true;

		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		if(this.path != "minhas-solicitacoes-alteracao")
		{
			this.idProcesso = this.route.snapshot.params['idProcesso'];
			this.buscarCodigoProcessoPorId();
		}else{
			this.setHistoryUrl()
		}
	}

	setHistoryUrl(){
		let arrayUrl: Array<any> = [this.router.url];
		sessionStorage.setItem("arrayUrl", JSON.stringify(arrayUrl))
	}

	ngOnInit(): void {

		this.retornaValorSessao();

		if (!this.StorageClear) {
			this.grid.page = this.parametros.page;
			this.grid.size = this.parametros.size;
			this.grid.sort.field = this.parametros.sort;
			this.grid.sort.reverse = this.parametros.reverse;
			this.listar();
		} else {
			this.parametros.status = 99; //TODOS
			let dat = new Date();
			this.parametros.dataInicioString = dat.toLocaleDateString('en-CA');
			this.parametros.dataFimString = dat.toLocaleDateString('en-CA');
			this.setarDadosExportacao();
		}
	}

	buscarCodigoProcessoPorId(){
			this.applicationService.get('RecuperarNumeroAnoProcesso', Number(this.idProcesso)).subscribe((result: any) => {
			this.parametros.numeroAnoProcessoFormatado = result;
			this.numeroAnoProcesso = result;
		});
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

	limpar() {

		// this.parametros = new PRCSolicitacaoAlteracaoVM();
		// this.parametros.id = null;
		// this.parametros.numeroSolicitacao = null;
		// this.parametros.anoSolicitacao = null;
		// this.parametros.status = null;
		// this.parametros.dataInclusao = null;
		// this.parametros.cpfResponsavel = null;
		// this.parametros.nomeResponsavel = null;
		// this.parametros.processoVM = null;
		// this.parametros.idProcesso = null;
		// this.parametros.dataAlteracao = null;
		// this.parametros.cnpj = null;
		// this.parametros.razaoSocial = null;

		//complementos
		// this.parametros.quantidaDeItens = null;
		// this.parametros.dataInicio = null;
		// this.parametros.dataFim = null;
		// this.parametros.numeroProcesso = null;
		// this.parametros.anoProcesso = null;
		// this.parametros.descricaoStatus = null;
		this.parametros.numeroAnoProcessoFormatado = 
						this.parametros.numeroProcesso = 
						this.parametros.anoProcesso    = null;

		this.parametros.numeroAnoSolicitacaoFormatado = null;
		this.parametros.dataInicioString = "";
		this.parametros.dataFimString = "";

		if(this.path != "minhas-solicitacoes-alteracao")
		{
			this.parametros.numeroAnoProcessoFormatado = this.numeroAnoProcesso;
		}

		this.parametros.status = 99;
	}

	validar(exibirMensagem){
		this.validarData();

		if(this.parametros.numeroAnoProcessoFormatado){
			if(this.parametros.numeroAnoProcessoFormatado.length < 9){
				this.numeroProcesso.nativeElement.setCustomValidity('Campo inválido');
			} else {
				let Processo = this.parametros.numeroAnoProcessoFormatado.split("/");
				this.parametros.numeroProcesso = Number(Processo[0]);
				this.parametros.anoProcesso = Number(Processo[1]);
			}
		}else{
			this.parametros.numeroProcesso = null;
			this.parametros.anoProcesso = null;
		}

		if(this.parametros.numeroAnoSolicitacaoFormatado){
			if(this.parametros.numeroAnoSolicitacaoFormatado.length < 9){
				this.numeroSolicitacao.nativeElement.setCustomValidity('Campo inválido');
			} else {
				let Solicitacao = this.parametros.numeroAnoSolicitacaoFormatado.split("/");
				this.parametros.numeroSolicitacao = Number(Solicitacao[0]);
				this.parametros.anoSolicitacao = Number(Solicitacao[1]);
			}
		} else {
			this.parametros.numeroSolicitacao = -1;
			this.parametros.anoSolicitacao = -1;
		}

		this.idProcesso > 0 ?
			this.parametros.idProcesso = Number(this.idProcesso) :
				this.parametros.idProcesso = 0;

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

			if (this.dataInicio.nativeElement.value == "" && this.dataFim.nativeElement.value == null) {
				this.parametros.dataFim  = this.parametros.dataInicio = null;
			}
			else {
				this.parametros.dataInicio = this.dataInicio.nativeElement.value;
				this.parametros.dataFim = this.dataFim.nativeElement.value;
			}
		}
		else
		{
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

		this.applicationService.get(this.servico, this.parametros).subscribe((result: PagedItems) => {
			if(result != null) {
				if(result.total > 0){
					this.isModificouPesquisa = false;
					this.isBuscaSalva = true;
					this.grid.lista = result.items;
					this.grid.total = result.total;
					this.setarDadosExportacao();
				} else{
					this.grid = { sort: {} };
				}
				this.gravarBusca();
			}
			else{
				this.modal.alerta("Número de processo inexistente.")
			}
		});
    }

	validarData() {
		try {
			this.dataInicio.nativeElement.setCustomValidity('');
			this.dataFim.nativeElement.setCustomValidity('');

			var dataFim = new Date(this.parametros.dataFimString);
			var dataInicio = new Date(this.parametros.dataInicioString);

			if( (this.parametros.dataFimString != null && this.parametros.dataFimString != undefined) &&
				(this.parametros.dataInicioString != null && this.parametros.dataInicioString != undefined)
			 ){
				if (this.parametros.dataInicioString && this.parametros.dataFimString) {
					if (dataFim < dataInicio) {
						this.dataFim.nativeElement.setCustomValidity('Campo inválido. Data inicio maior que data final');
					}
				} else {
					this.parametros.dataInicio = dataInicio;
					this.parametros.dataFim = dataFim;
				}
			}

		} catch (e) {
			this.dataInicio.nativeElement.setCustomValidity('Informe uma data válida.');
		}
	}

	gravarBusca() {
		sessionStorage.removeItem(this.router.url);
		sessionStorage.setItem(this.router.url, JSON.stringify(this.parametros));
	}

	voltar(){

		let arrayUrl = sessionStorage.getItem("arrayUrl");
		let obj = JSON.parse(arrayUrl)
		let url = obj[obj.length - 2]
		obj.pop()
		sessionStorage.removeItem("arrayUrl")
		sessionStorage.setItem("arrayUrl", JSON.stringify(obj))
		
		this.router.navigate([url]);

		sessionStorage.setItem("backToAcompanharProcesso", "true");
	}

	setarDadosExportacao() {
		this.parametros.exportarListagem = true;
		this.parametros.titulo = "MINHAS SOLICITAÇÕES DE ALTERAÇÃO"
		this.parametros.width = { 0: { columnWidth: 150 },
								  1: { columnWidth: 120 },
								  2: { columnWidth: 120 },
								  3: { columnWidth: 160 },
								  4: { columnWidth: 180 } };
		this.parametros.servico = this.servico;
		this.parametros.columns = [
			"Nº Processo",
			"Nº Solicitação",
			"Quantidade de Itens",
			"Data",
			"Status"
		]
			;
		this.parametros.fields = [
			"numeroAnoProcessoFormatado",
			"numeroAnoSolicitacaoFormatado",
			"quantidaDeItens",
			"dataInclusaoFormatada",
			"descricaoStatus"
		];
	}

	retornaValorSessao() {
		if (sessionStorage.getItem(this.router.url) != null && sessionStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(sessionStorage.getItem(this.router.url));
			this.isBuscaSalva = true;
			this.StorageClear = false;
		}
	}

}

