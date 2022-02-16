import { Component, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'toastr-ng2/toastr';
import { ModalService } from '../../../../shared/services/modal.service';
import { ApplicationService } from '../../../../shared/services/application.service';
import { ThrowStmt } from '@angular/compiler';
import { ModalJustificativaErroComponent } from '../../../manter-plano-exportacao/modal/justificativa-de-erro/modal-justificativa-erro.component';
import { MessagesService } from '../../../../shared/services/messages.service';

@Component({
	selector: 'app-modal-justificativa-reprovacao',
	templateUrl: './modal-justificativa.component.html',
})

export class ModalJustificativaReprovacaoComponent implements OnInit {

	model: any;
	servico='CancelarProcesso'
	parametros: any = {};
	servicoReprovar="ReprovarSolicitacao"
	formModal:any
	formPai :any
	objeto:any

	@ViewChild('appModalJustificativaReprovacao') appModalJustificativaReprovacao;
	@ViewChild('appModalJustificativaReprovacaoBackground') appModalJustificativaReprovacaoBackground;
	@ViewChild('appModalAnaliseSolicitacao') appModalAnaliseSolicitacao;
	@ViewChild('btnlimpar') btnlimpar;



	constructor(
		private toastrService: ToastrService,
		private router: Router,
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
	

	) {

	}

	ngOnInit() {
		this.model = {};
		this.parametros = {};

	}

 public abrir(item,formModal,objeto,formPai){

		this.model = item;
		this.parametros = {};
		this.objeto= objeto
		this.formPai =formPai
		this.formModal =formModal
	     this.appModalJustificativaReprovacao.nativeElement.style.display = 'block';
  		 this.appModalJustificativaReprovacaoBackground.nativeElement.style.display = 'block';
         console.log(item)
		 this.formModal.fecharModal()
	
		}

	limpar() {

	}


	public fechar() {
		this.appModalJustificativaReprovacao.nativeElement.style.display = 'none';
		this.appModalJustificativaReprovacaoBackground.nativeElement.style.display = 'none';

	}

	public cancelar(){
		this.parametros.idProcesso= this.model.idProcesso;
	}
	reprovarSolicitacao(){
		
		if(this.parametros.justificativa==null || this.parametros.justificativa == undefined){
			this.modal.alerta("Preencha o campo Justificativa ", "Atenção", "");
			return;
		}else{

		this.model.justificativa = this.parametros.justificativa
		this.applicationService.get(this.servicoReprovar,this.model).subscribe((result :any)=> {
			if(result.resultado){
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO,"Sucesso","").subscribe(()=>{
					this.fechar()
					this.formModal.abrir(this.objeto,this.formPai)
					this.formModal.buscar()
				
				})
			}else if(result.resultado){
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO,"Atenção","")
				console.log(result.mensagem);
			
			}
		})
		

	
}
}






}
