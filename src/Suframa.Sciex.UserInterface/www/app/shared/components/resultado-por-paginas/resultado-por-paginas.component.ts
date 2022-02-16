import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
	selector: 'app-resultado-por-paginas',
	templateUrl: './resultado-por-paginas.component.html',
})
export class ResultadoPorPaginasComponent implements OnInit {
	@Input() size: number;

	@Output() onChangeSize: EventEmitter<number> = new EventEmitter();

	constructor() { }

	ngOnInit() {
	
		this.size = this.size || 10;
	}

	changeSize(target): void {
		this.size = target.value;
		this.onChangeSize.emit(target.value);
	}
}
