import { Component, Output, Input, OnInit, EventEmitter, ViewChild, Injectable, OnChanges, SimpleChanges } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterNcmExcecaoVM } from "../../../view-model/ManterNcmExcecaoVM";
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { forEach } from '@angular/router/src/utils/collection';
import { PagedItems } from '../../../view-model/PagedItems';

@Component({
	selector: 'app-historico-li-grid',
	templateUrl: './grid-historico.component.html'
})

@Injectable()
export class ManterCancelarLiHistoricoGridComponent implements OnChanges {
	servicoAliHistorico = 'AliHistorico';

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
	) { }

	@Input() lista: any[];
	@Input() total: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() page: number;
	@Input() exportarListagem: boolean;
	@Input() parametros: any = {};
	@Input() ocultarbotaocheck: boolean;

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();


	ngOnChanges(changes: SimpleChanges) {

	}

	changeSize($event) {
		this.onChangeSize.emit(+$event);
	}

	changeSort($event) {
		this.sorted = $event.field;
		if (this.parametros.reverse == undefined) {
			this.parametros.reverse = true;
		} else {
			this.parametros.reverse = !this.parametros.reverse;
		}
		this.onChangeSort.emit($event);
		this.changePage(this.page);
	}

	changePage($event) {
		this.page = $event;
		this.buscar();
	}

	buscar() {
		this.parametros.page = this.page;
		this.parametros.total = this.total;
		this.parametros.size = this.size;
		this.parametros.sort = this.sorted;

		this.applicationService.get(this.servicoAliHistorico, this.parametros).subscribe((result: PagedItems) => {
			this.total = result.total;
			this.lista = result.items;
		});
	}

}
