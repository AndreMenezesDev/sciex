import { Component, OnInit, Injectable, ViewChild } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router } from '@angular/router';
import { AuthGuard } from '../../shared/guards/auth-guard.service';
import { ValidationService } from '../../shared/services/validation.service';
import { Location } from '@angular/common';
import { FormatCodeService } from '../../shared/services/format-code.service';
import { ExcelService } from '../../shared/services/excel.service';
import { PDFService } from '../../shared/services/pdf.service';
@Component({
	selector: 'app-plano-de-exportacao',
	templateUrl: './relatorio-listagem-exportada.component.html'
})

@Injectable()
export class RelatorioListagemExportacaoComponent implements OnInit {
	formPai = this;

	grid: any = { sort: {} };
	parametros: any = {};
	result: boolean = false;
	dataInicio : string;
	dataFim: string;
	servico = 'RelatorioListagemExportacaoAprovada';
	lista: any = {};
	exibeRelatorio: boolean = false;
	constructor(
		private applicationService: ApplicationService,
		private formatCodeService: FormatCodeService,
		private modal: ModalService,
		private excelService: ExcelService,
		private pdfService: PDFService,
		private validationService: ValidationService,
		private msg: MessagesService,
		private router: Router,
		private Location: Location,
		private authguard: AuthGuard
	) {
	}
	ngOnInit(): void {
	}
	ExportarPDF(){
		if(this.parametros.razaoSocial == null){
			return this.modal.alerta("Campo 'Empresa' não pode estar vazio", "Alerta!");
		}
		this.applicationService.get(this.servico, this.parametros).subscribe((result: any) => {
			if(result == null){
				return this.modal.alerta("Nenhum registro encontrado para o filtro selecionado", "Atenção!");
			}
			this.lista = result;
			this.exibeRelatorio = true;
			this.lista;
			//this.dataInicio = new Date(this.lista.dataInicio).toISOString().slice(0, 9);
			console.log(this.lista.dataInicio);
			console.log(this.lista.dataFim);
			console.log(this.dataInicio);
			console.log(this.dataFim);
		});
	}

	LimparFiltros(){
		this.parametros.inscricaoCadastral = null;
		this.parametros.razaoSocial = null;
		this.parametros.dataInicio = null;
		this.parametros.dataFim = null;
	}


}
