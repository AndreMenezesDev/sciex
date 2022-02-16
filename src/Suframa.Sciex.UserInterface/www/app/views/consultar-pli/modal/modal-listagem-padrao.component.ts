import { Component, ViewChild, Output, EventEmitter} from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';

@Component({
	selector: 'app-modal-listagem-padrao',
	templateUrl: './modal-listagem-padrao.component.html',
})

export class ModalListagemPadraoComponent {
	grid: any = { sort: {} };
	parametros: any = {};	
	servico = 'ConsultarListagemPadrao';
	textoTitulo: string;	
	retorno: any[];

	@ViewChild('appModalListagemPadrao') appModalListagemPadrao;
	@ViewChild('appModalListagemPadraoBackground') appModalListagemPadraoBackground;

	@Output() objetoInfo = new EventEmitter();
	
	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService
	) { }

	onChangeSort($event) {
		this.grid.sort = $event;
	}

	changeSize($event) {
		this.grid.size = $event;
	}

	changePage($event) {
		this.grid.page = $event;
		this.listar();
	}

	public abrir(form) {
		this.parametros = {};
		this.appModalListagemPadrao.nativeElement.style.display = 'block';
		this.appModalListagemPadraoBackground.nativeElement.style.display = 'block';
		
		this.parametros.titulo = "Buscar Listagem Padrão";

		this.parametros.codigoNCMMercadoria = form.codigoNCMMercadoria;

		this.listar();		
	}

	listar() {
		
        this.parametros.page = this.grid.page;
        this.parametros.size = this.grid.size;
        this.parametros.sort = this.grid.sort.field;
        this.parametros.reverse = this.grid.sort.reverse;

		this.applicationService.get(this.servico, this.parametros).subscribe((result: PagedItems) => {
			if(result){				
				this.grid.lista = result.items;
				this.grid.total = result.total;	
			}		
		});
	}

	selectionarInsumo(descricao) {
		//this.appModalBuscarInsumo.nativeElement.style.display = 'none';
		//this.appModalBuscarInsumoBackground.nativeElement.style.display = 'none';	
		//this.objetoInfo.emit(descricao);		
	}

	public fechar() {
		this.parametros = {};
		this.appModalListagemPadrao.nativeElement.style.display = 'none';
		this.appModalListagemPadraoBackground.nativeElement.style.display = 'none';		
	}
}

