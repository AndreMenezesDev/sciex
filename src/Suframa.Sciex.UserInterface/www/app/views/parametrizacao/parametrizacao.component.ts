import { ActivatedRoute } from '@angular/router';
import { ApplicationService } from '../../shared/services/application.service';
import { Component } from '@angular/core';
import { MessagesService } from '../../shared/services/messages.service';
import { ModalService } from '../../shared/services/modal.service';

@Component({
	selector: 'app-parametrizacao',
	templateUrl: './parametrizacao.component.html'
})
export class ParametrizacaoComponent {
	constructor(
		private activatedRoute: ActivatedRoute,
		private applicationService: ApplicationService,
		private messagesService: MessagesService,
		private modalService: ModalService,
	) { }
}
