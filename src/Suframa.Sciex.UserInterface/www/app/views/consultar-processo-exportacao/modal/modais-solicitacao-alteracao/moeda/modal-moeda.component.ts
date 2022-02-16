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


@Component({
	selector: 'app-modal-moeda',
	templateUrl: './modal-moeda.component.html',
})

export class ModalMoedaComponent implements OnInit {

	formPai = this;
	model: any;
	servico = 'calcularMoeda';
	parametros: any = {};
	descricaoMoeda: string;
	idProduto : number = 0;
	idProcesso : number = 0;
	codigoInsumo: string = "";
	descricaoInsumo: string = "";
	numeroSequencial : string = "";
	somenteLeitura: any;

	@ViewChild('appModalMoeda') appModalMoeda;
	@ViewChild('appModalMoedaBackground') appModalMoedaBackground;
	@ViewChild('moeda') moeda;

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
		this.limpar();
	 }

	public abrir(objeto) {

		this.descricaoMoeda = objeto.codigoDescricaoMoeda;
		this.idProcesso = Number(objeto.prcInsumo.produto.idProcesso);
		this.idProduto =  Number(objeto.prcInsumo.produto.idProduto);

		this.descricaoInsumo = objeto.prcInsumo.descricaoInsumo;
		this.codigoInsumo = objeto.prcInsumo.codigoInsumo;
		this.numeroSequencial = objeto.numeroSequencial;

		this.parametros.PRCInsumoDE = {}
		this.parametros.PRCInsumoDE.idInsumo =Number(objeto.prcInsumo.idInsumo);
		this.parametros.PRCInsumoDE.codigoInsumo = Number(objeto.prcInsumo.codigoInsumo);
		this.parametros.PRCInsumoDE.idPrcProduto = Number(objeto.prcInsumo.idPrcProduto);

		this.appModalMoeda.nativeElement.style.display = 'block';
		this.appModalMoedaBackground.nativeElement.style.display = 'block';
	}

	fechar() {
		this.limpar();
		this.appModalMoeda.nativeElement.style.display = 'none';
		this.appModalMoedaBackground.nativeElement.style.display = 'none';
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

	calcularMoeda(){
		if(this.parametros.idMoeda > 0){
			if(this.validarMoeda()){
				this.applicationService.post(this.servico, this.parametros).subscribe((result: any) => {
					this.limparVariaveis();
					if(result == null){
						this.modal.alerta("Não existe valor de paridade cambial para a moeda utilizada, na data de hoje", "Informação", "");
					}else{
						this.parametros.calcularMoedaPara = result;

					}
				});
			}else{
				this.modal.alerta("Moedas iguais. Selecione um moeda diferente",'Insumo Alteração - Moeda');
			}
		}else{
			this.modal.alerta('Campo Moeda (Para) <b> Não Informado','Insumo Alteração - Moeda');
		}
	}

	salvar(){
		
		if(this.parametros.idMoeda > 0){
			if(this.validarMoeda()){
				this.parametros.idProcesso = this.idProcesso;
				this.parametros.idProduto =  this.idProduto;
				this.parametros.descricaoMoedaDE = this.descricaoMoeda;
				this.parametros.descricaoMoedaPARA = this.moeda.model;
				this.parametros.prcDetalheInsumoPara = !this.parametros.prcDetalheInsumoPara ? {} : this.parametros.prcDetalheInsumoPara ;
				this.parametros.prcInsumoPara = !this.parametros.prcInsumoPara ? {} : this.parametros.prcDetalheInsumoPara ;

				this.applicationService.put(this.servico, this.parametros).subscribe((result: boolean) => {
					if(result){
						this.modal.resposta("Operação realizada com Sucesso!", "Informação", "");
						this.ConsultarPlanoFormularioDetalhesInsumosComponent.ngOnInit();
						this.fechar();
					}
					else if(!result){
						this.modal.alerta("Não existe valor de paridade cambial para a moeda utilizada, na data de hoje", "Informação", "");
					}
					else{
						
					}		
				});
			}else{
				this.modal.alerta("Moedas iguais. Selecione um moeda diferente",'Insumo Alteração - Moeda');
			}
		}else{
			this.modal.alerta('Campo Moeda (Para) <b> Não Informado','Insumo Alteração - Moeda');
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

	validarMoeda(): boolean{

		var moedaDe = this.descricaoMoeda.split(" | ")[1].replace(" ", "");
		var moedaPara = this.moeda.model.split(" | ")[1].replace(" ", "");

		if(moedaPara == moedaDe){
			return false;
		}else{
			return true;
		}
	}

	limpar(){

		this.moeda.onClear(true);
		this.parametros = {};
		this.parametros.calcularMoedaPara = {};
		this.parametros.idMoeda = 0;
	}

	limparVariaveis(){
		this.parametros.calcularMoedaPara.paridade = "0";
		this.parametros.calcularMoedaPara.saldoQuantidade = "0";
		this.parametros.calcularMoedaPara.saldoValorUS = "0";
		this.parametros.calcularMoedaPara.acrescimoUS="0";
		this.parametros.calcularMoedaPara.saldoFinalUS="0";		
	}
}
