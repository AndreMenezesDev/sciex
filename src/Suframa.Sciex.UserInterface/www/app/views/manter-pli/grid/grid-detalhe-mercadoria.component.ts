import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterPliMercadoriaVM } from '../../../view-model/ManterPliMercadoriaVM';
import { manterPliDetalheMercadoriaVM } from '../../../view-model/ManterPliDetalheMercadoriaVM';
import { manterPliProdutoVM } from '../../../view-model/ManterPliProdutoVM';
import { Event } from '@angular/router';

@Component({
	selector: 'app-manter-pli-detalhe-mercadoria-grid',
	templateUrl: './grid-detalhe-mercadoria.component.html'
})

export class ManterPliDetalheMercadoriaGridComponent implements OnInit {
	servicoPliDetalheMercadoria = 'PliDetalheMercadoria';
	servicoPliDetalheMercadoriaGrid = 'PliDetalheMercadoriaGrid';
	servicoViewDetalheMercadoria = 'ViewDetalheMercadoriaDropDown';

	modelMercadoria: manterPliMercadoriaVM = new manterPliMercadoriaVM();

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
	@Input() somenteLeitura: boolean = false;

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
		this.changePage(this.page);
	}

	changePage($event) {
		this.page = $event;
		this.onChangePage.emit($event);
	}

	ngOnInit() {

	}

	alterarDetalheMercadoria(item: manterPliDetalheMercadoriaVM) {
		if (item.idPliDetalheMercadoria == undefined) {
			item.index = this.parametros.modal.listaItemMercadoria.indexOf(item);
		}
		else {
			if (this.parametros.modal.model.idPLIAplicacao == 2) {
				item.index = this.parametros.modal.listaItemMercadoria.indexOf(item);
				this.modelMercadoria = this.parametros.modelMercadoria;
				var parametros: any = {};

				parametros.codigoProduto = this.modelMercadoria.codigoProduto;
				parametros.codigoNCMMercadoria = this.modelMercadoria.codigoNCMMercadoria;
				parametros.codigoDetalheMercadoria = item.codigoDetalheMercadoria;
				parametros.Id = item.idDetalheMercadoria;

				if (item.idPLIAplicacao == 2) {
					this.applicationService.get(this.servicoViewDetalheMercadoria, parametros).subscribe(result => {
						this.parametros.modal.idDetalheMercadoria = result[0]['id'];
						item.idDetalheMercadoria = result[0]['id'];
					});
				}
			}
			else {
				item.index = this.parametros.modal.listaItemMercadoria.findIndex(obj => obj.idPliDetalheMercadoria == item.idPliDetalheMercadoria);
				//this.modelMercadoria = this.parametros.modelMercadoria;
			}
		}

		if (item.idPLIAplicacao != 2) {
			this.parametros.modal.descricaoDetalheMercadoria.nativeElement.value = item.descricaoDetalhe;
		}
		else{
			this.parametros.modal.itemMercadoria.valorInput.nativeElement.value = item.descricaoDetalhe;
		}
		
		this.parametros.modal.valorUnitarioCondicaoVenda.nativeElement.value = item.valorUnitarioCondicaoVendaFormatada;
		this.parametros.modal.quantidadeUnidadeComercializada.nativeElement.value = item.quantidadeComercializadaFormatada;
		this.parametros.modal.unidadeComercializada.valorInput.nativeElement.value = item.descricaoUnidadeMedida;
		this.parametros.modal.calcularValorCondicaoVenda();
		this.parametros.modal.calcularValorTotalCondicaoVenda();
		this.parametros.modal.modelDetalheMercadoria = item;
	}

	deletar(item: manterPliDetalheMercadoriaVM) {

		if (this.lista.length > 0) {
			const index: number = this.lista.indexOf(item);
			if (index !== -1) {
				item.excluir = true;
				this.lista[index] = item;
			}

			this.lista = this.lista.filter(
				mer => mer.excluir === false);					

			this.parametros.modal.calcularValorTotalCondicaoVenda(this.lista);
			this.parametros.modal.limparCamposItemMercadoria();

		}
	}

	confirmarExclusao(item: manterPliDetalheMercadoriaVM) {

		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.deletar(item);
				}
			});
	}

}
