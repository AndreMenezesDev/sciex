import { Directive, forwardRef, ElementRef, Input, SimpleChanges, HostListener } from '@angular/core';
import { NG_VALIDATORS } from '@angular/forms';
import { Validator, AbstractControl } from '@angular/forms';

@Directive({
	selector: '[validateEmail][formControlName], [validateEmail][formControl],[validateEmail][ngModel]',
})
export class EmailDirective {
	constructor(private elementRef: ElementRef) { }

	@HostListener('blur') validate() {
		const EMAIL_REGEXP = /^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$/i;

		const value = this.elementRef.nativeElement.value;

		if (!value) { return; }

		if (!EMAIL_REGEXP.test(value)) {
			this.elementRef.nativeElement.value = '';
		}
	}
}
