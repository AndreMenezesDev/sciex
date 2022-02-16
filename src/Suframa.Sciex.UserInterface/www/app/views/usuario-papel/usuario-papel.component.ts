import { ActivatedRoute } from '@angular/router';
import { ApplicationService } from '../../shared/services/application.service';
import { Component } from '@angular/core';
import { MessagesService } from '../../shared/services/messages.service';
import { ModalService } from '../../shared/services/modal.service';
import { ToastrService } from 'toastr-ng2';

@Component({
	selector: 'app-usuario-papel',
	templateUrl: './usuario-papel.component.html'
})
export class UsuarioPapelComponent {
	parametros: any = {};
	grid: any = { sort: {} };

	constructor(
		private applicationService: ApplicationService,
		private messagesService: MessagesService,
		private modalService: ModalService,
		private route: ActivatedRoute,
		private toastrService: ToastrService,
	) {
		this.listar();
	}

	onChangeSort($event) {
		this.grid.sort = $event;
	}

	onChangePage($event) {
		this.grid.page = $event;
		this.listar();
	}

	limpar() {
		this.parametros = {};
	}

	onChangeStatus($event) {
		this.listar();
	}

	listar() {
		this.parametros.page = this.grid.page;
		this.parametros.size = 2147483647;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;

		this.applicationService.get<any>('UsuarioInternoPapelGrid', this.parametros).subscribe(result => {
			this.grid.lista = result.items;
			this.grid.total = result.total;
		});
	}

	incluir() {
		const parametros = {
			IdUsuarioInterno: this.parametros.idUsuarioInterno,
			IdPapel: this.parametros.idPapel
		};

		this.applicationService.post('UsuarioInternoPapel', parametros).subscribe(result => {
			this.limpar();
			this.listar();
			this.toastrService.success(this.messagesService.OPERACAO_REALIZADA_COM_SUCESSO);
		});
	}
}
