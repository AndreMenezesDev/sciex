import { Component, ViewChild, Input, EventEmitter, Output,  } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { Location } from '@angular/common';

@Component({

	selector: 'app-consultar-protocolo-envio-plano-erros',
	templateUrl: './consultar-protocolo-envio-plano-erros.component.html',

})

export class ManterConsultarProtocoloEnvioPlanoErrosFormularioComponent {

	path: string;
	tituloPanel: string;
	parametros: any = {};
	modelEstrutura: any = {};
	servicoErroProcessamentoPEGrid = 'ErroProcessamentoPEGrid';
	grid: any = { sort: {} };


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

	ngOnInit(): void {
			this.modelEstrutura = {};
			this.parametros = {};
			this.parametros.servico = this.servicoErroProcessamentoPEGrid;
			this.parametros.titulo = "CONSULTAR PROTOCOLO DE ENVIO"
			//this.parametros.width = {
			//	0: { columnWidth: 100 }, 1: { columnWidth: 80 }, 2: { columnWidth: 280 }, 3: { columnWidth: 83 }, 4: { columnWidth: 70 }, 5: { columnWidth: 200 } };
			this.parametros.columns = ["Local do Erro", "Mensagem de Erro", "Origem do Erro"];
		this.parametros.fields = ["localErro", "descricao", "origemErro"];

	}

	onChangeSort($event) {

		this.grid.sort = $event;

	}

	onChangeSize($event) {

		this.grid.size = $event;

	}

	onChangePage($event) {

		this.grid.page = $event;
		this.listarErros((this.route.snapshot.params['id']),(this.route.snapshot.params['loteId']));

	}

	public verificarRota() {

		this.tituloPanel = 'Consultar Protocolo de Envio';

		if (this.path == 'visualizar-erros') {

			this.tituloPanel = 'Consultar Protocolo de Envio';
			this.selecionar((this.route.snapshot.params['id']),(this.route.snapshot.params['loteId']));

		}
	}

	public selecionar(idEstruturaPropria:number ,id: number) {

		if (id == undefined || id == 0) { return; }
			
		this.listarErros(idEstruturaPropria, id);

	}

	listarErros(idEstruturaPropria, id) {

		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;
		this.parametros.idEstruturaPropria = idEstruturaPropria;
		this.parametros.idLote = id;		
		this.parametros.exportarListagem = false;

		this.applicationService.get(this.servicoErroProcessamentoPEGrid, this.parametros).subscribe((result: any) => {

			this.grid.lista = result.listaErroProcessamentoPaginada.items;
			this.grid.total = result.listaErroProcessamentoPaginada.total;

			this.modelEstrutura = result.estruturaPropriaPE;
			
			if (result.total > 0) {

				this.parametros.exportarListagem = true;

			}

		});

	}

	voltar() {
		this._location.back();
	}

}
