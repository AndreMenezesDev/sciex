import { Directive, ElementRef, HostListener, OnChanges, Input, SimpleChanges } from '@angular/core';

@Directive({
	selector: '[ngModel][date]',
})
export class DateDirective implements OnChanges {
	@Input() ngModel: string;

	constructor(private elementRef: ElementRef) { }

	ngOnChanges(changes: SimpleChanges) {
		const value = changes.ngModel.currentValue;
		if (!value) { return; }
		setTimeout(() => this.elementRef.nativeElement.value = value.substr(0, 10), 0);
	}

	@HostListener('blur') validate() {
		if (isNaN(Date.parse(this.elementRef.nativeElement.value))) {
			setTimeout(() => this.elementRef.nativeElement.value = undefined, 0);
		}
	}
}
