import { Component, ViewChild, Input, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { manterPliMercadoriaVM } from '../../../view-model/ManterPliMercadoriaVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { Location } from '@angular/common';

@Component({

	selector: 'app-consultar-pli-mercadorias-descricao-formulario',
	templateUrl: './formulario-mercadorias-descricao.component.html',

})

export class ManterConsultarPliMercadoriasDescricaoFormularioComponent {

	path: string;
	titulo: string;
	tituloPanel: string;
	model: manterPliVM = new manterPliVM();
	modelMercadoria: manterPliMercadoriaVM = new manterPliMercadoriaVM();
	servico = 'Pli';
	servicoMercadoria = 'PliMercadoria'
	servicoGrid = 'PliGrid';
	notComercializacao: boolean = true;

	constructor(

		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private arrayService: ArrayService,
		private modal: ModalService,
		private validationService: ValidationService,
		private router: Router,
		private _location: Location
	) {

		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.verificarRota();

	}

	public verificarRota() {
		this.tituloPanel = 'Detalhamento do PLI';
		if (this.path == 'visualizar-mercadoria-descricao') {

			this.tituloPanel = 'Detalhamento do PLI';
			this.selecionar(this.route.snapshot.params['id']);
			this.selecionarMercadoria(this.route.snapshot.params['idmercadoria']);
		}
	}

	public selecionar(id) {
		this.applicationService.get<manterPliVM>(this.servico, id).subscribe(result => {
			this.model = result;
			this.notComercializacao = this.model.idPLIAplicacao != 1 ? true : false;
		});
	}

	public selecionarMercadoria(idMercadoria) {
		this.applicationService.get<manterPliMercadoriaVM>(this.servicoMercadoria, idMercadoria).subscribe(result => {
			this.modelMercadoria = result;
		});
	}

	voltar() {
		this._location.back();
	}
}
