import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../../shared/services/application.service';
import { MessagesService } from '../../../../shared/services/messages.service';
import { ModalService } from '../../../../shared/services/modal.service';
import { manterPliVM } from '../../../../view-model/ManterPliVM';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalSolicitarAlteracaoQuantidadeCoeficienteTecnicoComponent } from '../../modal/modais-solicitacao-alteracao/quantidade/modal-quantidade-coeficiente-tecnico.component';
import { ModalPaisComponent } from '../../modal/modais-solicitacao-alteracao/pais/modal-pais.component';
import { ModalMoedaComponent } from '../../modal/modais-solicitacao-alteracao/moeda/modal-moeda.component';
import { ModalSolicitacaoDetalhadaComponent } from '../../modal/modais-solicitacao-alteracao/modal-visualizar-solicitacao-detalhada/solicitacao-detalhada-alteracao.component';
import { ConsultarPlanoFormularioDetalhesInsumosComponent } from '../../formulario/formulario-detalhes-insumos.component';
import { ModalSolicitarInclusaoInsumoComponent } from '../../modal/modal-solicitar-inclusao-insumo.component';
import { ModalSolicitacaoComponent } from '../../modal/modais-solicitacao-alteracao/solicitacoes-alteracao/solicitacao-alteracao.component';
//import {ModalAnaliseSolicitacaoComponent} from '../../modal/modais-solicitacao-alteracao/solicitacoes-alteracao/solicitacao-alteracao.component';
import { ModalSolicitarAlteracaoValorUnitarioComponent } from '../../modal/modais-solicitacao-alteracao/valor-unitario/modal-valor-unitario.component';
import { ModalSolicitarAlteracaoValorFreteComponent } from '../../modal/modais-solicitacao-alteracao/valor-frete/modal-valor-frete.component';

enum EnumStatusRetornoRequisicao {	
	ERRO = 0,
	SUCESSO = 1,
	PARIDADE_CAMBIAL_NAO_CADASTRADA = 2,
	NAO_EXISTE_SOLCITACAO_ALTERACAO_CADASTRADA = 3
}

@Component({
	selector: 'app-grid-detalhe-processo-insumos',
	templateUrl: './grid-detalhe-processo-insumos.component.html'
})

export class DetalheInsumosGridComponent {
	servico = 'PlanoExportacao';
	servicoEntregarPlano = 'EntregarPlanoExportacao';
	servicoValidarPlano = 'ValidarPlanoExportacao';
	servicoApagarDetalheInsumo = "DetalheInsumos";
	servicoApagarAlteracaoEmAndamento = "SolicitacoesAlteracaoDetalhe";
	ocultarTituloGrid = true;
	checkAll1: any;
	exibirCabecalhoGrid = false;
	path: string;

	@ViewChild('ModalSolicitarAlteracaoValorFrete') ModalAlterarFreteComponent: ModalSolicitarAlteracaoValorFreteComponent;
	@ViewChild("checkedpli") checkedpli;
	@ViewChild('ModalSolicitarAlteracaoQuantidadeCoeficienteTecnicoComponent') ModalSolicitarAlteracaoQuantidadeCoeficienteTecnicoComponent : ModalSolicitarAlteracaoQuantidadeCoeficienteTecnicoComponent;
	@ViewChild('ModalPaisComponent') ModalPaisComponent: ModalPaisComponent;
	@ViewChild('ModalMoedaComponent') ModalMoedaComponent: ModalMoedaComponent;
	@ViewChild('ModalSolictacaoDetalhadaComponent') ModalSolictacaoDetalhadaComponent: ModalSolicitacaoDetalhadaComponent;
	@ViewChild('ModalSolicitacao') ModalSolicitacaoComponent: ModalSolicitacaoComponent;
	@ViewChild('ModalSolicitacaoAlteracaoValorUnitario') ModalValorUnitarioComponent: ModalSolicitarAlteracaoValorUnitarioComponent;

	constructor(
		private ConsultarPlanoFormularioDetalhesInsumosComponent: ConsultarPlanoFormularioDetalhesInsumosComponent,
		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
	) { 
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
	}

