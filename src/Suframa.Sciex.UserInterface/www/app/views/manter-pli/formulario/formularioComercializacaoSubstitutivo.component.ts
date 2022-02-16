import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { manterPliVM } from '../../../view-model/ManterPliVM';
import { manterPliMercadoriaVM } from '../../../view-model/ManterPliMercadoriaVM';
import { manterPliProdutoVM } from '../../../view-model/ManterPliProdutoVM';

declare var $: any;

@Component({
	selector: 'app-manter-pli-formulario-comercializacao-substitutivo',
	templateUrl: './formularioComercializacaoSubstitutivo.component.html'
})

export class ManterPliFormularioComercializacaoSubstitutivoComponent implements OnInit {
	path: string;
	titulo: string;
	tituloPanel: string;
	subtituloPainel: string
	desabilitado: boolean;
	servicoPli = "Pli";
	servicoPliProduto = "PliProduto"
	servicoPliMercadoria = "PliMercadoria";
	servicoPliMercadoriaGrid = "PliMercadoriaGrid";
	servicoViewPliProduto = "ViewPliProduto";		
	servicoPliGrid = "PliGrid";
	somenteLeitura: boolean;
	validar: boolean = false;		
	modelPli: manterPliVM = new manterPliVM();
	modelPliProduto: manterPliProdutoVM = new manterPliProdutoVM();
	modelPliMercadoria: manterPliMercadoriaVM = new manterPliMercadoriaVM();
	modelPliMercadoriaGrid: manterPliMercadoriaVM[] = [];
	codigo1: string;
	tipoManterPli: string;
	isCancelarVisible: boolean;
	parametros: any = {};
	grid: any = { sort: {} };
	listaProdutos: any;
	listaMercadorias: any;
	isShowPanel: boolean;
	descricaoTipoPli: string;
	isPlus: boolean = true;
	idPliProdutoOpen: number;
	CNPJ: string;
	cpfDigitado: string;
	mostrarBotao: boolean;

	@ViewChild('produto') produto;
	@ViewChild('mercadoria') mercadoria;
	@ViewChild('formulario') formulario;
	@ViewChild('codigo') codigo;
	@ViewChild('tipoPli') tipoPli;
	@ViewChild('appModalMercadoriaPli') appModalMercadoriaPli;
	@ViewChild('closeAll') closeAll;
	@ViewChild('optioncheckedTA') optioncheckedTA;
	@ViewChild('cpf') cpf;

	constructor(
		private route: ActivatedRoute,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private modal: ModalService,
		private router: Router,
		private validationService: ValidationService,
	) {
		this.path = this.route.snapshot.url[this.route.snapshot.url.length - 1].path;
		this.mostrarBotao = false;
	}

	ngOnInit() {
		this.verificarRota();
		$(document).ready(function () {
			$("#collapse-init").click(function () {
				$('.accordion-toggle').collapse('hide');
				$('.panel-collapse').collapse('hide');
			});
		});

		this.isShowPanel = false;
		if (localStorage.getItem('manter-pli') != null) {
			this.modelPli = JSON.parse(localStorage.getItem('manter-pli'));

			this.applicationService.get<manterPliVM>(this.servicoPli, this.modelPli.idPLI).subscribe(result => {
				this.modelPli = result;

				var numeroPli = this.modelPli.ano.toString() + '/' + ("000000" + this.modelPli.numeroPli.toString()).slice(-6);
				this.modelPli.numeroPLI = numeroPli;
				this.modelPli.numCPFRepLegalSISCO = this.mascararCPF(this.modelPli.numCPFRepLegalSISCO);
				this.descricaoTipoPli = "SUBSTITUTIVO"; // modificar para modo dinamico
			});
			localStorage.removeItem('manter-pli');
		}

		this.applicationService.get("UsuarioLogado", { cnpj: true }).subscribe((result: any) => {
			if (result != null)
				this.CNPJ = result;
		});

	}

	public verificarRota() {
		this.somenteLeitura = false;
		this.tituloPanel = 'Formulário';
		this.isCancelarVisible = true;

		this.desabilitarTela();

		this.tituloPanel = 'Dados do PLI';
		this.subtituloPainel = 'Adicione Mercadorias no PLI';
		this.isCancelarVisible = false;

		if (this.path == 'visualizarcomercializacaosubstitutivo') {
			this.selecionar(this.route.snapshot.params['id']);
			this.titulo = 'Visualizar'
			this.somenteLeitura = this.parametros.somenteLeitura = true;
			this.mostrarBotao = false;
		}
		else if (this.path == 'editarcomercializacaosubstitutivo') {
			this.selecionar(this.route.snapshot.params['id']);
			this.titulo = 'Alterar';
			this.mostrarBotao = true;
		}
		else if (this.path == 'cadastrarcomercializacaosubstitutivo') {
			this.selecionar(this.route.snapshot.params['id']);
			this.titulo = 'Cadastrar';
			this.mostrarBotao = true;
		}
		else {
			this.titulo = this.path[0].toUpperCase() + this.path.substr(1, this.path.length - 1);
		}

	}	

