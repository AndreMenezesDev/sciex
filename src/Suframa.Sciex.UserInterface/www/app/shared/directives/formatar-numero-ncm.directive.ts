import { Directive, ElementRef, HostListener, OnChanges, Input, SimpleChanges } from '@angular/core';

@Directive({
	selector: '[ngModel][formatarNumeroNcm]',
})
export class FormatarNumeroNCMDirective implements OnChanges {
	@Input() ngModel: string;
	value: string;

	constructor(private elementRef: ElementRef) { }

	ngOnChanges(changes: SimpleChanges) {
		if (changes && changes.ngModel && changes.ngModel.currentValue &&
			!changes.ngModel.previousValue && changes.ngModel.currentValue.length > 10) {
			this.format(changes.ngModel.currentValue);
		}
	}

	@HostListener('blur') format(value) {

		console.log(value);
		if (!value) { value = this.elementRef.nativeElement.value; }

		value = value.toString().replace(/\W/g, '');

		if (value.toString().length == 8) {
			value = value.toString().replace(/(\d{4})(\d{2})(\d{2})/g, '\$1.\$2.\$3');
		} else {
			value = '';
		}


		setTimeout(() => this.elementRef.nativeElement.value = value, 0);
	}

	
}