	@Input() lista: any[];
	@Input() total: number;
	@Input() size: number;
	@Input() page: number;
	@Input() sorted: string;
	@Input() isUsuarioInterno: boolean = false;
	@Input() exportarListagem: boolean;
	@Input() parametros: any = {};
	@Input() formPai: any;
	@Input() isQuadroNacional: boolean = false;

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();


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

	confirmarCopia(item){

		if (item.tipoModalidade != "S" && item.tipoExportacao != "AP"){
			this.modal.alerta("Somente poder??o ser copiados Planos na modalidade SUSPENS??O do tipo APROVA????O");
			return;
		}

		this.modal.confirmacao("Deseja criar uma nova c??pia de Plano a partir do Plano selecionado?","Confirma????o","")
		.subscribe(result => {
			if (result){
				this.gerarCopia(item);
			}
		})
	}

	gerarCopia(item: any) {

		this.applicationService.put(this.servico, item).subscribe((sucesso: any) => {
			
			if (!sucesso) {
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informa????o", "");
				return;
			}else{
				this.formPai.listar();
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informa????o", "");
			}
		});
	}

	apagar(item){
		this.modal.confirmacao("Confirma a opera????o?","Confirma????o","")
		.subscribe(result => {
			if (result){
				this.applicationService.delete(this.servicoApagarDetalheInsumo, item.idDetalheInsumo).subscribe((result: any) => {
					if (!result) {
						this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informa????o", "");
						return;
					}else{
						this.ConsultarPlanoFormularioDetalhesInsumosComponent.ngOnInit();
						this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informa????o", "");
					}
				});
			}
		});		
	}

	confirmarEntrega(item){

		this.modal.confirmacao("Confirma a opera????o?","Confirma????o","")
		.subscribe(result => {
			if (result){
				this.validar(item, true);
			}
		})
	}

	processarEntrega(item: any) {
		this.applicationService.post(this.servicoEntregarPlano, item).subscribe((result: any) => {
			if (!result.resultado) {
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informa????o", "");
				return;
			}else{
				this.formPai.listar();
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informa????o", "");
			}
		});
	}

