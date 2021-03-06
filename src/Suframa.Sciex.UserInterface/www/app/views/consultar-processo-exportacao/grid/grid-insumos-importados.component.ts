import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalDetalhesInsumoComponent } from '../modal/modal-detalhes-insumo.component';
import { ModalTransferenciaInsumoComponent } from '../modal/modal-transferencia-insumo.component';
import { ConsultarFormularioQuadrosInsumosComponent } from '../formulario/formulario-quadros-insumos.component';
import { ModalSolicitacaoDetalhadaComponent } from '../modal/modais-solicitacao-alteracao/modal-visualizar-solicitacao-detalhada/solicitacao-detalhada-alteracao.component';
@Component({
	selector: 'app-consultar-insumos-importados-grid',
	templateUrl: './grid-insumos-importados.component.html'
})

export class ConsultarInsumosImportadosGridComponent {
	servicoExcluirInsumo = "ExcluirInsumo";
	servico = 'PlanoExportacao';
	servicoEntregarPlano = 'EntregarPlanoExportacao';
	servicoValidarPlano = 'ValidarPlanoExportacao';
	serviceDetalheInsumo = 'DetalheInsumos';
	ocultarTituloGrid = true;
	emAlteracao = 2;
	Ativo = 1;
	path: string;

	constructor(
		private route: ActivatedRoute,
		private ConsultarFormularioQuadrosInsumosComponent : ConsultarFormularioQuadrosInsumosComponent,
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
	) {
		
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
	@Input() somenteLeitura: boolean;
	@Input() dadosSolicitacao: any = {};

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();

	@ViewChild('appModalDetalheInsumo') appModalDetalheInsumo : ModalDetalhesInsumoComponent;
	@ViewChild('appModalTransferenciaInsumo') appModalTransferenciaInsumo: ModalTransferenciaInsumoComponent;
	@ViewChild('ModalSolicitacaoDetalhada') ModalSolicitacaoDetalhadaComponent: ModalSolicitacaoDetalhadaComponent;

	public abrirModalDetalheInsumo(dadosInsumo){
		this.appModalDetalheInsumo.abrir(this, false,dadosInsumo);
	}
	public abrirModalVisualizarSolicitacaoDetalhada(item){
		this.ModalSolicitacaoDetalhadaComponent.abrir(item);

	}

	excluirInsumo(dados) {
		this.modal.confirmacao("Confirma a opera????o?","Confirma????o","")
		.subscribe(result => {
			if (result){
				this.realizarExclusaoInsumo(dados);
			}
		});		
	}

	realizarExclusaoInsumo(dados){
		let id = dados.idPEInsumo;
		this.applicationService.delete(this.servicoExcluirInsumo, id).subscribe((result: any) => {
			
			if (!result) {
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informa????o", "");
				return;
			}else{				
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informa????o", "")
				.subscribe(()=>{
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

	abrirModalTransferenciaInsumo(item){
		this.appModalTransferenciaInsumo.abrir(item);
	}

	deletarInsumo(objeto){
		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
		.subscribe(isConfirmado => {
			if (isConfirmado) {
				this.excluir(objeto);
			}
		});
	}

	excluir(objeto){
		this.applicationService.post(this.serviceDetalheInsumo, objeto).subscribe((result: boolean) => {
			if(result){
				this.modal.resposta("Opera????o realizada com sucesso!", "Informa????o", "");
				this.ConsultarFormularioQuadrosInsumosComponent.ngOnInit();
			}			
		});
	}

}
