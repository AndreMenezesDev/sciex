import { Component, ViewChild, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { manterFornecedorVM } from '../../../view-model/manterFornecedorVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';



@Component({
	selector: 'app-fornecedor-formulario',
	templateUrl: './formulario.component.html',
})

export class ManterFornecedorFormularioComponent implements OnInit{
	path: string;
	titulo: string;
	tituloPanel: string;
	desabilitado: boolean;
	model: manterFornecedorVM = new manterFornecedorVM();
	servicoFornecedor = 'Fornecedor';
	servicoFornecedorGrid = 'FornecedorGrid';
	isVoltarVisible: boolean;
	isCancelarVisible: boolean;
	habilitarCampoCodigo: boolean;
	habilitarCampoPais: boolean;
	botaoCancelarVisivel: boolean;
	botaoVoltarVisivel: boolean;
	botaoVisivel: boolean;
	paisSelecionado: string;
	@ViewChild('pais') pais;
	@ViewChild("formulario") formulario;
	CNPJ: string;

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
		this.applicationService.get("UsuarioLogado", { cnpj: true }).subscribe((result: any) => {
			if (result != null)
				this.CNPJ = result;
		});
	}


	public verificarRota() {
		this.isVoltarVisible = false;
		this.isCancelarVisible = true;
		this.habilitarCampoCodigo = false;
		this.habilitarCampoPais = false;
		this.botaoCancelarVisivel = true;
		this.tituloPanel = 'Formulário';

		if (this.path == 'visualizar') {
			this.desabilitarTela();
			this.tituloPanel = 'Registros';
			this.isVoltarVisible = true;
			this.isCancelarVisible = false;
			this.habilitarCampoPais = true;
			this.botaoCancelarVisivel = false;
			this.botaoVoltarVisivel = true;
		}

		if (this.path == 'editar' || this.path == 'visualizar') {
			this.selecionar(this.route.snapshot.params['id']);
			this.habilitarCampoCodigo = true;
			this.tituloPanel = "Formulário"
		}

		if (this.path == 'editar') {
			this.titulo = 'Alterar';
			this.botaoVisivel = true;
		} else {
			this.titulo = this.path[0].toUpperCase() + this.path.substr(1, this.path.length - 1);
			this.botaoVisivel = true;
		}
	}

	// adicionar o evento no componente de ação -- (blur)="onBlurEvent()"
	public onBlurEvent() {
		if (this.model.idFornecedor !== undefined) {
			this.applicationService.get(this.servicoFornecedorGrid, this.model).subscribe((result: PagedItems) => {
				if (result.total > 0) {
					this.model = result.items[0];
					this.habilitarCampoCodigo = true;
					this.titulo = 'Alterar';
				}
			});
		}
	}

	public desabilitarTela() {
		this.desabilitado = true;
	}

	public cancelarOperacao() {
		this.modal.confirmacao(this.msg.CONFIRMAR_CONFIRMACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.router.navigate(['/fornecedor']);
				}
			});
	}

	public selecionar(id: number) {
		if (!id) { return; }
		this.applicationService.get<manterFornecedorVM>(this.servicoFornecedor, id).subscribe(result => {
			this.model = result;
		});
	}

	public salvar() {
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
		this.model.CNPJImportador = this.CNPJ;
		this.model.descricaoPais = this.pais.valorInput.nativeElement.value.split("|")[1];
		this.applicationService.put<manterFornecedorVM>(this.servicoFornecedor, this.model).subscribe(result => {
			this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO
				+ (this.titulo != 'Alterar' ? " <br/><br/> " + "Código do Fornecedor: " + result.codigo : ""), "Sucesso", "/fornecedor");
			this.model = result;
			if (this.path != "editar")
				localStorage.clear();
		});
	}


}
