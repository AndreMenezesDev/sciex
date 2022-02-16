import { Component, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { manterFabricanteVM } from '../../../view-model/ManterFabricanteVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';


@Component({
	selector: 'app-fabricante-formulario',
	templateUrl: './formulario.component.html'
})

export class ManterFabricanteFormularioComponent implements OnInit {
	path: string;
	titulo: string;
	tituloPanel: string;
	desabilitado: boolean;
	model: manterFabricanteVM = new manterFabricanteVM();
	servicoFabricante = 'Fabricante';
	servicoFabricanteGrid = 'FabricanteGrid';
	isVoltarVisible: boolean;
	isCancelarVisible: boolean;
	habilitarCampoCodigo: boolean;
	botaoCancelarVisivel: boolean;
	botaoVoltarVisivel: boolean;
	paisSelecionado: string;
	cadastrar: boolean;
	CNPJ: string;
	@ViewChild('pais') pais;
	@ViewChild('formulario') formulario;


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
		this.botaoCancelarVisivel = true;
		this.botaoVoltarVisivel = false;
		this.tituloPanel = 'Formulário';
		this.cadastrar = true;

		if (this.path == 'visualizar') {
			this.isVoltarVisible = true;
			this.isCancelarVisible = false;
			this.desabilitarTela();
			this.botaoCancelarVisivel = false;
			this.botaoVoltarVisivel = true;
			this.tituloPanel = 'Registros';
		}

		if (this.path == 'editar' || this.path == 'visualizar') {
			this.selecionar(this.route.snapshot.params['id']);
			this.habilitarCampoCodigo = true;
		}

		if (this.path == 'editar') {
			this.titulo = 'Alterar';
			this.botaoCancelarVisivel = true;
			this.botaoVoltarVisivel = false;
		} else {
			this.titulo = this.path[0].toUpperCase() + this.path.substr(1, this.path.length - 1);
		}
	}

	// adicionar o evento no componente de ação -- (blur)="onBlurEvent()"
	public onBlurEvent() {
		if (this.model.idFabricante !== undefined) {
			this.applicationService.get(this.servicoFabricante, this.model).subscribe((result: PagedItems) => {
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

	public selecionar(id: number) {
		if (!id) { return; }
		this.applicationService.get<manterFabricanteVM>(this.servicoFabricante, id).subscribe(result => {		
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
		this.applicationService.post<manterFabricanteVM>(this.servicoFabricante, this.model).subscribe(result => {
			this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO
				+ (this.titulo != 'Alterar' ? " <br/><br/> " + "Código do Fabricante: " + result.codigo : ""), "Sucesso", "/fabricante");
			this.model = result;
			
			if ( this.path != "editar")
				localStorage.clear();
		});
	}

	public cancelarOperacao() {
		this.modal.confirmacao(this.msg.CONFIRMAR_CONFIRMACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.router.navigate(['/fabricante']);
				}
			});

	}
}
