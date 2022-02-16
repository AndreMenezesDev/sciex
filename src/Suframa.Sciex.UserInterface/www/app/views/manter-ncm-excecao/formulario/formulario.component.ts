import { Component, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedItems } from '../../../view-model/PagedItems';
import { ModalService } from '../../../shared/services/modal.service';
import { ArrayService } from '../../../shared/services/array.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { manterNcmExcecaoVM } from "../../../view-model/ManterNcmExcecaoVM";
import { viewMunicipioVM } from "../../../view-model/ViewMunicipioVM";

declare var $: any;

@Component({
	selector: 'app-manter-ncm-excecao-formulario',
	templateUrl: './formulario.component.html'
})

export class ManterNCMExcecaoFormularioComponent implements OnInit {
	isVisibleFieldCod = false;
	parametros: any = {};
	isTableMunicipio: boolean = false;
	path: string;
	titulo: string;
	tituloPanel: string;
	desabilitado: boolean;
	servicoNCM = 'NCM';
	servicoNCMExcecao = 'NcmExcecao';
	servicoNCMGrid = 'NCMGrid';
	servicoNCMExcecaoGrid = 'NcmExcecaoGrid';
	servicoViewMunicipioGrid = 'ViewMunicipioGrid';
	habilitarCampoCodigo: boolean;
	validar: boolean = false;
	model: manterNcmExcecaoVM = new manterNcmExcecaoVM();
	paramodel: manterNcmExcecaoVM = new manterNcmExcecaoVM();
	codigo1: string;
	isCancelarVisible: boolean;
	listaFiltrada: Array<viewMunicipioVM>;
	listaSelecionada: Array<manterNcmExcecaoVM>;
	listaSelecionadaAux: Array<manterNcmExcecaoVM>;
	listaSelecionadaFiltrada: Array<manterNcmExcecaoVM>;
	listaAuxiliar: Array<viewMunicipioVM>;
	lista: Array<viewMunicipioVM>;
	checkAll1: any;
	checkAll2: any;
	check1 = [];
	check2 = [];
	siglaUf: any;
	isDisplay: boolean = false;
	arrayhelp = [];
	arrayhelp2 = [];
	isAdding: boolean = true;
	isOnChanging: boolean = true;
	totalEncontrado: number;
	totalEncontradoMunicipiosSelecionados: number;
	ufAnterior: string;
	habilitaUF: boolean;
	habilitaData: boolean;
	habilitaSetor: boolean;
	@ViewChild('formulario') formulario;
	@ViewChild('codigo') codigo;
	@ViewChild('checkedMunicipio') checkedMunicipio;
	@ViewChild('checkedSelecionada') checkedSelecionada;
	@ViewChild('filtroMunicipio') filtroMunicipio;
	@ViewChild('filtroMunicipioAssociados') filtroMunicipioAssociados;
	@ViewChild('ncm') ncm;
	@ViewChild('rchk') rchk;
	@ViewChild('uf') uf;
	@ViewChild('setor') setor;
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
		this.listaSelecionada = new Array<manterNcmExcecaoVM>();
		this.listaSelecionadaFiltrada = new Array<manterNcmExcecaoVM>();
	}

	ngOnInit() {
		this.totalEncontrado = 0;
		this.listaAuxiliar = new Array<viewMunicipioVM>();
		this.listaSelecionadaAux = new Array<manterNcmExcecaoVM>();
	}

	limparItens() {
		this.uf.clear();
		this.setor.clear();
		this.dataInicioVigencia.nativeElementValue = "";
	}

	limparMunicipiosSelecionados() {
		this.listaFiltrada = this.lista;
	}

	limparMunicipiosAssociados() {
		this.listaSelecionada = this.listaSelecionadaAux;
	}

	onChangeCheckAllGridMunicipio() {
		for (var i = 0; i < this.listaFiltrada.length; i++) {

			if (this.checkedMunicipio.nativeElement.checked == true) {

				this.listaFiltrada[i].checkbox = true;
				this.checkAll1 = true;

			} else {

				this.listaFiltrada[i].checkbox = false;
				this.checkAll1 = false;

			}

		}

	}

	onChangeCheckAllGridMunicipioSelecionada() {
		for (var i = 0; i < this.listaSelecionada.length; i++) {
			if (this.checkedSelecionada.nativeElement.checked == true) {

				this.listaSelecionada[i].checkbox = true;
				this.checkAll2 = true;

			} else {
				this.listaSelecionada[i].checkbox = false;
				this.checkAll2 = false;
			}

		}

	}

	addItem() {
		if (this.listaFiltrada == undefined || this.listaFiltrada.length == 0) {
			this.modal.alerta('Selecione um Estado e um ou mais Municípios.');
		}
		else if ((this.listaFiltrada.filter(o => o.checkbox == true).length == 0)) {
			this.modal.alerta('Selecione pelo menos um município da lista.');
		}
		else {

			this.totalEncontrado = this.listaFiltrada.length;
			this.checkedMunicipio.nativeElement.checked = false;
			this.checkedSelecionada.nativeElement.checked = false;

			for (var i = 0; i < this.listaFiltrada.length; i++) {

				if (this.listaFiltrada[i].checkbox == true) {
					let ms = new manterNcmExcecaoVM();
					ms.checkbox = false;
					ms.codigoMunicipio = this.listaFiltrada[i].codigoMunicipio;
					ms.descricaoMunicipio = this.listaFiltrada[i].descricao;
					ms.uf = this.listaFiltrada[i].siglaUF;
					ms.status = 1;
					this.listaSelecionada.push(ms);
				}

			}

			this.listaAuxiliar = new Array<viewMunicipioVM>();
			this.carregarGrid(false);

		}

	}

	validarRemoverItemSelecionados() {

		let validado = true;
		let arraysize = this.listaSelecionada.length;

		if (this.listaSelecionada == undefined || this.listaSelecionada.length == 0) {
			validado = false;
			this.modal.alerta('Selecione um ou mais Municípios.');


		} else {
			let exibirMensagem = false;
			for (var i = 0; i < arraysize; i++) {
				if (this.listaSelecionada[i].checkbox == true) {
					exibirMensagem = true;
					break;
				}
			}

			if (!exibirMensagem) {
				validado = false;
				this.modal.alerta('Selecione pelo menos um município da lista.');
			}
		}
		return validado;
	}

	removeItem() {
		if (this.validarRemoverItemSelecionados()) {
			let arraysize = this.listaSelecionada.length;
			for (var i = 0; i < arraysize; i++) {
				if (this.listaSelecionada[i].checkbox == true) {

					this.listaSelecionada[i].status = 0;
					let ms = new viewMunicipioVM();
					ms.checkbox = false;
					ms.codigoMunicipio = this.listaSelecionada[i].codigoMunicipio;
					ms.descricao = this.listaSelecionada[i].descricaoMunicipio;
					this.listaAuxiliar.push(ms);
					this.arrayhelp[i] = i;
				}
			}

			if (this.checkedSelecionada.nativeElement.checked == true || this.listaSelecionada.length == this.listaAuxiliar.length) {
				this.listaSelecionada = new Array<manterNcmExcecaoVM>();
			} else {
				var arraytam = this.listaSelecionada.length - 1;
				for (i = arraytam; i >= 0; i--) {
					if (this.listaSelecionada[i] != undefined) {
						if (this.listaSelecionada[i].checkbox == true && this.arrayhelp[i] == i) {
							let ncm = this.listaSelecionada[i];
							ncm.status = 0;
							this.listaSelecionada[i] = ncm;
							this.listaSelecionada = this.listaSelecionada.filter(o => o.status == 1);
							//this.listaSelecionada.splice(this.arrayhelp[i], 1);
						} else {
							this.listaSelecionada[i].checkbox = false;
						}
					}
				}

			}

			this.listaAuxiliar = new Array<viewMunicipioVM>();
			this.checkedMunicipio.nativeElement.checked = false;
			this.checkedSelecionada.nativeElement.checked = false;

			this.carregarGrid(false);
		}

	}

	removeCheckAll() {
		this.checkedMunicipio.nativeElement.checked = false;
		this.checkedSelecionada.nativeElement.checked = false;
	}

	filtrarListaMunicipio() {
		this.listaFiltrada = this.lista.filter(o => o.descricao.toLowerCase().indexOf(this.filtroMunicipio.nativeElement.value.toLowerCase()) > -1)
		for (var i = 0; i < this.listaFiltrada.length; i++) {
			this.listaFiltrada[i].checkbox = false;
		}
		this.checkAll1 = false;
		for (var i = 0; i < this.lista.length; i++) {
			this.lista[i].checkbox = false;
		}
		if (this.check1.length > 0) {
			for (var i = 0; i < this.check1.length; i++) {
				this.check1[i] = 0;
			}
		}
		this.totalEncontrado = this.listaFiltrada.length;
	}

	filtrarListaMunicipioAssociados() {
		this.listaSelecionada = this.listaSelecionadaAux.filter(o => o.descricaoMunicipio.toLowerCase().indexOf(this.filtroMunicipioAssociados.nativeElement.value.toLowerCase()) > -1)
	}

	carregarGrid(atualizarListaAssociados: boolean) {

		this.isOnChanging = false;
		this.parametros.siglaUF = this.siglaUf;
		this.totalEncontrado = 0;
		this.applicationService.get(this.servicoViewMunicipioGrid, this.parametros).subscribe(result => {

			this.listaFiltrada = this.lista = JSON.parse(JSON.stringify(result));
			this.totalEncontrado = this.listaFiltrada.length;

			if (this.totalEncontrado > 0) {
				if (atualizarListaAssociados || this.siglaUf != this.ufAnterior) {
					this.carregarGridMunicipiosAssociados();
				}
				else {

					this.carregarItensFiltradosPorSelecionado(this.listaSelecionada, this.listaFiltrada);
				}
			}

			//ordenar lista selecionada
			this.listaSelecionada.sort(function (a, b) {
				if (a.descricaoMunicipio != undefined && b.descricaoMunicipio != undefined) {

					var mapaAcentosHex = {
						a: /[\xE0-\xE6]/g,
						A: /[\xC0-\xC6]/g,
						e: /[\xE8-\xEB]/g,
						E: /[\xC8-\xCB]/g,
						i: /[\xEC-\xEF]/g,
						I: /[\xCC-\xCF]/g,
						o: /[\xF2-\xF6]/g,
						O: /[\xD2-\xD6]/g,
						u: /[\xF9-\xFC]/g,
						U: /[\xD9-\xDC]/g,
						c: /\xE7/g,
						C: /\xC7/g,
						n: /\xF1/g,
						N: /\xD1/g,
					};

					var string1 = a.descricaoMunicipio;
					var string2 = b.descricaoMunicipio;
					
					for (var letra in mapaAcentosHex) {
						var expressaoRegular = mapaAcentosHex[letra];
						string1 = string1.replace(expressaoRegular, letra);
					}

					for (var letra in mapaAcentosHex) {
						var expressaoRegular = mapaAcentosHex[letra];
						string2 = string2.replace(expressaoRegular, letra);
					}
										
					if (string1 < string2) { return -1; }
					if (string1 > string2) { return 1; }
				}
				return 0;
			});
			this.listaSelecionadaAux = this.listaSelecionada;

			this.ufAnterior = this.siglaUf;
		});

	}

	carregarItensFiltradosPorSelecionado(listaSelecionada, listaFiltrada) {
		for (var i = 0; i < listaSelecionada.length; i++) {

			this.check2[i] = 0;
			for (var j = 0; j < listaFiltrada.length; j++) {
				this.check1[j] = 0;
				if (listaFiltrada[j].descricao.toLowerCase() == listaSelecionada[i].descricaoMunicipio.toLowerCase()) {
					listaFiltrada.splice(j, 1);

					this.totalEncontrado = this.listaFiltrada.length;
					break;

				}
			}
		}
		this.listaSelecionadaAux = this.listaSelecionada;
	}

	carregarGridMunicipiosAssociados() {

		this.parametros.Codigo = this.ncm.valorInput.nativeElement.value.split("|")[0];
		this.parametros.UF = this.uf.select.nativeElement.value;
		this.parametros.DescricaoSetor = this.model.descricaoSetor;
		this.parametros.Status = 1;

		this.applicationService.get(this.servicoNCMExcecao, this.parametros).subscribe((result) => {
			this.listaSelecionada = new Array<manterNcmExcecaoVM>();

			this.listaSelecionada = JSON.parse(JSON.stringify(result));
			this.carregarItensFiltradosPorSelecionado(this.listaSelecionada, this.listaFiltrada);
		});

	}

	public onKeyUp($event: KeyboardEvent) {
		const e = <KeyboardEvent>event;
		if (e.keyCode == 8) {
			this.dataInicioVigencia.nativeElement.value = "";
			this.dataInicioVigencia.nativeElement.blur();
			this.dataInicioVigencia.nativeElement.focus();
		}
		this.dataInicioVigencia.nativeElement.setCustomValidity('');
	}

	public verificarRota() {

		this.habilitarCampoCodigo = false;
		this.tituloPanel = 'Formulário';
		this.isCancelarVisible = true;

		if (this.path == 'visualizar') {
			this.tituloPanel = 'Registros';
			this.isCancelarVisible = false;
		}

		if (this.path == 'editar' || this.path == 'visualizar') {
			this.habilitarCampoCodigo = true;
		}

		if (this.path == 'editar') {
			this.titulo = 'Alterar';

		} else {
			this.titulo = this.path[0].toUpperCase() + this.path.substr(1, this.path.length - 1);
		}

	}

	public getSelectedOptionText(event: Event) {

		let selectedOptions = event.target['options'];
		let selectedIndex = selectedOptions.selectedIndex;
		let selectElementText = selectedOptions[selectedIndex].text;
		this.model.descricaoSetor = selectElementText;
		this.habilitaUF = true;
		if (this.uf.select.nativeElement.value != "")
			this.carregarGridMunicipiosAssociados();

	}

	public salvar() {

		var data = new Date(),
			dia = data.getDate().toString(),
			diaF = (dia.length == 1) ? '0' + dia : dia,
			mes = (data.getMonth() + 1).toString(),
			mesF = (mes.length == 1) ? '0' + mes : mes,
			anoF = data.getFullYear();
		var dataAtual = anoF + "-" + mesF + "-" + diaF;

		if (new Date(this.model.dataInicioVigencia) <= new Date(dataAtual)) {
			this.dataInicioVigencia.nativeElement.setCustomValidity('A data da vigência deverá ser superior a data atual');
		} else {
			this.dataInicioVigencia.nativeElement.setCustomValidity('');
		}


		this.model.codigo = this.ncm.valorInput.nativeElement.value.split(" | ")[0];
		this.model.descricaoNcm = this.ncm.valorInput.nativeElement.value.split(" | ")[1];
		this.model.listaMunicipios = this.listaSelecionadaFiltrada.length == 0 ? this.listaSelecionada : this.listaSelecionadaFiltrada;
		this.model.status = 1;
		this.model.uf = this.siglaUf;

		if (!this.validationService.form('formulario')) { return; }
		if (!this.formulario.valid) { return; }

		if (this.listaSelecionada == undefined || this.listaSelecionada.length == 0) {
			this.modal.alerta('Selecione um ou mais Municípios.');
			return;
		}

		if (this.model.codigo == 0) {
			if (this.codigo != "" && this.model.descricaoNcm != "") {
				this.paramodel.codigo = this.ncm.valorInput.nativeElement.value.split("|")[0];

				this.applicationService.get(this.servicoNCMExcecaoGrid, this.paramodel).subscribe((result: PagedItems) => {
					if (result.total > 0) {
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
									this.applicationService.get(this.servicoNCMExcecaoGrid, this.parametros).subscribe((result: PagedItems) => {
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

										}
									});
								}
							});
					}

				});
			}
		} else {
			this.salvarRegistro();
		}
	}

	private salvarRegistro() {

		this.applicationService.put<manterNcmExcecaoVM>(this.servicoNCMExcecao, this.model).subscribe(result => {

			if (result.mensagemErro != null) {

				this.modal.alerta(result.mensagemErro, "Informação", "");

			} else {

				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "/manter-ncm-excecao");
				if (this.path != "editar")
					localStorage.clear();
			}

		});

	}

	public cancelarOperacao() {

		this.modal.confirmacao(this.msg.CONFIRMAR_CONFIRMACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.router.navigate(['/manter-ncm-excecao']);
				}
			});

	}

	

}
