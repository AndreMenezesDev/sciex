import { Component, ViewChild, OnInit } from '@angular/core';
import { ValidationService } from '../../../../shared/services/validation.service';
import { ModalService } from '../../../../shared/services/modal.service';
import { MessagesService } from '../../../../shared/services/messages.service';
import { ApplicationService } from '../../../../shared/services/application.service';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../../shared/guards/auth-guard.service';
import { ToastrService } from 'toastr-ng2/toastr';
import { AuthenticationService } from '../../../../shared/services/authentication.service';
import { PagedItems } from '../../../../view-model/PagedItems';
import { AssertNotNull, ThrowStmt } from '@angular/compiler';



@Component({
	selector: 'app-modal-analise-solicitacao',
	templateUrl: './analise-solicitacao-alteracao.component.html',
})

export class ModalAnaliseSolicitacaoComponent implements OnInit {

	formPai: any;
	formModal =this;
	objeto: any
	model: any;
	servico = 'SolictitacoesAlteracaoDetalheInsumo';
	servicoAnaliseInsumoImportado ="AnaliseInsumoImportado";
	servicoCalculoParidade="CalculoParidadeValorParaInsumo"
	servicoAprovar="AprovarSolicitacao"
	servicoReprovar ="ReprovarSolicitacao"
	parametros: any = {};
	grid: any = { sort: {} };
	idProduto : number = 0;
	idProcesso : number = 0;
	listaDetalhesSolicAlteracao : any = {} = null;
	descInsumo : string = "";
	codigoInsumo : string = "";
	numeroSequencial : string = "";
	IdInsumo : number 
	valorDe :any
	valorPara:any
	listaDetalhe: any = {}
	
	//valores DE
	paridade :any
	somatoriaValorUnitarioDeFOB =0 
	justificativa : string 
	quantidadeSaldoDe: any
	valorUnitarioCFRDe :any
	saldoValorDe: any
	valorComprovadoDe: any
	valorTotalFOBDe : any
	valorTotalCFR : any
	valorAprovadoDe: any

	//valores Para
	somatoriaValorUnitarioDeFOBPara=0
	quantidadeSaldoPara: any
	valorUnitarioCFRPara :any
	saldoValorPara: any
	valorAcrescimoPara: any
	valorTotalFOBPara : any
	valorTotalCFRPara: any
	valorAprovadoPara: any
	
	@ViewChild('appModalAnaliseSolicitacao') appModalAnaliseSolicitacao;
	@ViewChild('appModalAnaliseSolicitacaoBackground') appModalAnaliseSolicitacaoBackground;
	@ViewChild('appModalJustificativaReprovacao') appModalJustificativaReprovacao;


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

	public abrir(objeto,formPai) {	

		this.parametros.idProcesso = objeto.prcInsumo.produto.idProcesso;
		this.descInsumo = objeto.prcInsumo.codigoInsumo + " | " + objeto.prcInsumo.descricaoInsumo;
		this.numeroSequencial = objeto.numeroSequencial;
		this.IdInsumo =objeto.prcInsumo.idInsumo
		this.objeto=objeto

		this.formPai = formPai
		this.parametros.PRCInsumoDE = {};
		this.parametros.PRCInsumoDE.IdInsumo = objeto.prcInsumo.idInsumo;		

		this.parametros.PRCInsumoDE.codigoInsumo = objeto.prcInsumo.codigoInsumo;
		this.parametros.PRCInsumoDE.idPrcProduto = objeto.prcInsumo.idPrcProduto;
		this.abrirModal()
		this.buscar();
		

	}
	public abrirModal(){

		this.appModalAnaliseSolicitacao.nativeElement.style.display = 'block';
		this.appModalAnaliseSolicitacaoBackground.nativeElement.style.display = 'block';
	}
	public fecharModal(){
		this.appModalAnaliseSolicitacao.nativeElement.style.display = 'none';
		this.appModalAnaliseSolicitacaoBackground.nativeElement.style.display = 'none';
	
	}
	public fechar() {

		this.descInsumo = "";
		this.numeroSequencial = "";

		this.grid = { sort: {} };
		this.parametros = {};
	
		this.formPai.carregarListaDetalheInsumo()
		this.fecharModal()
	}

	abrirModalJustificativa(item){
		this.appModalJustificativaReprovacao.abrir(item, this.formModal,this.objeto,this.formPai);
		this.fechar()
	}

	buscar(){
		var objetoConsulta = {codigoInsumo: this.parametros.PRCInsumoDE.codigoInsumo, idProduto: this.parametros.PRCInsumoDE.idPrcProduto  }
		this.applicationService.get(this.servicoAnaliseInsumoImportado,objetoConsulta ).subscribe((result: any) => {

		if(result!=null){
			this.valorDe=result[0]
			console.log(this.valorDe)
			this.valorPara= result[1]
			console.log(this.valorPara)
			if(this.valorPara!=null){
				
				this.listaDetalhe = result[1].listaSolicDetalhe
				console.log(this.listaDetalhe)
			}
			console.log(this.valorPara)
			this.calculaValorDe()
			this.calcularValoresPrevistos()
			this.calcularValoresPrevistosPara()
			this.calculaValorPara()
			this.calculoParidade()


		}
			this.listaDetalhesSolicAlteracao = {};
			this.listaDetalhesSolicAlteracao = (result) ? result : null;
		});
	}
	
