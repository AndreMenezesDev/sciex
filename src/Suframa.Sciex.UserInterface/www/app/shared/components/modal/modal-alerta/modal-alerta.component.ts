import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { DialogComponent, DialogService } from 'angularx-bootstrap-modal';
import { Router } from '@angular/router';

export interface AlertaModel {
	title: string;
	message: string;
	caminhoURL?: string;
}

@Component({
	selector: 'app-modal',
	templateUrl: './modal-alerta.component.html',
})

export class ModalAlertaComponent extends DialogComponent<AlertaModel, boolean> implements AlertaModel, OnInit {
	title: string;
	message: string;
	caminhoURL?: string;

    @ViewChild('fechar') button: ElementRef;

	constructor(dialogService: DialogService,
		private router: Router) {
		super(dialogService);
	}

	ngOnInit() {
        this.button.nativeElement.focus();
	}

	confirm() {
		this.result = true;
		this.close();
	}
}
