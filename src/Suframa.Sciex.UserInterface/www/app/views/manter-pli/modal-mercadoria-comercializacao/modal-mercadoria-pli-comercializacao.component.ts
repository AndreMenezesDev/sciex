import { Component, ViewChild, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { PagedItems } from '../../../view-model/PagedItems';
import { manterPliMercadoriaVM } from '../../../view-model/ManterPliMercadoriaVM';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../shared/guards/auth-guard.service';
import { ToastrService } from 'toastr-ng2/toastr';
import { manterPliDetalheMercadoriaVM } from '../../../view-model/ManterPliDetalheMercadoriaVM';
import { manterFornecedorVM } from '../../../view-model/manterFornecedorVM';
import { manterFabricanteVM } from '../../../view-model/ManterFabricanteVM';
import { manterPliFornecedorFabricanteVM } from '../../../view-model/ManterPliFornecedorFabricanteVM';
import { manterPliProcessoAnuenteVM } from '../../../view-model/ManterPliProcessoAnuenteVM';
import { FormatarNumeroDecimal5Directive } from '../../../shared/directives/formatar-numero-decimal-5.directive';
import { window } from 'rxjs/operators';
import { AuthenticationService } from '../../../shared/services/authentication.service';
import { ValoresVM } from '../../../view-model/ValoresVM';


declare var $: any;

@Component({
	selector: 'app-modal-mercadoria-pli-comercializacao',
	templateUrl: './modal-mercadoria-pli-comercializacao.component.html',
})

export class ModalMercadoriaPliComercializacaoComponent implements OnInit, OnDestroy {
	parametros: any = {};
	parametrosCra: any = {};
	parametrosMascara: any = {};
	parametrosValores: any = {};
	gridDetalheMercadoria: any = { sort: {} };
	gridProcessoAnuente: any = { sort: {} };
	isDisplay: boolean = false;
	cpfDigitado: string = '';
	opcaoSelecionada: any;
	model: manterPliMercadoriaVM = new manterPliMercadoriaVM();
	modelDetalheMercadoria: manterPliDetalheMercadoriaVM = new manterPliDetalheMercadoriaVM();
	modelProcessoAnuente: manterPliProcessoAnuenteVM = new manterPliProcessoAnuenteVM();
	listaItemMercadoria = Array<manterPliDetalheMercadoriaVM>();
	listaProcessoAnuente = Array<manterPliProcessoAnuenteVM>();
	listaMercadorias = Array<manterPliMercadoriaVM>();
	modelValores: ValoresVM = new ValoresVM();
	isRetificador: boolean = false;

	idPliMercadoria: number;
	produto: string;
	mercadoria: string;
	tipoFornecedor = [];
	tipoCobertura = [];
	tipoAcordoTarifario = [];
	listaRegimeTributario = [];
	listaCondicao = [];
	listaCondicaoMap = [];
	isShowPanel: boolean = false;
	isPlusFab: boolean = true;
	isPlusFor: boolean = true;
	isValidarDetalheItem: boolean = false;
	valorCondicaoVenda: number;

	servicoPliMercadoria = 'PliMercadoria';
	servicoPliDetalheMercadoria = 'PliDetalheMercadoria';
	servicoPliDetalheMercadoriaGrid = 'PliDetalheMercadoriaGrid';
	servicoViewDetalheMercadoria = 'ViewDetalheMercadoriaDropDown';
	servicoFornecedor = 'Fornecedor';
	servicoPliFornecedor = 'PliFornecedorFabricante';
	servicoFabricante = 'Fabricante';
	servicoViewProdutoEmpresaDropDown = "ViewProdutoEmpresaDropDown";
	titulo: string = "Dados da Mercadoria";
	idDetalheMercadoria: number;
	idUnidadeMedida: number;
	total: number = 0;
	valorDecimal: string = "0";
	valorDecimalTotal: string = "0";

	totalItensLista: number;
	indexAtual: number = 0;
	itemAtual: any;
	somenteLeitura: boolean;
	CNPJ: string;
	somenteLeituraFor: boolean;
	somenteLeituraFab: boolean;

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private applicationService: ApplicationService,
		private toastrService: ToastrService,
		private router: Router,
		private authguard: AuthGuard,
		private authenticationService: AuthenticationService,
	) {

	}

	@ViewChild('appModalPliMercadoria') appModalPliMercadoria;
	@ViewChild('appModalPliMercadoriaBackground') appModalPliMercadoriaBackground;
	@ViewChild('formulario') formulario;
	@ViewChild('formularioDetalhe') formularioDetalhe;
	@ViewChild('bemFabricado') bemFabricado;
	@ViewChild('materialUsado') materialUsado;
	@ViewChild('btnAlterarForFab') btnAlterarForFab;

	@ViewChild('abaDados') abaDados;
	@ViewChild('descricaoDetalheMercadoria') descricaoDetalheMercadoria;
	@ViewChild('detalheMercadoriaGrid') detalheMercadoriaGrid;
	@ViewChild('top') top;

	// inputs
	@ViewChild('valorCondicaoVendaInput') valorCondicaoVendaInput;
	@ViewChild('quantidadeUnidadeComercializada') quantidadeUnidadeComercializada;
	@ViewChild('valorUnitarioCondicaoVenda') valorUnitarioCondicaoVenda;
	@ViewChild('informacoesComplementares') informacoesComplementares;
	@ViewChild('complemento') complemento;
	@ViewChild('cpf') cpf;
	@ViewChild('numeroProcesso') numeroProcesso;

	@ViewChild('pesoLiquido') pesoLiquido;
	@ViewChild('quantidadeEstatistica') quantidadeEstatistica;

	//selects
	@ViewChild('moeda') moeda;
	@ViewChild('incoterms') incoterms;
	@ViewChild('pais') pais;
	@ViewChild('despacho') despacho;
	@ViewChild('entrada') entrada;
	@ViewChild('regimetributario') regimetributario;
	@ViewChild('fundamentolegal') fundamentolegal;
	@ViewChild('unidadeComercializada') unidadeComercializada;
	@ViewChild('fornecedorEmpresa') fornecedorEmpresa;
	@ViewChild('fabricanteEmpresa') fabricanteEmpresa;
	@ViewChild('idPaisOrigem') idPaisOrigem;
	@ViewChild('idModalidadePagamento') idModalidadePagamento;
	@ViewChild('idMotivo') idMotivo;
	@ViewChild('instituicaoFinanceira') instituicaoFinanceira;
	@ViewChild('acordoAladi') acordoAladi;
	@ViewChild('naladi') naladi;
	@ViewChild('orgaoAnuente') orgaoAnuente;
	@ViewChild('txtCRA') txtCRA;

	// fornecedor
	@ViewChild('paisFornecedor') paisFornecedor;

	ngOnInit() {
		setTimeout(x => { $(".modal-body").scrollTop(0); }, 500);
	}

	ngOnDestroy() {
		// ...
	}

	public dadosAleatorios() {

		this.tipoFornecedor = [
			{ idTipoFornecedor: 0, descricao: "0 | TIPO DE FORNECEDOR NÃO INFORMADO" },
			{ idTipoFornecedor: 1, descricao: "1 | O FABRICANTE/PRODUTOR É O EXPORTADOR" },
			{ idTipoFornecedor: 2, descricao: "2 | O FABRICANTE/PRODUTOR NÃO É O EXPORTADOR" },
			{ idTipoFornecedor: 3, descricao: "3 | O FABRICANTE/PRODUTOR É DESCONHECIDO" }
		];

		this.tipoCobertura = [
			{ idTipoCobertura: 0, descricao: this.somenteLeitura ? "" : "Selecione uma Opção" },
			{ idTipoCobertura: 1, descricao: "1 | ATÉ 180 DIAS" },
			{ idTipoCobertura: 2, descricao: "2 | DE 180 ATÉ 360 DIAS" },
			{ idTipoCobertura: 3, descricao: "3 | ACIMA DE 360 DIAS" },
			{ idTipoCobertura: 4, descricao: "4 | SEM COBERTURA" }
		];

		this.tipoAcordoTarifario = [
			{ idTipoAcordoTarifario: 0, descricao: this.somenteLeitura ? "" : "Selecione uma Opção" },
			{ idTipoAcordoTarifario: 2, descricao: "2 | ALADI" },
			{ idTipoAcordoTarifario: 3, descricao: "3 | OMC" },
			{ idTipoAcordoTarifario: 4, descricao: "4 | SGPC" }
		];

		this.listaRegimeTributario = [
			{ idRegimeTributario: 0, descricao: this.somenteLeitura ? "" : "Selecione uma Opção" },
			{ idRegimeTributario: 3, descricao: "3 | ISENÇÃO" },
			{ idRegimeTributario: 5, descricao: "5 | SUSPENSÃO" }
		];


		this.listaCondicao = [
			{ id: 1, descricao: "Bem fabricado sob encomenda", selected: false },
			{ id: 2, descricao: "Material usado", selected: false }
		];


	}

	changeIconColFor() {

		if ($("#abaFornecedor").hasClass('fa-plus-square-o')) {
			$(".a").removeClass('fa-minus-square-o');
			$(".a").addClass('fa-plus-square-o');
			$("#abaFornecedor").removeClass('fa-plus-square-o');
			$("#abaFornecedor").addClass('fa-minus-square-o');
		} else {
			$(".a").removeClass('fa-minus-square-o');
			$(".a").addClass('fa-plus-square-o');
			$("#abaFornecedor").removeClass('fa-minus-square-o');
			$("#abaFornecedor").addClass('fa-plus-square-o');
		}
	}

	changeIconColFab() {

		if ($("#abaFabricante").hasClass('fa-plus-square-o')) {
			$(".b").removeClass('fa-minus-square-o');
			$(".b").addClass('fa-plus-square-o');
			$("#abaFabricante").removeClass('fa-plus-square-o');
			$("#abaFabricante").addClass('fa-minus-square-o');
		} else {
			$(".b").removeClass('fa-minus-square-o');
			$(".b").addClass('fa-plus-square-o');
			$("#abaFabricante").removeClass('fa-minus-square-o');
			$("#abaFabricante").addClass('fa-plus-square-o');
		}

	}

	changeIconFor() {
		if (this.isPlusFab) {
			this.isPlusFab = false;
		} else {
			this.isPlusFab = true;
		}
	}

	changeIconFab() {
		if (this.isPlusFor) {
			this.isPlusFor = false;
		} else {
			this.isPlusFor = true;
		}
	}

	public abrirListaValidacao(listaMercadorias: Array<manterPliMercadoriaVM>, numeroPli: string) {
		this.somenteLeituraFor = false;
		this.somenteLeituraFab = false;
		this.indexAtual = 0;
		this.titulo = "Validar PLI Nº: " + numeroPli;
		this.listaMercadorias = listaMercadorias;
		this.total = this.listaMercadorias.length;

		this.pesoLiquido.nativeElement.focus();		
		this.Anterior();

		this.appModalPliMercadoria.nativeElement.style.display = 'block';
		this.appModalPliMercadoriaBackground.nativeElement.style.display = 'block';

		for (let item of listaMercadorias) {
			for (let item2 of item.listaPliDetalheMercadoriaVM) {
				if (item2.descricaoDetalhe == null || item2.descricaoDetalhe == "") {
					this.detalheMercadoriaGrid.alterarDetalheMercadoria(item2);
					this.isValidarDetalheItem = true;
					break;
				}
				if (item2.idUnidadeMedida == null || item2.idUnidadeMedida == 0) {
					this.detalheMercadoriaGrid.alterarDetalheMercadoria(item2);
					this.isValidarDetalheItem = true;
					break;
				}
				if (item2.quantidadeComercializada == null || item2.quantidadeComercializada == 0) {
					this.detalheMercadoriaGrid.alterarDetalheMercadoria(item2);
					this.isValidarDetalheItem = true;
					break;
				}
				if (item2.valorUnitarioCondicaoVenda == null || item2.valorUnitarioCondicaoVenda == 0) {
					this.detalheMercadoriaGrid.alterarDetalheMercadoria(item2);
					this.isValidarDetalheItem = true;
					break;
				}
			}
		}

		this.somenteLeituraFor = false;
		this.somenteLeituraFab = false;

	}

	public abrir(itemMercadoria: manterPliMercadoriaVM, listaDetalhesMercadorias, listaProcessoAnuente, parametros) {

		this.somenteLeituraFor = false;
		this.somenteLeituraFab = false;

		if(itemMercadoria.tipoALI == "Retificadora"){
			this.isRetificador = true;
		}

		this.somenteLeitura = parametros.somenteLeitura;
		this.limparCamposMercadoria();
		this.limparCamposItemMercadoria();
		this.limparCamposComplemento();

		this.abaDados.nativeElement.click();

		if (this.listaMercadorias.length == 0) {
			this.appModalPliMercadoria.nativeElement.style.display = 'block';
			this.appModalPliMercadoriaBackground.nativeElement.style.display = 'block';
		}

		this.idPliMercadoria = itemMercadoria.idPliMercadoria;
		this.parametros.modelMercadoria = this.model = itemMercadoria;

		if (this.model != null && this.model != undefined) {
			this.pesoLiquido.nativeElement.value = this.model.pesoLiquidoString;
			this.quantidadeEstatistica.nativeElement.value = this.model.quantidadeEstatisticaString;
		}

		this.inicializarItensTela();

		if (listaDetalhesMercadorias != undefined || listaDetalhesMercadorias != null) {
			this.gridDetalheMercadoria.lista = listaDetalhesMercadorias;
			this.listaItemMercadoria = listaDetalhesMercadorias;
			this.calcularValorTotalCondicaoVenda(undefined);
		}

		if (listaProcessoAnuente != undefined || listaProcessoAnuente != null) {
			this.gridProcessoAnuente.lista = listaProcessoAnuente;
			this.listaProcessoAnuente = listaProcessoAnuente;
		}

	}

	public consultarValorCra() {

		this.parametrosCra.CodigoProduto = this.model.codigoProduto;
		this.parametrosCra.CodigoTipoProduto = this.model.codigoTipoProduto;
		this.parametrosCra.CodigoModeloProduto = this.model.codigoModeloProduto;
		this.parametrosCra.Cnpj = this.CNPJ;

		this.applicationService.get(this.servicoViewProdutoEmpresaDropDown, this.parametrosCra).subscribe(result => {
            if (result[0] != null && result[0].lenght > 0 && result[0].crii == 2)
				this.model.valorCRA = parseFloat(result[0].descricaoCRII.replace("%", ""));
		});
	}

	public fechar(salvo) {

		if (salvo != "salvo") {
			this.modal.confirmacao("Os dados serão descartados. Deseja continuar?", '', '')

                .subscribe(isConfirmado => {
					if (isConfirmado) {
						$(".modal-body").scrollTop(0);
						this.model = new manterPliMercadoriaVM();
						this.appModalPliMercadoria.nativeElement.style.display = 'none';
						this.appModalPliMercadoriaBackground.nativeElement.style.display = 'none';
					}
				});
		} else {
			this.model = new manterPliMercadoriaVM();
			this.appModalPliMercadoria.nativeElement.style.display = 'none';
			this.appModalPliMercadoriaBackground.nativeElement.style.display = 'none';
		}


	}

	listarDetalhesMercadoria() {
		if (this.model.idPliMercadoria != undefined) {
			this.parametros.idPliMercadoria = this.model.idPliMercadoria;

			this.applicationService.get(this.servicoPliDetalheMercadoriaGrid, this.parametros).subscribe(result => {
				this.gridDetalheMercadoria.lista = result;
			});
		}

	}

	carregarDadosFornecedor(id) {
		if (id != undefined) {
			this.applicationService.get<manterFornecedorVM>(this.servicoFornecedor, id).subscribe(result => {
				this.parametros.codigoPaisFornecedor = result.codigoPais + " | " + result.descricaoPais;
				this.parametros.logradouroFornecedor = result.logradouro;
				this.parametros.complementoFornecedor = result.complemento;
				this.parametros.numeroFornecedor = result.numero;
				this.parametros.cidadeFornecedor = result.cidade;
				this.parametros.estadoFornecedor = result.estado;
			});
		}
	}

	carregarDadosPliFornecedor(id) {		
		if (id != undefined) {
			this.somenteLeituraFor = false;
			this.applicationService.get<manterPliFornecedorFabricanteVM>(this.servicoPliFornecedor, id).subscribe(result => {
				if (result != undefined && result != null) {				
					if (result.descricaoFornecedor != null && result.descricaoFornecedor != "") {
						this.parametros.codigoPaisFornecedor = result.codigoPaisFornecedor + "| " + result.descricaoPaisFornecedor;
						this.somenteLeituraFor = true;
						this.parametros.descricaoPaisFornecedor = result.descricaoPaisFornecedor;
						this.parametros.logradouroFornecedor = result.descricaoLogradouroFornecedor;
						this.parametros.complementoFornecedor = result.descricaoComplementoFornecedor;
						this.parametros.numeroFornecedor = result.numeroFornecedor;
						this.parametros.cidadeFornecedor = result.descricaoCidadeFornecedor;
						this.parametros.estadoFornecedor = result.descricaoEstadoFornecedor;
						this.parametros.nomeFornecedor = result.descricaoFornecedor;

						this.model.dadosFabricanteFornecedor.descricaoFornecedor = result.descricaoFornecedor;
						this.model.dadosFabricanteFornecedor.codigoPaisFornecedor = result.codigoPaisFornecedor + "| " + result.descricaoPaisFornecedor;
						this.model.dadosFabricanteFornecedor.descricaoPaisFornecedor = result.descricaoPaisFornecedor;
						this.model.dadosFabricanteFornecedor.descricaoLogradouroFornecedor = result.descricaoLogradouroFornecedor;
						this.model.dadosFabricanteFornecedor.descricaoComplementoFornecedor = result.descricaoComplementoFornecedor;
						this.model.dadosFabricanteFornecedor.numeroFornecedor = result.numeroFornecedor;
						this.model.dadosFabricanteFornecedor.descricaoCidadeFornecedor = result.descricaoCidadeFornecedor;
						this.model.dadosFabricanteFornecedor.descricaoEstadoFornecedor = result.descricaoEstadoFornecedor;
					}
				} else {
					this.somenteLeituraFor = false;
				}
			});
		}
	}

	carregarDadosFabricante(id) {
		if (id != undefined) {
			this.applicationService.get<manterFabricanteVM>(this.servicoFabricante, id).subscribe(result => {
				this.parametros.codigoPaisFabricante = result.codigoPais + " | " + result.descricaoPais;
				this.parametros.logradouroFabricante = result.logradouro;
				this.parametros.complementoFabricante = result.complemento;
				this.parametros.numeroFabricante = result.numero;
				this.parametros.cidadeFabricante = result.cidade;
				this.parametros.estadoFabricante = result.estado;

			});
		}
	}

	carregarDadosPliFabricante(id) {
		if (id != undefined) {
			this.somenteLeituraFab = false;
			this.applicationService.get<manterPliFornecedorFabricanteVM>(this.servicoPliFornecedor, id).subscribe(result => {
				if (result != undefined && result != null) {
					if (result.codigoPaisFabricante != null) {
						this.parametros.codigoPaisFabricante = result.codigoPaisFabricante + " | " + result.descricaoPaisFabricante;
					}
					this.somenteLeituraFab = true;
					this.parametros.descricaoPaisFabricante = result.descricaoPaisFabricante;
					this.parametros.logradouroFabricante = result.descricaoLogradouroFabricante;
					this.parametros.complementoFabricante = result.descricaoComplementoFabricante;
					this.parametros.numeroFabricante = result.numeroFabricante;
					this.parametros.cidadeFabricante = result.descricaoCidadeFabricante;
					this.parametros.estadoFabricante = result.descricaoEstadoFabricante;
					this.parametros.nomeFabricante = result.descricaoFabricante;

					this.model.dadosFabricanteFornecedor.descricaoFabricante = result.descricaoFabricante;
					this.model.dadosFabricanteFornecedor.codigoPaisFabricante = result.codigoPaisFabricante + " | " + result.descricaoPaisFabricante;
					this.model.dadosFabricanteFornecedor.descricaoPaisFabricante = result.descricaoPaisFabricante;
					this.model.dadosFabricanteFornecedor.descricaoLogradouroFabricante = result.descricaoLogradouroFabricante;
					this.model.dadosFabricanteFornecedor.descricaoComplementoFabricante = result.descricaoComplementoFabricante;
					this.model.dadosFabricanteFornecedor.numeroFabricante = result.numeroFabricante;
					this.model.dadosFabricanteFornecedor.descricaoCidadeFabricante = result.descricaoCidadeFabricante;
					this.model.dadosFabricanteFornecedor.descricaoEstadoFabricante = result.descricaoEstadoFabricante;
				} else {
					this.somenteLeituraFab = false;
				}
			});
		}
	}

	inicializarItensTela() {

		this.applicationService.get("UsuarioLogado", { cnpj: true }).subscribe((result: any) => {
			if (result != undefined) {
				this.CNPJ = result;

				this.produto = (this.model.codigoProdutoConcatenado + "00000000000").substring(0, 11).slice(-11) + " - " + this.model.descricaoProduto;
				this.mercadoria = this.model.codigoNCMMercadoria + " - " + this.model.descricaoNCMMercadoria;

				this.dadosAleatorios();


				this.carregarDadosPliFornecedor(this.model.idPliMercadoria);
				if (this.model.tipoFornecedor == 2) {
					this.carregarDadosPliFabricante(this.model.idPliMercadoria); 
				}

				if (this.model.valorCRA == undefined) {
					this.consultarValorCra();
				}
			}
		});

		this.parametros.modal = this;

		if (this.model.idRegimeTributario == undefined)
			this.model.idRegimeTributario = 0;

		if (this.model.tipoFornecedor == undefined)
			this.model.tipoFornecedor = 0;

		if (this.model.tipoAcordoTarifario == undefined)
			this.model.tipoAcordoTarifario = 0;

		if (this.model.tipoCOBCambial == undefined)
			this.model.tipoCOBCambial = 0;

		if (this.model.quantidadeUnidadeMedidaEstatistica == undefined)
			this.model.quantidadeUnidadeMedidaEstatistica = 0;

		if (this.model.pesoLiquido == undefined)
			this.model.pesoLiquido = 0;

		if (this.modelDetalheMercadoria.quantidadeComercializada == undefined || this.modelDetalheMercadoria.quantidadeComercializada == null) {
			this.modelDetalheMercadoria.quantidadeComercializada = 0;
		}

		if (this.modelDetalheMercadoria.valorUnitarioCondicaoVenda == undefined)
			this.modelDetalheMercadoria.valorUnitarioCondicaoVenda = 0;

		if (this.model.tipoBemEncomenda != undefined) {
			this.bemFabricado.nativeElement.checked = true;
		}

		if (this.model.tipoMaterialUsado != undefined) {
			this.materialUsado.nativeElement.checked = true;
		}

		this.model.dadosFabricanteFornecedor = new manterPliFornecedorFabricanteVM();
	}

	isSetorTrue() {
		this.isDisplay = true;
	}

	isSetorFalse() {
		this.isDisplay = false;
	}

	public salvar(confirmarMensagemModal: boolean) {

		if (!confirmarMensagemModal) {
			this.model.isValidarItemPli = true;
		}

		this.model.tipoBemEncomenda = null;
		this.model.tipoMaterialUsado = null;

		if (this.bemFabricado.nativeElement.checked) {
			this.model.tipoBemEncomenda = 1;
		}

		if (this.materialUsado.nativeElement.checked) {
			this.model.tipoMaterialUsado = 1;
		}

		if (this.model.idRegimeTributario == 0) {
			this.model.idRegimeTributario = null;
		}

		if (this.model.tipoCOBCambial == 0) {
			this.model.tipoCOBCambial = null;
		}

		if (this.model.tipoAcordoTarifario == 0) {
			this.model.tipoAcordoTarifario == null;
		}

		if (this.model.tipoFornecedor == 0 || this.model.tipoFornecedor == 1 || this.model.tipoFornecedor == 2)
			this.model.codigoPaisOrigemFabricante = null;

		// regra pais fabricante
		if (this.model.codigoPaisOrigemFabricante == undefined || this.idPaisOrigem.valorInput.nativeElement.value == "") {
			this.model.descricaoPaisOrigemFabricante = null;
		} else {
			this.model.descricaoPaisOrigemFabricante = this.idPaisOrigem.valorInput.nativeElement.value.split("|")[1].trim();
		}

		//regra pais mercadoria		
		if (this.model.codigoPais == undefined || this.pais.valorInput.nativeElement.value == "") {
			this.model.descricaoPais = null;
		} else {
			this.model.descricaoPais = this.pais.valorInput.nativeElement.value.split("|")[1].trim();
		}

		
		this.model.dadosFabricanteFornecedor.codigoAusenciaFabricante = this.model.tipoFornecedor;

		if (this.model.tipoFornecedor == 1 || this.model.tipoFornecedor == 2) {
			try {
				if (this.fornecedorEmpresa.valorInput.nativeElement.value.indexOf('|') > -1) {
					this.model.dadosFabricanteFornecedor.descricaoFornecedor = (this.somenteLeituraFor ? this.parametros.nomeFornecedor : (this.fornecedorEmpresa.valorInput.nativeElement.value != null && this.fornecedorEmpresa.valorInput.nativeElement.value != undefined && this.fornecedorEmpresa.valorInput.nativeElement.value != "" ? this.fornecedorEmpresa.valorInput.nativeElement.value.split("|")[1].trim() : ""));
					this.model.dadosFabricanteFornecedor.codigoPaisFornecedor = this.parametros.codigoPaisFornecedor;
					this.model.dadosFabricanteFornecedor.descricaoPaisFornecedor = this.parametros.descricaoPaisFornecedor
					this.model.dadosFabricanteFornecedor.descricaoLogradouroFornecedor = this.parametros.logradouroFornecedor;
					this.model.dadosFabricanteFornecedor.descricaoComplementoFornecedor = this.parametros.complementoFornecedor;
					this.model.dadosFabricanteFornecedor.numeroFornecedor = this.parametros.numeroFornecedor;
					this.model.dadosFabricanteFornecedor.descricaoCidadeFornecedor = this.parametros.cidadeFornecedor;
					this.model.dadosFabricanteFornecedor.descricaoEstadoFornecedor = this.parametros.estadoFornecedor;
				}
			} catch (e) {
			}
		}

		if (this.model.tipoFornecedor == 2) {

			try {
				if (this.fabricanteEmpresa.valorInput.nativeElement.value.indexOf('|') > -1) {
					this.model.dadosFabricanteFornecedor.descricaoFabricante = (this.somenteLeituraFab ? this.parametros.nomeFabricante : (this.fabricanteEmpresa.valorInput.nativeElement.value != null && this.fabricanteEmpresa.valorInput.nativeElement.value != undefined && this.fabricanteEmpresa.valorInput.nativeElement.value != "" ? this.fabricanteEmpresa.valorInput.nativeElement.value.split("|")[1].trim() : ""));
					this.model.dadosFabricanteFornecedor.codigoPaisFabricante = this.parametros.codigoPaisFabricante;
					this.model.dadosFabricanteFornecedor.descricaoPaisFabricante = this.parametros.descricaoPaisFabricante;
					this.model.dadosFabricanteFornecedor.descricaoLogradouroFabricante = this.parametros.logradouroFabricante;
					this.model.dadosFabricanteFornecedor.descricaoComplementoFabricante = this.parametros.complementoFabricante;
					this.model.dadosFabricanteFornecedor.numeroFabricante = this.parametros.numeroFabricante;
					this.model.dadosFabricanteFornecedor.descricaoCidadeFabricante = this.parametros.cidadeFabricante;
					this.model.dadosFabricanteFornecedor.descricaoEstadoFabricante = this.parametros.estadoFabricante;
				}
			}
			catch (e){
			}
		}


		if (this.model.isValidarItemPli) {
			this.applicationService.put(this.servicoPliMercadoria, this.model).subscribe((result: PagedItems) => {
				if (confirmarMensagemModal) {
					this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "")
						.subscribe(isConfirmado => {
							if (isConfirmado) {
								$(".modal-body").scrollTop(0);
								this.fechar("salvo");
							}
						});
				}
				else {
					this.toastrService.success(this.msg.OPERACAO_REALIZADA_COM_SUCESSO).onShown.subscribe(o => {

						this.listaMercadorias[this.indexAtual] = JSON.parse(JSON.stringify(result));
						this.indexAtual = this.indexAtual + 1 == this.listaMercadorias.length ? this.listaMercadorias.length - 1 : this.indexAtual + 1;
						var manterPliMercadoria = JSON.parse(JSON.stringify(this.listaMercadorias));

						this.abrir(manterPliMercadoria[this.indexAtual],
							manterPliMercadoria[this.indexAtual].listaPliDetalheMercadoriaVM,
							manterPliMercadoria[this.indexAtual].listaPliProcessoAnuenteVM, this.parametros);
					});
				}
			});

		} else {

			if (this.idPliMercadoria != undefined) {
				var nomeFormulario = "formulario" + this.idPliMercadoria;

				if (!this.validationService.form(nomeFormulario)) { return; }

				this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
					.subscribe(isConfirmado => {
						if (isConfirmado) {

							this.applicationService.put<manterPliMercadoriaVM>(this.servicoPliMercadoria, this.model).subscribe(result => {
								if (result.mensagemErro != "" && result.mensagemErro != null && result.mensagemErro.length > 0) {
									this.modal.alerta(result.mensagemErro, "", "");
								} else {
									this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "")
										.subscribe(isConfirmado => {
											if (isConfirmado) {
												$(".modal-body").scrollTop(0);
												this.fechar("salvo");
											}
										});
								}
							});
						}
					});
			}
		}

	}

	validarItensMercadoria() {
		this.unidadeComercializada.valorInput.nativeElement.setCustomValidity('');
		this.quantidadeUnidadeComercializada.nativeElement.setCustomValidity('');
		this.valorUnitarioCondicaoVenda.nativeElement.setCustomValidity('');
		this.descricaoDetalheMercadoria.nativeElement.setCustomValidity('');

		if (this.unidadeComercializada.valorInput.nativeElement.value.trim() == "") {
			this.unidadeComercializada.valorInput.nativeElement.setCustomValidity('Preencha este campo.');
		}
		else if (this.quantidadeUnidadeComercializada.nativeElement.value == "" || parseFloat(this.quantidadeUnidadeComercializada.nativeElement.value.replace(",", ".")) == 0) {
			this.quantidadeUnidadeComercializada.nativeElement.setCustomValidity('Preencha este campo.');
		} else if (this.valorUnitarioCondicaoVenda.nativeElement.value == "" || parseFloat(this.valorUnitarioCondicaoVenda.nativeElement.value.replace(",", ".")) == 0) {
			this.valorUnitarioCondicaoVenda.nativeElement.setCustomValidity('Preencha este campo.');
		}
		else if (this.descricaoDetalheMercadoria.nativeElement.value.trim() == "") {
			this.descricaoDetalheMercadoria.nativeElement.setCustomValidity('Preencha este campo.')
		}

	}

	incluirItemMercadoria() {
		if (this.listaItemMercadoria.length == 99) {
			this.modal.alerta("A mercadoria já possui limite de 99 itens cadastrados.", "Informação");
			return;
		}

		this.validarItensMercadoria();

		if (!this.validationService.form('formulario')) { return; }
		if (!this.formulario.valid) { return; }

		var nomeFormularioDetalhe = "formularioDetalhe" + this.idPliMercadoria.toString();
		if (!this.validationService.form(nomeFormularioDetalhe)) { return; }

		if (this.modelDetalheMercadoria.idUnidadeMedida != undefined && this.modelDetalheMercadoria.idUnidadeMedida > 0) {
			this.modelDetalheMercadoria.descricaoUnidadeMedida = this.unidadeComercializada.valorInput.nativeElement.value.split("|")[1].trim();
			this.modelDetalheMercadoria.siglaUnidadeMedida = this.unidadeComercializada.valorInput.nativeElement.value.split("|")[0].trim();
		}
		else if (this.unidadeComercializada.valorInput.nativeElement.value.trim() != "") {
			this.modelDetalheMercadoria.descricaoUnidadeMedida = this.unidadeComercializada.valorInput.nativeElement.value;
		}
		else {
			this.modelDetalheMercadoria.descricaoUnidadeMedida = null;
		}

		if (this.modelDetalheMercadoria.codigoDetalheMercadoria == null) {
			this.modelDetalheMercadoria.codigoDetalheMercadoria = this.listaItemMercadoria.length + 1;
		}

		this.modelDetalheMercadoria.idDetalheMercadoria = this.idDetalheMercadoria;
		this.modelDetalheMercadoria.idUnidadeMedida = this.idUnidadeMedida;

		this.modelDetalheMercadoria.excluir = false;

		if (this.modelDetalheMercadoria.index == undefined) {
			this.listaItemMercadoria.push(this.modelDetalheMercadoria);
		}
		else {
			this.listaItemMercadoria[this.modelDetalheMercadoria.index] = this.modelDetalheMercadoria;
		}

		this.model.listaPliDetalheMercadoriaVM = this.listaItemMercadoria;
		this.gridDetalheMercadoria.lista = this.listaItemMercadoria;
		this.limparCamposItemMercadoria();
		this.calcularValorTotalCondicaoVenda(undefined);
		this.isValidarDetalheItem = false;
	}

	limparCamposMercadoria() {

		this.moeda.clear();
		this.incoterms.clear();
		this.pais.clear();
		this.despacho.clear();
		this.entrada.clear();
		this.fundamentolegal.clear();
		//this.itemMercadoria.clear();
		this.unidadeComercializada.clear();
		this.idModalidadePagamento.clear();
		this.idMotivo.clear();
		this.instituicaoFinanceira.clear();
		this.acordoAladi.clear();
		this.naladi.clear();


		this.model.idFornecedor = this.parametros.estadoFornecedor = this.parametros.cidadeFornecedor =
			this.parametros.numeroFornecedor = this.parametros.complementoFornecedor =
			this.parametros.logradouroFornecedor = this.parametros.codigoPaisFornecedor = null;

		this.model.idFabricante = this.parametros.cidadeFabricante =
			this.parametros.estadoFabricante = this.parametros.complementoFabricante =
			this.parametros.numeroFabricante = this.parametros.logradouroFabricante =
			this.parametros.codigoPaisFabricante = null;


	}

	incluirComplemento() {
		this.validarComplemento();

		var nomeFormularioComplemento = "formularioComplemento" + this.idPliMercadoria.toString();
		if (!this.validationService.form(nomeFormularioComplemento)) { return; }

		var nomeFormularioDetalhe = "formularioDetalhe" + this.idPliMercadoria.toString();
		if (!this.validationService.form(nomeFormularioDetalhe)) { return; }

		this.modelProcessoAnuente.idOrgaoAnuente = this.orgaoAnuente.valorInput.nativeElement.value.split('|')[0].trim();
		this.modelProcessoAnuente.descricao = this.orgaoAnuente.valorInput.nativeElement.value.split('|')[1].trim();

		this.modelProcessoAnuente.excluir = false;

		if (this.modelProcessoAnuente.index == undefined) {
			this.listaProcessoAnuente.push(JSON.parse(JSON.stringify(this.modelProcessoAnuente)));
		}
		else {
			this.listaProcessoAnuente[this.modelProcessoAnuente.index] = JSON.parse(JSON.stringify(this.modelProcessoAnuente));
		}

		this.model.listaPliProcessoAnuenteVM = this.listaProcessoAnuente;
		this.gridProcessoAnuente.lista = this.listaProcessoAnuente;
		this.limparCamposComplemento();
	}

	onChangeOpcaoTipoCobertura() {

		if (this.model.tipoCOBCambial == 1) {
			this.model.idInstituicaoFinanceira = undefined;
			this.model.idMotivo = undefined;

			this.instituicaoFinanceira.clear();
			this.idMotivo.clear();
		}
		else if (this.model.tipoCOBCambial == 2) {
			this.model.idInstituicaoFinanceira = undefined;
			this.model.idMotivo = undefined;
			this.model.numeroCOBCambialLimiteDiasPagamento = undefined;

			this.idMotivo.clear();
			this.instituicaoFinanceira.clear();
		}
		else if (this.model.tipoCOBCambial == 3) {
			this.model.idMotivo = undefined;
			this.model.numeroCOBCambialLimiteDiasPagamento = undefined;
			this.model.idModalidadePagamento = undefined;

			this.idMotivo.clear();
			this.idModalidadePagamento.clear();
		}
		else if (this.model.tipoCOBCambial == 4) {
			this.model.idModalidadePagamento = undefined;
			this.model.numeroCOBCambialLimiteDiasPagamento = undefined;
			this.model.idInstituicaoFinanceira = undefined;

			this.idModalidadePagamento.clear();
			this.instituicaoFinanceira.clear();
		}
	}

	limparCamposComplemento() {

		this.orgaoAnuente.clear();
		this.modelProcessoAnuente = new manterPliProcessoAnuenteVM();
	}


	validarComplemento() {
		this.numeroProcesso.nativeElement.setCustomValidity('');
		this.orgaoAnuente.valorInput.nativeElement.setCustomValidity('');

		if (this.numeroProcesso.nativeElement.value == "") {
			this.numeroProcesso.nativeElement.setCustomValidity('Preencha este campo.');

		} else if (this.orgaoAnuente.valorInput.nativeElement.value == "") {
			this.orgaoAnuente.valorInput.nativeElement.setCustomValidity('Preencha este campo.');
		}
	}

	limparCamposItemMercadoria() {

		this.valorUnitarioCondicaoVenda.nativeElement.value = "0,0000000"
		this.quantidadeUnidadeComercializada.nativeElement.value = "0,00000";
		this.valorCondicaoVendaInput.nativeElement.value = "";

		//this.itemMercadoria.clear();
		this.unidadeComercializada.clear();

		this.idUnidadeMedida = null;
		this.idDetalheMercadoria = null;

		this.modelDetalheMercadoria = new manterPliDetalheMercadoriaVM();
	}

	calcularValorCondicaoVenda() {

		this.parametrosValores.valor1 = this.quantidadeUnidadeComercializada.nativeElement.value;
		this.parametrosValores.valor2 = this.valorUnitarioCondicaoVenda.nativeElement.value;

		this.applicationService.get<ValoresVM>("Funcoes", this.parametrosValores).subscribe(result => {
			this.modelDetalheMercadoria.valorCondicaoVenda =
				result.valorTotalDecimal;

			this.modelDetalheMercadoria.quantidadeComercializada = result.valorQuantidade;
			this.modelDetalheMercadoria.valorUnitarioCondicaoVenda = result.valorCondicao;

			this.valorCondicaoVendaInput.nativeElement.value = result.valorTotalFormatado;
		});

		this.valorDecimal = "0";

		//this.modelDetalheMercadoria.valorCondicaoVenda =
		//	this.modelDetalheMercadoria.quantidadeComercializada *
		//	this.modelDetalheMercadoria.valorUnitarioCondicaoVenda;

		//this.valorCondicaoVendaInput.nativeElement.value =
		//	this.modelDetalheMercadoria.valorCondicaoVenda.toFixed(7);
	}

	calcularValorTotalCondicaoVenda(lista) {
		if (lista != undefined) {
			this.listaItemMercadoria = lista;
		}
		this.valorDecimalTotal = "0";
		var soma = 0;
		for (var i = 0; i < this.listaItemMercadoria.length; i++) {
			soma += (this.listaItemMercadoria[i].quantidadeComercializada * this.listaItemMercadoria[i].valorUnitarioCondicaoVenda);
		}
		this.valorDecimalTotal = soma.toFixed(2);
	}

	Anterior() {

		this.indexAtual = this.indexAtual == 0 ? 0 : this.indexAtual - 1;
		this.itemAtual = this.listaMercadorias[this.indexAtual];
		var manterPliMercadoria = JSON.parse(JSON.stringify(this.listaMercadorias));
		this.abrir(manterPliMercadoria[this.indexAtual],
			manterPliMercadoria[this.indexAtual].listaPliDetalheMercadoriaVM,
			manterPliMercadoria[this.indexAtual].listaPliProcessoAnuenteVM, this.parametros);
	}

	Proximo() {

		this.itemAtual = this.listaMercadorias[this.indexAtual];
		this.salvar(false);

	}

	public mascara() {
		if (this.txtCRA.nativeElement.value != undefined && this.txtCRA.nativeElement.value != "") {
			var numero = parseFloat(this.txtCRA.nativeElement.value);
			this.txtCRA.nativeElement.value = numero.toFixed(2).toString().replace('.', ',');
		}
	}

	public onKeyDown($event: KeyboardEvent) {
		const e = <KeyboardEvent>event;
		if (e.keyCode == 13) {
			e.preventDefault();
			return;
		}
	}

	habilitarFor() {
		this.somenteLeituraFor = false;
	}

	habilitarFab() {
		this.somenteLeituraFab = false;
	}

	formatarMascaraEntrar(mascara, valor) {		
	}

	formatarMascaraSair(mascara, componente, tamanhomascara) {
		var valor = "0";

		switch (componente) {
			case 1: { //peso liquido
				valor = this.pesoLiquido.nativeElement.value;
				break;
			}
			case 2: { //quantidade estatistica
				valor = this.quantidadeEstatistica.nativeElement.value;
				break;
			}
			case 3: { //quantidade unidade comercializada
				valor = this.quantidadeUnidadeComercializada.nativeElement.value;
				break;
			}
			case 4: { //valor unitario condicao venda
				valor = this.valorUnitarioCondicaoVenda.nativeElement.value;
				break;
			}
			default: break;
		}

		if (valor == undefined || valor == null || valor.toString().trim() == "") {
			valor = "0";
		}

		this.parametrosMascara.mascara = mascara;
		this.parametrosMascara.valor = valor;
		this.parametrosMascara.tamanhoMascara = tamanhomascara;

		this.applicationService.get<string>("Funcoes", this.parametrosMascara).subscribe(result => {

			switch (componente) {
				case 1: { //peso liquido

					if (result.indexOf("Valor") > -1) {
						this.modal.alerta(result, "", "");
						this.pesoLiquido.nativeElement.value = 0;
						this.model.pesoLiquidoString = "0";
					}
					else {
						this.pesoLiquido.nativeElement.value = result;
						this.model.pesoLiquidoString = result;
					}
					break;
				}
				case 2: { //quantidade estatistica

					if (result.indexOf("Valor") > -1) {
						this.modal.alerta(result, "", "");
						this.quantidadeEstatistica.nativeElement.value = 0;
						this.model.quantidadeEstatisticaString = "0";
					}
					else {
						this.quantidadeEstatistica.nativeElement.value = result;
						this.model.quantidadeEstatisticaString = result;
					}
					break;
				}
				case 3: { //quantidade unidade comercializada

					if (result.indexOf("Valor") > -1) {
						this.modal.alerta(result, "", "");
						this.quantidadeUnidadeComercializada.nativeElement.value = 0;
						this.modelDetalheMercadoria.quantidadeComercializadaFormatada = "0";
					}
					else {
						this.quantidadeUnidadeComercializada.nativeElement.value = result;
						this.modelDetalheMercadoria.quantidadeComercializadaFormatada = result;
					}

					this.calcularValorCondicaoVenda();
					
					break;

				}
				case 4: { //quantidade unidade comercializada

					if (result.indexOf("Valor") > -1) {
						this.modal.alerta(result, "", "");
						this.valorUnitarioCondicaoVenda.nativeElement.value = 0;
						this.modelDetalheMercadoria.valorUnitarioCondicaoVendaFormatada = "0";
					}
					else {
						this.valorUnitarioCondicaoVenda.nativeElement.value = result;
						this.modelDetalheMercadoria.valorUnitarioCondicaoVendaFormatada = result;
					}

					this.calcularValorCondicaoVenda();
			
					break;

				}


				default: break;
			}			
		});

		
	}

}
