import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { DialogComponent, DialogService } from 'angularx-bootstrap-modal';

export interface PromptModel {
	title: string;
	message: string;
	titlePrompt: string;
}

@Component({
	selector: 'app-modal-prompt',
	templateUrl: './modal-prompt.component.html'
})

export class ModalPromptComponent extends DialogComponent<PromptModel, boolean> implements PromptModel, OnInit {
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

	confirm() {
		if (!this.validForm('formularioPrompt')) { return false; }

		this.result.isConfirmar = true;
		this.close();
	}
}
