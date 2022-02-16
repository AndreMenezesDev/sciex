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


@Component({
	selector: 'app-modal-solicitacao-detalhada',
	templateUrl: './solicitacao-detalhada-alteracao.component.html',
})

export class ModalSolicitacaoDetalhadaComponent implements OnInit {

	formPai = this;
	model: any;
	servico = 'BuscarValoresInsumo';
	parametros: any = {};
	grid: any = { sort: {} };
	
	insumoStatusUm: any;
	insumoStatusDois: any;
	listaDetalheInsumo = Array();
	produto: {};

	// Alteraçoes Solicitadas

	listaSolicitacaoDetalhe = Array();

	// Valores Atuais

	valorSaldoQuantidade = 0;
	somatorioValorUnitarioFOB = 0;
	somatorioValorUnitarioCRF = 0;
	somatorioSaldoValor = 0;
	valorSaldoComprovado = 0;
	somatorioValorTotalFOB = 0;
	somatorioValorTotalCRF = 0;
	valorTotalAprovado = 0;


	// Valores Previstos

	valorSaldoQuantidadePrevista = 0;
	somatorioValorUnitarioFOBPrevisto = 0;
	somatorioValorUnitarioCRFPrevisto = 0;
	somatorioSaldoFinal = 0;
	valorSaldoAcrescimo = 0;
	somatorioValorTotalFOBPrevisto = 0;
	somatorioValorTotalCRFPrevisto = 0;
	valorTotalAprovadoPrevisto = 0;

	paridadeValoresStatusUm : Number = 0;
	paridadeValoresStatusDois : Number = 0;


	listaDetalhesStatusDois = Array();


	@ViewChild('appModalSolicitacaoDetalhada') appModalSolicitacaoDetalhada;
	@ViewChild('appModalSolicitacaoDetalhadaBackground') appModalSolicitacaoDetalhadaBackground;

