import { Component, ViewChild, Output, EventEmitter } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { PagedItems } from '../../../view-model/PagedItems';
import { Router } from '@angular/router';

@Component({
	selector: 'app-modal-empresa-pdi',
	templateUrl: './modal-empresa-pdi.component.html',
})

export class ModalEmpresaPDIComponent {
    
	isDisplay: boolean = false;
	servicoEmpresaPDI = 'EmpresaPDIGrid';
	parametros: any = {};
	grid: any = { sort: {} };
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	ordenarGrid: boolean = true;
	isBuscaSalva: boolean = false;
	trouxedados: boolean = false;



	@ViewChild('appModalEmpresaPDIBackground') appModalEmpresaPDIBackground;
	@ViewChild('appModalEmpresaPDI') appModalEmpresaPDI;

	formPai: any;

    constructor(		
		private validationService: ValidationService,
		private modal: ModalService,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private router: Router,
	) { }

	onChangeSort($event) {
		this.grid.sort = $event;
	}

	onChangeSize($event) {
		this.grid.size = $event;
	}

	onChangePage($event) {
		this.grid.page = $event;
		this.listar();
	}

	public abrir() {
		this.appModalEmpresaPDIBackground.nativeElement.style.display = 'block';
		this.appModalEmpresaPDI.nativeElement.style.display = 'block';
		this.parametros.idTaxaEmpresaAtuacao = 0;
		this.listar();
	}

	public fechar() {
		this.trouxedados = false;
		this.parametros = {};
		this.appModalEmpresaPDIBackground.nativeElement.style.display = 'none';
		this.appModalEmpresaPDI.nativeElement.style.display = 'none';
	}

	public listarEmpresaPDI() {

	}	

	public listar(){		
		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;
		this.parametros.exportarListagem = false;

		this.applicationService.get(this.servicoEmpresaPDI, this.parametros).subscribe((result: PagedItems) => {
						
			this.grid.lista = result.items;
			this.grid.total = result.total;
			this.trouxedados = true;
			this.parametros.servico = this.servicoEmpresaPDI;
			this.parametros.titulo = "EMPRESA PDI"
			this.parametros.width = { 0: { columnWidth: 180 }, 1: { columnWidth: 540 }};
			this.parametros.columns = ["CNPJ", "Razão Social"] ;
			this.parametros.fields = ["cnpj", "razaoSocial"];

			if (result.total > 0) {
				this.parametros.exportarListagem = true;
			}

		});
	
	}


}

