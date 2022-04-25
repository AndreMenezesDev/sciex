import { Component, OnInit, Injectable, ViewChild, Input } from '@angular/core';
import { PagedItems } from '../../../view-model/PagedItems';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../shared/guards/auth-guard.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { Location } from '@angular/common';
import { RelatorioListagemHistoricoInsumosVM } from '../../../view-model/RelatorioListagemHistoricoInsumosVM';

@Component({
	selector: 'app-formulatorio-relatorio-lista-insumos',
	templateUrl: './formulatorio-relatorio.component.html'
})

@Injectable()
export class FormularioRelatorioListaInsumosComponent implements OnInit {

	@Input() objetoParametro : Array<RelatorioListagemHistoricoInsumosVM> = new Array<RelatorioListagemHistoricoInsumosVM>();

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private validationService: ValidationService,
		private msg: MessagesService,
		private router: Router,
		private Location: Location,
		private authguard: AuthGuard
	) {}

	ngOnInit(): void {}

}
