import { Component, ViewChild, Output, EventEmitter } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { Router } from '@angular/router';

@Component({
	selector: 'app-modal-estrutura-propria-pe',
	templateUrl: './modal-estrutura-propria-pe.component.html',
})

export class ModalInformacoesPEComponent {

	@ViewChild('appModalEstruturaPropriaPEBackground') appModalEstruturaPropriaPEBackground;
	@ViewChild('appModalEstruturaPropriaPE') appModalEstruturaPropriaPE;

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private router: Router
	) { }



	public abrir() {

		this.appModalEstruturaPropriaPEBackground.nativeElement.style.display = 'block';
		this.appModalEstruturaPropriaPE.nativeElement.style.display = 'block';

	}

	public fechar() {

		this.appModalEstruturaPropriaPEBackground.nativeElement.style.display = 'none';
		this.appModalEstruturaPropriaPE.nativeElement.style.display = 'none';

	}

}

