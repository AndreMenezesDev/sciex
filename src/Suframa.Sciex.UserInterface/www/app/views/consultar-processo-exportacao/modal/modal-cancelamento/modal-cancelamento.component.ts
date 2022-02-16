import { Component, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'toastr-ng2/toastr';
import { ModalService } from '../../../../shared/services/modal.service';
import { ApplicationService } from '../../../../shared/services/application.service';
import { ThrowStmt } from '@angular/compiler';
import { ConsultarProcessoExportacaoComponent } from '../../consultar-processo-exportacao.component';

@Component({
	selector: 'app-modal-cancelar',
	templateUrl: './modal-cancelamento.component.html',
})

export class ModalCancelarComponent implements OnInit {

	model: any;
	servico='CancelarProcesso'
	parametros: any = {};


	@ViewChild('appModalCancelar') appModalCancelar;
	@ViewChild('appModalCancelarBackground') appModalCancelarBackground;

	@ViewChild('btnlimpar') btnlimpar;



	constructor(
		private toastrService: ToastrService,
		private router: Router,
		private applicationService: ApplicationService,
		private modal: ModalService,
		private ConsultarProcessoExportacaoComponent: ConsultarProcessoExportacaoComponent,

	) {

	}

	ngOnInit() {
		this.model = {};
		this.parametros = {};

	}

 public abrir(item){

		this.model = item;
		this.parametros = {};

	     this.appModalCancelar.nativeElement.style.display = 'block';
  		 this.appModalCancelarBackground.nativeElement.style.display = 'block';
         console.log(item)
	}

	limpar() {

	}


	public fechar() {
		this.appModalCancelar.nativeElement.style.display = 'none';
		this.appModalCancelarBackground.nativeElement.style.display = 'none';

	}

	public cancelar(){
		this.parametros.idProcesso= this.model.idProcesso;
		this.model.descricaoObservacao = this.parametros.descricaoObservacao;

		if(this.parametros.descricaoObservacao==null || this.parametros.descricaoObservacao == undefined){
			this.modal.alerta("Preencha o campo Justificativa ", "Atenção", "");

			return;
		}
		this.applicationService.get(this.servico, this.parametros).subscribe((result: any) => {
		 	console.log(result.statusProcessamento );

			 if(result.statusProcessamento ==1){
				this.modal.alerta("Não é possível cancelar o processo, pois existem insumos que já foram importados", "Atenção", "");
			}
			else if(result.statusProcessamento ==2)
			{
				this.modal.alerta("Houve um erro inesperado","Atenção ",'')
			}
			else if(result.statusProcessamento == 3)
			{
				this.modal.alerta("Não existe valor de paridade cambial para a data de hoje.","Atenção ",'')
			}
			else if(result.statusProcessamento ==0)
			{
				this.modal.resposta("Processo cancelado com sucesso","Atenção ",'')
				this.fechar();
				this.ConsultarProcessoExportacaoComponent.buscar(true);
			}


		});


	}






}
