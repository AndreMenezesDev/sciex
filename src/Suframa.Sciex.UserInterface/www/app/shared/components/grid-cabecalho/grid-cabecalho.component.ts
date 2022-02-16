import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
	selector: 'app-grid-cabecalho',
	templateUrl: './grid-cabecalho.component.html',
})
export class GridCabecalhoComponent {
	@Input() size: number;

	@Output() onChangeSize: EventEmitter<number> = new EventEmitter();

	constructor() { }

	changeSize($event) {
		this.onChangeSize.emit($event);
	}


}
