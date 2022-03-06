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
	selector: 'app-consultar-formulario-propriedade-produto-comprovacao-suframa',
	templateUrl: './formulario-propriedade-produto-comprovacao-suframa.component.html'
})

export class ConsultarFormularioPropriedadeProdutoComprovacaoSuframaComponent implements OnInit {
	path: string;
	servico = "ProcessoProduto";
	modelProduto: any = {};
	modelProcesso: any = {};
	parametros: any = {};
	listaPais = [];
	totalpais: number = 0;
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
		this.idProduto = this.route.snapshot.params['idProduto'];
	}

	ngOnInit() {
		this.selecionarProduto(this.idProduto);
		this.modelProduto = {};
		this.listaPais = [];
	}

	changeSort($event) {
		this.sorted = $event.field;
		this.changePage(this.page);
	}

	changePage($event) {
		this.page = $event;
	}
	public selecionarProduto(id: number) {
		if (!id) { return; }
		this.applicationService.get(this.servico, id).subscribe((result: any) => {
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
		var objeto : any = {};
		objeto.idPRCProduto = Number(this.idProduto);
		objeto.sort = this.grid.sort;
		objeto.size = this.grid.size;
		objeto.page = this.grid.page;

		this.applicationService.get(this.servicoDocumentosComprobatorios,objeto).subscribe((result :any)=>{
			console.log(result)

			this.grid.lista = result.items;

			this.grid.total = result.total;

		})
	}

	onChangeSort($event) {
		this.grid.sort = $event;

	}

	onChangeSize($event) {
		this.grid.size = $event;
	}

	onChangePage($event) {
		this.grid.page = $event;
		this.documentosComprobatorios();
	}
}