import { Component, ViewChild } from '@angular/core';
import { ModalService } from '../../../../../shared/services/modal.service';
import { ApplicationService } from '../../../../../shared/services/application.service';


@Component({
	selector: 'app-modal-analise-detalhes-insumo',
	templateUrl: './modal-analise-detalhes-insumo.component.html',
})

export class ModalAnaliseDetalhesInsumoComponent {	
	tituloCorrecao = '';
	grid: any = { sort: {} };
	isQuadroNacional: boolean;
	servicoBuscarDetalhePorIdInsumoAtuais = "ListarParaAnaliseDetalheInsumoPorIdInsumo";
	servicoBuscarDetalhePorIdInsumoAnteriores = "ListarParaAnaliseDetalheInsumoAnterioresPorIdInsumo";
	servicoBuscarInsumoAnterior = "BuscarUltimoInsumoAnteriorPorIdInsumoAtual";
	parametros: any = {};
	formPai: any;
	modelInsumo: any = {};
	modelInsumoAnterior: any = {};
	idPEInsumo: any;
	formGridInsumo: any;
	visualizar: boolean;
	isInsumoCorrigido: boolean = false;

	constructor(
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
		this.carregarListaDetalheInsumo(this.idPEInsumo)
	}

	public abrir(formGridInsumos, isQuadroNacional: boolean, dadosInsumo) {
		this.modelInsumo = dadosInsumo;
		this.isInsumoCorrigido = dadosInsumo.situacaoAnalise == 4 ? true : false;
		this.modelInsumo.descTipoInsumo = this.tratarTipoInsumo(this.modelInsumo.tipoInsumo);
		this.visualizar = dadosInsumo.visualizar;
		this.formGridInsumo = formGridInsumos;
		this.formPai = this;
		this.idPEInsumo = dadosInsumo.idPEInsumo;
		this.isQuadroNacional = isQuadroNacional;

		this.parametros = {};
		this.carregarOpcoesPaginacaoInicial();
		this.carregarListaDetalheInsumo(this.idPEInsumo);
		this.appModalDetalhesInsumo.nativeElement.style.display = 'block';
		this.appModalDetalhesInsumoBackground.nativeElement.style.display = 'block';
	}
	public carregarListaDetalheInsumo(id?: number) {
		
		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;

		this.parametros.isQuadroNacional = this.isQuadroNacional;
		this.parametros.IdPEInsumo = id || this.idPEInsumo;;

		

		if (!this.isInsumoCorrigido){
			this.tituloCorrecao = '';
			this.applicationService.get(this.servicoBuscarDetalhePorIdInsumoAtuais, this.parametros).subscribe((result: any) => {
				this.grid.lista = result.items;
				this.grid.total = result.total;
			});
		}else{
			this.tituloCorrecao = '- Dados Novos';

			//buscar detalhes atuais
			this.applicationService.get(this.servicoBuscarDetalhePorIdInsumoAtuais, this.parametros).subscribe((result: any) => {
				this.grid.lista = result.items;
				this.grid.total = result.total;
				this.buscarInsumoAnterior();
			});			
		}	
		
	}
	buscarInsumoAnterior(){
		this.applicationService.get(this.servicoBuscarInsumoAnterior, {id: this.idPEInsumo}).subscribe((result: any) => {
			if(result != null) {
				this.modelInsumoAnterior = result;
				this.buscarDetalheInsumosAnterior(result.idPEInsumo);
			}
			
		});
	}
	buscarDetalheInsumosAnterior(idInsumoAnterior){
		let obj = {
			idPEInsumo: idInsumoAnterior
		};

		this.applicationService.get(this.servicoBuscarDetalhePorIdInsumoAnteriores, obj).subscribe((result: any) => {
			this.grid.lista2 = result.items;
			this.grid.total2 = result.total;
		});
	}

	carregarOpcoesPaginacaoInicial(){
		this.parametros.page = 1;
		this.parametros.size = 10;
		this.grid.page = 1;
		this.grid.size = 10;
		this.grid.total = 0;
	}
	public fechar() {
		this.formGridInsumo.carregarDadosInsumos();
		this.appModalDetalhesInsumo.nativeElement.style.display = 'none';
		this.appModalDetalhesInsumoBackground.nativeElement.style.display = 'none';
	}
	
	tratarTipoInsumo(tipoInsumo: string){

		var descricaoTipoInsumo;

		if(tipoInsumo == 'E'){
			descricaoTipoInsumo="E - Extra listagem padrão"
		}else if(tipoInsumo == 'N'){
			descricaoTipoInsumo=" N - Nacional"
		}else if('P'){
			descricaoTipoInsumo="P - Padrão"
		}else if('R'){
		descricaoTipoInsumo="R - Regional"
		}else{
			descricaoTipoInsumo = '';
		}

		return descricaoTipoInsumo;
	}

}
