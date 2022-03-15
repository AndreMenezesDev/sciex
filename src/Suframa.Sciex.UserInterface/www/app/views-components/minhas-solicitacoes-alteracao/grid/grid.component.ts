import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalDetalhesInsumoComponent } from '../../../views/consultar-processo-exportacao/modal/modal-detalhes-insumo.component';

@Component({
	selector: 'app-solicitacoes-alteracao-grid',
	templateUrl: './grid.component.html'
})

export class GridSolicitacoesAlteracaoComponent {

	servicoExcluirInsumo = "ExcluirInsumo";
	servico = 'PlanoExportacao';
	servicoEntregarPlano = 'EntregarPlanoExportacao';
	servicoValidarPlano = 'ValidarPlanoExportacao';
	ocultarTituloGrid = true;
	path : string; 
	url : string;

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
		private route: ActivatedRoute
	) {
		this.url = this.router.url;
	}
	
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

	redirectToDetails(idSolicitacao){

		let url = `/detalhe-minha-solicitacao/${idSolicitacao}`;
		this.setHistoryUrl(url)
		this.router.navigate([url])
	}
	
	setHistoryUrl(url){
		let arrayUrl = sessionStorage.getItem("arrayUrl");
		let listArray = [];
		if (arrayUrl == undefined || arrayUrl == null){
			arrayUrl = `["${url}"]`;
			listArray = JSON.parse( arrayUrl);
		}else{
			listArray = JSON.parse( arrayUrl)
			listArray.push(url)
			sessionStorage.removeItem("arrayUrl");
		}
		
		sessionStorage.setItem("arrayUrl",JSON.stringify(listArray));
	}

}
