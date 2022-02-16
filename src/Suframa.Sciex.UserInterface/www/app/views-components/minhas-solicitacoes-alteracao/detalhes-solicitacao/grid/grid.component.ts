import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../../shared/services/application.service';
import { MessagesService } from '../../../../shared/services/messages.service';
import { ModalService } from '../../../../shared/services/modal.service';
import { Router } from '@angular/router';
import { DetalheSolicitacaoComponent } from '../detalhe-solicitacao.component';

@Component({
	selector: 'app-detalhe-solicitacao-grid',
	templateUrl: './grid.component.html'
})

export class GridDetalheSolicitacaoComponent {

	servico = 'SolicitacoesAlteracaoDetalhe';
	ocultarTituloGrid = true;

	constructor(
		private DetalheSolicitacaoComponent : DetalheSolicitacaoComponent,
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

	excluir(item){
		this.modal.confirmacao("Deseja realmente excluir este detalhe da solicitação?", '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.deletar(item.id);
				}
		});
	}

	deletar(id : number){		
		this.applicationService.delete(this.servico, id).subscribe((result:boolean) => {
			result ?
				this.DetalheSolicitacaoComponent.listar() : 
						'';
			
		});
	}

}
