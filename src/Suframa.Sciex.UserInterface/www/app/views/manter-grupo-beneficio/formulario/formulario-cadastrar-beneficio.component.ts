import { Component, OnInit, Injectable, ViewChild, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from "@angular/forms";
import { PagedItems } from '../../../view-model/PagedItems';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ValidationService } from "../../../shared/services/validation.service";
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { TaxaGrupoBeneficioVM } from '../../../view-model/TaxaGrupoBeneficioVM';
import { forEach } from '@angular/router/src/utils/collection';
import { EnumPerfil } from '../../../shared/enums/EnumPerfil';

@Component({
	selector: 'app-cadastrar-beneficio',
	templateUrl: './formulario-cadastrar-beneficio.component.html'
})

@Injectable()
export class CadastrarBeneficioComponent implements OnInit {

    path: string;
    telaAtual: number;

	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	ocultarbotaocheck: boolean = false;
	ordenarGrid: boolean = true;
	inscricaoSuframa = '';
	razaoSocialEmpresa = '';
	isUsuarioImportador: boolean = false;
	desabilitado: boolean;

	maior100: boolean = false;
	percentNotNumber: boolean = false;

	tituloPanel: string;

	modelGrupoBeneficio: TaxaGrupoBeneficioVM = new TaxaGrupoBeneficioVM();
	servicoGrupoBeneficio = "Beneficio";
	selected: boolean = true;

	form: FormGroup;
	formPai : any;

	isModificouPesquisa: boolean = false;
	model: manterPliVM = new manterPliVM();
	isBuscaSalva: boolean = false;
	carregarGrid: boolean = false;
	opcaoStatus: number;
	isGeracaoDebito: boolean;
	isAnaliseVisual: boolean;
	isAguardandoProcessamento: boolean;
	isProcessado: boolean;
	ocultarBotaoReprocessar: boolean = true;

	custom = 'tipoBeneficio';

	@ViewChild('inscricaoCadastral') inscricaoCadastral;
	@ViewChild('empresa') empresa;
	@ViewChild('formulario') formulario;
    @ViewChild('descricao') descricao;
    @ViewChild('isencao') isencao;
	@ViewChild('reducao') reducao;
	@ViewChild('suspensao') suspensao;
    @ViewChild('percentualBeneficio') percentualBeneficio;
    @ViewChild('statustf') statustf;
	@ViewChild('textAmparoLegal') textAmparoLegal;

	constructor(
        private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
	) {
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.verificarRota();
	}

	ngOnInit(): void {
		this.isUsuarioImportador = false;

		this.ocultarBotaoReprocessar = true;

		// this.applicationService.get("UsuarioLogado").subscribe((result: any) => {
		// 	if (result && result.perfis.includes(EnumPerfil.pessoaJuridica)) {
		// 		this.isUsuarioImportador = true;
		// 		this.razaoSocialEmpresa = result.empresaRepresentadaRazaoSocial;
		// 		this.ocultarbotaocheck = true;
		// 		this.ocultarBotaoReprocessar = true;
		// 		this.inscricaoCadastral.nativeElement.value = result.usuInscricaoCadastral;
		// 		this.empresa.nativeElement.value = result.usuNomeEmpresaOuLogado;
		// 		this.parametros.razaoSocial = result.usuNomeEmpresaOuLogado;
		// 		this.parametros.inscricaoCadastral = result.usuInscricaoCadastral;
		// 		this.isModificouPesquisa = true
		// 	}

		// });

		if (this.model.statusPliSelecionado == undefined) {
			this.parametros.statusPliSelecionado = 0;
		}

	}

	onChangeSort($event) {
		this.grid.sort = $event;
	}

	onChangeSize($event) {
		this.grid.size = $event;
	}

	onChangePage($event) {
		this.grid.page = $event;
    }

    onChangeTipoSituacao(){

    }

	onChangeTipoDeBeneficio(){

    }

