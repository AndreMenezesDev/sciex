import { Component, Injectable, OnInit, ViewChild } from "@angular/core";
import { THROW_IF_NOT_FOUND } from "@angular/core/src/di/injector";
import { Router } from "@angular/router";
import { ApplicationService } from "../../shared/services/application.service";
import { MessagesService } from "../../shared/services/messages.service";
import { ModalService } from "../../shared/services/modal.service";
import { PagedItems } from "../../view-model/PagedItems";

@Component({
	selector: 'app-manter-via-transporte',
	templateUrl: './manter-via-transporte.component.html'
})

@Injectable()
export class ManterViaTransporteComponent implements OnInit {

	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	servicoViaTransporteGrid = 'ViaTransporteGrid';

	@ViewChild('codigo') codigo;
	@ViewChild('descricao') descricao;
	@ViewChild('ativo') ativo;
	@ViewChild('inativo') inativo;
	@ViewChild('todos') todos;
	
    constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router
	) {

    }
    
    ngOnInit(): void{
		if (this.parametros != undefined || this.parametros != null) {
			this.grid.page = this.parametros.page;
			this.grid.size = this.parametros.size;
			this.grid.sort.field = this.parametros.sort;
			this.grid.sort.reverse = this.parametros.reverse;

			this.listar();
		}
    }

	buscar() {
		this.listar();
	}

	ocultar() {
		if (this.ocultarFiltro === false)
			this.ocultarFiltro = true;
		else
			this.ocultarFiltro = false;
	}
	onChangeSort($event) {
		this.grid.sort = $event;
	}

	onChangeSize($event) {
		this.grid.size = $event;
	}

	onChangePage($event) {
		this.grid.page = $event;
		this.buscar();
	}

	limpar() {
		this.descricao.nativeElement.value = "";
		this.codigo.nativeElement.value = "";
		this.ativo.nativeElement.value ="";
		this.inativo.nativeElement.value = "";
		this.todos.nativeElement.checked = "checked";
	}

	listar() 
	{
		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;

		this.parametros.codigo = this.codigo.nativeElement.value;
		this.parametros.descricao = this.descricao.nativeElement.value;

		if (this.ativo.nativeElement.checked == false && this.inativo.nativeElement.checked == false) {
			this.parametros.status = 2;
		} else if (this.ativo.nativeElement.checked == true && this.inativo.nativeElement.checked == false) {
			this.parametros.status = 1;
		} else {
			this.parametros.status = 0;
		}
		
		this.applicationService.get(this.servicoViaTransporteGrid, this.parametros).subscribe((result: PagedItems) => {			
			if (result.total > 0) {
				this.parametros.exportarListagem = true;
				this.parametros.servico = this.servicoViaTransporteGrid;
				this.parametros.titulo = "MANTER VIA DE TRANSPORTE";
				this.parametros.width = { 0: { columnWidth: 80 }, 1: { columnWidth: 600 }, 2: { columnWidth: 80 }  };
				this.parametros.columns = ["Código", "Descrição", "Status"];
				this.parametros.fields  = ["codigo", "descricao", "status"];
			}else{
				this.parametros.exportarListagem = false;
			}

			this.grid.lista = result.items;
			this.grid.total = result.total;
		});
	}

}