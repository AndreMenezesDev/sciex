import { Component, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'toastr-ng2/toastr';
import { ModalService } from '../../../../shared/services/modal.service';
import { ApplicationService } from '../../../../shared/services/application.service';
import { ThrowStmt } from '@angular/compiler';
import { MessagesService } from '../../../../shared/services/messages.service';
import { ConsultarProcessoExportacaoComponent } from '../../consultar-processo-exportacao.component';


@Component({
	selector: 'app-modal-adiamento',
	templateUrl: './modal-adiamento.component.html',
})

export class ModalAdiamentoComponent implements OnInit {

	servicoProrrogarSolicitacao = "ProrrogarSolicitacao";
	model: any = {};
	parametros: any = {};
	@ViewChild('appModalAdiamento') appModalAdiamento;
	@ViewChild('appModalAdiamentoBackground') appModalAdiamentoBackground;
	@ViewChild('btnlimpar') btnlimpar;
	formPrincipal: ConsultarProcessoExportacaoComponent;
	titulo: string;
	isProrrogacaoEspecial: boolean;
	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService
	) {
	}
	ngOnInit() {
	}
	public abrir(item, formPai: ConsultarProcessoExportacaoComponent) {
		this.model = item;
		this.formPrincipal = formPai;
		this.isProrrogacaoEspecial = item.jaPossuiProrrogacao;

		if(this.isProrrogacaoEspecial){
			this.titulo = "em caráter especial";
		}else{
			this.titulo = "de Exportação";
		}
		this.appModalAdiamento.nativeElement.style.display = 'block';
		this.appModalAdiamentoBackground.nativeElement.style.display = 'block';
	}

	confirmarCancelar() {
		this.modal.confirmacao(this.msg.CONFIRMAR_CONFIRMACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.fechar();
				}
			});
	}
	public fechar() {
		this.appModalAdiamento.nativeElement.style.display = 'none';
		this.appModalAdiamentoBackground.nativeElement.style.display = 'none';
	}


	public salvar(){

		if(this.isProrrogacaoEspecial && (this.model.justificativa == undefined || this.model.justificativa == null)){
			this.modal.alerta(this.msg.CAMPO_$_OBRIGATORIO_NAO_INFORMADO.replace('$','justificativa'), 'Atenção')
			return;
		}

		this.applicationService.post(this.servicoProrrogarSolicitacao, this.model).subscribe((result:any) =>{
			if (result.sucesso){
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, 'Sucesso', '').subscribe(()=>{
					this.formPrincipal.listar();
					this.fechar();
				});
			}else{
				this.modal.resposta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, 'Sucesso', '');
				console.log(result.mensagemErros)
			}
		});
	}
}
