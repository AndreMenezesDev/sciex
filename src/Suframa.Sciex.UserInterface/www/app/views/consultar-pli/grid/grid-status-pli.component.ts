import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { PagedItems } from '../../../view-model/PagedItems';
import { viewRelatorioAnaliseProcessamentoPliVM } from '../../../view-model/ViewRelatorioAnaliseProcessamentoPliVM';
import { manterPliMercadoriaVM } from '../../../view-model/ManterPliMercadoriaVM';
import { manterLiVM } from '../../../view-model/ManterLiVM';

@Component({
	selector: 'app-manter-status-pli-grid',
	templateUrl: './grid-status-pli.component.html'
})

export class ManterConsultarRelatorioStatusPliGridComponent implements OnInit {

	servicoStatusPliGrid = 'RelatorioAnaliseAliGrid';
	servico = 'Li';
	mensagem: string;

	model: manterPliVM = new manterPliVM();
	viewModel: viewRelatorioAnaliseProcessamentoPliVM = new viewRelatorioAnaliseProcessamentoPliVM();

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
	) { }

	@ViewChild('appModalAliIndeferidaBackground') appModalAliIndeferidaBackground;
	@ViewChild('appModalAliIndeferida') appModalAliIndeferida;

	@Input() lista: any[];
	@Input() total: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() page: number;
	@Input() exportarListagem: boolean;
	@Input() parametros: any = {};
	@Input() isShowPanel: boolean = false;
	@Input() mostrarBotao: boolean = true;
	@Input() quantidadeErroAli: number;
	@Input() notComercializacao: boolean = true;

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();


	ngOnInit() {
		if (localStorage.getItem("GridStatusPli") != null) {
			this.parametros = JSON.parse(localStorage.getItem("GridStatusPli"));
		}
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
		console.log(this.parametros);
		if (!localStorage.getItem("GridStatusPli") != null) {
			this.parametros.page = this.page;
			this.parametros.total = this.total;
			this.parametros.size = this.size;
			this.parametros.sort = this.sorted;
		} else {
			this.parametros = JSON.parse(localStorage.getItem("GridStatusPli"));
		}

		this.applicationService.get(this.servicoStatusPliGrid, this.parametros).subscribe((result: PagedItems) => {
			this.total = result.total;
			this.lista = result.items;

			this.gravarBusca();
		});
	}

	mostrarMensagemLi(idMercadoria) {

		this.parametros.idMercadoria = idMercadoria;

		this.applicationService.get<manterLiVM>(this.servico, this.parametros).subscribe(result => {

			this.mensagem = result.mensagemErroLI;
			this.appModalAliIndeferida.abrir(this.mensagem);

		});

	}

	gravarBusca() {

		this.parametros.page = this.page;
		this.parametros.total = this.total;
		this.parametros.size = this.size;
		this.parametros.sort = this.sorted;

		localStorage.removeItem("GridStatusPli");
		localStorage.setItem("GridStatusPli", JSON.stringify(this.parametros));

	}
}
