import { Component, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { ManterFundamentoLegalVM } from '../../../view-model/ManterFundamentoLegalVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { isNullOrUndefined } from 'util';

@Component({
	selector: 'app-manter-fundamento-legal-formulario',
	templateUrl: './formulario.component.html'
})

export class ManterFundamentoLegalFormularioComponent implements OnInit {
	parametros: ManterFundamentoLegalVM = new ManterFundamentoLegalVM();
	path: string;
	titulo: string;
	tituloPanel: string;
	desabilitado: boolean;
	servicoFundamentoLegal = 'FundamentoLegal';
	servicoFundamentoLegalGrid = 'FundamentoLegalGrid';
	habilitarCampoCodigo: boolean;
	validar: boolean = false;
	model: ManterFundamentoLegalVM = new ManterFundamentoLegalVM();
	codigo1: string;
	isCancelarVisible: boolean;
	listaBeneficios = [];

	@ViewChild('formulario') formulario;
	@ViewChild('codigo') codigo;
	//@ViewChild('descricao') descricao;



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
		this.inicializarDadosAreabeneficios();
	}

	public verificarRota() {
		this.habilitarCampoCodigo = false;
		this.tituloPanel = 'Formulário';
		this.isCancelarVisible = true;

		if (this.path == 'visualizar') {
			this.desabilitarTela();
			this.tituloPanel = 'Registros';
			this.isCancelarVisible = false;
		}

		if (this.path == 'editar' || this.path == 'visualizar') {
			this.selecionar(this.route.snapshot.params['id']);
			this.habilitarCampoCodigo = true;
		}

		if (this.path == 'editar') {
			this.titulo = 'Alterar';

		} else {
			this.titulo = this.path[0].toUpperCase() + this.path.substr(1, this.path.length - 1);
		}
	}


	public onBlurEvent() {

		if (this.codigo.nativeElement.value != undefined)
			if (this.codigo.nativeElement.value.length != undefined)
				if (this.codigo.nativeElement.value.length > 0) {

					this.codigo1 = this.codigo.nativeElement.value;
					this.codigo1 = ("00" + this.codigo1).slice(-2);
				}
	}

	public desabilitarTela() {
		this.desabilitado = true;
	}

	public selecionar(id: number) {

		if (!id) { return; }
		this.applicationService.get<ManterFundamentoLegalVM>(this.servicoFundamentoLegal, id).subscribe(result => {
			this.model = result;
			this.codigo1 = this.model.codigo.toString();
			this.codigo.nativeElement.value = this.codigo1;
			this.onBlurEvent();
		});
	}

	public salvar() {

		if (this.model.tipoAreaBeneficio == 0) {
			this.model.tipoAreaBeneficio == null;
		}

		if (!this.validationService.form('formulario')) { return; }
		if (!this.formulario.valid) { return; }

		if (this.model.descricao.length != 0) {
			if (this.codigo1 != "" && this.model.descricao != "") {
				this.parametros.codigo = +this.codigo1;
				this.applicationService.get(this.servicoFundamentoLegalGrid, this.parametros).subscribe((result: PagedItems) => {
					if (result.total > 0 && this.habilitarCampoCodigo == false) {
						this.modal.confirmacao(this.msg.O_CODIGO + " " + this.codigo1 + " " + this.msg.JA_CADASTRADO, 'Confirmação', '')
							.subscribe(isConfirmado => {
								if (isConfirmado) {
									this.model = result.items[0];
									this.habilitarCampoCodigo = true;
									this.titulo = 'Alterar';
								}
							});

					} else {

						this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
							.subscribe(isConfirmado => {
								if (isConfirmado) {
									this.applicationService.get(this.servicoFundamentoLegalGrid, this.parametros).subscribe((result: PagedItems) => {
										if (result.total > 0 && this.habilitarCampoCodigo == false) {
											this.modal.confirmacao(this.msg.O_CODIGO + " " + this.codigo1 + " " + this.msg.JA_CADASTRADO, 'Confirmação', '')
												.subscribe(isConfirmado => {
													if (isConfirmado) {
														this.model = result.items[0];
														this.habilitarCampoCodigo = true;
														this.titulo = 'Alterar';
													}
												});
										}
										else {
											this.salvarRegistro();
										}
									});
								}
							});
					}

				});
			}

		}
	}

	private salvarRegistro() {

		this.model.codigo = + this.codigo1;
		this.applicationService.put<ManterFundamentoLegalVM>(this.servicoFundamentoLegal, this.model).subscribe(result => {
			this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "/fundamento-legal");
			this.model = result;
			if (this.path != "editar")
				localStorage.clear();
		});

	}

	public cancelarOperacao() {
		this.modal.confirmacao(this.msg.CONFIRMAR_CONFIRMACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.router.navigate(['/fundamento-legal']);
				}
			});
	}

	inicializarDadosAreabeneficios() {
		this.listaBeneficios = [
			{ idAreaBeneficio: 0, descricao: "Selecione uma opção" },
			{ idAreaBeneficio: 1, descricao: "ZFM – Zona Franca de Manaus" },
			{ idAreaBeneficio: 2, descricao: "ALC – Área de Livre Comércio" },
			{ idAreaBeneficio: 3, descricao: "AO – Amazônia Ocidental" }
		];

		this.model.tipoAreaBeneficio = 0;
	}


}