	constructor(
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

	public abrir(item) {	
		this.appModalSolicitacaoDetalhada.nativeElement.style.display = 'block';
		this.appModalSolicitacaoDetalhadaBackground.nativeElement.style.display = 'block';
		this.buscarValoresInsumo(item)
	}

	public fechar() {
		this.appModalSolicitacaoDetalhada.nativeElement.style.display = 'none';
		this.appModalSolicitacaoDetalhadaBackground.nativeElement.style.display = 'none';
		this.limparDadosMostrados();
	}

	buscarValoresInsumo(item){
		this.applicationService.get(this.servico, {codigoInsumo: item.codigoInsumo, idProduto: item.idPrcProduto}).subscribe( (result: any) => {

			this.listaSolicitacaoDetalhe = null;

			this.insumoStatusUm = result[0]
			let insumoStatusUm = result[0]
			if(this.insumoStatusUm != null){

				this.somatoriosValoresAtuais();
				if(result[1] != null){
					this.listaSolicitacaoDetalhe = result[1].listaSolicDetalhe;
				}
				
				 
				if(insumoStatusUm.listaDetalheInsumos.length > 0) {
					insumoStatusUm.listaDetalheInsumos[0].moeda.codigoMoeda > 0 ?					
						this.recuperarValorParidadeStatusUm(Number(insumoStatusUm.listaDetalheInsumos[0].moeda.codigoMoeda)) : 
							0;
				}
			}

			this.insumoStatusDois = result[1]
			let insumoStatusDois = result[1]
			if(this.insumoStatusDois != null){
				
				this.valorSaldoAcrescimo = this.insumoStatusDois.ValorDolarSaldo;

				this.somatorioValoresPrevistos();
					this.listaDetalhesStatusDois = insumoStatusDois;

				if(insumoStatusDois.listaDetalheInsumos.length > 0) {
					insumoStatusDois.listaDetalheInsumos[0].moeda.codigoMoeda > 0 ?					
						this.recuperarValorParidadeStatusDois(Number(insumoStatusDois.listaDetalheInsumos[0].moeda.codigoMoeda)) : 
							0;
				}
			}	
		});
	}

	somatoriosValoresAtuais(){
		this.valorSaldoQuantidade = this.insumoStatusUm.quantidadeSaldo != null ? this.insumoStatusUm.quantidadeSaldo:0;
		this.insumoStatusUm.listaDetalheInsumos.forEach(element => {
			this.somatorioValorUnitarioFOB += element.valorUnitario != null ? element.valorUnitario:0;
		});
		this.somarValorUnitarioCRF();
		this.somatorioSaldoValor = (this.insumoStatusUm.valorDolarSaldo != null) ? this.insumoStatusUm.valorDolarSaldo: 0;
		this.valorSaldoComprovado = (this.insumoStatusUm.valorDolarComp != null) ? this.insumoStatusUm.valorDolarCom: 0;

		if(this.insumoStatusUm.valorDolarFOBAprovado != null){
			this.somatorioValorTotalFOB += this.insumoStatusUm.valorDolarFOBAprovado;
		}
		if(this.insumoStatusUm.valorAdicional != null){
			this.somatorioValorTotalFOB += this.insumoStatusUm.valorAdicional;
		}

		if(this.insumoStatusUm.valorDolarFOBAprovado != null){
			this.somatorioValorTotalCRF += this.insumoStatusUm.valorDolarFOBAprovado;
			this.valorTotalAprovado += this.insumoStatusUm.valorDolarFOBAprovado;
		}
		if(this.insumoStatusUm.valorAdicional != null){
			this.somatorioValorTotalCRF += this.insumoStatusUm.valorAdicional;
			this.valorTotalAprovado += this.insumoStatusUm.valorAdicional;
		}
		if(this.insumoStatusUm.valorFreteAprovado != null){
			this.somatorioValorTotalCRF += this.insumoStatusUm.valorFreteAprovado;
			this.valorTotalAprovado += this.insumoStatusUm.valorFreteAprovado;
		}
		if(this.insumoStatusUm.valorAdicionalFrete != null){
			this.somatorioValorTotalCRF += this.insumoStatusUm.valorAdicionalFrete;
			this.valorTotalAprovado += this.insumoStatusUm.valorAdicionalFrete;
		}
	}

	somarValorUnitarioCRF(){
		var aux1 = 0;
		var aux2 = 0;

		if(this.insumoStatusUm.valorDolarFOBAprovado != null){
			aux1 += this.insumoStatusUm.valorDolarFOBAprovado;
		}
		if(this.insumoStatusUm.valorAdicional != null){
			aux1 += this.insumoStatusUm.valorAdicional;
		}
		if(this.insumoStatusUm.valorFreteAprovado != null){
			aux1 += this.insumoStatusUm.valorFreteAprovado;
		}
		if(this.insumoStatusUm.valorAdicionalFrete != null){
			aux1 += this.insumoStatusUm.valorAdicionalFrete;
		}

		if(this.insumoStatusUm.quantidadeAprovado != null){
			aux2 += this.insumoStatusUm.quantidadeAprovado;
		}
		if(this.insumoStatusUm.quantidadeAdicional != null){
			aux2 += this.insumoStatusUm.quantidadeAdicional;
		}

		if(aux2 != 0){
			this.somatorioValorUnitarioCRF = aux1/aux2;
		}else{
			this.somatorioValorUnitarioCRF = aux1;
		}
	}

	somatorioValoresPrevistos(){
		if(this.insumoStatusDois.quantidadeSaldo != null){
			this.valorSaldoQuantidadePrevista = this.insumoStatusDois.quantidadeSaldo;
		}
		this.insumoStatusDois.listaDetalheInsumos.forEach(element => {
			this.somatorioValorUnitarioFOBPrevisto += element.valorUnitario != null? element.valorUnitario: 0;
		});
		this.somarValorUnitarioCRFPrevisto();
		this.somatorioSaldoFinal = this.insumoStatusDois.valorDolarSaldo != null? this.insumoStatusDois.valorDolarSaldo: 0;
		this.valorSaldoAcrescimo = this.insumoStatusDois.valorDolarSaldo != null? this.insumoStatusDois.valorDolarSaldo: 0;

		if(this.insumoStatusDois.valorDolarFOBAprovado != null){
			this.somatorioValorTotalFOBPrevisto += this.insumoStatusDois.valorDolarFOBAprovado;
		}
		if(this.insumoStatusDois.valorAdicional != null){
			this.somatorioValorTotalFOBPrevisto += this.insumoStatusDois.valorAdicional;
		}

		if(this.insumoStatusDois.valorDolarFOBAprovado != null){
			this.somatorioValorTotalCRFPrevisto += this.insumoStatusDois.valorDolarFOBAprovado;
			this.valorTotalAprovadoPrevisto += this.insumoStatusDois.valorDolarFOBAprovado;
		}
		if(this.insumoStatusDois.valorAdicional != null){
			this.somatorioValorTotalCRF += this.insumoStatusDois.valorAdicional;
			this.valorTotalAprovadoPrevisto += this.insumoStatusDois.valorAdicional;
		}
		if(this.insumoStatusDois.valorFreteAprovado != null){
			this.somatorioValorTotalCRF += this.insumoStatusDois.valorFreteAprovado;
			this.valorTotalAprovadoPrevisto += this.insumoStatusDois.valorFreteAprovado;
		}
		if(this.insumoStatusDois.valorAdicionalFrete != null){
			this.somatorioValorTotalCRF += this.insumoStatusDois.valorAdicionalFrete;
			this.valorTotalAprovadoPrevisto += this.insumoStatusDois.valorAdicionalFrete;
		}
	}

	somarValorUnitarioCRFPrevisto(){
		var aux1 = 0;
		var aux2 = 0;

		if(this.insumoStatusDois.valorDolarFOBAprovado != null){
			aux1 += this.insumoStatusUm.valorDolarFOBAprovado;
		}
		if(this.insumoStatusDois.valorAdicional != null){
			aux1 += this.insumoStatusUm.valorAdicional;
		}
		if(this.insumoStatusDois.valorFreteAprovado != null){
			aux1 += this.insumoStatusUm.valorFreteAprovado;
		}
		if(this.insumoStatusDois.valorAdicionalFrete != null){
			aux1 += this.insumoStatusUm.valorAdicionalFrete;
		}

		if(this.insumoStatusDois.quantidadeAprovado != null){
			aux2 += this.insumoStatusUm.quantidadeAprovado;
		}
		if(this.insumoStatusDois.quantidadeAdicional != null){
			aux2 += this.insumoStatusUm.quantidadeAdicional;
		}

		if(aux2 != 0){
			this.somatorioValorUnitarioCRFPrevisto = aux1/aux2;
		}else{
			this.somatorioValorUnitarioCRFPrevisto = aux1;
		}
	}

	recuperarValorParidadeStatusUm(codigoMoeda : Number){
		this.applicationService.get("CalculaParidade", {codigoMoeda : codigoMoeda}).subscribe((result: any) => {
				this.paridadeValoresStatusUm = (result.valorParidade > 0) ? result.valorParidade : 0.00;
			}
		);
	}

	recuperarValorParidadeStatusDois(codigoMoeda){
		this.applicationService.get("CalculaParidade", {codigoMoeda : codigoMoeda}).subscribe((result: any) => {
				this.paridadeValoresStatusDois = (result.valorParidade > 0) ? result.valorParidade : 0.00;
			}
		);
	}

	limparDadosMostrados(){
		this.valorSaldoQuantidade = 0;
		this.somatorioValorUnitarioFOB = 0;
		this.somatorioValorUnitarioCRF = 0;
		this.somatorioSaldoValor = 0;
		this.valorSaldoComprovado = 0;
		this.somatorioValorTotalFOB = 0;
		this.somatorioValorTotalCRF = 0;
		this.valorTotalAprovado = 0;
		this.valorSaldoQuantidadePrevista = 0;
		this.somatorioValorUnitarioFOBPrevisto = 0;
		this.somatorioValorUnitarioCRFPrevisto = 0;
		this.somatorioSaldoFinal = 0;
		this.valorSaldoAcrescimo = 0;
		this.somatorioValorTotalFOBPrevisto = 0;
		this.somatorioValorTotalCRFPrevisto = 0;
		this.valorTotalAprovadoPrevisto = 0;
	}
}
//04337168000148

