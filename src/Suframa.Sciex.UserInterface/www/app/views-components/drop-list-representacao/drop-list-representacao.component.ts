import { Component, OnInit, ViewChild, ElementRef, Input, Pipe,PipeTransform } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { DatePipe } from '@angular/common';
import { RepresentacaoVM } from "../../view-model/RepresentacaoVM";
import { LayoutComponent } from "../../layout/layout.component";

@Component({
  selector: 'app-droplist-representacao',
  templateUrl: './drop-list-representacao.component.html'
})

export class DropListRepresentacaoComponent {
	Representacoes = [];
	resultado = [];

	constructor(
		private applicationService: ApplicationService,
		private layout: LayoutComponent

	) { }

	ngOnInit() {
		this.applicationService.get('RepresentanteLegal').subscribe((result: any) => {
			if (result && result.length > 0) {
				this.resultado = result;
				for (var i = 0; i < this.resultado.length; i++) {
					this.Representacoes.push({nome:this.resultado[i].nome,cnpj:this.resultado[i].cnpj,isUsuarioLogado:false}); 
				}

				this.Representacoes.push({cnpj:'',nome:'Encerrar Representação',isUsuarioLogado:true}); 
			}
		});
	}


	representar(representacao){
		this.applicationService.post('RepresentanteLegal',representacao).subscribe((result: any) => {
			//window.location.replace(result);
			var token = result;//.substr(7);
            this.applicationService.setToken(token);
			//this.menu.recuperarMenu();
			this.layout.recarregarLayout(representacao);
			
		});
	}
}