	public onBlurEvent() {
		if (this.codigo.nativeElement.value != undefined)
			if (this.codigo.nativeElement.value.length != undefined)
				if (this.codigo.nativeElement.value.length > 0) {

					this.codigo1 = this.codigo.nativeElement.value;
					this.codigo1 = ("000000" + this.codigo1).slice(-6);
				}
	}

	public desabilitarTela() {
		this.desabilitado = true;
	}

	public selecionar(id: number) {
		if (!id) { return; }
		this.listaProdutos = null;
		this.applicationService.get<manterPliVM>(this.servicoPli, id).subscribe(result => {

			this.modelPli = result;
			this.descricaoTipoPli = "SUBSTITUTIVO"; // modificar para modo dinamico

			var numeroPli = this.modelPli.ano + '/' + ("000000" + this.modelPli.numeroPli).slice(-6);
			this.modelPli.numeroPLI = numeroPli;
			this.parametros.idPLI = this.modelPli.idPLI;
			this.parametros.codigoPLIStatus = result.codigoPLIStatus;
			this.parametros.idPLIAplicacao = result.idPLIAplicacao;


			this.optioncheckedTA.nativeElement.checked =
				(result.statusPliTecnologiaAssistiva == 1 ? true : false);

			this.modelPli.numCPFRepLegalSISCO = this.mascararCPF(this.modelPli.numCPFRepLegalSISCO);

			this.applicationService.get(this.servicoPliMercadoriaGrid, this.parametros).subscribe((result: PagedItems) => {
				this.grid.total = result.total;
				for (let child of result.items) {
					child.numeroLIReferencia = this.modelPli.numeroLIReferencia;
				}
				this.grid.lista = result.items;
			});
		});
	}

	adicionarMercadoria() {
		
		this.mercadoria.valorInput.nativeElement.setCustomValidity('');

		if (this.grid.lista.length > 0) {
			this.modal.alerta("Não é possivel adicionar mais de um Produto para PLI Substitutivo!", "");
			return;
		}
		
		if (this.parametros.idMercadoria == undefined || this.parametros.idMercadoria == null) {
			this.mercadoria.valorInput.nativeElement.setCustomValidity('Preencha este campo.');
		}
		
		if (!this.validationService.form('formulario')) { return; }
		if (!this.formulario.valid) { return; }

		this.modelPliMercadoria.idPLI = this.parametros.idPLI;
		this.modelPliMercadoria.idMercadoria = this.parametros.idMercadoria;
		this.modelPliMercadoria.idPLIAplicacao = this.modelPli.idPLIAplicacao;
		this.parametros.idPLIAplicacao = this.modelPli.idPLIAplicacao;


		this.applicationService.put<manterPliMercadoriaVM>(this.servicoPliMercadoria, this.modelPliMercadoria).subscribe(result => {
			if (result.mensagemErro != "" && result.mensagemErro != null) {
				this.modal.alerta(result.mensagemErro, "Informação", "");
			} else {
				this.parametros.fields = ["codigoNCMMercadoria", "descricaoNCMMercadoria"];
				
				this.applicationService.get(this.servicoPliMercadoriaGrid, this.parametros).subscribe((result: PagedItems) => {
					this.grid.total = result.total;
					this.grid.lista = result.items;

					//Limpar componente select 2
					this.mercadoria.clearInput = true;
					let element: HTMLElement = document.getElementById('valorInput') as HTMLElement;
					element.click();
					this.mercadoria.clearInput = false;

				});
			}
		});
	}

