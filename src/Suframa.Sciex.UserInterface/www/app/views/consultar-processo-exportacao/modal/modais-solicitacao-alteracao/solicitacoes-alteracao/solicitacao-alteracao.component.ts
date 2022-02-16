import { Component, ViewChild, OnInit } from '@angular/core';
import { ValidationService } from '../../../../../shared/services/validation.service';
import { ModalService } from '../../../../../shared/services/modal.service';
import { MessagesService } from '../../../../../shared/services/messages.service';
import { ApplicationService } from '../../../../../shared/services/application.service';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../../../shared/guards/auth-guard.service';
import { ToastrService } from 'toastr-ng2/toastr';
import { AuthenticationService } from '../../../../../shared/services/authentication.service';
import { PagedItems } from '../../../../../view-model/PagedItems';
import { AssertNotNull, ThrowStmt } from '@angular/compiler';


@Component({
	selector: 'app-modal-solicitacao',
	templateUrl: './solicitacao-alteracao.component.html',
})

export class ModalSolicitacaoComponent implements OnInit {

	formPai = this;
	model: any;
	servico = 'SolictitacoesAlteracaoDetalheInsumo';
	parametros: any = {};
	grid: any = { sort: {} };
	idProduto : number = 0;
	idProcesso : number = 0;
	listaDetalhesSolicAlteracao : any = {} = null;
	descInsumo : string = "";
	codigoInsumo : string = "";
	numeroSequencial : string = "";

	@ViewChild('appModalSolicitacao') appModalSolicitacao;
	@ViewChild('appModalSolicitacaoBackground') appModalSolicitacaoBackground;

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

	ngOnInit() { }

	public abrir(objeto) {	

		this.parametros.idProcesso = objeto.prcInsumo.produto.idProcesso;
		this.descInsumo = objeto.prcInsumo.codigoInsumo + " | " + objeto.prcInsumo.descricaoInsumo;
		this.numeroSequencial = objeto.numeroSequencial;

		this.parametros.PRCInsumoDE = {};
		this.parametros.PRCInsumoDE.IdInsumo = objeto.prcInsumo.idInsumo;		

		this.parametros.PRCInsumoDE.codigoInsumo = objeto.prcInsumo.codigoInsumo;
		this.parametros.PRCInsumoDE.idPrcProduto = objeto.prcInsumo.idPrcProduto;

		this.appModalSolicitacao.nativeElement.style.display = 'block';
		this.appModalSolicitacaoBackground.nativeElement.style.display = 'block';
		this.buscar();
	}

	public fechar() {

		this.descInsumo = "";
		this.numeroSequencial = "";

		this.grid = { sort: {} };
		this.parametros = {};

		this.appModalSolicitacao.nativeElement.style.display = 'none';
		this.appModalSolicitacaoBackground.nativeElement.style.display = 'none';
	}

	buscar(){
		this.applicationService.get(this.servico, this.parametros).subscribe((result: any) => {
			this.listaDetalhesSolicAlteracao = {};
			this.listaDetalhesSolicAlteracao = (result) ? result : null;
		});
	}


}