	validar(item, realizarEntrega: boolean){
		let idPlanoExportacao = item.idPlanoExportacao;
		this.applicationService.post(this.servicoValidarPlano, item).subscribe((result: any) => {
			
			if (!result.resultado) {
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO + ": "+result.mensagem, "Aten????o", "");
			}else if(result.camposNaoValidos == null){
				this.modal.resposta("Plano sem erros de valida????o", "Informa????o", "")
				.subscribe(()=>{
					if (realizarEntrega){
						this.processarEntrega(item);
					}
				});
			}else if(result.camposNaoValidos != null){
				
				if (result.camposNaoValidos.naoExisteProduto){
					this.modal.confirmacao("Erro na valida????o. Plano n??o possui produto cadastrado, deseja corrigir os dados?","Confirma????o","")
					.subscribe(result => {
						if (result){
							this.router.navigate([`manter-plano-exportacao/${idPlanoExportacao}/validar-produto`])
						}
					});
				}
				else if (result.camposNaoValidos.naoExistePais){
					let idProduto = result.camposNaoValidos.idProduto;
					this.modal.confirmacao("Erro na valida????o. Existem produtos sem cadastro de pa??s, deseja corrigir os dados?","Confirma????o","")
					.subscribe(result => {
						if (result){
							this.router.navigate([`manter-plano-exportacao/${idProduto}/validar-propriedadeproduto`])
						}
					});
				}
				else if (result.camposNaoValidos.naoExisteInsumo){
					let idProduto = result.camposNaoValidos.idProduto;
					this.modal.confirmacao("Erro na valida????o. Existem produtos que n??o possuem insumos cadastrados, deseja corrigir os dados?","Confirma????o","")
					.subscribe(result => {
						if (result){
							//this.router.navigate([`manter-plano-exportacao/${idProduto}/cadastrar`])
						}
					});
				}
				else if (result.camposNaoValidos.naoExisteDetalhe){
					let idInsumo = result.camposNaoValidos.idInsumo;
					this.modal.confirmacao("Erro na valida????o. Existem insumos que n??o possuem detalhes cadastrados, deseja corrigir os dados?","Confirma????o","")
					.subscribe(result => {
						if (result){
							//this.router.navigate([`manter-plano-exportacao/${idInsumo}/cadastrar`])
						}
					});
				}
			}
		});
	}

	abrirModalSolicitacaoAlteracaoPais(item){
		if(item.flagExisteAlteracaoPais){
			this.apagarSolicAlteracao(item, item.idSolicDetalhePais, "Pa??s");
		} else {
			this.ModalPaisComponent.abrir(item);
		}	
	}

	abrirModalSolicitacaoAlteracaoMoeda(item){
		if(item.flagExisteAlteracaoMoeda){
			this.apagarSolicAlteracao(item, item.idSolicDetalheMoeda, "Moeda");
		} else {
			this.ModalMoedaComponent.abrir(item);
		}		
	}

	abrirModalSolicitacaoAlteracaoCoefTecnico(item){
		if(item.flagExisteAlteracaoQuantidade){
			this.apagarSolicAlteracao(item, item.idSolicDetalheQuantidade, "Quantidade");
		} else {
			this.ModalSolicitarAlteracaoQuantidadeCoeficienteTecnicoComponent.abrir(item);
		}		
	}

	abrirModalSolicitacaoAlteracaoValorUnitario(item){
		if(item.flagExisteAlteracaoValorUnitario){
			this.apagarSolicAlteracao(item, item.idSolicDetalheVlrUnitario, "Vlr. Unit??rio (FOB)");
		} else {
			this.ModalValorUnitarioComponent.abrir(item);
		}	
	}

	abrirModalSolicitacaoAlteracaoValorFrete(item){
		if(item.flagExisteAlteracaoValorFrete){
			this.apagarSolicAlteracao(item, item.idSolicDetalheVlrFrete, "Vlr. Frete (US$)");
		} else {
			this.ModalAlterarFreteComponent.abrir(item);
		}	
	}

	abrirModalVisualizarSolicitacao(item){
		this.ModalSolicitacaoComponent.abrir(item);		
	}
	
	apagarSolicAlteracao(item, idSolicDetalhe, tipoAlteracao){

		this.modal.confirmacao("Existe uma altera????o do tipo " + tipoAlteracao + 
							   " em andamento. Deseja criar uma nova altera????o de " + tipoAlteracao + " no Insumo? ","Confirma????o","")
		.subscribe(result => {
			if (result){
				this.applicationService.delete(this.servicoApagarAlteracaoEmAndamento, idSolicDetalhe).subscribe((resultado: Number) => {
					if(resultado == EnumStatusRetornoRequisicao.SUCESSO) {
						this.buscarDados().then(() => this.validaAbrirModal(tipoAlteracao, item));	
					} 
					else if(resultado == EnumStatusRetornoRequisicao.NAO_EXISTE_SOLCITACAO_ALTERACAO_CADASTRADA) {
						this.modal.alerta("N??o Existe Solicita????o de Altera????o cadastrada Para Esse Insumo!", "Informa????o", "");
						return false;
					} 
					else if(resultado == EnumStatusRetornoRequisicao.PARIDADE_CAMBIAL_NAO_CADASTRADA) {
						this.modal.alerta("Paridade cambial nao datastrada para a Data Atual!", "Informa????o", "");
						return false;
					}
					else {
						this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informa????o", "");
						return false;
					}	
				});
			}
		});		
	}

	validaAbrirModal(tipoAlteracao, item){
		if(tipoAlteracao == 'Pa??s'){
			this.ModalPaisComponent.abrir(item);
		} else if (tipoAlteracao == 'Moeda'){
			this.ModalMoedaComponent.abrir(item);
		} else if (tipoAlteracao == 'Quantidade'){
			this.ModalSolicitarAlteracaoQuantidadeCoeficienteTecnicoComponent.abrir(item);
		} else if (tipoAlteracao == 'Vlr. Unit??rio (FOB)'){
			this.ModalValorUnitarioComponent.abrir(item);
		} else if (tipoAlteracao == 'Vlr. Frete (US$)'){
			this.ModalAlterarFreteComponent.abrir(item);
		}
	}

	async buscarDados(){
		await this.ConsultarPlanoFormularioDetalhesInsumosComponent.ngOnInit();
	}
	

}
