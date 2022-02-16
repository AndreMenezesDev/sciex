import { Component, ViewChild, Output, EventEmitter } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { Router } from '@angular/router';
import { manterLiVM } from '../../../view-model/ManterLiVM';
import { PagedItems } from '../../../view-model/PagedItems';


@Component({
	selector: 'app-modal-historico-li',
	templateUrl: './modal-historico-li.component.html',
})

export class ModalHistoricoLIComponent {
	isDisplay: boolean = false;
	servicoCancelarLi = 'CancelarLi';
	servicoAliHistorico = 'AliHistorico';
	parametros: any = {};

	grid: any = { sort: {} };
	ocultarFiltro: boolean = false;
	ocultarGrid: boolean = false;
	ordenarGrid: boolean = true;
	isBuscaSalva: boolean = false;

	modelLI: manterLiVM = new manterLiVM();

	@ViewChild('appModalHistoricoLIBackground') appModalHistoricoLIBackground;
	@ViewChild('appModalHistoricoLI') appModalHistoricoLI;

	formPai: any;


	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private applicationService: ApplicationService,
		private msg: MessagesService,
		private router: Router,
	) { }


	public abrir(numeroLI) {

		if (!numeroLI) { return; }

		this.parametros.servico = this.servicoAliHistorico;
		this.parametros.titulo = "Histórico de Cancelamento de LI";
		this.parametros.width = { 0: { columnWidth: 170 }, 1: { columnWidth: 80 }, 2: { columnWidth: 150 }, 3: { columnWidth: 200 }, 4: { columnWidth: 180 } };
		this.parametros.columns = ["Ação", "Data", "Login Responsável", "Responsável", "Resposta"];
		this.parametros.fields = ["descricaoStatus", "dataFormadata", "loginResponsavel", "nomeResponsavel", "observacao"];
		this.parametros.numeroLI = numeroLI;

		this.applicationService.get<manterLiVM>(this.servicoCancelarLi, this.parametros).subscribe(result => {
			this.modelLI = result;

			this.listarHistoricoLI(this.modelLI.idPliMercadoria);
		});

		this.appModalHistoricoLIBackground.nativeElement.style.display = 'block';
		this.appModalHistoricoLI.nativeElement.style.display = 'block';
	}

	public fechar() {
		this.appModalHistoricoLIBackground.nativeElement.style.display = 'none';
		this.appModalHistoricoLI.nativeElement.style.display = 'none';
	}

	public listarHistoricoLI(idMercadoria) {

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


		this.parametros.idPliMercadoria = idMercadoria;
		this.parametros.exportarListagem = false;
		this.applicationService.get(this.servicoAliHistorico, this.parametros).subscribe((result: PagedItems) => {

			this.grid.lista = result.items;
			this.grid.total = result.total;

			if (result.total > 0) {
				this.parametros.exportarListagem = true;
			}
		});
	}

}

