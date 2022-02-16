import { Component, ViewChild} from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { Router } from '@angular/router';

@Component({
	selector: 'app-modal-ali-indeferida',
	templateUrl: './modal-ali-indeferida.component.html',
})

export class ModalAliIndeferidaComponent {

	@ViewChild('appModalAliIndeferidaBackground') appModalAliIndeferidaBackground;
	@ViewChild('appModalAliIndeferida') appModalAliIndeferida;
	mensagem: string;

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private router: Router
	) { }



	public abrir(msg) {

		this.mensagem = msg;

		this.appModalAliIndeferidaBackground.nativeElement.style.display = 'block';
		this.appModalAliIndeferida.nativeElement.style.display = 'block';

	}

	public fechar() {

		this.appModalAliIndeferidaBackground.nativeElement.style.display = 'none';
		this.appModalAliIndeferida.nativeElement.style.display = 'none';

	}

}

