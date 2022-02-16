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
	selector: 'app-manter-plano-formulario-detalhes-insumos',
	templateUrl: './formulariodetalhesInsumos.component.html',
	styleUrls: ['../grid/grid-insumos.component.css']
})

export class ManterPlanoFormularioDetalhesInsumosComponent implements OnInit {
	idPEInsumo: number;
	servicoInsumo = "PEInsumo";
	servicoCorrigirDadosInsumo = "CorrigirDadosInsumo";
	servicoDetalhe = "PEDetalheInsumo";
	servicoBuscarDetalhePorIdInsumo = "ListarDetalheInsumoPorIdInsumo";
	modelInsumo: any = {};
	listaDetalhes = [];
	modelDetalhes: any = {};
	
	titulo: string;
	subtitulo: string;
	isQuadroNacional: boolean;
	grid: any = { sort: {} };
	path: string;
	desabilitado: boolean;
	servico = "PEProduto";
	servicoPais = "PEPais";
	servicoCorrigirDetalheInsumo = 'CorrigirPEDetalheInsumo';
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
	@ViewChild('appModalEditarDetalheInsumo') appModalEditarDetalheInsumo;
	@ViewChild('pais') pais;
	@ViewChild('moeda') moeda;
	@ViewChild("modalJustificativaErro") modalJustificativaErro;
	validar: boolean;
	coeficienteTecnico: any;
	qtdProduto: any;
	percentualPerda: any;
	isCorrecao: boolean = false;
	tituloCorrecao: string = "";
	

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
		this.idPEInsumo = this.route.snapshot.params['id'];
	}

	ngOnInit() {
		this.parametros = {}
		this.verificarRota();
		this.modelPE = {};
		this.modelProduto = {};
		this.listaPais = [];
	}

	limparSelects(){
		this.pais.onClear(true);
		this.moeda.onClear(true);
	}

	public verificarRota() {
		this.somenteLeitura = false;
		this.validar = false;
		this.titulo = "-";
		this.subtitulo = "-";
		this.isQuadroNacional = false;

		this.desabilitarTela();

		if (this.path == 'detalhe-nacional') {
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.titulo = "Nacional ou Regional";
			this.subtitulo = "Nacional ou Regional";
			this.isQuadroNacional = true;
			this.selecionarInsumo(this.idPEInsumo);
		}
		else if (this.path == 'detalhe-nacional-correcao') {
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.titulo = "Nacional ou Regional";
			this.subtitulo = "Nacional ou Regional";
			this.isQuadroNacional = true;
			this.isCorrecao = true;
			this.tituloCorrecao = "Solicitar Correção ";
			this.selecionarInsumo(this.idPEInsumo);
		}
		else if (this.path == 'detalhe-nacional-visualizar') {
			this.somenteLeitura = this.parametros.somenteLeitura = true;
			this.titulo = "Nacional ou Regional";
			this.subtitulo = "Nacional ou Regional";
			this.isQuadroNacional = true;
			this.selecionarInsumo(this.idPEInsumo);
		}
		else if (this.path == 'detalhe-importado') {
			this.isQuadroNacional = false;
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.titulo = "Importado";
			this.subtitulo = "Padrão ou Extra Padrão";
			this.selecionarInsumo(this.idPEInsumo);
		}
		else if (this.path == 'detalhe-importado-correcao') {
			this.isQuadroNacional = false;
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.titulo = "Importado";
			this.subtitulo = "Padrão ou Extra Padrão";
			this.isCorrecao = true;
			this.tituloCorrecao = "Solicitar Correção ";
			this.selecionarInsumo(this.idPEInsumo);
		}
		else if (this.path == 'detalhe-importado-visualizar') {
			this.isQuadroNacional = false;
			this.somenteLeitura = this.parametros.somenteLeitura = true;
			this.titulo = "Importado";
			this.subtitulo = "Padrão ou Extra Padrão";
			this.selecionarInsumo(this.idPEInsumo);
		}
		else if (this.path == 'validar-detalhe-nacional') {
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.validar = true;
			this.titulo = "Nacional ou Regional";
			this.subtitulo = "Nacional ou Regional";
			this.isQuadroNacional = true;
			this.selecionarInsumo(this.idPEInsumo);
		}
		else if (this.path == 'validar-detalhe-importado') {
			this.isQuadroNacional = false;
			this.somenteLeitura = this.parametros.somenteLeitura = false;
			this.validar = true;
			this.titulo = "Importado";
			this.subtitulo = "Padrão ou Extra Padrão";
			this.selecionarInsumo(this.idPEInsumo);
		}
	}

	public desabilitarTela() {
		this.desabilitado = true;
	}

	public selecionarInsumo(id: number) {
		if (!id) { return; }
		this.applicationService.get(this.servicoInsumo, id).subscribe((result: any) => {
			this.modelInsumo = result;
			this.coeficienteTecnico = result.valorCoeficienteTecnico || 0;
			this.percentualPerda = result.valorPercentualPerda || 0;
			this.idPEProduto = result.idPEProduto;
			this.carregarDadosDetalheInsumo(result.idPEInsumo);
			this.selecionarProduto(result.idPEProduto)
		});
	}
	carregarDadosDetalheInsumo(idPEInsumo: any) {
		if (!idPEInsumo) { return; }
		let dados = {
			IdPEInsumo:idPEInsumo,
			isQuadroNacional: this.isQuadroNacional,
			isCorrecao: this.isCorrecao
		}
		this.applicationService.get(this.servicoBuscarDetalhePorIdInsumo, dados).subscribe((result: any) => {
			this.modelDetalhes = result;
			this.listaDetalhes = result.listaDetalhesInsumos;
		});
	}

	public selecionarProduto(id: number) {
		if (!id) { return; }
		this.applicationService.get(this.servico, id).subscribe((result: any) => {
			this.modelProduto = result;
			this.qtdProduto = result.qtd || 0;
			this.calcularQtdMaxima();
		});
	}
	calcularQtdMaxima() {
		// let qtdMaxima = this.qtdProduto * this.coeficienteTecnico;

		// this.parametros.qtdMaxima = qtdMaxima + (qtdMaxima * (this.percentualPerda / 100));

		let dados = {
			QtdProduto: this.qtdProduto,
			ValorPercentualPerda: this.percentualPerda,
			ValorCoeficienteTecnico: this.coeficienteTecnico,

		}
		this.applicationService.put("PEInsumoQtdMaxFormat", dados).subscribe((result: any) => {
			this.parametros.qtdMaxima = result;
		});
	}

	replaceVirgula(value): string{
		return value.replace(',','.');
	}
	onBlurQtTotal(event){
		if(event.target.value !== ''){

			let value = Number(this.replaceVirgula(event.target.value));

			if (isNaN(value)){
				this.modal.alerta("Número inválido").subscribe(()=>{
					event.target.value = '';
				})
			}else{
				this.parametros.qtd = (value as number).toLocaleString('pt-BR',{style:"decimal",maximumFractionDigits:5});
			}
		}
	}

	onBlurVlrUnitario(event){
		if(event.target.value !== ''){

			let value = Number(this.replaceVirgula(event.target.value));

			if (isNaN(value)){
				this.modal.alerta("Número inválido").subscribe(()=>{
					event.target.value = '';
				})
			}else{
				this.parametros.valorUnitario = (value as number).toLocaleString('pt-BR',{style:"decimal",maximumFractionDigits:7});
			}
		}
	}

	onBlurVlrUnitarioFOB(event){
		if(event.target.value !== ''){

			let value = Number(this.replaceVirgula(event.target.value));

			if (isNaN(value)){
				this.modal.alerta("Número inválido").subscribe(()=>{
					event.target.value = '';
				})
			}else{
				this.parametros.valorUnitarioFOB = (value as number).toLocaleString('pt-BR',{style:"decimal",maximumFractionDigits:7});
			}
		}
	}
	onBlurVlrUnitarioFrete(event){
		if(event.target.value !== ''){

			let value = Number(this.replaceVirgula(event.target.value));

			if (isNaN(value)){
				this.modal.alerta("Número inválido").subscribe(()=>{
					event.target.value = '';
				})
			}else{
				this.parametros.valorFrete = (value as number).toLocaleString('pt-BR',{style:"decimal",maximumFractionDigits:7});
			}
		}
	}

	public incluirDetalhe(){
		
		if(this.isQuadroNacional) {
			
			if(this.parametros.qtd == null || this.parametros.qtd == ""){
				this.modal.alerta("Preencha o campo 'Quantidade'", "Informação");
				return;
			}else if (this.parametros.valorUnitario == null || this.parametros.valorUnitario == ""){
				this.modal.alerta("Preencha o campo 'Valor Unitário'", "Informação");
				return;
			}

			this.parametros.qtd = Number(this.parametros.qtd);
			this.parametros.valorUnitario = Number(this.parametros.valorUnitario);

		}
		else{
			if (this.parametros.codigoPais == null || this.parametros.codigoPais == undefined){
				this.modal.alerta("Preencha o campo 'País'", "Informação");
				return;
			}		
			
			//this.parametros.codigoPais = this.pais.valorInput.nativeElement.value.split("|")[1];

			if (this.parametros.idMoeda == null || this.parametros.idMoeda == undefined){
				this.modal.alerta("Preencha o campo 'País'", "Informação");
				return;
			}
			
			if(this.parametros.qtd == null || this.parametros.qtd == ""){
				this.modal.alerta("Preencha o campo 'Quantidade'", "Informação");
				return;
			}else if (this.parametros.valorUnitarioFOB == null || this.parametros.valorUnitarioFOB == ""){
				this.modal.alerta("Preencha o campo 'Valor Unitário'", "Informação");
				return;
			}

			this.parametros.qtd = Number(this.parametros.qtd);
			this.parametros.valorUnitarioFOB = Number(this.parametros.valorUnitarioFOB);
			this.parametros.valorFrete = Number(this.parametros.valorFrete);
			
		}
		

		this.parametros.idPEProduto = this.idPEProduto;
		this.parametros.idPEInsumo = this.idPEInsumo;
		this.parametros.isQuadroNacional = this.isQuadroNacional;
		this.applicationService.post(this.servicoDetalhe, this.parametros).subscribe((result: any) => {			
			if (result == 1) {
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informação", "");
				return;
			}else if (result == 2){				
				this.modal.resposta("Detalhe já cadastrado", "Informação", "");
				return;
			}
			else if (result == 3){				
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informação", "")
				.subscribe(()=>{
					
					if (!this.isQuadroNacional){
						this.limparSelects();
					}
					this.ngOnInit()
				});
			}
			else if (result == 4){				
				this.modal.alerta("A quantidade total excede a quantidade máxima permitida", "Informação", "");
				return;
			}
		});
	}

	formatarParaConverter(value): string{
		let valor = value.replace('.','');
		valor = valor.replace(',','.');
		return valor;
	}
	confirmarExclusaoDetalhe(item) {
		this.modal.confirmacao("Confirma a operação?", '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.excluirDetalhe(item);
				}
			});
	}
	public excluirDetalhe(item) {
		let id = item.idPEDetalheInsumo;
		this.applicationService.delete(this.servicoDetalhe, id).subscribe(result => {
			if (!result) {
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informação", "");
				return;
			}
			else{				
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informação", "")
				.subscribe(()=>{
					this.ngOnInit()
				});
			}
		});			
	}

	abrirModalAlterar(item){
		let idPEDetalheInsumo = item.idPEDetalheInsumo;
		let dados = {
			quantidade: item.quantidade,
			valorUnitario: item.valorUnitario,
			valorFrete:item.valorFrete,
			codigoInsumo: this.modelInsumo.codigoInsumo,
			isQuadroNacional: this.isQuadroNacional,
			isCorrecao: this.isCorrecao
		}
		this.appModalEditarDetalheInsumo.abrir(this,idPEDetalheInsumo,dados);
	}

	alterarInsumo(){
		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.salvarAlteracaoInsumo();
				}
			});
	}

	salvarAlteracaoInsumo(){
		let dados: any = {};
		dados.idPEInsumo = this.idPEInsumo;
		dados.descricaoPartNumber = this.modelInsumo.descricaoPartNumber;
		dados.valorPercentualPerda = this.modelInsumo.valorPercentualPerda;

		if(!this.isCorrecao) {
			this.applicationService.put(this.servicoInsumo, dados).subscribe(result => {
				if (!result) {
					this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informação", "");
					return;
				}
				else{				
					this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informação", "")
					.subscribe(()=>{
						this.Location.back();
					});
				}
			});
		}
		else{
			this.applicationService.put(this.servicoCorrigirDadosInsumo, dados).subscribe((retorno:any) => {
				
				if (retorno.resultado) {
					this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informação", "")
					.subscribe(()=>{
						this.Location.back();
					});
				}
				else{				
					this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informação", "")
					.subscribe(()=>{
						console.log(retorno.mensagem);
					});
					return;
				}
			});
		}
	}

	


	public selecionarPaises(id: number) {
		if (!id) { return; }
		this.parametros.idPEProduto = this.idPEProduto;
		this.applicationService.get(this.servicoPais, this.parametros).subscribe((result: any) => {
			this.listaPais = result.items;
			this.totalpais = result.total;
		});
	}

	

	confirmaCancelar(){
		this.modal.confirmacao(this.msg.CONFIRMAR_CONFIRMACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.Location.back();
				}
			});
	}
	cancelar(){
		this.Location.back();
	}

	confirmarInativacao(item){

		this.modal.confirmacao("Confirma a operação?","Confirmação","")
		.subscribe(result => {
			if (result){
				this.inativarInsumo(item);
			}
		})
	}

	inativarInsumo(item){
		this.applicationService.post(this.servicoCorrigirDetalheInsumo, item).subscribe((retorno: any) => {
			
			if (retorno.resultado) {
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informação", "")
				.subscribe(()=>{
					this.ngOnInit();
				});				
			}else{				
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informação", "")
				.subscribe(()=>{
					console.log(retorno.mensagem);
				});
				return;
			}
		});
	}

	abrirModalJustificativaErro(item){
		this.modalJustificativaErro.abrir(item);
	}
}
