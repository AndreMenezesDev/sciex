import { Directive, Input, ElementRef, Inject, Provider, forwardRef, Renderer } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

export const MONEY: Provider = {
	provide: NG_VALUE_ACCESSOR,
	useExisting: forwardRef(() => FormatarNumeroDirective),
	multi: true
};

@Directive({
	selector: '[formatar-numero]',
	host: {
		'(input)': 'input($event.target.value, $event.target)',
		'(blur)': 'onTouched($event)',
	},
	providers: [MONEY]
})

export class FormatarNumeroDirective implements ControlValueAccessor {
	@Input('prefix') prefix: string;

	constructor( @Inject(Renderer) private renderer: Renderer, @Inject(ElementRef) private element: ElementRef) { }

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
					value = value[0] + '.00';
				}
			} else if (value[1].length == 1) {
				value = value[0] + '.' + value[1] + '0';
			} else if (value[1].length == 2) {
				value = value[0] + '.' + value[1];
			} else if (value[1].length > 2) {
				value = value[0] + '.' + value[1].slice(0, 2);
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
			unmask = '00' + unmask;
		}

		if (unmask.length == 2) {
			unmask = '0' + unmask;
		}

		const dec = unmask.slice(unmask.length - 2, unmask.length);
		const mil = unmask.slice(0, unmask.length - 2);

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
