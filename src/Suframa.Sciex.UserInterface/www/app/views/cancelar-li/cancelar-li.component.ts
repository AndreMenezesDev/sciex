import { Component, OnInit, Injectable, ViewChild, Output, EventEmitter } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router } from '@angular/router';
import { ValidationService } from "../../shared/services/validation.service";
import { manterPliVM } from '../../view-model/ManterPliVM';
import { forEach } from '@angular/router/src/utils/collection';
import { ManterCancelarLiGridComponent } from './grid/grid.component';

@Component({
	selector: 'app-cancelar-li',
	templateUrl: './cancelar-li.component.html',
	providers: [ManterCancelarLiGridComponent]
})

@Injectable()
export class ManterCancelarLiComponent implements OnInit {
	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	ocultarbotaocheck: boolean = false;
	ordenarGrid: boolean = true;
	servicoCancelarLiGrid = 'CancelarLiGrid';
	servicoCancelarLi = 'CancelarLi';

    @ViewChild('dataInicio') dataInicio;
    @ViewChild('dataFim') dataFim;
    @ViewChild('statusLI') statusLI;
	@ViewChild('inscricaoCadastral') inscricaoCadastral;
	@ViewChild('empresa') empresa;
	@ViewChild('npli') npli;
	@ViewChild('nli') nli;
	@ViewChild('dataEnvioInicial') dataEnvioInicial;
	@ViewChild('dataEnvioFinal') dataEnvioFinal;
	@ViewChild('appModalJustificativaReprocessar') appModalJustificativaReprocessar;
	@ViewChild('grid') grid1;
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

	constructor(
		private applicationService: ApplicationService,
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
		private ManterConsultarPliGridComponent: ManterCancelarLiGridComponent
	) {

		if (
			(localStorage.getItem(this.router.url) == null ||
				localStorage.getItem(this.router.url) == "" ||
				localStorage.getItem(this.router.url) == undefined)
			&& localStorage.length > 0) {
			localStorage.clear();
			this.parametros = null;
		}
	}

	ngOnInit(): void {

		this.ocultarBotaoReprocessar = false;

		this.retornaValorSessao();
		
		if (this.parametros != undefined || this.parametros != null) {

			this.grid.page = this.parametros.page;
			this.grid.size = this.parametros.size;
			this.grid.sort.field = this.parametros.sort;
			this.grid.sort.reverse = this.parametros.reverse;

			if (this.parametros.codigo != "-1"){
				localStorage.removeItem(this.router.url);
			}

			this.listar();
		}
		else {

			this.parametros = {};
			this.parametros.servico = this.servicoCancelarLiGrid;
			this.parametros.titulo = "CANCELAR LI";
			this.parametros.width = { 0: { columnWidth: 80 }, 1: { columnWidth: 80 },2: { columnWidth: 80 }, 3: { columnWidth: 80 }, 4: { columnWidth: 200 }, 5: { columnWidth: 80 }, 6: { columnWidth: 220 } };
			this.parametros.columns = ["N° PLI", "N° LI", "N° de LI de Referência", "NCM", "Descrição NCM", "Data Cadastro", "Status"];
			this.parametros.fields = ["numeroPLIFormatado", "numeroLi","numeroReferencia", "numeroNCM", "descricaoNCMMercadoria", "dataCadastroFormatado", "descricaoStatusAcao"];

			if (this.parametros.statusPli == null)
				this.parametros.statusPli = 0;

			this.parametros.idLiStatus = 0;

			let dat = new Date();
			this.parametros.dataInicio = dat.toLocaleDateString('en-CA');
			this.parametros.dataFim = dat.toLocaleDateString('en-CA');
		}

		
	}

