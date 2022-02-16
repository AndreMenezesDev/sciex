import { Component, Input, Output, EventEmitter} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import {Location} from '@angular/common';

@Component({

	selector: 'app-consultar-di-formulario',
	templateUrl: './formulario-detalhamento-di.component.html',
})

export class ManterConsultarDiFormularioComponent {

	path: string;
	titulo: string;
	tituloPanel: string;
	id: number;
	model: manterPliVM = new manterPliVM();
	servico = 'Di';
	rotaRecebida: any;
	rotaVoltar: string;

	totalArmazems: number;
	totalEmbalagems: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() page: number;
	@Input() exportarListagem: boolean = false;
	@Input() parametros: any = {};
	@Input() isShowPanel: boolean = false;
	@Input() somenteLeitura: boolean = false;

	@Input() rota: any;

	constructor(
		private modal: ModalService,
		private router: Router,
		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private _location: Location
	){
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.id = this.route.snapshot.params['id'];
		this.model.di = {};
		this.parametros.exportarListagem = false;
		this.verificarRota();
		
	}

	ngOnInit() {
	}

	changeSize($event) {		
	}

	changeSort($event) {
		this.sorted = $event.field;
		this.changePage(this.page);
	}

	changePage($event) {
		this.page = $event;
	}


	public ReceberRota(RotaPai){
		this.rotaRecebida = RotaPai;
	}

	public verificarRota() {
		this.tituloPanel = 'Detalhamento da DI';
		if (this.path == 'visualizar-detalhamento-di') {
			this.tituloPanel = 'Detalhamento da DI';
			this.selecionar(this.route.snapshot.params['id']);
		}
	}

	public selecionar(id: number) {
		if (!id) { return; }
		this.applicationService.get<manterPliVM>(this.servico, id).subscribe(result => {
			this.model = result;
			this.totalArmazems = result.di.listaArmazems.length;
			this.totalEmbalagems = result.di.listaEmbalagems.length;
		});

	}

	voltar(){
		this._location.back();
    }

}
