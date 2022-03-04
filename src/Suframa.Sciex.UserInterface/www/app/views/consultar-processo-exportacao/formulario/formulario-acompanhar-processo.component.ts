import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalService } from '../../../shared/services/modal.service';
import { PagedItems } from '../../../view-model/PagedItems';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import {Location} from '@angular/common';
import { forEach } from '@angular/router/src/utils/collection';

declare var $: any;

@Component({
	selector: 'app-formulario-acompanhar-processoo',
	templateUrl: './formulario-acompanhar-processo.component.html'
})

export class FormularioAcompanharProcessoComponent implements OnInit {

	grid: any = { sort: {} };
	insumosAprovados: any = {};
	insumosComprovados: any = {};
	listaProdutosBackup = [];
	saldos: any = {};
	path: string;
	formPai = this;
	servico = "ProcessoExportacao";
	servicoHistoricoProcesso = "HistoricoProcesso";
	model: any = {};
	parametros: any = {};
	listaProdutos: any;
	idProcesso: any;
	backToAcompanharProcesso : string;
	url : string;

	constructor(
		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private Location: Location,
		private router: Router
	) {
		this.url = this.router.url;
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.idProcesso = this.route.snapshot.params['idProcesso'];
		this.backToAcompanharProcesso = sessionStorage.getItem("backToAcompanharProcesso");
	}

	ngOnInit() {
		this.selecionar(this.idProcesso);

		$(document).ready(function () {
			$("#collapse-init").click(function () {
				$('.accordion-toggle').collapse('hide');
				$('.panel-collapse').collapse('hide');
			});
		});
	}
	public selecionar(id: number) {
		if (!id) { return; }
		this.listaProdutos = null;
		this.applicationService.get(this.servico, id).subscribe((result: any) => {
			this.model = result;
			this.insumosAprovados = result.insumosAprovados;
			this.saldos = result.saldos;
			this.insumosComprovados = result.insumosComprovados;
			this.listaProdutos = this.listaProdutosBackup = result.listaProduto;
			this.buscarHistorico();
		});
	}

	abrirInformacoesProdutoPai(idProduto){
		let url = `/consultar-processo-exportacao/${idProduto}/visualizar-propriedade-produto`;
		this.setHistoryUrl(url)
		this.router.navigate([url])
	}

	abrirInsumoNacionalOuRegional(idProduto){
		let url = `/consultar-processo-exportacao/${idProduto}/visualizar-quadro-nacional`;
		this.setHistoryUrl(url)
		this.router.navigate([url])
	}

	abrirInsumoImportado(idProduto){
		let url = `/consultar-processo-exportacao/${idProduto}/visualizar-quadro-importado`;
		this.setHistoryUrl(url)
		this.router.navigate([url])
	}
	setHistoryUrl(url){
		let arrayUrl = sessionStorage.getItem("arrayUrl");
		let listArray = JSON.parse( arrayUrl)
		listArray.push(url)
		sessionStorage.removeItem("arrayUrl");
		sessionStorage.setItem("arrayUrl",JSON.stringify(listArray));
	}

	voltar(){
		sessionStorage.setItem('sameScreen','consultar-processo-exportacao');

		let arrayUrl = sessionStorage.getItem("arrayUrl");
		let obj = JSON.parse(arrayUrl)
		let url = obj[obj.length - 2]
		obj.pop()
		sessionStorage.removeItem("arrayUrl")
		sessionStorage.setItem("arrayUrl", JSON.stringify(obj))

		this.router.navigate([url]);
	}

	filtrarProdutos(){

		if (this.parametros.pesquisaDescricaoModelo != '' && this.parametros.pesquisaDescricaoModelo != undefined){

			let stringPesquisada = (this.parametros.pesquisaDescricaoModelo as string).toUpperCase();

			let novaLista = this.listaProdutosBackup.filter(element => (element.descricaoModelo as string).toUpperCase().includes(stringPesquisada))

			this.listaProdutos = novaLista;
		}else{
			this.listaProdutos = this.listaProdutosBackup;
		}
	}

	abrirMinhasSolicitacoes(){

		let url = `/minhas-solicitacoes-alteracao/${this.idProcesso}/visualizar-quadro-importado`;
		this.setHistoryUrl(url)
		this.router.navigate([url])
	}

	buscarHistorico(){
		var objeto : any = {};
		objeto.idProcesso = Number(this.idProcesso);
		objeto.sort = this.grid.sort;
		objeto.size = this.grid.size;
		objeto.page = this.grid.page;
		this.applicationService.get(this.servicoHistoricoProcesso, objeto).subscribe((result: PagedItems) => {
			console.log(result)
			for(var i =0; i<result.items.length;i++){

				if(result.items[i].anoNumeroSolicitacaoFormatado=="0/00000"){
					result.items[i].anoNumeroSolicitacaoFormatado=" "
				}
				if(result.items[i].descricaoPlanoExportacao=="/"){
					result.items[i].descricaoPlanoExportacao=" "
				}
			}
			console.log(result)
			this.grid.lista = result.items;

			this.grid.total = result.total;
		});
	}

	onChangeSort($event) {
		this.grid.sort = $event;

	}

	onChangeSize($event) {
		this.grid.size = $event;
	}

	onChangePage($event) {
		this.grid.page = $event;
		this.buscarHistorico();
	}
}
