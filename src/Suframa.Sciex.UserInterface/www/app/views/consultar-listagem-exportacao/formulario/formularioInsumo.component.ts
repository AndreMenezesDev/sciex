import { Component, Input, ViewChild} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import {Location} from '@angular/common';
import { PagedItems } from '../../../view-model/PagedItems';

@Component({

	selector: 'app-consultar-insumo-formulario',
	templateUrl: './formularioInsumo.component.html',
})

export class ConsultarLEInsumoFormularioComponent {

	path: string;
	titulo: string;
	subtitulo: string;
	id: number;
	model: any = {};
	parametros: any = {};
	servico = 'LEProduto';
	servicoGrid = 'LEInsumoGrid';
	rotaRecebida: any;
	rotaVoltar: string;
	ocultarFiltro: boolean = false;
	grid: any = { sort: {} };
	somenteLeitura: boolean = false;
	desabilitarBotaoFinalizar: boolean = true;

	@Input() rota: any;
	
	@ViewChild('codigoNCM') codigoNCM;
	@ViewChild('appModalConsultarInsumo') appModalConsultarInsumo;
	@ViewChild('codigoNCM2') codigoNCM2;
	servicoFinalizarAnalise = "LEAnaliseProduto";
	codigoProdutoSuframa: any;
	

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

	ngOnInit() {

	}
	public verificarRota() {
		this.titulo = 'Consultar Insumos da LE';
		this.subtitulo = 'Consultar LE';
		this.somenteLeitura = false;
		this.parametros.codigoNCM2 = null;
		this.selecionar(this.route.snapshot.params['id']);
	}

	public selecionar(id: number) {
		if (!id) { return; }
		this.applicationService.get(this.servico, id).subscribe((result:any) => {
			this.model = result;
			this.codigoProdutoSuframa = result.descCodigoProdutoSuframa
			this.buscar();
		});
	}

	buscar(){
		this.parametros.descricaoCodigoProdutoSuframa = this.codigoProdutoSuframa

		if(!this.parametros.situacaoInsumo)
			this.parametros.situacaoInsumo = 99;

		if(!this.parametros.tipoInsumo)
			this.parametros.tipoInsumo = "99";
		
		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;

		this.parametros.idLe = this.model.idLe;
		this.parametros.idCodigoNCM = this.model.idCodigoNCM;
		
		if(this.parametros.idCodigoNCM == null)
			this.parametros.codigoNCM = null;

		this.applicationService.get(this.servicoGrid, this.parametros).subscribe((result: PagedItems) => {
			this.grid.lista = result.items;
			this.grid.total = result.total;
			let existePendente = false;
			this.grid.lista.forEach(element => {
				if (element.situacaoInsumo == null){
					existePendente = true;
				}
			});

			this.desabilitarBotaoFinalizar = existePendente;
		});
	}

	public selecionaNCM(event){
		if(event != null){
			let cod = event.text.split("|")[0].trim();
			this.parametros.codigoNCM = cod;
		}
	}

	limpar() {
		if(this.codigoNCM != null)
			this.codigoNCM.clear();
		this.parametros = {};
		this.codigoNCM2.onClear(true);
	}

	visualizar(item){
		if(item != null)
			this.appModalConsultarInsumo.visualizar(this, item);
	}

	ocultar() {
		if (this.ocultarFiltro === false)
			this.ocultarFiltro = true;
		else
			this.ocultarFiltro = false;
	}

	voltar(){
		this._location.back();
	}
	
	onChangeSort($event) {
		this.grid.sort = $event;

	}

	onChangeSize($event) {
		this.grid.size = $event;
	}

	onChangePage($event) {
		this.grid.page = $event;
		this.buscar();
	}
}
