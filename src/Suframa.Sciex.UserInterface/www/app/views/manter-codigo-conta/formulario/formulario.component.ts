import { Component, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { manterCodigoContaVM } from '../../../view-model/ManterCodigoContaVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { isNullOrUndefined } from 'util';

@Component({
	selector: 'app-manter-codigo-conta-formulario',
	templateUrl: './formulario.component.html'
})

export class ManterCodigoContaFormularioComponent implements OnInit {
	parametros: manterCodigoContaVM = new manterCodigoContaVM();
	model: manterCodigoContaVM = new manterCodigoContaVM();
	path: string;
	titulo: string;
	tituloPanel: string;
	desabilitado: boolean;
	servicoCodigoConta = 'CodigoConta';
	servicoCodigoContaGrid = 'CodigoContaGrid';
	habilitarCampoCodigo: boolean;
	isCampoStatus: boolean;
	validar: boolean = false;
	codigo1: string;
	status: boolean;
	isCancelarVisible: boolean;
	isAtivoVisible: boolean;

	@ViewChild('formulario') formulario;
	@ViewChild('codigo') codigo;
	@ViewChild('statustf') statustf;

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
		this.model.status = 1;
	}

	public verificarRota() {
		this.habilitarCampoCodigo = false;
		this.tituloPanel = 'Formulário';
		this.isCancelarVisible = true;
		this.isCampoStatus = true;
		this.isAtivoVisible = true;

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
			this.isCampoStatus = false;
			this.isAtivoVisible = false;
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
		this.applicationService.get<manterCodigoContaVM>(this.servicoCodigoConta, id).subscribe(result => {
			this.model = result;
			this.codigo1 = this.model.codigo.toString();
			this.codigo.nativeElement.value = this.codigo1;

			//rn interface
			if (this.model.status == 0) {
				this.statustf.nativeElement.checked = false;
			} else {
				this.statustf.nativeElement.checked = true;
			}
			this.onBlurEvent();
		});
	}

	public salvar() {
		if (!this.validationService.form('formulario')) { return; }
		if (!this.formulario.valid) { return; }

		if (this.model.descricao.length != 0) {
			if (this.codigo1 != "" && this.model.descricao != "") {
				this.parametros.codigo = +this.codigo1;
				this.parametros.status = 2;
				if (this.statustf.nativeElement.checked == true) {
					this.model.status = 1;
					this.model.inativar = 0;
				} else {
					this.model.status = 0;
					this.model.inativar = 1;
				}

				this.applicationService.get(this.servicoCodigoContaGrid, this.parametros).subscribe((result: PagedItems) => {
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
									this.applicationService.get(this.servicoCodigoContaGrid, this.parametros).subscribe((result: PagedItems) => {
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
		this.applicationService.put<manterCodigoContaVM>(this.servicoCodigoConta, this.model).subscribe(result => {
			if (result.mensagemErro != null) {
				this.modal.alerta(this.msg.REGISTRO_JA_VINCULADO, "Alerta", "/manter-codigo-conta");
			}
			else {
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "/manter-codigo-conta");
				this.model = result;
			}
			if (this.path != "editar")
				localStorage.clear();
		});
	}

	public cancelarOperacao() {
		this.modal.confirmacao(this.msg.CONFIRMAR_CONFIRMACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.router.navigate(['/manter-codigo-conta']);
				}
			});
	}
}
