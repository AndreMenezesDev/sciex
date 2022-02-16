import { Component, Output, Input, EventEmitter } from '@angular/core';

import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ToastrService } from 'toastr-ng2/toastr';

@Component({
	selector: 'app-manter-regime-tributario-teste-grid',
	templateUrl: './grid.component.html'
})
export class ManterRegimeTributarioTesteGridComponent {
	servicoRegimeTributarioTeste = 'RegimeTributarioTeste';

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
		private toastrService: ToastrService,
	) { }

	@Input() lista: any;
	@Input() total: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() page: number;

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();

	changeSize($event) {
		this.onChangeSize.emit(+$event);
	}

	changeSort($event) {
		this.sorted = $event.field;
		this.onChangeSort.emit($event);
		this.changePage(1);
	}

	changePage($event) {
		this.page = $event;
		this.onChangePage.emit($event);
	}

	deletar(id) {
		this.applicationService.delete(this.servicoRegimeTributarioTeste, id).subscribe(result => {
			this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", '');
			this.changePage(this.page);
		});
	}

	confirmarExclusao(id) {
		this.modal.confirmacao(this.msg.CONFIRMAR_EXCLUSAO, 'Exclusão','')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.deletar(id);
				}
			});
	}
}
