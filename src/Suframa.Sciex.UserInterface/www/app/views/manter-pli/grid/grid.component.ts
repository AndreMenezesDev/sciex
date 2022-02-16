import { Component, Output, Input, OnInit, EventEmitter, ViewChild } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ModalService } from '../../../shared/services/modal.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { Router } from '@angular/router';

@Component({
	selector: 'app-manter-pli-grid',
	templateUrl: './grid.component.html'
})

export class ManterPliGridComponent {
	servicoPli = 'Pli';
	servicoPliGrid = 'PliGrid';
	model: manterPliVM = new manterPliVM();

	@ViewChild('appModalMercadoriaPli') appModalMercadoriaPli;
	@ViewChild('appModalMercadoriaComercializacaoPli') appModalMercadoriaPliComercializacao;
	@ViewChild('appModalResposta') appModalResposta;
	@ViewChild('editar') editar;

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
	) { }

	@Input() lista: any[];
	@Input() total: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() page: number;
	@Input() exportarListagem: boolean;
	@Input() parametros: any = {};

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

	deletar(id) {

		this.applicationService.delete(this.servicoPli, id).subscribe(result => {

			this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", '').subscribe(isConfirmado => {

				if (this.lista.length == 1)
					this.changePage(this.page - 1);
				else
					this.changePage(this.page);
			});
		}, error => {
			if (this.lista.length == 1)
				this.changePage(this.page - 1);
			else
				this.changePage(this.page);
		});

	}

	confirmarExclusao(id) {
		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.deletar(id);
				}
			});
	}

	confirmarCopiaPli(id) {
		this.modal.confirmacao("Deseja criar uma nova cópia de PLI a partir do PLI selecionado?", '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.copiarPli(id);
				}
			});
	}

	copiarPli(item) {
		this.model.idPLI = item.idPLI;
		this.model.entregarPli = false;
		this.model.validarPli = false;
		this.model.copiaPli = true;

		this.applicationService.put<manterPliVM>(this.servicoPli, this.model).subscribe(result => {
			if (result.mensagemErro == null) {
				this.modal.resposta("PLI copiado com sucesso. Novo PLI N° " + result.numeroPliConcatenado, "Sucesso", '').subscribe(isConfirmado => {
					this.changePage(this.page);
				});
			} else {
				if (result.mensagemErro.length > 0) {
					this.modal.resposta(result.mensagemErro, "Erro", '');
				}
			}
		}, error => {
			if (this.lista.length == 1)
				this.changePage(this.page - 1);
			else
				this.changePage(this.page);
		});
	}

	confirmarValidarPli(item) {
		this.validarPli(item);
	}

	validarPli(item) {
		this.model.idPLI = item.idPLI;
		this.model.idPLIAplicacao = item.idPLIAplicacao;
		this.model.entregarPli = false;
		this.model.validarPli = true;
		this.model.copiaPli = false;

		this.applicationService.put<manterPliVM>(this.servicoPli, this.model).subscribe(result => {

			if (result.isPliValidado) {
				this.modal.resposta('PLI sem erro de validação.', "Sucesso", '');
			}
			else if (!result.isPliValidado) {
				if (result.tipoErro == 1) {
					this.modal.confirmacao(result.mensagemErro, "Confirmação", '').subscribe((isConfirmado) => {
                        if (isConfirmado) {
							if(result.mensagemErro == "Erro na validação do Anexo. Deseja corrigir os dados?"){
								if (result.codigoPliAplicao == 0 || result.codigoPliAplicao == 2 || result.codigoPliAplicao == 3) {
									this.router.navigate([`/manter-pli/${result.idPLI}/editarretificadoracomercializacao`]);
								}
								if (result.codigoPliAplicao == 1) {
									this.router.navigate([`/manter-pli/${result.idPLI}/editarretificadora`]);
								}
							}
							else{
								let liref = result.numeroLIReferencia;
								for (var i = 0; i < result.listaMercadorias.length; i++) {
									result.listaMercadorias[i].numeroLIReferencia = liref
									result.listaMercadorias[i].idPLIAplicacao = this.model.idPLIAplicacao;
								}
	
								if (this.model.idPLIAplicacao == 2)
									this.appModalMercadoriaPli.abrirListaValidacao(result.listaMercadorias, result.numeroPliConcatenado);
								else
									this.appModalMercadoriaPliComercializacao.abrirListaValidacao(result.listaMercadorias, result.numeroPliConcatenado);
							}
						}
					});
				} else {
					this.modal.alerta(result.mensagemErro, "Informação", '');
				}
			}
		});
	}

	abrirModalResposta(item){
		this.appModalResposta.abrir(item);
	}

	confirmarEntregaPli(item) {
		this.modal.confirmacao('Deseja realizar a entrega do PLI?', '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.entregarPli(item);
				}
			});
	}

	entregarPli(item) {
		this.model.idPLI = item.idPLI;
		this.model.idPLIAplicacao = item.idPLIAplicacao;
		this.model.entregarPli = true;
		this.model.validarPli = false;
		this.model.copiaPli = false;

		this.applicationService.put<manterPliVM>(this.servicoPli, this.model).subscribe(result => {
			if(result != null && result.mensagemErro != null && result.mensagemErro != ''){
				this.modal.alerta(result.mensagemErro, "Erro", '')
				.subscribe(isConfirmado => {
					if (this.lista.length == 1)
						this.changePage(this.page - 1);
					else
						this.changePage(this.page);
				});
			}
			else if (!result.isPliValidado) {
				this.validarPli(item);
				return;
			}
			else {
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", '').subscribe(isConfirmado => {

					if (this.lista.length == 1)
						this.changePage(this.page - 1);
					else
						this.changePage(this.page);
				});
			}
		});
	}

}
