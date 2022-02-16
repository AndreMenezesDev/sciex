import { Component, Input, ViewChild} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import {Location} from '@angular/common';
import { PagedItems } from '../../../view-model/PagedItems';

@Component({

	selector: 'app-manter-insumo-formulario',
	templateUrl: './formularioInsumo.component.html',
})

export class ManterLEInsumoFormularioComponent {

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
	isLEBloq: boolean = false;

	@Input() rota: any;
	
	@ViewChild('codigoNCM') codigoNCM;
	@ViewChild('appModalNovoInsumo') appModalNovoInsumo;
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

	public ReceberRota(RotaPai){
		this.rotaRecebida = RotaPai; 
	}

	public verificarRota() {
		if (this.path == 'visualizar') {
			this.titulo = 'Visualizar LE';
			this.subtitulo = 'Visualizar LE';
			this.somenteLeitura = true;
			this.selecionar(this.route.snapshot.params['id']);
		}
		else if (this.path == 'editar') {
			this.titulo = 'Manter Insumo';
			this.subtitulo = 'Cadastrar Insumo';
			this.somenteLeitura = false;
			this.selecionar(this.route.snapshot.params['id']);
		}
		else if (this.path == 'corrigir') {
			this.titulo = 'Manter Insumo';
			this.subtitulo = 'Corrigir Insumo';
			this.somenteLeitura = false;
			this.isLEBloq = true;
			this.selecionar(this.route.snapshot.params['id']);
		}
	}

	public selecionar(id: number) {
		if (!id) { return; }
		console.log(id);
		this.applicationService.get(this.servico, id).subscribe((result: any) => {
			this.model = result;
			this.codigoProdutoSuframa = result.descCodigoProdutoSuframa
			this.parametros.statusLEAlteracao = result.statusLEAlteracao;
			this.parametros.statusLE = result.statusLE;
			this.buscar();
		});
	}

	buscar(){
		this.parametros.descricaoCodigoProdutoSuframa = this.codigoProdutoSuframa
		this.parametros.idLe = this.model.idLe;
		if(this.parametros.idCodigoNCM == null)
			this.parametros.codigoNCM = null;
		
		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;

		this.parametros.titulo = "Insumo no Produto"
		
		this.parametros.servico = this.servicoGrid;

		// if(!this.parametros.tipoInsumo || this.parametros.tipoInsumo == undefined)
		// 	this.parametros.tipoInsumo = "99";

		if(!this.parametros.situacaoInsumo)
			this.parametros.situacaoInsumo = 99;

		if(this.isLEBloq){
			this.parametros.width = { 0: { columnWidth: 60 }, 1: { columnWidth: 60 }, 2: { columnWidth: 80 }, 3: { columnWidth: 300 }, 4: { columnWidth: 80 } , 5: { columnWidth: 80 } , 6: { columnWidth: 80 }};
			this.parametros.columns = ["Código Insumo", "Tipo","NCM", "Descrição do Insumo", "Coeficiente Técnico", "Situação", "Tipo Alteração"];
			this.parametros.fields = ["codigoInsumo", "descricaoTipoInsumo", "codigoNCMFormatado", "descricaoInsumo", "valorCoeficienteTecnico", "descricaoSituacaoInsumo", "descricaoTipoInsumoAlteracao"];
		}
		else{
			this.parametros.width = { 0: { columnWidth: 60 }, 1: { columnWidth: 60 }, 2: { columnWidth: 80 }, 3: { columnWidth: 300 }, 4: { columnWidth: 80 } };
			this.parametros.columns = ["Código Insumo", "Tipo","NCM", "Descrição do Insumo", "Coeficiente Técnico"];
			this.parametros.fields = ["codigoInsumo", "descricaoTipoInsumo", "codigoNCMFormatado", "descricaoInsumo", "valorCoeficienteTecnico"];
		}
		this.parametros.exportarListagem = true;

		// console.log(JSON.stringify(this.parametros));
		try {
			this.applicationService.get(this.servicoGrid, this.parametros).subscribe((result: PagedItems) => {
				this.grid.lista = result.items;
				this.grid.total = result.total;
			});
		} catch (err) {
			console.log(JSON.stringify(err));
		}
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
		this.parametros.idCodigoNCM = null
		this.parametros.tipoInsumo == undefined

	}

	visualizar(item){
		if(item != null)
			this.appModalNovoInsumo.visualizar(this, item);
	}

	alterar(item){
		if(item != null)
			this.appModalNovoInsumo.alterar(this, item);
	}

	alterarBloqueado(item){
		if(item != null)
			this.appModalNovoInsumo.alterarBloqueado(this, item);
	}

	cadastrar() {
		this.appModalNovoInsumo.cadastrar(this);
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