    public verificarRota() {

		this.tituloPanel = 'Cadastrar Benefício';
		this.telaAtual = this.route.snapshot.params['tela'];
	}

	cancelarOperacao(){

		this.modal.confirmacao(this.msg.CONFIRMAR_CONFIRMACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.router.navigate(['/grupo-beneficio']);
				}
			});
	}

	validar(){

		if( !(this.modelGrupoBeneficio.descricao)
		 ||  (this.isencao.nativeElement.checked != true  &&  this.reducao.nativeElement.checked != true && this.suspensao.nativeElement.checked != true)
		 ){
			this.modal.alerta(this.msg.CAMPO_OBRIGATORIO_NAO_INFORMADO_PREENCHIDO_INCORRETAMENTE, 'Alerta');
			return false;

		} else if(!this.percentualBeneficio.nativeElement.value){

			this.modal.alerta("Percentual inválido", 'Informação');
			return false;
		}
		else if(this.maior100){

			this.modal.alerta("O percentual não pode ser maior que 100%", 'Informação');
			this.maior100 = true;
			return false;
		} else if(this.percentNotNumber){

			this.modal.alerta("Percentual inválido", 'Informação');
		    this.percentNotNumber = true;
			return false;
		} else{

			this.statustf.nativeElement.checked ? this.modelGrupoBeneficio.statusBeneficio = 1 : this.modelGrupoBeneficio.statusBeneficio = 0;
			this.isencao.nativeElement.checked ? this.modelGrupoBeneficio.tipoBeneficio = 1 : this.reducao.nativeElement.checked ? this.modelGrupoBeneficio.tipoBeneficio = 2 : this.suspensao.nativeElement.checked ? this.modelGrupoBeneficio.tipoBeneficio = 3 : this.modelGrupoBeneficio.tipoBeneficio = 0;
			this.modelGrupoBeneficio.valorPercentualReducao = (this.modelGrupoBeneficio.valorPercentualReducao / 100) //calculando porcentagem

			this.modelGrupoBeneficio.dataCadastro = new Date();

			this.salvar();
		}

	}

	salvar(){

		this.applicationService.put<TaxaGrupoBeneficioVM>(this.servicoGrupoBeneficio, this.modelGrupoBeneficio).subscribe(result => {

			if (result) {

				this.limparCampos();
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "");
				this.router.navigate(['/grupo-beneficio']);
			}else{

				this.modal.alerta("Erro ao tentar salvar o Grupo de Benefício." , "Alerta");
				return false;
			}
		});
	}

	limparCampos(){
		this.descricao.nativeElement.value = '';
		this.textAmparoLegal.nativeElement.value = '';
		this.percentualBeneficio.nativeElement.value = '';
		this.isencao.nativeElement.checked = true;
		this.statustf.nativeElement.checked = false;
	}

	onBlurValidarPercentual(){

		if(this.percentualBeneficio.nativeElement.value>100 &&
		   this.percentualBeneficio.nativeElement.value)
		{

			this.modal.alerta("O percentual não pode ser maior que 100%", 'Informação');
			this.maior100 = true;
			return false;
		} else if(!(this.isNumber(this.percentualBeneficio.nativeElement.value)) &&
					this.percentualBeneficio.nativeElement.value){

			this.modal.alerta("Percentual inválido", 'Informação');
		    this.percentNotNumber = true;
			return false;
		} else {
			this.maior100 = false;
			this.percentNotNumber = false;
			this.modelGrupoBeneficio.valorPercentualReducao = parseFloat(this.replaceVirgula(this.percentualBeneficio.nativeElement.value));
			return true;
		}
	}

	replaceVirgula(value): string{
		return value.replace(',','.');
	}

	 isNumber(n: string) {
		let value = this.replaceVirgula(n);
		return !isNaN(parseFloat(value)) && isFinite(parseFloat(value));
	 // $.isNumeric('123'); // true
	 // $.isNumeric('asdasd123'); // false
	}

}
