import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { PagedItems } from '../../../view-model/PagedItems';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import {Location} from '@angular/common';

declare var $: any;

@Component({
	selector: 'app-manter-plano-formulario-quadros-insumos',
	templateUrl: './formularioQuadrosInsumos.component.html'
})

export class ManterPlanoFormularioQuadrosInsumosComponent implements OnInit {
	titulo: string;
	subtitulo: string;
	isQuadroNacional: boolean;
	grid: any = { sort: {} };
	path: string;
	desabilitado: boolean;
	servico = "PEProduto";
	servicoPlanoExportacao = "PlanoExportacao";
	servicoPais = "PEPais";
	servicoListarInsumos = "ListarInsumosNacionalOuImportadoPorIdProduto";
	servicoListarInsumosParaCorrecao = "ListarInsumosNacionalOuImportadoPorIdProdutoParaCorrecao";
	formPai = this;

	somenteLeitura: boolean;
	modelPE: any = {};
	modelProduto: any = {};
	codigo1: string;
	parametros: any = {};
	listaPais = [];
	totalpais: number = 0;
	idPEProdutoOpen: number;
	idPEProduto : number;

	@ViewChild('formulario') formulario;
	@ViewChild('appModalIncluirInsumo') appModalIncluirInsumo;
	@ViewChild('pais') pais;
	validar: boolean;
	isCorrecao: boolean = false;

	constructor(
		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private modal: ModalService,
		private router: Router,
		private validationService: ValidationService,
		private Location: Location,
	) {
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.idPEProduto = this.route.snapshot.params['id'];
	}

	onChangeSort($event) {
		this.grid.sort = $event;
	}
	onChangeSize($event) {
		this.grid.size = $event;
	}
	onChangePage($event) {
		this.grid.page = $event;
		this.carregarInsumos(this.idPEProduto);
	}
	carregarOpcoesPaginacaoInicial(){
		this.parametros.page = 1;
		this.parametros.size = 10;
		this.grid.page = 1;
		this.grid.size = 10;
	}

	ngOnInit() {
		this.parametros = {};
		this.modelPE = {};
		this.modelProduto = {};
		this.listaPais = [];
		this.verificarRota();
	}

	public verificarRota() {
		this.carregarOpcoesPaginacaoInicial();
		this.somenteLeitura = false;
		this.validar = false;
		this.titulo = "-";
		this.subtitulo = "-";
		this.isQuadroNacional = false;

		this.desabilitarTela();

		if (this.path == 'quadro-nacional') {
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.titulo = "Manter Insumos Nacionais e Regionais Quadro II";
			this.subtitulo = "Cadastro de Insumos dos tipos Nacional e Regional";
			this.isQuadroNacional = true;
			this.selecionarProduto(this.idPEProduto);
		}
		else if (this.path == 'quadro-nacional-correcao') {
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.titulo = "Insumos Nacionais e Regionais Quadro II";
			this.subtitulo = "Solicitar Correção Insumos Nacional e Regional";
			this.isQuadroNacional = true;
			this.isCorrecao = true;
			this.selecionarProduto(this.idPEProduto);
		}
		else if (this.path == 'quadro-nacional-visualizar') {
			this.somenteLeitura = this.parametros.somenteLeitura = true;
			this.titulo = "Manter Insumos Nacionais e Regionais Quadro II";
			this.subtitulo = "Cadastro de Insumos dos tipos Nacional e Regional";
			this.isQuadroNacional = true;
			this.selecionarProduto(this.idPEProduto);
		}
		else if (this.path == 'quadro-importado') {
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.titulo = "Manter Insumos Importados Quadro III";
			this.subtitulo = "Cadastro de Insumos dos tipos Padrão e Extra Padrão";
			this.isQuadroNacional = false;
			this.selecionarProduto(this.idPEProduto);
		}
		else if (this.path == 'quadro-importado-correcao') {
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.titulo = "Insumos Importados Quadro III";
			this.subtitulo = "Solicitar Correção Insumos Padrão e Extra Padrão";
			this.isQuadroNacional = false;
			this.isCorrecao = true;
			this.selecionarProduto(this.idPEProduto);
		}
		else if (this.path == 'quadro-importado-visualizar') {
			this.somenteLeitura = this.parametros.somenteLeitura = true;
			this.titulo = "Manter Insumos Importados Quadro III";
			this.subtitulo = "Cadastro de Insumos dos tipos Padrão e Extra Padrão";
			this.isQuadroNacional = false;
			this.selecionarProduto(this.idPEProduto);
		}
		else if (this.path == 'validar-quadro-nacional') {
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.validar = true;
			this.isQuadroNacional = false;
			this.selecionarProduto(this.idPEProduto);
		}
		else if (this.path == 'validar-quadro-importado') {
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.validar = true;
			this.isQuadroNacional = false;
			this.selecionarProduto(this.idPEProduto);
		}
	}

