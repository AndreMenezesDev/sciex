import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { PagedItems } from '../../../../view-model/PagedItems';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalService } from '../../../../shared/services/modal.service';
import { MessagesService } from '../../../../shared/services/messages.service';
import { ApplicationService } from '../../../../shared/services/application.service';
import { ValidationService } from '../../../../shared/services/validation.service';
import {Location} from '@angular/common';

declare var $: any;

@Component({
	selector: 'app-analisar-formulario-quadros-insumos',
	templateUrl: './formulario-analisar-quadro-insumos.component.html'
})

export class AnalisarFormularioQuadrosInsumosComponent implements OnInit {
	titulo: string;
	subtitulo: string;
	isQuadroNacional: boolean;
	grid: any = { sort: {} };
	path: string;
	servico = "PEProduto";
	servicoPlanoExportacao = "PlanoExportacao";
	servicoPais = "PEPais";
	servicoListarInsumos = "ListarInsumosNacionalOuImportadoPorIdProdutoAnalise";
	servicoAprovarTodosInsumos = "AprovarTodosInsumosEDetalhesAnalise";
	formPai = this;
	isBuscaSalva: boolean = false;
	modelPE: any = {};
	modelProduto: any = {};
	parametros: any = {};
	listaPais = [];
	totalpais: number = 0;
	isModificouPesquisa: boolean = false;
	idPEProduto : number;
	visualizar: boolean;

	constructor(
		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private modal: ModalService,
		private Location: Location,
		private msg: MessagesService,
		private router: Router,
	) {
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.idPEProduto = this.route.snapshot.params['id'];
	}

	onChangeSort($event) {
		this.grid.sort = $event;
	}
	onChangeSize($event) {
		this.grid.size = $event;
	}
	onChangePage($event) {
		this.grid.page = $event;
		this.carregarInsumos(this.idPEProduto);
	}
	carregarOpcoesPaginacaoInicial(){
		this.parametros.page = 1;
		this.parametros.size = 10;
		this.grid.page = 1;
		this.grid.size = 10;
	}

	ngOnInit() {
		
		this.parametros = {};
	    this.modelPE = {};
		this.modelProduto = {};
		this.listaPais = [];
		this.verificarRota();
		this.grid.total = 0;			
	}

	public verificarRota() {
		this.carregarOpcoesPaginacaoInicial();
		this.titulo = "-";
		this.subtitulo = "-";
		this.isQuadroNacional = false;

		if (this.path == 'analisar-quadro-nacional') {
			this.selecionarProduto(this.idPEProduto);
			this.titulo = "Nacionais e Regionais Quadro II";
			this.subtitulo = "Nacional e Regional";
			this.isQuadroNacional = true;
			this.visualizar = false;
		}
		else if (this.path == 'analisar-quadro-importado') {
			this.selecionarProduto(this.idPEProduto);
			this.titulo = "Importados Quadro III";
			this.subtitulo = "Padrão e Extra Padrão";
			this.isQuadroNacional = false;
			this.visualizar = false;
		}
		else if (this.path == 'visualizar-quadro-nacional') {
			this.selecionarProduto(this.idPEProduto);
			this.titulo = "Nacionais e Regionais Quadro II";
			this.subtitulo = "Nacional e Regional";
			this.isQuadroNacional = true;
			this.visualizar = true;
		}
		else if (this.path == 'visualizar-quadro-importado') {
			this.selecionarProduto(this.idPEProduto);
			this.titulo = "Importados Quadro III";
			this.subtitulo = "Padrão e Extra Padrão";
			this.isQuadroNacional = false;
			this.visualizar = true;
		}


	}
	public selecionarProduto(id: number) {
		if (!id) { return; }
		this.applicationService.get(this.servico, id).subscribe((result: any) => {
			this.modelProduto = result;
			this.selecionarPE(result.idPlanoExportacao);
		});
	}

	carregarInsumos(id?:number){
		id = id || this.idPEProduto;

		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;

		this.parametros.isQuadroNacional = this.isQuadroNacional;
		this.parametros.idPEProduto = id;

		this.applicationService.get(this.servicoListarInsumos, this.parametros).subscribe((result: PagedItems) => {
			if (result.total > 0){

				this.parametros.page = result.page;
				this.parametros.size = result.size;
				this.parametros.total = result.total;
				this.parametros.servico = this.servicoListarInsumos;		
				this.parametros.width = { 
											0: { columnWidth: 40 }, 1: { columnWidth: 40 }, 2: { columnWidth: 40 }, 3: { columnWidth: 50 }, 4: { columnWidth: 100 }, 5: { columnWidth: 50 }, 6: { columnWidth: 60 },
											7: { columnWidth: 60 }, 8: { columnWidth: 80 }, 9: { columnWidth: 80 }, 10: { columnWidth: 80 }, 11: { columnWidth: 80 }, 12: { columnWidth: 80 }
										};
				this.parametros.columns = [
					                        "Status", "Cód.Insumo", "Tipo", "NCM", "Descrição do Insumo", "Coef. Técnico", 
											"Unidade", "Especificação Técnica", "Part Number", "Perc Perda(%)", "Quantidade", "Valor do Insumo (R$)"
										  ];
		
				this.parametros.fields = [
					                        "situacaoAnaliseString", "codigoInsumo", "tipoInsumo", "codigoNcm", "descricaoInsumo", "valorCoeficienteTecnico",
											"codigoUnidade", "descricaoEspecificacaoTecnica", "descricaoPartNumber", "valorPercentualPerda", "qtdSomatorioDetalheInsumoFormatada", "valorInsumoFormatada"
										 ];

				this.parametros.titulo = "Insumos " + this.titulo;	

				if (this.isQuadroNacional){
					this.modelProduto.valorTotalInsumoFormatado = result.items[0].valorTotalInsumoFormatado;
				}else{
					this.modelProduto.valorTotalFOBFormatado = result.items[0].valorTotalFOBFormatado;
					this.modelProduto.valorTotalFreteFormatado = result.items[0].valorTotalFreteFormatado;
					this.modelProduto.valorTotalCFRFormatado = result.items[0].valorTotalCFRFormatado;
				}
				this.parametros.exportarListagem = true;
			}
			this.grid.lista = result.items;
			this.grid.total = result.total;			
		});
	}

	public selecionarPE(id: number) {
		if (!id) { return; }
		this.applicationService.get(this.servicoPlanoExportacao, id).subscribe((result: any) => {
			this.modelPE = result;
			this.selecionarPaises(this.idPEProduto);
		});
	}
	public selecionarPaises(id: number) {
		if (!id) { return; }
		this.parametros.idPEProduto = this.idPEProduto;
		this.applicationService.get(this.servicoPais, this.parametros).subscribe((result: any) => {
			this.listaPais = result.items;
			this.totalpais = result.total;
			this.carregarInsumos(this.idPEProduto);
		});
	}
	voltar(){
		this.Location.back();
	}

	confirmarAprovarTodos(){
		this.modal.confirmacao("Todos os insumos e seus detalhes serão aprovados, deseja continuar?", 'Confirmar Aprovação Todos', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.aprovarTodos();
				}
			});
	}

	aprovarTodos(){
		let dados: any = {};
		dados = Object.assign(dados,this.parametros);
		
		this.applicationService.post(this.servicoAprovarTodosInsumos, dados).subscribe((result: any) => {
			if (result.resultado){
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO,'Sucesso','').
				subscribe(()=>{
					this.carregarInsumos(this.idPEProduto);
				});
			}else{
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO);
				console.log(result.mensagem);
			}
		});
	}

	retornaValorSessao() {
	
		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;
		}
	}
}
