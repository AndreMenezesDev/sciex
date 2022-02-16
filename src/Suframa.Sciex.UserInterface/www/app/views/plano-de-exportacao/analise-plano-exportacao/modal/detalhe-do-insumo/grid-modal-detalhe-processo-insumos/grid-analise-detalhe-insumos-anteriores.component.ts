import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../../../../shared/services/application.service';
import { MessagesService } from '../../../../../../shared/services/messages.service';
import { ModalService } from '../../../../../../shared/services/modal.service';
import { Router } from '@angular/router';
import { ModalJustificaGlosaQuadroExportacaooComponent } from '../../justificativa/modal-justificativa.component';
import { ModalJustificativaIndeferirComponent } from '../../../../justificativa/modal-justificativa.component';

@Component({
	selector: 'app-grid-modal-analise-detalhe-processo-insumos-anteriores',
	templateUrl: './grid-analise-detalhe-insumos-anteriores.component.html'
})

export class AnaliseDetalheInsumosAnterioresModalGridComponent {
	servicoAnalisar = "AnalisarPlanoDeExportacao";
	exibirCabecalhoGrid = false;

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
	@Input() parametros: any = {};
	@Input() formPai: any;
	@Input() isQuadroNacional: boolean = false;
	@Input() visualizar: boolean;

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();

	@ViewChild('appModalJustificativaGlosa') appModalJustificativaGlosa: ModalJustificaGlosaQuadroExportacaooComponent;
	@ViewChild('modalJustificativaIndeferir') appmodalJustificativaIndeferir: ModalJustificativaIndeferirComponent;

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

		dados.telaSolicitada = 'DETALHE-INSUMO';
		dados.acaoIsAprovar = false;
		dados.idPEDetalheInsumo = dados.idPEDetalheInsumo;
		
		this.appModalJustificativaGlosa.abrir(dados,this);
	}

	fecharGlosa(){
		this.appModalJustificativaGlosa.fechar();
		this.formPai.carregarListaDetalheInsumo();
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
			telaSolicitada:'DETALHE-INSUMO',
			acaoIsAprovar:true,
			idPEDetalheInsumo:item.idPEDetalheInsumo
		}
		this.applicationService.post(this.servicoAnalisar, dados).subscribe((result:any) => {
			if (result.resultado){
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO,'Sucesso','').
				subscribe(()=>{
					this.formPai.carregarListaDetalheInsumo();
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
