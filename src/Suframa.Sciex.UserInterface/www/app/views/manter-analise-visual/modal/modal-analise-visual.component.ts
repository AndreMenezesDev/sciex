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
import { AuthenticationService } from '../../../shared/services/authentication.service';
import { ValoresVM } from '../../../view-model/ValoresVM';


declare var $: any;

@Component({
	selector: 'app-modal-analise-visual',
	templateUrl: './modal-analise-visual.component.html',
})

export class ModalAnaliseVisualComponent implements OnInit, OnDestroy {
	parametros: any = {};
	item: any = {};

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

	@ViewChild('appModalAnaliseVisual') appModalAnaliseVisual;
	@ViewChild('appModalAnaliseVisualBackground') appModalAnaliseVisualBackground;

	ngOnInit() {
		setTimeout(x => { $(".modal-body").scrollTop(0); }, 500);
	}

	ngOnDestroy() {
		// ...
	}

	public abrir(item) {
		this.item = item;
		this.appModalAnaliseVisual.nativeElement.style.display = 'block';
		this.appModalAnaliseVisualBackground.nativeElement.style.display = 'block';
	}

	public fechar() {
		this.appModalAnaliseVisual.nativeElement.style.display = 'none';
		this.appModalAnaliseVisualBackground.nativeElement.style.display = 'none';
	}

	inicializarItensTela() {

	}

	public salvar(confirmarMensagemModal: boolean) {

	}

	
	downloadFile(){
		if (this.item.anexo) {
			const hashPDF = this.item.anexo;
			const linkSource = 'data:' + 'application/zip' + ';base64,' + hashPDF;
			const downloadLink = document.createElement('a');
			const fileName = this.item.nomeAnexo;

			document.body.appendChild(downloadLink);

			downloadLink.href = linkSource;
			downloadLink.download = fileName;

			downloadLink.target = '_self';

			downloadLink.click();
		} else {
			this.modal.alerta('Erro ao baixar arquivo.', 'Informação');
		}
	}

}
