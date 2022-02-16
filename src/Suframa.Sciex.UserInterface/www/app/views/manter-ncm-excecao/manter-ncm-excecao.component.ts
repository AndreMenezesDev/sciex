import { Component, OnInit, Injectable, ViewChild } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router } from '@angular/router';
import { ValidationService } from "../../shared/services/validation.service";

@Component({
	selector: 'app-manter-ncm-excecao',
	templateUrl: './manter-ncm-excecao.component.html'
})

@Injectable()
export class ManterNCMExcecaoComponent implements OnInit {

	grid: any = { sort: {} };
	parametros: any = {};
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	ordenarGrid: boolean = true;
	servicoNcmExcecaoGrid = 'NcmExcecaoGrid';
	@ViewChild('codigo') codigo;
	@ViewChild('descricao') descricao;
	@ViewChild('municipio') municipio;
	@ViewChild('setor') setor;
	@ViewChild('todos') todos;
	@ViewChild('ativo') ativo;
	@ViewChild('inativo') inativo;
	@ViewChild('dataInicio') dataInicio;
	@ViewChild('dataFim') dataFim;
	isModificouPesquisa: boolean = false;
	isBuscaSalva: boolean = false;
	carregarGrid: boolean = false;
	opcaoStatus: number;

	constructor(
		private applicationService: ApplicationService,
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router
	) {
		if (localStorage.getItem(this.router.url) == null && localStorage.length > 0) {
			localStorage.clear();
		}
	}

	ngOnInit(): void {

		this.retornaValorSessao();

		if (this.parametros != undefined || this.parametros != null) {

			this.grid.page = this.parametros.page;
			this.grid.size = this.parametros.size;
			this.grid.sort.field = this.parametros.sort;
			this.grid.sort.reverse = this.parametros.reverse;

			if (this.parametros.codigo != "-1")
				localStorage.removeItem(this.router.url);

			this.listar();
		}
		else {
			this.parametros = {};
			this.parametros.servico = this.servicoNcmExcecaoGrid;
			this.parametros.titulo = "MANTER NCM EXCEÇÃO"
			this.parametros.width = { 0: { columnWidth: 80 }, 1: { columnWidth: 300 }, 2: { columnWidth: 140 }, 3: { columnWidth: 80 }, 4: { columnWidth: 80 }, 5: { columnWidth: 80 }};
			this.parametros.columns = ["Código", "Descrição", "Município", "Setor", "Vigência", "Status"];
			this.parametros.fields = ["codigoNCM", "descricaoNcm", "descricaoMunicipioCodigo", "descricaoSetor", "dataInicioVigenciaFormatado", "status"];

		}

	}

	retornaValorSessao() {

		if (localStorage.getItem(this.router.url) != null || localStorage.getItem(this.router.url) != "") {
			this.parametros = JSON.parse(localStorage.getItem(this.router.url));
			this.isBuscaSalva = true;
		}
	}

	validarData() {

		try {
			this.dataInicio.nativeElement.setCustomValidity('');
			this.dataFim.nativeElement.setCustomValidity('');

			var dataFim = new Date(this.parametros.dataFim);
			var dataInicio = new Date(this.parametros.dataInicio);


			if (this.dataInicio.nativeElement.value.length > 0 || this.dataFim.nativeElement.value.length > 0) {
				if (this.dataInicio.nativeElement.value.length >= 0 && this.dataInicio.nativeElement.value.length < 10) {
					this.dataInicio.nativeElement.setCustomValidity('Campo inválido');
				} else if (this.dataFim.nativeElement.value.length >= 0 && this.dataFim.nativeElement.value.length < 10) {
					this.dataFim.nativeElement.setCustomValidity('Campo inválido');

				} else if (new Date(this.parametros.dataFim) < new Date(this.parametros.dataInicio)) {
					this.dataFim.nativeElement.setCustomValidity('Campo inválido. Data inicio maior que data final');
				}
			}
		} catch (e) {

			this.dataInicio.nativeElement.setCustomValidity('Informe uma data válida.');
		}

	}

	buscar(exibirMensagem, grid) {

		if (grid)
			return;

		this.validarData();
		if (!this.validationService.form('formBusca')) { return; }

		if (this.todos.nativeElement.value == false) {
			if (exibirMensagem) {
				this.modal.alerta(this.msg.INFORME_OS_FILTROS_DE_PESQUISA, 'Informação');
			} else

				this.validarData();

			if (!this.validationService.form('formBusca')) { return; }

			if (this.isBuscaSalva) {
				this.listar();
			}
		}
		else {

			if (exibirMensagem) {
				this.isModificouPesquisa = true;
			}
			else {
				this.isBuscaSalva = true;
			}
			this.listar();
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
		this.buscar(false, (this.grid.lista == undefined || this.grid.lista.lenght == 0));
	}

	limpar() {

		this.codigo.nativeElement.value = "";
		this.descricao.nativeElement.value = "";
		this.dataInicio.nativeElement.value = "";
		this.dataFim.nativeElement.value = "";
		this.municipio.clear();
		this.parametros = {};
		this.todos.nativeElement.checked = true;
		this.ativo.nativeElement.checked = false;
		this.inativo.nativeElement.checked = false;

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

			if (this.codigo.nativeElement.value == "")
				this.parametros.codigo = null;
			else
				this.parametros.codigo = this.removerCaractere(this.codigo.nativeElement.value);

			if (this.descricao.nativeElement.value == "")
				this.parametros.descricaoNcm = "";
			else
				this.parametros.descricaoNcm = this.descricao.nativeElement.value;

			if (this.dataInicio.nativeElement.value == "") {
				this.parametros.dataVigenciaInicio = "";
			} else {
				this.parametros.dataVigenciaInicio = this.dataInicio.nativeElement.value;
			}

			if (this.dataFim.nativeElement.value == "") {
				this.parametros.dataVigenciaFim = "";
			} else {
				this.parametros.dataVigenciaFim = this.dataFim.nativeElement.value;
			}

			// if (this.municipio.valorInput.nativeElement.value.split(" | ")[1] != undefined) {
			if (this.municipio.valorInput.nativeElement.value.split(" | ")[0] != undefined) {
				// this.parametros.descricaoMunicipio = this.municipio.valorInput.nativeElement.value.split(" | ")[1];
				this.parametros.codigoMunicipio = this.municipio.valorInput.nativeElement.value.split(" | ")[0];
			} else {
				this.parametros.codigoMunicipio = "";
			}

			// Verificação da opção status
			if (this.todos.nativeElement.checked == true) {
				this.parametros.status = 2;
			}
			if (this.ativo.nativeElement.checked == true) {
				this.parametros.status = 1;
			}
			if (this.inativo.nativeElement.checked == true) {
				this.parametros.status = 0;
			}

			//codigoMunicipio

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


		this.parametros.exportarListagem = false;
		this.applicationService.get(this.servicoNcmExcecaoGrid, this.parametros).subscribe((result: PagedItems) => {
			this.isModificouPesquisa = false;
			this.isBuscaSalva = true;
			this.grid.lista = result.items;
			this.grid.total = result.total;

			if (result.total > 0) {
				this.parametros.exportarListagem = true;
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


}

