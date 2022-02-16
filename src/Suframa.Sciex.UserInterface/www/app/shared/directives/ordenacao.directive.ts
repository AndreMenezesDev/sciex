import { Directive, ElementRef, Renderer } from '@angular/core';

@Directive({
	selector: '[appOrdenacao]'
})
export class OrdenacaoDirective {
	constructor(el: ElementRef, renderer: Renderer) {
		// Use renderer to render the element with styles
		renderer.setElementStyle(el.nativeElement, 'color', 'blue');
	}
}
