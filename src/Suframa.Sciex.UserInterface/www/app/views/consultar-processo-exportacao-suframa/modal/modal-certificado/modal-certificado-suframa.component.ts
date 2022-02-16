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
	selector: 'app-modal-certificado-suframa',
	templateUrl: './modal-certificado-suframa.component.html',
})

export class ModalCertificadoSuframaComponent implements OnInit {
	formPai = this;
	model: any;
	servicoGrid = 'Certificado';
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = true;
	grid: any = { sort: {} };
	@ViewChild('appModalCertificado') appModalCertificado;
	@ViewChild('appModalCertificadoBackground') appModalCertificadoBackground;

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
		this.appModalCertificado.nativeElement.style.display = 'block';
		this.appModalCertificadoBackground.nativeElement.style.display = 'block';
	}

	limpar() {
		this.parametros.tipoCertificado = undefined;
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
		this.appModalCertificado.nativeElement.style.display = 'none';
		this.appModalCertificadoBackground.nativeElement.style.display = 'none';
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
		this.parametros.idProcesso = item.idProcesso;
		

		this.ocultarGrid = true;
		
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
