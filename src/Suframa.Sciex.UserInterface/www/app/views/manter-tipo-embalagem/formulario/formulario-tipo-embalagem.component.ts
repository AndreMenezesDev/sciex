import { Component, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { ApplicationService } from "../../../shared/services/application.service";
import { ArrayService } from "../../../shared/services/array.service";
import { MessagesService } from "../../../shared/services/messages.service";
import { ModalService } from "../../../shared/services/modal.service";
import { ValidationService } from "../../../shared/services/validation.service";
import { manterViaTransporteVM } from "../../../view-model/ManterViaTransporteVM";
import { PagedItems } from "../../../view-model/PagedItems";


@Component({
	selector: 'app-manter-formulario-tiop-embalagem',
	templateUrl: './formulario-tipo-embalagem.component.html'
})

export class ManteTipoEmbalagemFormularioComponent implements OnInit {

	parametros: any = {};
	objetoCodigoExistente: any = {};
	pesquisouCodigoExistente: boolean = false;

	path: string;
	titulo: string;
	tituloPanel: string;
	desabilitado: boolean;
	servico = 'TipoEmbalagem';
	//servicoViaTransporteGrid = 'ViaTransporteGrid';
	habilitarCampoCodigo: boolean;
	validar: boolean = false;
	codigo1: string;
	isCancelarVisible: boolean;
	isCampoStatus: boolean;
	status: boolean;
	isAtivoVisible: boolean;

	@ViewChild('formulario') formulario;
	@ViewChild('codigo') codigo;
	@ViewChild('descricao') descricao;
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

	ngOnInit() { }
    
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
		} 
		else {
			this.path === 'novo' ? 
				this.titulo = 'Cadastrar' : 
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
		this.applicationService.get(this.servico, id).subscribe((result : any) => {
			this.parametros = result;
			if (this.parametros.status == 0) {
				this.statustf.nativeElement.checked = false;
			} else {
				this.statustf.nativeElement.checked = true;
			}
			this.onBlurEvent();
		});
	}

	public cancelarOperacao() {
		this.modal.confirmacao(this.msg.CONFIRMAR_CONFIRMACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.router.navigate(['/manter-tipo-embalagem']);
				}
		});
	}

	public salvar() {
		if (!this.validationService.form('formulario')) { return; }
		if (!this.formulario.valid) { return; }

		this.parametros.status ? this.parametros.status = 1 : this.parametros.status = 0;
		this.parametros.codigo = Number(this.parametros.codigo);

		if(this.path == 'novo' && !this.pesquisouCodigoExistente){
			this.verificarCodigoExistente(Number(this.parametros.codigo));
		} else {
			this.salvarRegistro();
		}		
	}

	salvarRegistro() {
		this.applicationService.post(this.servico, this.parametros).subscribe(result => {
			if (result == 1) {
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Alerta", "/manter-tipo-embalagem");
			}
		});
	}

	verificarCodigoExistente(codigo:number){
		var objeto : any = {};
		objeto.codigo = codigo;
		this.applicationService.get(this.servico, objeto).subscribe((result : any) => {
			if(result) {
				this.modal.confirmacao('O código '+codigo+' já foi cadastrado. Deseja alterar o registro?', '', '')
				.subscribe(isConfirmado => {
					if (isConfirmado) {
						this.pesquisouCodigoExistente = true;
						this.parametros = result;	
					}
				});
			} else {
				this.salvarRegistro();
			}
		});
	}

}
