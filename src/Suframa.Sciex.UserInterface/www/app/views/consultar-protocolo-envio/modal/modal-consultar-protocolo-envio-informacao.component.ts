import { Component, ViewChild, Output, EventEmitter } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { Router } from '@angular/router';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
	selector: 'app-modal-consultar-protocolo-envio-informacao',
	templateUrl: './modal-consultar-protocolo-envio-informacao.component.html',
})

export class ModalConsultarProtocoloEnvioComponent {

	@ViewChild('appModalConsultarProtocoloEnvioInformacaoBackground') appModalConsultarProtocoloEnvioInformacaoBackground;
	@ViewChild('appModalConsultarProtocoloEnvioInformacao') appModalConsultarProtocoloEnvioInformacao;

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private router: Router
	) { }



	public abrir() {

		this.appModalConsultarProtocoloEnvioInformacaoBackground.nativeElement.style.display = 'block';
		this.appModalConsultarProtocoloEnvioInformacao.nativeElement.style.display = 'block';

	}

	public fechar() {

		this.appModalConsultarProtocoloEnvioInformacaoBackground.nativeElement.style.display = 'none';
		this.appModalConsultarProtocoloEnvioInformacao.nativeElement.style.display = 'none';

	}

}

