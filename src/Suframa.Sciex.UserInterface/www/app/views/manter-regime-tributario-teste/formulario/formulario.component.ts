import { Component, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'toastr-ng2';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { PagedItems } from '../../../view-model/PagedItems';
import { manterRegimeTributarioTesteVM } from '../../../view-model/ManterRegimeTributarioTesteVM';

@Component({
	selector: 'app-manter-regime-tributario-teste-formulario',
	templateUrl: './formulario.component.html'
})

export class ManterRegimeTributarioTesteFormularioComponent {
	path: string;
	titulo: string;
	tituloPanel: string;
	desabilitado: boolean;
	model: manterRegimeTributarioTesteVM = new manterRegimeTributarioTesteVM();
	servicoRegimeTributarioTeste = 'RegimeTributarioTeste';
	servicoRegimeTributarioTesteGrid = 'RegimeTributarioTesteGrid';
	habilitarCampoCodigo: boolean;

	@ViewChild('formulario') formulario;
	@ViewChild('Codigo') codigo;
	constructor(
		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private arrayService: ArrayService,
		private toastrService: ToastrService,
		private modal: ModalService,
		private validationService: ValidationService
	) {
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.verificarRota();
	}

	public verificarRota() {
		this.tituloPanel = 'Formulário';

		if (this.path == 'visualizar') {
			this.desabilitarTela();
			this.tituloPanel = 'Registros';
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

	// adicionar o evento no componente de ação -- (blur)="onBlurEvent()"
	public onBlurEvent() {
		if (this.model.codigo !== undefined) {
			this.applicationService.get(this.servicoRegimeTributarioTesteGrid, this.model).subscribe((result: PagedItems) => {

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
		this.applicationService.get<manterRegimeTributarioTesteVM>(this.servicoRegimeTributarioTeste, id).subscribe(result => {
			this.model = result;
		});
	}

	public salvar() {
		this.codigo.nativeElement.setCustomValidity('');
		if (this.codigo.nativeElement.value == 'A') {
			this.codigo.nativeElement.setCustomValidity("Não pode ser igual a A!");
		}

		if (!this.validationService.form('formulario')) { return; }

		if (!this.formulario.valid) { return; }


		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '','')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.salvarRegistro();
				}
			});
	}
	private salvarRegistro() {
		this.applicationService.put<manterRegimeTributarioTesteVM>(this.servicoRegimeTributarioTeste, this.model).subscribe(result => {
			this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "/regime-tributario-teste");
			this.model = result;
		});

	}
}
