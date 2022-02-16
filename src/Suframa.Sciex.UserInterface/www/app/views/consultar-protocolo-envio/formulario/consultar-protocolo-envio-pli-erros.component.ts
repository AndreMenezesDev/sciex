import { Component, ViewChild, Input, EventEmitter, Output,  } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { SolicitacaoPliVM } from '../../../view-model/SolicitacaoPLIVM';
import { PagedItems } from '../../../view-model/PagedItems';
import { Location } from '@angular/common';

@Component({

	selector: 'app-consultar-protocolo-envio-pli-erros',
	templateUrl: './consultar-protocolo-envio-pli-erros.component.html',

})

export class ManterConsultarProtocoloEnvioPliErrosFormularioComponent {

	path: string;
	tituloPanel: string;
	parametros: any = {};	
	model1: SolicitacaoPliVM = new SolicitacaoPliVM();
	servicoErroProcessamentoGrid = 'ErroProcessamentoGrid';
	grid: any = { sort: {} };
	cnpjEmpresa: string;
	cnpjRazaoSocial: string;
	nPliImportador: string;


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

			this.parametros = {};
			this.parametros.servico = this.servicoErroProcessamentoGrid;
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
		this.listarErros(this.parametros.IdSolicitacaoPli);

	}

	public verificarRota() {

		this.tituloPanel = 'Consultar Protocolo de Envio';

		if (this.path == 'visualizar-erros') {

			this.tituloPanel = 'Consultar Protocolo de Envio';
			this.model1 = JSON.parse(localStorage.getItem("SolicitacaoPli"));
			this.selecionar((this.route.snapshot.params['idsolicitacaopli']));

		}
	}

	public selecionar(idsolicitacao: number) {

		if (idsolicitacao == undefined || idsolicitacao == 0) { return; }
			
		this.listarErros(idsolicitacao);

	}

	listarErros(id) {

		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;
		this.parametros.IdSolicitacaoPli = id;		
		this.parametros.exportarListagem = false;
		if (this.model1 != null && this.model1 != undefined) {
			this.parametros.numeroPliImportador = this.model1.numeroPliImportador;
		}

		if (id == undefined || id == null) {

			this.parametros.IdSolicitacaoPli = this.route.snapshot.params['idsolicitacaopli'];

		}

		this.applicationService.get(this.servicoErroProcessamentoGrid, this.parametros).subscribe((result: PagedItems) => {

			this.grid.lista = result.items;
			this.grid.total = result.total;
			
			if (result.total > 0) {

				this.parametros.exportarListagem = true;

			}

		});

	}

	voltar() {
		this._location.back();
	}
}
