import { Component, ViewChild, OnInit } from '@angular/core';
import { ValidationService } from '../../../../shared/services/validation.service';
import { ModalService } from '../../../../shared/services/modal.service';
import { MessagesService } from '../../../../shared/services/messages.service';
import { ApplicationService } from '../../../../shared/services/application.service';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../../shared/guards/auth-guard.service';
import { ToastrService } from 'toastr-ng2/toastr';
import { AuthenticationService } from '../../../../shared/services/authentication.service';
import { PagedItems } from '../../../../view-model/PagedItems';


@Component({
	selector: 'app-modal-descricao-observacao-suframa',
	templateUrl: 'modal-descricao-observacao-suframa.component.html',
})

export class ModalDescricaoObservacaoSuframaComponent implements OnInit {
	formPai = this;
	model: any;
	servicoGrid = 'descricaoObservacaoSuframa';
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = true;
	grid: any = { sort: {} };

	@ViewChild('appModalDescricaoObservacaoSuframa') appModalDescricaoObservacaoSuframa;
	@ViewChild('appModalDescricaoObservacaoSuframaBackground') appModalDescricaoObservacaoSuframaBackground;

	@ViewChild('btnlimpar') btnlimpar;



	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private applicationService: ApplicationService,
		private toastrService: ToastrService,
		private router: Router,
		private authguard: AuthGuard,
		private authenticationService: AuthenticationService,
	) {

	}

	ngOnInit() {
		this.model = {};
		this.parametros = {};
	}

	public abrir(item) {
		this.model = item;
		this.parametros = {};
		this.grid = {};
		this.appModalDescricaoObservacaoSuframa.nativeElement.style.display = 'block';
		this.appModalDescricaoObservacaoSuframaBackground.nativeElement.style.display = 'block';
	}

	ocultar() {
		if (this.ocultarFiltro === false)
			this.ocultarFiltro = true;
		else
			this.ocultarFiltro = false;
	}

	public fechar() {
		this.appModalDescricaoObservacaoSuframa.nativeElement.style.display = 'none';
		this.appModalDescricaoObservacaoSuframaBackground.nativeElement.style.display = 'none';
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

	buscar() {
		this.listar(this.model);
	}

	listar(item) {
		this.parametros.anoProcesso = item.anoProcesso;
		this.parametros.numeroProcesso = item.numeroProcesso;

		this.ocultarGrid = true;
		this.parametros.exportarListagem = false;
		this.applicationService.get(this.servicoGrid, this.parametros).subscribe((result: PagedItems) => {
			this.grid.lista = result.items;
			this.grid.total = result.total;

			if (result.total > 0){
				this.ocultarGrid = false;
			}else{
				this.ocultarGrid = true;
			}
		});
	}
}
