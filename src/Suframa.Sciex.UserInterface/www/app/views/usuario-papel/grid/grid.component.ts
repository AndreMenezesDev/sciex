import { ApplicationService } from '../../../shared/services/application.service';
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ToastrService } from 'toastr-ng2';

@Component({
	selector: 'app-usuario-papel-grid',
	templateUrl: './grid.component.html',
})
export class UsuarioPapelGridComponent {
	colorClass = 'even';

	@Input() lista: any;
	@Input() page: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() total: number;

	@Output() changeStatus: EventEmitter<any> = new EventEmitter<any>();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();

	constructor(
		private applicationService: ApplicationService,
		private messagesService: MessagesService,
		private modalService: ModalService,
		private toastrService: ToastrService) { }

	changeSize($event) {
		this.onChangeSize.emit($event);
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

	ativarInativar(item) {
		const parametros = { IdUsuarioInternoPapel: item.idUsuarioInternoPapel };

		this.applicationService.put('UsuarioInternoPapel', parametros).subscribe(result => {
			item.isAtivo = !item.isAtivo;
			this.toastrService.success(this.messagesService.OPERACAO_REALIZADA_COM_SUCESSO);
			this.changeStatus.emit();
		});
	}

	excluir(item) {
		this.applicationService.delete('UsuarioInternoPapel', item.idUsuarioInternoPapel).subscribe(result => {
			this.changePage(1);
			this.toastrService.success(this.messagesService.OPERACAO_REALIZADA_COM_SUCESSO);
		});
	}

	getColorClass(index) {
		if (index == 0) {
			this.colorClass = 'even';
			return this.colorClass;
		}

		const papelAnterior = this.lista[index - 1].papel;

		const papelAtual = this.lista[index].papel;

		if (papelAnterior != papelAtual) {
			this.colorClass = (this.colorClass == 'odd' ? 'even' : 'odd');
		}

		return this.colorClass;
	}
}
