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

enum EnumStatusRetornoRequisicao {	
	ERRO = 0,
	SUCESSO = 1,
	PARIDADE_CAMBIAL_NAO_CADASTRADA = 2,
	NAO_EXISTE_SOLCITACAO_ALTERACAO_CADASTRADA = 3
}

@Component({
	selector: 'app-modal-pais',
	templateUrl: './modal-pais.component.html',
})

export class ModalPaisComponent implements OnInit {

	formPai = this;
	model: any;
	servico = 'SolicAlteracaoPais';
	parametros: any = {};
	grid: any = { sort: {} };
	idProduto : number = 0;
	idProcesso : number = 0;
	paisDe : string = "";
	descInsumo : string = "";
	codigoInsumo : string = "";
	numeroSequencial : string = "";
	somenteLeitura: any;

	@ViewChild('appModalPais') appModalPais;
	@ViewChild('appModalPaisBackground') appModalPaisBackground;
	@ViewChild('pais') pais;

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
		this.paisDe = objeto.descricaoPais;
		this.descInsumo = objeto.prcInsumo.codigoInsumo + " | " + objeto.prcInsumo.descricaoInsumo;
		this.parametros.prcInsumoDE = objeto.prcInsumo;
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
			}			
		});


		this.appModalPais.nativeElement.style.display = 'block';
		this.appModalPaisBackground.nativeElement.style.display = 'block';
	}

	public fechar() {
		this.pais.clear();
		this.descInsumo = "";
		this.numeroSequencial = "";
		this.grid = { sort: {} };
		this.parametros = {};
		this.appModalPais.nativeElement.style.display = 'none';
		this.appModalPaisBackground.nativeElement.style.display = 'none';
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

	salvar() {

		if (this.parametros.codigoPais == null || this.parametros.codigoPais == undefined){
			this.modal.alerta("Preencha o campo 'PARA'", "Informação");
			return;
		}	

		var nomePaisSelecionado = this.pais.model.split(" | ")[1].replace(" ", "");
		if(nomePaisSelecionado == this.paisDe){
			this.modal.alerta("Países iguais. Selecione um país diferente", "Informação");
			return;
		}

		this.parametros.idProcesso = this.idProcesso;
		this.parametros.idProduto =  this.idProduto;
		this.parametros.prcsDetalheInsumoPara = this.parametros.codigoPais;
		this.parametros.prcDetalheInsumoPara = !this.parametros.prcDetalheInsumoPara ? {} : this.parametros.prcDetalheInsumoPara ;
		this.parametros.prcInsumoPara = !this.parametros.prcInsumoPara ? {} : this.parametros.prcDetalheInsumoPara ;
		this.parametros.codigoPaisDe = Number(this.parametros.prcDetalheInsumoDE.codigoPais);
		this.parametros.codigoPaisPara = Number(this.parametros.codigoPais);

		this.applicationService.post(this.servico, this.parametros).subscribe((result: Number) => {
			if(result == EnumStatusRetornoRequisicao.SUCESSO) {
				this.modal.resposta("Operação realizada com Sucesso!", "Informação", "");
				this.ConsultarPlanoFormularioDetalhesInsumosComponent.ngOnInit();
				this.fechar();
			} 
			else if(result == EnumStatusRetornoRequisicao.NAO_EXISTE_SOLCITACAO_ALTERACAO_CADASTRADA) {
				this.modal.alerta("Não Existe Solicitação de Alteração cadastrada Para Esse Insumo!", "Informação", "");
				return false;
			} else {
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informação", "");
				return false;
			}
			
		});

	}

	confirmarSalvar() {
		
		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.salvar();
				}
			});

	}


}