	abrirListaMercadorias(idPliProduto) {
		this.idPliProdutoOpen = idPliProduto;
		this.parametros.modelProduto = this.modelPliProduto;
		this.parametros.idPliProduto = idPliProduto;


		if ($("#" + idPliProduto).hasClass('fa-plus-square-o')) {
			$(".a").removeClass('fa-minus-square-o');
			$(".a").addClass('fa-plus-square-o');
			$("#" + idPliProduto).removeClass('fa-plus-square-o');
			$("#" + idPliProduto).addClass('fa-minus-square-o');
		} else {
			$(".a").removeClass('fa-minus-square-o');
			$(".a").addClass('fa-plus-square-o');
			$("#" + idPliProduto).removeClass('fa-minus-square-o');
			$("#" + idPliProduto).addClass('fa-plus-square-o');
		}

		$('.accordion-toggle').collapse('hide');
		$('.panel-collapse').collapse('hide');

		this.parametros.fields = ["codigoNCMMercadoria", "descricaoNCMMercadoria"];
		this.applicationService.get(this.servicoPliMercadoriaGrid, this.parametros).subscribe((result: PagedItems) => {
			this.grid.total = result.total;
			this.grid.lista = result.items;			
		});
	}

	mascararCPF(documentoInformado) {
		var cpf = documentoInformado;
		
		cpf = cpf.replace(/\D/g, "");
		cpf = cpf.replace(/(\d{3})(\d)/, "$1.$2");
		cpf = cpf.replace(/(\d{3})(\d)/, "$1.$2");
		cpf = cpf.replace(/(\d{3})(\d{1,2})$/, "$1-$2");

		return cpf;
	}

	atualizarCPFTecnologia() {

		this.cpfDigitado = this.modelPli.numCPFRepLegalSISCO;
		if (this.validaCPF(this.removerCaractere(this.cpfDigitado)) == false) {
			this.cpf.nativeElement.setCustomValidity('CPF Inválido');
		}

		if (!this.validationService.form('formularioB')) { return; }

		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {

					this.applicationService.get<manterPliVM>(this.servicoPli, this.modelPli.idPLI).subscribe(result => {
						this.modelPli = result;

						this.modelPli.statusPliTecnologiaAssistiva =
							(this.optioncheckedTA.nativeElement.checked ? 1 : 0);
						this.modelPli.numCPFRepLegalSISCO = this.removerCaractere(this.cpfDigitado);

						this.applicationService.put<manterPliVM>(this.servicoPli, this.modelPli).subscribe(result => {
							this.modelPli = result;

							var numeroPli = this.modelPli.ano.toString() + '/' + ("000000" + this.modelPli.numeroPli.toString()).slice(-6);
							this.modelPli.numeroPLI = numeroPli;
							this.modelPli.numCPFRepLegalSISCO = this.mascararCPF(this.modelPli.numCPFRepLegalSISCO);
							this.descricaoTipoPli = "SUBSTITUTIVO"; // modificar para modo dinamico
						});
					});
				}
			});
	}

	public validaCPF(cpfDigitado) {
		var Soma;
		var Resto;
		var i;
		Soma = 0;

		this.cpf.nativeElement.setCustomValidity('');

		//Invalida CPF menor que 11 dígitos
		if (this.cpf.nativeElement.value.length < 11) { return false; }

		//Invalida CPF com dígitos semelhantes
		if ((cpfDigitado == "00000000000") ||
			cpfDigitado == "11111111111" ||
			cpfDigitado == "22222222222" ||
			cpfDigitado == "33333333333" ||
			cpfDigitado == "44444444444" ||
			cpfDigitado == "55555555555" ||
			cpfDigitado == "66666666666" ||
			cpfDigitado == "77777777777" ||
			cpfDigitado == "88888888888" ||
			cpfDigitado == "99999999999") { return false; }

		for (i = 1; i <= 9; i++) Soma = Soma + parseInt(cpfDigitado.substring(i - 1, i)) * (11 - i);
		Resto = (Soma * 10) % 11;

		if ((Resto == 10) || (Resto == 11)) Resto = 0;
		if (Resto != parseInt(cpfDigitado.substring(9, 10))) { return false; }

		Soma = 0;
		for (i = 1; i <= 10; i++) Soma = Soma + parseInt(cpfDigitado.substring(i - 1, i)) * (12 - i);
		Resto = (Soma * 10) % 11;

		if ((Resto == 10) || (Resto == 11)) Resto = 0;
		if (Resto != parseInt(cpfDigitado.substring(10, 11))) { return false; }

		if (this.cpf.nativeElement.value.length == 0) { return false }

		return true;
	}

	removerHint() {
		if (this.cpf.nativeElement.value.length > 0) {
			this.cpf.nativeElement.setCustomValidity('');
		}
	}

	removerCaractere(documento) {
		var nomeDocumento = "";
		for (var i = 0; i < documento.length; i++) {
			if (documento[i] != "." && documento[i] != "-" && documento[i] != "/") {
				nomeDocumento = nomeDocumento + documento[i];
			}
		}
		return nomeDocumento;
	}
}
