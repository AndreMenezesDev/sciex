import { Directive, ElementRef, EventEmitter, Output, HostListener } from '@angular/core';

@Directive({
	selector: '[porcentagem]',
})
export class PorcentagemDirective {
	constructor(private elementRef: ElementRef) {
		if (!elementRef.nativeElement.classList.contains(elementRef.nativeElement.name)) {
			elementRef.nativeElement.classList.add(elementRef.nativeElement.name);
		}

		this.onInit();
	}

	cleave: any;

	@Output() ngModelChange = new EventEmitter();

	@HostListener('input') onInit() {
		setTimeout(x => {
			// this.cleave = new Cleave('.input-porcentagem', {
			//    delimiter: '',
			//    numeral: true,
			//    numeralDecimalMark: ',',
			//    numeralDecimalScale: 7,
			//    numeralIntegerScale: 3,
			// });
		}, 0);
	}

	onInput($event) {
		this.cleave.onChange();
		this.ngModelChange.emit($event.target.value);
	}
}
