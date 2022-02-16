import { Component, ViewChild, Output, EventEmitter } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { Router } from '@angular/router';
import { manterLiVM } from '../../../view-model/ManterLiVM';
import { PagedItems } from '../../../view-model/PagedItems';
import { manterNcmVM } from '../../../view-model/ManterNcmVM';
import { auditoriaVM } from '../../../view-model/AuditoriaVM';
import { TaxaGrupoBeneficioVM } from '../../../view-model/TaxaGrupoBeneficioVM';


@Component({
	selector: 'app-modal-historico-beneficio',
	templateUrl: './modal-historico-beneficio.component.html',
})

export class ModalHistoricoBeneficioComponent {
	isDisplay: boolean = false;
	servicoHistoricoBeneficio = 'BeneficioHistoricoGrid';
	parametros: any = {};

	grid: any = { sort: {} };
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	ordenarGrid: boolean = true;
	isBuscaSalva: boolean = false;

	model: TaxaGrupoBeneficioVM = new TaxaGrupoBeneficioVM();

	@ViewChild('appModalHistoricoBeneficioBackground') appModalHistoricoBeneficioBackground;
	@ViewChild('appModalHistoricoBeneficio') appModalHistoricoBeneficio;

	formPai: any;


	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private router: Router,
	) { }


	public abrir(model: TaxaGrupoBeneficioVM) {

		 if (!model) { return; }

		// this.parametros.servico = this.servicoHistoricoNcm;
		// this.parametros.titulo = "Histórico de Cancelamento de LI";
		// this.parametros.width = { 0: { columnWidth: 170 }, 1: { columnWidth: 80 }, 2: { columnWidth: 150 }, 3: { columnWidth: 200 }, 4: { columnWidth: 180 } };
		// this.parametros.columns = ["Ação", "Data", "Login Responsável", "Responsável", "Resposta"];
		// this.parametros.fields = ["descricaoStatus", "dataFormadata", "loginResponsavel", "nomeResponsavel", "observacao"];
		this.model = model;
		this.parametros.descricao = model.descricao;

		this.listarHistoricoBeneficio(model.idTaxaGrupoBeneficio);

		this.appModalHistoricoBeneficioBackground.nativeElement.style.display = 'block';
		this.appModalHistoricoBeneficio.nativeElement.style.display = 'block';
	}

	public fechar() {
		this.appModalHistoricoBeneficioBackground.nativeElement.style.display = 'none';
		this.appModalHistoricoBeneficio.nativeElement.style.display = 'none';
	}

	public listarHistoricoBeneficio(idBeneficio) {

		console.log('listar historico');
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

		this.parametros.idTaxaGrupoBeneficio = idBeneficio;
		this.parametros.exportarListagem = false;
		this.applicationService.get(this.servicoHistoricoBeneficio, this.parametros).subscribe((result: PagedItems) => {

			this.grid.lista = result.items;
			this.grid.total = result.total;

			if (result.total > 0) {
				this.parametros.exportarListagem = true;
			}
		});
	}

}

