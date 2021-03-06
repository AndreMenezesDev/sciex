import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../../shared/services/application.service';
import { MessagesService } from '../../../../shared/services/messages.service';
import { ModalService } from '../../../../shared/services/modal.service';
import { manterPliVM } from '../../../../view-model/ManterPliVM';
import { Router } from '@angular/router';
import { ModalJustificativaGenericoComponent } from '../../justificativa/modal-justificativa-generico.component';

@Component({
	selector: 'app-analise-dados-comprobatorios-grid',
	templateUrl: './grid-analise-dados-comprobatorios.component.html',
	styleUrls: ['./grid-insumos.component.css']
})

export class AnalisarDocumentosComprobatorioslGridComponent {
	servicoDUE = "DeclaracaoUnicaExportacao";
	servico = 'PlanoExportacao';
	servicoCorrigirInsumo = 'CorrigirInsumo';
	servicoEntregarPlano = 'EntregarPlanoExportacao';
	servicoValidarPlano = 'ValidarPlanoExportacao';
	ocultarTituloGrid = true;
	servicoExcluirInsumo = "ExcluirInsumo";

	@ViewChild("modalJustificativaErro") modalJustificativaErro;
	@ViewChild('appModalEditarDocumentoDue') appModalEditarDocumentoDue;
	@ViewChild('appModalJustificativaGlosa') appModalJustificativaGlosa: ModalJustificativaGenericoComponent;

	servicoAnalisar = "AnalisarDeclaracaoUnicaExportacao";

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
	@Input() isCorrecao: boolean;
	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();

	abrirGlosa(dados){
		dados.acaoIsAprovar = false;
		dados.idDue = dados.idDue;
		
		this.appModalJustificativaGlosa.abrir(dados,this.servicoAnalisar,this.formPai);
	}

	fecharGlosa(){
		this.appModalJustificativaGlosa.fechar();
		this.carregarDadosInsumos();
	}
	carregarDadosInsumos(){
		this.formPai.carregarInsumos();
	}
	aprovarItem(item){
		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.salvarRegistro(item);
				}
			}
		);
	}
	salvarRegistro(item) {
		let dados = {
			acaoIsAprovar:true,
			idDue:item.idDue
		}
		this.applicationService.post(this.servicoAnalisar, dados).subscribe((result:any) => {
			if (result.resultado){
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO,'Sucesso','').
				subscribe(()=>{
					this.formPai.listarDUE();
				});
			}else{
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO);
			}
		});
	}

	

	abrirModalEditar(item){
		this.appModalEditarDocumentoDue.abrir(this.formPai, item);
	}

    alterar(item){
		this.abrirModalEditar(item);
    }

	confirmarExclusao(item){

		this.modal.confirmacao("Confirma a opera????o?","Confirma????o","")
		.subscribe(result => {
			if (result){
				this.excluirDue(item);
			}
		})
	}
	excluirDue(item){
		let id = item.idDue
		this.applicationService.delete(this.servicoDUE, id).subscribe((result: any) =>{
			if (!result) {
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informa????o", "");
				return;
			}else{
				this.formPai.selecionarProduto();
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informa????o", "");
			}
		});
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

	abrirModalJustificativaErro(item){
		this.modalJustificativaErro.abrir(item);
	}



	inativarInsumo(item){
		this.applicationService.put(this.servicoCorrigirInsumo, item).subscribe((retorno: any) => {

			if (retorno.resultado) {
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informa????o", "")
				.subscribe(()=>{
					this.formPai.ngOnInit();
				});
			}else{
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informa????o", "")
				.subscribe(()=>{
					console.log(retorno.mensagem);
				});
				return;
			}
		});
	}
}
