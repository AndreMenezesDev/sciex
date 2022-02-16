import { Component, Output, Input, EventEmitter } from '@angular/core';

@Component({
	selector: 'app-ordenacao',
	templateUrl: './ordenacao.component.html',
})
export class OrdenacaoComponent {
	@Input() field: string;
	@Input() sorted: string;

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();

	reverse: boolean;

	changeSort(sortedBy) {
		this.sorted = sortedBy;
		if (this.field == sortedBy) {
			this.reverse = !this.reverse;
		} else {
			this.reverse = false;
		}

		this.onChangeSort.emit({ field: this.field, reverse: this.reverse });
	}
}
