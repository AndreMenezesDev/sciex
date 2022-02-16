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
import { ThrowStmt } from '@angular/compiler';


@Component({
	selector: 'app-modal-historico-insumos-importados-suframa',
	templateUrl: './modal-historico-insumos-importados-suframa.component.html',
})

export class ModalHistoricoInsumosImportadosComponent{
	formPai = this;
	model: any;
	servicoGrid = 'ParecerTecnicoSuframa';
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = true;
	grid: any = { sort: {} };
	@ViewChild('appModalHistoricoInsumos') appModalHistoricoInsumos;
	@ViewChild('appModalHistoricoInsumosBackground') appModalHistoricoInsumosBackground;

	@ViewChild('btnlimpar') btnlimpar;
	@ViewChild('relatorio') relatorio;



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

	public abrir(item) {
		this.model = item;
		this.parametros = {};
		this.grid = {};
		this.appModalHistoricoInsumos.nativeElement.style.display = 'block';
		this.appModalHistoricoInsumosBackground.nativeElement.style.display = 'block';
	}

	limpar() {
		this.parametros.tipoStatus = undefined;
		this.parametros.numeroControleString = null;
		this.grid.lista = null;
		this.grid.total = 0;
	}

	ocultar() {
		if (this.ocultarFiltro === false)
			this.ocultarFiltro = true;
		else
			this.ocultarFiltro = false;
	}

	public fechar() {
		this.appModalHistoricoInsumos.nativeElement.style.display = 'none';
		this.appModalHistoricoInsumosBackground.nativeElement.style.display = 'none';
		this.limpar();
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

	emitirHistoricoInsumo(isInsumo){
		let id;
		if(isInsumo){
           id = this.model.idInsumo;
		}else{
           id = this.model.idPrcProduto;
		}
		this.relatorio.emitirHistoricoInsumo(id, isInsumo);
	}
}
