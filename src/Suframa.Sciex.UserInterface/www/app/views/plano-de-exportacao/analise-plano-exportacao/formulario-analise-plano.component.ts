import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { Location } from '@angular/common';

declare var $: any;

@Component({
	selector: 'app-analisar-formulario-plano',
	templateUrl: './formulario-analise-plano.component.html'
})

export class AnalisePlanoFormularioPlanoComponent implements OnInit {
	path: string;
	servico = "PlanoExportacao";
	servicoProduto = "PEProduto"
	servicoAnexo = "PlanoExportacaoAnexo"
	model: any = {};
	modelProduto: any = {};
	codigo1: string;
	parametros: any = {};
	listaProdutos: any;

	@ViewChild('arquivo') arquivo;
	ocultarInputAnexo = false;
	limiteArquivo = 10485760; // 10MB
	temArquivo = false;
	filetype: string;
	filesize: number;
	types = ['application/x-zip-compressed', 'application/zip', 'application/pdf'];
	visualizar: boolean;

	constructor(
		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private modal: ModalService,
		private validationService: ValidationService,
		private Location: Location,
	) {
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
	}

	ngOnInit() {
		this.verificarRota();

		$(document).ready(function () {
			$("#collapse-init").click(function () {
				$('.accordion-toggle').collapse('hide');
				$('.panel-collapse').collapse('hide');
			});
		});
	}

	public verificarRota() {
		this.temArquivo = false;
		this.ocultarInputAnexo = false;

		if (this.path == 'analisar-info-plano') {
			this.visualizar = false;
		}else if (this.path == 'visualizar-info-plano') {
			this.visualizar = true;
		}

		this.selecionar(this.route.snapshot.params['id']);

		
	}
	public selecionar(id: number) {
		if (!id) { return; }
		this.listaProdutos = null;
		this.applicationService.get(this.servico, id).subscribe((result: any) => {

			this.model = result;

			if (result.listaAnexos.length > 0 && result.listaAnexos[0].anexo != "") {
				this.temArquivo = true;
				this.ocultarInputAnexo = true;
			}

			this.listaProdutos = result.listaPEProdutos;
		});
	}

	voltar() {
		sessionStorage.setItem('sameScreen','plano-de-exportacao');
		this.Location.back();
	}

	downloadAnexo() {
		try {
			const hashPDF = this.model.listaAnexos[0].anexo;
			const linkSource = 'data:' + 'application/pdf' + ';base64,' + hashPDF;
			const downloadLink = document.createElement('a');
			const fileName = this.model.listaAnexos[0].nomeArquivo;

			document.body.appendChild(downloadLink);

			downloadLink.href = linkSource;
			downloadLink.download = fileName;

			downloadLink.target = '_self';

			downloadLink.click();
		} catch (err) {
			this.modal.alerta('Erro ao baixar arquivo.', 'Informação');
		}

	}

}
