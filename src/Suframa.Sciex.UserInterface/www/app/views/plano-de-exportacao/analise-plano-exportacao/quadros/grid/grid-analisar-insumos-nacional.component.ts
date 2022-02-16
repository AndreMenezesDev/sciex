import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../../../shared/services/application.service';
import { MessagesService } from '../../../../../shared/services/messages.service';
import { ModalService } from '../../../../../shared/services/modal.service';
import { Router } from '@angular/router';
import { ModalJustificaGlosaQuadroExportacaooComponent } from '../../modal/justificativa/modal-justificativa.component';
import { ModalAnaliseDetalhesInsumoComponent } from '../../modal/detalhe-do-insumo/modal-analise-detalhes-insumo.component';
import { ModalJustificativaIndeferirComponent } from '../../../justificativa/modal-justificativa.component';

@Component({
	selector: 'app-analisar-insumos-nacional-grid',
	templateUrl: './grid-analisar-insumos-nacional.component.html',
	styleUrls:['./grid-analisar-insumos.component.css']
})

export class AnalisarInsumosNacionalGridComponent {
	servicoAnalisar = "AnalisarPlanoDeExportacao";

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
	) { }

	@Input() lista: any[];
	@Input() total: number;
	@Input() size: number;
	@Input() page: number;
	@Input() sorted: string;
	@Input() isUsuarioInterno: boolean = false;
	@Input() parametros: any = {};
	@Input() formPai: any;
	@Input() visualizar: boolean;

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();

	@ViewChild('appModalJustificativaGlosa') appModalJustificativaGlosa: ModalJustificaGlosaQuadroExportacaooComponent;
	@ViewChild('appModalDetalheInsumo') appModalDetalheInsumo : ModalAnaliseDetalhesInsumoComponent;
	@ViewChild('modalJustificativaIndeferir') appmodalJustificativaIndeferir: ModalJustificativaIndeferirComponent;

	public abrirModalDetalheInsumo(dadosInsumo){
		dadosInsumo.visualizar = this.visualizar;
		this.appModalDetalheInsumo.abrir(this, true,dadosInsumo);
	}
	

	changeSize($event) {
		this.onChangeSize.emit($event);
	}

	changeSort($event) {
		this.sorted = $event.field;
		this.onChangeSort.emit($event);
		this.changePage(this.page);
	}

	changePage($event) {
		this.page = $event;
		this.onChangePage.emit($event);
		console.log($event);
	}

	abrirGlosa(dados){
		dados.telaSolicitada = 'QUADRO-INSUMO';
		dados.acaoIsAprovar = false;
		dados.idPEInsumo = dados.idPEInsumo;
		
		this.appModalJustificativaGlosa.abrir(dados,this);
	}

	fecharGlosa(){
		this.appModalJustificativaGlosa.fechar();
		this.carregarDadosInsumos();
	}
	carregarDadosInsumos(){
		this.formPai.carregarInsumos();
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
			telaSolicitada:'QUADRO-INSUMO',
			acaoIsAprovar:true,
			idPEInsumo:item.idPEInsumo
		}
		this.applicationService.post(this.servicoAnalisar, dados).subscribe((result:any) => {
			if (result.resultado){
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO,'Sucesso','').
				subscribe(()=>{
					this.formPai.carregarInsumos();
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
