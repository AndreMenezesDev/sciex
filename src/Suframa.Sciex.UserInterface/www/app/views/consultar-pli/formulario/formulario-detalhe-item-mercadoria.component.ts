import { Component, ViewChild, Input, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { Location } from '@angular/common';
import { manterPliMercadoriaVM } from '../../../view-model/ManterPliMercadoriaVM';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { manterPliDetalheMercadoriaVM } from '../../../view-model/ManterPliDetalheMercadoriaVM';

@Component({

	selector: 'app-consultar-detalhe-item-mercadoria-formulario',
	templateUrl: './formulario-detalhe-item-mercadoria.component.html',
})

export class ManterConsultarDetalheItemMercadoriaFormularioComponent {

	path: string;
	titulo: string;
	tituloPanel: string;
	modelDetalheMercadoria: manterPliDetalheMercadoriaVM = new manterPliDetalheMercadoriaVM();
	modelMercadoria: manterPliMercadoriaVM = new manterPliMercadoriaVM();
	modelPli: manterPliVM = new manterPliVM();
	servico = 'Pli';
	servicoMercadoria = 'PliMercadoria'
	servicoDetalheMercadoria = 'PliDetalheMercadoria'
	servicoGrid = 'PliGrid';
	notComercializacao: boolean = true;

	@ViewChild('appModalListagemPadrao') appModalListagemPadrao;

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
		this.tituloPanel = 'Detalhe da Mercadoria item';
		if (this.path == 'visualizar-detalhe-item-mercadoria') {

			this.tituloPanel = 'Detalhamento do PLI';
			this.selecionar(this.route.snapshot.params['id']);
			this.selecionarMercadoria(this.route.snapshot.params['idmercadoria']);
			this.selecionarDetalhe(this.route.snapshot.params['iddetalhe']);
		}
	}

	abrirModalListagemPadrao(){
		this.appModalListagemPadrao.abrir(this.modelMercadoria);
	}

	public selecionar(id: number) {
		if (!id) { return; }
		this.applicationService.get<manterPliVM>(this.servico, id).subscribe(result => {
			this.modelPli = result;
			this.notComercializacao = this.modelPli.idPLIAplicacao != 1 ? true : false;
		});
	}

	public selecionarMercadoria(idMercadoria) {
		this.applicationService.get<manterPliMercadoriaVM>(this.servicoMercadoria, idMercadoria).subscribe(result => {
			this.modelMercadoria = result;
			console.log(this.modelMercadoria);
		});
	}

	public selecionarDetalhe(idDetalhe) {
		this.applicationService.get<manterPliDetalheMercadoriaVM>(this.servicoDetalheMercadoria, idDetalhe).subscribe(result => {
			this.modelDetalheMercadoria = result;
		});
	}

	voltar() {
		this._location.back();
	}
}
