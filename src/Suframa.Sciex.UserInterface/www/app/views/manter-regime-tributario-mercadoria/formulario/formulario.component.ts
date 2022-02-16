import { Component, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { manterRegimeTributarioMercadoriaVM } from '../../../view-model/ManterRegimeTributarioMercadoriaVM';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { isNullOrUndefined } from 'util';

@Component({
	selector: 'app-manter-regime-tributario-mercadoria-formulario',
	templateUrl: './formulario.component.html'
})

export class ManterRegimeTributarioMercadoriaFormularioComponent implements OnInit {
	parametros: manterRegimeTributarioMercadoriaVM = new manterRegimeTributarioMercadoriaVM();
	path: string;
	titulo: string;
	tituloPanel: string;
	desabilitado: boolean;
	servico = 'RegimeTributarioMercadoria';
	servicoGrid = 'RegimeTributarioMercadoriaGrid';
	habilitarCampoCodigo: boolean;
	isCampoStatus: boolean;
	validar: boolean = false;
	model: manterRegimeTributarioMercadoriaVM = new manterRegimeTributarioMercadoriaVM();
	codigo1: string;
	status: boolean;
	isCancelarVisible: boolean;
	Uf: string;

	@ViewChild('formulario') formulario;
	@ViewChild('codigoMunicipio') codigoMunicipio;
	@ViewChild('dataInicioVigencia') dataInicioVigencia;

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
	}

	public capturarUF(UF: any) {
		if (UF != null || UF != undefined) {
			this.Uf = UF;
			this.model.UF = this.Uf;
		}
	}

	public onBlurEvent() {
		
	}

	public desabilitarTela() {
		this.desabilitado = true;
	}

	public limpar() {
		this.codigoMunicipio.clearInput = true;
	}

	public validarData() {

		this.dataInicioVigencia.nativeElement.setCustomValidity('');

		if (this.model.dataInicioVigencia != undefined) {

			if (this.dataInicioVigencia.nativeElement.value.length < 10) {
				this.dataInicioVigencia.nativeElement.setCustomValidity('Campo inválido');
			}

			if (new Date(this.model.dataInicioVigencia) <= new Date()) {
				this.dataInicioVigencia.nativeElement.setCustomValidity('A data da vigência deverá ser superior à data atual.');
			}
		}

	}

	public salvar() {

		this.model.codigoDoMunicipio = this.codigoMunicipio.valorSelecionado;
		this.validarData();
		this.model.descricaoMunicipio = this.codigoMunicipio.valorInput.nativeElement.value.split("|")[1];
		this.model.codigoDoMunicipio = this.codigoMunicipio.valorInput.nativeElement.value.split("|")[0];

	
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
		this.applicationService.put<manterRegimeTributarioMercadoriaVM>(this.servico, this.model).subscribe(result => {
			if (result.mensagemErro != null) { 
				this.modal.alerta(this.msg.REGISTRO_JA_CADASTRADO, "Alerta", "");
				
			}

			else {
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "/manter-regime-tributario-mercadoria");
				//this.model = result;
			}
			//this.model = result;
			if (this.path != "editar")
				localStorage.clear();
		});

	}

	public cancelarOperacao() {
		this.modal.confirmacao(this.msg.CONFIRMAR_CONFIRMACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.router.navigate(['/manter-regime-tributario-mercadoria']);
				}
			});
	}
}
