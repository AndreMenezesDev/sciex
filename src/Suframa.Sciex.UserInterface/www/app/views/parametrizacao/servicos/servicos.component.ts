import { ApplicationService } from '../../../shared/services/application.service';
import { Component, OnDestroy } from '@angular/core';
import { MessagesService } from '../../../shared/services/messages.service';
import { ToastrService } from 'toastr-ng2';

@Component({
	selector: 'app-parametrizacao-servicos',
	templateUrl: './servicos.component.html'
})
export class ParametrizacaoServicosComponent {
	servicos: any;

	constructor(
		private applicationService: ApplicationService,
		private messagesService: MessagesService,
		private toastrService: ToastrService,
	) {
		this.listarServicos();
	}

	listarServicos() {
		this.applicationService.get<any>('Servico').subscribe(result => { this.servicos = result; });
	}

	salvar() {
		this.applicationService.post('Servico', this.servicos).subscribe(result => {
			this.toastrService.success(this.messagesService.OPERACAO_REALIZADA_COM_SUCESSO);
		});
	}
}
