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
	selector: 'app-manter-plano-formulario-propriedade-produto',
	templateUrl: './formularioPropriedadeProduto.component.html'
})

export class ManterPlanoFormularioPropriedadeProdutoComponent implements OnInit {
	path: string;
	desabilitado: boolean;
	servico = "PEProduto";
	servicoPlanoExportacao = "PlanoExportacao";
	servicoPais = "PEPais";
	somenteLeitura: boolean;
	modelPE: any = {};
	modelProduto: any = {};
	codigo1: string;
	parametros: any = {};
	listaPais = [];
	totalpais: number = 0;
	idPEProdutoOpen: number;
	idPEProduto : number;

	@Input() sorted: string;
	@Input() page: number;

	@ViewChild('formulario') formulario;
	@ViewChild('appModalAlterarPais') appModalAlterarPais;
	@ViewChild('pais') pais;
	validar: boolean;

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

	ngOnInit() {
		this.verificarRota();
		this.modelPE = {};
		this.modelProduto = {};
		this.listaPais = [];
	}

	changeSort($event) {
		this.sorted = $event.field;
		//this.onChangeSort.emit($event);
		this.changePage(this.page);
	}

	changePage($event) {
		this.page = $event;
		//this.onChangePage.emit($event);
		//console.log($event);
	}

	public verificarRota() {
		this.somenteLeitura = false;
		this.validar = false;

		this.desabilitarTela();

		if (this.path == 'propriedadeproduto') {
			this.selecionarProduto(this.idPEProduto);
			this.somenteLeitura = this.parametros.somenteLeitura = false;
		}
		else if (this.path == 'validar-propriedadeproduto') {
			this.selecionarProduto(this.idPEProduto);
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.validar = true;
		}
		else if (this.path == 'visualizarpropriedadeproduto') {
			this.selecionarProduto(this.idPEProduto);
			this.somenteLeitura = this.parametros.somenteLeitura = true;
		}
	}

	public desabilitarTela() {
		this.desabilitado = true;
	}

	public selecionarProduto(id: number) {
		if (!id) { return; }
		this.applicationService.get(this.servico, id).subscribe((result: any) => {
			this.modelProduto = result;

			this.selecionarPaises(this.idPEProduto);
			this.selecionarPE(result.idPlanoExportacao);
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

	public selecionarPE(id: number) {
		if (!id) { return; }
		this.applicationService.get(this.servicoPlanoExportacao, id).subscribe((result: any) => {
			this.modelPE = result;
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

		this.parametros.quantidade = Number(this.parametros.quantidade);
		this.parametros.valorDolar = Number(this.parametros.valorDolar);
		
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
