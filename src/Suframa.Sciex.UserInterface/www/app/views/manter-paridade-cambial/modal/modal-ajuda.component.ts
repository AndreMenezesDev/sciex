import { Component, ViewChild } from '@angular/core';


@Component({
	selector: 'app-modal-ajuda',
	templateUrl: './modal-ajuda.component.html',
})

export class ModalAjudaComponent {

	@ViewChild('appModalAjuda') appModalAjuda;
	@ViewChild('appModalAjudaBackground') appModalAjudaBackground;

	public abrir() {
		this.appModalAjuda.nativeElement.style.display = 'block';
		this.appModalAjudaBackground.nativeElement.style.display = 'block';

	}

	public fechar() {
		this.appModalAjuda.nativeElement.style.display = 'none';
		this.appModalAjudaBackground.nativeElement.style.display = 'none';
	}

}
