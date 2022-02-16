import { Directive, Input, ElementRef, Inject, Provider, forwardRef, Renderer } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { ApplicationService } from '../services/application.service';
import { ModalService } from '../services/modal.service';

export const MONEY: Provider = {
	provide: NG_VALUE_ACCESSOR,
	useExisting: forwardRef(() => FormatarNumeroDecimal5Directive),
	multi: true
};

@Directive({
	selector: '[formatar-numero-decimal5]',
	host: {
		'(input)': 'input($event.target.value, $event.target)',
		'(blur)': 'input($event.target.value, $event.target)'
	},
	providers: [MONEY]
})

export class FormatarNumeroDecimal5Directive implements ControlValueAccessor {

	@Input('prefix') prefix: string;
	parametros: any = {};

	constructor(
		@Inject(Renderer) private renderer: Renderer,
		@Inject(ElementRef) private element: ElementRef,
		private modal: ModalService,
		private applicationService: ApplicationService) { }

	writeValue(value: any) {

		if (value === undefined || value === null) {
			this.propagateChange(null);
			this.renderer.setElementProperty(this.element.nativeElement, 'value', '');
			this.element.nativeElement.value = this.element.nativeElement.value.replace('undefined', '').trim();
		} else {
			value = value.toString().split('.');

			if (typeof value[1] == 'undefined') {
				if (value[0] == 0) {
					value = '0';
				} else {
					value = value[0] + '.00000';
				}
			} else if (value[1].length == 1) {
				value = value[0] + '.' + value[1] + '0';
			} else if (value[1].length == 2) {
				value = value[0] + '.' + value[1];
			} else if (value[1].length > 2) {
				value = value[0] + '.' + value[1].slice(0, 5);
			}

			this.input(value);
			this.element.nativeElement.value = this.element.nativeElement.value.replace('undefined', '').trim();
		}
	}

	propagateChange = (_: any) => { };

	registerOnChange(fn) {
		this.propagateChange = fn;
	}

	public onTouched: any = () => { };

	registerOnTouched(fn: any) {
		this.onTouched = fn;
	}

	input(val, event?) {
	
		if (val === '0') {
			this.propagateChange('0');
			this.renderer.setElementProperty(this.element.nativeElement, 'value', this.prefix + ' 0,00');
			this.element.nativeElement.value = this.element.nativeElement.value.replace('undefined', '').trim();
			return true;
		}


		let unmask = val.toString().replace(new RegExp(/[^\d]/, 'g'), '').replace(/^0+/, '');

		if (unmask.length == 0) {
			this.propagateChange(null);
			this.renderer.setElementProperty(this.element.nativeElement, 'value', '');
			this.element.nativeElement.value = this.element.nativeElement.value.replace('undefined', '').trim();
			return true;
		}

		if (unmask.length == 1) {
			unmask = '00000'+ unmask;
		}

		if (unmask.length == 2) {
			unmask =  '0000'+ unmask;
		}

		if (unmask.length == 3) {
			unmask = '000'+ unmask;
		}

		if (unmask.length == 4) {
			unmask =  '00'+ unmask;
		}

		if (unmask.length == 5) {
			unmask = '0'+ unmask;
		}

		const dec = unmask.slice(unmask.length - 5, unmask.length);
		const mil = unmask.slice(0, unmask.length - 5);

		let cont = 0;
		let ref = '';
		for (let i = mil.length; i > 0; i--) {
			if (cont === 2 && i > 1) {
				ref = '.' + mil.slice(i - 1, i) + ref;
				cont = 0;
			} else {
				ref = mil.slice(i - 1, i) + ref;
				cont++;
			}
		}

		const mascared = this.prefix + ' ' + ref + ',' + dec;

		this.propagateChange(mil + '.' + dec);
		this.renderer.setElementProperty(this.element.nativeElement, 'value', mascared);


		this.element.nativeElement.value = this.element.nativeElement.value.replace('undefined', '').trim();
		setTimeout(function () {
			if (event !== undefined) {
				event.setSelectionRange(mascared.length, mascared.length);
			}
		}, 0);
	}
}
