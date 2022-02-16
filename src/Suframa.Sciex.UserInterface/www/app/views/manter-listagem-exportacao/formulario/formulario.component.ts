import { Component, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';

@Component({
	selector: 'app-manter-le-produto-formulario',
	templateUrl: './formulario.component.html'
})

export class ManterLEProdutoFormularioComponent implements OnInit {
	parametros: any;
	path: string;
	titulo: string;
	tituloPanel: string;
	desabilitado: boolean;
	somenteLeitura: boolean;
	validar: boolean = false;
	model: any;
	servico = "LEProduto";

	@ViewChild('formulario') formulario;
	
	@ViewChild('codigoProduto') codigoProduto;
	@ViewChild('codigoTipoProduto') codigoTipoProduto;
	@ViewChild('codigoNCM') codigoNCM;
	@ViewChild('unidadeMedida') unidadeMedida;
	@ViewChild('modeloProduto') modeloProduto;



	constructor(
		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private arrayService: ArrayService,
		private modal: ModalService,
		private validationService: ValidationService,
		private router: Router
	) {
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.verificarRota();
	}

	ngOnInit() {
	
	}

	public verificarRota() {
		this.tituloPanel = 'Formulário';
		this.parametros = {};
		this.model = {};

		if (this.path == 'cadastrar') {
			this.titulo = 'Cadastrar';
					
		} else {
			this.titulo = this.path[0].toUpperCase() + this.path.substr(1, this.path.length - 1);
		}
	}

	public desabilitarTela() {
		this.desabilitado = true;
	}

	public salvar() {
		this.validarIncluir();

		if (!this.validationService.form('formulario')) { return; }
		if (!this.formulario.valid) { return; }

		this.applicationService.put(this.servico, this.parametros).subscribe((result: any) => {
			if (result.mensagemErro != "" && result.mensagemErro != null) {
				this.modal.alerta(this.msg.PRODUTO_JA_ADICIONADO, "Informação", "");
			} else {
				this.modal.resposta(result.mensagem, "Informação", "/manter-listagem-exportacao");
			}
		});
	}

	validarIncluir() {
		// this.codigoProduto.valorInput.nativeElement.setCustomValidity('');
		// this.codigoTipoProduto.valorInput.nativeElement.setCustomValidity('');
		// this.codigoNCM.valorInput.nativeElement.setCustomValidity('');
		// this.unidadeMedida.valorInput.nativeElement.setCustomValidity('');

		if (this.codigoProduto.valorInput.nativeElement.value.trim() == "") {
			this.modal.alerta("Preencha o campo Código Produto Suframa", "Informação");
			//this.codigoProduto.valorInput.nativeElement.setCustomValidity('Preencha este campo.');
		}
		else if (this.codigoTipoProduto.valorInput.nativeElement.value.trim() == "") {
			this.modal.alerta("Preencha o campo Tipo do Produto", "Informação");
			//this.codigoTipoProduto.valorInput.nativeElement.setCustomValidity('Preencha este campo.');
		} else if (this.codigoNCM.valorInput.nativeElement.value.trim() == "" ) {
			this.modal.alerta("Preencha o campo NCM", "Informação");
			//this.codigoNCM.valorInput.nativeElement.setCustomValidity('Preencha este campo.');
		}
		else if (this.unidadeMedida.valorInput.nativeElement.value.trim() == "") {
			this.modal.alerta("Preencha o campo Unidade de Medida", "Informação");
			//this.unidadeMedida.valorInput.nativeElement.setCustomValidity('Preencha este campo.')
		}
		else if (this.modeloProduto.nativeElement.value.trim() == "") {
			this.modal.alerta("Preencha o campo Modelo do Produto", "Informação");
		}

	}

	public cancelarOperacao() {
		this.modal.confirmacao(this.msg.CONFIRMAR_CONFIRMACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.router.navigate(['/manter-listagem-exportacao']);
				}
			});
	}


}
