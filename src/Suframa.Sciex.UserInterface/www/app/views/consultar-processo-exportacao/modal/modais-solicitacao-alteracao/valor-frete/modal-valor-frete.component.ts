import { Component, ViewChild, OnInit } from '@angular/core';
import { ValidationService } from '../../../../../shared/services/validation.service';
import { ModalService } from '../../../../../shared/services/modal.service';
import { MessagesService } from '../../../../../shared/services/messages.service';
import { ApplicationService } from '../../../../../shared/services/application.service';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../../../shared/guards/auth-guard.service';
import { ToastrService } from 'toastr-ng2/toastr';
import { AuthenticationService } from '../../../../../shared/services/authentication.service';
import { PagedItems } from '../../../../../view-model/PagedItems';
import { AssertNotNull, ThrowStmt } from '@angular/compiler';
import { ConsultarPlanoFormularioDetalhesInsumosComponent } from '../../../formulario/formulario-detalhes-insumos.component'; 
import { EnumStatusRetornoRequisicao } from '../../../../../shared/enums/EnumSituacaoRespostaRequisicao';

@Component({
	selector: 'app-modal-solicitacao-alteracao-valor-frete',
	templateUrl: './modal-valor-frete.component.html',
})

export class ModalSolicitarAlteracaoValorFreteComponent implements OnInit {

	formPai = this;
	model: any;
	servico = 'SolicitacaoAlteracaoValorFrete';
	servicoCalcularValorFrete = "ValorFrete";

	parametros: any = {};
	grid: any = { sort: {} };
	exibirModal : boolean = false;
	idPrcInsumo : number;
	idProduto : number = 0;
	idProcesso : number = 0;
	quantidadeMaxima : number = null;

	valorCoeficienteTecnicoPara : any;
	valorQuantidadePara : any;

	@ViewChild('appModalValorFrete') appModalValorFrete;
	@ViewChild('appModalValorFreteBackground') appModalValorFreteBackground;


	dadosAtuais:any ={};
	dadosInsumo: any = {};
	dadosDetalheDE: any = {};
	valorUnitarioPARA: number;
	valorPara: number;
	dadosPrevistos: any = {};

	constructor(
		private ConsultarPlanoFormularioDetalhesInsumosComponent : ConsultarPlanoFormularioDetalhesInsumosComponent,
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private applicationService: ApplicationService,
		private toastrService: ToastrService,
		private router: Router,
		private authguard: AuthGuard,
		private authenticationService: AuthenticationService,
	) {

	}

	ngOnInit() { 
		this.inicializarCampos();

	}

	inicializarCampos(){
		this.parametros = {};
		this.dadosPrevistos = {};
		this.parametros.prcInsumoDE = {};
		this.parametros.prcDetalheInsumoDE = {};
		this.parametros.prcDetalheInsumoPARA = {};
		this.parametros.prcDetalheInsumoPARA.valorFrete = this.valorPara = 0;
	}

	public abrir(objeto) {	
		this.inicializarCampos();
		this.dadosInsumo = objeto.prcInsumo;
		
		this.idProcesso = Number(objeto.prcInsumo.produto.idProcesso);
		this.idProduto =  Number(objeto.prcInsumo.produto.idProduto);

		this.parametros.prcInsumoDE.idInsumo = objeto.prcInsumo.idInsumo;
		this.parametros.prcInsumoDE.codigoInsumo = objeto.prcInsumo.codigoInsumo;
		this.parametros.prcInsumoDE.idPrcProduto = objeto.prcInsumo.idPrcProduto;

		this.parametros.prcDetalheInsumoDE.idDetalheInsumo = Number(objeto.idDetalheInsumo);
		this.parametros.prcDetalheInsumoDE.numeroSequencial = Number(objeto.numeroSequencial);

		this.applicationService.get(this.servico, this.parametros).subscribe((result: any) => {
			if(!result){
				this.modal.alerta("Sem conexao com servidor. Tente novamente.", "");
				return false;
			} else{
				this.parametros = result;
				if (this.parametros.valorFreteAlteracaoPara == null){
					this.parametros.valorFreteAlteracaoPara = {};
					this.parametros.valorFreteAlteracaoPara.valorPara = 0;
				}else{
					this.dadosPrevistos = result.valorFreteAlteracaoPara;
					this.valorPara = this.dadosPrevistos.valorPara;
				}
				this.exibirModal = true;
			}			
		});

		this.appModalValorFrete.nativeElement.style.display = 'block';
		this.appModalValorFreteBackground.nativeElement.style.display = 'block';
	}

	public fechar() {
		
		this.exibirModal = false;
		this.appModalValorFrete.nativeElement.style.display = 'none';
		this.appModalValorFreteBackground.nativeElement.style.display = 'none';
	}

	confirmarCancelamento(): any
	{
		this.modal.confirmacao("Os dados serão descartados. Deseja continuar?", '', '')
				.subscribe(isConfirmado => {
					if (isConfirmado){
						this.fechar()
					}
				});
	}

	calcular(){
		if(this.validar()){	
			this.parametros.valorFreteAlteracaoPara.valorPara = this.valorPara;
			this.applicationService.post(this.servico, this.parametros).subscribe((result: any) => {
				if(result == null){
					this.modal.alerta("Não existe valor de paridade cambial para a moeda utilizada, na data de hoje", "Informação", "");
					return false;
				}
				else {
					this.dadosPrevistos = result;
				}
			});
		}	
	}

	salvar(){		
		if(this.validar()){

			this.parametros.idProcesso = this.idProcesso;
			this.parametros.idProduto =  this.idProduto;
			this.parametros.valorFreteAlteracaoPara.valorPara = Number(this.valorPara);
			this.parametros.prcDetalheInsumoPara = !this.parametros.prcDetalheInsumoPara ? {} : this.parametros.prcDetalheInsumoPara ;
			this.parametros.prcInsumoPara = !this.parametros.prcInsumoPara ? {} : this.parametros.prcDetalheInsumoPara ;

			this.applicationService.put(this.servico, this.parametros).subscribe((result: number) => {
				if(result == EnumStatusRetornoRequisicao.SUCESSO) {
					this.modal.resposta("Operação realizada com Sucesso!", "Informação", "");
					this.ConsultarPlanoFormularioDetalhesInsumosComponent.ngOnInit();
					this.fechar();
				} 
				else if(result == EnumStatusRetornoRequisicao.NAO_EXISTE_SOLCITACAO_ALTERACAO_CADASTRADA) {
					this.modal.alerta("Não Existe Solicitação de Alteração cadastrada Para Esse Insumo!", "Informação", "");
					return false;
				} 
				else if(result == EnumStatusRetornoRequisicao.PARIDADE_CAMBIAL_NAO_CADASTRADA) {
					this.modal.alerta("Não existe valor de paridade cambial para a moeda utilizada, na data de hoje", "Informação", "");
					return false;
				}
				else {
					this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informação", "");
					return false;
				}		
			});
		}		
	}

	confirmarSalvar() {
		
		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.salvar();
				}
			});

	}

	validar(){
				
		if(this.valorPara != null) {
			if(!this.valorPara){
				this.modal.alerta("Valor <b>Frete (Para)</b> Inválido!")
				return false;
			}	
		}

		return true;
	}


}
