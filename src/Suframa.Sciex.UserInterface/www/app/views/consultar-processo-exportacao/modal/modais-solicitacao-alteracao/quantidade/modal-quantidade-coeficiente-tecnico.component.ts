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
	selector: 'app-modal-solicitacao-alteracao-quantidade-coeficiente-tecnico',
	templateUrl: './modal-quantidade-coeficiente-tecnico.component.html',
})

export class ModalSolicitarAlteracaoQuantidadeCoeficienteTecnicoComponent implements OnInit {

	
	formPai = this;
	model: any;
	servico = 'QuantidadeCoeficiente';
	servicoCalcularCoeficienteTecnico = "CalculaCoeficienteTecnico";
	parametros: any = {};
	grid: any = { sort: {} };
	exibirModal : boolean = false;
	idPrcInsumo : number;
	idProduto : number = 0;
	idProcesso : number = 0;
	quantidadeMaxima : number = null;
	descricaoInsumo : string = "";
	codigoInsumo : string = "";
	numeroSequencial : string = "";

	valorCoeficienteTecnicoPara : any;
	valorQuantidadePara : any;

	@ViewChild('appModalQuantidadeCoeficienteTecnico') appModalQuantidadeCoeficienteTecnico;
	@ViewChild('appModalQuantidadeCoeficienteTecnicoBackground') appModalQuantidadeCoeficienteTecnicoBackground;

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

	ngOnInit() { }

	public abrir(objeto) {	
		
		this.idProcesso = Number(objeto.prcInsumo.produto.idProcesso);
		this.idProduto =  Number(objeto.prcInsumo.produto.idProduto);

		this.descricaoInsumo = objeto.prcInsumo.descricaoInsumo;
		this.codigoInsumo = objeto.prcInsumo.codigoInsumo;

		this.numeroSequencial = objeto.numeroSequencial;

		this.parametros.PRCInsumoDE = {}
		this.parametros.PRCInsumoDE.codigoInsumo = Number(objeto.prcInsumo.codigoInsumo);
		this.parametros.PRCInsumoDE.idPrcProduto = Number(objeto.prcInsumo.idPrcProduto);
		
		this.applicationService.get(this.servico, this.parametros).subscribe((result: any) => {
			if(!result){
				this.modal.alerta("Sem conexao com servidor. Tente novamente.", "");
				return false;
			} else{
				this.parametros = result;
				this.exibirModal = true;
			}			
		});

		this.appModalQuantidadeCoeficienteTecnico.nativeElement.style.display = 'block';
		this.appModalQuantidadeCoeficienteTecnicoBackground.nativeElement.style.display = 'block';
	}

	public fechar() {

		this.idProcesso = 0;
		this.quantidadeMaxima = null;
		this.exibirModal = false;
		this.grid = { sort: {} };
		this.parametros = {};
	
		this.appModalQuantidadeCoeficienteTecnico.nativeElement.style.display = 'none';
		this.appModalQuantidadeCoeficienteTecnicoBackground.nativeElement.style.display = 'none';
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
			this.applicationService.post(this.servico, this.parametros).subscribe((result: any) => {
				if(result == null){
					this.modal.alerta("Não existe valor de paridade cambial para a moeda utilizada, na data de hoje", "Informação", "");
					return false;
				} 
				else
				{
					this.parametros.quantidadeCoefTecnicoPara = result;
					this.parametros.quantidadeCoefTecnicoPara.valorParaCoeficienteTecnico = this.valorCoeficienteTecnicoPara;
					this.parametros.quantidadeCoefTecnicoPara.valorPara = this.valorQuantidadePara;
				}
				
			});
		}	
	}

	salvar(){		
		if(this.validar()){

			this.parametros.idProcesso = this.idProcesso;
			this.parametros.idProduto =  this.idProduto;
			this.parametros.quantidadeCoefTecnicoPara.valorParaCoeficienteTecnico = Number(this.valorCoeficienteTecnicoPara);
			this.parametros.quantidadeCoefTecnicoPara.valorPara = Number(this.valorQuantidadePara);
			this.parametros.prcDetalheInsumoPara = !this.parametros.prcDetalheInsumoPara ? {} : this.parametros.prcDetalheInsumoPara ;
			this.parametros.prcInsumoPara = !this.parametros.prcInsumoPara ? {} : this.parametros.prcDetalheInsumoPara ;

			this.applicationService.put(this.servico, this.parametros).subscribe((result: Number) => {
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
		if(!this.quantidadeMaxima || this.quantidadeMaxima == 0){
			this.modal.alerta("Valor <b>Coeficiente Técnico (Para)</b> Inválido!")
			return false;
		}
		if(!this.parametros.quantidadeCoefTecnicoPara.valorParaCoeficienteTecnico){
			this.modal.alerta("Valor <b>Coeficiente Técnico (Para)</b> Inválido!")
			return false;
		}		
		if(!this.parametros.quantidadeCoefTecnicoPara.valorPara){
			this.modal.alerta("Valor <b>Quantidade (Para)</b> Inválido!")
			return false;
		}	
		if(Number(this.parametros.quantidadeCoefTecnicoPara.valorPara) > this.quantidadeMaxima){
			this.modal.alerta("<b>Quantidade (Para)</b> maior que a <b>Quantidade Máxima</b>!")
			return false;
		}
		if(Number(this.parametros.prcInsumoDE.valorCoeficienteTecnico) >= Number(this.parametros.quantidadeCoefTecnicoPara.valorParaCoeficienteTecnico)){
			this.modal.alerta("<b>Quantidade (Para)</b> do Coeficiente Técnico, deve ser maior que a <b>Quantidade De</b>!")
			return false;
		}
		if(Number(this.parametros.prcInsumoDE.quantidade) >= Number(this.parametros.quantidadeCoefTecnicoPara.valorPara)){
			this.modal.alerta("<b>Quantidade (Para)</b> da Quantidade, deve ser maior que a <b>Quantidade De</b>!")
			return false;
		}

		this.valorCoeficienteTecnicoPara = this.parametros.quantidadeCoefTecnicoPara.valorParaCoeficienteTecnico;
		this.valorQuantidadePara = this.parametros.quantidadeCoefTecnicoPara.valorPara;
		return true;
	}

	calculaCoeficienteTecnico(){
		
		if(this.parametros.quantidadeCoefTecnicoPara.valorParaCoeficienteTecnico && (Number(this.parametros.quantidadeCoefTecnicoPara.valorParaCoeficienteTecnico) > 0)){
		
			if(Number(this.parametros.prcInsumoDE.valorCoeficienteTecnico) >= Number(this.parametros.quantidadeCoefTecnicoPara.valorParaCoeficienteTecnico)){
				this.parametros.quantidadeCoefTecnicoPara.valorParaCoeficienteTecnico = null;
				this.modal.alerta("<b>Quantidade (Para)</b> do Coeficiente Técnico, deve ser maior que a <b>Quantidade De</b>!")
				return false;
			}

			this.parametros.idProduto = this.idProduto;
			this.applicationService.post(this.servicoCalcularCoeficienteTecnico, this.parametros).subscribe((result: any) => {
				if(result == null){
					this.modal.alerta("Não existe valor de paridade cambial para a moeda utilizada, na data de hoje", "Informação", "");
					return false;
				}else{
					this.quantidadeMaxima = result;
				}				
			});
		} else{
			this.quantidadeMaxima = null;
		}
	}


}
