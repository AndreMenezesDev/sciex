import { Directive, HostBinding, HostListener, ElementRef, Input } from '@angular/core';

@Directive({
	selector: '[inputTextFilter]'
})


export class InputTextFilterDirective {
	regexStr = '[()&-_,!ãÃâÂáÁàÀêÊéÉèÈîÎíÍìÌõÕôÔóÓòÒûÛúÚùÙÇça-zA-Z0-9 ]';
	constructor(private el: ElementRef) {
		(el.nativeElement as HTMLInputElement).value = '';
	}

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

		return;
	}

	replaceText() {

		if (this.el.nativeElement.value.length == 1 && this.el.nativeElement.value == " ")
			this.trimText();

		this.el.nativeElement.value = this.el.nativeElement.value.replace('#', '');
		this.el.nativeElement.value = this.el.nativeElement.value.replace('$', '');
		this.el.nativeElement.value = this.el.nativeElement.value.replace('%', '');
		this.el.nativeElement.value = this.el.nativeElement.value.replace('¨', '');
		this.el.nativeElement.value = this.el.nativeElement.value.replace('*', '');

	}

	trimText() {
		(this.el.nativeElement as HTMLInputElement).value = (this.el.nativeElement as HTMLInputElement).value.trim();
	}

	@HostListener('window:keydown', ['$event'])
	onKeyPress($event: KeyboardEvent) {
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
}
