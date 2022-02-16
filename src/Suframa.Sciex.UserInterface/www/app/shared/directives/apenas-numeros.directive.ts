import { Directive, ElementRef, HostListener, OnChanges, Input, SimpleChanges, Renderer, Inject } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Directive({
	selector: '[ngModel][apenasNumeros]',
	host: {
		'(input)': 'input($event.target.value, $event.target)',
		'(blur)': 'onTouched($event)',
	},
})
export class ApenasNumerosDirective implements ControlValueAccessor {
	@Input() ngModel: string;

	numeros = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
	regexStr = '[0-9,{1}?0-9]+';
	constructor(@Inject(Renderer) private renderer: Renderer, @Inject(ElementRef) private elementRef: ElementRef) { }

	ngOnChanges(changes: SimpleChanges) {		
	}

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
			return;
		}

		const regEx = new RegExp(this.regexStr);

		if (!regEx.test(e.key)) {
			e.preventDefault();
		}
		else {
			this.elementRef.nativeElement.value = this.soNumeros("");
			return;
		}
	}

	soNumeros(documentoInformado) {
		var numeros = "";
		if (documentoInformado != "") {
			numeros = documentoInformado;
		} else {
			numeros = this.elementRef.nativeElement.value;
		}
		numeros = numeros.replace(/[A-Za-z\.\@\!\#\$\%\&\*\(\)\+\=\{\}\:\;\[\]\^\~\"\'\-\=\+\º\ª\°\`\´\?\/\\]+/g, "");
		numeros = this.removeCaractere(numeros, ",");
		return numeros;
	}

	input(val, event?) {
		const mascared = this.soNumeros(val);

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
		value = this.soNumeros(value);
		this.input(value);
		this.elementRef.nativeElement.value = this.elementRef.nativeElement.value.replace('undefined', '').trim() == "" ? "0,0000000" : this.elementRef.nativeElement.value.replace('undefined', '').trim();
	}

	removeCaractere(cadeia, caracter) {
		var novaString = "";
		var possuiCaracter = false;

		for (var i = 0; i < cadeia.length; i++) {
			if (i == 0 && cadeia[i] == caracter) {
				novaString = "";
			}
			else if (cadeia[i] == caracter && !possuiCaracter) {
				possuiCaracter = true;
				novaString = novaString + cadeia[i];
			} else if (cadeia[i] != caracter) {
				novaString = novaString + cadeia[i];
			}
		}
		return novaString;
	}
}
