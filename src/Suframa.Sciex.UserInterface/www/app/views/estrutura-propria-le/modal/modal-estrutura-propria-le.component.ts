import { Component, ViewChild, Output, EventEmitter } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { Router } from '@angular/router';

@Component({
	selector: 'app-modal-estrutura-propria-le',
	templateUrl: './modal-estrutura-propria-le.component.html',
})

export class ModalAjudaEnviarLEComponent {

	@ViewChild('appModalEstruturaPropriaLEBackground') appModalEstruturaPropriaLEBackground;
	@ViewChild('appModalEstruturaPropriaLE') appModalEstruturaPropriaLE;

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private router: Router
	) { }



	public abrir() {

		this.appModalEstruturaPropriaLEBackground.nativeElement.style.display = 'block';
		this.appModalEstruturaPropriaLE.nativeElement.style.display = 'block';

	}

	public fechar() {

		this.appModalEstruturaPropriaLEBackground.nativeElement.style.display = 'none';
		this.appModalEstruturaPropriaLE.nativeElement.style.display = 'none';

	}

}

