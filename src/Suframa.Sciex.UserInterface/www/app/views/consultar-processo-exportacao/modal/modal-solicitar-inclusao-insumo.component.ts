import { Component, ViewChild, OnInit } from '@angular/core';
import { ValidationService } from '../../../shared/services/validation.service';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { Router } from '@angular/router';
import { AuthGuard } from '../../../shared/guards/auth-guard.service';
import { ToastrService } from 'toastr-ng2/toastr';
import { AuthenticationService } from '../../../shared/services/authentication.service';
import { PagedItems } from '../../../view-model/PagedItems';
import { AssertNotNull, ThrowStmt } from '@angular/compiler';
import { ConsultarFormularioQuadrosInsumosComponent } from '../formulario/formulario-quadros-insumos.component';


@Component({
	selector: 'app-modal-inclusao-insumo',
	templateUrl: './modal-solicitar-inclusao-insumo.component.html',
})

export class ModalSolicitarInclusaoInsumoComponent implements OnInit {

	formPai = this;
	model: any;
	servico = 'SolicitarInclusaoInsumo';
	parametros: any = {};
	grid: any = { sort: {} };
	idProduto : number = 0;
	idProcesso : number = 0;
	infoModal : any = {};
	salvouItem : boolean;

	@ViewChild('appModalSolicitarInclusaoInsumo') appModalSolicitarInclusaoInsumo;
	@ViewChild('appModalSolicitarInclusaoInsumoBackground') appModalSolicitarInclusaoInsumoBackground;

	constructor(
		private ConsultarFormularioQuadrosInsumosComponent : ConsultarFormularioQuadrosInsumosComponent,
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

	ngOnInit() {
	}

	public abrir(idProduto, codigoProduto, objetoProcesso) {	
		this.salvouItem = false;
		this.parametros.idProduto = idProduto;
		this.parametros.codigoProduto = codigoProduto;
		this.infoModal = objetoProcesso;
		this.appModalSolicitarInclusaoInsumo.nativeElement.style.display = 'block';
		this.appModalSolicitarInclusaoInsumoBackground.nativeElement.style.display = 'block';
		this.listar();
	}

	public fechar() {
		this.salvouItem ? 
			this.ConsultarFormularioQuadrosInsumosComponent.verificarRota() : '';
	
		this.grid = { sort: {} };
		this.parametros = {};
		this.appModalSolicitarInclusaoInsumo.nativeElement.style.display = 'none';
		this.appModalSolicitarInclusaoInsumoBackground.nativeElement.style.display = 'none';
		this.salvouItem = false;
	}

	confirmarCancelamento(): any
	{
		this.modal.confirmacao("Os dados serão descartados. Deseja continuar?", '', '')
				.subscribe(isConfirmado => {
					if (isConfirmado){
						this.fechar()
					}
				});
	}

	onChangeSort($event) {
		this.grid.sort = $event;
	}

	onChangeSize($event) {
		this.grid.size = $event;
	}

	onChangePage($event) {
		this.grid.page = $event;
		this.listar();
	}

	listar() {		
		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field;
		this.parametros.reverse = this.grid.sort.reverse;
		this.applicationService.get(this.servico, this.parametros).subscribe((result: PagedItems) => {
			if(result.total > 0){
				this.grid.lista = result.items;
				this.grid.total = result.total;
				this.prencheParametrosDeExportacao();				
			} else {
				this.parametros.exportarListagem = false;
				this.grid = { sort: {} };
			}		
		});
	}

	prencheParametrosDeExportacao(){
		
		this.parametros.exportarListagem = true;
		this.parametros.servico = this.servico;
		this.parametros.titulo = "Lista Insumos da LE"

		this.parametros.columns = [ 
									"Cód. Insumo", "Tipo","NCM", 
									"Descrição do Insumo", "Coef. Técnico", 
									"Unidade", "Especificação Técnica" 
								  ];

		this.parametros.fields = [ 
									"codigoInsumo", "tipoInsumo", "codigoNCM", 
									"undMed", "descricaoInsumo", "valorCoeficienteTecnico", 
									"descricaoUnidadeMedida", "descricaoEspecTecnica"
								 ];

		this.parametros.width = { 
									0: { columnWidth: 80 }, 1: { columnWidth: 80 }, 
									2: { columnWidth: 100 }, 3: { columnWidth: 100 }, 
									4: { columnWidth: 100 }, 5: { columnWidth: 100 } 
								};
	}

	Salvar() {

		let qtdItensSelecionados = 0;
		
		this.grid.lista.forEach(element => {
			element.checkbox ? qtdItensSelecionados++ : '';			
			element.idProduto = this.parametros.idProduto;
			element.idProcesso = this.infoModal.idProcesso;
			}
		);

		if(qtdItensSelecionados == 0){
			this.modal.alerta("Nenhum insumo selecionado");
			return false;
		}
			
		this.applicationService.post(this.servico, this.grid.lista).subscribe((result: string) => {
			if(result == "OK"){
				this.salvouItem = true;
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Informação", "");
				this.fechar();
			} 		
		});
	}

	confirmarSalvar() {
		
		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, '', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.Salvar();
				}
			});

	}

}