	calcularValoresPrevistos(){
		console.log(this.valorDe)
		if(this.valorDe==null){
			this.somatoriaValorUnitarioDeFOBPara= 0;
			return 0
			
		}
		if(this.valorDe.listaDetalheInsumos!=null){
			var objeto_somatoria =this.valorDe.listaDetalheInsumos	
			objeto_somatoria.map(item =>{
				console.log(item)
			 this.somatoriaValorUnitarioDeFOB +=item.valorUnitario
		})
	}
	else{
		this.somatoriaValorUnitarioDeFOB =0;
		}
	}

	calcularValoresPrevistosPara(){
		console.log(this.valorPara)
		if(this.valorPara==null){
			this.somatoriaValorUnitarioDeFOBPara= 0;
			return 0
		}
		if(this.valorPara.listaDetalheInsumos!=null){
		var objeto_somatoria =	this.valorPara.listaDetalheInsumos
		objeto_somatoria.map(item =>{
			if(item.valorUnitario==null){
				this.somatoriaValorUnitarioDeFOB +=0;
			}else{
			
			 this.somatoriaValorUnitarioDeFOBPara +=item.valorUnitario
			}
			})
		}
	}

	calculaValorDe(){
		
		if(this.valorDe !=null){
		this.quantidadeSaldoDe= this.valorDe.quantidadeSaldo
	
		this.valorUnitarioCFRDe = (this.valorDe.valorDolarFOBAprovado+this.valorDe.valorAdicional+ this.valorDe.valorFreteAprovado+this.valorDe.valorAdicionalFrete)/(this.valorDe.quantidadeAprovado+this.valorDe.quantidadeAdicional)
	
		this.saldoValorDe = this.valorDe.valorDolarSaldo
		
		this.valorComprovadoDe =this.valorDe.valorDolarComp
		
		this.valorTotalFOBDe =this.valorDe.valorDolarFOBAprovado+this.valorDe.valorAdicional
	
		this.valorTotalCFR =this.valorDe.valorDolarFOBAprovado+this.valorDe.valorAdicional+this.valorDe.valorFreteAprovado+this.valorDe.valorAdicionalFrete
	
		this.valorAprovadoDe =this.valorDe.valorDolarFOBAprovado+this.valorDe.valorAdicional+this.valorDe.valorFreteAprovado+this.valorDe.valorAdicionalFrete
		}else{
		this.quantidadeSaldoDe= 0
	
		this.valorUnitarioCFRDe = 0
	
		this.saldoValorDe = 0
		
		this.valorComprovadoDe =0
		
		this.valorTotalFOBDe =0
	
		this.valorTotalCFR =0
	
		this.valorAprovadoDe =0
	}
    }

	calculaValorPara(){
		if(this.valorPara!=null){
		this.quantidadeSaldoPara= this.valorPara.quantidadeSaldo
		
		this.valorUnitarioCFRPara = (this.valorPara.valorDolarFOBAprovado+this.valorPara.valorAdicional+ this.valorPara.valorFreteAprovado+this.valorPara.valorAdicionalFrete)/(this.valorPara.quantidadeAprovado+this.valorPara.quantidadeAdicional)
	
		this.saldoValorPara = this.valorPara.valorDolarSaldo
	
		this.valorAcrescimoPara =this.valorPara.valorAdicional
	
		this.valorTotalFOBPara =this.valorPara.valorDolarFOBAprovado+this.valorPara.valorAdicional
	
		this.valorTotalCFRPara =this.valorPara.valorDolarFOBAprovado+this.valorPara.valorAdicional+this.valorPara.valorFreteAprovado+this.valorPara.valorAdicionalFrete
	
		this.valorAprovadoPara =this.valorPara.valorDolarFOBAprovado+this.valorPara.valorAdicional+this.valorPara.valorFreteAprovado+this.valorPara.valorAdicionalFrete
		}
		else{
			this.quantidadeSaldoPara= 0
		
			this.valorUnitarioCFRPara = 0
		
			this.saldoValorPara =0
			
			this.valorAcrescimoPara=0
			
			this.valorTotalFOBPara =0
		
			this.valorTotalCFRPara =0
			
			this.valorAprovadoPara =0
		
			
	}
    }

	calculoParidade(){
			this.applicationService.get(this.servicoCalculoParidade).subscribe((result :any)=>{
				this.paridade =result
			})
	}

	aprovarSolicitacao(item){
			
		console.log(item)
			this.applicationService.get(this.servicoAprovar,item).subscribe((result :any)=> {
				if(result.resultado){
					this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO,"Sucesso","").subscribe(()=>{
						this.buscar()
					})
				}else{
					if (result.codigoErro == 1) {
						this.modal.alerta('Não existe paridade cadastrada no dia de hoje.',"Atenção","");
					}else{
						this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO,"Atenção","");
						console.log(result.mensagem);
					}
				
				}
			})

	}

	

	

}