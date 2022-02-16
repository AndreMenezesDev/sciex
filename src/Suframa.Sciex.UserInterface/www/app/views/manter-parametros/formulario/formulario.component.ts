import { Component, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { manterParametrosVM } from '../../../view-model/ManterParametrosVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { FormArray, FormControl, Validators, FormGroup } from '@angular/forms';
import { viewClassName } from '@angular/compiler';

declare var $: any;

@Component({
	selector: 'app-manter-parametros-formulario',
	templateUrl: './formulario.component.html'
})

export class ManterParametrosFormularioComponent implements OnInit {
	path: string;
	titulo: string;
	tituloPanel: string;
	desabilitado: boolean;
	servicoParametros = 'Parametros';
	servicoParametrosGrid = 'ParametrosGrid';
	habilitarCampoCodigo: boolean;
	isVoltarVisible: boolean;
	isCancelarVisible: boolean;
	isFornecedorVisible: boolean;
	isFabricanteVisible: boolean;
	isPaisOrigemVisible: boolean;
	isModalidadeVisible: boolean;


	isLimiteVisible: boolean;
	isInstituicaoVisible: boolean;
	isMotivoVisible: boolean;
	habilitarMaxHeight: boolean;
	botaoCancelarVisivel: boolean;
	botaoVoltarVisivel: boolean;
	model: manterParametrosVM = new manterParametrosVM();
	moedaSelecionada: string;
	valorAba: any;
	CNPJ: string;

	@ViewChild('formulario') formulario;
	@ViewChild('fornecedor') fornecedor;
	@ViewChild('abaDadosMercadoria') abaDadosMercadoria;
	@ViewChild('abaNegociacao') abaNegociacao;
	@ViewChild('botaoSalvar') botaoSalvar;
	@ViewChild('pais') pais;
	@ViewChild('paisOrigemFabricante') paisOrigemFabricante;
	

	rForm: FormGroup;

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

	ngOnInit(): void {
		this.isFornecedorVisible = true;
		this.isFabricanteVisible = true;
		this.isPaisOrigemVisible = true;
		this.isModalidadeVisible = true;
		this.isLimiteVisible = true;
		this.isInstituicaoVisible = true;
		this.isMotivoVisible = true;
		this.habilitarMaxHeight = true;

		if (this.model.tipoFornecedor == null)
			this.model.tipoFornecedor = -1;


		this.applicationService.get("UsuarioLogado", { cnpj: true }).subscribe((result: any) => {
			if (result != null) {
				this.CNPJ = result;
				this.model.CPNJImportador = this.CNPJ;
			}
			
		});
	}


	public verificarRota() {
		this.isVoltarVisible = false;
		this.isCancelarVisible = true;
		this.botaoCancelarVisivel = true;
		this.tituloPanel = 'Formulário';

		if (this.path == 'visualizar') {
			this.isVoltarVisible = true;
			this.isCancelarVisible = false;
			this.botaoCancelarVisivel = false;
			this.botaoVoltarVisivel = true;
			this.tituloPanel = 'Registros';
			this.desabilitarTela();
		}

		if (this.path == 'editar' || this.path == 'visualizar') {
			this.selecionar(this.route.snapshot.params['id']);
		}

		if (this.path == 'editar') {
			this.titulo = 'Alterar';


		} else {
			this.titulo = this.path[0].toUpperCase() + this.path.substr(1, this.path.length - 1);
		}
	}

	public desabilitarTela() {
		this.desabilitado = true;
	}

	// adicionar o evento no componente de ação -- (blur)="onBlurEvent()"
	public onBlurEvent() {
		if (this.model.codigoPaisOrigemFabricante !== undefined) {
			this.applicationService.get(this.servicoParametrosGrid, this.model).subscribe((result: PagedItems) => {
				if (result.total > 0) {
					this.model = result.items[0];
					this.habilitarCampoCodigo = true;
					this.titulo = 'Alterar';
				}
			});
		}
	}

	public onChangeTipoFornecedor() {

		if (this.model.tipoFornecedor == 0) {
			this.isFornecedorVisible = true;
			this.isFabricanteVisible = true;
			this.isPaisOrigemVisible = true;
		} else if (this.model.tipoFornecedor == 1) {
			this.isFornecedorVisible = false;
			this.isFabricanteVisible = true;
			this.isPaisOrigemVisible = true;
		} else if (this.model.tipoFornecedor == 2) {
			this.isFornecedorVisible = false;
			this.isFabricanteVisible = false;
			this.isPaisOrigemVisible = true;
		} else if (this.model.tipoFornecedor == 3) {
			this.isFornecedorVisible = true;
			this.isFabricanteVisible = true;
			this.isPaisOrigemVisible = false;
		} else {
			this.isFornecedorVisible = true;
			this.isFabricanteVisible = true;
			this.isPaisOrigemVisible = true;
		}
	}

	public onChangeCoberturaCambial() {
		if (this.model.tipoCorbeturaCambial == 1) {
			this.isModalidadeVisible = false;
			this.isLimiteVisible = false;
			this.isInstituicaoVisible = true;
			this.isMotivoVisible = true;
		} else if (this.model.tipoCorbeturaCambial == 2) {
			this.isModalidadeVisible = false;
			this.isLimiteVisible = true;
			this.isInstituicaoVisible = true;
			this.isMotivoVisible = true;
		} else if (this.model.tipoCorbeturaCambial == 3) {
			this.isModalidadeVisible = true;
			this.isLimiteVisible = true;
			this.isInstituicaoVisible = false;
			this.isMotivoVisible = true;
		} else if (this.model.tipoCorbeturaCambial == 4) {
			this.isModalidadeVisible = true;
			this.isLimiteVisible = true;
			this.isInstituicaoVisible = true;
			this.isMotivoVisible = false;
		} else {
			this.isModalidadeVisible = true;
			this.isLimiteVisible = true;
			this.isInstituicaoVisible = true;
			this.isMotivoVisible = false;
		}
	}

	public onClickEvent() {
		this.habilitarMaxHeight = false;
	}

	public selecionar(id: number) {
		if (!id) { return; }
		this.applicationService.get<manterParametrosVM>(this.servicoParametros, id).subscribe(result => {
			this.model = result;
			this.onChangeTipoFornecedor();
			this.onChangeCoberturaCambial();
		});
	}

	validarFormulario() {

		var userForm = new FormGroup({
			fornecedor: new FormControl(this.model.idFornecedor, !this.isFornecedorVisible ? Validators.required: null),
			fabricante: new FormControl(this.model.idFabricante, !this.isFabricanteVisible ? Validators.required : null),
			modalidadePagamento: new FormControl(this.model.idModalidadePagamento, !this.isModalidadeVisible ? Validators.required : null),
			limitePagamento: new FormControl(this.model.quantidadeDiaLimite, !this.isLimiteVisible ? Validators.required : null),
			instituicaoFinanceira: new FormControl(this.model.idInstituicaoFinanceira, !this.isInstituicaoVisible ? Validators.required : null),
			motivo: new FormControl(this.model.idMotivo, !this.isMotivoVisible ? Validators.required : null),
			paisOrigemFabricante: new FormControl(this.model.codigoPaisOrigemFabricante, !this.isPaisOrigemVisible ? Validators.required : null)
		});

		if (this.model.tipoFornecedor == 1 || this.model.tipoFornecedor == 2) {
			if (!userForm.get('fornecedor').valid) {				
				return this.validarAbaMercadoria();
			}
		}

		if (this.model.tipoFornecedor == 2 || this.model.tipoFornecedor == 3) {
			if (!userForm.get('fabricante').valid) {				
				return this.validarAbaMercadoria();
			}

			if (this.model.tipoFornecedor == 3) {
				if (!userForm.get('paisOrigemFabricante').valid) {
					return this.validarAbaMercadoria();
				}
			}
		}
		 
		if (this.model.tipoCorbeturaCambial == 1 || this.model.tipoCorbeturaCambial == 2) {

			if (!userForm.get('modalidadePagamento').valid) {				
				return this.validarAbaNegociacao();
			}

			if (this.model.tipoCorbeturaCambial == 1) {
				if (!userForm.get('limitePagamento').valid) {					
					return this.validarAbaNegociacao();
				}
			}
		}
		if (this.model.tipoCorbeturaCambial == 3) {
			if (!userForm.get('instituicaoFinanceira').valid) {				
				return this.validarAbaNegociacao();
			}
		}
		if (this.model.tipoCorbeturaCambial == 4) {
			if (!userForm.get('motivo').valid) {				
				return this.validarAbaNegociacao();
			}
		}

		return true;
	}

	validarAbaNegociacao() {
		this.abaNegociacao.nativeElement.click();
		this.botaoSalvar.nativeElement.click();


		setTimeout(() => {
			if (!this.validationService.form('formulario')) { return false; }
		}, 600);
	}

	validarAbaMercadoria() {
		this.abaDadosMercadoria.nativeElement.click();
		this.botaoSalvar.nativeElement.click();

		setTimeout(() => {
			if (!this.validationService.form('formulario')) { return false; }
		}, 600);
	}

	public salvar() {
		if (!this.validarFormulario()) {
			return;
		}
		if (!this.validationService.form('formulario')) { return; }
		if (!this.formulario.valid) { return; }

		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.salvarRegistro();
				}
			});

		
	}

	private salvarRegistro() {
		
		this.model.CPNJImportador = this.CNPJ;
		console.log(this.model.CPNJImportador);
		this.model.codigoPaisOrigemFabricante == undefined ?  " " : this.model.descricaoPaisOrigemFabricante = this.paisOrigemFabricante.valorInput.nativeElement.value.split("|")[1];
		this.model.codigoPaiMercadoria == undefined ? " " : this.model.descricaoPaiMercadoria = this.pais.valorInput.nativeElement.value.split("|")[1];
		console.log(this.model);
		this.applicationService.put<manterParametrosVM>(this.servicoParametros, this.model).subscribe(result => {
			console.log(this.model);
			$(".modal-body").scrollTop(0);
			this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "/parametros");
			this.model = result;
			if (this.path != "editar")
				localStorage.clear(); 
		});
		
	}

	public cancelarOperacao() {
		this.modal.confirmacao(this.msg.CONFIRMAR_CONFIRMACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					$(".modal-body").scrollTop(0);
					this.router.navigate(['/parametros']);
				}
			});
	}
}
