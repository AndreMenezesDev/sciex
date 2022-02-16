import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { DialogComponent, DialogService } from 'angularx-bootstrap-modal';

export interface PromptVisualizacaoModel {
	title: string;
	message: string;
	titlePrompt: string;
}

export enum PromptVisualizacaoResultado {
	Cancelar = 1,
	Visualizar = 2,
	Salvar = 3
}

@Component({
	selector: 'app-modal-prompt-visualizacao',
	templateUrl: './modal-prompt-visualizacao.component.html'
})

export class ModalPromptVisualizacaoComponent extends DialogComponent<PromptVisualizacaoModel, PromptVisualizacaoResultado> implements PromptVisualizacaoModel, OnInit {
	message: string;
	result: any = {};
	title: string;
	titlePrompt: string;

	@ViewChild('cancelar') button: ElementRef;

	constructor(dialogService: DialogService) {
		super(dialogService);
	}

	ngOnInit() {
		this.button.nativeElement.focus();
	}

	validForm(id) {
		if (!id || !document.forms || !document.forms[id] || !document.forms[id].reportValidity) { return true; }
		const form = document.forms[id];
		return form.reportValidity();
	}

	action(id: number) {
		if (id == PromptVisualizacaoResultado.Salvar && !this.validForm('formularioPrompt')) { return false; }
		this.result.action = id;
		this.close();
	}
}
