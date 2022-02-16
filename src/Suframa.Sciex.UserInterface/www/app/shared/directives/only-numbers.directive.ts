import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
	selector: '[onlyNumber]',
})
export class OnlyNumbersDirective {
	numeros = [ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
	regexStr = '^[0-9]*$';
	constructor(private el: ElementRef) { }

	@HostListener('keydown', ['$event']) onKeyDown(event) {
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

		this.replaceText();

		if (!regEx.test(e.key)) {
			e.preventDefault();
		}
		else {
			return;
		}
	}


	@HostListener('window:keydown', ['$event'])
	onKeyPress($event: KeyboardEvent) {
		this.replaceText();
		if (($event.ctrlKey || $event.metaKey) && $event.keyCode == 86) {
			this.replaceText();
		}

	}

	@HostListener('change')
	onChange() {
		this.trimText();
		this.replaceText();
	}

	@HostListener('blur')
	onBlur() {
		this.trimText();
		this.replaceText();
	}

	@HostListener('input')
	onInput() {
		this.replaceText();
	}

	@HostListener('window:keyup', ['$event'])
	onKeyUp(event: KeyboardEvent) {

		this.replaceText();
	}

	trimText() {
		(this.el.nativeElement as HTMLInputElement).value = (this.el.nativeElement as HTMLInputElement).value.trim();
		this.el.nativeElement.value = (this.el.nativeElement as HTMLInputElement).value;
	}

	replaceText() {

		if (this.el.nativeElement.value.length == 1 && this.el.nativeElement.value == " ")

		this.trimText();

		var split = this.el.nativeElement.value.split("");

		for (var i = 0; i < split.length; i++) {			
			var key = split[i];
			if (this.numeros.indexOf(+key) == -1) {

				this.el.nativeElement.value = this.el.nativeElement.value.replace(key, '');
				this.el.nativeElement.value = this.el.nativeElement.value.replace('¨', '');

			}
		}
			

	}
}
