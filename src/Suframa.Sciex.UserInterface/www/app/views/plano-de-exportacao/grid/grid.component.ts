import { Component, Output, Input, OnInit, EventEmitter, ViewChild, ViewContainerRef } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { parametroAnalistaVM } from '../../../view-model/ParametroAnalistaVM';
import { ModalJustificativaIndeferirComponent } from '../justificativa/modal-justificativa.component';

@Component({
	selector: 'app-plano-exportacao-grid-component',
	templateUrl: './grid.component.html'
})

export class PlanoDeExportacaoGridComponent {

	servicoParametrizarAnalista = 'ParametroAnalista1';
	model: parametroAnalistaVM = new parametroAnalistaVM();
	tipoSistema: number;
	modalAtivarAnalista: boolean;
	
	isDeferir: boolean = false;

	servicoDeferir ="DeferirPlanoExportacao";


	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
	) { }

	@Input() lista: any;
	@Input() total: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() page: number;
	@Input() formPai: any;
	@Input() parametros: any = {};

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();

	@ViewChild('modalJustificativaIndeferir') appmodalJustificativaIndeferir: ModalJustificativaIndeferirComponent;
    
	changeSize($event) {
		this.onChangeSize.emit(+$event);
	}

	changeSort($event) {
		this.sorted = $event.field;
		this.onChangeSort.emit($event);
		this.changePage(this.page);
	}

	changePage($event) {
		this.page = $event;
		this.onChangePage.emit($event);
	}

	confirmarIndeferimento(item){
		this.modal.confirmacao("Deseja INDEFERIR o Plano de Exportação selecionado?","Confirmação","")
		.subscribe(result => {
			if (result){
				this.appmodalJustificativaIndeferir.abrir(item,this.formPai);
			}
		})
	}

	confirmarDeferir(id) {
		this.modal.confirmacao("Deseja DEFERIR o Plano de Exportação selecionado?", '', '')
			.subscribe(isDeferir => {
				if (isDeferir) {
					this.deferirPlanoExportacao(id);
				}
			});
	}

	deferirPlanoExportacao(id) {

		this.applicationService.post(this.servicoDeferir, id).subscribe((result:any) => {			

			if (!result.resultado) {
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO + ": "+result.mensagem, "Atenção", "");
			}else if(result.camposNaoValidos == null && result.possuiTodosRegistros){
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO + ' '+ result.mensagem, "Sucesso", '')
				.subscribe(()=>{
					this.formPai.listar();
				});
			}else if(result.camposNaoValidos != null){
				if (result.camposNaoValidos.naoExisteParidadeCambial){
					this.modal.alerta("Falha ao tentar deferir. Não existe valor de paridade cambial para a moeda utilizada, na data de hoje.","Atenção","");
				}
				else if (result.camposNaoValidos.naoExisteParidadeCambialEstrangeira){
					this.modal.alerta("Falha ao tentar deferir. Não existe valor de paridade cambial para a moeda utilizada, na data de hoje.","Atenção","");
				}
			}else if(!result.possuiTodosRegistros){
				this.modal.alerta("Existem itens não analisados ou reprovados nessa análise. Não é possível deferir esse plano.","Erro",'');
			}

		});

	}
}
