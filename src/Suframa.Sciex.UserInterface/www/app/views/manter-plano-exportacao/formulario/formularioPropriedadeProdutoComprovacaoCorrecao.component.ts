import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import {Location} from '@angular/common';
import { each } from 'jquery';

declare var $: any;

@Component({
	selector: 'app-manter-plano-formulario-propriedade-produto-comprovacao',
	templateUrl: './formularioPropriedadeProdutoComprovacaoCorrecao.component.html'
})

export class ManterPlanoFormularioPropriedadeProdutoComprovacaoCorrecaoComponent implements OnInit {
	isCorrecao = false;
	isQuadroNacional = false;
	formPai = this;
	path: string;
	desabilitado: boolean;
	servico = "PEProduto";
	servicoPlanoExportacao = "PlanoExportacao";
	servicoPais = "PEPaisCorrecao";
	servicoIncluirEditarDue = "IncluirEditarDue";
	somenteLeitura: boolean;
	modelPE: any = {};
	modelProduto: any = {};
	codigo1: string;
	parametros: any = {};
	parametrosPais: any = {};
	parametrosSolic: any = { sort: {}};
	listaPais = [];
	totalpais: number = 0;
	idPEProdutoOpen: number;
	idPEProduto : number;
	gridDadosComprovacao: any = {};
	@Input() sorted: string;
	@Input() page: number;

