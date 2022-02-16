import { Component, OnInit, Injectable } from '@angular/core';
import { ApplicationService } from '../../../shared/services/application.service';
import { MessagesService } from '../../../shared/services/messages.service';

@Component({
	selector: 'app-sincronizar-paridade-cambial',
    templateUrl: './sincronizar-paridade-cambial.component.html'
})

@Injectable()
export class SincronizarParidadeCambialComponent implements OnInit {	
	parametros: any = {};	
	servicoParidadeCambial = 'ParidadeCambial';    
	constructor(
		private applicationService: ApplicationService,		
		private msg: MessagesService,
	) { }

    ngOnInit(): void {
        this.parametros.tipoGeracao = 3;
		this.sincronizarParidade();
	}

    sincronizarParidade()
    {
        this.applicationService.put(this.servicoParidadeCambial, this.parametros).subscribe(result => {

        });       
    }	
}
