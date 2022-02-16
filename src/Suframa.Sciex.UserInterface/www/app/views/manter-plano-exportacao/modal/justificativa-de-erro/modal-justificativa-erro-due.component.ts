import { Component, ViewChild, Injectable, OnInit } from '@angular/core';
import { ValidationService } from '../../../../shared/services/validation.service';
import { ModalService } from '../../../../shared/services/modal.service';
import { ApplicationService } from '../../../../shared/services/application.service';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../../shared/guards/auth-guard.service';
import { MessagesService } from '../../../../shared/services/messages.service';

@Component({
	selector: 'app-modal-justificativa-erro-due',
	templateUrl: './modal-justificativa-erro-due.component.html',
})

@Injectable()
export class ModalJustificativaErroDueComponent implements OnInit{
	parametros: any = {};
	model: any;
	formPai: any;

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private applicationService: ApplicationService,
		private router: Router,
		private authguard: AuthGuard,
		private msg: MessagesService,
	) { }

	@ViewChild('appModalJustificativaErroDue') appModalJustificativaErroDue;
	@ViewChild('appModalJustificativaErroDueBackground') appModalJustificativaErroDueBackground;

	ngOnInit() {
		this.model = {};

	}

	public abrir(item) {
		this.model = item;
		this.appModalJustificativaErroDue.nativeElement.style.display = 'block';
		this.appModalJustificativaErroDueBackground.nativeElement.style.display = 'block';
	}

	public fechar() {
		this.appModalJustificativaErroDue.nativeElement.style.display = 'none';
		this.appModalJustificativaErroDueBackground.nativeElement.style.display = 'none';
	}
}
