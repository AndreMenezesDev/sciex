import { Component, OnInit, Injectable, ViewChild, Output, EventEmitter } from '@angular/core';
import { PagedItems } from '../../../view-model/PagedItems';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ValidationService } from "../../../shared/services/validation.service";
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { forEach } from '@angular/router/src/utils/collection';
import { EnumPerfil } from '../../../shared/enums/EnumPerfil';
import { TaxaGrupoBeneficioVM } from '../../../view-model/TaxaGrupoBeneficioVM';
import { TaxaNCMBeneficioVM } from '../../../view-model/TaxaNCMBeneficioVM';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

@Component({
	selector: 'app-cadastrar-ncm-beneficio',
	templateUrl: './formulario-cadastrar-ncm-beneficio.component.html'
})

@Injectable()
export class CadastrarNCMBeneficioComponent implements OnInit {

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
	idTaxaGrupoBeneficio: any;

    tituloPanel: string;

    @ViewChild('inscricaoCadastral') inscricaoCadastral;
    @ViewChild('formulario') formulario;
    @ViewChild('empresa') empresa;
    @ViewChild('descricao') descricao;
    @ViewChild('isencao') isencao;
    @ViewChild('reducao') reducao;
    @ViewChild('percentualBeneficio') percentualBeneficio;
    @ViewChild('statustf') statustf;
	@ViewChild('textAmparoLegal') textAmparoLegal;
	@ViewChild('codigoNcm') codigoNcm;

	isModificouPesquisa: boolean = false;
	model: TaxaGrupoBeneficioVM = new TaxaGrupoBeneficioVM();
	modelNCM: TaxaNCMBeneficioVM = new TaxaNCMBeneficioVM();
	isBuscaSalva: boolean = false;
	carregarGrid: boolean = false;
	opcaoStatus: number;
	isGeracaoDebito: boolean;
	isAnaliseVisual: boolean;
	isAguardandoProcessamento: boolean;
	isProcessado: boolean;
	ocultarBotaoReprocessar: boolean = true;
	servicoNCMBeneficio = "BeneficioNCMGrid";
	servicoGrupoBeneficio = "Beneficio";

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

		this.parametros.servico = this.servicoNCMBeneficio;
		this.parametros.titulo = "MANTER NCM BENEFICIO"
		this.parametros.width = { 0: { columnWidth: 100 }, 1: { columnWidth: 100 }};
		this.parametros.columns = ["Código", "Descrição"] ;
		this.parametros.fields = ["codigoNCM", "descricaoNCM" ];
	}

	onChangeSort($event) {
		this.grid.sort = $event;
	}

	onChangeSize($event) {
		this.grid.size = $event;
	}

	onChangePage($event) {
		this.grid.page = $event;
		this.selecionarNCM(this.route.snapshot.params['id']);
    }

    onChangeTipoSituacao(){
    }

	onChangeTipoDeBeneficio(){
    }

    public verificarRota() {

		this.tituloPanel = 'Formulário';
		this.selecionarNCM(this.route.snapshot.params['id']);
		this.selecionarBeneficio(this.route.snapshot.params['id']);
		this.telaAtual = this.route.snapshot.params['tela'];
	}

	public selecionarNCM(idTaxaGrupoBeneficio){

		//pagina == 0 ? this.parametros.page = this.grid.page : this.parametros.page = 1;
		//this.parametros.page = this.grid.page;
		//this.grid.page = pagina;
		this.parametros.page = this.grid.page;
        this.parametros.size = this.grid.size;
        this.parametros.sort = this.grid.sort.field;
        this.parametros.reverse = this.grid.sort.reverse;

		this.parametros.idTaxaGrupoBeneficio = idTaxaGrupoBeneficio;
		this.idTaxaGrupoBeneficio = idTaxaGrupoBeneficio;
		//this.model = this.parametros;


		this.applicationService.get(this.servicoNCMBeneficio, this.parametros).subscribe((result: PagedItems) =>  {

			if(result.total > 0){

				this.grid.lista = result.items;
				this.grid.total = result.total;

				this.isModificouPesquisa = false;
				this.isBuscaSalva = true;
				this.parametros.exportarListagem = true;
				//.clearInput = true;
			} else if(result.total == 0) {

				//this.modal.alerta("Benefício não possui NCM Cadastrado", "Alerta");
				this.grid.lista = null;
				this.grid.total = null;
				return false;
			} else{

				//ordenação grid
			}

		});

	}


	public selecionarBeneficio(idTaxaGrupoBeneficio){

		this.model.idTaxaGrupoBeneficio = idTaxaGrupoBeneficio;

		this.applicationService.get<TaxaGrupoBeneficioVM>(this.servicoNCMBeneficio, this.model.idTaxaGrupoBeneficio).subscribe(result => {

			if(result != null){

				this.parametros.beneficio = result.descricao;
				result.tipoBeneficio == 1 ? this.parametros.tipoBeneficio = 'Isenção' : result.tipoBeneficio == 2 ? this.parametros.tipoBeneficio = 'Redução' :result.tipoBeneficio == 3 ? this.parametros.tipoBeneficio = 'Suspensão' : this.parametros.tipoBeneficio = '';
				this.parametros.percentualBeneficio = result.valorPercentualReducao;
			} else{

				this.modal.alerta("Falha ao Buscar Benefício", "Alerta");
				return false;
			}

		});

	}

	cancelarOperacao(){
		// this.modal.confirmacao("Os dados preechidos serão descartados. Deseja continuar?", '', '')
		// 	.subscribe(isConfirmado => {
		// 		if (isConfirmado) {
					this.router.navigate(['/grupo-beneficio']);
			// 	}
			// });
	}

	salvar(){
		//console.log("parametros NCM: " + this.parametros.CodigoNCM);

		this.modelNCM.idTaxaGrupoBeneficio =  this.route.snapshot.params['id'];

		if(this.codigoNcm.model == "")
		{

			this.modal.alerta("NCM inválido", 'Informação');
			return false;
		} else{

			this.modelNCM.idNcm = this.codigoNcm.model.id;
		}

		this.modelNCM.dataCadastro = new Date();

		this.applicationService.put(this.servicoNCMBeneficio, this.modelNCM).subscribe( result =>  {

			if(result == 2){

				this.modal.alerta("NCM já Vinculado ao Atual Benefício", "Alerta");
				return false;
			} else if(result == 0) {

				this.modal.alerta("Erro ao Salvar NCM", "Alerta");
				return false;
			} else if(result == 1) {
				this.selecionarNCM(this.route.snapshot.params['id']);
				//this.codigoNcm.clearInput = true;
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "");
			}


		});

	}

}
