import { Directive, ElementRef, HostListener, Input, Renderer, Inject } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Directive({
	selector: '[mascararNCM]',
	host: {
		'(input)': 'input($event.target.value, $event.target)',
		'(blur)': 'onTouched($event)',
	},
})
export class MascaraNcmDirective implements ControlValueAccessor {
	@Input() ngModel: string;

	numeros = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
	regexStr = '[0-9,{1}?0-9]+';
	constructor(@Inject(Renderer) private renderer: Renderer, @Inject(ElementRef) private elementRef: ElementRef) { }	

	propagateChange = (_: any) => { };

	registerOnChange(fn) {
		this.propagateChange = fn;
	}

	public onTouched: any = () => { };

	registerOnTouched(fn: any) {
		this.onTouched = fn;
	}
	
	mascararNCM(documentoInformado) {
		var ncm = "";
		if (documentoInformado != "") {
			ncm = documentoInformado;
		} else {
			ncm = this.elementRef.nativeElement.value;
		}
		ncm = ncm.replace(/\D/g, "");
		ncm = ncm.substr(0, 8);
		ncm = ncm.replace(/(\d{4})(\d)/, "$1.$2");
		ncm = ncm.replace(/(\d{4}[.]\d{2})(\d)/, "$1.$2");

		return ncm;
	}

	input(val, event?) {
		const mascared = this.mascararNCM(val);

		this.propagateChange(mascared);
		this.renderer.setElementProperty(this.elementRef.nativeElement, 'value', mascared);

		this.elementRef.nativeElement.value = this.elementRef.nativeElement.value.replace('undefined', '').trim();
		setTimeout(function () {
			if (event !== undefined) {
				event.setSelectionRange(mascared.length, mascared.length);
			}
		}, 0);
	}

	writeValue(value: any) {
		value = this.mascararNCM(value);
		this.input(value);
		this.elementRef.nativeElement.value = this.elementRef.nativeElement.value.replace('undefined', '').trim() == "" ? "0,0000000" : this.elementRef.nativeElement.value.replace('undefined', '').trim();
	}

}
