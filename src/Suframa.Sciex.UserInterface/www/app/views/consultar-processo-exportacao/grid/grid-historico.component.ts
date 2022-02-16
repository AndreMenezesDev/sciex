import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { Router } from '@angular/router';
import { ModalDetalhesInsumoComponent } from '../modal/modal-detalhes-insumo.component';

@Component({
	selector: 'app-consultar-historicos-grid',
	templateUrl: './grid-historico.component.html'
})

export class ConsultarHistoricoGridComponent {

	ocultarTituloGrid = true;
	tituloGrid = "Histórico do Processo de Exportação";

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
	) { }

	@Input() lista: any[];
	@Input() total: number;
	@Input() size: number;
	@Input() page: number;
	@Input() sorted: string;
	@Input() isUsuarioInterno: boolean = false;
	@Input() exportarListagem: boolean;
	@Input() parametros: any = {};
	@Input() formPai: any;
	@Input() somenteLeitura: boolean;

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();

	@ViewChild('appModalDetalheInsumo') appModalDetalheInsumo : ModalDetalhesInsumoComponent;
	@ViewChild('appModalDescricaoObservacao') appModalDescricaoObservacao;
	@ViewChild('appModalDescricaoObservacaoBackground') appModalDescricaoObservacaoBackground;

	public abrirModalDetalheInsumo(dadosInsumo){
		this.appModalDetalheInsumo.abrir(this, false,dadosInsumo);
	}
	abrirModalDescricaoObservacao(item){
		this.appModalDescricaoObservacao.abrir(item);
	}
	changeSize($event) {
		this.onChangeSize.emit($event);
	}

	changeSort($event) {
		this.sorted = $event.field;
		this.onChangeSort.emit($event);
		this.changePage(this.page);
	}

	changePage($event) {
		this.page = $event;
		this.onChangePage.emit($event);
		console.log($event);
	}






}
