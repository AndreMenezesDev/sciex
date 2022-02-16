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

	apagar(item){
		this.modal.confirmacao("Confirma a operação?","Confirmação","")
		.subscribe(result => {
			if (result){
				this.applicationService.delete(this.servicoApagarDetalheInsumo, item.idDetalheInsumo).subscribe((result: any) => {
					if (!result) {
						this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informação", "");
						return;
					}else{
						this.ConsultarPlanoFormularioDetalhesInsumosComponent.ngOnInit();
						this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informação", "");
					}
				});
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

	processarEntrega(item: any) {
		this.applicationService.post(this.servicoEntregarPlano, item).subscribe((result: any) => {
			if (!result.resultado) {
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informação", "");
				return;
			}else{
				this.formPai.listar();
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informação", "");
			}
		});
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
							//this.router.navigate([`manter-plano-exportacao/${idProduto}/cadastrar`])
						}
					});
				}
				else if (result.camposNaoValidos.naoExisteDetalhe){
					let idInsumo = result.camposNaoValidos.idInsumo;
					this.modal.confirmacao("Erro na validação. Existem insumos que não possuem detalhes cadastrados, deseja corrigir os dados?","Confirmação","")
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
			this.apagarSolicAlteracao(item, item.idSolicDetalhePais, "País");
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
			this.apagarSolicAlteracao(item, item.idSolicDetalheVlrUnitario, "Vlr. Unitário (FOB)");
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

		this.modal.confirmacao("Existe uma alteração do tipo " + tipoAlteracao + 
							   " em andamento. Deseja criar uma nova alteração de " + tipoAlteracao + " no Insumo? ","Confirmação","")
		.subscribe(result => {
			if (result){
				this.applicationService.delete(this.servicoApagarAlteracaoEmAndamento, idSolicDetalhe).subscribe((resultado: Number) => {
					if(resultado == EnumStatusRetornoRequisicao.SUCESSO) {
						this.buscarDados().then(() => this.validaAbrirModal(tipoAlteracao, item));	
					} 
					else if(resultado == EnumStatusRetornoRequisicao.NAO_EXISTE_SOLCITACAO_ALTERACAO_CADASTRADA) {
						this.modal.alerta("Não Existe Solicitação de Alteração cadastrada Para Esse Insumo!", "Informação", "");
						return false;
					} 
					else if(resultado == EnumStatusRetornoRequisicao.PARIDADE_CAMBIAL_NAO_CADASTRADA) {
						this.modal.alerta("Paridade cambial nao datastrada para a Data Atual!", "Informação", "");
						return false;
					}
					else {
						this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informação", "");
						return false;
					}	
				});
			}
		});		
	}

	validaAbrirModal(tipoAlteracao, item){
		if(tipoAlteracao == 'País'){
			this.ModalPaisComponent.abrir(item);
		} else if (tipoAlteracao == 'Moeda'){
			this.ModalMoedaComponent.abrir(item);
		} else if (tipoAlteracao == 'Quantidade'){
			this.ModalSolicitarAlteracaoQuantidadeCoeficienteTecnicoComponent.abrir(item);
		} else if (tipoAlteracao == 'Vlr. Unitário (FOB)'){
			this.ModalValorUnitarioComponent.abrir(item);
		} else if (tipoAlteracao == 'Vlr. Frete (US$)'){
			this.ModalAlterarFreteComponent.abrir(item);
		}
	}

	async buscarDados(){
		await this.ConsultarPlanoFormularioDetalhesInsumosComponent.ngOnInit();
	}
	

}
