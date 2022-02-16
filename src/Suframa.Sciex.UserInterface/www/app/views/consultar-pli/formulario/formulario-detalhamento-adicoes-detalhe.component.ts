import { Component, ViewChild, Input, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { manterPliMercadoriaVM } from '../../../view-model/ManterPliMercadoriaVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { Location } from '@angular/common';

@Component({

	selector: 'app-detalhamento-adicoes-detalhe-formulario',
	templateUrl: './formulario-detalhamento-adicoes-detalhe.component.html',

})

export class ManterDetalhamentoAdicoesDetalheFormularioComponent {

	path: string;
	titulo: string;
	tituloPanel: string;
	model: any = {};
	modelDiLi: any = {};
	servico = 'Di';
	servicoDiLi = 'DiLi'
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
		this.tituloPanel = 'Descrição da Adição';
		if (this.path == 'visualizar-detalhamento-adicoes-detalhe') {

			this.tituloPanel = 'Descrição da Adição';
			this.selecionar(this.route.snapshot.params['id']);
			this.selecionaDiLi(this.route.snapshot.params['iddi']);
			this.modelDiLi.fundamentoLegal = {};
			this.modelDiLi.viaTransporte = {};
		}
	}

	public selecionar(id) {
		this.applicationService.get<manterPliVM>(this.servico, id).subscribe(result => {
			this.model = result;
		});
	}

	public selecionaDiLi(id) {
		this.applicationService.get(this.servicoDiLi, id).subscribe(result => {
			this.modelDiLi = result;
		});
	}

	voltar() {
		this._location.back();
	}
}
