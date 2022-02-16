import { Component, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { manterUnidadeReceitaFederalVM } from '../../../view-model/ManterUnidadeReceitaFederalVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { isNullOrUndefined } from 'util';

@Component({
	selector: 'app-manter-unidade-receita-federal-formulario',
	templateUrl: './formulario.component.html'
})

export class ManterUnidadeReceitaFederalFormularioComponent implements OnInit {
	parametros: manterUnidadeReceitaFederalVM = new manterUnidadeReceitaFederalVM();
	path: string;
	titulo: string;
	tituloPanel: string;
	desabilitado: boolean;
	servicoUnidadeReceitaFederal = 'UnidadeReceitaFederal';
	servicoUnidadeReceitaFederalGrid = 'UnidadeReceitaFederalGrid';
	habilitarCampoCodigo: boolean;
	validar: boolean = false;
	model: manterUnidadeReceitaFederalVM = new manterUnidadeReceitaFederalVM();
	codigo1: string;
	isCancelarVisible: boolean;

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
					this.codigo1 = ("0000000" + this.codigo1).slice(-7);
				}
	}

	public desabilitarTela() {
		this.desabilitado = true;
	}

	public selecionar(id: number) {

		if (!id) { return; }
		this.applicationService.get<manterUnidadeReceitaFederalVM>(this.servicoUnidadeReceitaFederal, id).subscribe(result => {
			this.model = result;
			this.codigo1 = this.model.codigo.toString();
			this.codigo.nativeElement.value = this.codigo1;
			this.onBlurEvent();
		});
	}

	public salvar() {



		if (!this.validationService.form('formulario')) { return; }
		if (!this.formulario.valid) { return; }


		if (this.model.descricao.length != 0) {
			if (this.codigo1 != "" && this.model.descricao != "") {
				this.parametros.codigo = +this.codigo1;
				this.applicationService.get(this.servicoUnidadeReceitaFederalGrid, this.parametros).subscribe((result: PagedItems) => {
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
									this.applicationService.get(this.servicoUnidadeReceitaFederalGrid, this.parametros).subscribe((result: PagedItems) => {
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
		this.applicationService.put<manterUnidadeReceitaFederalVM>(this.servicoUnidadeReceitaFederal, this.model).subscribe(result => {
			this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "/unidadeReceitaFederal");
			this.model = result;
			if (this.path != "editar")
				localStorage.clear();
		});

	}

	public cancelarOperacao() {
		this.modal.confirmacao(this.msg.CONFIRMAR_CONFIRMACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.router.navigate(['/unidadeReceitaFederal']);
				}
			});
	}


}
