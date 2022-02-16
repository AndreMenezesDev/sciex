import { Directive, ElementRef, HostListener, Input, SimpleChanges, Renderer, Inject } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Directive({
	selector: '[ngModel][cpfCnpj]',
	host: {
		'(input)': 'input($event.target.value, $event.target)',
		'(blur)': 'onTouched($event)',
	},
})
export class CpfCnpjDirective implements ControlValueAccessor {
	@Input() ngModel: string;

	numeros = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
	regexStr = '^[0-9]*$';
	constructor(@Inject(Renderer) private renderer: Renderer, @Inject(ElementRef) private elementRef: ElementRef) { }

	propagateChange = (_: any) => { };

	registerOnChange(fn) {
		this.propagateChange = fn;
	}

	public onTouched: any = () => { };

	registerOnTouched(fn: any) {
		this.onTouched = fn;
	}

	@HostListener('keyup', ['$event']) onKeyUp(event) {
		const e = <KeyboardEvent>event;

		if ([46, 8, 9, 27, 13, 110, 190].indexOf(e.keyCode) != -1 ||
			// Allow: Ctrl+A
			(e.keyCode == 65 && e.ctrlKey == true) ||
			// Allow: Ctrl+C
			(e.keyCode == 67 && e.ctrlKey == true) ||
			// Allow: Ctrl+V
			(e.keyCode == 86 && e.ctrlKey == true) ||
			// Allow: Ctrl+X
			(e.keyCode == 88 && e.ctrlKey == true) ||
			// Allow: home, end, left, right
			(e.keyCode >= 35 && e.keyCode <= 39)) {
			// let it happen, don't do anything
			this.marcararCPF("");
			return;
		}

		const regEx = new RegExp(this.regexStr);

		if (!regEx.test(e.key)) {
			e.preventDefault();
		}
		else {
			this.elementRef.nativeElement.value = this.marcararCPF("");
			return;
		}
	}

	marcararCPF(documentoInformado) {
		var cpf = "";
		if (documentoInformado != "") {
			cpf = documentoInformado;
		} else {
			cpf = this.elementRef.nativeElement.value;
		}
		cpf = cpf.replace(/\D/g, "");
		cpf = cpf.substr(0, 11);
		cpf = cpf.replace(/(\d{3})(\d)/, "$1.$2");
		cpf = cpf.replace(/(\d{3})(\d)/, "$1.$2");
		cpf = cpf.replace(/(\d{3})(\d{1,2})$/, "$1-$2");

		return cpf;
	}

	input(val, event?) {
		const mascared = this.marcararCPF(val);

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
		value = this.marcararCPF(value);
		this.input(value);
		this.elementRef.nativeElement.value = this.elementRef.nativeElement.value.replace('undefined', '').trim() == "" ? "0,0000000" : this.elementRef.nativeElement.value.replace('undefined', '').trim();
	}
}
