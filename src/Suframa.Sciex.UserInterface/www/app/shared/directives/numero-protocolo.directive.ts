import { Directive, ElementRef, HostListener, OnChanges, Input, SimpleChanges } from '@angular/core';

@Directive({
	selector: '[ngModel][numeroProtocolo]',
})
export class NumeroProtocoloDirective implements OnChanges {
	constructor(private elementRef: ElementRef) { }

	@Input() ngModel: string;

	ngOnChanges(changes: SimpleChanges) {
		if (changes && changes.ngModel && changes.ngModel.currentValue &&
			!changes.ngModel.previousValue && changes.ngModel.currentValue.length > 10) {
			this.format(changes.ngModel.currentValue);
		}
	}

	@HostListener('blur') format(value) {
		if (!value) { value = this.elementRef.nativeElement.value; }

		// Remove caracteres deixando apenas os números
		value = value.toString().replace(/[^0-9]/g, '');

		if (value.toString().length >= 5) {
			value = value.toString().split('').reverse().join('');
			value = [value.slice(0, 4), '/', value.slice(4)].join('');
			value = value.toString().split('').reverse().join('');
		} else {
			value = '';
		}

		setTimeout(() => this.elementRef.nativeElement.value = value, 0);
	}
}
