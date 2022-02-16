import { Component, OnInit, Injectable, Input } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';

@Component({
	selector: 'app-manter-regime-tributario-teste',
	templateUrl: './manter-regime-tributario-teste.component.html'
})                        

@Injectable()
export class ManterRegimeTributarioTesteComponent implements OnInit {
	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = true;
	teste: string = 'teste';
	servicoRegimeTributarioTesteGrid = 'RegimeTributarioTesteGrid';

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService
	) { }

	ngOnInit(): void {
		console.log('Chegou aqui');
		this.listar();
	}

	buscar() {
		if (
			!this.parametros.codigo &&
			!this.parametros.descricao) {
			this.modal.alerta(this.msg.INFORME_OS_FILTROS_DE_PESQUISA, 'Informação');
		} else {
			this.grid.page = 1;
			this.listar();
		}
	}

	ocultar() {
		if (this.ocultarFiltro === false)
			this.ocultarFiltro = true;
		else
			this.ocultarFiltro = false;
	}

	onChangeSort($event) {
		this.grid.sort = $event;
	}

	onChangeSize($event) {
		this.grid.size = $event;
	}

	onChangePage($event) {
		this.grid.page = $event;
		this.listar();
	}

	limpar() {
		this.parametros = {};
		this.listar();
	}

	listar() {
		console.log('Chegou aqui');
		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;

		this.applicationService.get(this.servicoRegimeTributarioTesteGrid, this.parametros).subscribe((result: PagedItems) => {
			this.grid.lista = result.items;
			this.grid.total = result.total;

			if ((!this.grid.lista || this.grid.lista.length == 0)) {
				this.ocultarGrid = true;
				this.modal.alerta(this.msg.NENHUM_REGISTRO_ENCONTRADO);
			}
			else {
				this.ocultarGrid = false;
			}
		});
	}
}