	public abrirModalIncluirInsumo(){
		this.appModalIncluirInsumo.abrir(this,this.isQuadroNacional,this.idPEProduto);
	}

	public desabilitarTela() {
		this.desabilitado = true;
	}

	public selecionarProduto(id: number) {
		if (!id) { return; }
		this.applicationService.get(this.servico, id).subscribe((result: any) => {
			this.modelProduto = result;

			this.selecionarPaises(this.idPEProduto);
			this.carregarInsumos(this.idPEProduto);
			this.selecionarPE(result.idPlanoExportacao);
		});
	}

	carregarInsumos(id:number){
		if (!id) { return; }

		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;

		this.parametros.isQuadroNacional = this.isQuadroNacional;
		this.parametros.idPEProduto = id;
		this.parametros.isCorrecao = this.isCorrecao;

		if(!this.isCorrecao){
			this.applicationService.get(this.servicoListarInsumos, this.parametros).subscribe((result: PagedItems) => {
				this.grid.lista = result.items;
				this.grid.total = result.total;
			});
		}
		else{
			this.applicationService.get(this.servicoListarInsumosParaCorrecao, this.parametros).subscribe((result: PagedItems) => {
				this.grid.lista = result.items;
				this.grid.total = result.total;
			});
		}
	}

	public selecionarPE(id: number) {
		if (!id) { return; }
		this.applicationService.get(this.servicoPlanoExportacao, id).subscribe((result: any) => {
			this.modelPE = result;
		});
	}
	public selecionarPaises(id: number) {
		if (!id) { return; }
		this.parametros.idPEProduto = this.idPEProduto;
		this.applicationService.get(this.servicoPais, this.parametros).subscribe((result: any) => {
			this.listaPais = result.items;
			this.totalpais = result.total;
		});
	}

	public incluirPais(){
		if(this.parametros.codigoPais == null || this.parametros.codigoPais == ""){
			this.modal.alerta("Preencha o campo 'País'", "Informação");
			return;
		}else if (this.parametros.quantidade == null || this.parametros.quantidade == ""){
			this.modal.alerta("Preencha o campo 'Quantidade Total'", "Informação");
			return;
		}
		else if (this.parametros.valorDolar == null || this.parametros.valorDolar == ""){
			this.modal.alerta("Preencha o campo 'Valor Total (US$)''", "Informação");
			return;	
		}

		this.applicationService.put(this.servicoPais, this.parametros).subscribe((result: any) => {			
			if (result.mensagem != null && result.mensagem != '') {
				this.modal.alerta(result.mensagem, "Informação", "");
				return;
			}
			else if (result.mensagem == null || result.mensagem == ''){
				this.pais.clear();
				this.parametros = {};
				this.selecionarProduto(this.idPEProduto);
				this.modal.resposta("Salvo com sucesso!", "Informação", "");
			}
		});
	}

	voltar(){
		this.Location.back();
	}

	public excluirPais(item) {
		this.applicationService.delete(this.servicoPais, item.idPEProdutoPais).subscribe(result => {
			const index: number = this.listaPais.indexOf(item);
			if (index !== -1)
				this.listaPais.splice(index, 1);

			this.selecionarProduto(this.idPEProduto);
		}, error => {
			const index: number = this.listaPais.indexOf(item);
			if (index !== -1)
				this.listaPais.splice(index, 1);
		});			
	}

	confirmarExclusaoPais(item) {
		this.modal.confirmacao("Confirma a operação?", '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.excluirPais(item);
				}
			});
	}
}
