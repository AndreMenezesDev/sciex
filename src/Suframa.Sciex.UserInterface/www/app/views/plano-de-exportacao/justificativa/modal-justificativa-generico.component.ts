import { Component, ViewChild } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';

@Component({
	selector: 'app-modal-justificativa-generico',
	templateUrl: './modal-justificativa-generico.component.html',
})

export class ModalJustificativaGenericoComponent {

    parametros : any = {};
	servico: string;
	formPai: any;
	visualizar: boolean = false;

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private applicationService: ApplicationService
	) { }

	@ViewChild('appModalJustificativaGlosa') appModalJustificativaGlosa;
	@ViewChild('appModalJustificativaGlosaBackground') appModalJustificativaGlosaBackground;
	
	public abrir(dados, servico, formPai, visualizar?) {
		this.formPai = formPai;
		this.parametros = dados;
		this.servico = servico;

		if(visualizar == undefined) {
			this.visualizar = false;
			this.parametros.descricaoJustificativa = null;
		}else{
			this.visualizar = true;
		}
		
		this.appModalJustificativaGlosa.nativeElement.style.display = 'block';
		this.appModalJustificativaGlosaBackground.nativeElement.style.display = 'block';
	}

	public fechar() {	
		this.appModalJustificativaGlosa.nativeElement.style.display = 'none';
		this.appModalJustificativaGlosaBackground.nativeElement.style.display = 'none';
	}

	public salvar() {

		/*if (this.parametros.descricaoJustificativaErro == null){
			this.modal.alerta(this.msg.CAMPO_OBRIGATORIO_NAO_INFORMADO.replace('$','justificativa'));
			return;
		}*/

		this.applicationService.post(this.servico, this.parametros).subscribe((result:any) => {
			if (result.resultado){
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "").
				subscribe(()=>{
					this.formPai.atualizarDados();
					this.fechar();
				});
			}else{
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO + " : "+result.mensagem);
			}
		});
	}

	
}
