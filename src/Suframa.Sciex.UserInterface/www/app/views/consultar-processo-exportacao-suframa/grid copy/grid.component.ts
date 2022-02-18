import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
	selector: 'app-consultar-processo-exportacao-suframa-grid',
	templateUrl: './grid.component.html'
})

export class ConsultarProcessoExportacaoSuframaGridComponent {
	servico = 'PlanoExportacao';
	servicoEntregarPlano = 'EntregarPlanoExportacao';
	servicoValidarPlano = 'ValidarPlanoExportacao';
	servicoConsultarProcesso="ConsultarProcessoImportacao";
	validarProg:boolean
	@ViewChild('appModalParecer') appModalParecer;
	@ViewChild('appModalAnalisarPedidoProrrogacao') appModalAnalisarPedidoProrrogacao;
	@ViewChild('appModalCertificado') appModalCertificado;
	@ViewChild('appModalDescricaoObservacaoSuframa') appModalDescricaoObservacaoSuframa;
	servicoSelecionarProdutoEmAnalisePorIdProcesso = "SelecionarProdutoEmAnalisePorIdProcesso";

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
	) { }

	@Input() lista: any[];
	@Input() total: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() page: number;
	@Input() isUsuarioInterno: boolean = false;
	@Input() exportarListagem: boolean;
	@Input() parametros: any = {};
	@Input() formPai: any;

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();

	abrirVisualizar(idProcesso){

		let url = `/consultar-processo-exportacao-suframa/${idProcesso}/visualizar`;
		this.setHistoryUrl(url)
		this.router.navigate([url])
	}
	setHistoryUrl(url){
		let arrayUrl = sessionStorage.getItem("arrayUrl");
		let listArray = JSON.parse( arrayUrl)
		listArray.push(url)
		sessionStorage.removeItem("arrayUrl");
		sessionStorage.setItem("arrayUrl",JSON.stringify(listArray));
	}
	changeSize($event) {
		this.onChangeSize.emit($event);
	}

	changeSort($event) {
		this.sorted = $event.field;
		this.onChangeSort.emit($event);
		this.changePage(this.page);
	}

	changePage($event) {
		this.page = $event;
		this.onChangePage.emit($event);
		console.log($event);
	}

	// ngOnInit(){
	// 	this.applicationService.get(this.servicoConsultarProcesso).subscribe((result: any) => {
	// 		console.log(result.definidorGrid)
	// 		if (result.definidorGrid == 0){
	// 		this.validarProg =false;
	// 		console.log(this.validarProg)

	// 		}else if(result.definidorGrid == 1){
	// 		this.validarProg =true;
	// 		console.log(this.validarProg)
	// 		}

	// 	});

	// }
	async abrirTelaVisualizaQuadroImportado(idProcesso){
		(await this.selecionarPrimeiroProdutoComSolicitacaoEmAnalise(idProcesso)).subscribe( (registroProduto:any) =>{
			if (registroProduto != null){

				let url = `/consultar-processo-exportacao-suframa/${registroProduto.idProduto}/visualizar-quadro-importado`;
				this.setHistoryUrl(url)
				this.router.navigate([url])
			}
		});
	}
	async selecionarPrimeiroProdutoComSolicitacaoEmAnalise<T>(idProcesso: any){

		let id = idProcesso;
		return this.applicationService.get(this.servicoSelecionarProdutoEmAnalisePorIdProcesso, id);
	}
	confirmarCopia(item){

		if (item.tipoModalidade != "S" && item.tipoExportacao != "AP"){
			this.modal.alerta("Somente poderão ser copiados Planos na modalidade SUSPENSÃO do tipo APROVAÇÃO");
			return;
		}

		this.modal.confirmacao("Deseja criar uma nova cópia de Plano a partir do Plano selecionado?","Confirmação","")
		.subscribe(result => {
			if (result){
				this.gerarCopia(item);
			}
		})
	}
	gerarCopia(item: any) {

		this.applicationService.put(this.servico, item).subscribe((sucesso: any) => {

			if (!sucesso) {
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informação", "");
				return;
			}else{
				this.formPai.listar();
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informação", "");
			}
		});
	}

	confirmarEntrega(item){

		this.modal.confirmacao("Confirma a operação?","Confirmação","")
		.subscribe(result => {
			if (result){
				this.validar(item, true);
			}
		})
	}

	abrirModalParecer(item){
		this.appModalParecer.abrir(item);
	}

	abrirModalAnalisarPedidoProrrogacao(idProcesso){

		this.appModalAnalisarPedidoProrrogacao.abrir(idProcesso, this.formPai);
	}
	abrirModalCertificado(item){
		this.appModalCertificado.abrir(item);
	}
	abrirModalDescricaoObservacaoSuframa(item){
		this.appModalDescricaoObservacaoSuframa.abrir(item);
	}
	processarEntrega(item: any) {
		this.applicationService.post(this.servicoEntregarPlano, item).subscribe((result: any) => {
			if (!result.resultado) {
				if(result.mensagem != null && result.mensagem != ""){
					this.modal.alerta(result.mensagem, "Informação", "");
				}
				else {
					this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informação", "");
				}
				return;
			}else{
				this.formPai.listar();
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informação", "");
			}
		});
	}

	prorrogarAparecer(){

	}

	validar(item, realizarEntrega: boolean){
		let idPlanoExportacao = item.idPlanoExportacao;
		this.applicationService.post(this.servicoValidarPlano, item).subscribe((result: any) => {

			if (!result.resultado) {
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO + ": "+result.mensagem, "Atenção", "");
			}else if(result.camposNaoValidos == null){
				this.modal.resposta("Plano sem erros de validação", "Informação", "")
				.subscribe(()=>{
					if (realizarEntrega){
						this.processarEntrega(item);
					}
				});
			}else if(result.camposNaoValidos != null){

				if (result.camposNaoValidos.naoExisteProduto){
					this.modal.confirmacao("Erro na validação. Plano não possui produto cadastrado, deseja corrigir os dados?","Confirmação","")
					.subscribe(result => {
						if (result){
							this.router.navigate([`manter-plano-exportacao/${idPlanoExportacao}/validar-produto`])
						}
					});
				}
				else if (result.camposNaoValidos.naoExistePais){
					let idProduto = result.camposNaoValidos.idProduto;
					this.modal.confirmacao("Erro na validação. Existem produtos sem cadastro de país, deseja corrigir os dados?","Confirmação","")
					.subscribe(result => {
						if (result){
							this.router.navigate([`manter-plano-exportacao/${idProduto}/validar-propriedadeproduto`])
						}
					});
				}
				else if (result.camposNaoValidos.naoExisteInsumo){
					let idProduto = result.camposNaoValidos.idProduto;

					this.modal.confirmacao("Erro na validação. Existem produtos que não possuem insumos cadastrados, deseja corrigir os dados?","Confirmação","")
					.subscribe(result => {
						if (result){
							this.router.navigate([`manter-plano-exportacao/${idPlanoExportacao}/validar-insumo`])
						}
					});
				}
				else if (result.camposNaoValidos.naoExisteDetalhe){
					let idInsumo = result.camposNaoValidos.idInsumo;
					let isNacional = result.camposNaoValidos.isNacional;
					this.modal.confirmacao("Erro na validação. Existem insumos que não possuem detalhes cadastrados, deseja corrigir os dados?","Confirmação","")
					.subscribe(result => {
						if (result){
							if (isNacional){
								this.router.navigate([`manter-plano-exportacao-detalhes-insumos/${idInsumo}/validar-detalhe-nacional`]);
							}
							else{
								this.router.navigate([`manter-plano-exportacao-detalhes-insumos/${idInsumo}/validar-detalhe-importado`]);
							}
						}
					});
				}else if (result.camposNaoValidos.naoExisteParidadeCambial){
					this.modal.alerta("Erro na validação. Não existe valor de paridade cambial para a data de hoje.","Atenção","");
				}
				else if (result.camposNaoValidos.naoExisteParidadeCambialEstrangeira){
					this.modal.alerta("Erro na validação. Não existe valor de paridade cambial estrangeira para a data de hoje.","Atenção","");
				}
			}
		});
	}

	excluirInsumo(dados) {
		this.modal.confirmacao("Confirma a operação?","Confirmação","")
		.subscribe(result => {
			if (result){
				this.realizarExcluirInsumo(dados);
			}
		});
	}
	realizarExcluirInsumo(dados){
		let id = dados.idPlanoExportacao;
		this.applicationService.delete(this.servico, id).subscribe((result: any) => {

			if (!result) {
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informação", "");
				return;
			}else{
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informação", "")
				.subscribe(()=>{
					this.formPai.ngOnInit();
				});
			}
		});
	}

}
