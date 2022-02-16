import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { DialogComponent, DialogService } from 'angularx-bootstrap-modal';
import {Router} from '@angular/router';

export interface ConfirmacaoModel {
	title: string;
	message: string;
	caminho: string;
}

@Component({
	selector: 'app-modal-confirmacao',
	templateUrl: './modal-confirmacao.component.html',
})

export class ModalConfirmacaoComponent extends DialogComponent<ConfirmacaoModel, boolean> implements ConfirmacaoModel, OnInit {
	title: string;
	message: string;
	caminho: string;

	@ViewChild('cancelar') button: ElementRef;

	constructor(dialogService: DialogService, private router: Router) {
        super(dialogService);      	
	}

	ngOnInit() {
		this.button.nativeElement.focus();
	}

    confirm() {
        this.result = true;        
        this.close();		        
	}

	noCofirm() {
		if (this.caminho != '')
			this.router.navigate([this.caminho]);
		this.close();
	}

}

