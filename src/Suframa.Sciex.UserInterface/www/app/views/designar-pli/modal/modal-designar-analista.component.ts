import { Component, ViewChild, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../shared/guards/auth-guard.service';
import { ToastrService } from 'toastr-ng2/toastr';
import { AuthenticationService } from '../../../shared/services/authentication.service';
import { DesignarPliComponent } from '../designar-pli.component';
import { DesignarPliGridComponent } from '../grid/grid.component';

@Component({
	selector: 'app-modal-designar-analista',
	templateUrl: './modal-designar-analista.component.html',
})

export class ModalDesignarAnalistaComponent implements OnInit {
	parametros: any = {};
	item: any = {};
	isPli: boolean;
	isLe: boolean;
	isPlano: boolean;
	isSolic: boolean;

	constructor(
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private applicationService: ApplicationService,
		private toastrService: ToastrService,
		private router: Router,
		private authguard: AuthGuard,
		private DesignarPliComponent : DesignarPliComponent,
		private DesignarPliGridComponent : DesignarPliGridComponent,
		private authenticationService: AuthenticationService,
	) {

	}


	servicoPli = 'DesignarAnalista';
	servicoLe = 'DesignarAnalistaLe';
	servicoPlano = 'DesignarAnalistaPlano';
	servicoSolic = 'DesignarAnalistaSolicitacao';

	@ViewChild('appModalDesignarAnalista') appModalDesignarAnalista;
	@ViewChild('appModalDesignarAnalistaBackground') appModalDesignarAnalistaBackground;

	ngOnInit() {
		setTimeout(x => { $(".modal-body").scrollTop(0); }, 500);
	}

	public abrir(item, isPli, isLe, isPlano, isSolic) {
		this.item = item;
		this.isPli = isPli;
		this.isLe = isLe;
		this.isPlano = isPlano;
		this.isSolic = isSolic;
		this.appModalDesignarAnalista.nativeElement.style.display = 'block';
		this.appModalDesignarAnalistaBackground.nativeElement.style.display = 'block';
	}

	public fechar() {
		this.appModalDesignarAnalista.nativeElement.style.display = 'none';
		this.appModalDesignarAnalistaBackground.nativeElement.style.display = 'none';
	}

	inicializarItensTela() {

	}

	public salvar() {
		if(this.parametros.idAnalistaDesignado == null || this.parametros.idAnalistaDesignado == 0){
			this.modal.alerta('Selecione o analista!', 'Informação');
			return;
		}

		for (let item of this.item){
			item.idAnalistaDesignado = this.parametros.idAnalistaDesignado;
		}

		this.parametros.lista = this.item;

		if (this.isPli){
			this.applicationService.put(this.servicoPli, this.parametros).subscribe(result => {
				if (result != null) {
					this.modal.resposta("Salvo com Sucesso!", "Informação", "").subscribe( result => {
						this.DesignarPliComponent.listar();
						this.parametros.idAnalistaDesignado = null;
						this.DesignarPliGridComponent.masterSelected = false;
						this.fechar();
					});
				}
			});
		}
		else if(this.isLe){
			this.applicationService.put(this.servicoLe, this.parametros).subscribe(result => {
				if (result != null) {
					this.modal.resposta("Salvo com Sucesso!", "Informação", "").subscribe( result => {
						this.DesignarPliComponent.listarLes();
						this.parametros.idAnalistaDesignado = null;
						this.DesignarPliGridComponent.masterSelected = false;
						this.fechar();
					});
				}
			});
		}
		else if(this.isPlano){
			this.applicationService.put(this.servicoPlano, this.parametros).subscribe(result => {
				if (result != null) {
					this.modal.resposta("Salvo com Sucesso!", "Informação", "").subscribe( result => {
						this.DesignarPliComponent.listarPlanos();
						this.parametros.idAnalistaDesignado = null;
						this.DesignarPliGridComponent.masterSelected = false;
						this.fechar();
					});
				}
			});
		}
		else if(this.isSolic){
			this.applicationService.put(this.servicoSolic, this.parametros).subscribe(result => {
				if (result != null) {
					this.modal.resposta("Salvo com Sucesso!", "Informação", "").subscribe( result => {
						this.DesignarPliComponent.listarSolicitacoes();
						this.parametros.idAnalistaDesignado = null;
						this.DesignarPliGridComponent.masterSelected = false;
						this.fechar();
					});
				}
			});
		}
		
	}

}
