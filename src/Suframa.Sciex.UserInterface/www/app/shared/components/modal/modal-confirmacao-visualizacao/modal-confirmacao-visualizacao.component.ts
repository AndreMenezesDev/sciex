import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { DialogComponent, DialogService } from 'angularx-bootstrap-modal';

export interface ConfirmacaoVisualizacaoModel {
	title: string;
	message: string;
}

export enum ConfirmacaoVisualizacaoResultado {
	Nao = 1,
	Visualizar = 2,
	Sim = 3
}

@Component({
	selector: 'app-modal-confirmacao-visualizacao',
	templateUrl: './modal-confirmacao-visualizacao.component.html',
})

export class ModalConfirmacaoVisualizacaoComponent extends DialogComponent<ConfirmacaoVisualizacaoModel, ConfirmacaoVisualizacaoResultado> implements ConfirmacaoVisualizacaoModel, OnInit {
	title: string;
	message: string;

	@ViewChild('cancelar') button: ElementRef;

	constructor(dialogService: DialogService) {
		super(dialogService);
	}

	ngOnInit() {
		this.button.nativeElement.focus();
	}

	action(id: number) {
		this.result = id;
		this.close();
	}

	
}
