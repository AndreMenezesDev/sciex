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
	selector: 'app-modal-solicitacao-alteracao-valor-unitario',
	templateUrl: './modal-valor-unitario.component.html',
})

export class ModalSolicitarAlteracaoValorUnitarioComponent implements OnInit {

	formPai = this;
	model: any;
	servico = 'SolicitacaoAlteracaoValorUnitario';
	parametros: any = {};
	grid: any = { sort: {} };
	exibirModal : boolean = false;
	idPrcInsumo : number;
	idProduto : number = 0;
	idProcesso : number = 0;
	quantidadeMaxima : number = null;

	valorCoeficienteTecnicoPara : any;
	valorQuantidadePara : any;

	@ViewChild('appModalValorUnitario') appModalValorUnitario;
	@ViewChild('appModalValorUnitarioBackground') appModalValorUnitarioBackground;


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
		this.parametros.prcInsumoDE = {};
		this.parametros.prcDetalheInsumoDE = {};
		this.parametros.prcDetalheInsumoPARA = {};
		this.parametros.prcDetalheInsumoPARA.valorUnitario = this.valorPara = 0;
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
				if (this.parametros.valorUnitarioAlteracaoPara == null){
					this.parametros.valorUnitarioAlteracaoPara = {};
					this.parametros.valorUnitarioAlteracaoPara.valorPara = 0;
				}else{
					this.dadosPrevistos = result.valorUnitarioAlteracaoPara;
					this.valorPara = this.dadosPrevistos.valorPara;
				}
				this.exibirModal = true;
			}			
		});

		this.appModalValorUnitario.nativeElement.style.display = 'block';
		this.appModalValorUnitarioBackground.nativeElement.style.display = 'block';
	}

	public fechar() {
	
		this.exibirModal = false;
		this.appModalValorUnitario.nativeElement.style.display = 'none';
		this.appModalValorUnitarioBackground.nativeElement.style.display = 'none';
	}

	confirmarCancelamento(): any
	{
		this.modal.confirmacao("Os dados ser??o descartados. Deseja continuar?", '', '')
				.subscribe(isConfirmado => {
					if (isConfirmado){
						this.fechar()
					}
				});
	}

	calcular(){
		if(this.validar()){	
			this.parametros.valorUnitarioAlteracaoPara.valorPara = this.valorPara;
			this.applicationService.post(this.servico, this.parametros).subscribe((result: any) => {
				if(result == null){
					this.modal.alerta("N??o existe valor de paridade cambial para a moeda utilizada, na data de hoje", "Informa????o", "");
					return false;
				}
				else{
					this.dadosPrevistos = result;
				}
			});
		}	
	}

	salvar(){		
		if(this.validar()){

			this.parametros.idProcesso = this.idProcesso;
			this.parametros.idProduto =  this.idProduto;
			this.parametros.valorUnitarioAlteracaoPara.valorPara = Number(this.valorPara);
			this.parametros.prcDetalheInsumoPara = !this.parametros.prcDetalheInsumoPara ? {} : this.parametros.prcDetalheInsumoPara ;
			this.parametros.prcInsumoPara = !this.parametros.prcInsumoPara ? {} : this.parametros.prcDetalheInsumoPara ;

			this.applicationService.put(this.servico, this.parametros).subscribe((result: number) => {
				if(result == EnumStatusRetornoRequisicao.SUCESSO) {
					this.modal.resposta("Opera????o realizada com Sucesso!", "Informa????o", "");
					this.ConsultarPlanoFormularioDetalhesInsumosComponent.ngOnInit();
					this.fechar();
				} 
				else if(result == EnumStatusRetornoRequisicao.NAO_EXISTE_SOLCITACAO_ALTERACAO_CADASTRADA) {
					this.modal.alerta("N??o Existe Solicita????o de Altera????o cadastrada Para Esse Insumo!", "Informa????o", "");
					return false;
				} 
				else if(result == EnumStatusRetornoRequisicao.PARIDADE_CAMBIAL_NAO_CADASTRADA) {
					this.modal.alerta("N??o existe valor de paridade cambial para a moeda utilizada, na data de hoje", "Informa????o", "");
					return false;
				}
				else {
					this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informa????o", "");
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
				this.modal.alerta("Valor <b>Unit??rio (Para)</b> Inv??lido!")
				return false;
			}	
		}

		return true;
	}


}
