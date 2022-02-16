import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterPliMercadoriaVM } from '../../../view-model/ManterPliMercadoriaVM';
import { PagedItems } from '../../../view-model/PagedItems';

@Component({
	selector: 'app-manter-pli-mercadoria-grid',
	templateUrl: './grid-mercadoria.component.html'
})

export class ManterPliMercadoriaGridComponent implements OnInit {
	servicoPliMercadoria = 'PliMercadoria';
	servicoPliMercadoriaGrid = 'PliMercadoriaGrid';
	servicoPliDetalheMercadoria = 'PliDetalheMercadoria';
	servicoPliProcessoAnuente = 'PliProcessoAnuente';

	model: manterPliMercadoriaVM = new manterPliMercadoriaVM();
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
	@Input() isRetificador: boolean = false;

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();

	@Output() retornaItem: EventEmitter<any> = new EventEmitter();

	@ViewChild('appModalMercadoriaPli') appModalMercadoriaPli;
	@ViewChild('appModalMercadoriaPliComercializacao') appModalMercadoriaPliComercializacao;
	@ViewChild('appModalAplicarParametros') appModalAplicarParametros;
	@ViewChild('modalManterPliFormularioRetificador') modalManterPliFormularioRetificador;

	ngOnInit() {
	}

	changeSize($event) {
		this.size = $event;
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

	alterarNCM(item){
		this.retornaItem.emit(item);
	}

	buscar() {
		this.parametros.page = this.page;
		this.parametros.total = this.total;
		this.parametros.size = this.size;
		this.parametros.sort = this.sorted;

		this.applicationService.get(this.servicoPliMercadoriaGrid, this.parametros).subscribe((result: PagedItems) => {
			this.total = result.total;
			this.lista = result.items;
		});
	}

	deletar(item) {
		this.applicationService.delete(this.servicoPliMercadoria, item.idPliMercadoria).subscribe(result => {
				const index: number = this.lista.indexOf(item);
				if (index !== -1) {
					this.total = this.total - 1;
					this.lista.splice(index, 1);
				}
		}, error => {
			const index: number = this.lista.indexOf(item);
			if (index !== -1)
				this.lista.splice(index, 1);
		});
	}

	confirmarExclusao(id) {
		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.deletar(id);
				}
			});
	}

	abrirModalMercadoria(item: manterPliMercadoriaVM) {
		if (item.idPliMercadoria != undefined) {
			this.parametros.idPliMercadoria = item.idPliMercadoria;
			this.applicationService.get<manterPliMercadoriaVM>(this.servicoPliMercadoria, item.idPliMercadoria).subscribe(result => {
				let liref = item.numeroLIReferencia;
				let idPliAplicacao = item.idPLIAplicacao;
                item = result;
				item.numeroLIReferencia = liref;
				item.idPLIAplicacao = idPliAplicacao;

				for (let o of item.listaPliDetalheMercadoriaVM) {
					o.idPLIAplicacao = item.idPLIAplicacao;
				}

				if (idPliAplicacao == 2)
					this.appModalMercadoriaPli.abrir(item, item.listaPliDetalheMercadoriaVM, item.listaPliProcessoAnuenteVM, this.parametros);
				else
					this.appModalMercadoriaPliComercializacao.abrir(item, item.listaPliDetalheMercadoriaVM, item.listaPliProcessoAnuenteVM, this.parametros);
			});

		}
	}

	abrirModalAplicarParametro(item: manterPliMercadoriaVM) {
		this.appModalAplicarParametros.abrir(item.idPliMercadoria, item.idPliProduto, item.idPLI);
	}

}
