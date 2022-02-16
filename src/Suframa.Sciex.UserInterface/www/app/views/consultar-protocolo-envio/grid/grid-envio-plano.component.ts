import { Component, Output, Input, OnInit, EventEmitter, ViewChild, Injectable, OnChanges, SimpleChanges } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterNcmExcecaoVM } from "../../../view-model/ManterNcmExcecaoVM";
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { forEach } from '@angular/router/src/utils/collection';
import { EstruturaPropriaPLIVM } from '../../../view-model/EstruturaPropriaPLIVM';
import { SolicitacaoPliVM } from '../../../view-model/SolicitacaoPLIVM';
import { importExpr } from '@angular/compiler/src/output/output_ast';

@Component({
	selector: 'app-consultar-protocolo-envio-plano-grid',
	templateUrl: './grid-envio-plano.component.html'
})

@Injectable()
export class ManterConsultarProtocoloEnvioPlanoGridComponent implements OnInit {

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

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();

	solicitacaoPliVM: SolicitacaoPliVM = new SolicitacaoPliVM();

	ngOnInit() {
		localStorage.removeItem("SolicitacaoPli");
	}

	changeSize($event) {		
		this.onChangeSize.emit(+$event);
	}

	changeSort($event) {
		this.sorted = $event.field;
		this.onChangeSort.emit($event);
		this.changePage(this.page);
	}

	changePage($event) {
		this.page = $event;
		this.onChangePage.emit($event);
	}

}
