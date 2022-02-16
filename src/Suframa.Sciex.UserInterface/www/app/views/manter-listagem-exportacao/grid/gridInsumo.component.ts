import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { Router } from '@angular/router';
import { ManterLEInsumoFormularioComponent } from '../formulario/formularioInsumo.component';

@Component({
	selector: 'app-manter-le-insumo-grid',
	templateUrl: './gridInsumo.component.html'
})

export class ManterLEInsumoGridComponent {
	servico = 'LEInsumo';
	servicoExcluirLeNormal = 'ExcluirLEInsumo';
	servicoCancelarLe = 'LECancelarInsumo';
	model: any;

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
		private ManterLEInsumoFormularioComponent: ManterLEInsumoFormularioComponent,
	) { }

	@Input() lista: any[];
	@Input() total: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() page: number;
	@Input() exportarListagem: boolean;
	@Input() isLEBloq: boolean;
	@Input() parametros: any = {};
	@Input() somenteLeitura: any = {};

	
	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
	@Output() onChange: EventEmitter<any> = new EventEmitter();

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

	abrirMensagemErro(item){
		this.modal.resposta(item.ultimoInsumoErro.descricaoErro, "Informação", "");
	}

	deletar(id) {

		if(this.parametros.statusLE != 1 ) {
			this.applicationService.delete(this.servico, id).subscribe(result => {
	
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", '').subscribe(isConfirmado => {
	
					this.ManterLEInsumoFormularioComponent.buscar();
				});
			}, error => {
			});
		}
		else{
			this.applicationService.delete(this.servicoExcluirLeNormal, id).subscribe(result => {
	
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", '').subscribe(isConfirmado => {
	
					this.ManterLEInsumoFormularioComponent.buscar();
				});
			}, error => {
			});
		}

	}

	cancelarInsumo(item) {
		this.modal.confirmacao("Deseja realmente cancelar o insumo?", '', '')
		.subscribe(isConfirmado => {
			if (isConfirmado) {
				this.parametros.idLeInsumo = item.idLeInsumo;
		
				this.applicationService.put(this.servicoCancelarLe, this.parametros).subscribe((result: any) => {			
					if (result.mensagemErro != null && result.mensagemErro != "") {
						this.modal.alerta(result.mensagemErro, "Informação", "");
						return;
					}
					else if (result.mensagem != null && result.mensagem != ""){
						this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", '').subscribe(isConfirmado => {
							if (this.lista.length == 1)
								this.changePage(this.page - 1);
							else
								this.changePage(this.page);
						});
					}
				});
			}
		});
	}

	alterarBloqueado(item){
		this.ManterLEInsumoFormularioComponent.alterarBloqueado(item);
	}

	visualizar(item){
		this.ManterLEInsumoFormularioComponent.visualizar(item);
	}

	alterar(item){
		this.ManterLEInsumoFormularioComponent.alterar(item);
	}

	confirmarExclusao(id) {
		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.deletar(id);
				}
			});
	}
}
