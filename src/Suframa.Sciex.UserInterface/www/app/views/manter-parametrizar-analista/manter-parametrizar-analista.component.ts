import { Component, OnInit, Injectable, ViewChild, Output, EventEmitter, AfterViewInit, Input } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router } from '@angular/router';
import { ValidationService } from "../../shared/services/validation.service";
import { manterPliVM } from '../../view-model/ManterPliVM';

import { EnumPerfil } from '../../shared/enums/EnumPerfil';

@Component({
	selector: 'app-manter-parametrizar-analista',
	templateUrl: './manter-parametrizar-analista.component.html',
	providers: []
})

@Injectable()
export class ManterParametrizarAnalistaComponent implements OnInit {
	servico= 'ParametrizarAnalista';

	constructor(
		private applicationService: ApplicationService,
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
	) {

	}

	isHideCabecalho = false;
	isHidePaginacao = false;
	isShowPanel: boolean = true;
	lista: any[];
	total: number;
	size: number;
	sorted: any = {};
	page: number;
	exportarListagem: boolean;
	parametros: any = {};

	ngOnInit(): void {
		this.listar();
	}

	inativarAnalistaPli(item){
		let msgInativacao = "A inativação irá desvincular todos os PLIs que estão sob responsabilidade do analista $. Confirma a operação?";
		this.modal.confirmacao(msgInativacao.replace('$', item.nome), 'Inativar Analista', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					item.situacaoVisual = 0;
					item.path = "PLI";
					this.applicationService.put(this.servico, item).subscribe((result) => {
						if(result != null){
							this.listar();
						}
					});
				}
			});
	}

	ativarAnalistaPli(item){
		let msgAtivacao = "Deseja realmente ativar o analista "+item.nome+"? ";
		this.modal.confirmacao(msgAtivacao, 'Ativar Analista', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					item.situacaoVisual = 1;
					item.path = "PLI";
					this.applicationService.put(this.servico, item).subscribe((result) => {
						if(result != null){
							this.listar();
						}
					});
				}
			});
	}

	inativarAnalistaLe(item){
		let msgInativacao = "A inativação irá desvincular todas as LEs, Planos e Solicitações de Alteração de Processo que estão sob responsabilidade do analista $. Confirma a operação?";
		this.modal.confirmacao(msgInativacao.replace('$', item.nome), 'Inativar Analista', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					item.situacaoLE = 0;
					item.path = "LE";
					this.applicationService.put(this.servico, item).subscribe((result) => {
						if(result != null){
							this.listar();
						}
					});
				}
			});
	}

	ativarAnalistaLe(item){
		let msgAtivacao = "Deseja realmente ativar o analista "+item.nome+"? ";
		this.modal.confirmacao(msgAtivacao, 'Ativar Analista', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					item.situacaoLE = 1;
					item.path = "LE";
					this.applicationService.put(this.servico, item).subscribe((result) => {
						if(result != null){
							this.listar();
						}
					});
				}
			});
	}

	listar() {
		this.parametros.page = this.page;
		this.parametros.size = this.size;
		this.parametros.sort = this.sorted.field;
		this.parametros.reverse = this.sorted.reverse;
		this.applicationService.get(this.servico, this.parametros).subscribe((result: PagedItems) => {
			this.lista = JSON.parse(JSON.stringify(result.items));
			this.total = result.total;
		});

	}

	changeSize($event) {		
		this.changePage(1);
	}

	changeSort($event) {
		this.sorted = $event;
		this.changePage(this.page);
	}

	changePage($event) {
		this.page = $event;
		this.listar();
	}
}
