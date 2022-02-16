import { Component, ViewChild, Input, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import {Location} from '@angular/common';

@Component({

	selector: 'app-consultar-pli-formulario',
	templateUrl: './formulario-cancelar-consultar-pli.component.html',
})

export class CancelaLIFormularioPLIFormularioComponent {

	path: string;
	titulo: string;
	tituloPanel: string;
	id: number;
	model: manterPliVM = new manterPliVM();
	servico = 'Pli';
	servicoGrid = 'PliGrid';
	rotaRecebida: any; 

	telaAtual: number;

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
		this.verificarRota();
	}

	ngOnInit(){
		console.log('a rota recebida por parametro é: ' + this.rota);
	}

	public ReceberRota(RotaPai){
		this.rotaRecebida = RotaPai; 
	}

	public verificarRota() {
		this.tituloPanel = 'Detalhamento do PLI';
		if (this.path == 'visualizar') {
			this.tituloPanel = 'Detalhamento do PLI';
			this.selecionar(this.route.snapshot.params['id']);
			this.telaAtual = this.route.snapshot.params['tela'];
		}
		else if (this.path == 'visualizar-detalhamento-pli') {
			this.tituloPanel = 'Detalhamento do PLI';
			this.selecionar(this.route.snapshot.params['id']);
			this.telaAtual = this.route.snapshot.params['tela'];
		}
	}

	public selecionar(id: number) {
		if (!id) { return; }
		this.applicationService.get<manterPliVM>(this.servico, id).subscribe(result => {
			this.model = result;
		});

	}

	voltar(){
		//this.router.navigate([this.rota]);
		this._location.back();
    }

}
