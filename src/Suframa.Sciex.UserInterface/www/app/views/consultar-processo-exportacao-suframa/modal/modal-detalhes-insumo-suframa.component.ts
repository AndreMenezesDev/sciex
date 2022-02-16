import { Component, ViewChild } from '@angular/core';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';


@Component({
	selector: 'app-modal-detalhes-insumo-suframa',
	templateUrl: './modal-detalhes-insumo-suframa.component.html',
})

export class ModalDetalhesInsumoSuframaComponent {	
	modelProduto: any = {};
	grid: any = { sort: {} };
	isQuadroNacional: boolean;
	servico = 'ListarProcessoInsumosNacionalOuImportadoPorIdInsumoSuframa';
	servicoInfAdicionaisProduto = 'InformacoesAdicionaisProduto';
	parametros: any = {};
	formPai: any;
	path: string;
	modelInsumo: any = {};
	idInsumo: any;
	infComplementar : any = {};

	constructor(
		private modal: ModalService,
		private applicationService: ApplicationService,
	) { }

	@ViewChild('appModalDetalhesInsumo') appModalDetalhesInsumo;
	@ViewChild('appModalDetalhesInsumoBackground') appModalDetalhesInsumoBackground;

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

	public abrir(formPai, isQuadroNacional: boolean, dadosInsumo) {
		this.modelInsumo = dadosInsumo;
		this.modelProduto = {};
		this.formPai = formPai;
		this.idInsumo = dadosInsumo.idInsumo;
		this.isQuadroNacional = isQuadroNacional;

		this.parametros = {};
		this.carregarOpcoesPaginacaoInicial();
		this.carregarListaDetalheInsumo(this.idInsumo);
		this.appModalDetalhesInsumo.nativeElement.style.display = 'block';
		this.appModalDetalhesInsumoBackground.nativeElement.style.display = 'block';
	}
	public carregarListaDetalheInsumo(id: number) {
		if (!id) { return; }

		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;

		this.parametros.isQuadroNacional = this.isQuadroNacional;
		this.parametros.idProcessoInsumo = id;

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

	carregarOpcoesPaginacaoInicial(){
		this.parametros.page = 1;
		this.parametros.size = 10;
		this.grid.page = 1;
		this.grid.size = 10;
	}
	public fechar() {
		this.appModalDetalhesInsumo.nativeElement.style.display = 'none';
		this.appModalDetalhesInsumoBackground.nativeElement.style.display = 'none';
	}
	

}
