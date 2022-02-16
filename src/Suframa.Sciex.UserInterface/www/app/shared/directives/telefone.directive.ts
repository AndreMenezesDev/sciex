import { Directive, ElementRef, HostListener, OnChanges, Input, SimpleChanges } from '@angular/core';

@Directive({
	selector: '[ngModel][telefone]',
})
export class TelefoneDirective implements OnChanges {
	constructor(private elementRef: ElementRef) { }

	@Input() ngModel: string;

	ngOnChanges(changes: SimpleChanges) {
		if (changes && changes.ngModel && changes.ngModel.currentValue &&
			!changes.ngModel.previousValue && changes.ngModel.currentValue.toString().length > 9) {
			this.format(changes.ngModel.currentValue);
		}
	}

	@HostListener('blur') format(value) {
		if (!value) { value = this.elementRef.nativeElement.value; }

		value = value.toString().replace(/\W/g, '');

		if (value.toString().length == 10) {
			value = value.toString().replace(/(\d{2})(\d{4})(\d{4})/g, '(\$1) \$2-\$3');
		} else if (value.toString().length == 11) {
			value = value.toString().replace(/(\d{2})(\d{5})(\d{4})/g, '(\$1) \$2-\$3');
		} else {
			value = '';
		}

		setTimeout(() => this.elementRef.nativeElement.value = value, 0);
	}
}
