import { Directive, Input, ElementRef, Inject, Provider, forwardRef, Renderer, HostListener } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

//export const MONEY: Provider = {
//	provide: NG_VALUE_ACCESSOR,
//	useExisting: forwardRef(() => FormatarNumeroPLIDirective),
//	multi: true
//};

@Directive({
	selector: '[formatarNumeroPli]',
	host: {
		'(input)': 'input($event.target.value, $event.target)',
		'(blur)': 'onTouched($event)',
	},
	//providers: [MONEY]
})

export class FormatarNumeroPLIDirective implements ControlValueAccessor {
	@Input('prefix') prefix: string;
	@Input() ngModel: string;
	value: string;
	constructor(@Inject(Renderer) private renderer: Renderer, @Inject(ElementRef) private element: ElementRef) { }

	propagateChange = (_: any) => { };

	registerOnChange(fn) {
		this.propagateChange = fn;
	}

	public onTouched: any = () => { };

	registerOnTouched(fn: any) {
		this.onTouched = fn;
	}

	@HostListener('blur') format(value) {

		if (value == undefined) {
			value = this.element.nativeElement.value;
			if (value == "")
				return;
		}
		else if (this.element.nativeElement.value == "") {
			value = this.element.nativeElement.value;
			return;
		}
		else {
			setTimeout(() => this.element.nativeElement.value = "", 0);
			return;
		}
		value = value.toString().replace(/\W/g, '');

		let inteiro = parseInt(value);

		var ano = inteiro.toString().substring(0, 4);
		var barra = "/";
		var numero = inteiro.toString().substring(4, 10);
		numero = ("000000" + numero).slice(-6);
		ano = ("0000" + ano).slice(-4);
		value = ano + barra + numero;

		setTimeout(() => this.element.nativeElement.value = value, 0);
	}

	mascaraPLI(documentoInformado) {
		var numeropli = "";
		if (documentoInformado != "") {
			numeropli = documentoInformado;
		} else {
			numeropli = this.element.nativeElement.value;
		}
		numeropli = numeropli.replace(/\D/g, "");
		numeropli = numeropli.substr(0, 10);
		numeropli = numeropli.replace(/(\d{4})(\d)/, "$1/$2");

		return numeropli;
	}

	writeValue(value: any) {
		value = this.mascaraPLI(value);
		this.input(value);
		this.element.nativeElement.value = this.element.nativeElement.value.replace('undefined', '').trim() == "" ? "0000/000000" : this.element.nativeElement.value.replace('undefined', '').trim();
	}

	input(val, event?) {
		const mascared = this.mascaraPLI(val);

		this.propagateChange(mascared);
		this.renderer.setElementProperty(this.element.nativeElement, 'value', mascared);

		this.element.nativeElement.value = this.element.nativeElement.value.replace('undefined', '').trim();
		setTimeout(function () {
			if (event !== undefined) {
				event.setSelectionRange(mascared.length, mascared.length);
			}
		}, 0);
	}

	//writeValue(value: any) {

	//	//if (value === undefined || value === null) {
	//	//	this.propagateChange(null);
	//	//	this.renderer.setElementProperty(this.element.nativeElement, 'value', '');
	//	//	this.element.nativeElement.value = this.element.nativeElement.value.replace('undefined', '').trim();
	//	//} else {
	//	//	value = value.toString().split('/');

	//	//	if (typeof value[1] == 'undefined') {
	//	//		if (value[0] == 0) {
	//	//			value = '0';
	//	//		} else {
	//	//			value = value[0] + '000/0000000';
	//	//		}
	//	//	} else if (value[1].length == 1) {
	//	//		value = value[0] + '000/' + value[1] + '0';
	//	//	} else if (value[1].length == 2) {
	//	//		value = value[0] + '/' + value[1];
	//	//	} else if (value[1].length > 2) {
	//	//		value = value[0] + '/' + value[1].slice(0, 6);
	//	//	}

	//	//	this.input(value);
	//	//	this.element.nativeElement.value = this.element.nativeElement.value.replace('undefined', '').trim() == "" ? "0,0000000" : this.element.nativeElement.value.replace('undefined', '').trim();

	//	//}
	//}

	//input(val, event?) {

	//	if (val === '0') {
	//		this.propagateChange('0');
	//		this.renderer.setElementProperty(this.element.nativeElement, 'value', this.prefix + ' /000000');
	//		this.element.nativeElement.value = this.element.nativeElement.value.replace('undefined', '').trim();
	//		return true;
	//	}
		
	//	let unmask = val.toString().replace(new RegExp(/[^\d]/, 'g'), '').replace(/^0+/, '');
		
	//	if (unmask.length == 0) {
	//		this.propagateChange(null);
	//		this.renderer.setElementProperty(this.element.nativeElement, 'value', '');
	//		this.element.nativeElement.value = this.element.nativeElement.value.replace('undefined', '').trim();
	//		return true;
	//	}

	//	if (unmask.length == 1) {
	//		unmask = '000000' + unmask;
	//	}

	//	if (unmask.length == 2) {
	//		unmask = '00000' + unmask;
	//	}

	//	if (unmask.length == 3) {
	//		unmask = '0000' + unmask;
	//	}

	//	if (unmask.length == 4) {
	//		unmask = '000' + unmask;
	//	}

	//	if (unmask.length == 5) {
	//		unmask = '00' + unmask;
	//	}

	//	if (unmask.length == 6) {
	//		unmask = '0' + unmask;
	//	}
		


	//	const dec = unmask.slice(unmask.length - 6, unmask.length);
	//	const mil = unmask.slice(0, unmask.length - 6);

	//	let cont = 0;
	//	let ref = '';
	//	for (let i = mil.length; i > 0; i--) {
	//		if (cont === 3 && i > 1) {
	//			ref = '/' + mil.slice(i - 1, i) + ref;
	//			cont = 0;
	//		} else {
	//			ref = mil.slice(i - 1, i) + ref;
	//			cont++;
	//		}
	//	}

	//	const mascared = this.prefix + ' ' + ref + '/' + dec;

	//	this.propagateChange(mil + '/' + dec);
	//	this.renderer.setElementProperty(this.element.nativeElement, 'value', mascared);

	//	this.element.nativeElement.value = this.element.nativeElement.value.replace('undefined', '').trim();
	//	setTimeout(function () {
	//		if (event !== undefined) {
	//			event.setSelectionRange(mascared.length, mascared.length);
	//		}
	//	}, 0);
	//}




}