	@ViewChild('formulario') formulario;
	@ViewChild('appModalAlterarPais') appModalAlterarPais;
	@ViewChild('pais') pais;
	validar: boolean;
	servicoDUE = "DueCorrecao";

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
		this.inicializarPaginacao();
	}
	inicializarPaginacao() {
		this.gridDadosComprovacao.page = 1;
		this.gridDadosComprovacao.size = 10;
	}

	ngOnInit() {
		this.verificarRota();
		this.modelPE = {};
		this.modelProduto = {};
		this.listaPais = [];
	}

	onChangeSort($event) {
		this.parametrosSolic.sort = $event.field;
		this.parametrosSolic.reverse = $event.reverse;
	}
	onChangeSize($event) {
		this.parametrosSolic.size = $event;
	}
	onChangePage($event) {
		this.parametrosSolic.page = $event;
		this.listarDUE();
	}

	changeSortPais($event) {
		this.parametrosPais.sort = $event.field;
		this.parametrosPais.reverse = $event.reverse;
		this.selecionarPaises(this.idPEProduto);
	}
	public verificarRota() {
		this.somenteLeitura = false;
		this.validar = false;

		this.desabilitarTela();

		if (this.path == 'propriedadeprodutocomprovacao') {
			this.selecionarProduto(this.idPEProduto);
			this.somenteLeitura = this.parametros.somenteLeitura = false;
		}else if (this.path == 'propriedadeprodutocomprovacaocorrecao') {
			this.selecionarProduto(this.idPEProduto);
			this.somenteLeitura = this.parametros.somenteLeitura = false;
		}
		else if (this.path == 'validar-propriedadeproduto') {
			this.selecionarProduto(this.idPEProduto);
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.validar = true;
		}
		else if (this.path == 'visualizarpropriedadeprodutocomprovacao') {
			this.selecionarProduto(this.idPEProduto);
			this.somenteLeitura = this.parametros.somenteLeitura = true;
		}
		else if (this.path == 'visualizarpropriedadeprodutocomprovacaocorrecao') {
			this.selecionarProduto(this.idPEProduto);
			this.somenteLeitura = this.parametros.somenteLeitura = true;
		}
	}

	public desabilitarTela() {
		this.desabilitado = true;
	}

	public selecionarProduto(idProduto?: number) {
		let id = idProduto || this.idPEProduto;
		this.applicationService.get(this.servico, id).subscribe((result: any) => {
			this.modelProduto = result;
			this.selecionarPaises(this.idPEProduto);
			this.selecionarPE(result.idPlanoExportacao);
		});
	}
	listarDUE() {
		this.parametrosSolic.idPEProduto = this.idPEProduto;
		this.applicationService.get(this.servicoDUE, this.parametrosSolic).subscribe((result: any) => {
			this.gridDadosComprovacao.lista = result.items;
			this.gridDadosComprovacao.total = result.total;
		});
	}

	public selecionarPaises(id: number) {
		if (!id) { return; }
		
		if (this.parametrosPais == null || this.parametrosPais == undefined){
			this.parametrosPais = {};
		}
		
		this.parametrosPais.idPEProduto = this.idPEProduto;
		this.applicationService.get(this.servicoPais, this.parametrosPais).subscribe((result: any) => {
			this.listaPais = result.items;
			this.totalpais = result.total;
			this.listarDUE();
		});
	}

	public selecionarPE(id: number) {
		if (!id) { return; }
		this.applicationService.get(this.servicoPlanoExportacao, id).subscribe((result: any) => {
			this.modelPE = result;
		});
	}
	limpar() {
		this.pais.clear();
		this.parametros.numero = null;
		this.parametros.quantidade = null;
		this.parametros.valorDolar = null;
		this.parametros.dataAverbacao = null;
	}
	public incluirEditarItemLista(){
		if(this.parametros.codigoPais == null || this.parametros.codigoPais == ""){
			this.modal.alerta("Preencha o campo 'País'", "Informação");
			return;
		}else if (this.parametros.numero == null || this.parametros.numero == ""){
			this.modal.alerta("Preencha o campo 'DU-E'", "Informação");
			return;
		}
		else if (this.parametros.quantidade == null || this.parametros.quantidade == ""){
			this.modal.alerta("Preencha o campo 'Quantidade Total'", "Informação");
			return;
		}
		else if (this.parametros.valorDolar == null || this.parametros.valorDolar == ""){
			this.modal.alerta("Preencha o campo 'Valor Total (US$)''", "Informação");
			return;
		}
		else if (this.parametros.quantidade <= 0){
			this.modal.alerta("Informe uma quantidade válida.", "Informação");
			return;
		}
		else if (this.parametros.valorDolar <= 0){
			this.modal.alerta("Informe um valor válido.", "Informação");
			return;
		}
		else if (this.parametros.numero <= 0){
			this.modal.alerta("informe um numero DUE válido.", "Informação");
			return;
		}
		this.parametros.quantidade = Number(this.parametros.quantidade);
		this.parametros.valorDolar = Number(this.parametros.valorDolar);
		this.parametros.idPEProduto = this.idPEProduto;
		if(this.parametros.idDue == null){
			this.applicationService.post(this.servicoIncluirEditarDue, this.parametros).subscribe((result:any) =>{
				if (result.sucesso){
					this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, 'Sucesso', '');
					this.selecionarProduto();
					console.log(result.retornoString);
					this.limpar();
				}else{
					this.modal.alerta(result.retornoString, 'Atenção', '');
					console.log(result.retornoString);
				}
			});
		}else{
			this.applicationService.put(this.servicoIncluirEditarDue, this.parametros).subscribe((result:any) =>{
				if (result.sucesso){
					this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, 'Sucesso', '');
					this.selecionarProduto();
					this.parametros.idDue = null;
					this.limpar();
				}else{
					this.modal.alerta(result.retornoString, 'Atenção', '');
					console.log(result.retornoString);
				}
			});
		}

	}
	public incluirPais(){
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
	formatarParaConverter(value): string{
		let valor = value.replace('.','');
		valor = valor.replace(',','.');
		return valor;
	}

	public abrirModalAlterar(item){
		this.appModalAlterarPais.abrir(this,item);
	}


	voltar(){
		this.Location.back();
	}


	onBlurQtTotal(event){
		if(event.target.value !== ''){

			let value = Number(this.replaceVirgula(event.target.value));

			if (isNaN(value)){
				this.modal.alerta("Número inválido").subscribe(()=>{
					event.target.value = '';
				})
			}else{
				this.parametros.quantidade = (value as number).toLocaleString('pt-BR',{style:"decimal",maximumFractionDigits:5});
			}
		}
	}

	onBlurVlrTotal(event){
		if(event.target.value !== ''){

			let value = Number(this.replaceVirgula(event.target.value));

			if (isNaN(value)){
				this.modal.alerta("Número inválido").subscribe(()=>{
					event.target.value = '';
				})
			}else{
				this.parametros.valorDolar = (value as number).toLocaleString('pt-BR',{style:"decimal",maximumFractionDigits:2});
			}
		}
	}

	replaceVirgula(value): string{
		return value.replace(',','.');
	}
}
