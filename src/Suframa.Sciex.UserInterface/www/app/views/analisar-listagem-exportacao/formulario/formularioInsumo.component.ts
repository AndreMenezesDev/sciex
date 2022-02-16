import { Component, Input, ViewChild} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import {Location} from '@angular/common';
import { PagedItems } from '../../../view-model/PagedItems';

@Component({

	selector: 'app-analisar-insumo-formulario',
	templateUrl: './formularioInsumo.component.html',
})

export class AnalisarLEInsumoFormularioComponent {

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

	@ViewChild('codigoNCM2') codigoNCM2;
	@ViewChild('codigoNCM') codigoNCM;
	@ViewChild('appModalAnalisarInsumo') appModalAnalisarInsumo;
	servicoFinalizarAnalise = "LEAnaliseProduto";
	servicoAprovarAnalise = "AprovarAnaliseLEProdutoInsumo";
	isAlteracao: boolean = false;
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
		else if (this.path == 'analisar') {
			this.titulo = 'Analisar Insumos';
			this.subtitulo = 'Analisar LE';
			this.somenteLeitura = false;
			this.selecionar(this.route.snapshot.params['id']);
		}
	}

	public finalizarAnalise(isAprovarAnalise:boolean) {

		this.parametros.isAlteracao = this.isAlteracao;
		this.parametros.isAprovarAnalise = isAprovarAnalise;
		this.applicationService.put(this.servicoAprovarAnalise, this.parametros).subscribe((result: any) => {
			if (result.mensagemErro != null) {
				this.modal.alerta(result.mensagemErro, "Informação", "");
				return;
			}
			else{
				this.modal.resposta(result.mensagem, "Informação", "");
				this.router.navigate(['analisar-listagem-exportacao'])
			}
		});
	}

	public selecionar(id: number) {
		if (!id) { return; }
		this.applicationService.get(this.servico, id).subscribe((result:any) => {
			this.model = result;
			this.codigoProdutoSuframa = result.descCodigoProdutoSuframa
			this.isAlteracao = this.parametros.isAlteracao = result.statusLEAlteracao == 3 ? true : false;
			this.buscarInsumos();
		});
	}

	buscarInsumos(){
		this.parametros.isAlteracao = this.isAlteracao;
		this.parametros.descricaoCodigoProdutoSuframa = this.codigoProdutoSuframa

		if(!this.parametros.codigoInsumo)
			this.parametros.codigoInsumo = null;

		if(!this.parametros.situacaoInsumo)
			this.parametros.situacaoInsumo = 99;

		if(!this.parametros.tipoInsumo)
			this.parametros.tipoInsumo = "99";

		if(this.parametros.codigoNCM){
			this.parametros.codigoNCM = this.codigoNCM2.model.split(" | ")[0].replace(" ", "");
		} else{
			this.parametros.codigoNCM = null;
		}

		this.parametros.idLe = this.model.idLe;
		this.parametros.idCodigoNCM = this.model.idCodigoNCM;
		if(this.parametros.idCodigoNCM == null)
			this.parametros.codigoNCM = null;

		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;

		this.applicationService.get(this.servicoGrid, this.parametros).subscribe((result: PagedItems) => {
			this.grid.lista = result.items;
			this.grid.total = result.total;
			let existePendente = false;
			this.grid.lista.forEach(element => {
				if (element.situacaoInsumo == null || element.descricaoTipoInsumoAlteracao == "Cancelado"){
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
		this.parametros.codigoInsumo = null;
		this.parametros.tipoInsumo = '99';
		this.parametros.situacaoInsumo = '99';
		this.parametros.codigoNCM = null;
	}

	analisar(item){
		if(item != null)
			this.appModalAnalisarInsumo.analisar(this, item, this.isAlteracao);
	}

	alterar(item){
		if(item != null)
			this.appModalAnalisarInsumo.alterar(this, item, this.isAlteracao);
	}

	cadastrar() {
		this.appModalAnalisarInsumo.cadastrar(this, this.isAlteracao);
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
		this.buscarInsumos();
	}

	buscar(){

	}
}
