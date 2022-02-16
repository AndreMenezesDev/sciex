import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../../shared/services/application.service';
import { MessagesService } from '../../../../shared/services/messages.service';
import { ModalService } from '../../../../shared/services/modal.service';
import { manterPliVM } from '../../../../view-model/ManterPliVM';
import { Router } from '@angular/router';

@Component({
	selector: 'app-dados-comprobatorios-correcao-grid',
	templateUrl: './grid-dados-comprobatorios-correcao.component.html',
	styleUrls: ['./grid-dados-comprobatorios-correcao.component.css'],
})

export class DocumentosComprobatoriosCorrecaoGridComponent {
	servicoInativarDue = "InativarDUE";
	ocultarTituloGrid = true;
	@ViewChild("appModalJustificativaErroDue") appModalJustificativaErroDue;
	@ViewChild('appModalEditarDocumentoDueCorrecao') appModalEditarDocumentoDueCorrecao;

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
	@Input() exportarListagem: boolean;
	@Input() parametros: any = {};
	@Input() formPai: any;
	@Input() somenteLeitura: boolean;
	@Input() isCorrecao: boolean;
	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();

	abrirModalEditar(item){
		this.appModalEditarDocumentoDueCorrecao.abrir(this.formPai, item);
	}

    alterar(item){
		this.abrirModalEditar(item);
    }

	Inativar(item){

		this.modal.confirmacao("Confirma a operação?","Confirmação","")
		.subscribe(result => {
			if (result){
				this.InativarDue(item);
			}
		})
	}
	InativarDue(item){
		let id = item.idDue
		this.applicationService.put(this.servicoInativarDue, item).subscribe((result: any) =>{
			if (result == 1) {
				this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO, "Informação", "");
				return;
			}else{
				this.formPai.selecionarProduto();
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informação", "");
			}
		});
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
	abrirModalJustificativaErro(item){
		this.appModalJustificativaErroDue.abrir(item);
	}

}
