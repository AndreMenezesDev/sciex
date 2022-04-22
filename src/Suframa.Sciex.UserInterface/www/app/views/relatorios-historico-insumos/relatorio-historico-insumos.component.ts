import { Component, OnInit, Injectable, ViewChild } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router } from '@angular/router';
import { AuthGuard } from '../../shared/guards/auth-guard.service';
import { ValidationService } from '../../shared/services/validation.service';
import { Location } from '@angular/common';
import * as html2pdf from 'html2pdf.js';
import { AssignHour } from '../../shared/services/assignHour.service';
import { FormatCodeService } from '../../shared/services/format-code.service';
import { ExcelService } from '../../shared/services/excel.service';

@Component({
	selector: 'relatorio-historico-insumos',
	templateUrl: './relatorio-historico-insumos.component.html'
})

@Injectable()
export class RelatorioHistoricoInsumosComponent implements OnInit {
	processo: any;
	produto: any;
	exibeRelatorio: boolean = false;
	@ViewChild('relatorio') relatorio;

	constructor(
		private applicationService: ApplicationService,
		private modal: ModalService,
		private assignHour: AssignHour,
		private excelService: ExcelService,
		private formatCodeService: FormatCodeService,
		private msg: MessagesService
	) {

	}

	ngOnInit(): void {

	}

	limpar(){
		this.processo = '';
		this.produto = '';
	}

	emitirHistoricoInsumo(){
		if (this.processo == '' || this.processo == null)
			this.modal.alerta("N° do Processo não informado");

		if (this.produto == '' || this.produto == null)
			this.modal.alerta("produto não informado");

		if(this.produto){

			this.relatorio.emitirHistoricoInsumo(this.produto);
		}
	}
}
