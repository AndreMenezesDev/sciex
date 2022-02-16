import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalService } from '../../../../shared/services/modal.service';
import { MessagesService } from '../../../../shared/services/messages.service';
import { ApplicationService } from '../../../../shared/services/application.service';
import { ValidationService } from '../../../../shared/services/validation.service';
import {Location} from '@angular/common';
import { ModalJustificaGlosaQuadroExportacaooComponent } from '../modal/justificativa/modal-justificativa.component';
import { ModalJustificativaIndeferirComponent } from '../../justificativa/modal-justificativa.component';
@Component({
	selector: 'app-analisar-formulario-propriedade-produto',
	templateUrl: './formulario-analisar-propriedade-produto.component.html'
})

export class AnalisarFormularioPropriedadeProdutoComponent implements OnInit {
	path: string;
	servico = "PEProduto";
	servicoPlanoExportacao = "PlanoExportacao";
	servicoPais = "PEPais";
	servicoAnalisar = "AnalisarPlanoDeExportacao";
	modelPE: any = {};
	modelProduto: any = {};
	parametros: any = {};
	listaPais = [];
	totalpais: number = 0;
	idPEProduto : number;

	@Input() sorted: string;
	@Input() page: number;

	@ViewChild('appModalJustificativaGlosa') appModalJustificativaGlosa: ModalJustificaGlosaQuadroExportacaooComponent;
	@ViewChild('modalJustificativaIndeferir') appmodalJustificativaIndeferir: ModalJustificativaIndeferirComponent;
	idPlanoExportacao: any;
	visualizar: boolean = false;

	constructor(
		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private modal: ModalService,
		private Location: Location,
	) {
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.idPEProduto = this.route.snapshot.params['id'];
	}

	ngOnInit() {
		this.modelPE = {};
		this.modelProduto = {};
		this.listaPais = [];

		if (this.path == 'analisar-propriedade-produto') {
			this.visualizar = false;
		}else if (this.path == 'visualizar-propriedade-produto') {
			this.visualizar = true;
		}
		this.selecionarProduto(this.idPEProduto);
	}

	changeSort($event) {
		this.sorted = $event.field;
		//this.onChangeSort.emit($event);
		this.changePage(this.page);
	}

	changePage($event) {
		this.page = $event;
		//this.onChangePage.emit($event);
		//console.log($event);
	}

	public selecionarProduto(id: number) {
		if (!id) { return; }
		this.applicationService.get(this.servico, id).subscribe((result: any) => {
			this.modelProduto = result;
			this.idPlanoExportacao = result.idPlanoExportacao;
			this.selecionarPE(this.idPlanoExportacao);
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
		});
	}
	voltar(){
		this.Location.back();
	}

	abrirGlosa(dados){
		dados.telaSolicitada = 'PRODUTO-PAIS',
		dados.acaoIsAprovar = false,
		dados.idPEProdutoPais = dados.idPEProdutoPais
		
		this.appModalJustificativaGlosa.abrir(dados,this);
	}

	fecharGlosa(){
		this.appModalJustificativaGlosa.fechar();
		this.selecionarPaises(this.idPEProduto);
	}

	aprovarItem(item){
		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.salvarRegistro(item);
				}
			}
		);
	}	
	salvarRegistro(item) {
		let dados = {
			telaSolicitada:'PRODUTO-PAIS',
			acaoIsAprovar:true,
			idPEProdutoPais:item.idPEProdutoPais
		}
		this.applicationService.post(this.servicoAnalisar, dados).subscribe((result:any) => {
			if (result.resultado){
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO,'Sucesso','').
				subscribe(()=>{
					this.selecionarPaises(this.idPEProduto);
				});
			}else{
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO);
			}
		});
	}
	abrirVisualizarGlosa(item){
		this.appmodalJustificativaIndeferir.abrir(item,null, true);
	}
}
