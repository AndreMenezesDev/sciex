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
import { ThrowStmt } from '@angular/compiler';


@Component({
	selector: 'app-modal-analisar-pedido-prorrogacao',
	templateUrl: './modal-analisar-pedido-prorrogacao.component.html',
})

export class ModalAnalisarPedidoProrrogacaoComponent implements OnInit {
	formPai :any;
	model: any;
	servicoGrid = 'ParecerTecnicoSuframa';
	servicoListarRegistroAlteracao="ListarRegistroAlteracao";
	servicoAprovarProrrogacao ="AprovarProrrogacao"
	servicoRecusarProrrogacao="ReprovarProrrogacao"
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = true;
	grid: any = { sort: {} };
	@ViewChild('appModalAnalisarPedidoProrrogacao') appModalAnalisarPedidoProrrogacao;
	@ViewChild('appModalAnalisarPedidoProrrogacaoBackground') appModalAnalisarPedidoProrrogacaoBackground;

	@ViewChild('btnlimpar') btnlimpar;



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

	ngOnInit() {
		this.model = {};
		this.parametros = {};
	}

	public abrir(item,formPai) {
		console.log(item)
		this.formPai= formPai
		this.parametros.idProcesso =item.idProcesso
		this.model =item;
		this.applicationService.get(this.servicoListarRegistroAlteracao, this.parametros).subscribe((result: any)=>{
			this.model.justificativa = result.justificativa;

		})
		this.parametros = {};
		this.grid = {};
		this.appModalAnalisarPedidoProrrogacao.nativeElement.style.display = 'block';
		this.appModalAnalisarPedidoProrrogacaoBackground.nativeElement.style.display = 'block';

	}

	limpar() {
		this.parametros.tipoParecer = undefined;
		this.parametros.numeroControleString = null;
		this.grid.lista = null;
		this.grid.total = 0;
	}

	ocultar() {
		if (this.ocultarFiltro === false)
			this.ocultarFiltro = true;
		else
			this.ocultarFiltro = false;
	}

	public fechar() {
		this.appModalAnalisarPedidoProrrogacao.nativeElement.style.display = 'none';
		this.appModalAnalisarPedidoProrrogacaoBackground.nativeElement.style.display = 'none';
		this.limpar();
	}

	onChangeSort($event) {
		this.grid.sort = $event;

	}

	onChangeSize($event) {
		this.grid.size = $event;
	}

	onChangePage($event) {
		this.grid.page = $event;
		this.buscar();
	}

	buscar() {
		
	}

	aceitar(){
		console.log(this.model)
		this.parametros.idProcesso =this.model.idProcesso
		this.parametros.descricaoObservacao = this.model.justificativa;
		this.parametros.dataValidade = this.model.dataValidadeProrrogada
		this.modal.confirmacao("Confirma a operação?", '', '')
			.subscribe(isConfirmado => {
				
				if (isConfirmado) {
					
					
				
					this.applicationService.get(this.servicoAprovarProrrogacao, this.parametros).subscribe((result: any) => {
						console.log(result)
						if(result.aprovacao==0){
							this.modal.resposta("Operação realizada com sucesso","Informação","").subscribe(()=>{
								this.formPai.buscar(false)
								this.fechar();
							})
						}
						else if(result.aprovacao==1){
							this.modal.alerta("Algo deu errado","Erro"," ")
						} else{
							this.modal.alerta("Cpf nome do responsavel não encontrado","Erro"," ")
						}
					
					});
					
				
				
				}
			});

	}

	reprovar(){

		if(this.parametros.justificativaReprovado!=null){
		this.modal.confirmacao("Confirma a operação?", '', '')
		.subscribe(isConfirmado => {
			if (isConfirmado) {
		this.parametros.idProcesso = this.model.idProcesso;
	
		this.applicationService.get(this.servicoRecusarProrrogacao, this.parametros).subscribe((result:any)=>{
			console.log(result)
			if(result.reprovacao==true){
				this.modal.resposta("Operação realizada com sucesso","Informação","").subscribe(()=>{
					this.formPai.buscar(false)
					this.fechar();
				})
		
			}else{
				this.modal.alerta("Ocorreu um erro","Atenção"," ")
			}

		});
	}	
			})}
			else{
				this.modal.alerta("Preencha o campo Justificativa","Atenção","")
			}


	
	}

}
