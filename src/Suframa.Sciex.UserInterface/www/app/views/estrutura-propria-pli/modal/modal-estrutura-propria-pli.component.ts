import { Component, ViewChild, Output, EventEmitter } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { Router } from '@angular/router';

@Component({
	selector: 'app-modal-estrutura-propria-pli',
	templateUrl: './modal-estrutura-propria-pli.component.html',
})

export class ModalConsultarProtocoloEnvioComponent {

	@ViewChild('appModalEstruturaPropriaPliBackground') appModalEstruturaPropriaPliBackground;
	@ViewChild('appModalEstruturaPropriaPli') appModalEstruturaPropriaPli;

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private router: Router
	) { }



	public abrir() {

		this.appModalEstruturaPropriaPliBackground.nativeElement.style.display = 'block';
		this.appModalEstruturaPropriaPli.nativeElement.style.display = 'block';

	}

	public fechar() {

		this.appModalEstruturaPropriaPliBackground.nativeElement.style.display = 'none';
		this.appModalEstruturaPropriaPli.nativeElement.style.display = 'none';

	}

}

