import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { Router } from '@angular/router';
import { ModalDetalhesInsumoSuframaComponent } from '../modal/modal-detalhes-insumo-suframa.component';

@Component({
	selector: 'app-consultar-insumos-importados-suframa-grid',
	templateUrl: './grid-insumos-importados-suframa.component.html',
	styles: [
		`
		  .greenClass { background-color: #F0FFF0 }
		  .blueClass { background-color: #E0FFFF }
		`
	  ]
})

export class ConsultarInsumosImportadosSuframaGridComponent {
	servicoExcluirInsumo = "ExcluirInsumo";
	servico = 'PlanoExportacao';
	servicoEntregarPlano = 'EntregarPlanoExportacao';
	servicoValidarPlano = 'ValidarPlanoExportacao';
	servicoListarInsumos = "ListarProcessoInsumosNacionalOuImportadoPorIdProdutoSuframa";
	ocultarTituloGrid = true;

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
	) { }

	@Input() lista: any[];
	@Input() total: number;
	@Input() size: number;
	@Input() page: number;
	@Input() sorted: string;
	@Input() isUsuarioInterno: boolean = false;
	@Input() exportarListagem: boolean;
	@Input() parametros: any = {};
	@Input() formPai: any;
	@Input() somenteLeitura: boolean;
	@Input() possuiSolicitacaoAlteracao: boolean;

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();

	@ViewChild('appModalDetalheInsumo') appModalDetalheInsumo: ModalDetalhesInsumoSuframaComponent;
	@ViewChild('ModalAnaliseSolicitacaoComponent') ModalAnaliseSolicitacaoComponent;
	@ViewChild('ModalHistoricoInsumosImportadosComponent') ModalHistoricoInsumosImportadosComponent;

	public abrirModalDetalheInsumo(dadosInsumo, possuiSolicitacaoAlteracao) {

		if(!possuiSolicitacaoAlteracao) {
			this.appModalDetalheInsumo.abrir(this, false, dadosInsumo);
		}else{
			let idInsumo = dadosInsumo.idInsumo;
			this.router.navigate([`/consultar-detalhe-processo-insumo/${idInsumo}/visualizar-quadro-importado`])
		}
	}

	excluirInsumo(dados) {
		this.modal.confirmacao("Confirma a opera????o?", "Confirma????o", "")
			.subscribe(result => {
				if (result) {
					this.realizarExclusaoInsumo(dados);
				}
			});
	}
	realizarExclusaoInsumo(dados) {
		let id = dados.idPEInsumo;
		this.applicationService.delete(this.servicoExcluirInsumo, id).subscribe((result: any) => {

			if (!result) {
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informa????o", "");
				return;
			} else {
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informa????o", "")
					.subscribe(() => {
						this.formPai.ngOnInit();
					});
			}
		});
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

	confirmarCopia(item) {

		if (item.tipoModalidade != "S" && item.tipoExportacao != "AP") {
			this.modal.alerta("Somente poder??o ser copiados Planos na modalidade SUSPENS??O do tipo APROVA????O");
			return;
		}

		this.modal.confirmacao("Deseja criar uma nova c??pia de Plano a partir do Plano selecionado?", "Confirma????o", "")
			.subscribe(result => {
				if (result) {
					this.gerarCopia(item);
				}
			})
	}
	gerarCopia(item: any) {

		this.applicationService.put(this.servico, item).subscribe((sucesso: any) => {

			if (!sucesso) {
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informa????o", "");
				return;
			} else {
				this.formPai.listar();
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informa????o", "");
			}
		});
	}

	confirmarEntrega(item) {

		this.modal.confirmacao("Confirma a opera????o?", "Confirma????o", "")
			.subscribe(result => {
				if (result) {
					this.validar(item, true);
				}
			})
	}
	processarEntrega(item: any) {
		this.applicationService.post(this.servicoEntregarPlano, item).subscribe((result: any) => {
			if (!result.resultado) {
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informa????o", "");
				return;
			} else {
				this.formPai.listar();
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informa????o", "");
			}
		});
	}

	validar(item, realizarEntrega: boolean) {
		let idPlanoExportacao = item.idPlanoExportacao;
		this.applicationService.post(this.servicoValidarPlano, item).subscribe((result: any) => {

			if (!result.resultado) {
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO + ": " + result.mensagem, "Aten????o", "");
			} else if (result.camposNaoValidos == null) {
				this.modal.resposta("Plano sem erros de valida????o", "Informa????o", "")
					.subscribe(() => {
						if (realizarEntrega) {
							this.processarEntrega(item);
						}
					});
			} else if (result.camposNaoValidos != null) {

				if (result.camposNaoValidos.naoExisteProduto) {
					this.modal.confirmacao("Erro na valida????o. Plano n??o possui produto cadastrado, deseja corrigir os dados?", "Confirma????o", "")
						.subscribe(result => {
							if (result) {
								this.router.navigate([`manter-plano-exportacao/${idPlanoExportacao}/validar-produto`])
							}
						});
				}
				else if (result.camposNaoValidos.naoExistePais) {
					let idProduto = result.camposNaoValidos.idProduto;
					this.modal.confirmacao("Erro na valida????o. Existem produtos sem cadastro de pa??s, deseja corrigir os dados?", "Confirma????o", "")
						.subscribe(result => {
							if (result) {
								this.router.navigate([`manter-plano-exportacao/${idProduto}/validar-propriedadeproduto`])
							}
						});
				}
				else if (result.camposNaoValidos.naoExisteInsumo) {
					let idProduto = result.camposNaoValidos.idProduto;
					this.modal.confirmacao("Erro na valida????o. Existem produtos que n??o possuem insumos cadastrados, deseja corrigir os dados?", "Confirma????o", "")
						.subscribe(result => {
							if (result) {
								//this.router.navigate([`manter-plano-exportacao/${idProduto}/cadastrar`])
							}
						});
				}
				else if (result.camposNaoValidos.naoExisteDetalhe) {
					let idInsumo = result.camposNaoValidos.idInsumo;
					this.modal.confirmacao("Erro na valida????o. Existem insumos que n??o possuem detalhes cadastrados, deseja corrigir os dados?", "Confirma????o", "")
						.subscribe(result => {
							if (result) {
								//this.router.navigate([`manter-plano-exportacao/${idInsumo}/cadastrar`])
							}
						});
				}
			}
		});
	}

	abrirModalAlteracaoSolicitacao(item) {
		this.ModalAnaliseSolicitacaoComponent.abrir(item);
	}

	aprovarReprovarAlteracaoInsumo(item, isAprovar){
        let obj = {

			idInsumo : item.idInsumo,
			isAprovarAnalise : isAprovar
		}

		this.applicationService.put(this.servicoListarInsumos, obj).subscribe((response: any) => {

			if (!response.sucesso) {
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informa????o", "");
				return;
			}else{
				this.formPai.carregarInsumos();
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informa????o", "");
			}
		});
	}

	verificarSeAprovado(item){
		let reg = item.prcSolicitacaoAlteracao.listaSolicDetalhe[0];

		if( reg != null || reg != undefined){

			return (reg.status != 2) ?true : false

		}else{

			return false;

		}

	}

	verificarSeReprovado(item){
		let reg = item.prcSolicitacaoAlteracao.listaSolicDetalhe[0];

		if( reg != null || reg != undefined){

			return (reg.status != 3) ?true : false

		}else{

			return false;

		}
	}

	abrirModalHistorico(item){
        this.ModalHistoricoInsumosImportadosComponent.abrir(item);
	}

}
