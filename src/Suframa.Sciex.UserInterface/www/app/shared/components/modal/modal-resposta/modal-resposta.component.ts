import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { DialogComponent, DialogService } from 'angularx-bootstrap-modal';

import { Location } from '@angular/common';
import {Router} from '@angular/router';

export interface RespostaModel {
	title: string;
    message: string;
    caminho: string;

}

@Component({
	selector: 'app-modal-resposta',
    templateUrl: './modal-resposta.component.html',
})

export class ModalRespostaComponent extends DialogComponent<RespostaModel, boolean> implements RespostaModel, OnInit {
	title: string;
    message: string;
    caminho: string;


	@ViewChild('fechar') button: ElementRef;

    constructor(dialogService: DialogService, private router: Router) {
        super(dialogService);        
	}
	
    ngOnInit() {      
        this.button.nativeElement.focus();
	}

    confirm() {
		this.result = true;
        this.close();
        if (this.caminho != '')
			this.router.navigate([this.caminho]); 		
    }


}
