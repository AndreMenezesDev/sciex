import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { PagedItems } from '../../../view-model/PagedItems';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import {Location} from '@angular/common';
declare var $: any;

@Component({
	selector: 'app-consultar-formulario-propriedade-produto-comprovacao',
	templateUrl: './formulario-propriedade-produto-comprovacao.component.html'
})

export class ConsultarFormularioPropriedadeProdutoComprovacaoComponent implements OnInit {
	path: string;
	servico = "ProcessoProduto";
	modelProduto: any = {};
	modelProcesso: any = {};
	parametros: any = {};
	parametrosListaPais: any = {};
	listaPais = [];
	totalpais: number = 0;
	objeto : any = {};
	@Input() sorted: string;
	@Input() page: number;
	idProduto: any;
	servicoDocumentosComprobatorios = "DocumentosComprobatoriosGrid"
	grid: any = { sort: {} };
	formPai = this;

	constructor(
		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private modal: ModalService,
		private router: Router,
		private validationService: ValidationService,
		private Location: Location,
	) {
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.idProduto = this.route.snapshot.params['idProcesso'];
	}

	ngOnInit() {
		this.selecionarProduto(this.idProduto);
		this.objeto.sort = null;
		this.objeto.size = 10;
		this.objeto.page = 1;
		this.modelProduto = {};
		this.listaPais = [];
	}

	changeSortPais($event) {
		this.parametrosListaPais.sort = $event.field;
		this.parametrosListaPais.reverse = $event.reverse;
		this.selecionarProduto(this.idProduto);
	}

	changePage($event) {
		this.page = $event;
	}
	public selecionarProduto(id: number) {
		if (!id) { return; }
		this.parametrosListaPais.idProduto = id;
		this.applicationService.get(this.servico, this.parametrosListaPais).subscribe((result: any) => {
			this.modelProduto = result;
			this.modelProcesso = result.processo;
			this.listaPais = result.listaProdutoPaisPaginada.items;
			this.totalpais = result.listaProdutoPaisPaginada.total;
			this.documentosComprobatorios();
		});

	}

	voltar(){
		let arrayUrl = sessionStorage.getItem("arrayUrl");
		let obj = JSON.parse(arrayUrl)
		let url = obj[obj.length - 2]
		obj.pop()
		sessionStorage.removeItem("arrayUrl")
		sessionStorage.setItem("arrayUrl", JSON.stringify(obj))

		this.router.navigate([url]);
	}

	documentosComprobatorios(){
		this.objeto.idPRCProduto = Number(this.idProduto);

		this.applicationService.get(this.servicoDocumentosComprobatorios, this.objeto).subscribe((result :any)=>{
			console.log(result)

			this.grid.lista = result.items;

			this.grid.total = result.total;

		})
	}

	onChangeSort($event) {
		this.objeto.sort = $event.field;
		this.objeto.reverse = $event.reverse;
	}

	onChangeSize($event) {
		this.grid.size = $event;
	}

	onChangePage($event) {
		this.grid.page = $event;
		this.documentosComprobatorios();
	}

}
