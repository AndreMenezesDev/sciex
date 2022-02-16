import { Component, OnInit, Injectable, ViewChild, Output, EventEmitter } from '@angular/core';
import { PagedItems } from '../../../view-model/PagedItems';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ValidationService } from "../../../shared/services/validation.service";
import { forEach } from '@angular/router/src/utils/collection';
import { EnumPerfil } from '../../../shared/enums/EnumPerfil';
import { TaxaGrupoBeneficioVM } from '../../../view-model/TaxaGrupoBeneficioVM';

@Component({
	selector: 'app-alterar-beneficio',
	templateUrl: './formulario-alterar-beneficio.component.html'
})

@Injectable()
export class  AlterarBeneficioComponent implements OnInit {

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

    tituloPanel: string;

    @ViewChild('inscricaoCadastral') inscricaoCadastral;
	@ViewChild('inscricaoCadastral') codigo;
	@ViewChild('formulario') formulario;
    @ViewChild('empresa') empresa;
    @ViewChild('descricao') descricao;
    @ViewChild('isencao') isencao;
	@ViewChild('reducao') reducao;
	@ViewChild('suspensao') suspensao;
    @ViewChild('percentualBeneficio') percentualBeneficio;
    @ViewChild('statustf') statustf;
	@ViewChild('textAmparoLegal') textAmparoLegal;
	@ViewChild('justificativa') justificativa;

	isModificouPesquisa: boolean = false;
	servicoGrupoBeneficio = "Beneficio";
	isBuscaSalva: boolean = false;
	carregarGrid: boolean = false;
	opcaoStatus: number;
	isGeracaoDebito: boolean;
	isAnaliseVisual: boolean;
	isAguardandoProcessamento: boolean;
	isProcessado: boolean;
	model: TaxaGrupoBeneficioVM = new TaxaGrupoBeneficioVM();
	ocultarBotaoReprocessar: boolean = true;

	validadorPercentual: boolean = false;

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

		/*this.applicationService.get("UsuarioLogado").subscribe((result: any) => {
			if (result && result.perfis.includes(EnumPerfil.pessoaJuridica)) {
				this.isUsuarioImportador = true;
				this.razaoSocialEmpresa = result.empresaRepresentadaRazaoSocial;
				this.ocultarbotaocheck = true;
				this.ocultarBotaoReprocessar = true;
				this.inscricaoCadastral.nativeElement.value = result.usuInscricaoCadastral;
				this.empresa.nativeElement.value = result.usuNomeEmpresaOuLogado;
				this.parametros.razaoSocial = result.usuNomeEmpresaOuLogado;
				this.parametros.inscricaoCadastral = result.usuInscricaoCadastral;
				this.isModificouPesquisa = true
			}

		});

		if (this.model.statusPliSelecionado == undefined) {
			this.parametros.statusPliSelecionado = 0;
		}*/

	}

	public selecionarBeneficio(codigo){
		//console.log("id do benefício é: " + idBeneficio);
		let that = this;
		this.applicationService.get<TaxaGrupoBeneficioVM>(this.servicoGrupoBeneficio, codigo).subscribe(result => {
			that.isencao.nativeElement.checked = that.reducao.nativeElement.checked = false;

			if(result != null){
				that.model.idTaxaGrupoBeneficio = result.idTaxaGrupoBeneficio;
				that.model.dataCadastro = result.dataCadastro;
				result.tipoBeneficio == 1 ? that.isencao.nativeElement.checked = true : result.tipoBeneficio == 2 ? that.reducao.nativeElement.checked = true : result.tipoBeneficio == 3 ? that.suspensao.nativeElement.checked = true : that.isencao.nativeElement.checked = true;
				result.statusBeneficio == 1 ? that.statustf.nativeElement.checked = true : that.statustf.nativeElement.checked = false;
                result.descricaoAmparoLegal ? that.model.descricaoAmparoLegal = result.descricaoAmparoLegal : that.model.descricaoAmparoLegal = "";
                result.descricao ? that.model.descricao = result.descricao : that.parametros.descricao = "";
                result.codigo ? that.model.codigo = result.codigo : that.model.codigo = null;
				result.valorPercentualReducao ? that.model.valorPercentualReducao = result.valorPercentualReducao : that.model.valorPercentualReducao = null;
			} else{
				this.modal.alerta("Error ao buscar Benefício." , "Alerta");
				return false;
			}

		});

	}

    public verificarRota() {
		this.tituloPanel = 'Formulário';

        this.selecionarBeneficio(this.route.snapshot.params['id']);
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

		if( !(this.descricao.nativeElement.value)
		 ||  (this.isencao.nativeElement.checked != true  &&  this.reducao.nativeElement.checked != true &&  this.suspensao.nativeElement.checked != true)
		 || !(this.percentualBeneficio.nativeElement.value)
		 //||!(this.textAmparoLegal.nativeElement.value)
		 || !(this.justificativa.nativeElement.value)
		 ){
			this.modal.alerta(this.msg.CAMPO_OBRIGATORIO_NAO_INFORMADO_PREENCHIDO_INCORRETAMENTE, 'Alerta');
			return false;

		} else if (!this.percentualBeneficio.nativeElement.value){

			this.modal.alerta("O percentual não pode ser maior que 100%", 'Alerta');
			this.validadorPercentual = false;
			return false;
		} else{

			this.statustf.nativeElement.checked ? this.model.statusBeneficio = 1 : this.model.statusBeneficio = 0;
			this.isencao.nativeElement.checked ? this.model.tipoBeneficio = 1 : this.reducao.nativeElement.checked ? this.model.tipoBeneficio = 2 : this.suspensao.nativeElement.checked ? this.model.tipoBeneficio = 3 : this.model.tipoBeneficio = 0;
			this.model.valorPercentualReducao = parseFloat(this.percentualBeneficio.nativeElement.value) / 100;
			this.model.descricao = this.descricao.nativeElement.value;
			this.model.descricaoAmparoLegal = this.textAmparoLegal.nativeElement.value;
			this.model.justificativa = this.justificativa.nativeElement.value;
			//this.model.dataCadastro = new Date();

			this.salvar();
		}

	}

	salvar(){

		this.applicationService.put<TaxaGrupoBeneficioVM>(this.servicoGrupoBeneficio, this.model).subscribe(result => {

			if (result) {
				//this.limparCampos();
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "");
				this.router.navigate(['/grupo-beneficio']);
			}else{
				this.modal.alerta("Error ao tentar salvar o Grupo de Benefício.");
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

		if(this.percentualBeneficio.nativeElement.value>100)
		{
			this.modal.alerta("O percentual não pode ser maior que 100%", 'Informação');
			this.validadorPercentual = false;
			return false;
		}else{
			this.percentualBeneficio.nativeElement.value = this.replaceVirgula(this.percentualBeneficio.nativeElement.value);
			this.validadorPercentual = true;
		}
	}

	replaceVirgula(value): string{
		return value.replace(',','.');
	}



}
