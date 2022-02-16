import { Component, Injectable, OnInit, ViewChild } from '@angular/core';
import { ModalService } from '../../../../shared/services/modal.service';
import { ApplicationService } from '../../../../shared/services/application.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PRCInsumoVM } from '../../../../view-model/PRCInsumoVM';
import { Location } from '@angular/common';


@Component({
	
	selector: 'app-formulario-detalhes-insumo-suframa',
	templateUrl: './formulario-detalhes-insumo-suframa.component.html',
})
@Injectable()

export class FormularioDetalhesInsumoSuframaComponent implements OnInit {	
	titulo: string;
	subtitulo: string;
	idProcesso : number;
	modelProcesso: any = {};
	url : string;
	servicoProduto = "ProcessoProduto";
	servicoListarInsumos = "ListarProcessoInsumosNacionalOuImportadoPorIdProduto";
	servicoPrcInsumo = "ListarProcessoInsumosNacionalOuImportadoPorIdProduto";

	modelProduto: any = {};
	grid: any = { sort: {} };
	isQuadroNacional: boolean;
	servico = 'ListarProcessoInsumoImportadoParaAnalise';
	servicoInfAdicionaisProduto = 'InformacoesAdicionaisProduto';
	parametros: any = {};
	formPai =  this;
	path: string;
	modelInsumo: any = {};
	idInsumo: any;
	infComplementar : any = {};
	coeficienteTecnico: any; 
	percentualPerda: any; 
	idProduto: any;

	constructor(
		private modal: ModalService,
		private applicationService: ApplicationService,
		private route: ActivatedRoute,
		private router: Router,
		private Location: Location,
	) {
		this.url = this.router.url;
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.idInsumo = this.route.snapshot.params['idInsumo'];
	 }

	@ViewChild('appModalDetalhesInsumo') appModalDetalhesInsumo;
	@ViewChild('appModalDetalhesInsumoBackground') appModalDetalhesInsumoBackground;


	ngOnInit(){
		this.verificarRota();
	}
	public verificarRota() {
		this.carregarOpcoesPaginacaoInicial();
		this.titulo = "-";
		this.subtitulo = "-";
		this.isQuadroNacional = false;

		if (this.path == 'visualizar-quadro-nacional') {
			this.selecionarInsumo(this.idInsumo);
			this.titulo = "Nacionais e Regionais - Quadro II";
			this.subtitulo = "Nacional e Regional";
			this.isQuadroNacional = true;
		}
		else if (this.path == 'visualizar-quadro-importado') {
			this.selecionarInsumo(this.idInsumo);
			this.titulo = "Importados - Quadro III";
			this.subtitulo = "Padrão e Extra Padrão";
			this.isQuadroNacional = false;
		}
	}
	public selecionarInsumo(id: number) {
		if (!id) { return; }
		this.applicationService.get(this.servicoPrcInsumo, id).subscribe((result: any) => {
			this.modelInsumo = result;
			this.coeficienteTecnico = result.valorCoeficienteTecnico || 0; 
			this.percentualPerda = result.valorPercentualPerda || 0;	
			this.idProduto = result.idPrcProduto;	
			this.idInsumo = result.idInsumo;
			this.selecionarProduto(this.idProduto)
			this.carregarListaDetalheInsumo(this.idInsumo);
		});
	}
	public selecionarProduto(id: number) {
		if (!id) { return; }
		this.applicationService.get(this.servicoProduto, id).subscribe((result: any) => {
			this.modelProduto = result;
			this.idProcesso = result.idProcesso;
			this.modelProcesso = result.processo;
		});
	}

	public carregarListaDetalheInsumo(id?: number) {
		//if (!id) { return; }

		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;

		this.parametros.isQuadroNacional = this.isQuadroNacional;
		this.parametros.idProcessoInsumo = id || this.idInsumo;

		this.applicationService.get(this.servico, this.parametros).subscribe((result: any) => {
			this.grid.lista = result.items;
			this.grid.total = result.total;
		});
		var paramInfAdiocnal : any = {};
		paramInfAdiocnal.codigoUnidade = this.modelInsumo.codigoUnidade;
		this.applicationService.get(this.servicoInfAdicionaisProduto, paramInfAdiocnal).subscribe((result: any) => {
			this.infComplementar = result;
		});
	}
	voltar(){
		this.Location.back();
	}

	onChangeSort($event) {
		this.grid.sort = $event;
	}
	onChangeSize($event) {
		this.grid.size = $event;
	}
	onChangePage($event) {
		this.grid.page = $event;
		this.carregarListaDetalheInsumo(this.idInsumo)
	}
	carregarOpcoesPaginacaoInicial(){
		this.parametros.page = 1;
		this.parametros.size = 10;
		this.grid.page = 1;
		this.grid.size = 10;
	}	

}
