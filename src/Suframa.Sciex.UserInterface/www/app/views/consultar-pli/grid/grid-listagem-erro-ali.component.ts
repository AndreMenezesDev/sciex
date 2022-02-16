import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { PagedItems } from '../../../view-model/PagedItems';
import { viewRelatorioAnaliseProcessamentoPliVM } from '../../../view-model/ViewRelatorioAnaliseProcessamentoPliVM';

@Component({
	selector: 'app-manter-relatorio-listagem-erro-ali-grid',
	templateUrl: './grid-listagem-erro-ali.component.html'
})

export class ManterListagemErroAliGridComponent implements OnInit {
	servicoStatusPliAliGrid = 'ErroProcessamentoAliGrid';

	model: manterPliVM = new manterPliVM();
	viewModel: viewRelatorioAnaliseProcessamentoPliVM = new viewRelatorioAnaliseProcessamentoPliVM();

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
	@Input() isShowPanel: boolean = false;
	@Input() mostrarBotao: boolean = true;

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();
	

	ngOnInit() {
	}

	changeSize($event) {
		this.size = $event;
		//this.onChangeSize.emit(+$event);
	}

	changeSort($event) {
		this.sorted = $event.field;
		if (this.parametros == undefined) {
			this.parametros.reverse = true;
		} else {
			this.parametros.reverse = (this.parametros.reverse ? false : true);
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
		this.applicationService.get(this.servicoStatusPliAliGrid, this.parametros).subscribe((result: PagedItems) => {
			this.total = result.total;
			this.lista = result.items;
		});
	}







}