	retornaValorSessao() {

		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;
		}
	}

	public onChangeTipoStatus() {

		if (this.parametros.statusPli == 22) {
			this.isGeracaoDebito = false;
			this.isAnaliseVisual = false;
			this.isAguardandoProcessamento = false;
			this.isProcessado = false;
		} else if (this.parametros.statusPli == 23) {
			this.isGeracaoDebito = false;
			this.isAnaliseVisual = true;
			this.isAguardandoProcessamento = false;
			this.isProcessado = false;
		} else if (this.parametros.statusPli == 24) {
			this.isGeracaoDebito = false;
			this.isAnaliseVisual = false;
			this.isAguardandoProcessamento = true;
			this.isProcessado = false;
		} else if (this.parametros.statusPli == 25) {
			this.isGeracaoDebito = false;
			this.isAnaliseVisual = false;
			this.isAguardandoProcessamento = false;
			this.isProcessado = true;
		} else {
			this.isGeracaoDebito = true;
			this.isAnaliseVisual = true;
			this.isAguardandoProcessamento = true;
			this.isProcessado = true;
		}
	}

	// validacaoData(){
	// 	let dataInicial = this.dataInicio.nativeElement.value;
	// 	let dataFinal = this.dataFim.nativeElement.value;
	// 	if (dataInicial != '' && dataFinal != ''){
	// 		if (dataFinal > dataInicial){
	// 			let periodo = dataFinal - dataInicial;
	// 			if (periodo <=30){
	// 				return true;
	// 			}
	// 			else{
	// 				this.modal.alerta("Período não pode ser maior que 30 dias", 'Informação');
	// 				return false;
	// 			}
	// 		}
	// 		else{
	// 			this.modal.alerta('Campo inválido. Data inicio maior que data final');
	// 			return false;
	// 		}
	// 	}
	// 	else{
	// 		false;
	// 	}
	// }

	buscar(exibirMensagem) {

		//if (!this.validationService.form('formBusca')) { return; }
		let dataInicial = this.dataInicio.nativeElement.value;
		let dataFinal = this.dataFim.nativeElement.value;


        if (
			(this.statusLI.select.nativeElement.value != '' && this.statusLI.select.nativeElement.value != '0')
			||
			((dataInicial != '' && dataFinal != ''))
			|| (this.npli.nativeElement.value != '')
			|| (this.nli.nativeElement.value != '')
           )
        {
            if (exibirMensagem) {
                this.isModificouPesquisa = true;
            }
            else {
                this.isBuscaSalva = true;
            }

            if (this.validarData()) {
                this.listar();
            }
		}
        else
        {
            if (exibirMensagem) {
                this.modal.alerta(this.msg.INFORME_OS_FILTROS_DE_PESQUISA, 'Informação');
            } else {
                if (this.isBuscaSalva) {
                    if (this.validarData()) {
                        this.listar();
                    }
                }
            }
		}
    }

    validarData(): any {

        try {
            this.dataInicio.nativeElement.setCustomValidity('');
            this.dataFim.nativeElement.setCustomValidity('');

            var dataFim = new Date(this.dataInicio.nativeElement.value);
            var dataInicio = new Date(this.dataFim.nativeElement.value);

            var dias = ((dataInicio.getTime()-dataFim.getTime()) / 86400000);
            if (dias > 30) {
                this.modal.alerta("Período não pode ser maior que 30 dias", 'Informação');
                return false;
            }
            if (this.dataInicio.nativeElement.value.length > 0 || this.dataFim.nativeElement.value.length > 0) {
                if (this.dataInicio.nativeElement.value.length >= 0 && this.dataInicio.nativeElement.value.length < 10) {
                    this.dataInicio.nativeElement.setCustomValidity('Campo inválido');
                } else if (this.dataFim.nativeElement.value.length >= 0 && this.dataFim.nativeElement.value.length < 10) {
                    this.dataFim.nativeElement.setCustomValidity('Campo inválido');

                } else if (new Date(this.parametros.dataFim) < new Date(this.parametros.dataInicio)) {
                    this.dataFim.nativeElement.setCustomValidity('Campo inválido. Data inicio maior que data final');
                }
            }

            return true;
        } catch (e) {

            this.dataInicio.nativeElement.setCustomValidity('Informe uma data válida.');
        }

    }

	ocultar() {
		if (this.ocultarFiltro === false)
			this.ocultarFiltro = true;
		else
			this.ocultarFiltro = false;
	}

	onChangeSort($event) {

		this.grid.sort = $event;
	}

	onChangeSize($event) {
		this.grid.size = $event;
	}

	onChangePage($event) {
		this.grid.page = $event;
		this.buscar(false);
	}

	limpar() {
		this.npli.nativeElement.value = "";
		this.nli.nativeElement.value = "";
		this.dataInicio.nativeElement.value = "";
		this.dataFim.nativeElement.value = "";
		this.statusLI.clear();
	}

	listar() {

		if (!this.isBuscaSalva || this.isModificouPesquisa) {

			if (this.isModificouPesquisa) {
				this.parametros.page = 1;
				this.parametros.size = 10;
				this.grid.page = 1;
				this.grid.size = 10;
			}
			else {
				this.parametros.page = this.grid.page;
				this.parametros.size = this.grid.size;
			}
			this.parametros.sort = this.grid.sort.field;
			this.parametros.reverse = this.grid.sort.reverse;

			//Busca o numero PLI
			if (this.npli.nativeElement.value == "") {
				this.parametros.NumeroPli = -1;
				this.parametros.Ano = -1;
			} else {
				this.parametros.AnoPli = this.npli.nativeElement.value.split("/")[0];
				this.parametros.NumeroPli = +this.npli.nativeElement.value.split("/")[1];
            }

			//Busca o numero LI
			if (this.nli.nativeElement.value == "") {
				this.parametros.NumeroLi = -1;
			} else {
				this.parametros.NumeroLi = this.nli.nativeElement.value;
            }

            if (this.statusLI.model == "" || this.statusLI.model == "0") {
                this.parametros.Status = -1;
            } else {
                this.parametros.Status = this.statusLI.model;
            }

            if (this.dataInicio.nativeElement.value == "") {
                this.parametros.DataCadastro = null;
            } else {
                this.parametros.DataCadastro = this.dataInicio.nativeElement.value;
            }

            if (this.dataFim.nativeElement.value == "") {
                this.parametros.DataCancelamento = null;
            } else {
                this.parametros.DataCancelamento = this.dataFim.nativeElement.value;
            }
		}
		else {

			// Recuperar dados do localStorage
			if (this.parametros.page != this.grid.page)
				this.parametros.page = this.grid.page;
			else
				this.grid.page = this.parametros.page;

			if (this.grid.size != this.parametros.size) {
				this.parametros.size = this.grid.size;
			}
			else {
				this.grid.size = this.parametros.size;
			}

			if (this.grid.sort.field != this.parametros.sort)
				this.parametros.sort = this.grid.sort.field;
			else
				this.grid.sort.field = this.parametros.sort;

			if (this.grid.sort.reverse != this.parametros.reverse)
				this.parametros.reverse = this.grid.sort.reverse;
			else
				this.grid.sort.reverse = this.parametros.reverse;
		}

		this.ocultarBotaoReprocessar = false;
		this.parametros.exportarListagem = false;
		this.applicationService.get(this.servicoCancelarLiGrid, this.parametros).subscribe((result: PagedItems) => {
			this.isModificouPesquisa = false;
			this.isBuscaSalva = true;
			this.grid.lista = JSON.parse(JSON.stringify(result.items));
			this.grid.total = result.total;

			if (result.total > 0) {
				this.parametros.exportarListagem = true;
				for (var i = 0; i < result.items.length; i++) {
					this.ocultarBotaoReprocessar = true;
					this.ocultarbotaocheck = true;
				}
			} else {
				this.ocultarBotaoReprocessar = false;
				this.ocultarbotaocheck = false;
			}

			//verificar se tem algum pli com status de aguardando processamento para mostrar o botao
			for (var i = 0; i < result.items.length; i++) {
				if (result.items[i].statusPliProcessamento == 3) {
					this.ocultarBotaoReprocessar = true;
					break;
				}
			}
			this.gravarBusca();
		});
	}

	gravarBusca() {
		localStorage.removeItem(this.router.url);
		localStorage.setItem(this.router.url, JSON.stringify(this.parametros));
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

	cancelamentos() {

        if (this.grid.lista != undefined) {
            var countLiReferencia = 0;
			var selecionadoErrado = false;
			this.model.listaSelecionados = new Array<number>();

			for (var i = 0; i < this.grid.lista.length; i++) {
                if (this.grid.lista[i].checkbox) {
                    this.model.listaSelecionados.push(this.grid.lista[i].idPliMercadoria);
                    if (this.grid.lista[i].tipoLi == 2) {
                        countLiReferencia++;
                    }
                }
			}

			if (this.model.listaSelecionados == null) {
				selecionadoErrado = true;
			}
		}

		if (selecionadoErrado) {
		} else
			if (this.model.listaSelecionados.length > 0) {

                if (countLiReferencia > 0) {
                    this.modal.confirmacao("Deseja Ativar a LI Original (1ª LI Deferida - Normal) ?", "Atenção!", '')
                        .subscribe(isConfirmado => {
                            if (isConfirmado) {
                                this.model.ativarLIOriginal = isConfirmado;
                                this.applicationService.put(this.servicoCancelarLi, this.model).subscribe(result => {
                                    if (this.parametros.mensagemErro != null) {
                                        this.modal.alerta(this.parametros.mensagemErro, "Informação");
                                        this.listar();
                                    }
                                    else {
                                        this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "");
                                        this.listar();
                                    }
                                });
                            }
                            else {
                                this.model.ativarLIOriginal = isConfirmado;
                                this.applicationService.put(this.servicoCancelarLi, this.model).subscribe(result => {
                                    if (this.parametros.mensagemErro != null) {

                                        this.modal.alerta(this.parametros.mensagemErro, "Informação");
                                        this.listar();
                                    }
                                    else {
                                        this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "");
                                        this.listar();
                                    }
                                });
                            }
                        });
                }
                else
                {
                    this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
                        .subscribe(isConfirmado => {
                            if (isConfirmado) {

                                this.applicationService.put(this.servicoCancelarLi, this.model).subscribe(result => {

                                    if (this.parametros.mensagemErro != null) {

                                        this.modal.alerta(this.parametros.mensagemErro, "Informação");
                                        this.listar();
                                    }
                                    else {
                                        this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "");
                                        this.listar();
                                    }

                                });
                            }
                        });
                }

			} else {
				this.modal.alerta("Nenhuma LI selecionada para ser cancelada", 'Informação');
			}

	}

}
